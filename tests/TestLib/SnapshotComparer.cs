using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Diffs two snapshot documents and classifies every difference as compatible,
/// warning or breaking. Path rules, the dynamic-value exclusions and the 50%
/// duration tolerance match the Go and PHP SDK test suites.
/// </summary>
public sealed partial class SnapshotComparer
{
    /// <summary>A duration difference is only reported once it exceeds this percentage.</summary>
    public const double DurationTolerancePercent = 50d;

    /// <summary>Response headers that change on every request and carry no contract meaning.</summary>
    private static readonly string[] VolatileHeaders =
    {
        "date", "server", "x-saasus-trace-id", "x-request-id", "x-correlation-id", "x-runtime",
        // transport framing: a chunked response omits Content-Length and vice versa
        "transfer-encoding", "content-length"
    };

    /// <summary>
    /// Response collections whose order the API does not guarantee. Everything else - the story
    /// steps, pricing tiers, scalar lists - keeps its positional comparison.
    /// </summary>
    private static readonly string[] UnorderedCollections =
    {
        "units", "pricing_menus", "pricing_plans", "tax_rates", "api_logs", "counts",
        // Auth listings: the API returns the environment's tenants, users and environments in an
        // order that changes between calls.
        "tenants", "users", "envs"
    };

    /// <summary>
    /// Listings of whatever the shared environment contains. Their length is environment data, not
    /// an SDK contract, so a tenant or user added between runs is not a difference.
    /// </summary>
    private static readonly string[] EnvironmentListings = { "tenants", "users" };

    public SnapshotComparison Compare(string story, object baseline, object current, string oldTag = "old", string newTag = "new")
    {
        var issues = new List<CompatibilityIssue>();
        CompareTokens(ToToken(baseline), ToToken(current), "$", issues);
        return new SnapshotComparison(story, oldTag, newTag, issues);
    }

    public SnapshotComparison Compare(object baseline, object current) => Compare("snapshot", baseline, current);

    private static JToken ToToken(object value) =>
        value as JToken ?? JToken.FromObject(value, SnapshotJson.CreateSerializer());

    private static void CompareTokens(JToken old, JToken current, string path, ICollection<CompatibilityIssue> issues)
    {
        if (Ignored(path)) return;
        // The tolerance only applies to two numeric durations; a numeric-to-string change must
        // still fall through to the generic type comparison below.
        if (IsDurationPath(path) && IsNumeric(old) && IsNumeric(current))
        {
            CompareDuration(old, current, path, issues);
            return;
        }
        if (old.Type != current.Type)
        {
            Add("type", path, $"Type changed from {old.Type} to {current.Type}.", old, current, issues);
            return;
        }
        // The raw body on the framework's envelope is JSON text: comparing it structurally
        // applies the same rules as the parsed payload, so formatting and the order of an
        // unordered collection do not register.
        if (EnvelopeBodyRegex().IsMatch(path) && old.Type == JTokenType.String &&
            TryParseJson(old, out var oldBody) && TryParseJson(current, out var newBody))
        {
            CompareTokens(oldBody, newBody, path, issues);
            return;
        }
        if (old is JObject left && current is JObject right)
        {
            foreach (var property in left.Properties())
            {
                var propertyPath = $"{path}.{property.Name}";
                if (right[property.Name] is { } value)
                {
                    CompareTokens(property.Value, value, propertyPath, issues);
                }
                else if (!Ignored(propertyPath))
                {
                    Add("removed", propertyPath, "Field was removed.", property.Value, null, issues);
                }
            }
            foreach (var property in right.Properties().Where(p => left[p.Name] is null))
            {
                var propertyPath = $"{path}.{property.Name}";
                if (!Ignored(propertyPath))
                    Add("added", propertyPath, "Field was added.", null, property.Value, issues);
            }
            return;
        }
        if (old is JArray oldArray && current is JArray newArray)
        {
            // The SaaSus collection endpoints do not guarantee the order of their items, so two
            // orderings of the same objects are not a difference. Ordered arrays - the story
            // steps and payload lists such as pricing tiers - stay positional.
            if (IsUnorderedCollectionPath(path) && IsReordered(oldArray, newArray)) return;
            var common = Math.Min(oldArray.Count, newArray.Count);
            for (var i = 0; i < common; i++) CompareTokens(oldArray[i], newArray[i], $"{path}[{i}]", issues);
            if (oldArray.Count != newArray.Count && !IsEnvironmentListingPath(path))
                Add("array_length", path, $"Array length changed from {oldArray.Count} to {newArray.Count}.",
                    oldArray.Count, newArray.Count, issues, oldArray.Count > newArray.Count
                        ? CompatibilityLevel.Breaking : CompatibilityLevel.Warning);
            return;
        }
        if (!JToken.DeepEquals(old, current))
        {
            // Two redacted values carry no contract information: the mask length changes
            // whenever a test credential is rotated, which is not a compatibility signal.
            if (IsMasked(old) && IsMasked(current)) return;
            // A normalised value carries no information either, so comparing it against a
            // recorded value (for example an empty body) is not a compatibility signal.
            if (IsNormalized(old) || IsNormalized(current)) return;
            Add("value", path, "Value changed.", old, current, issues);
        }
    }

