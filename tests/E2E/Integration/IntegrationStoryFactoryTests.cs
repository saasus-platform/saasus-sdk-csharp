using System.Net;
using integrationapi.Client;
using integrationapi.Model;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Integration;

public sealed class IntegrationStoryFactoryTests
{
    [Fact]
    public async Task ExecutesEveryIntegrationMethodAndLeavesSettingsUnconfigured()
    {
        var client = new FakeIntegrationEventBridgeClient();
        var stories = IntegrationStoryFactory.Create(client);
        var coverage = CreateCoverage();

        var results = await new E2EEngine(new Config
        {
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }, coverage).ExecuteAsync(stories);

        Assert.Equal(4, results.Count);
        Assert.Equal(40, results.Sum(result => result.Steps.Count));
        Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
        Assert.Empty(coverage.Untested);
        Assert.Equal(20, coverage.Entries.Count);
        Assert.Null(client.Settings);
    }

    [Fact]
    public async Task RestoresExistingSettingsAfterEveryStory()
    {
        var original = new EventBridgeSettings("111122223333", AwsRegion.ApNortheast1);
        var client = new FakeIntegrationEventBridgeClient(original);
        var stories = IntegrationStoryFactory.Create(client);
        var engine = new E2EEngine(new Config
        {
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }, CreateCoverage());

        foreach (var story in stories)
        {
            var result = await engine.ExecuteStoryAsync(story);

            Assert.Equal(TestStatus.Passed, result.Status);
            Assert.NotNull(client.Settings);
            Assert.Equal(original.AwsAccountId, client.Settings!.AwsAccountId);
            Assert.Equal(original.AwsRegion, client.Settings.AwsRegion);
        }
    }

    [Fact]
    public async Task LeavesIncompleteExistingSettingsUntouched()
    {
        // Only the account ID is configured. Such a record cannot be restored through the Save
        // endpoint, so the story must refuse to run instead of deleting it.
        var partial = new EventBridgeSettings("111122223333", (AwsRegion)(-1));
        var client = new FakeIntegrationEventBridgeClient(partial);

        var result = await new E2EEngine(new Config
        {
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }, CreateCoverage()).ExecuteStoryAsync(IntegrationStoryFactory.Create(client).First());

        Assert.Equal(TestStatus.Failed, result.Status);
        Assert.NotNull(result.SetupError);
        Assert.NotNull(client.Settings);
        Assert.Equal(partial.AwsAccountId, client.Settings!.AwsAccountId);
        Assert.Equal(partial.AwsRegion, client.Settings.AwsRegion);
    }

    [Theory]
    [InlineData("SaveEventBridgeSettings", "SaveEventBridgeSettings")]
    [InlineData("DeleteEventBridgeSettings", "DeleteEventBridgeSettings")]
    [InlineData("CreateEventBridgeTestEvent", "CreateEventBridgeTestEvent")]
    public async Task RejectsNonEmptyVoidResponsesForHttpInfoStories(
        string operation,
        string expectedFailedStep)
    {
        var client = new FakeIntegrationEventBridgeClient(operationWithNonEmptyResponse: operation);
        var stories = IntegrationStoryFactory.Create(client);

        var results = await new E2EEngine(new Config
        {
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }, CreateCoverage()).ExecuteAsync(stories);

        var failedStories = results.Where(result => result.Status == TestStatus.Failed).ToArray();
        Assert.Equal(2, failedStories.Length);
        Assert.All(failedStories, result =>
            Assert.Equal(TestStatus.Failed,
                result.Steps.Single(step => step.Name == expectedFailedStep).Status));
    }

    [Fact]
    public void DeclaresExactlyTheGeneratedEventBridgeMethods()
    {
        var generatedMethods = typeof(integrationapi.Api.IEventBridgeApi)
            .GetInterfaces()
            .SelectMany(type => type.GetMethods())
            .Select(method => method.Name)
            .Where(name => name.Contains("EventBridge", StringComparison.Ordinal))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        var declaredMethods = IntegrationStoryFactory.Methods
            .Select(method => method.Method)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(generatedMethods, declaredMethods);
    }

    private sealed class FakeIntegrationEventBridgeClient : IIntegrationEventBridgeClient
    {
        public FakeIntegrationEventBridgeClient(
            EventBridgeSettings? initialSettings = null,
            string? operationWithNonEmptyResponse = null)
        {
            Settings = Copy(initialSettings);
            OperationWithNonEmptyResponse = operationWithNonEmptyResponse;
        }

        public EventBridgeSettings? Settings { get; private set; }
        private string? OperationWithNonEmptyResponse { get; }

