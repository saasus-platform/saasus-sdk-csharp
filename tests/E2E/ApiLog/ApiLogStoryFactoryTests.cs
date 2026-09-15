using System.Net;
using Newtonsoft.Json.Linq;
using apilogapi.Api;
using apilogapi.Client;
using apilogapi.Model;
using SaasusSdk.Tests.TestLib;
using ApiLogModel = apilogapi.Model.ApiLog;

namespace SaasusSdk.Tests.E2E.ApiLog;

public sealed class ApiLogStoryFactoryTests
{
    [Fact]
    public async Task ExecutesEveryApiLogMethodInEveryCallStyleOffline()
    {
        var client = new FakeApiLogClient();
        var stories = ApiLogStoryFactory.Create(client);
        var coverage = new CoverageTracker();
        foreach (var method in ApiLogStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);

        var results = await new E2EEngine(OfflineConfig(), coverage).ExecuteAsync(stories);

        Assert.Equal(4, results.Count);
        Assert.Equal(16, results.Sum(result => result.Steps.Count));
        Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
        Assert.Empty(coverage.Untested);
        Assert.Equal(8, coverage.Entries.Count);
        Assert.Equal(4, client.LogIds.Count);
        Assert.All(client.LogIds, id => Assert.Equal("api-log-1", id));
        Assert.Equal(4, client.Queries.Count(query => query.Cursor == "cursor-1"));
    }

    [Fact]
    public void DeclaresExactlyTheGeneratedApiLogMethods()
    {
        var generatedMethods = typeof(IApiLogApi)
            .GetInterfaces()
            .SelectMany(type => type.GetMethods())
            .Select(method => method.Name)
            .Where(name => name != "GetBasePath" &&
                           !name.StartsWith("get_", StringComparison.Ordinal) &&
                           !name.StartsWith("set_", StringComparison.Ordinal))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        var declaredMethods = ApiLogStoryFactory.Methods
            .Select(method => method.Method)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(generatedMethods, declaredMethods);
    }

    [Fact]
    public void BuildsQueryParametersFromTheFirstLog()
    {
        var client = new FakeApiLogClient();
        var story = ApiLogStoryFactory.Create(client)
            .Single(value => value.Name == "ApiLog API - synchronous responses");
        var context = new TestContext(new Dictionary<string, object?>(story.InitialVariables));

        var stateStep = story.Steps.Single(step => step.Name == "GetApiLogs");
        var queryStep = story.Steps.Single(step => step.Name == "GetApiLogs_WithQueryParameters");
        var stateParameters = Assert.IsType<Func<TestContext, object?>>(stateStep.Parameters);
        var queryParameters = Assert.IsType<Func<TestContext, object?>>(queryStep.Parameters);

        var stateQuery = Assert.IsType<Dictionary<string, object?>>(stateParameters(context));
        Assert.Empty(stateQuery);

        context.Variables["api_log_id"] = "api-log-1";
        context.Variables["created_date"] = "2024-01-01";
        context.Variables["created_at"] = "2024-01-01T00:00:00Z";
        context.Variables["cursor"] = "cursor-1";
        var query = Assert.IsType<Dictionary<string, object?>>(queryParameters(context));
        Assert.Equal("2024-01-01", query["created_date"]);
        Assert.Equal("2024-01-01T00:00:00.0000000Z", query["created_at"]);
        Assert.Null(query["limit"]);
        Assert.Equal("cursor-1", query["cursor"]);

        context.Variables["limit"] = 25L;
        query = Assert.IsType<Dictionary<string, object?>>(queryParameters(context));
        Assert.Equal(25L, query["limit"]);
    }

    [Theory]
    [InlineData(CallStyle.Sync)]
    [InlineData(CallStyle.WithHttpInfo)]
    [InlineData(CallStyle.Async)]
    [InlineData(CallStyle.WithHttpInfoAsync)]
    public async Task RetriesEmptyLogResponsesForEveryCallStyle(CallStyle callStyle)
    {
        var client = new FakeApiLogClient(emptyResponsesBeforeSuccess: 2);
        var story = ApiLogStoryFactory.Create(client)
            .Single(value => value.Steps.All(step => step.CallStyle == callStyle));

        var results = await new E2EEngine(OfflineConfig()).ExecuteAsync(new[] { story });

        var result = Assert.Single(results);
        Assert.Equal(TestStatus.Passed, result.Status);
        Assert.Equal(4, client.Queries.Count);
        Assert.Single(client.LogIds);
    }

    [Fact]
    public async Task FailsWhenLogsRemainEmptyAfterRetries()
    {
        var client = new FakeApiLogClient(emptyResponsesBeforeSuccess: int.MaxValue);
        var story = ApiLogStoryFactory.Create(client)
            .Single(value => value.Steps.All(step => step.CallStyle == CallStyle.Sync));

        var results = await new E2EEngine(OfflineConfig()).ExecuteAsync(new[] { story });

        var result = Assert.Single(results);
        Assert.Equal(TestStatus.Failed, result.Status);
        Assert.Equal(7, client.Queries.Count);
        Assert.Empty(client.LogIds);
    }

