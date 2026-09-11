namespace SaasusSdk.Tests.TestLib;

public enum TestStatus
{
    Passed,
    Failed,
    Skipped
}

public enum CallStyle
{
    Sync,
    WithHttpInfo,
    Async,
    WithHttpInfoAsync
}

public enum LogLevel
{
    Debug,
    Info,
    Warning,
    Error,
    None
}

public sealed record ExecutionResult(
    object? Response = null,
    int? StatusCode = null,
    IReadOnlyDictionary<string, IReadOnlyList<string>>? Headers = null,
    Exception? Error = null,
    string? Body = null,
    // The generated C# clients expose the deserialized value and the ApiResponse
    // wrapper separately.  Keep the wrapper available to module-specific raw
    // response validators without changing the snapshot response shape.
    object? RawResponse = null)
{
    public bool IsSuccess => Error is null;
    public bool HasStatusCode => StatusCode is > 0;

    public static ExecutionResult Success(object? response = null) => new(response);

    public static ExecutionResult WithHttp(
        object? response,
        int statusCode,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? headers = null) =>
        new(response, statusCode, headers);

    public static ExecutionResult Failure(
        Exception error,
        int? statusCode = null,
        IReadOnlyDictionary<string, IReadOnlyList<string>>? headers = null,
        string? body = null) =>
        new(null, statusCode, headers, error, body);

    public static ExecutionResult DryRun() => new(StatusCode: 200);
}

public sealed record StepResult(
    string Name,
    string Method,
    CallStyle CallStyle,
    TestStatus Status,
    TimeSpan Duration,
    int? StatusCode = null,
    object? Response = null,
    IReadOnlyDictionary<string, IReadOnlyList<string>>? Headers = null,
    Exception? Error = null,
    string? SkipReason = null,
    int Attempts = 1,
    DateTimeOffset? Timestamp = null,
    string? Body = null,
    IReadOnlyDictionary<string, StateChange>? StateChanges = null,
    object? Parameters = null);

/// <summary>
/// A single variable transition recorded for a step. The <c>old_value</c> /
/// <c>new_value</c> / <c>timestamp</c> shape matches Go's <c>TrackStateChange</c> and PHP's
/// <c>snapshotStateChanges</c>, so state changes are comparable across languages.
/// </summary>
public sealed record StateChange(object? OldValue, object? NewValue, DateTimeOffset Timestamp);

public sealed record StoryResult(
    string Name,
    TestStatus Status,
    TimeSpan Duration,
    IReadOnlyList<StepResult> Steps,
    Exception? SetupError = null,
    Exception? CleanupError = null,
    IReadOnlyDictionary<string, object?>? Variables = null);

public sealed record CoverageEntry(
    string Method,
    CallStyle CallStyle,
    int Executions,
    int Successes,
    int Failures,
    TimeSpan TotalDuration);