        public EventBridgeSettings GetEventBridgeSettings() => Copy(Settings);
        public ApiResponse<EventBridgeSettings> GetEventBridgeSettingsWithHttpInfo() =>
            Response(Copy(Settings));
        public Task<EventBridgeSettings> GetEventBridgeSettingsAsync(CancellationToken cancellationToken) =>
            Task.FromResult(Copy(Settings));
        public Task<ApiResponse<EventBridgeSettings>> GetEventBridgeSettingsWithHttpInfoAsync(
            CancellationToken cancellationToken) =>
            Task.FromResult(GetEventBridgeSettingsWithHttpInfo());

        public void SaveEventBridgeSettings(EventBridgeSettings settings) => Save(settings);
        public ApiResponse<object> SaveEventBridgeSettingsWithHttpInfo(EventBridgeSettings settings)
        {
            Save(settings);
            return VoidResponse(nameof(SaveEventBridgeSettings));
        }
        public Task SaveEventBridgeSettingsAsync(
            EventBridgeSettings settings,
            CancellationToken cancellationToken)
        {
            Save(settings);
            return Task.CompletedTask;
        }
        public Task<ApiResponse<object>> SaveEventBridgeSettingsWithHttpInfoAsync(
            EventBridgeSettings settings,
            CancellationToken cancellationToken) =>
            Task.FromResult(SaveEventBridgeSettingsWithHttpInfo(settings));

        public void DeleteEventBridgeSettings() => Settings = null;
        public ApiResponse<object> DeleteEventBridgeSettingsWithHttpInfo()
        {
            DeleteEventBridgeSettings();
            return VoidResponse(nameof(DeleteEventBridgeSettings));
        }
        public Task DeleteEventBridgeSettingsAsync(CancellationToken cancellationToken)
        {
            DeleteEventBridgeSettings();
            return Task.CompletedTask;
        }
        public Task<ApiResponse<object>> DeleteEventBridgeSettingsWithHttpInfoAsync(
            CancellationToken cancellationToken) =>
            Task.FromResult(DeleteEventBridgeSettingsWithHttpInfo());

        public void CreateEventBridgeEvent(CreateEventBridgeEventParam parameter) => ThrowNotImplemented();
        public ApiResponse<object> CreateEventBridgeEventWithHttpInfo(CreateEventBridgeEventParam parameter) =>
            throw NotImplementedException();
        public Task CreateEventBridgeEventAsync(
            CreateEventBridgeEventParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromException(new ApiException(501, "EventBridge event endpoint is not implemented."));
        public Task<ApiResponse<object>> CreateEventBridgeEventWithHttpInfoAsync(
            CreateEventBridgeEventParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromException<ApiResponse<object>>(NotImplementedException());

        public void CreateEventBridgeTestEvent() { }
        public ApiResponse<object> CreateEventBridgeTestEventWithHttpInfo() =>
            VoidResponse(nameof(CreateEventBridgeTestEvent), HttpStatusCode.Created);
        public Task CreateEventBridgeTestEventAsync(CancellationToken cancellationToken) =>
            Task.CompletedTask;
        public Task<ApiResponse<object>> CreateEventBridgeTestEventWithHttpInfoAsync(
            CancellationToken cancellationToken) =>
            Task.FromResult(CreateEventBridgeTestEventWithHttpInfo());

        private void Save(EventBridgeSettings settings)
        {
            Assert.False(string.IsNullOrWhiteSpace(settings.AwsAccountId));
            Assert.True(Enum.IsDefined(typeof(AwsRegion), settings.AwsRegion));
            Settings = Copy(settings);
        }

        private static void ThrowNotImplemented() => throw NotImplementedException();

        private ApiResponse<object> VoidResponse(
            string operation,
            HttpStatusCode statusCode = HttpStatusCode.OK) =>
            Response<object>(OperationWithNonEmptyResponse == operation ? new object() : null!, statusCode);

        private static ApiException NotImplementedException() =>
            new(501, "EventBridge event endpoint is not implemented.");

        private static EventBridgeSettings Copy(EventBridgeSettings? settings) =>
            settings is null ? null! : new EventBridgeSettings(settings.AwsAccountId, settings.AwsRegion);

        private static ApiResponse<T> Response<T>(
            T data,
            HttpStatusCode statusCode = HttpStatusCode.OK) =>
            new(statusCode, new Multimap<string, string>(), data, "{}");
    }

    private static CoverageTracker CreateCoverage()
    {
        var coverage = new CoverageTracker();
        foreach (var method in IntegrationStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);
        return coverage;
    }
}
