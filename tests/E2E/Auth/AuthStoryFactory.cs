using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;
using authapi.Api;
using authapi.Client;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Auth;

internal interface IAuthInvoker
{
    Task<ExecutionResult> InvokeAsync(
        string operation,
        CallStyle callStyle,
        TestContext context,
        CancellationToken cancellationToken);

    IReadOnlyDictionary<string, object?> DescribeParameters(
        string operation,
        CallStyle callStyle,
        TestContext context);

    Task ValidateResponseAsync(
        string operation,
        CallStyle callStyle,
        ExecutionResult result,
        TestContext context);

    Task ValidateRawResponseAsync(
        string operation,
        CallStyle callStyle,
        ExecutionResult result,
        TestContext context);

    Task CleanupAsync(TestContext context, CancellationToken cancellationToken);
}

/// <summary>
/// Reflection adapter for the generated Auth clients.  The generated API is split
/// across thirteen classes, while every operation has four public call styles.  The
/// adapter keeps the stories readable and makes a change in the generated method
/// signatures visible through the coverage test below.
/// </summary>
internal sealed class AuthApiClient : IAuthInvoker
{
    private readonly Dictionary<(string Operation, CallStyle Style), (object Client, MethodInfo Method)> _methods = new();
    private readonly CognitoTokenProvider? _cognito;
    private readonly Config? _executionConfig;

    public AuthApiClient(
        authapi.Client.Configuration configuration,
        CognitoTokenProvider? cognito = null,
        Config? executionConfig = null)
    {
        _cognito = cognito;
        _executionConfig = executionConfig;
        foreach (var apiType in AuthStoryFactory.GeneratedApiTypes)
        {
            var client = Activator.CreateInstance(apiType, configuration)
                ?? throw new InvalidOperationException($"Could not create {apiType.Name}.");
            foreach (var operation in AuthStoryFactory.BaseMethods)
            {
                if (!apiType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Any(method => method.Name == operation))
                    continue;

                foreach (var callStyle in Enum.GetValues<CallStyle>())
                {
                    var methodName = AuthStoryFactory.GeneratedMethodName(operation, callStyle);
                    var method = apiType.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance)
                        ?? throw new InvalidOperationException(
                            $"Generated Auth method {apiType.Name}.{methodName} was not found.");
                    _methods[(operation, callStyle)] = (client, method);
                }
            }
        }

