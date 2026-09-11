using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

/// <summary>
/// Covers the reporter's text and JSON output, the coverage percentage and per-method
/// statistics, and the structured execution log.
/// </summary>
public sealed class ReportingAndLoggingTests
{
    [Fact]
    public void CoverageExposesPercentageAndPerMethodStatistics()
    {
        var coverage = new CoverageTracker();
        coverage.Register("Get", CallStyle.Async);
        coverage.Register("Delete", CallStyle.Async);
        coverage.Register("Update", CallStyle.WithHttpInfoAsync);
        coverage.Record("Get", CallStyle.Async, true, TimeSpan.FromMilliseconds(10));
        coverage.Record("Get", CallStyle.Async, false, TimeSpan.FromMilliseconds(30));
        coverage.Record("Delete", CallStyle.Async, true, TimeSpan.FromMilliseconds(4));

        var summary = coverage.Coverage;
        Assert.Equal(2, summary.Covered);
        Assert.Equal(3, summary.Total);
        Assert.Equal(66.7, summary.Percentage);
        Assert.False(coverage.IsFullyCovered);

        var stats = coverage.Stats("Get", CallStyle.Async);
        Assert.Equal(2, stats.Executions);
        Assert.Equal(50d, stats.SuccessRate);
        Assert.Equal(TimeSpan.FromMilliseconds(20), stats.AverageDuration);

        var untested = coverage.Stats("Update", CallStyle.WithHttpInfoAsync);
        Assert.Equal(0, untested.Executions);
        Assert.Equal(TimeSpan.Zero, untested.AverageDuration);
    }

    [Fact]
    public void EmptyRegistrationCountsAsFullyCovered()
    {
        var coverage = new CoverageTracker();

        Assert.Equal(100d, coverage.Coverage.Percentage);
        Assert.True(coverage.IsFullyCovered);
    }

    [Fact]
    public void TextReportIncludesCoveragePercentageAndFailureDetail()
    {
        var coverage = new CoverageTracker();
        coverage.Register("Get", CallStyle.Async);
        coverage.Register("Delete", CallStyle.WithHttpInfoAsync);
        coverage.Record("Get", CallStyle.Async, false, TimeSpan.FromMilliseconds(12));

        var report = new Reporter().Render(new[] { FailedStory() }, coverage);

        Assert.Contains("Method coverage: 1/2 (50.0%)", report, StringComparison.Ordinal);
        Assert.Contains("Steps: 0/1 passed", report, StringComparison.Ordinal);
        Assert.Contains("http 500", report, StringComparison.Ordinal);
        Assert.Contains("after 3 attempts", report, StringComparison.Ordinal);
        Assert.Contains("! boom", report, StringComparison.Ordinal);
        Assert.Contains("! cleanup failed: cleanup exploded", report, StringComparison.Ordinal);
        Assert.Contains("UNTESTED Delete [WithHttpInfoAsync]", report, StringComparison.Ordinal);
    }

    [Fact]
    public void TextReportRedactsCredentialsInErrorMessages()
    {
        var story = new StoryResult("story", TestStatus.Failed, TimeSpan.Zero,
            new[] { new StepResult("s", "M", CallStyle.Async, TestStatus.Failed, TimeSpan.Zero,
                Error: new InvalidOperationException("failed with Bearer abc.def")) });

        var report = new Reporter().Render(new[] { story }, new CoverageTracker());

        Assert.Contains("Bearer [MASKED]", report, StringComparison.Ordinal);
        Assert.DoesNotContain("abc.def", report, StringComparison.Ordinal);
    }

