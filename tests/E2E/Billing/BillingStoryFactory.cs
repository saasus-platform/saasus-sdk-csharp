using System.Net;
using billingapi.Client;
using billingapi.Model;
using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Billing;

internal interface IBillingStripeClient
{
    StripeInfo GetStripeInfo();
    ApiResponse<StripeInfo> GetStripeInfoWithHttpInfo();
    Task<StripeInfo> GetStripeInfoAsync(CancellationToken cancellationToken);
    Task<ApiResponse<StripeInfo>> GetStripeInfoWithHttpInfoAsync(CancellationToken cancellationToken);
    void UpdateStripeInfo(UpdateStripeInfoParam parameter);
    ApiResponse<object> UpdateStripeInfoWithHttpInfo(UpdateStripeInfoParam parameter);
    Task UpdateStripeInfoAsync(UpdateStripeInfoParam parameter, CancellationToken cancellationToken);
    Task<ApiResponse<object>> UpdateStripeInfoWithHttpInfoAsync(
        UpdateStripeInfoParam parameter,
        CancellationToken cancellationToken);
    void DeleteStripeInfo();
    ApiResponse<object> DeleteStripeInfoWithHttpInfo();
    Task DeleteStripeInfoAsync(CancellationToken cancellationToken);
    Task<ApiResponse<object>> DeleteStripeInfoWithHttpInfoAsync(CancellationToken cancellationToken);
}

internal sealed class BillingStripeClient : IBillingStripeClient
{
    private readonly billingapi.Api.IStripeApi _api;

    public BillingStripeClient(billingapi.Api.IStripeApi api) => _api = api;