        var missing = AuthStoryFactory.Methods
            .Where(method => !_methods.ContainsKey((AuthStoryFactory.BaseOperation(method.Method), method.CallStyle)))
            .Select(method => method.Method)
            .ToArray();
        if (missing.Length > 0)
            throw new InvalidOperationException(
                $"Generated Auth methods are missing from the adapter: {string.Join(", ", missing)}");
    }

    public async Task<ExecutionResult> InvokeAsync(
        string operation,
        CallStyle callStyle,
        TestContext context,
        CancellationToken cancellationToken)
    {
        if (!_methods.TryGetValue((operation, callStyle), out var entry))
            throw new InvalidOperationException($"Auth operation is not registered: {operation}/{callStyle}");

        await PrepareCognitoRequestAsync(operation, context, cancellationToken).ConfigureAwait(false);
        var arguments = AuthFixture.BuildArguments(entry.Method, operation, context, cancellationToken);
        object? rawResponse;
        try
        {
            rawResponse = await InvokeWithStripeRecoveryAsync(entry, arguments, cancellationToken)
                .ConfigureAwait(false);
        }
        catch (Exception error)
        {
            // The generated API exceptions carry the status code, error content and headers.
            // Rethrowing would leave the snapshot and the report without the failing response.
            // A create can also commit remotely and still fail here - a response timeout or a reset
            // connection - so the resource is recorded before the failure is reported.
            AuthFixture.RecordCreatedResource(operation, null, context);
            return MethodExecutor.FromException(error);
        }

        // Creates are recorded before the engine validates the response: a malformed or missing
        // payload aborts the step before UpdateState runs, and cleanup would then skip a resource
        // the API has already created.
        AuthFixture.RecordCreatedResource(operation, rawResponse, context);

        if (_cognito is not null && _cognito.CanProvisionUsers && operation == "CreateSaasUser")
        {
            var email = context.GetRequired<string>("user_email");
            var password = context.GetRequired<string>("password");
            var tokens = await _cognito.EnsureUserTokensAsync(email, password, cancellationToken)
                .ConfigureAwait(false);
            AuthFixture.SetCognitoTokens(context, tokens);
            context.Variables["cognito_user_ready"] = true;
        }

        if (callStyle is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync)
        {
            if (rawResponse is not IApiResponse response)
                throw new InvalidOperationException(
                    $"{entry.Method.Name} did not return an ApiResponse.");
            return ExecutionResult.WithHttp(
                response.Content,
                (int)response.StatusCode,
                ToHeaders(response.Headers)) with { Body = response.RawContent };
        }

        return ExecutionResult.Success(rawResponse);
    }

    /// <summary>
    /// Invokes the generated method and retries once when the API reports that no Stripe key is
    /// registered: the shared environment loses the registration whenever the Billing story runs.
    /// This mirrors the Go suite's <c>shouldRetryAfterStripeKeyError</c> fallback.
    /// </summary>
    private async Task<object?> InvokeWithStripeRecoveryAsync(
        (object Client, MethodInfo Method) entry,
        object?[] arguments,
        CancellationToken cancellationToken)
    {
        try
        {
            return await InvokeGeneratedAsync(entry, arguments, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception error) when (_executionConfig is not null &&
                                      AuthStripeIntegration.IsStripeKeyMissing(error))
        {
            var prepared = await AuthStripeIntegration
                .TryPrepareAsync(_executionConfig, cancellationToken).ConfigureAwait(false);
            if (!prepared.Enabled) throw;
            return await InvokeGeneratedAsync(entry, arguments, cancellationToken).ConfigureAwait(false);
        }
    }

    /// <summary>
    /// Invokes the generated method. Blocking call styles are dispatched to the thread pool, the
    /// same way <see cref="MethodExecutor"/> does, so the engine's per-step timeout still applies
    /// to a hung endpoint instead of blocking the test runner.
    /// </summary>
    private static async Task<object?> InvokeGeneratedAsync(
        (object Client, MethodInfo Method) entry,
        object?[] arguments,
        CancellationToken cancellationToken)
    {
        if (typeof(Task).IsAssignableFrom(entry.Method.ReturnType))
        {
            var task = (Task)Unwrap(entry, arguments)!;
            await task.ConfigureAwait(false);
            return entry.Method.ReturnType.IsGenericType &&
                   entry.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>)
                ? task.GetType().GetProperty("Result")?.GetValue(task)
                : null;
        }

        return await Task.Run(() => Unwrap(entry, arguments), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>Invokes through reflection, surfacing the API exception instead of the wrapper.</summary>
    private static object? Unwrap((object Client, MethodInfo Method) entry, object?[] arguments)
    {
        try
        {
            return entry.Method.Invoke(entry.Client, arguments);
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            throw error.InnerException;
        }
    }

    public Task ValidateResponseAsync(
        string operation,
        CallStyle callStyle,
        ExecutionResult result,
        TestContext context)
    {
        if (!_methods.TryGetValue((operation, callStyle), out var entry))
            throw new InvalidOperationException($"Auth operation is not registered: {operation}/{callStyle}");
        AuthResponseValidator.Validate(operation, ResponseType(entry.Method), result, context);
        return Task.CompletedTask;
    }

    public Task ValidateRawResponseAsync(
        string operation,
        CallStyle callStyle,
        ExecutionResult result,
        TestContext context)
    {
        AuthResponseValidator.ValidateRaw(operation, callStyle, result);
        return Task.CompletedTask;
    }

    public IReadOnlyDictionary<string, object?> DescribeParameters(
        string operation,
        CallStyle callStyle,
        TestContext context)
    {
        if (!_methods.TryGetValue((operation, callStyle), out var entry))
            return new Dictionary<string, object?>();

        var arguments = AuthFixture.BuildArguments(entry.Method, operation, context, CancellationToken.None);
        var parameters = new Dictionary<string, object?>(StringComparer.Ordinal);
        var definitions = entry.Method.GetParameters();
        for (var index = 0; index < definitions.Length; index++)
        {
            var parameter = definitions[index];
            if (parameter.Name is "operationIndex" or "cancellationToken") continue;
            parameters[parameter.Name ?? $"arg{index}"] = arguments[index];
        }

        return parameters;
    }

    public async Task CleanupAsync(TestContext context, CancellationToken cancellationToken)
    {
        // Cleanup is deliberately performed through the HTTP-info async methods so
        // that a partially completed story is cleaned up regardless of its story style.
        // Every deletion is attempted even if an earlier one fails, otherwise a single failure
        // would leak all remaining resources into the shared environment.
        var failures = new List<Exception>();

        // Self-registration is shared SaaS state: if the SignUp opt-in switched it on, the captured
        // original settings are written back before the resources are removed.
        if (IsTrue(context, "sign_in_settings_modified") &&
            context.Variables.TryGetValue("original_sign_in_settings", out var originalSignInSettings) &&
            originalSignInSettings is not null)
        {
            try
            {
                AuthFixture.DisableSelfRegistration(context);
                var restored = await InvokeAsync("UpdateSignInSettings", CallStyle.WithHttpInfoAsync, context, cancellationToken)
                    .ConfigureAwait(false);
                if (restored.Error is not null)
                    throw new InvalidOperationException(
                        $"Could not restore the original sign-in settings: {restored.Error.Message}", restored.Error);
                // Cleared only once the shared setting is really restored, so a retry still knows
                // that self-registration is still switched on.
                context.Variables["sign_in_settings_modified"] = false;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested) { throw; }
            catch (Exception error) { failures.Add(error); }
        }

        foreach (var (operation, valueKey, createdKey) in new[]
                 {
                     ("DeleteStripeTenantAndPricing", "tenant_id", "stripe_pricing_created"),
                     // Attempted before the tenant user is removed: a failed DeleteTenantUser would
                     // otherwise leave the assignment attached in the shared tenant.
                     ("DeleteTenantUserRole", "role_name", "tenant_user_role_created"),
                     ("DeleteTenantUser", "tenant_user_id", "tenant_user_created"),
                     ("DeleteTenantInvitation", "invitation_id", "invitation_created"),
                     ("DeleteTenant", "tenant_id", "tenant_created"),
                     ("DeleteSaasUser", "user_id", "user_created"),
                     ("DeleteEnv", "env_id", "env_created"),
                     ("DeleteTenantAttribute", "tenant_attribute_name", "tenant_attribute_created"),
                     ("DeleteUserAttribute", "attribute_name", "user_attribute_created"),
                     // The SaaS-level definition is created under its own name and is removed by the
                     // same endpoint; an environment that does not expose it answers 404, which
                     // DeleteIfCreatedAsync treats as already cleaned up.
                     ("DeleteUserAttribute", "saas_user_attribute_name", "saas_user_attribute_created"),
                     ("DeleteRole", "role_name", "role_created"),
                     ("DeleteSaasUser", "signup_user_id", "signup_user_created")
                 })
        {
            try
            {
                await DeleteIfCreatedAsync(operation, valueKey, createdKey).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception error)
            {
                failures.Add(error);
            }
        }

        // The Cognito accounts provisioned for the story are removed after the SaaS resources, so a
        // user the platform deletes on its own is already gone and reported as UserNotFound.
        if (_cognito is not null && _cognito.CanProvisionUsers)
        {
            try
            {
                await _cognito.DeleteProvisionedUsersAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception error)
            {
                failures.Add(error);
            }
        }

        // A create that committed without a usable identifier cannot be deleted. It is reported so
        // the run fails visibly instead of leaving an unnoticed resource behind.
        var unresolved = new[]
            {
                "user_created", "signup_user_created", "tenant_created",
                "tenant_user_created", "invitation_created"
            }
            .Where(createdKey => IsTrue(context, AuthFixture.UnresolvedKey(createdKey)))
            .ToArray();
        if (unresolved.Length > 0)
            failures.Add(new InvalidOperationException(
                "Auth cleanup could not identify resources the API may have created: " +
                $"{string.Join(", ", unresolved)}. Remove them manually."));

        if (failures.Count == 1) throw failures[0];
        if (failures.Count > 1) throw new AggregateException("Auth cleanup did not complete.", failures);

        async Task DeleteIfCreatedAsync(string operation, string valueKey, string createdKey)
        {
            if (!IsTrue(context, createdKey) || !context.Variables.TryGetValue(valueKey, out var value) || value is null)
                return;

            // A cleanup call whose parameter differs from the story's own fixture value publishes an
            // override variable for the duration of that call only.
            var overrideKey = CleanupOverrideKey(operation, valueKey);
            var hasOverride = context.Variables.TryGetValue(overrideKey, out var previousOverride);
            if (overrideKey.Length > 0)
                context.Variables[overrideKey] = value;
            try
            {
                var result = await InvokeAsync(operation, CallStyle.WithHttpInfoAsync, context, cancellationToken)
                    .ConfigureAwait(false);
                // InvokeAsync reports a failed call as a result rather than an exception, so the
                // result must be inspected or a leaked resource would pass cleanup silently.
                // HTTP 404 means the resource is already gone, which is a successful cleanup.
                if (result.StatusCode is { } status && status != 404 && status is not (>= 200 and < 300))
                    throw new InvalidOperationException(
                        $"Cleanup of {operation} returned HTTP {status}.", result.Error);
                if (result.Error is not null && result.StatusCode != 404)
                    throw new InvalidOperationException(
                        $"Cleanup of {operation} failed: {result.Error.Message}", result.Error);
                context.Variables[createdKey] = false;
            }
            finally
            {
                if (overrideKey.Length > 0)
                {
                    if (hasOverride) context.Variables[overrideKey] = previousOverride;
                    else context.Variables.Remove(overrideKey);
                }
            }
        }
    }

    /// <summary>
    /// The variable a cleanup call reads its parameter from when the story's own fixture value would
    /// address a different resource.
    /// </summary>
    private static string CleanupOverrideKey(string operation, string valueKey) => (operation, valueKey) switch
    {
        ("DeleteSaasUser", "signup_user_id") => "cleanup_user_id",
        ("DeleteUserAttribute", "saas_user_attribute_name") => "cleanup_attribute_name",
        _ => string.Empty
    };

    private async Task PrepareCognitoRequestAsync(
        string operation,
        TestContext context,
        CancellationToken cancellationToken)
    {
        if (_cognito is null || !_cognito.CanProvisionUsers ||
            operation is not ("CreateSecretCode" or "UpdateSoftwareToken"))
            return;

        if (context.Variables.TryGetValue("software_token_secret", out var existing) &&
            existing is string existingSecret && !string.IsNullOrWhiteSpace(existingSecret))
            return;

        var accessToken = context.Variables.TryGetValue("access_token", out var value) &&
                          value is string token
            ? token
            : string.Empty;
        var secret = await _cognito.AssociateSoftwareTokenAsync(accessToken, cancellationToken)
            .ConfigureAwait(false);
        context.Variables["software_token_secret"] = secret;
        context.Variables["mfa_code"] = CognitoTokenProvider.GenerateCurrentTotp(secret);
    }

    private static Type? ResponseType(MethodInfo method)
    {
        var type = method.ReturnType;
        if (type == typeof(Task)) return null;
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            type = type.GetGenericArguments()[0];
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ApiResponse<>))
            type = type.GetGenericArguments()[0];
        return type == typeof(void) ? null : type;
    }

    private static bool IsTrue(TestContext context, string key) =>
        context.Variables.TryGetValue(key, out var value) && value is true;

    private static IReadOnlyDictionary<string, IReadOnlyList<string>>? ToHeaders(
        Multimap<string, string>? headers)
    {
        if (headers is null || headers.Count == 0) return null;
        return headers.ToDictionary(
            pair => pair.Key,
            pair => (IReadOnlyList<string>)pair.Value.ToArray(),
            StringComparer.OrdinalIgnoreCase);
    }
}

