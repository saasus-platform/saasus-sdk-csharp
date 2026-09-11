using System.Net;
using Newtonsoft.Json.Linq;

namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Converts an executed <see cref="StoryResult"/> into the cross-language snapshot
/// document. Field names, nanosecond durations and the nested
/// <c>return_value</c>/<c>http_response</c> shape mirror the Go and PHP suites.
/// </summary>
public static class SnapshotFactory
{
    private const string TraceHeader = "X-Saasus-Trace-Id";

    public static StorySnapshot Create(Story story, StoryResult result, SnapshotConfig config)
    {
        var masker = SnapshotMasker.For(config);
        var steps = config.CaptureLevel == CaptureLevel.Story
            ? Array.Empty<StepSnapshot>()
            : result.Steps.Select(step => CreateStep(step, config, masker)).ToArray();

        var summary = CreateSummary(steps);
        return new StorySnapshot(
            story.Name,
            story.Description,
            DateTimeOffset.UtcNow,
            SnapshotJson.ToNanoseconds(result.Duration),
            result.Status,
            // Go and PHP serialise story variables at every capture level; dropping them would
            // discard the run context and make the artifact incomparable.
            MaskDictionary(result.Variables, masker),
            steps,
            summary,
            new SnapshotMetadata(
                config.SdkVersion,
                config.TestEnvironment,
                config.CaptureLevel,
                GitInfo.ExactTag() ?? string.Empty,
                GitInfo.Commit() ?? string.Empty));
    }

    private static StepSnapshot CreateStep(StepResult step, SnapshotConfig config, SnapshotMasker masker)
    {
        var includeResponse = config.CaptureLevel is CaptureLevel.Full or CaptureLevel.Response;
        var headers = includeResponse ? FlattenHeaders(step.Headers, masker) : new Dictionary<string, string>();
        var statusCode = step.StatusCode ?? 0;

        SnapshotReturnValue? returnValue = null;
        if (includeResponse && (step.Response is not null || step.StatusCode.HasValue || step.Body is not null))
        {
            var body = masker.ProcessBody(step.Body);
            returnValue = new SnapshotReturnValue(
                step.Response?.GetType().FullName ?? "void",
                statusCode,
                DescribeStatus(step.StatusCode),
                CreateHttpResponse(step, headers, body),
                // Go and PHP emit an empty object for a void call, so a later typed response is
                // an added field rather than an incompatible type change.
                step.Response is null ? new JObject() : masker.ProcessTokenCopy(step.Response),
                body,
                headers);
        }

        var error = step.Error is null
            ? null
            : new SnapshotError(
                step.Error.GetType().FullName ?? step.Error.GetType().Name,
                masker.ProcessText(step.Error.Message),
                string.IsNullOrEmpty(step.Body) ? null : masker.ProcessBody(step.Body));

        var parameters = config.CaptureLevel is CaptureLevel.Full or CaptureLevel.Step
            ? WrapParameters(masker.ProcessTokenCopy(step.Parameters ?? new Dictionary<string, object?>()))
            : new JObject();

        var stateChanges = MaskStateChanges(step.StateChanges, masker);
        return new StepSnapshot(
            step.Name,
            step.Method,
            parameters,
            returnValue,
            SnapshotJson.ToNanoseconds(step.Duration),
            statusCode,
            step.Status == TestStatus.Passed,
            step.Status,
            // Matches the reference omitempty shape, and a reason can quote an API error, so it is
            // redacted before it reaches the artifact.
            string.IsNullOrWhiteSpace(step.SkipReason) ? null : masker.ProcessText(step.SkipReason),
            error,
            step.Timestamp ?? DateTimeOffset.UtcNow,
            stateChanges.Count == 0 ? null : stateChanges,
            step.CallStyle,
            step.Attempts);
    }

    /// <summary>
    /// Summarises the steps that were actually written to the artifact. Go's
    /// <c>generateExecutionSummary</c> and PHP's <c>summary</c> both sum the captured step
    /// durations, so setup, cleanup and gaps between steps are excluded and the STORY level
    /// reports an empty summary.
    /// </summary>
    /// <summary>
    /// <c>parameters</c> is always an object in the shared contract. PHP's <c>snapshotParameters</c>
    /// stores a scalar or list under a <c>value</c> property, so a non-object capture stays
    /// comparable across languages.
    /// </summary>
    private static JToken WrapParameters(JToken? parameters) => parameters switch
    {
        null => new JObject(),
        JObject obj => obj,
        _ => new JObject { ["value"] = parameters }
    };

