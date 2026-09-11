using System.Runtime.CompilerServices;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Auth;

public sealed class AuthStoryFactoryTests
{
    [Fact]
    public void ValidatesTypedIdsAttributesCollectionsAndVoidResponses()
    {
        var variables = new Dictionary<string, object?>(AuthStoryFactory.CreateVariables(AuthStoryOptions.Offline(true)))
        {
            ["user_id"] = "user-1",
            ["user_created"] = true,
            ["saas_user_attribute_created"] = true,
            ["saas_user_attribute_name"] = "department",
            ["expected_attribute_value"] = "engineering"
        };
        var context = new TestContext(variables);
        var user = new authapi.Model.SaasUser(
            "user-1",
            context.GetRequired<string>("expected_user_email"),
            new Dictionary<string, object> { ["department"] = "engineering" });

        AuthResponseValidator.Validate("GetSaasUser", typeof(authapi.Model.SaasUser),
            ExecutionResult.Success(user), context);
        AuthResponseValidator.Validate("UpdateSaasUserPassword", null,
            ExecutionResult.Success(), context);

        Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.Validate(
            "GetSaasUser", typeof(authapi.Model.SaasUser), ExecutionResult.Success(new object()), context));
        Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.Validate(
            "UpdateSaasUserPassword", null, ExecutionResult.Success(new object()), context));
    }

    [Fact]
    public void ValidatesGeneratedEnvironmentCollectionPropertyAndRawResponseShape()
    {
        var variables = new Dictionary<string, object?>(AuthStoryFactory.CreateVariables(AuthStoryOptions.Offline(true)))
        {
            ["env_id"] = 42,
            ["env_created"] = true
        };
        var context = new TestContext(variables);
        var envs = new authapi.Model.Envs(new List<authapi.Model.Env>
        {
            new(42, context.GetRequired<string>("env_name"))
        });

        AuthResponseValidator.Validate("GetEnvs", typeof(authapi.Model.Envs),
            ExecutionResult.Success(envs), context);
        AuthResponseValidator.ValidateRaw(
            "GetBasicInfo",
            CallStyle.WithHttpInfo,
            ExecutionResult.WithHttp(
                new object(),
                200,
                new Dictionary<string, IReadOnlyList<string>>
                {
                    ["Content-Type"] = new[] { "application/json" }
                }) with { Body = "{\"ok\":true}" });

        Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.ValidateRaw(
            "GetBasicInfo", CallStyle.WithHttpInfo, ExecutionResult.WithHttp(new object(), 200)));
    }

    [Fact]
    public void PreservesDefaultDependencySkipBehavior()
    {
        var stories = AuthStoryFactory.Create(new RecordingAuthInvoker());
        var basicInfoSteps = stories.SelectMany(story => story.Steps)
            .Where(step => AuthStoryFactory.BaseOperation(step.Method) == "GetBasicInfo");
        var cognitoSteps = stories.SelectMany(story => story.Steps)
            .Where(step => AuthStoryFactory.BaseOperation(step.Method) == "GetUserInfo");

        // Assert.All passes for an empty sequence, so the expected surface is pinned first.
        Assert.Equal(4, basicInfoSteps.Count());
        Assert.Equal(4, cognitoSteps.Count());
        Assert.All(basicInfoSteps, step => Assert.False(step.Skip));
        Assert.All(cognitoSteps, step => Assert.True(step.Skip));
    }

    [Fact]
    public async Task ExecutesEveryGeneratedAuthMethodInEveryCallStyleOffline()
    {
        var invoker = new RecordingAuthInvoker();
        var stories = AuthStoryFactory.Create(invoker, executeEnvironmentDependentOperations: true);
        var coverage = new CoverageTracker();
        foreach (var method in AuthStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);

        var results = await new E2EEngine(new Config
        {
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }, coverage).ExecuteAsync(stories);

        Assert.Equal(4, results.Count);
        Assert.Equal(AuthStoryFactory.Methods.Count, results.Sum(result => result.Steps.Count));
        Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
        Assert.Empty(coverage.Untested);
        Assert.Equal(AuthStoryFactory.Methods.Count, coverage.Entries.Count);
        Assert.Equal(AuthStoryFactory.Methods.Count, invoker.Calls.Count);
        Assert.Equal(
            Enum.GetValues<CallStyle>().SelectMany(callStyle => AuthStoryFactory.BaseMethods
                .Select(operation => (AuthStoryFactory.GeneratedMethodName(operation, callStyle), callStyle))),
            invoker.Calls);
    }

    [Fact]
    public void TracksTheCurrentGeneratedAuthSurfaceAndAllFourCallStyles()
    {
        Assert.Equal(85, AuthStoryFactory.BaseMethods.Count);
        Assert.Equal(AuthStoryFactory.BaseMethods.Count * 4, AuthStoryFactory.Methods.Count);

        // Discovered from the generated assembly rather than from AuthStoryFactory's own list, so
        // omitting an API type is detected instead of moving both sides of the comparison together.
        // ErrorApi hosts only the intentional 500 endpoint, which the Go and PHP suites also skip.
        var discoveredApiTypes = typeof(authapi.Api.BasicInfoApi).Assembly.GetTypes()
            .Where(type => type.IsClass && type.IsPublic && !type.IsAbstract &&
                           type.Namespace == "authapi.Api" &&
                           type.Name.EndsWith("Api", StringComparison.Ordinal) &&
                           type.Name != "ErrorApi")
            .OrderBy(type => type.Name, StringComparer.Ordinal)
            .ToArray();
        Assert.Equal(
            discoveredApiTypes,
            AuthStoryFactory.GeneratedApiTypes.OrderBy(type => type.Name, StringComparer.Ordinal));

        var generatedMethods = discoveredApiTypes
            .SelectMany(type => type.GetMethods()
                .Where(method => method.DeclaringType == type && !method.IsSpecialName && IsBaseMethod(method.Name)))
            .SelectMany(method => new[]
            {
                method.Name,
                $"{method.Name}WithHttpInfo",
                $"{method.Name}Async",
                $"{method.Name}WithHttpInfoAsync"
            })
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        var declaredMethods = AuthStoryFactory.Methods
            .Select(method => method.Method)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(generatedMethods, declaredMethods);
    }

    [Fact]
    public void BindsEveryGeneratedAuthMethodAndBuildsItsArgumentsOffline()
    {
        var client = new AuthApiClient(new modules.Configuration().GetAuthApiClientConfig());
        foreach (var story in AuthStoryFactory.Create(client, executeEnvironmentDependentOperations: true))
        {
            var context = new TestContext(new Dictionary<string, object?>(story.InitialVariables));
            foreach (var step in story.Steps)
            {
                var parameters = Assert.IsType<Func<TestContext, object?>>(step.Parameters);
                Assert.NotNull(parameters(context));
            }
        }
    }

    [Fact]
    public async Task ExecutesRawResponseStoriesOffline()
    {
        var invoker = new RecordingAuthInvoker();
        var stories = AuthRawResponseStoryFactory.Create(invoker, AuthStoryOptions.Offline(true));
        var results = await new E2EEngine(new Config
        {
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }).ExecuteAsync(stories);

        Assert.Equal(2, results.Count);
        Assert.Equal(32, results.Sum(result => result.Steps.Count));
        Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
        Assert.All(results.SelectMany(result => result.Steps), step =>
            Assert.Contains(step.CallStyle, new[] { CallStyle.WithHttpInfo, CallStyle.WithHttpInfoAsync }));
        // The recorded operations are compared against an independent set: a duplicated or
        // unrelated operation would otherwise still produce 32 passing steps.
        Assert.Equal(
            RawResponseOperations.OrderBy(name => name, StringComparer.Ordinal),
            invoker.Calls.Select(call => AuthStoryFactory.BaseOperation(call.Method))
                .Distinct(StringComparer.Ordinal)
                .OrderBy(name => name, StringComparer.Ordinal));
    }

    [Fact]
    public void RecordsCreatedAuthResourcesBeforeTheResponseIsValidated()
    {
        var context = new TestContext(new Dictionary<string, object?>(
            AuthStoryFactory.CreateVariables(AuthStoryOptions.Offline(true))));

        AuthFixture.RecordCreatedResource(
            "CreateSaasUser",
            new authapi.Model.SaasUser("user-77", "auth-e2e@example.com", new Dictionary<string, object>()),
            context);
        AuthFixture.RecordCreatedResource("CreateSaasUserAttribute", null, context);
        AuthFixture.RecordCreatedResource("CreateTenantUserRoles", null, context);
        // A create whose response carries no identifier must not be recorded under the fixture
        // placeholder: cleanup would delete an unrelated resource with that name.
        AuthFixture.RecordCreatedResource("CreateTenant", new object(), context);

        Assert.Equal("user-77", context.Variables["user_id"]);
        Assert.Equal(true, context.Variables["user_created"]);
        Assert.Equal(true, context.Variables["saas_user_attribute_created"]);
        Assert.Equal(true, context.Variables["tenant_user_role_created"]);
        Assert.Equal(false, context.Variables["tenant_created"]);
        Assert.Equal(true, context.Variables[AuthFixture.UnresolvedKey("tenant_created")]);
    }

    [Fact]
    public void RestoresTheOriginalSelfRegistrationSettingDuringCleanup()
    {
        // Only the captured SelfRegist matters here, and the generated model rejects the null
        // required properties a hand-built instance would need.
        var captured = (authapi.Model.SignInSettings)RuntimeHelpers.GetUninitializedObject(
            typeof(authapi.Model.SignInSettings));
        captured.SelfRegist = new authapi.Model.SelfRegist(false);
        var variables = new Dictionary<string, object?>(AuthStoryFactory.CreateVariables(
            AuthStoryOptions.Offline(true)))
        {
            ["signup_enabled"] = true,
            ["original_sign_in_settings"] = captured
        };
        var context = new TestContext(variables);
        var method = typeof(authapi.Api.AuthInfoApi).GetMethod(
            "UpdateSignInSettings",
            new[] { typeof(authapi.Model.UpdateSignInSettingsParam), typeof(int) })!;

        var optIn = Assert.IsType<authapi.Model.UpdateSignInSettingsParam>(
            AuthFixture.BuildArguments(method, "UpdateSignInSettings", context, CancellationToken.None)[0]);
        Assert.True(optIn.SelfRegist.Enable);
        Assert.Equal(true, context.Variables["sign_in_settings_modified"]);

        AuthFixture.DisableSelfRegistration(context);
        var restored = Assert.IsType<authapi.Model.UpdateSignInSettingsParam>(
            AuthFixture.BuildArguments(method, "UpdateSignInSettings", context, CancellationToken.None)[0]);
        Assert.False(restored.SelfRegist.Enable);
    }

    [Fact]
    public void DeletesTheSaasLevelAttributeWithItsOwnNameDuringCleanup()
    {
        var context = new TestContext(new Dictionary<string, object?>(
            AuthStoryFactory.CreateVariables(AuthStoryOptions.Offline(true))));
        var method = typeof(authapi.Api.UserAttributeApi).GetMethod(
            "DeleteUserAttribute",
            new[] { typeof(string), typeof(int) })!;

        Assert.Equal(
            context.GetRequired<string>("attribute_name"),
            AuthFixture.BuildArguments(method, "DeleteUserAttribute", context, CancellationToken.None)[0]);

        context.Variables["cleanup_attribute_name"] = context.GetRequired<string>("saas_user_attribute_name");
        Assert.Equal(
            context.GetRequired<string>("saas_user_attribute_name"),
            AuthFixture.BuildArguments(method, "DeleteUserAttribute", context, CancellationToken.None)[0]);
    }

    [Fact]
    public void SignsTheConfiguredCognitoEndpointPath()
    {
        Assert.Equal("/", CognitoTokenProvider.CanonicalPath(
            new Uri("https://cognito-idp.ap-northeast-1.amazonaws.com/")));
        Assert.Equal("/cognito-idp/", CognitoTokenProvider.CanonicalPath(
            new Uri("http://localhost:4566/cognito-idp/")));
    }

    /// <summary>
    /// The read-only operations the raw-response stories must cover, spelled out here so a change to
    /// AuthRawResponseStoryFactory's own list cannot silently move the expectation with it.
    /// </summary>
    private static readonly string[] RawResponseOperations =
    {
        "GetBasicInfo", "GetAuthInfo", "GetSaasUsers", "GetRoles", "GetUserAttributes",
        "GetTenantAttributes", "GetTenants", "GetAllTenantUsers", "FindNotificationMessages",
        "GetIdentityProviders", "GetSignInSettings", "GetCustomizePages", "GetCustomizePageSettings",
        "GetEnvs", "GetSingleTenantSettings", "GetCloudFormationLaunchStackLinkForSingleTenant"
    };

    private static bool IsBaseMethod(string name) =>        name != "ReturnInternalServerError" &&
        name != "GetBasePath" &&
        !name.EndsWith("WithHttpInfo", StringComparison.Ordinal) &&
        !name.EndsWith("Async", StringComparison.Ordinal);

    private sealed class RecordingAuthInvoker : IAuthInvoker
    {
        public List<(string Method, CallStyle CallStyle)> Calls { get; } = new();

        public Task<ExecutionResult> InvokeAsync(
            string operation,
            CallStyle callStyle,
            TestContext context,
            CancellationToken cancellationToken)
        {
            Calls.Add((AuthStoryFactory.GeneratedMethodName(operation, callStyle), callStyle));
            if (callStyle is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync)
            {
                return Task.FromResult(ExecutionResult.WithHttp(
                    new object(),
                    // Independent of AuthStoryFactory: a shared helper would move the fake and the
                    // expectation together and never test the contract.
                    ExpectedStatus(operation),
                    new Dictionary<string, IReadOnlyList<string>>
                    {
                        ["Content-Type"] = new[] { "application/json" }
                    }) with { Body = "{\"ok\":true}" });
            }

            return Task.FromResult(ExecutionResult.Success(new object()));
        }

        /// <summary>The status the API answers with, spelled out independently of the factory.</summary>
        private static int ExpectedStatus(string operation) =>
            operation.StartsWith("Create", StringComparison.Ordinal) ||
            operation.StartsWith("SignUp", StringComparison.Ordinal)
                ? 201
                : 200;

        public IReadOnlyDictionary<string, object?> DescribeParameters(
            string operation,
            CallStyle callStyle,
            TestContext context) =>
            new Dictionary<string, object?>
            {
                ["operation"] = operation,
                ["call_style"] = callStyle.ToString()
            };

        // The per-operation content assertions need real generated models, so the response
        // contract itself is covered directly by AuthResponseValidatorTests instead.
        public Task ValidateResponseAsync(
            string operation,
            CallStyle callStyle,
            ExecutionResult result,
            TestContext context) =>
            Task.CompletedTask;

        public Task ValidateRawResponseAsync(
            string operation,
            CallStyle callStyle,
            ExecutionResult result,
            TestContext context)
        {
            AuthResponseValidator.ValidateRaw(operation, callStyle, result);
            return Task.CompletedTask;
        }

        public Task CleanupAsync(TestContext context, CancellationToken cancellationToken) =>
            Task.CompletedTask;
    }
}