internal static class AuthStoryFactory
{
    private static readonly int EnvironmentIdSeed =
        10_000 + Math.Abs(Environment.TickCount % 800_000);
    private static int _environmentId = EnvironmentIdSeed;

    // ErrorApi.ReturnInternalServerError is an intentional 500 test endpoint, not a
    // successful Auth operation, and is excluded just like the Go/PHP suites.
    internal static readonly IReadOnlyList<Type> GeneratedApiTypes = new[]
    {
        typeof(AuthInfoApi),
        typeof(BasicInfoApi),
        typeof(CredentialApi),
        typeof(EnvApi),
        typeof(InvitationApi),
        typeof(RoleApi),
        typeof(SaasUserApi),
        typeof(SingleTenantApi),
        typeof(TenantApi),
        typeof(TenantAttributeApi),
        typeof(TenantUserApi),
        typeof(UserAttributeApi),
        typeof(UserInfoApi)
    };

    private static readonly string[] PreferredOperationOrder =
    {
        "GetBasicInfo", "UpdateBasicInfo", "GetAuthInfo", "UpdateAuthInfo",
        "GetSaasUsers", "CreateSaasUser", "UpdateSaasUserPassword",
        "UpdateSaasUserEmail", "CreateSaasUserAttribute", "UpdateSaasUserAttributes", "GetSaasUser", "GetUserMfaPreference",
        "CreateRole", "GetRoles", "CreateUserAttribute", "GetUserAttributes",
        "CreateTenantAttribute", "GetTenantAttributes",
        "FindNotificationMessages", "UpdateNotificationMessages", "GetCustomizePages",
        "UpdateCustomizePages", "GetCustomizePageSettings", "UpdateCustomizePageSettings",
        "GetEnvs", "CreateEnv", "GetEnv", "UpdateEnv", "GetSignInSettings",
        "UpdateSignInSettings", "GetTenants", "CreateTenant", "GetTenant",
        "UpdateTenant", "CreateTenantUser", "GetAllTenantUsers", "GetAllTenantUser",
        "GetTenantUsers", "UpdateTenantUser", "GetTenantUser", "CreateTenantUserRoles",
        "DeleteTenantUserRole", "GetTenantInvitations", "CreateTenantInvitation",
        "GetTenantInvitation", "GetInvitationValidity", "ValidateInvitation",
        "DeleteTenantInvitation", "UpdateTenantPlan", "UpdateTenantBillingInfo",
        "GetIdentityProviders", "UpdateIdentityProvider", "GetTenantIdentityProviders",
        "UpdateTenantIdentityProvider",
        "GetCloudFormationLaunchStackLinkForSingleTenant", "GetSingleTenantSettings",
        "UpdateSingleTenantSettings", "GetUserInfo", "GetUserInfoByEmail",
        "CreateAuthCredentials", "GetAuthCredentials", "CreateSecretCode",
        "UpdateSoftwareToken", "UpdateUserMfaPreference", "RequestEmailUpdate",
        "ConfirmEmailUpdate", "RequestExternalUserLink", "ConfirmExternalUserLink",
        "UnlinkProvider", "SignUp", "ResendSignUpConfirmationEmail",
        "SignUpWithAwsMarketplace", "ConfirmSignUpWithAwsMarketplace", "LinkAwsMarketplace",
        "CreateTenantAndPricing", "GetStripeCustomer", "DeleteStripeTenantAndPricing",
        "ResetPlan", "DeleteTenantUser", "DeleteTenant", "DeleteSaasUser", "DeleteEnv",
        "DeleteTenantAttribute", "DeleteUserAttribute", "DeleteRole"
    };

    private static readonly IReadOnlyDictionary<string, string> LiveSkipReasons =
        new Dictionary<string, string>(StringComparer.Ordinal)
        {
            ["CreateAuthCredentials"] = "Requires valid Cognito ID/access/refresh tokens.",
            ["GetAuthCredentials"] = "Requires a temporary auth code or refresh token.",
            ["CreateSecretCode"] = "Requires a Cognito user access token.",
            ["UpdateSoftwareToken"] = "Requires Cognito MFA setup and a live TOTP code.",
            ["UpdateUserMfaPreference"] = "Requires Cognito MFA setup in the shared test environment.",
            ["RequestEmailUpdate"] = "Depends on email delivery and a verification-code backend.",
            ["ConfirmEmailUpdate"] = "Requires a verification code delivered by email.",
            ["RequestExternalUserLink"] = "Requires an external-provider access token.",
            ["ConfirmExternalUserLink"] = "Requires an external-provider linking flow.",
            ["UnlinkProvider"] = "Requires an existing linked social provider.",
            ["SignUp"] = "Disabled by default to protect the shared Cognito email quota.",
            ["ResendSignUpConfirmationEmail"] = "Disabled by default to protect the shared Cognito email quota.",
            ["SignUpWithAwsMarketplace"] = "Requires a configured AWS Marketplace listing.",
            ["ConfirmSignUpWithAwsMarketplace"] = "Requires a configured AWS Marketplace listing and token.",
            ["LinkAwsMarketplace"] = "Requires a valid AWS Marketplace registration token.",
            ["CreateTenantAndPricing"] = "Requires Stripe-backed pricing configuration.",
            ["GetStripeCustomer"] = "Requires Stripe-backed pricing configuration.",
            ["DeleteStripeTenantAndPricing"] = "Requires Stripe-backed pricing configuration.",
            ["UpdateTenantIdentityProvider"] = "Requires a tenant-level SAML provider configuration.",
            // The API never returns the existing OAuth secret, so an update could only send
            // placeholders and would overwrite the shared SaaS provider configuration.
            ["UpdateIdentityProvider"] = "Requires SaaS-level OAuth credentials the API does not return.",
            ["CreateTenantInvitation"] = "Requires an inviter who belongs to the target tenant.",
            ["GetTenantInvitation"] = "Requires an invitation created by a tenant member.",
            ["GetInvitationValidity"] = "Requires an invitation created by a tenant member.",
            ["ValidateInvitation"] = "Requires a real invitation token and recipient password.",
            ["DeleteTenantInvitation"] = "Requires an invitation created by a tenant member.",
            ["GetUserInfo"] = "Requires a valid SaaS user ID token."
        };

