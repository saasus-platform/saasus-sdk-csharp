using System.Net;
using integrationapi.Api;
using integrationapi.Client;
using integrationapi.Model;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Integration;

internal interface IIntegrationEventBridgeClient
{
    EventBridgeSettings GetEventBridgeSettings();
    ApiResponse<EventBridgeSettings> GetEventBridgeSettingsWithHttpInfo();
    Task<EventBridgeSettings> GetEventBridgeSettingsAsync(CancellationToken cancellationToken);
    Task<ApiResponse<EventBridgeSettings>> GetEventBridgeSettingsWithHttpInfoAsync(
        CancellationToken cancellationToken);

    void SaveEventBridgeSettings(EventBridgeSettings settings);
    ApiResponse<object> SaveEventBridgeSettingsWithHttpInfo(EventBridgeSettings settings);
    Task SaveEventBridgeSettingsAsync(EventBridgeSettings settings, CancellationToken cancellationToken);
    Task<ApiResponse<object>> SaveEventBridgeSettingsWithHttpInfoAsync(
        EventBridgeSettings settings,
        CancellationToken cancellationToken);

    void DeleteEventBridgeSettings();
    ApiResponse<object> DeleteEventBridgeSettingsWithHttpInfo();
    Task DeleteEventBridgeSettingsAsync(CancellationToken cancellationToken);
    Task<ApiResponse<object>> DeleteEventBridgeSettingsWithHttpInfoAsync(
        CancellationToken cancellationToken);

    void CreateEventBridgeEvent(CreateEventBridgeEventParam parameter);
    ApiResponse<object> CreateEventBridgeEventWithHttpInfo(CreateEventBridgeEventParam parameter);
    Task CreateEventBridgeEventAsync(CreateEventBridgeEventParam parameter, CancellationToken cancellationToken);
    Task<ApiResponse<object>> CreateEventBridgeEventWithHttpInfoAsync(
        CreateEventBridgeEventParam parameter,
        CancellationToken cancellationToken);

    void CreateEventBridgeTestEvent();
    ApiResponse<object> CreateEventBridgeTestEventWithHttpInfo();
    Task CreateEventBridgeTestEventAsync(CancellationToken cancellationToken);
    Task<ApiResponse<object>> CreateEventBridgeTestEventWithHttpInfoAsync(
        CancellationToken cancellationToken);
}

internal sealed class IntegrationEventBridgeClient : IIntegrationEventBridgeClient
{
    private readonly IEventBridgeApi _api;

    public IntegrationEventBridgeClient(IEventBridgeApi api) => _api = api;

    public EventBridgeSettings GetEventBridgeSettings() => _api.GetEventBridgeSettings();
    public ApiResponse<EventBridgeSettings> GetEventBridgeSettingsWithHttpInfo() =>
        _api.GetEventBridgeSettingsWithHttpInfo();
    public Task<EventBridgeSettings> GetEventBridgeSettingsAsync(CancellationToken cancellationToken) =>
        _api.GetEventBridgeSettingsAsync(cancellationToken: cancellationToken);
    public Task<ApiResponse<EventBridgeSettings>> GetEventBridgeSettingsWithHttpInfoAsync(
        CancellationToken cancellationToken) =>
        _api.GetEventBridgeSettingsWithHttpInfoAsync(cancellationToken: cancellationToken);