    [Fact]
    public void JsonReportUsesTheSharedContract()
    {
        var coverage = new CoverageTracker();
        coverage.Register("Get", CallStyle.Async);
        coverage.Register("Delete", CallStyle.WithHttpInfoAsync);
        coverage.Record("Get", CallStyle.Async, false, TimeSpan.FromMilliseconds(12));

        var document = JObject.Parse(new Reporter().ToJson(new[] { FailedStory() }, coverage));

        Assert.Equal(
            new[] { "timestamp", "summary", "stories", "coverage_details", "untested" },
            document.Properties().Select(property => property.Name).ToArray());
        Assert.Equal(
            new[] { "total_stories", "passed_stories", "failed_stories", "skipped_stories", "total_steps", "passed_steps", "coverage", "covered_methods", "total_methods", "duration" },
            ((JObject)document["summary"]!).Properties().Select(property => property.Name).ToArray());
        Assert.Equal(50d, document["summary"]!["coverage"]);
        Assert.Equal(1, document["summary"]!["failed_stories"]);

        var story = (JObject)document["stories"]![0]!;
        Assert.Equal("failed", story["status"]);
        Assert.Equal("cleanup exploded", story["cleanup_error"]);
        Assert.Null(story["setup_error"]);

        var step = (JObject)story["steps"]![0]!;
        Assert.Equal(
            new[] { "name", "method", "call_style", "status", "status_code", "duration", "attempts", "error" },
            step.Properties().Select(property => property.Name).ToArray());
        Assert.Equal(500, step["status_code"]);
        Assert.Equal(3, step["attempts"]);

        var detail = (JObject)document["coverage_details"]![0]!;
        Assert.Equal(
            new[] { "method", "call_style", "executions", "success_rate", "average_duration" },
            detail.Properties().Select(property => property.Name).ToArray());
        Assert.Equal(12_000_000L, detail["average_duration"]);

        var untested = (JObject)document["untested"]![0]!;
        Assert.Equal("Delete", untested["method"]);
        Assert.Equal("with_http_info_async", untested["call_style"]);
    }

    [Fact]
    public void JsonReportRedactsVariables()
    {
        var story = new StoryResult("story", TestStatus.Passed, TimeSpan.Zero, Array.Empty<StepResult>(),
            Variables: new Dictionary<string, object?>
            {
                ["stripe_secret_key"] = "sk_test_visible",
                ["tenant"] = "acme"
            });

        var document = JObject.Parse(new Reporter().ToJson(new[] { story }, new CoverageTracker()));
        var variables = document["stories"]![0]!["variables"]!;

        Assert.Equal("[MASKED len=15]", variables["stripe_secret_key"]);
        Assert.Equal("acme", variables["tenant"]);
    }