    internal static readonly IReadOnlyList<string> BaseMethods = DiscoverBaseMethods();

    internal static readonly IReadOnlyList<(string Method, CallStyle CallStyle)> Methods =
        BaseMethods
            .SelectMany(operation => Enum.GetValues<CallStyle>()
                .Select(callStyle => (GeneratedMethodName(operation, callStyle), callStyle)))
            .ToArray();

    internal static IReadOnlyCollection<string> LiveSkippedOperations => LiveSkipReasons.Keys.ToArray();

    internal static IReadOnlyList<Story> Create(
        IAuthInvoker client,
        bool executeEnvironmentDependentOperations = false)
    {
        return Create(client, AuthStoryOptions.Offline(executeEnvironmentDependentOperations));
    }

    internal static IReadOnlyList<Story> Create(
        IAuthInvoker client,
        AuthStoryOptions options)
    {
        return Enum.GetValues<CallStyle>()
            .Select(callStyle => CreateStory(client, callStyle, options))
            .ToArray();
    }

    internal static string GeneratedMethodName(string operation, CallStyle callStyle) => callStyle switch
    {
        CallStyle.Sync => operation,
        CallStyle.WithHttpInfo => $"{operation}WithHttpInfo",
        CallStyle.Async => $"{operation}Async",
        CallStyle.WithHttpInfoAsync => $"{operation}WithHttpInfoAsync",
        _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
    };

    internal static string BaseOperation(string generatedMethodName)
    {
        if (generatedMethodName.EndsWith("WithHttpInfoAsync", StringComparison.Ordinal))
            return generatedMethodName[..^"WithHttpInfoAsync".Length];
        if (generatedMethodName.EndsWith("WithHttpInfo", StringComparison.Ordinal))
            return generatedMethodName[..^"WithHttpInfo".Length];
        if (generatedMethodName.EndsWith("Async", StringComparison.Ordinal))
            return generatedMethodName[..^"Async".Length];
        return generatedMethodName;
    }

    internal static bool IsLiveSkipped(string operation) => LiveSkipReasons.ContainsKey(operation);

    internal static IReadOnlyCollection<string> SkippedOperations(AuthStoryOptions options) =>
        BaseMethods.Where(operation => ShouldSkip(operation, options)).ToArray();

    internal static string LiveSkipReason(string operation) =>
        LiveSkipReasons.TryGetValue(operation, out var reason) ? reason : string.Empty;

    private static bool ShouldSkip(string operation, AuthStoryOptions options)
    {
        if (options.IgnoreDependencyChecks)
            return false;

        if (!LiveSkipReasons.ContainsKey(operation)) return false;
        if (!options.ExecuteEnvironmentDependentOperations)
            return true;
        if (operation is "CreateTenantInvitation" or "GetTenantInvitation" or "GetInvitationValidity" or
            "ValidateInvitation" or "DeleteTenantInvitation" or "UpdateTenantIdentityProvider" or
            "UpdateIdentityProvider")
            return true;
        if (operation is "CreateTenantAndPricing" or "GetStripeCustomer" or "DeleteStripeTenantAndPricing")
            return !options.StripeEnabled;
        if (operation is "CreateAuthCredentials" or "GetAuthCredentials" or "GetUserInfo")
            return options.Cognito is null;
        if (operation is "CreateSecretCode" or "UpdateSoftwareToken" or "UpdateUserMfaPreference")
            return !options.CognitoUserFlowsEnabled;
        if (operation is "RequestEmailUpdate" or "ConfirmEmailUpdate")
            return !options.EmailUpdateEnabled;
        if (operation is "RequestExternalUserLink" or "ConfirmExternalUserLink" or "UnlinkProvider")
            return !options.ExternalProviderEnabled;
        if (operation is "SignUp" or "ResendSignUpConfirmationEmail")
            return !options.SignUpEnabled;
        return true;
    }

    private static string SkipReason(string operation, AuthStoryOptions options)
    {
        if (operation is "CreateTenantAndPricing" or "GetStripeCustomer" or "DeleteStripeTenantAndPricing")
            return options.StripeEnabled ? string.Empty : LiveSkipReason(operation);
        if (operation is "CreateAuthCredentials" or "GetAuthCredentials" or "GetUserInfo")
            return options.Cognito is null
                ? LiveSkipReason(operation)
                : "Cognito token provider did not become available.";
        if (operation is "CreateSecretCode" or "UpdateSoftwareToken" or "UpdateUserMfaPreference")
            return options.CognitoUserFlowsEnabled
                ? string.Empty
                : "Cognito user provisioning credentials are not configured.";
        if (operation is "RequestEmailUpdate" or "ConfirmEmailUpdate")
            return options.EmailUpdateEnabled
                ? string.Empty
                : "Set Cognito credentials and AUTH_E2E_EMAIL_CONFIRMATION_CODE to enable email-update tests.";
        if (operation is "RequestExternalUserLink" or "ConfirmExternalUserLink" or "UnlinkProvider")
            return options.ExternalProviderEnabled
                ? string.Empty
                : "Set Cognito credentials and external-provider test tokens to enable provider-link tests.";
        if (operation is "SignUp" or "ResendSignUpConfirmationEmail")
            return options.SignUpEnabled
                ? string.Empty
                : LiveSkipReason(operation);
        return LiveSkipReason(operation);
    }

    private static IReadOnlyList<string> DiscoverBaseMethods()
    {
        var generated = GeneratedApiTypes
            .SelectMany(apiType => apiType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(method => method.DeclaringType == apiType && !method.IsSpecialName)
                .Select(method => method.Name)
                .Where(IsBaseMethod))
            .Distinct(StringComparer.Ordinal)
            .ToHashSet(StringComparer.Ordinal);

        var ordered = PreferredOperationOrder.Where(generated.Remove).ToList();
        ordered.AddRange(generated.OrderBy(name => name, StringComparer.Ordinal));
        return ordered;
    }

    private static bool IsBaseMethod(string name) =>
        name != "ReturnInternalServerError" &&
        name != "GetBasePath" &&
        !name.EndsWith("WithHttpInfo", StringComparison.Ordinal) &&
        !name.EndsWith("Async", StringComparison.Ordinal);

    private static Story CreateStory(
        IAuthInvoker client,
        CallStyle callStyle,
        AuthStoryOptions options)
    {
        var variables = CreateVariables(options);
        var steps = BaseMethods.Select(operation => CreateStep(
            client,
            operation,
            callStyle,
            options)).ToArray();

        return new Story
        {
            Name = $"Auth API - {CallStyleDescription(callStyle)}",
            Description = "Exercises the Auth resource lifecycle and the configuration operations covered by the Go and PHP E2E suites.",
            Module = "auth",
            InitialVariables = variables,
            Steps = steps,
            SetupAsync = options.StripeEnabled
                ? async (_, cancellationToken) =>
                {
                    var configuration = options.ExecutionConfig
                        ?? throw new InvalidOperationException("Live Auth options are missing their execution configuration.");
                    var stripe = await AuthStripeIntegration.TryPrepareAsync(configuration, cancellationToken)
                        .ConfigureAwait(false);
                    if (!stripe.Enabled)
                        throw new InvalidOperationException(
                            $"Stripe setup failed before the Auth story: {stripe.Reason}");
                }
                : null,
            CleanupAsync = (context, cancellationToken) => client.CleanupAsync(context, cancellationToken)
        };
    }