    public StripeInfo GetStripeInfo() => _api.GetStripeInfo();
    public ApiResponse<StripeInfo> GetStripeInfoWithHttpInfo() => _api.GetStripeInfoWithHttpInfo();
    public Task<StripeInfo> GetStripeInfoAsync(CancellationToken cancellationToken) =>
        _api.GetStripeInfoAsync(cancellationToken: cancellationToken);
    public Task<ApiResponse<StripeInfo>> GetStripeInfoWithHttpInfoAsync(CancellationToken cancellationToken) =>
        _api.GetStripeInfoWithHttpInfoAsync(cancellationToken: cancellationToken);
    public void UpdateStripeInfo(UpdateStripeInfoParam parameter) => _api.UpdateStripeInfo(parameter);
    public ApiResponse<object> UpdateStripeInfoWithHttpInfo(UpdateStripeInfoParam parameter) =>
        _api.UpdateStripeInfoWithHttpInfo(parameter);
    public Task UpdateStripeInfoAsync(UpdateStripeInfoParam parameter, CancellationToken cancellationToken) =>
        _api.UpdateStripeInfoAsync(parameter, cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> UpdateStripeInfoWithHttpInfoAsync(
        UpdateStripeInfoParam parameter,
        CancellationToken cancellationToken) =>
        _api.UpdateStripeInfoWithHttpInfoAsync(parameter, cancellationToken: cancellationToken);
    public void DeleteStripeInfo() => _api.DeleteStripeInfo();
    public ApiResponse<object> DeleteStripeInfoWithHttpInfo() => _api.DeleteStripeInfoWithHttpInfo();
    public Task DeleteStripeInfoAsync(CancellationToken cancellationToken) =>
        _api.DeleteStripeInfoAsync(cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> DeleteStripeInfoWithHttpInfoAsync(CancellationToken cancellationToken) =>
        _api.DeleteStripeInfoWithHttpInfoAsync(cancellationToken: cancellationToken);
}

internal static class BillingStoryFactory
{
    internal static readonly IReadOnlyList<(string Method, CallStyle CallStyle)> Methods = new[]
    {
        ("GetStripeInfo", CallStyle.Sync),
        ("UpdateStripeInfo", CallStyle.Sync),
        ("DeleteStripeInfo", CallStyle.Sync),
        ("GetStripeInfoWithHttpInfo", CallStyle.WithHttpInfo),
        ("UpdateStripeInfoWithHttpInfo", CallStyle.WithHttpInfo),
        ("DeleteStripeInfoWithHttpInfo", CallStyle.WithHttpInfo),
        ("GetStripeInfoAsync", CallStyle.Async),
        ("UpdateStripeInfoAsync", CallStyle.Async),
        ("DeleteStripeInfoAsync", CallStyle.Async),
        ("GetStripeInfoWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("UpdateStripeInfoWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("DeleteStripeInfoWithHttpInfoAsync", CallStyle.WithHttpInfoAsync)
    };

    public static IReadOnlyList<Story> Create(IBillingStripeClient client, string stripeSecretKey) =>
        new[]
        {
            CreateSyncStory(client, stripeSecretKey),
            CreateWithHttpInfoStory(client, stripeSecretKey),
            CreateAsyncStory(client, stripeSecretKey),
            CreateWithHttpInfoAsyncStory(client, stripeSecretKey)
        };

    private static Story CreateSyncStory(IBillingStripeClient client, string stripeSecretKey) => new()
    {
        Name = "Billing API - synchronous responses",
        Description = "Reproduces the Billing Postman lifecycle with synchronous object methods.",
        Module = "billing",
        InitialVariables = Variables(stripeSecretKey),
        SetupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
        CleanupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
        Steps = new[]
        {
            GetStep("Pre_GetStripeConnectionInformation", "GetStripeInfo", CallStyle.Sync,
                MethodExecutor.Sync(_ => client.GetStripeInfo()), false),
            UpdateStep("UpdateStripeConnectionInformation", "UpdateStripeInfo", CallStyle.Sync,
                MethodExecutor.Sync(context => client.UpdateStripeInfo(Parameter(context)))),
            GetStep("GetStripeConnectionInformation", "GetStripeInfo", CallStyle.Sync,
                MethodExecutor.Sync(_ => client.GetStripeInfo()), true),
            DeleteStep("DeleteStripeConnectionInformation", "DeleteStripeInfo", CallStyle.Sync,
                MethodExecutor.Sync(_ => client.DeleteStripeInfo())),
            GetStep("Final_GetStripeConnectionInformation", "GetStripeInfo", CallStyle.Sync,
                MethodExecutor.Sync(_ => client.GetStripeInfo()), false)
        }
    };

    private static Story CreateWithHttpInfoStory(IBillingStripeClient client, string stripeSecretKey) => new()
    {
        Name = "Billing API - synchronous HTTP responses",
        Description = "Reproduces the Billing Postman lifecycle with synchronous WithHttpInfo methods.",
        Module = "billing",
        InitialVariables = Variables(stripeSecretKey),
        SetupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
        CleanupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
        Steps = new[]
        {
            GetStep("Pre_GetStripeConnectionInformation", "GetStripeInfoWithHttpInfo", CallStyle.WithHttpInfo,
                MethodExecutor.WithHttpInfo(_ => Result(client.GetStripeInfoWithHttpInfo())), false),
            UpdateStep("UpdateStripeConnectionInformation", "UpdateStripeInfoWithHttpInfo", CallStyle.WithHttpInfo,
                MethodExecutor.WithHttpInfo(context => Result(client.UpdateStripeInfoWithHttpInfo(Parameter(context))))),
            GetStep("GetStripeConnectionInformation", "GetStripeInfoWithHttpInfo", CallStyle.WithHttpInfo,
                MethodExecutor.WithHttpInfo(_ => Result(client.GetStripeInfoWithHttpInfo())), true),
            DeleteStep("DeleteStripeConnectionInformation", "DeleteStripeInfoWithHttpInfo", CallStyle.WithHttpInfo,
                MethodExecutor.WithHttpInfo(_ => Result(client.DeleteStripeInfoWithHttpInfo()))),
            GetStep("Final_GetStripeConnectionInformation", "GetStripeInfoWithHttpInfo", CallStyle.WithHttpInfo,
                MethodExecutor.WithHttpInfo(_ => Result(client.GetStripeInfoWithHttpInfo())), false)
        }
    };

    private static Story CreateAsyncStory(IBillingStripeClient client, string stripeSecretKey) => new()
    {
        Name = "Billing API - asynchronous responses",
        Description = "Reproduces the Billing Postman lifecycle with asynchronous object methods.",
        Module = "billing",
        InitialVariables = Variables(stripeSecretKey),
        SetupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
        CleanupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
        Steps = new[]
        {
            GetStep("Pre_GetStripeConnectionInformation", "GetStripeInfoAsync", CallStyle.Async,
                MethodExecutor.Async(async (_, token) => await client.GetStripeInfoAsync(token)), false),
            UpdateStep("UpdateStripeConnectionInformation", "UpdateStripeInfoAsync", CallStyle.Async,
                MethodExecutor.Async((context, token) => client.UpdateStripeInfoAsync(Parameter(context), token))),
            GetStep("GetStripeConnectionInformation", "GetStripeInfoAsync", CallStyle.Async,
                MethodExecutor.Async(async (_, token) => await client.GetStripeInfoAsync(token)), true),
            DeleteStep("DeleteStripeConnectionInformation", "DeleteStripeInfoAsync", CallStyle.Async,
                MethodExecutor.Async((_, token) => client.DeleteStripeInfoAsync(token))),
            GetStep("Final_GetStripeConnectionInformation", "GetStripeInfoAsync", CallStyle.Async,
                MethodExecutor.Async(async (_, token) => await client.GetStripeInfoAsync(token)), false)
        }
    };

    private static Story CreateWithHttpInfoAsyncStory(
        IBillingStripeClient client,
        string stripeSecretKey) => new()
        {
            Name = "Billing API - asynchronous HTTP responses",
            Description = "Reproduces the Billing Postman lifecycle with asynchronous WithHttpInfo methods.",
            Module = "billing",
            InitialVariables = Variables(stripeSecretKey),
            SetupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
            CleanupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
            Steps = new[]
        {
            GetStep("Pre_GetStripeConnectionInformation", "GetStripeInfoWithHttpInfoAsync", CallStyle.WithHttpInfoAsync,
                MethodExecutor.WithHttpInfoAsync(async (_, token) => Result(
                    await client.GetStripeInfoWithHttpInfoAsync(token))), false),
            UpdateStep("UpdateStripeConnectionInformation", "UpdateStripeInfoWithHttpInfoAsync", CallStyle.WithHttpInfoAsync,
                MethodExecutor.WithHttpInfoAsync(async (context, token) => Result(
                    await client.UpdateStripeInfoWithHttpInfoAsync(Parameter(context), token)))),
            GetStep("GetStripeConnectionInformation", "GetStripeInfoWithHttpInfoAsync", CallStyle.WithHttpInfoAsync,
                MethodExecutor.WithHttpInfoAsync(async (_, token) => Result(
                    await client.GetStripeInfoWithHttpInfoAsync(token))), true),
            DeleteStep("DeleteStripeConnectionInformation", "DeleteStripeInfoWithHttpInfoAsync", CallStyle.WithHttpInfoAsync,
                MethodExecutor.WithHttpInfoAsync(async (_, token) => Result(
                    await client.DeleteStripeInfoWithHttpInfoAsync(token)))),
            GetStep("Final_GetStripeConnectionInformation", "GetStripeInfoWithHttpInfoAsync", CallStyle.WithHttpInfoAsync,
                MethodExecutor.WithHttpInfoAsync(async (_, token) => Result(
                    await client.GetStripeInfoWithHttpInfoAsync(token))), false)
        }
        };

    private static Step GetStep(
        string name,
        string method,
        CallStyle callStyle,
        Func<TestContext, CancellationToken, Task<ExecutionResult>> execute,
        bool expectedRegistered) => new()
        {
            Name = name,
            Method = method,
            CallStyle = callStyle,
            ExpectedStatus = callStyle is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync ? 200 : null,
            ExecuteAsync = execute,
            ValidateAsync = (result, _) =>
            {
                if (result.Response is not StripeInfo info)
                    throw new InvalidOperationException("Expected Billing API to return StripeInfo.");
                if (info.IsRegistered != expectedRegistered)
                    throw new InvalidOperationException(
                        $"Expected Stripe registration state {expectedRegistered}, but received {info.IsRegistered}.");
                // A coercible payload such as {"is_registered":"false"} deserialises to false, so
                // the raw envelope has to be checked to reject a malformed response.
                RequireBooleanBody(result.Body, "is_registered", expectedRegistered, name, callStyle);
                return Task.CompletedTask;
            }
        };

    /// <summary>
    /// Asserts that the raw HTTP body is a JSON object whose <paramref name="property"/> is a real
    /// boolean with the expected value. The object call styles expose no body; an HTTP-info call
    /// must supply one, otherwise a lost <c>RawContent</c> would silently skip the check.
    /// </summary>
    private static void RequireBooleanBody(
        string? body, string property, bool expected, string step, CallStyle callStyle)
    {
        var requiresBody = callStyle is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync;
        if (string.IsNullOrWhiteSpace(body))
        {
            if (requiresBody)
                throw new InvalidOperationException($"{step}: the HTTP response carried no raw body.");
            return;
        }
        JToken parsed;
        try { parsed = JToken.Parse(body); }
        catch (Newtonsoft.Json.JsonReaderException error)
        {
            throw new InvalidOperationException($"{step}: response body is not valid JSON.", error);
        }
        if (parsed is not JObject document)
            throw new InvalidOperationException($"{step}: response body is not a JSON object.");
        if (document[property] is not { } value)
            throw new InvalidOperationException($"{step}: response body has no \"{property}\" property.");
        if (value.Type != JTokenType.Boolean)
            throw new InvalidOperationException(
                $"{step}: \"{property}\" is {value.Type}, expected a JSON boolean.");
        if (value.Value<bool>() != expected)
            throw new InvalidOperationException(
                $"{step}: \"{property}\" is {value.Value<bool>()}, expected {expected}.");
    }

    private static Step UpdateStep(
        string name,
        string method,
        CallStyle callStyle,
        Func<TestContext, CancellationToken, Task<ExecutionResult>> execute) => new()
        {
            Name = name,
            Method = method,
            CallStyle = callStyle,
            Parameters = (Func<TestContext, object?>)(context => Parameter(context)),
            ExpectedStatus = callStyle is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync ? 200 : null,
            ExecuteAsync = execute,
            ValidateAsync = (result, _) => { RequireVoidResponse(result, name); return Task.CompletedTask; },
            UpdateStateAsync = (_, context) =>
            {
                context.Variables["billing_state"] = "registered";
                return Task.CompletedTask;
            }
        };

    /// <summary>
    /// UpdateStripeInfo and DeleteStripeInfo are void operations: the generated
    /// <c>ApiResponse&lt;object&gt;</c> must carry no payload and an empty HTTP body.
    /// </summary>
    private static void RequireVoidResponse(ExecutionResult result, string step)
    {
        // Only a null payload is a void response: a scalar such as an empty string carries no
        // JToken children and would otherwise pass unnoticed.
        if (result.Response is not null)
            throw new InvalidOperationException(
                $"{step}: expected a void response but received a payload of " +
                $"{result.Response.GetType().Name}.");
        if (string.IsNullOrWhiteSpace(result.Body)) return;
        var trimmed = result.Body.Trim();
        if (trimmed is not ("{}" or "[]" or "null" or "\"\""))
            throw new InvalidOperationException($"{step}: expected an empty body but received {trimmed}.");
    }

    private static Step DeleteStep(
        string name,
        string method,
        CallStyle callStyle,
        Func<TestContext, CancellationToken, Task<ExecutionResult>> execute) => new()
        {
            Name = name,
            Method = method,
            CallStyle = callStyle,
            ExpectedStatus = callStyle is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync ? 200 : null,
            ExecuteAsync = execute,
            ValidateAsync = (result, _) => { RequireVoidResponse(result, name); return Task.CompletedTask; },
            UpdateStateAsync = (_, context) =>
            {
                context.Variables["billing_state"] = "unregistered";
                return Task.CompletedTask;
            }
        };

    private static IReadOnlyDictionary<string, object?> Variables(string stripeSecretKey) =>
        new Dictionary<string, object?>
        {
            ["stripe_secret_key"] = stripeSecretKey,
            ["billing_state"] = "unknown"
        };

    private static UpdateStripeInfoParam Parameter(TestContext context) =>
        new(context.GetRequired<string>("stripe_secret_key"));

    private static ExecutionResult Result<T>(ApiResponse<T> response) =>
        new(response.Data, (int)response.StatusCode, Headers(response.Headers), Body: response.RawContent);

    private static IReadOnlyDictionary<string, IReadOnlyList<string>>? Headers(
        Multimap<string, string>? headers) =>
        headers?.ToDictionary(
            pair => pair.Key,
            pair => (IReadOnlyList<string>)pair.Value.ToArray(),
            StringComparer.OrdinalIgnoreCase);

    private static async Task CleanupAsync(
        IBillingStripeClient client,
        CancellationToken cancellationToken)
    {
        var current = await client.GetStripeInfoWithHttpInfoAsync(cancellationToken)
            .ConfigureAwait(false);
        if (current.StatusCode != HttpStatusCode.OK)
            throw new InvalidOperationException(
                $"Could not inspect Stripe registration during cleanup: HTTP {(int)current.StatusCode}.");
        if (current.Data?.IsRegistered == true)
            await client.DeleteStripeInfoWithHttpInfoAsync(cancellationToken).ConfigureAwait(false);
    }
}
