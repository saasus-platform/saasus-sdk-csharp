namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Level-filtered logger with automatic credential redaction. The structured helpers
/// mirror Go's and PHP's story/step logging so <c>E2E_LOG_LEVEL=debug</c> produces a
/// usable execution trace.
/// </summary>
public sealed class Logger
{
    private readonly TextWriter _writer;
    private readonly Masker _masker;

    public Logger(LogLevel minimum = LogLevel.Info, TextWriter? writer = null, Masker? masker = null)
    {
        Minimum = minimum;
        _writer = writer ?? Console.Out;
        _masker = masker ?? new Masker();
    }

    public LogLevel Minimum { get; }

    public bool IsEnabled(LogLevel level) => Minimum != LogLevel.None && level >= Minimum;

    public void Write(LogLevel level, string message)
    {
        if (!IsEnabled(level)) return;
        _writer.WriteLine($"[{DateTimeOffset.UtcNow:O}] [{level.ToString().ToUpperInvariant()}] {_masker.MaskText(message)}");
    }

    public void Debug(string message) => Write(LogLevel.Debug, message);
    public void Info(string message) => Write(LogLevel.Info, message);
    public void Warning(string message) => Write(LogLevel.Warning, message);
    public void Error(string message) => Write(LogLevel.Error, message);

    public void LogStoryStart(Story story) =>
        Debug($"story start: {story.Name} ({story.Steps.Count} steps)" +
              (string.IsNullOrEmpty(story.Module) ? string.Empty : $" module={story.Module}"));

    public void LogStoryEnd(StoryResult result) =>
        Debug($"story end: {result.Name} -> {Describe(result.Status)} " +
              $"({result.Duration.TotalMilliseconds:F0} ms, " +
              $"{result.Steps.Count(step => step.Status == TestStatus.Passed)}/{result.Steps.Count} steps passed)");

    public void LogStepStart(Story story, Step step, int index) =>
        Debug($"step start: {story.Name} #{index + 1} {step.Name} -> {step.Method} [{step.CallStyle}]");

    public void LogStepResult(StepResult result)
    {
        if (!IsEnabled(LogLevel.Debug)) return;
        var status = result.StatusCode is { } code and > 0 ? $" http={code}" : string.Empty;
        var attempts = result.Attempts > 1 ? $" attempts={result.Attempts}" : string.Empty;
        Debug($"step result: {result.Name} -> {Describe(result.Status)}{status}{attempts} " +
              $"({result.Duration.TotalMilliseconds:F0} ms)");
    }

    public void LogSkip(Step step) =>
        Debug($"step skipped: {step.Name}" + (string.IsNullOrEmpty(step.SkipReason) ? string.Empty : $" ({step.SkipReason})"));

    public void LogValidation(string stepName, bool passed, string? detail = null) =>
        Debug($"validation: {stepName} -> {(passed ? "ok" : "failed")}" +
              (string.IsNullOrEmpty(detail) ? string.Empty : $" ({detail})"));

    /// <summary>
    /// Logs the transitions a step produced. Values pass through credential redaction keyed by the
    /// variable name; the rendering deliberately avoids <c>key=value</c> so the line-level
    /// assignment redaction does not mangle an already-masked value.
    /// </summary>
    public void LogStateUpdate(string stepName, IReadOnlyDictionary<string, StateChange> changes)
    {
        if (!IsEnabled(LogLevel.Debug) || changes.Count == 0) return;
        var rendered = string.Join(", ", changes.Select(pair =>
            $"{pair.Key}: {Render(pair.Key, pair.Value.OldValue)} -> {Render(pair.Key, pair.Value.NewValue)}"));
        Debug($"state update: {stepName} -> {rendered}");
    }

    public void LogRetry(string method, int attempt, int maximum, string reason) =>
        Warning($"retrying {method} (attempt {attempt}/{maximum}): {reason}");

    public void LogCoverage(CoverageTracker coverage)
    {
        var summary = coverage.Coverage;
        Info($"method coverage: {summary.Covered}/{summary.Total} ({summary.Percentage:F1}%)");
        foreach (var missing in coverage.Untested) Warning($"untested call: {missing.Method} [{missing.CallStyle}]");
    }

    private string Render(string key, object? value)
    {
        if (value is null) return "null";
        var text = value.ToString() ?? string.Empty;
        return _masker.IsSensitiveKey(key) ? Masker.MaskValue(text) : _masker.MaskText(text);
    }

    private static string Describe(TestStatus status) => status.ToString().ToLowerInvariant();
}