    private static Step CreateStep(
        IAuthInvoker client,
        string operation,
        CallStyle callStyle,
        AuthStoryOptions options)
    {
        var generatedName = GeneratedMethodName(operation, callStyle);
        var isSkipped = ShouldSkip(operation, options);

        return new Step
        {
            Name = generatedName,
            Method = generatedName,
            CallStyle = callStyle,
            Skip = isSkipped,
            SkipReason = isSkipped ? SkipReason(operation, options) : string.Empty,
            Parameters = (Func<TestContext, object?>)(context =>
                client.DescribeParameters(operation, callStyle, context)),
            // Per-operation contract: creates accept 200 or 201, like the Go and PHP suites; every
            // other operation must answer exactly 200. Object call styles surface no status.
            ExpectedStatus = callStyle is not (CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync)
                ? null
                : ExpectedHttpStatus(operation),
            AllowedStatuses = callStyle is not (CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync)
                ? Array.Empty<int>()
                : AllowedHttpStatuses(operation),
            ExecuteAsync = (context, cancellationToken) =>
                client.InvokeAsync(operation, callStyle, context, cancellationToken),
            ValidateAsync = (result, context) =>
                client.ValidateResponseAsync(operation, callStyle, result, context),
            UpdateStateAsync = (result, context) =>
            {
                UpdateState(operation, result, context);
                return Task.CompletedTask;
            }
        };
    }

    /// <summary>
    /// The success contract of a generated Auth operation. Go uses <c>statusOKOrCreated</c> and PHP
    /// <c>allowedStatuses: [200, 201]</c> for the create operations, and the recorded baselines show
    /// the same endpoint answering 200 in one environment and 201 in another, so both are accepted.
    /// Everything else must answer exactly 200.
    /// </summary>
    internal static IReadOnlyList<int> AllowedHttpStatuses(string operation) =>
        IsCreateOperation(operation) ? new[] { 200, 201 } : Array.Empty<int>();

    internal static int? ExpectedHttpStatus(string operation) =>
        IsCreateOperation(operation) ? null : 200;

    internal static bool IsCreateOperation(string operation) =>
        operation.StartsWith("Create", StringComparison.Ordinal) ||
        operation.StartsWith("SignUp", StringComparison.Ordinal);

    private static void UpdateState(string operation, ExecutionResult result, TestContext context)
    {
        if (operation.StartsWith("Get", StringComparison.Ordinal) ||
            operation == "FindNotificationMessages")
        {
            var key = operation switch
            {
                "GetBasicInfo" => "original_basic_info",
                "GetAuthInfo" => "original_auth_info",
                "GetSignInSettings" => "original_sign_in_settings",
                "GetCustomizePages" => "original_customize_pages",
                "GetCustomizePageSettings" => "original_customize_page_settings",
                "FindNotificationMessages" => "original_notification_messages",
                "GetTenant" => "original_tenant",
                "GetTenantDetail" => "original_tenant",
                // Captured so the update steps can send the environment's own values back instead
                // of fixture placeholders, which would permanently reconfigure the shared SaaS.
                "GetSingleTenantSettings" => "original_single_tenant_settings",
                "GetIdentityProviders" => "original_identity_providers",
                _ => string.Empty
            };
            if (key.Length > 0 && result.Response is not null)
            {
                context.Variables[key] = result.Response;
                if (operation == "GetTenant")
                {
                    var billingInfo = result.Response.GetType().GetProperty("BillingInfo")?.GetValue(result.Response);
                    if (billingInfo is not null) context.Variables["original_billing_info"] = billingInfo;
                }
            }
        }

        // Also recorded here so the offline stories, which never pass through the invoker's own
        // pre-validation hook, keep tracking their created resources.
        AuthFixture.RecordCreatedResource(operation, result.Response, context);

        switch (operation)
        {
            case "UpdateSaasUserEmail":
                if (context.Variables.TryGetValue("updated_email", out var updatedEmail) && updatedEmail is string email)
                    context.Variables["expected_user_email"] = email;
                break;
            case "ConfirmEmailUpdate":
                if (context.Variables.TryGetValue("email_update_target", out var emailUpdateTarget) &&
                    emailUpdateTarget is string confirmedEmail)
                    context.Variables["expected_user_email"] = confirmedEmail;
                break;
            case "CreateAuthCredentials":
                SetStringProperty(context, "authorization_code", result.Response, "Code");
                break;
            case "CreateSecretCode":
                SetStringProperty(context, "software_token_secret", result.Response, "SecretCode");
                if (context.Variables.TryGetValue("software_token_secret", out var secret) &&
                    secret is string secretCode && !string.IsNullOrWhiteSpace(secretCode))
                {
                    try
                    {
                        context.Variables["mfa_code"] = CognitoTokenProvider.GenerateCurrentTotp(secretCode);
                    }
                    catch (FormatException)
                    {
                        // The API may return a non-TOTP test secret in a mock environment.
                    }
                }
                break;
            case "DeleteStripeTenantAndPricing":
                // The story removes the Stripe tenant/pricing link itself, so cleanup must not
                // repeat the call: the API answers the second delete with HTTP 400.
                context.Variables["stripe_pricing_created"] = false;
                break;
            case "DeleteTenantUser":
                context.Variables["tenant_user_created"] = false;
                break;
            case "DeleteTenantUserRole":
                context.Variables["tenant_user_role_created"] = false;
                break;
            case "DeleteTenant":
                context.Variables["tenant_created"] = false;
                break;
            case "DeleteSaasUser":
                context.Variables["user_created"] = false;
                break;
            case "DeleteEnv":
                context.Variables["env_created"] = false;
                break;
            case "DeleteTenantAttribute":
                context.Variables["tenant_attribute_created"] = false;
                break;
            case "DeleteUserAttribute":
                context.Variables["user_attribute_created"] = false;
                break;
            case "DeleteRole":
                context.Variables["role_created"] = false;
                break;
            case "DeleteTenantInvitation":
                context.Variables["invitation_created"] = false;
                break;
        }
    }

    private static void SetStringProperty(
        TestContext context,
        string key,
        object? response,
        string propertyName)
    {
        var value = response?.GetType().GetProperty(propertyName)?.GetValue(response)?.ToString();
        if (!string.IsNullOrWhiteSpace(value)) context.Variables[key] = value;
    }

