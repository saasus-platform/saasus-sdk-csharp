using SaasusSdk.Tests.E2E.Billing;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Auth;

internal sealed class AuthStoryOptions
{
    public bool ExecuteEnvironmentDependentOperations { get; init; }
    public bool IgnoreDependencyChecks { get; init; }
    public Config? ExecutionConfig { get; init; }
    public CognitoTokenProvider? Cognito { get; init; }
    public bool StripeEnabled { get; init; }
    public bool SignUpEnabled { get; init; }
    public bool EmailUpdateEnabled { get; init; }
    public bool ExternalProviderEnabled { get; init; }
    public string ExternalProviderName { get; init; } = "Google";
    public string ExternalProviderAccessToken { get; init; } = string.Empty;
    public string ExternalProviderCode { get; init; } = string.Empty;
    public string EmailConfirmationCode { get; init; } = string.Empty;
    public string DefaultPassword { get; init; } = "Passw0rd!";
    public string UpdatedPassword { get; init; } = "UpdatedPassw0rd!";

    public CognitoTokens? InitialTokens => Cognito?.ConfiguredUserTokens;

    public bool CognitoUserFlowsEnabled => Cognito?.CanProvisionUsers == true;

    public static AuthStoryOptions Offline(bool executeEnvironmentDependentOperations) => new()
    {
        ExecuteEnvironmentDependentOperations = executeEnvironmentDependentOperations,
        IgnoreDependencyChecks = executeEnvironmentDependentOperations
    };

    public static AuthStoryOptions Live(
        Config config,
        CognitoTokenProvider? cognito,
        bool stripeEnabled) => new()
        {
            ExecuteEnvironmentDependentOperations = true,
            ExecutionConfig = config,
            Cognito = cognito,
            StripeEnabled = stripeEnabled,
            SignUpEnabled = config.AuthSignUpEnabled,
            EmailUpdateEnabled = cognito?.CanProvisionUsers == true &&
                                 !string.IsNullOrWhiteSpace(config.AuthEmailConfirmationCode),
            ExternalProviderEnabled = cognito?.CanProvisionUsers == true &&
                                      !string.IsNullOrWhiteSpace(config.AuthExternalProviderName) &&
                                      !string.IsNullOrWhiteSpace(config.AuthExternalProviderCode),
            ExternalProviderName = string.IsNullOrWhiteSpace(config.AuthExternalProviderName)
                ? "Google"
                : config.AuthExternalProviderName,
            ExternalProviderAccessToken = config.AuthExternalProviderAccessToken,
            ExternalProviderCode = config.AuthExternalProviderCode,
            EmailConfirmationCode = config.AuthEmailConfirmationCode,
            DefaultPassword = config.AuthDefaultPassword,
            UpdatedPassword = config.AuthUpdatedPassword
        };
}

internal sealed record AuthStripePreparationResult(
    bool Enabled,
    string Reason);

internal static class AuthStripeIntegration
{
    /// <summary>
    /// Registers the Stripe secret key for the run. The registration is idempotent and is issued
    /// before every story, matching the Go suite's <c>attachStripeSetupToStories</c> and PHP's
    /// story setup: an environment that reports itself registered can still have lost the key -
    /// the Billing story removes it - and the Auth endpoints then answer
    /// <c>stripe key is not registered</c>.
    /// </summary>
    public static async Task<AuthStripePreparationResult> TryPrepareAsync(
        Config config,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(config.StripeSecretKey))
            return new(false, "STRIPE_SECRET_KEY is not configured.");

        if (config.DryRun)
            return new(false, "Stripe setup is disabled in dry-run mode.");

        try
        {
            var moduleConfiguration = new modules.Configuration().GetBillingApiClientConfig();
            var client = new BillingStripeClient(new billingapi.Api.StripeApi(moduleConfiguration));
            var updated = await client.UpdateStripeInfoWithHttpInfoAsync(
                new billingapi.Model.UpdateStripeInfoParam(config.StripeSecretKey),
                cancellationToken).ConfigureAwait(false);
            if ((int)updated.StatusCode is not (200 or 201))
                return new(false, $"Billing API returned HTTP {(int)updated.StatusCode} while configuring Stripe.");

            var registered = await client.GetStripeInfoWithHttpInfoAsync(cancellationToken).ConfigureAwait(false);
            if ((int)registered.StatusCode != 200 || registered.Data?.IsRegistered != true)
                return new(false, "Billing API did not report Stripe as registered after configuration.");

            return new(true, string.Empty);
        }
        catch (billingapi.Client.ApiException error) when (error.ErrorCode == 400 && IsAlreadyRegistered(error))
        {
            // The API rejects a repeated registration: an existing integration is present and
            // usable, which is exactly what the stories need. Like Go and PHP, it is left in place.
            return new(true, string.Empty);
        }
        catch (Exception error)
        {
            return new(false, $"Stripe setup failed: {Masker.Redact(error.Message)}");
        }
    }

    /// <summary>Recognises the API's response to a call made without a usable Stripe key.</summary>
    public static bool IsStripeKeyMissing(Exception error)
    {
        var content = error is authapi.Client.ApiException apiError
            ? apiError.ErrorContent?.ToString() ?? apiError.Message
            : error.Message;
        return content.Contains("stripe key", StringComparison.OrdinalIgnoreCase) &&
               content.Contains("not registered", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>Recognises the API's "already configured" rejection of a repeated registration.</summary>
    private static bool IsAlreadyRegistered(billingapi.Client.ApiException error)
    {
        var content = error.ErrorContent?.ToString() ?? error.Message;
        return content.Contains("already", StringComparison.OrdinalIgnoreCase) ||
               content.Contains("exist", StringComparison.OrdinalIgnoreCase) ||
               content.Contains("registered", StringComparison.OrdinalIgnoreCase);
    }
}