    public void SaveEventBridgeSettings(EventBridgeSettings settings) => _api.SaveEventBridgeSettings(settings);
    public ApiResponse<object> SaveEventBridgeSettingsWithHttpInfo(EventBridgeSettings settings) =>
        _api.SaveEventBridgeSettingsWithHttpInfo(settings);
    public Task SaveEventBridgeSettingsAsync(
        EventBridgeSettings settings,
        CancellationToken cancellationToken) =>
        _api.SaveEventBridgeSettingsAsync(settings, cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> SaveEventBridgeSettingsWithHttpInfoAsync(
        EventBridgeSettings settings,
        CancellationToken cancellationToken) =>
        _api.SaveEventBridgeSettingsWithHttpInfoAsync(settings, cancellationToken: cancellationToken);

    public void DeleteEventBridgeSettings() => _api.DeleteEventBridgeSettings();
    public ApiResponse<object> DeleteEventBridgeSettingsWithHttpInfo() =>
        _api.DeleteEventBridgeSettingsWithHttpInfo();
    public Task DeleteEventBridgeSettingsAsync(CancellationToken cancellationToken) =>
        _api.DeleteEventBridgeSettingsAsync(cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> DeleteEventBridgeSettingsWithHttpInfoAsync(
        CancellationToken cancellationToken) =>
        _api.DeleteEventBridgeSettingsWithHttpInfoAsync(cancellationToken: cancellationToken);

    public void CreateEventBridgeEvent(CreateEventBridgeEventParam parameter) =>
        _api.CreateEventBridgeEvent(parameter);
    public ApiResponse<object> CreateEventBridgeEventWithHttpInfo(CreateEventBridgeEventParam parameter) =>
        _api.CreateEventBridgeEventWithHttpInfo(parameter);
    public Task CreateEventBridgeEventAsync(
        CreateEventBridgeEventParam parameter,
        CancellationToken cancellationToken) =>
        _api.CreateEventBridgeEventAsync(parameter, cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> CreateEventBridgeEventWithHttpInfoAsync(
        CreateEventBridgeEventParam parameter,
        CancellationToken cancellationToken) =>
        _api.CreateEventBridgeEventWithHttpInfoAsync(parameter, cancellationToken: cancellationToken);

    public void CreateEventBridgeTestEvent() => _api.CreateEventBridgeTestEvent();
    public ApiResponse<object> CreateEventBridgeTestEventWithHttpInfo() =>
        _api.CreateEventBridgeTestEventWithHttpInfo();
    public Task CreateEventBridgeTestEventAsync(CancellationToken cancellationToken) =>
        _api.CreateEventBridgeTestEventAsync(cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> CreateEventBridgeTestEventWithHttpInfoAsync(
        CancellationToken cancellationToken) =>
        _api.CreateEventBridgeTestEventWithHttpInfoAsync(cancellationToken: cancellationToken);
}

internal static class IntegrationStoryFactory
{
    internal static readonly IReadOnlyList<(string Method, CallStyle CallStyle)> Methods = new[]
    {
        ("GetEventBridgeSettings", CallStyle.Sync),
        ("SaveEventBridgeSettings", CallStyle.Sync),
        ("DeleteEventBridgeSettings", CallStyle.Sync),
        ("CreateEventBridgeEvent", CallStyle.Sync),
        ("CreateEventBridgeTestEvent", CallStyle.Sync),
        ("GetEventBridgeSettingsWithHttpInfo", CallStyle.WithHttpInfo),
        ("SaveEventBridgeSettingsWithHttpInfo", CallStyle.WithHttpInfo),
        ("DeleteEventBridgeSettingsWithHttpInfo", CallStyle.WithHttpInfo),
        ("CreateEventBridgeEventWithHttpInfo", CallStyle.WithHttpInfo),
        ("CreateEventBridgeTestEventWithHttpInfo", CallStyle.WithHttpInfo),
        ("GetEventBridgeSettingsAsync", CallStyle.Async),
        ("SaveEventBridgeSettingsAsync", CallStyle.Async),
        ("DeleteEventBridgeSettingsAsync", CallStyle.Async),
        ("CreateEventBridgeEventAsync", CallStyle.Async),
        ("CreateEventBridgeTestEventAsync", CallStyle.Async),
        ("GetEventBridgeSettingsWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("SaveEventBridgeSettingsWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("DeleteEventBridgeSettingsWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("CreateEventBridgeEventWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("CreateEventBridgeTestEventWithHttpInfoAsync", CallStyle.WithHttpInfoAsync)
    };

    public static IReadOnlyList<Story> Create(IIntegrationEventBridgeClient client) => new[]
    {
        CreateLifecycleStory(client, CallStyle.Sync),
        CreateLifecycleStory(client, CallStyle.WithHttpInfo),
        CreateLifecycleStory(client, CallStyle.Async),
        CreateLifecycleStory(client, CallStyle.WithHttpInfoAsync)
    };

    internal static string GetTestAwsAccountId()
    {
        var accountId = Environment.GetEnvironmentVariable("TEST_AWS_ACCOUNT_ID");
        return string.IsNullOrWhiteSpace(accountId) ? "267185063265" : accountId;
    }

    internal static string GetTestAwsRegion()
    {
        var region = Environment.GetEnvironmentVariable("TEST_AWS_REGION");
        return string.IsNullOrWhiteSpace(region) ? "ap-northeast-1" : region;
    }

    private static Story CreateLifecycleStory(
        IIntegrationEventBridgeClient client,
        CallStyle callStyle)
    {
        var state = new StoryState();
        var responseDescription = callStyle switch
        {
            CallStyle.Sync => "synchronous responses",
            CallStyle.WithHttpInfo => "synchronous HTTP responses",
            CallStyle.Async => "asynchronous responses",
            CallStyle.WithHttpInfoAsync => "asynchronous HTTP responses",
            _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
        };

        return new Story
        {
            Name = $"Integration API - {responseDescription}",
            Description = "Exercises the EventBridge settings and event delivery flow from the Go and PHP E2E tests.",
            Module = "integration",
            InitialVariables = Variables(),
            SetupAsync = (_, cancellationToken) => PrepareAsync(client, state, cancellationToken),
            CleanupAsync = (_, cancellationToken) => CleanupAsync(client, state, cancellationToken),
            Steps = BuildSteps(client, callStyle)
        };
    }

    private static IReadOnlyList<Step> BuildSteps(
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => new[]
    {
        GetSettingsStep("Pre_GetEventBridgeSettings", client, callStyle, false),
        SaveSettingsStep("SaveEventBridgeSettings", client, callStyle),
        GetSettingsStep("GetEventBridgeSettings_AfterSave", client, callStyle, true),
        DeleteSettingsStep("DeleteEventBridgeSettings", client, callStyle),
        GetSettingsStep("GetEventBridgeSettings_AfterDelete", client, callStyle, false),
        GetSettingsStep("GetEventBridgeSettings_Setup", client, callStyle, false),
        SaveSettingsStep("SaveEventBridgeSettings_ForTest", client, callStyle),
        CreateTestEventStep(client, callStyle),
        CreateEventStep(client, callStyle),
        DeleteSettingsStep("DeleteEventBridgeSettings_Cleanup", client, callStyle)
    };

    private static Step GetSettingsStep(
        string name,
        IIntegrationEventBridgeClient client,
        CallStyle callStyle,
        bool expectedConfigured) => new()
        {
            Name = name,
            Method = MethodName("GetEventBridgeSettings", callStyle),
            CallStyle = callStyle,
            ExpectedStatus = IsHttpInfo(callStyle) ? 200 : null,
            ExecuteAsync = GetSettingsExecutor(client, callStyle),
            ValidateAsync = (result, context) => ValidateSettingsAsync(result, context, expectedConfigured)
        };

    private static Step SaveSettingsStep(
        string name,
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => new()
        {
            Name = name,
            Method = MethodName("SaveEventBridgeSettings", callStyle),
            CallStyle = callStyle,
            Parameters = (Func<TestContext, object?>)(Parameter),
            ExpectedStatus = IsHttpInfo(callStyle) ? 200 : null,
            ExecuteAsync = SaveSettingsExecutor(client, callStyle),
            ValidateAsync = ValidateEmptyResponseAsync,
            UpdateStateAsync = (_, context) => SetStateAsync(context, "configured")
        };

    private static Step DeleteSettingsStep(
        string name,
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => new()
        {
            Name = name,
            Method = MethodName("DeleteEventBridgeSettings", callStyle),
            CallStyle = callStyle,
            ExpectedStatus = IsHttpInfo(callStyle) ? 200 : null,
            ExecuteAsync = DeleteSettingsExecutor(client, callStyle),
            ValidateAsync = ValidateEmptyResponseAsync,
            UpdateStateAsync = (_, context) => SetStateAsync(context, "unconfigured")
        };

    private static Step CreateTestEventStep(
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => new()
        {
            Name = "CreateEventBridgeTestEvent",
            Method = MethodName("CreateEventBridgeTestEvent", callStyle),
            CallStyle = callStyle,
            ExpectedStatus = IsHttpInfo(callStyle) ? 201 : null,
            ExecuteAsync = CreateTestEventExecutor(client, callStyle),
            ValidateAsync = ValidateEmptyResponseAsync
        };

    private static Step CreateEventStep(
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => new()
        {
            Name = "CreateEventBridgeEvent",
            Method = MethodName("CreateEventBridgeEvent", callStyle),
            CallStyle = callStyle,
            // The endpoint is intentionally still reported as not implemented by the API.
            // Expected errors are treated as successful coverage by E2EEngine.
            ExpectedStatus = 501,
            Parameters = CreateEventParameter(),
            ExecuteAsync = CreateEventExecutor(client, callStyle)
        };

    private static Func<TestContext, CancellationToken, Task<ExecutionResult>> GetSettingsExecutor(
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => callStyle switch
        {
            CallStyle.Sync => MethodExecutor.Sync(_ => client.GetEventBridgeSettings()),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(
                _ => ToExecutionResult(client.GetEventBridgeSettingsWithHttpInfo())),
            CallStyle.Async => MethodExecutor.Async(
                async (_, cancellationToken) => await client.GetEventBridgeSettingsAsync(cancellationToken)),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(
                async (_, cancellationToken) => ToExecutionResult(
                    await client.GetEventBridgeSettingsWithHttpInfoAsync(cancellationToken))),
            _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
        };

    private static Func<TestContext, CancellationToken, Task<ExecutionResult>> SaveSettingsExecutor(
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => callStyle switch
        {
            CallStyle.Sync => MethodExecutor.Sync(context => client.SaveEventBridgeSettings(Parameter(context))),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(
                context => ToExecutionResult(client.SaveEventBridgeSettingsWithHttpInfo(Parameter(context)))),
            CallStyle.Async => MethodExecutor.Async(
                (context, cancellationToken) =>
                    client.SaveEventBridgeSettingsAsync(Parameter(context), cancellationToken)),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(
                async (context, cancellationToken) => ToExecutionResult(
                    await client.SaveEventBridgeSettingsWithHttpInfoAsync(
                        Parameter(context), cancellationToken))),
            _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
        };

    private static Func<TestContext, CancellationToken, Task<ExecutionResult>> DeleteSettingsExecutor(
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => callStyle switch
        {
            CallStyle.Sync => MethodExecutor.Sync(_ => client.DeleteEventBridgeSettings()),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(
                _ => ToExecutionResult(client.DeleteEventBridgeSettingsWithHttpInfo())),
            CallStyle.Async => MethodExecutor.Async(
                (_, cancellationToken) => client.DeleteEventBridgeSettingsAsync(cancellationToken)),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(
                async (_, cancellationToken) => ToExecutionResult(
                    await client.DeleteEventBridgeSettingsWithHttpInfoAsync(cancellationToken))),
            _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
        };

    private static Func<TestContext, CancellationToken, Task<ExecutionResult>> CreateTestEventExecutor(
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => callStyle switch
        {
            CallStyle.Sync => MethodExecutor.Sync(_ => client.CreateEventBridgeTestEvent()),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(
                _ => ToExecutionResult(client.CreateEventBridgeTestEventWithHttpInfo())),
            CallStyle.Async => MethodExecutor.Async(
                (_, cancellationToken) => client.CreateEventBridgeTestEventAsync(cancellationToken)),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(
                async (_, cancellationToken) => ToExecutionResult(
                    await client.CreateEventBridgeTestEventWithHttpInfoAsync(cancellationToken))),
            _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
        };

    private static Func<TestContext, CancellationToken, Task<ExecutionResult>> CreateEventExecutor(
        IIntegrationEventBridgeClient client,
        CallStyle callStyle) => callStyle switch
        {
            CallStyle.Sync => MethodExecutor.Sync(
                _ => client.CreateEventBridgeEvent(CreateEventParameter())),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(
                _ => ToExecutionResult(client.CreateEventBridgeEventWithHttpInfo(CreateEventParameter()))),
            CallStyle.Async => MethodExecutor.Async(
                (_, cancellationToken) =>
                    client.CreateEventBridgeEventAsync(CreateEventParameter(), cancellationToken)),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(
                async (_, cancellationToken) => ToExecutionResult(
                    await client.CreateEventBridgeEventWithHttpInfoAsync(
                        CreateEventParameter(), cancellationToken))),
            _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
        };

    private static async Task ValidateSettingsAsync(
        ExecutionResult result,
        TestContext context,
        bool expectedConfigured)
    {
        var settings = result.Response as EventBridgeSettings;
        if (!expectedConfigured)
        {
            if (HasConfiguredFields(settings))
                throw new InvalidOperationException("Expected EventBridge settings to be empty.");
            return;
        }

        if (settings is null || !HasCompleteSettings(settings))
            throw new InvalidOperationException("Expected EventBridge settings to be configured.");

        var accountId = context.GetRequired<string>("aws_account_id");
        var region = ParseAwsRegion(context.GetRequired<string>("aws_region"));
        if (!string.Equals(settings.AwsAccountId, accountId, StringComparison.Ordinal) ||
            settings.AwsRegion != region)
        {
            throw new InvalidOperationException(
                $"EventBridge settings do not match the saved values: " +
                $"expected {accountId}/{region}, received {settings.AwsAccountId}/{settings.AwsRegion}.");
        }

        await Task.CompletedTask;
    }

    private static Task ValidateEmptyResponseAsync(ExecutionResult result, TestContext _)
    {
        if (result.Response is not null)
        {
            throw new InvalidOperationException(
                $"Expected an empty Integration API response, but received {result.Response.GetType().Name}.");
        }

        return Task.CompletedTask;
    }

    private static async Task PrepareAsync(
        IIntegrationEventBridgeClient client,
        StoryState state,
        CancellationToken cancellationToken)
    {
        var current = await client.GetEventBridgeSettingsWithHttpInfoAsync(cancellationToken)
            .ConfigureAwait(false);
        EnsureSuccessful(current.StatusCode, "inspect existing EventBridge settings");

        // A configured but incomplete record cannot be reproduced through SaveEventBridgeSettings,
        // which requires both an account ID and a valid region. The story therefore refuses to run
        // instead of overwriting it and leaving the shared environment without its own settings.
        if (HasConfiguredFields(current.Data) && !HasCompleteSettings(current.Data))
            throw new InvalidOperationException(
                "Existing EventBridge settings are incomplete and could not be restored afterwards. " +
                "Complete or remove them before running the Integration E2E suite.");

        state.Captured = true;

        if (!HasCompleteSettings(current.Data)) return;

        state.Previous = CopySettings(current.Data);
        var deleted = await client.DeleteEventBridgeSettingsWithHttpInfoAsync(cancellationToken)
            .ConfigureAwait(false);
        EnsureSuccessful(deleted.StatusCode, "remove existing EventBridge settings");
    }

    private static async Task CleanupAsync(
        IIntegrationEventBridgeClient client,
        StoryState state,
        CancellationToken cancellationToken)
    {
        if (!state.Captured) return;

        // Restoration must be attempted even when the delete fails, otherwise a story that
        // replaced existing settings would leave the shared environment modified.
        Exception? deleteError = null;
        try
        {
            var current = await client.GetEventBridgeSettingsWithHttpInfoAsync(cancellationToken)
                .ConfigureAwait(false);
            EnsureSuccessful(current.StatusCode, "inspect EventBridge settings during cleanup");
            if (HasConfiguredFields(current.Data))
            {
                var deleted = await client.DeleteEventBridgeSettingsWithHttpInfoAsync(cancellationToken)
                    .ConfigureAwait(false);
                EnsureSuccessful(deleted.StatusCode, "remove EventBridge settings during cleanup");
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception error)
        {
            deleteError = error;
        }

        if (state.Previous is not null)
        {
            var restored = await client.SaveEventBridgeSettingsWithHttpInfoAsync(
                    state.Previous,
                    cancellationToken)
                .ConfigureAwait(false);
            EnsureSuccessful(restored.StatusCode, "restore existing EventBridge settings");
        }

        if (deleteError is not null) throw deleteError;
    }

    private static void EnsureSuccessful(HttpStatusCode statusCode, string operation)
    {
        var status = (int)statusCode;
        if (status is < 200 or >= 300)
            throw new InvalidOperationException($"Could not {operation}: HTTP {status}.");
    }

    private static Task SetStateAsync(TestContext context, string value)
    {
        context.Variables["integration_state"] = value;
        return Task.CompletedTask;
    }

    private static IReadOnlyDictionary<string, object?> Variables() =>
        new Dictionary<string, object?>
        {
            ["aws_account_id"] = GetTestAwsAccountId(),
            ["aws_region"] = GetTestAwsRegion(),
            ["integration_state"] = "unknown"
        };

    private static EventBridgeSettings Parameter(TestContext context) => new(
        context.GetRequired<string>("aws_account_id"),
        ParseAwsRegion(context.GetRequired<string>("aws_region")));

    private static CreateEventBridgeEventParam CreateEventParameter() =>
        new(new List<EventMessage>
        {
            new(
                "api_call",
                "create_user",
                "{id:8b79528a-ec3b-4f68-b7c4-d793e3894561,name:test222}")
        });

    private static string MethodName(string baseName, CallStyle callStyle) => callStyle switch
    {
        CallStyle.Sync => baseName,
        CallStyle.WithHttpInfo => $"{baseName}WithHttpInfo",
        CallStyle.Async => $"{baseName}Async",
        CallStyle.WithHttpInfoAsync => $"{baseName}WithHttpInfoAsync",
        _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
    };

    private static bool IsHttpInfo(CallStyle callStyle) =>
        callStyle is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync;

    private static bool HasConfiguredFields(EventBridgeSettings? settings) =>
        settings is not null &&
        (!string.IsNullOrWhiteSpace(settings.AwsAccountId) ||
         Enum.IsDefined(typeof(AwsRegion), settings.AwsRegion));

    private static bool HasCompleteSettings(EventBridgeSettings? settings) =>
        settings is not null &&
        !string.IsNullOrWhiteSpace(settings.AwsAccountId) &&
        Enum.IsDefined(typeof(AwsRegion), settings.AwsRegion);

    private static EventBridgeSettings CopySettings(EventBridgeSettings settings) =>
        new(settings.AwsAccountId, settings.AwsRegion);

    private static AwsRegion ParseAwsRegion(string value)
    {
        var normalized = NormalizeRegion(value);
        foreach (AwsRegion region in Enum.GetValues(typeof(AwsRegion)))
        {
            if (NormalizeRegion(region.ToString()) == normalized)
                return region;
        }

        throw new InvalidOperationException(
            $"Unsupported TEST_AWS_REGION '{value}'. Use an AWS region such as ap-northeast-1.");
    }

    private static string NormalizeRegion(string value) => new string(
        value.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();

    private static ExecutionResult ToExecutionResult<T>(ApiResponse<T> response) =>
        new(
            response.Data,
            (int)response.StatusCode,
            Headers(response.Headers),
            Body: response.RawContent);

    private static IReadOnlyDictionary<string, IReadOnlyList<string>>? Headers(
        Multimap<string, string>? headers) =>
        headers?.ToDictionary(
            pair => pair.Key,
            pair => (IReadOnlyList<string>)pair.Value.ToArray(),
            StringComparer.OrdinalIgnoreCase);

    private sealed class StoryState
    {
        public bool Captured { get; set; }
        public EventBridgeSettings? Previous { get; set; }
    }
}