    internal static IReadOnlyDictionary<string, object?> CreateVariables(AuthStoryOptions options)
    {
        var suffix = Guid.NewGuid().ToString("N");
        var envId = Math.Abs(Interlocked.Increment(ref _environmentId) % 800_000) + 10_000;
        var variables = new Dictionary<string, object?>
        {
            ["user_email"] = $"csharp-auth-e2e-{suffix}@example.com",
            ["updated_email"] = $"csharp-auth-e2e-updated-{suffix}@example.com",
            ["email_update_target"] = $"csharp-auth-e2e-verification-{suffix}@example.com",
            ["signup_email"] = $"csharp-auth-signup-{suffix}@example.com",
            ["staff_email"] = $"csharp-auth-e2e-staff-{suffix}@example.com",
            ["password"] = options.DefaultPassword,
            ["updated_password"] = options.UpdatedPassword,
            ["role_name"] = $"csharp-auth-e2e-role-{suffix}",
            ["attribute_name"] = $"csharp_auth_e2e_{suffix[..12]}",
            ["saas_user_attribute_name"] = $"csharp_saas_auth_e2e_{suffix[..12]}",
            ["tenant_attribute_name"] = $"csharp_tenant_auth_e2e_{suffix[..12]}",
            ["tenant_name"] = $"csharp-auth-e2e-tenant-{suffix[..12]}",
            ["env_id"] = envId,
            ["env_name"] = $"csharp-auth-e2e-env-{suffix[..12]}",
            ["env_display_name"] = $"C# Auth E2E Environment {suffix[..12]}",
            ["tenant_id"] = "auth-e2e-tenant",
            ["user_id"] = "auth-e2e-user",
            ["tenant_user_id"] = "auth-e2e-tenant-user",
            ["invitation_id"] = "auth-e2e-invitation",
            ["provider_name"] = options.ExternalProviderName,
            ["access_token"] = string.Empty,
            ["id_token"] = string.Empty,
            ["refresh_token"] = string.Empty,
            ["authorization_code"] = string.Empty,
            ["confirmation_code"] = options.EmailConfirmationCode,
            ["external_provider_access_token"] = options.ExternalProviderAccessToken,
            ["external_provider_code"] = options.ExternalProviderCode,
            ["software_token_secret"] = string.Empty,
            ["mfa_code"] = string.Empty,
            ["cognito_user_ready"] = false,
            ["expected_attribute_value"] = "csharp-auth-e2e",
            // Drives the self-registration opt-in for the SignUp story.
            ["signup_enabled"] = options.SignUpEnabled,
            ["user_created"] = false,
            ["tenant_created"] = false,
            ["tenant_user_created"] = false,
            ["tenant_user_role_created"] = false,
            ["invitation_created"] = false,
            ["env_created"] = false,
            ["role_created"] = false,
            ["user_attribute_created"] = false,
            ["saas_user_attribute_created"] = false,
            ["tenant_attribute_created"] = false,
            ["signup_user_created"] = false,
            ["signup_user_id"] = "auth-e2e-signup-user"
        };

        variables["expected_user_email"] = variables["user_email"];

        if (options.InitialTokens is { } tokens)
            AuthFixture.SetCognitoTokens(variables, tokens);
        return variables;
    }

    private static string CallStyleDescription(CallStyle callStyle) => callStyle switch
    {
        CallStyle.Sync => "synchronous responses",
        CallStyle.WithHttpInfo => "synchronous HTTP responses",
        CallStyle.Async => "asynchronous responses",
        CallStyle.WithHttpInfoAsync => "asynchronous HTTP responses",
        _ => callStyle.ToString()
    };
}

internal static class AuthFixture
{
    /// <summary>
    /// Records a resource the API has created, so cleanup can still delete it when the story fails
    /// before the engine updates the state - a malformed response, a timeout, or the Cognito
    /// provisioning that follows a <c>CreateSaasUser</c> call all abort the step.
    /// </summary>
    internal static void RecordCreatedResource(string operation, object? response, TestContext context)
    {
        var payload = response is IApiResponse apiResponse ? apiResponse.Content : response;
        switch (operation)
        {
            case "CreateSaasUser":
                SetCreatedResource(context, "user_id", "user_created", payload);
                break;
            case "SignUp":
                SetCreatedResource(context, "signup_user_id", "signup_user_created", payload);
                break;
            case "CreateTenant":
                SetCreatedResource(context, "tenant_id", "tenant_created", payload);
                break;
            case "CreateTenantUser":
                SetCreatedResource(context, "tenant_user_id", "tenant_user_created", payload);
                break;
            case "CreateTenantInvitation":
                SetCreatedResource(context, "invitation_id", "invitation_created", payload);
                break;
            case "CreateEnv":
                context.Variables["env_created"] = true;
                break;
            case "CreateTenantAndPricing":
                // Without this the Stripe tenant/pricing link survives a later failure and
                // poisons subsequent runs.
                context.Variables["stripe_pricing_created"] = true;
                break;
            case "CreateRole":
                context.Variables["role_created"] = true;
                break;
            case "CreateTenantUserRoles":
                context.Variables["tenant_user_role_created"] = true;
                break;
            case "CreateUserAttribute":
                context.Variables["user_attribute_created"] = true;
                break;
            case "CreateSaasUserAttribute":
                context.Variables["saas_user_attribute_created"] = true;
                break;
            case "CreateTenantAttribute":
                context.Variables["tenant_attribute_created"] = true;
                break;
        }
    }

    /// <summary>The variable that reports a created resource whose identifier is unknown.</summary>
    internal static string UnresolvedKey(string createdKey) => createdKey + "_unresolved";

    private static void SetCreatedResource(
        TestContext context,
        string idKey,
        string createdKey,
        object? response)
    {
        var id = response?.GetType().GetProperty("Id")?.GetValue(response)?.ToString();
        if (string.IsNullOrWhiteSpace(id))
        {
            // Without the server-generated identifier the resource cannot be addressed. Deleting the
            // fixture placeholder instead could remove an unrelated resource, so the resource is
            // marked unresolved and cleanup reports it rather than guessing.
            context.Variables[UnresolvedKey(createdKey)] = true;
            return;
        }

        context.Variables[idKey] = id;
        context.Variables[createdKey] = true;
        context.Variables[UnresolvedKey(createdKey)] = false;
    }

    internal static void SetCognitoTokens(TestContext context, CognitoTokens tokens) =>
        SetCognitoTokens(context.Variables, tokens);

    internal static void SetCognitoTokens(
        IDictionary<string, object?> variables,
        CognitoTokens tokens)
    {
        variables["access_token"] = tokens.AccessToken;
        variables["id_token"] = tokens.IdToken;
        variables["refresh_token"] = tokens.RefreshToken;
    }

    internal static object?[] BuildArguments(
        MethodInfo method,
        string operation,
        TestContext context,
        CancellationToken cancellationToken)
    {
        return method.GetParameters()
            .Select(parameter => BuildArgument(parameter, operation, context, cancellationToken))
            .ToArray();
    }

    private static object? BuildArgument(
        ParameterInfo parameter,
        string operation,
        TestContext context,
        CancellationToken cancellationToken)
    {
        var name = parameter.Name ?? string.Empty;
        var type = parameter.ParameterType;
        if (type == typeof(CancellationToken)) return cancellationToken;
        if (name == "operationIndex") return 0;
        if (type == typeof(string)) return StringValue(name, context, operation);
        if (type == typeof(int)) return IntegerValue(name, context);
        if (type == typeof(bool)) return false;
        if (type == typeof(object)) return null;
        return BuildModel(type, operation, context, 0);
    }

    private static object BuildModel(Type type, string operation, TestContext context, int depth)
    {
        if (depth > 3) return RuntimeHelpers.GetUninitializedObject(type);
        if (type.IsEnum) return Enum.GetValues(type).GetValue(0)!;
        if (Nullable.GetUnderlyingType(type) is { } nullableType)
            return BuildModel(nullableType, operation, context, depth + 1);

        // TenantIdentityProviderProps is a generated oneOf wrapper and has no
        // parameterless constructor.  Construct its valid SAML variant explicitly;
        // using an uninitialised wrapper makes its ActualInstance setter throw.
        if (type.Name == "TenantIdentityProviderProps" && type.Namespace == "authapi.Model")
        {
            var samlType = type.Assembly.GetType("authapi.Model.IdentityProviderSaml")
                ?? throw new InvalidOperationException("IdentityProviderSaml model was not generated.");
            var saml = BuildModel(samlType, operation, context, depth + 1);
            return Activator.CreateInstance(type, saml)
                ?? throw new InvalidOperationException("Could not construct TenantIdentityProviderProps.");
        }

