using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Auth;

internal static class AuthRawResponseStoryFactory
{
    private static readonly IReadOnlyList<string> ReadOnlyOperations = new[]
    {
        "GetBasicInfo",
        "GetAuthInfo",
        "GetSaasUsers",
        "GetRoles",
        "GetUserAttributes",
        "GetTenantAttributes",
        "GetTenants",
        "GetAllTenantUsers",
        "FindNotificationMessages",
        "GetIdentityProviders",
        "GetSignInSettings",
        "GetCustomizePages",
        "GetCustomizePageSettings",
        "GetEnvs",
        "GetSingleTenantSettings",
        "GetCloudFormationLaunchStackLinkForSingleTenant"
    };

    internal static IReadOnlyList<Story> Create(
        IAuthInvoker client,
        AuthStoryOptions options) =>
        new[]
        {
            CreateStory(client, options, CallStyle.WithHttpInfo),
            CreateStory(client, options, CallStyle.WithHttpInfoAsync)
        };

    private static Story CreateStory(
        IAuthInvoker client,
        AuthStoryOptions options,
        CallStyle callStyle)
    {
        var suffix = callStyle == CallStyle.WithHttpInfo ? "synchronous" : "asynchronous";
        return new Story
        {
            Name = $"Auth API - Raw responses ({suffix})",
            Description = "Validates Auth read-only HTTP responses, headers, raw JSON bodies, and deserialized content.",
            Module = "auth",
            InitialVariables = AuthStoryFactory.CreateVariables(options),
            Steps = ReadOnlyOperations.Select(operation => new Step
            {
                Name = $"{operation}RawResponse",
                Method = AuthStoryFactory.GeneratedMethodName(operation, callStyle),
                CallStyle = callStyle,
                ExpectedStatus = 200,
                Parameters = (Func<TestContext, object?>)(context =>
                    client.DescribeParameters(operation, callStyle, context)),
                ExecuteAsync = (context, cancellationToken) =>
                    client.InvokeAsync(operation, callStyle, context, cancellationToken),
                ValidateAsync = (result, context) =>
                    client.ValidateRawResponseAsync(operation, callStyle, result, context)
            }).ToArray(),
            CleanupAsync = (_, _) => Task.CompletedTask
        };
    }
}