    [Fact]
    public async Task JsonReportCanBeWrittenToDisk()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"saasus-report-{Guid.NewGuid():N}");
        try
        {
            var path = Path.Combine(directory, "nested", "report.json");
            await new Reporter().WriteJsonAsync(path, new[] { FailedStory() }, new CoverageTracker());

            Assert.True(File.Exists(path));
            var text = await File.ReadAllTextAsync(path);
            Assert.EndsWith("}\n", text, StringComparison.Ordinal);
            Assert.Equal(1, JObject.Parse(text)["summary"]!["total_stories"]);
        }
        finally { if (Directory.Exists(directory)) Directory.Delete(directory, true); }
    }

    [Fact]
    public void SummarizeProducesTheSameNumbersAsTheJsonReport()
    {
        var coverage = new CoverageTracker();
        coverage.Register("Get", CallStyle.Async);
        coverage.Record("Get", CallStyle.Async, true, TimeSpan.FromMilliseconds(5));

        var summary = new Reporter().Summarize(new[] { FailedStory() }, coverage);

        Assert.Equal(1, summary.TotalStories);
        Assert.Equal(0, summary.PassedStories);
        Assert.Equal(1, summary.FailedStories);
        Assert.Equal(1, summary.TotalSteps);
        Assert.Equal(100d, summary.Coverage);
        Assert.Equal(SnapshotJson.ToNanoseconds(TimeSpan.FromMilliseconds(20)), summary.DurationNanoseconds);
    }

    [Fact]
    public async Task DebugLogLevelProducesAStructuredExecutionTrace()
    {
        var writer = new StringWriter();
        var engine = new E2EEngine(new Config { LogLevel = LogLevel.Debug, MaxRetries = 0 },
            logger: new Logger(LogLevel.Debug, writer));
        var story = new Story
        {
            Name = "trace story",
            Module = "billing",
            Steps = new[]
            {
                new Step
                {
                    Name = "get",
                    Method = "GetStripeInfo",
                    ExecuteAsync = MethodExecutor.Async((_, _) => Task.FromResult<object?>(new { ok = true })),
                    UpdateStateAsync = (_, context) =>
                    {
                        context.Variables["tenant_id"] = "acme";
                        context.Variables["api_key"] = "super-secret-value";
                        return Task.CompletedTask;
                    }
                },
                new Step { Name = "skipped", Method = "Delete", Skip = true, SkipReason = "unavailable",
                    ExecuteAsync = MethodExecutor.Async((_, _) => Task.FromResult<object?>(null)) }
            }
        };

        await engine.ExecuteAsync(new[] { story });
        var log = writer.ToString();

        Assert.Contains("story start: trace story (2 steps) module=billing", log, StringComparison.Ordinal);
        Assert.Contains("step start: trace story #1 get -> GetStripeInfo [Async]", log, StringComparison.Ordinal);
        Assert.Contains("validation: get -> ok", log, StringComparison.Ordinal);
        Assert.Contains("state update: get ->", log, StringComparison.Ordinal);
        // State updates render the transition, not only the resulting value.
        Assert.Contains("tenant_id: null -> acme", log, StringComparison.Ordinal);
        Assert.Contains("step result: get -> passed", log, StringComparison.Ordinal);
        Assert.Contains("step skipped: skipped (unavailable)", log, StringComparison.Ordinal);
        Assert.Contains("story end: trace story -> passed", log, StringComparison.Ordinal);
        Assert.Contains("method coverage: 1/2 (50.0%)", log, StringComparison.Ordinal);
        Assert.Contains("untested call: Delete [Async]", log, StringComparison.Ordinal);

        Assert.Contains("api_key: null -> [MASKED len=18]", log, StringComparison.Ordinal);
        Assert.DoesNotContain("super-secret-value", log, StringComparison.Ordinal);
    }

    [Fact]
    public async Task InfoLogLevelStaysQuiet()
    {
        var writer = new StringWriter();
        var engine = new E2EEngine(new Config { LogLevel = LogLevel.Info },
            logger: new Logger(LogLevel.Info, writer));

        await engine.ExecuteAsync(new[] { new Story
        {
            Name = "quiet",
            Steps = new[] { new Step { Name = "get", Method = "Get",
                ExecuteAsync = MethodExecutor.Async((_, _) => Task.FromResult<object?>(new { ok = true })) } }
        } });

        Assert.Equal(string.Empty, writer.ToString());
    }

    [Fact]
    public async Task RetriesAreLoggedAsWarnings()
    {
        var writer = new StringWriter();
        var attempts = 0;
        var engine = new E2EEngine(new Config { LogLevel = LogLevel.Warning, MaxRetries = 2 },
            logger: new Logger(LogLevel.Warning, writer));

        await engine.ExecuteAsync(new[] { new Story
        {
            Name = "retry",
            Steps = new[] { new Step { Name = "get", Method = "Get",
                ExecuteAsync = MethodExecutor.WithHttpInfoAsync((_, _) =>
                {
                    attempts++;
                    return Task.FromResult(attempts < 3
                        ? ExecutionResult.WithHttp(null, 503)
                        : ExecutionResult.WithHttp(new { ok = true }, 200));
                }) } }
        } });

        var log = writer.ToString();
        Assert.Contains("retrying Get (attempt 1/3): HTTP 503", log, StringComparison.Ordinal);
        Assert.Contains("retrying Get (attempt 2/3): HTTP 503", log, StringComparison.Ordinal);
        Assert.Equal(3, attempts);
    }

    private static StoryResult FailedStory() => new(
        "story", TestStatus.Failed, TimeSpan.FromMilliseconds(20),
        new[]
        {
            new StepResult("get", "Get", CallStyle.Async, TestStatus.Failed, TimeSpan.FromMilliseconds(12),
                StatusCode: 500, Error: new InvalidOperationException("boom"), Attempts: 3)
        },
        CleanupError: new InvalidOperationException("cleanup exploded"));
}