        // Go/PHP send an empty reservation for UpdateTenantPlan.  Filling the
        // optional plan/tax IDs with fixture strings makes the live API reject
        // the request as an invalid Stripe tax-rate ID.
        if (type.Name == "PlanReservation" && type.Namespace == "authapi.Model")
            return Activator.CreateInstance(type, new object?[] { null, 0, null, null, false })
                ?? throw new InvalidOperationException("Could not construct PlanReservation.");

        // Single Tenant settings are shared SaaS state. The captured value is sent back so the
        // story exercises the endpoint without permanently disabling the environment's setting;
        // the optional role/template fields stay omitted because placeholder fixture values are
        // rejected as invalid content by the live API.
        if (type.Name == "UpdateSingleTenantSettingsParam" && type.Namespace == "authapi.Model")
        {
            var original = context.Variables.TryGetValue("original_single_tenant_settings", out var settings)
                ? settings
                : null;
            var enabled = original?.GetType().GetProperty("Enabled")?.GetValue(original) as bool? ?? false;
            return Activator.CreateInstance(type, new object?[] { enabled, null, null, null, null })
                ?? throw new InvalidOperationException("Could not construct UpdateSingleTenantSettingsParam.");
        }

        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            return BuildList(type.GetGenericArguments()[0], operation, context, depth + 1);
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            return BuildDictionary(type, context, operation);

        var instance = RuntimeHelpers.GetUninitializedObject(type);
        var source = SourceObject(type, context);
        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(property => property.CanWrite && property.SetMethod is not null))
        {
            object? value = null;
            var sourceProperty = source?.GetType().GetProperty(property.Name);
            if (sourceProperty is not null && sourceProperty.PropertyType == property.PropertyType)
            {
                value = sourceProperty.GetValue(source);
            }

            if (value is null)
                value = PropertyValue(property, operation, context, depth);

            try
            {
                property.SetValue(instance, value);
            }
            catch (ArgumentException)
            {
                // Some generated oneOf helper properties are intentionally not
                // assignable from a generic fixture value. They are not used by
                // operations exercised in this story.
            }
        }

        if (type.Name == "UpdateSignInSettingsParam" && type.Namespace == "authapi.Model")
            EnableSelfRegistration(instance, context);