    private static bool IsNormalized(JToken value) =>
        value.Type == JTokenType.String &&
        string.Equals(value.Value<string>(), SnapshotMasker.DynamicToken, StringComparison.Ordinal);

    private static bool IsUnorderedCollectionPath(string path) =>
        UnorderedCollections.Contains(path[(path.LastIndexOf('.') + 1)..], StringComparer.OrdinalIgnoreCase);

    private static bool IsEnvironmentListingPath(string path) =>
        EnvironmentListings.Contains(path[(path.LastIndexOf('.') + 1)..], StringComparer.OrdinalIgnoreCase);

    /// <summary>True when both arrays hold the same objects in a different order.</summary>
    private static bool IsReordered(JArray old, JArray current)
    {
        if (old.Count < 2 || old.Count != current.Count) return false;
        if (!old.All(item => item is JObject) || !current.All(item => item is JObject)) return false;
        if (JToken.DeepEquals(old, current)) return false;

        static IEnumerable<string> Canonical(JArray array) =>
            array.Select(item => item.ToString(Newtonsoft.Json.Formatting.None))
                .OrderBy(text => text, StringComparer.Ordinal);
        return Canonical(old).SequenceEqual(Canonical(current), StringComparer.Ordinal);
    }

    private static bool TryParseJson(JToken value, out JToken parsed)
    {
        var text = value.Value<string>();
        if (!string.IsNullOrWhiteSpace(text))
        {
            var trimmed = text.TrimStart();
            if (trimmed.StartsWith('{') || trimmed.StartsWith('['))
            {
                try
                {
                    parsed = JToken.Parse(text);
                    return true;
                }
                catch (Newtonsoft.Json.JsonReaderException) { }
            }
        }

        parsed = JValue.CreateNull();
        return false;
    }

    private static bool IsMasked(JToken value) =>
        value.Type == JTokenType.String && MaskedTokenRegex().IsMatch(value.Value<string>() ?? string.Empty);

    /// <summary>
    /// Durations drift on every run, so they are only reported when the change is
    /// large enough to indicate a real regression.
    /// </summary>
    private static void CompareDuration(JToken old, JToken current, string path, ICollection<CompatibilityIssue> issues)
    {
        var before = old.Value<double>();
        var after = current.Value<double>();
        // A non-positive duration is unmeasured, not a regression: comparing it would report a
        // large percentage change, which the reference implementations also suppress.
        if (before <= 0 || after <= 0) return;
        var change = (after - before) / before * 100d;
        if (Math.Abs(change) <= DurationTolerancePercent) return;
        issues.Add(new CompatibilityIssue("timing", path,
            // The persisted description must not vary with the machine locale.
            string.Format(CultureInfo.InvariantCulture, "Duration changed by {0:F1}%.", change),
            old, current, CompatibilityLevel.Warning));
    }

    private static bool IsNumeric(JToken value) =>
        value.Type is JTokenType.Integer or JTokenType.Float;

    private static void Add(string kind, string path, string description, object? old, object? current,
        ICollection<CompatibilityIssue> issues, CompatibilityLevel? level = null)
    {
        var oldToken = old is null ? null : old as JToken ?? JToken.FromObject(old);
        var newToken = current is null ? null : current as JToken ?? JToken.FromObject(current);
        issues.Add(new CompatibilityIssue(
            DifferenceType(kind, path),
            path,
            description,
            oldToken,
            newToken,
            level ?? Impact(kind, path, oldToken, newToken)));
    }

