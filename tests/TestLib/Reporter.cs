using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Renders E2E results as human-readable text or as a machine-readable JSON document.
/// The JSON summary keys mirror the Go and PHP reporters so CI tooling can consume any SDK.
/// </summary>
public sealed class Reporter
{
    private readonly Masker _masker = new();

    public string Render(IEnumerable<StoryResult> results, CoverageTracker coverage)
    {
        var stories = results.ToArray();
        var steps = stories.SelectMany(story => story.Steps).ToArray();
        var summary = coverage.Coverage;
        var builder = new StringBuilder();
        builder.AppendLine("SaaSus SDK E2E test report");
        builder.AppendLine($"Stories: {stories.Count(x => x.Status == TestStatus.Passed)}/{stories.Length} passed, " +
                           $"failed: {stories.Count(x => x.Status == TestStatus.Failed)}, " +
                           $"skipped: {stories.Count(x => x.Status == TestStatus.Skipped)}");
        builder.AppendLine($"Steps: {steps.Count(x => x.Status == TestStatus.Passed)}/{steps.Length} passed, " +
                           $"failed: {steps.Count(x => x.Status == TestStatus.Failed)}, " +
                           $"skipped: {steps.Count(x => x.Status == TestStatus.Skipped)}");
        builder.AppendLine($"Method coverage: {summary.Covered}/{summary.Total} ({summary.Percentage:F1}%)");
        builder.AppendLine($"Execution time: {TimeSpan.FromTicks(stories.Sum(x => x.Duration.Ticks)).TotalSeconds:F3}s");

        foreach (var story in stories)
        {
            builder.AppendLine($"- {story.Name}: {story.Status} ({story.Duration.TotalMilliseconds:F0} ms)");
            if (story.SetupError is not null)
                builder.AppendLine($"  ! setup failed: {_masker.MaskText(story.SetupError.Message)}");
            foreach (var step in story.Steps)
            {
                var status = step.StatusCode is { } code and > 0 ? $" http {code}" : string.Empty;
                var attempts = step.Attempts > 1 ? $" after {step.Attempts} attempts" : string.Empty;
                builder.AppendLine($"  - {step.Name} [{step.CallStyle}]: {step.Status}{status}{attempts}" +
                                   $" ({step.Duration.TotalMilliseconds:F0} ms)");
                if (step.Error is not null)
                    builder.AppendLine($"    ! {_masker.MaskText(step.Error.Message)}");
                if (!string.IsNullOrEmpty(step.SkipReason))
                    builder.AppendLine($"    skipped: {step.SkipReason}");
            }
            if (story.CleanupError is not null)
                builder.AppendLine($"  ! cleanup failed: {_masker.MaskText(story.CleanupError.Message)}");
        }

        builder.AppendLine($"Covered calls: {coverage.Entries.Count}, untested calls: {coverage.Untested.Count}");
        foreach (var missing in coverage.Untested)
            builder.AppendLine($"- UNTESTED {missing.Method} [{missing.CallStyle}]");
        return builder.ToString();
    }

    /// <summary>Structured summary suitable for assertions and for the JSON export.</summary>
    public ReportSummary Summarize(IEnumerable<StoryResult> results, CoverageTracker coverage)
    {
        var stories = results.ToArray();
        var steps = stories.SelectMany(story => story.Steps).ToArray();
        var summary = coverage.Coverage;
        return new ReportSummary(
            stories.Length,
            stories.Count(x => x.Status == TestStatus.Passed),
            stories.Count(x => x.Status == TestStatus.Failed),
            stories.Count(x => x.Status == TestStatus.Skipped),
            steps.Length,
            steps.Count(x => x.Status == TestStatus.Passed),
            summary.Percentage,
            summary.Covered,
            summary.Total,
            SnapshotJson.ToNanoseconds(TimeSpan.FromTicks(stories.Sum(x => x.Duration.Ticks))));
    }

    /// <summary>Serializes the full run as JSON, using the shared snake_case contract.</summary>
    public string ToJson(IEnumerable<StoryResult> results, CoverageTracker coverage)
    {
        var stories = results.ToArray();
        var document = new ReportDocument(
            DateTimeOffset.UtcNow,
            Summarize(stories, coverage),
            stories.Select(story => new ReportStory(
                story.Name,
                story.Status,
                SnapshotJson.ToNanoseconds(story.Duration),
                story.SetupError?.Message is { } setup ? _masker.MaskText(setup) : null,
                story.CleanupError?.Message is { } cleanup ? _masker.MaskText(cleanup) : null,
                Redact(story.Variables),
                story.Steps.Select(step => new ReportStep(
                    step.Name,
                    step.Method,
                    step.CallStyle,
                    step.Status,
                    step.StatusCode ?? 0,
                    SnapshotJson.ToNanoseconds(step.Duration),
                    step.Attempts,
                    step.Error?.Message is { } error ? _masker.MaskText(error) : null,
                    step.SkipReason)).ToArray())).ToArray(),
            coverage.AllStats.Select(stats => new ReportCoverage(
                stats.Method, stats.CallStyle, stats.Executions, stats.SuccessRate,
                SnapshotJson.ToNanoseconds(stats.AverageDuration))).ToArray(),
            coverage.Untested.Select(entry => new ReportUntested(entry.Method, entry.CallStyle)).ToArray());
        return JsonConvert.SerializeObject(document, SnapshotJson.Settings) + "\n";
    }

    /// <summary>Writes the JSON report, creating the parent directory if needed.</summary>
    public async Task WriteJsonAsync(string path, IEnumerable<StoryResult> results, CoverageTracker coverage,
        CancellationToken token = default)
    {
        var directory = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
        await File.WriteAllTextAsync(path, ToJson(results, coverage), token);
    }

    private JObject Redact(IReadOnlyDictionary<string, object?>? variables) =>
        _masker.Mask(variables ?? new Dictionary<string, object?>()) is { } masked
            ? JObject.FromObject(masked)
            : new JObject();
}

public sealed record ReportSummary(
    int TotalStories, int PassedStories, int FailedStories, int SkippedStories,
    int TotalSteps, int PassedSteps, double Coverage, int CoveredMethods, int TotalMethods,
    [property: JsonProperty("duration")] long DurationNanoseconds);

public sealed record ReportStep(
    string Name, string Method, CallStyle CallStyle, TestStatus Status, int StatusCode,
    [property: JsonProperty("duration")] long DurationNanoseconds, int Attempts,
    [property: JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)] string? Error,
    [property: JsonProperty("skip_reason", NullValueHandling = NullValueHandling.Ignore)] string? SkipReason);

public sealed record ReportStory(
    string Name, TestStatus Status,
    [property: JsonProperty("duration")] long DurationNanoseconds,
    [property: JsonProperty("setup_error", NullValueHandling = NullValueHandling.Ignore)] string? SetupError,
    [property: JsonProperty("cleanup_error", NullValueHandling = NullValueHandling.Ignore)] string? CleanupError,
    JObject Variables, IReadOnlyList<ReportStep> Steps);

public sealed record ReportCoverage(
    string Method, CallStyle CallStyle, int Executions, double SuccessRate,
    [property: JsonProperty("average_duration")] long AverageDurationNanoseconds);

public sealed record ReportUntested(string Method, CallStyle CallStyle);

public sealed record ReportDocument(
    DateTimeOffset Timestamp, ReportSummary Summary, IReadOnlyList<ReportStory> Stories,
    IReadOnlyList<ReportCoverage> CoverageDetails, IReadOnlyList<ReportUntested> Untested);