    private static SnapshotSummary CreateSummary(IReadOnlyList<StepSnapshot> steps)
    {
        var total = steps.Sum(step => step.DurationNanoseconds);
        var count = steps.Count;
        return new SnapshotSummary(
            count,
            steps.Count(x => x.Status == TestStatus.Passed),
            steps.Count(x => x.Status == TestStatus.Failed),
            steps.Count(x => x.Status == TestStatus.Skipped),
            total,
            count == 0 ? 0 : total / count);
    }

    /// <summary>
    /// Synthesises the <c>http_response</c> block for call styles that surface HTTP
    /// metadata. Returns null for plain object calls, which report no status code,
    /// matching Go's nil pointer.
    /// </summary>
    private static SnapshotHttpResponse? CreateHttpResponse(
        StepResult step, IReadOnlyDictionary<string, string> headers, string normalizedBody)
    {
        // Only the absence of a status code means the call style surfaced no HTTP metadata.
        // A response without headers still carries a status and a body worth recording.
        if (step.StatusCode is not { } statusCode) return null;
        // The artifact persists the masked body, so the length must describe that body: a
        // Content-Length header or a raw length would drift whenever a dynamic value is
        // normalised, and content_length is not an ignored path.
        var contentLength = System.Text.Encoding.UTF8.GetByteCount(normalizedBody);
        headers.TryGetValue(TraceHeader, out var traceId);
        return new SnapshotHttpResponse(statusCode, DescribeStatus(statusCode), headers, contentLength, traceId);
    }

    /// <summary>Formats a status code the way Go's <c>http.Response.Status</c> does.</summary>
    private static string DescribeStatus(int? statusCode) => statusCode is not { } code || code == 0
        ? string.Empty
        : $"{code} {ReasonPhrase(code)}".TrimEnd();

    private static string ReasonPhrase(int code) =>
        Enum.IsDefined(typeof(HttpStatusCode), code) ? SplitPascalCase(((HttpStatusCode)code).ToString()) : string.Empty;

    private static string SplitPascalCase(string value)
    {
        var builder = new System.Text.StringBuilder(value.Length + 4);
        for (var i = 0; i < value.Length; i++)
        {
            if (i > 0 && char.IsUpper(value[i]) && !char.IsUpper(value[i - 1])) builder.Append(' ');
            builder.Append(value[i]);
        }
        return builder.ToString();
    }

    /// <summary>
    /// Collapses the multi-value header dictionary into the single-value map used by Go and PHP,
    /// masking sensitive header values. PHP records <c>$values[0]</c>, so only the first value is
    /// kept: joining them would make a multi-valued header such as <c>Set-Cookie</c> differ.
    /// </summary>
    private static IReadOnlyDictionary<string, string> FlattenHeaders(
        IReadOnlyDictionary<string, IReadOnlyList<string>>? headers, SnapshotMasker masker)
    {
        var keyMasker = new Masker();
        var flattened = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var (key, values) in headers ?? new Dictionary<string, IReadOnlyList<string>>())
        {
            var first = values.Count > 0 ? values[0] : string.Empty;
            flattened[key] = keyMasker.IsSensitiveKey(key) ? Masker.MaskValue(first) : masker.ProcessText(first);
        }
        return flattened;
    }

    /// <summary>
    /// Serialises each transition as <c>old_value</c> / <c>new_value</c> / <c>timestamp</c>,
    /// matching Go's <c>TrackStateChange</c> and PHP's <c>snapshotStateChanges</c>. Both values
    /// are masked with the variable's own key so a volatile variable stays deterministic.
    /// </summary>
    private static IReadOnlyDictionary<string, object?> MaskStateChanges(
        IReadOnlyDictionary<string, StateChange>? changes,
        SnapshotMasker masker)
    {
        var result = new Dictionary<string, object?>(StringComparer.Ordinal);
        foreach (var (key, change) in changes ?? new Dictionary<string, StateChange>())
        {
            result[key] = new JObject
            {
                ["old_value"] = masker.ProcessValueForKey(key, change.OldValue),
                ["new_value"] = masker.ProcessValueForKey(key, change.NewValue),
                ["timestamp"] = change.Timestamp
            };
        }
        return result;
    }

    private static IReadOnlyDictionary<string, object?> MaskDictionary(
        IReadOnlyDictionary<string, object?>? values,
        SnapshotMasker masker)
    {
        var token = masker.ProcessTokenCopy(values ?? new Dictionary<string, object?>()) as JObject;
        return token?.Properties().ToDictionary(
            property => property.Name,
            property => property.Value.ToObject<object?>(),
            StringComparer.Ordinal) ?? new Dictionary<string, object?>();
    }
}