    /// <summary>
    /// Volatile framework metadata that must never fail a comparison. The patterns are anchored to
    /// the framework's own paths: a response field that happens to be called <c>timestamp</c> or
    /// <c>attempts</c> is part of the API contract and must still be compared.
    /// </summary>
    private static bool Ignored(string path) =>
        path.StartsWith("$.metadata.", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("$.timestamp", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("$.validation_time", StringComparison.OrdinalIgnoreCase) ||
        StepMetadataRegex().IsMatch(path) ||
        StateChangeTimestampRegex().IsMatch(path) ||
        TraceIdRegex().IsMatch(path) ||
        // transport framing on the envelope, derived from the (normalised) payload size
        EnvelopeContentLengthRegex().IsMatch(path) ||
        IsVolatileHeaderPath(path);

    /// <summary>
    /// Response headers are transport metadata only on the framework's HTTP envelope; a
    /// <c>headers</c> object inside an API payload is part of the contract.
    /// </summary>
    private static bool IsVolatileHeaderPath(string path)
    {
        var match = EnvelopeHeaderRegex().Match(path);
        return match.Success && VolatileHeaders.Contains(match.Groups[1].Value, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// The framework's own duration fields. A <c>duration</c> property inside an API payload is
    /// part of the contract and must be compared exactly.
    /// </summary>
    private static bool IsDurationPath(string path) =>
        path.Equals("$.duration", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("$.summary.total_duration", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("$.summary.average_step_duration", StringComparison.OrdinalIgnoreCase) ||
        StepDurationRegex().IsMatch(path);

    /// <summary>Maps a raw difference kind onto the domain-specific type reported in artifacts.</summary>
    private static string DifferenceType(string kind, string path)
    {
        if (path.EndsWith(".status_code", StringComparison.OrdinalIgnoreCase)) return "status_code";
        if (path.EndsWith(".method", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".step_name", StringComparison.OrdinalIgnoreCase)) return "step_sequence";
        if (path.Contains(".state_changes", StringComparison.OrdinalIgnoreCase) ||
            path.Contains("$.variables", StringComparison.OrdinalIgnoreCase)) return "state_transition";
        if (path.Contains(".headers", StringComparison.OrdinalIgnoreCase)) return "header";
        if (path.Contains(".return_value", StringComparison.OrdinalIgnoreCase)) return "response";
        if (path.Equals("$.status", StringComparison.OrdinalIgnoreCase)) return "story_status";
        return kind;
    }

    /// <summary>
    /// Classifies a difference. Anything that removes or reshapes the SDK contract is
    /// breaking; additive and cosmetic changes are warnings.
    /// </summary>
    private static CompatibilityLevel Impact(string kind, string path, JToken? old, JToken? current)
    {
        if (kind == "added") return CompatibilityLevel.Warning;
        if (kind is "removed" or "type") return CompatibilityLevel.Breaking;

        if (path.Equals("$.status", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".method", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".step_name", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".status_code", StringComparison.OrdinalIgnoreCase) ||
            path.EndsWith(".return_value.type", StringComparison.OrdinalIgnoreCase) ||
            path.Contains(".return_value.json_data", StringComparison.OrdinalIgnoreCase))
            return CompatibilityLevel.Breaking;

        if (path.Contains(".headers", StringComparison.OrdinalIgnoreCase) ||
            path.Contains(".state_changes", StringComparison.OrdinalIgnoreCase) ||
            path.Contains("$.variables", StringComparison.OrdinalIgnoreCase))
            return CompatibilityLevel.Warning;

        // steps[n].success mirrors the status: true -> false is a regression, like passed -> failed.
        if (path.EndsWith(".success", StringComparison.OrdinalIgnoreCase))
            return old?.Type == JTokenType.Boolean && old.Value<bool>() && current?.Value<bool>() == false
                ? CompatibilityLevel.Breaking
                : CompatibilityLevel.Warning;

        if (WasPassing(old) && !WasPassing(current)) return CompatibilityLevel.Breaking;
        return CompatibilityLevel.Warning;
    }

    private static bool WasPassing(JToken? value) => value?.Type == JTokenType.String &&
        string.Equals(value.Value<string>(), "passed", StringComparison.OrdinalIgnoreCase);

    [GeneratedRegex(@"^\[MASKED(?: len=\d+)?\]$")]
    private static partial Regex MaskedTokenRegex();

    /// <summary>The step's own <c>timestamp</c> and <c>attempts</c> fields, not nested response fields.</summary>
    [GeneratedRegex(@"^\$\.steps\[\d+\]\.(?:timestamp|attempts)$", RegexOptions.IgnoreCase)]
    private static partial Regex StepMetadataRegex();

    /// <summary>The timestamp the framework stamps on each recorded state transition.</summary>
    [GeneratedRegex(@"^\$\.steps\[\d+\]\.state_changes\.[^.]+\.timestamp$", RegexOptions.IgnoreCase)]
    private static partial Regex StateChangeTimestampRegex();

    /// <summary>The request trace identifier on the framework's HTTP envelope.</summary>
    [GeneratedRegex(@"^\$\.steps\[\d+\]\.return_value\.(?:http_response\.)?trace_id$|^\$\.trace_id$", RegexOptions.IgnoreCase)]
    private static partial Regex TraceIdRegex();

    /// <summary>The framework's own per-step duration, not a nested payload field.</summary>
    [GeneratedRegex(@"^\$\.steps\[\d+\]\.duration$", RegexOptions.IgnoreCase)]
    private static partial Regex StepDurationRegex();

    /// <summary>A header on the framework's HTTP envelope: <c>return_value[.http_response].headers.X</c>.</summary>
    [GeneratedRegex(@"^\$\.steps\[\d+\]\.return_value\.(?:http_response\.)?headers\.([^.]+)$", RegexOptions.IgnoreCase)]
    private static partial Regex EnvelopeHeaderRegex();

    /// <summary>The transport content length on the framework's HTTP envelope.</summary>
    [GeneratedRegex(@"^\$\.steps\[\d+\]\.return_value\.(?:http_response\.)?content_length$", RegexOptions.IgnoreCase)]
    private static partial Regex EnvelopeContentLengthRegex();

    /// <summary>The raw response body on the framework's envelope, which holds JSON text.</summary>
    [GeneratedRegex(@"^\$\.steps\[\d+\]\.return_value\.body$", RegexOptions.IgnoreCase)]
    private static partial Regex EnvelopeBodyRegex();
}