    [Fact]
    public async Task SnapshotCaptureUsesABoundedDeterministicLogQuery()
    {
        var client = new FakeApiLogClient(emptyResponsesBeforeSuccess: 1);
        var stories = ApiLogStoryFactory.Create(client, ApiLogSnapshotTests.SnapshotLogLimit);
        var story = stories.Single(value => value.Steps.All(step => step.CallStyle == CallStyle.Sync));

        var results = await new E2EEngine(OfflineConfig()).ExecuteAsync(new[] { story });
        var snapshot = SnapshotFactory.Create(story, results.Single(), new SnapshotConfig());

        Assert.Equal(TestStatus.Passed, results.Single().Status);
        // Every list call is bounded, including the two that send no other query parameter,
        // and the single empty response is retried instead of being snapshotted.
        Assert.All(client.Queries, query => Assert.Equal(ApiLogSnapshotTests.SnapshotLogLimit, query.Limit));
        Assert.Equal(4, client.Queries.Count);
        Assert.Equal(1L, Parameters(snapshot, "Pre_GetApiLogs")["limit"]!.Value<long>());
        Assert.Equal(1L, Parameters(snapshot, "GetApiLogs_WithQueryParameters")["limit"]!.Value<long>());

        // Volatile log metadata is normalised while the field set stays comparable.
        var log = (JObject)snapshot.Steps
            .Single(step => step.StepName == "GetApiLog").ReturnValue!.JsonData!;
        Assert.Equal(SnapshotMasker.DynamicToken, log["request_uri"]!.Value<string>());
        Assert.Equal(SnapshotMasker.DynamicToken, log["remote_address"]!.Value<string>());
    }

    [Fact]
    public async Task BoundedLogQueriesFailWhenThePageStaysEmpty()
    {
        var client = new FakeApiLogClient(emptyResponsesBeforeSuccess: int.MaxValue);
        var story = ApiLogStoryFactory.Create(client, ApiLogSnapshotTests.SnapshotLogLimit)
            .Single(value => value.Steps.All(step => step.CallStyle == CallStyle.Sync));

        var results = await new E2EEngine(OfflineConfig()).ExecuteAsync(new[] { story });

        var result = Assert.Single(results);
        Assert.Equal(TestStatus.Failed, result.Status);
        // The step that seeds the story state must not accept an empty page, while the
        // cursor-filtered page may legitimately be terminal.
        var failure = result.Steps.Single(step => step.Status == TestStatus.Failed);
        Assert.Equal("GetApiLogs", failure.Name);
        Assert.Contains("returned no logs after", failure.Error!.Message, StringComparison.Ordinal);
        Assert.Empty(client.LogIds);
    }

    private static JObject Parameters(StorySnapshot snapshot, string stepName) =>
        (JObject)snapshot.Steps.Single(step => step.StepName == stepName).Parameters;

    private static Config OfflineConfig() => new()
    {
        Timeout = TimeSpan.FromSeconds(2),
        MaxRetries = 0,
        LogLevel = LogLevel.None
    };

    private sealed class FakeApiLogClient : IApiLogClient
    {
        private readonly ApiLogs _logs;
        private readonly ApiLogModel _log;
        private int _emptyResponsesRemaining;

        public FakeApiLogClient(int emptyResponsesBeforeSuccess = 0)
        {
            _emptyResponsesRemaining = emptyResponsesBeforeSuccess;
            _log = new ApiLogModel(
                traceId: "trace-1",
                apiLogId: "api-log-1",
                createdAt: 1_704_067_200,
                createdDate: "2024-01-01",
                ttl: 1_735_689_600,
                requestMethod: "GET",
                saasId: "saas-id",
                apiKey: "api-key",
                responseStatus: "200",
                requestUri: "/v1/apilog/logs/2f47c6a1-55a6-40e5-bb85-2a705caa9fdd",
                remoteAddress: "10.88.2.209:46278",
                referer: string.Empty,
                requestBody: string.Empty,
                responseBody: "{}");
            _logs = new ApiLogs(new List<ApiLogModel> { _log }, "cursor-1");
        }

        public List<(DateTime? CreatedDate, DateTime? CreatedAt, long? Limit, string? Cursor)> Queries { get; } = new();
        public List<string> LogIds { get; } = new();

        public ApiLogs GetLogs(
            DateTime? createdDate = null,
            DateTime? createdAt = null,
            long? limit = null,
            string? cursor = null)
        {
            Queries.Add((createdDate, createdAt, limit, cursor));
            if (_emptyResponsesRemaining > 0)
            {
                _emptyResponsesRemaining--;
                return new ApiLogs(new List<ApiLogModel>(), string.Empty);
            }
            return _logs;
        }

        public ApiResponse<ApiLogs> GetLogsWithHttpInfo(
            DateTime? createdDate = null,
            DateTime? createdAt = null,
            long? limit = null,
            string? cursor = null) =>
            Response(GetLogs(createdDate, createdAt, limit, cursor));

        public Task<ApiLogs> GetLogsAsync(
            DateTime? createdDate = null,
            DateTime? createdAt = null,
            long? limit = null,
            string? cursor = null,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(GetLogs(createdDate, createdAt, limit, cursor));

        public Task<ApiResponse<ApiLogs>> GetLogsWithHttpInfoAsync(
            DateTime? createdDate = null,
            DateTime? createdAt = null,
            long? limit = null,
            string? cursor = null,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(GetLogsWithHttpInfo(createdDate, createdAt, limit, cursor));

        public ApiLogModel GetLog(string apiLogId)
        {
            LogIds.Add(apiLogId);
            Assert.Equal(_log.ApiLogId, apiLogId);
            return _log;
        }

        public ApiResponse<ApiLogModel> GetLogWithHttpInfo(string apiLogId) => Response(GetLog(apiLogId));

        public Task<ApiLogModel> GetLogAsync(string apiLogId, CancellationToken cancellationToken = default) =>
            Task.FromResult(GetLog(apiLogId));

        public Task<ApiResponse<ApiLogModel>> GetLogWithHttpInfoAsync(
            string apiLogId,
            CancellationToken cancellationToken = default) =>
            Task.FromResult(GetLogWithHttpInfo(apiLogId));

        private static ApiResponse<T> Response<T>(T data) =>
            new(HttpStatusCode.OK, new Multimap<string, string>(), data, "{}");
    }
}