        return instance;
    }

    private static object? PropertyValue(
        PropertyInfo property,
        string operation,
        TestContext context,
        int depth)
    {
        var type = property.PropertyType;
        if (type == typeof(string)) return StringValue(property.Name, context, operation);
        if (type == typeof(int)) return IntegerValue(property.Name, context);
        if (type == typeof(bool))
        {
            if (operation == "UpdateUserMfaPreference" &&
                property.Name.Equals("Enabled", StringComparison.OrdinalIgnoreCase))
                return true;
            return property.Name.Contains("Enabled", StringComparison.OrdinalIgnoreCase) ? false : true;
        }
        if (type.IsEnum || Nullable.GetUnderlyingType(type)?.IsEnum == true)
            return BuildModel(type, operation, context, depth + 1);
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
            return BuildList(type.GetGenericArguments()[0], operation, context, depth + 1);
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            return BuildDictionary(type, context, operation);
        if (type == typeof(object)) return null;
        if (type.Namespace == "authapi.Model") return BuildModel(type, operation, context, depth + 1);
        return type.IsValueType ? Activator.CreateInstance(type) : null;
    }

    private static object BuildList(Type elementType, string operation, TestContext context, int depth)
    {
        var listType = typeof(List<>).MakeGenericType(elementType);
        var list = (IList)Activator.CreateInstance(listType)!;
        if (elementType == typeof(string))
        {
            if (operation.Contains("Role", StringComparison.Ordinal)) list.Add(StringValue("roleName", context, operation));
            return list;
        }

        if (elementType.Namespace == "authapi.Model" &&
            elementType.Name == "InvitedUserEnvironmentInformationInner")
            list.Add(BuildModel(elementType, operation, context, depth));
        return list;
    }

    private static object BuildDictionary(Type dictionaryType, TestContext context, string operation)
    {
        var dictionary = (IDictionary)Activator.CreateInstance(dictionaryType)!;
        var attributeValue = "csharp-auth-e2e";
        context.Variables["expected_attribute_value"] = attributeValue;
        var key = operation.Contains("SaasUserAttributes", StringComparison.Ordinal)
            ? context.Variables.TryGetValue("saas_user_attribute_name", out var saasValue) && saasValue is string saasName
                ? saasName
                : "auth_e2e"
            : operation.Contains("TenantUser", StringComparison.Ordinal)
                ? context.Variables.TryGetValue("attribute_name", out var userValue) && userValue is string userName
                    ? userName
                    : "auth_e2e"
                : operation is "CreateTenant" or "UpdateTenant"
                    ? context.Variables.TryGetValue("tenant_attribute_name", out var tenantValue) && tenantValue is string tenantName
                        ? tenantName
                        : "auth_e2e"
                : "auth_e2e";
        dictionary[key] = attributeValue;
        return dictionary;
    }

    private static object? SourceObject(Type targetType, TestContext context)
    {
        var key = targetType.Name switch
        {
            "UpdateBasicInfoParam" => "original_basic_info",
            "AuthInfo" => "original_auth_info",
            "UpdateSignInSettingsParam" => "original_sign_in_settings",
            "UpdateNotificationMessagesParam" => "original_notification_messages",
            "UpdateCustomizePagesParam" => "original_customize_pages",
            "UpdateCustomizePageSettingsParam" => "original_customize_page_settings",
            "TenantProps" => "original_tenant",
            "BillingInfo" => "original_billing_info",
            _ => string.Empty
        };
        return key.Length > 0 && context.Variables.TryGetValue(key, out var value) ? value : null;
    }

    /// <summary>
    /// The opt-in SignUp story needs self-registration switched on. The flag is flipped on the
    /// captured settings so every other field keeps the environment's own value, and
    /// <see cref="CleanupAsync"/> restores the original settings afterwards.
    /// </summary>
    private static void EnableSelfRegistration(object parameter, TestContext context)
    {
        if (!(context.Variables.TryGetValue("signup_enabled", out var enabled) && enabled is true)) return;
        var selfRegist = parameter.GetType().GetProperty("SelfRegist")?.GetValue(parameter);
        var enable = selfRegist?.GetType().GetProperty("Enable");
        if (selfRegist is null || enable is null || !enable.CanWrite) return;
        if (enable.GetValue(selfRegist) as bool? == true) return;
        enable.SetValue(selfRegist, true);
        context.Variables["sign_in_settings_modified"] = true;
    }

    /// <summary>
    /// Reverts the SignUp opt-in before the restoring update. The captured settings share their
    /// <c>SelfRegist</c> instance with the parameter <see cref="EnableSelfRegistration"/> flipped, so
    /// the flag is flipped back and the opt-in switched off; otherwise the cleanup call would send
    /// <c>Enable = true</c> again and leave self-registration enabled. The opt-in only ever flips a
    /// disabled setting, so the original value is the disabled one.
    /// </summary>
    internal static void DisableSelfRegistration(TestContext context)
    {
        context.Variables["signup_enabled"] = false;
        if (!context.Variables.TryGetValue("original_sign_in_settings", out var settings) || settings is null)
            return;
        var selfRegist = settings.GetType().GetProperty("SelfRegist")?.GetValue(settings);
        var enable = selfRegist?.GetType().GetProperty("Enable");
        if (selfRegist is null || enable is null || !enable.CanWrite) return;
        enable.SetValue(selfRegist, false);
    }

    private static string StringValue(string parameterName, TestContext context, string operation)
    {
        var name = parameterName.ToLowerInvariant();
        string Read(string key, string fallback) =>
            context.Variables.TryGetValue(key, out var value) && value is string text && !string.IsNullOrWhiteSpace(text)
                ? text
                : fallback;

        if (name.Contains("backofficestaffemail", StringComparison.Ordinal))
            return Read("staff_email", "auth-e2e-staff@example.com");
        if (name.Contains("email", StringComparison.Ordinal) &&
            operation.Contains("TenantInvitation", StringComparison.Ordinal))
            return Read("staff_email", "auth-e2e-staff@example.com");
        if (name.Contains("email", StringComparison.Ordinal) &&
            operation is "CreateTenantUser" or "UpdateTenantUser")
            return Read("expected_user_email", "auth-e2e@example.com");
        if (name.Contains("email", StringComparison.Ordinal) &&
            (operation is "SignUp" or "ResendSignUpConfirmationEmail"))
            return Read("signup_email", "auth-e2e-signup@example.com");
        if (name.Contains("email", StringComparison.Ordinal) && operation is "RequestEmailUpdate")
            return Read("email_update_target", "auth-e2e-verification@example.com");
        if (name.Contains("email", StringComparison.Ordinal) && operation is "UpdateSaasUserEmail")
            return Read("updated_email", "auth-e2e-updated@example.com");
        if (name.Contains("email", StringComparison.Ordinal) && operation is "GetUserInfoByEmail")
            return Read("expected_user_email", "auth-e2e@example.com");
        if (name.Contains("email", StringComparison.Ordinal))
            return Read("user_email", "auth-e2e@example.com");
        if (name.Contains("password", StringComparison.Ordinal) &&
            operation is "UpdateSaasUserPassword")
            return Read("updated_password", "UpdatedPassw0rd!");
        if (name.Contains("password", StringComparison.Ordinal))
            return Read("password", "Passw0rd!");
        if (name.Contains("tenantid", StringComparison.Ordinal))
            return Read("tenant_id", "auth-e2e-tenant");
        if (name.Contains("userid", StringComparison.Ordinal) &&
            operation == "DeleteSaasUser" &&
            context.Variables.TryGetValue("cleanup_user_id", out var cleanupUserValue) &&
            cleanupUserValue is string cleanupUserId && !string.IsNullOrWhiteSpace(cleanupUserId))
            return cleanupUserId;
        if (name.Contains("userid", StringComparison.Ordinal) &&
            operation.Contains("TenantUser", StringComparison.Ordinal) &&
            context.Variables.TryGetValue("tenant_user_id", out var tenantUserValue) &&
            tenantUserValue is string tenantUserId && !string.IsNullOrWhiteSpace(tenantUserId))
            return tenantUserId;
        if (name.Contains("userid", StringComparison.Ordinal))
            return Read("user_id", "auth-e2e-user");
        if (name.Contains("invitationid", StringComparison.Ordinal))
            return Read("invitation_id", "auth-e2e-invitation");
        if (name.Contains("rolename", StringComparison.Ordinal))
            return Read("role_name", "auth-e2e-role");
        if (name.Contains("attributename", StringComparison.Ordinal) &&
            operation == "DeleteUserAttribute" &&
            context.Variables.TryGetValue("cleanup_attribute_name", out var cleanupAttributeValue) &&
            cleanupAttributeValue is string cleanupAttributeName && !string.IsNullOrWhiteSpace(cleanupAttributeName))
            return cleanupAttributeName;
        if (name.Contains("attributename", StringComparison.Ordinal) &&
            operation.Contains("TenantAttribute", StringComparison.Ordinal))
            return Read("tenant_attribute_name", "auth_e2e_tenant_attribute");
        if (name.Contains("attributename", StringComparison.Ordinal) &&
            operation.Contains("SaasUserAttribute", StringComparison.Ordinal))
            return Read("saas_user_attribute_name", "auth_e2e_saas_attribute");
        if (name.Contains("attributename", StringComparison.Ordinal))
            return Read("attribute_name", "auth_e2e_attribute");
        if (name.Contains("providername", StringComparison.Ordinal))
            return Read("provider_name", "Google");
        if (name.Contains("accesstoken", StringComparison.Ordinal) &&
            operation.Contains("ExternalUserLink", StringComparison.Ordinal) &&
            context.Variables.TryGetValue("external_provider_access_token", out var externalToken) &&
            externalToken is string externalAccessToken && !string.IsNullOrWhiteSpace(externalAccessToken))
            return externalAccessToken;
        if (name.Contains("accesstoken", StringComparison.Ordinal))
            return Read("access_token", string.Empty);
        if (name.Contains("idtoken", StringComparison.Ordinal))
            return Read("id_token", string.Empty);
        if (name.Contains("refreshtoken", StringComparison.Ordinal))
            return Read("refresh_token", string.Empty);
        if (name.Contains("verificationcode", StringComparison.Ordinal))
            return Read("mfa_code", "000000");
        if (name == "token") return Read("id_token", string.Empty);
        if (name.Contains("token", StringComparison.Ordinal)) return string.Empty;
        // The generated API accepts tempCodeAuth or refreshTokenAuth only, and the stories store a
        // temporary authorization code, so "refreshToken" would be rejected.
        if (name == "authflow") return "tempCodeAuth";
        if (name == "code" && (operation is "CreateAuthCredentials" or "GetAuthCredentials"))
            return Read("authorization_code", "auth-e2e-code");
        if (name == "code" && operation.Contains("ExternalUserLink", StringComparison.Ordinal))
            return Read("external_provider_code", "000000");
        if (name == "code") return Read("confirmation_code", "000000");
        if (name.Contains("domain", StringComparison.Ordinal)) return "auth-e2e.example.com";
        if (name.Contains("url", StringComparison.Ordinal)) return "https://auth-e2e.example.com";
        if (name.Contains("country", StringComparison.Ordinal)) return "JP";
        if (name.Contains("postal", StringComparison.Ordinal)) return "100-0001";
        if (name.Contains("state", StringComparison.Ordinal)) return "Tokyo";
        if (name.Contains("city", StringComparison.Ordinal)) return "Chiyoda";
        if (name.Contains("street", StringComparison.Ordinal)) return "1-1-1";
        if (name.Contains("displayname", StringComparison.Ordinal) &&
            operation.Contains("Env", StringComparison.Ordinal))
            return Read("env_display_name", "C# Auth E2E Environment");
        if (name.Equals("name", StringComparison.Ordinal) &&
            operation.Contains("Env", StringComparison.Ordinal))
            return Read("env_name", "auth-e2e-env");
        if (name.Contains("name", StringComparison.Ordinal))
            return Read("tenant_name", "auth-e2e-name");
        return "auth-e2e-value";
    }

    private static int IntegerValue(string parameterName, TestContext context)
    {
        if (parameterName.Contains("envid", StringComparison.OrdinalIgnoreCase) ||
            parameterName.Equals("id", StringComparison.OrdinalIgnoreCase))
        {
            if (context.Variables.TryGetValue("env_id", out var value) && value is int environmentId)
                return environmentId;
        }
        if (parameterName.Contains("expired", StringComparison.OrdinalIgnoreCase))
            return (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 86_400;
        return 1;
    }
}
