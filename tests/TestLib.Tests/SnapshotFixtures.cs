using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

/// <summary>Builders for snapshot artifacts so tests do not repeat the full record shape.</summary>
internal static class SnapshotFixtures
{
    public static StepSnapshot Step(
        string name = "step",
        string method = "Get",
        TestStatus status = TestStatus.Passed,
        int statusCode = 200,
        SnapshotReturnValue? returnValue = null,
        long durationNanoseconds = 1_000_000,
        IReadOnlyDictionary<string, object?>? stateChanges = null,
        JToken? parameters = null,
        SnapshotError? error = null,
        string? skipReason = null,
        DateTimeOffset? timestamp = null,
        CallStyle callStyle = CallStyle.WithHttpInfo) =>
        new(name, method, parameters ?? new JObject(), returnValue, durationNanoseconds, statusCode,
            status == TestStatus.Passed, status, skipReason, error,
            timestamp ?? DateTimeOffset.UtcNow, stateChanges, callStyle, 1);

    public static SnapshotReturnValue ReturnValue(
        object? response = null,
        string? type = null,
        int statusCode = 200,
        IReadOnlyDictionary<string, string>? headers = null,
        string body = "{}") =>
        new(type ?? response?.GetType().Name ?? "void", statusCode, statusCode == 0 ? "" : $"{statusCode} OK",
            null, response is null ? null : JToken.FromObject(response), body,
            headers ?? new Dictionary<string, string>());

    public static SnapshotSummary Summary(int total = 1, int passed = 1, int failed = 0, int skipped = 0,
        long durationNanoseconds = 1_000_000) =>
        new(total, passed, failed, skipped, durationNanoseconds,
            total == 0 ? 0 : durationNanoseconds / total);

    public static SnapshotMetadata Metadata(CaptureLevel level = CaptureLevel.Full,
        string gitTag = "", string gitCommit = "commit") =>
        new("unknown", "dev", level, gitTag, gitCommit);

    public static StorySnapshot Story(
        string name = "Billing story",
        string description = "",
        TestStatus status = TestStatus.Passed,
        IReadOnlyList<StepSnapshot>? steps = null,
        SnapshotSummary? summary = null,
        SnapshotMetadata? metadata = null,
        IReadOnlyDictionary<string, object?>? variables = null,
        long durationNanoseconds = 1_000_000,
        DateTimeOffset? timestamp = null)
    {
        var resolved = steps ?? new[] { Step() };
        return new StorySnapshot(name, description, timestamp ?? DateTimeOffset.UtcNow, durationNanoseconds,
            status, variables ?? new Dictionary<string, object?>(), resolved,
            summary ?? Summary(resolved.Count, resolved.Count(x => x.Status == TestStatus.Passed),
                resolved.Count(x => x.Status == TestStatus.Failed),
                resolved.Count(x => x.Status == TestStatus.Skipped), durationNanoseconds),
            metadata ?? Metadata());
    }

    /// <summary>A snapshot whose single step carries the supplied response payload.</summary>
    public static StorySnapshot WithResponse(object response, string name = "Billing story") =>
        Story(name, steps: new[] { Step(returnValue: ReturnValue(response)) });
}
