using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Normalises captured payloads so snapshots are deterministic: credentials are
/// redacted and volatile values (identifiers, e-mail addresses, timestamps,
/// cursors) are replaced with <c>[DYNAMIC]</c>. The default key set and the
/// replacement tokens match the Go and PHP SDK test suites.
/// </summary>
public sealed partial class SnapshotMasker
{
    public const string DynamicToken = "[DYNAMIC]";

    /// <summary>
    /// Field names whose values change on every run. Matching ignores case and
    /// underscores, so <c>tenant_id</c> and <c>tenantId</c> are both covered.
    /// </summary>
    public static readonly IReadOnlyList<string> DefaultDynamicFields = new[]
    {
        // identifiers
        "id", "user_id", "tenant_id", "tenant_user_id", "feedback_id", "comment_id",
        "env_id", "signup_user_id", "api_log_id", "pricing_unit_id", "pricing_menu_id",
        "pricing_plan_id", "tax_rate_id", "metering_unit_id", "trace_id", "request_id",
        "correlation_id",
        // Stripe issues a fresh customer per live run, and the committed baselines record it
        // as [DYNAMIC].
        "customer_id",
        // the pricing clients expose these as menuId/planId, which the reference list covers
        "menu_id", "plan_id",
        "tiered_pricing_unit_id",
        // identifier collections sent in request bodies
        "unit_ids", "menu_ids", "plan_ids",
        // addresses
        "email", "user_email", "signup_email", "updated_email", "staff_email",
        "back_office_staff_email",
        // time and paging
        "created_at", "updated_at", "deleted_at", "created_date", "timestamp",
        "start_timestamp", "end_timestamp", "date", "month", "ttl", "cursor",
        // Recorded API-log traffic. Which request happens to be logged differs on every run,
        // so these values are normalised while the field set - the part that is an SDK
        // contract - stays visible. The remote address also carries an ephemeral port.
        "request_uri", "request_method", "response_status", "request_body", "response_body",
        "remote_address"
    };

    private readonly Masker _masker = new();
    private readonly HashSet<string> _dynamicFields;
    private readonly DynamicFieldMode _mode;

    /// <summary>
    /// Listings of everything the environment happens to contain. Their items are normalised
    /// completely - only the field set, which is the SDK contract, is kept - because a shared
    /// environment gains and loses tenants and users between runs.
    /// </summary>
    private static readonly string[] EnvironmentListings = { "tenants", "users" };

    public SnapshotMasker(
        IEnumerable<string>? fields = null,
        DynamicFieldMode mode = DynamicFieldMode.Replace,
        bool useDefaults = true)
    {
        _dynamicFields = new HashSet<string>(StringComparer.Ordinal);
        if (useDefaults)
            foreach (var field in DefaultDynamicFields) _dynamicFields.Add(Normalize(field));
        foreach (var field in fields ?? Array.Empty<string>())
            if (!string.IsNullOrWhiteSpace(field)) _dynamicFields.Add(Normalize(field));
        _mode = mode;
    }

    public static SnapshotMasker For(SnapshotConfig config) =>
        new(config.DynamicFields, config.DynamicFieldMode, config.UseDefaultDynamicFields);

    public bool IsDynamicKey(string key) => _dynamicFields.Contains(Normalize(key));

    private static string Normalize(string key) =>
        key.Replace("_", string.Empty, StringComparison.Ordinal)
           .Replace("-", string.Empty, StringComparison.Ordinal)
           .ToLowerInvariant();

    public object? Process(object? value)
    {
        if (value is null) return null;
        var token = value as JToken ?? JToken.FromObject(value);
        token = ProcessToken(token);
        Canonicalize(token);
        return token.ToObject<object>();
    }

    public JToken? ProcessTokenCopy(object? value)
    {
        if (value is null) return null;
        var token = value is JToken source ? source.DeepClone() : JToken.FromObject(value);
        token = ProcessToken(token);
        Canonicalize(token);
        return token;
    }

    /// <summary>
    /// Normalises a value as if it were stored under <paramref name="key"/>. Go and PHP mask the
    /// old and new value of a state change with the variable's own key, so a volatile variable
    /// is normalised even though the surrounding keys are <c>old_value</c>/<c>new_value</c>.
    /// </summary>
    public JToken ProcessValueForKey(string key, object? value)
    {
        var wrapper = new JObject
        {
            [key] = value is null
                ? JValue.CreateNull()
                : value is JToken token ? token.DeepClone() : JToken.FromObject(value)
        };
        ProcessToken(wrapper);
        Canonicalize(wrapper);
        return wrapper[key] ?? JValue.CreateNull();
    }

    /// <summary>
    /// Normalises a raw response body. JSON bodies are normalised structurally and
    /// re-serialised with two-space indentation (matching Go and PHP); anything
    /// else falls back to text redaction.
    /// </summary>
    public string ProcessBody(string? value)
    {
        if (string.IsNullOrEmpty(value)) return value ?? string.Empty;
        var trimmed = value.TrimStart();
        if (trimmed.StartsWith('{') || trimmed.StartsWith('['))
        {
            try
            {
                var token = ProcessToken(JToken.Parse(value));
                Canonicalize(token);
                var text = token.ToString(Newtonsoft.Json.Formatting.Indented);
                return value.EndsWith('\n') ? text + "\n" : text;
            }
            catch (Newtonsoft.Json.JsonReaderException) { }
        }
        return _masker.MaskText(value);
    }

    /// <summary>Normalises a value that is stored inline (compact JSON, no pretty printing).</summary>
    public string ProcessText(string? value)
    {
        if (string.IsNullOrEmpty(value)) return value ?? string.Empty;
        try
        {
            var token = ProcessToken(JToken.Parse(value));
            Canonicalize(token);
            return token.ToString(Newtonsoft.Json.Formatting.None);
        }
        catch (Newtonsoft.Json.JsonReaderException)
        {
            return _masker.MaskText(value);
        }
    }

    /// <summary>
    /// Sorts object properties in ordinal order so an equivalent payload delivered with a
    /// different property order does not register as a snapshot difference. Array order is
    /// part of the contract and is preserved.
    /// </summary>
    private static void Canonicalize(JToken token)
    {
        switch (token)
        {
            case JObject obj:
                var properties = obj.Properties().OrderBy(property => property.Name, StringComparer.Ordinal).ToArray();
                obj.RemoveAll();
                foreach (var property in properties)
                {
                    Canonicalize(property.Value);
                    obj.Add(property);
                }
                break;
            case JArray array:
                foreach (var item in array) Canonicalize(item);
                break;
        }
    }

    /// <summary>
    /// Normalises a token in place and returns it. A root string value cannot be replaced in place
    /// because it has no parent, so callers that own the root must use the return value.
    /// </summary>
    private JToken ProcessToken(JToken token)
    {
        if (token is JObject obj)
        {
            foreach (var property in obj.Properties().ToArray())
            {
                // A generated attribute name is also used as an object key. Key changes are
                // classified as breaking, so the key itself has to be normalised.
                if (IsGeneratedResourceName(property.Name))
                {
                    var value = property.Value;
                    property.Remove();
                    obj[DynamicToken] = ProcessToken(value);
                    continue;
                }
                if (IsDynamicKey(property.Name))
                {
                    if (_mode == DynamicFieldMode.Exclude) property.Remove();
                    else property.Value = DynamicValue(property.Value);
                }
                else if (_masker.IsSensitiveKey(property.Name))
                {
                    property.Value = Masker.MaskSensitiveToken(property.Value);
                }
                else if (property.Name is "request_body" or "response_body" && property.Value.Type == JTokenType.String)
                {
                    // Embedded JSON documents are normalised structurally, like the PHP suite.
                    property.Value = ProcessText(property.Value.Value<string>());
                }
                else if (property.Name is "request_uri" && property.Value.Type == JTokenType.String)
                {
                    property.Value = NormalizeUri(property.Value.Value<string>());
                }
                else if (IsEnvironmentListing(property.Name) && property.Value is JArray listing)
                {
                    ProcessToken(listing);
                    property.Value = Blank(listing);
                }
                else
                {
                    ProcessToken(property.Value);
                }
            }
        }
        else if (token is JArray array)
        {
            foreach (var item in array.ToArray()) ProcessToken(item);
        }
        else if (token.Type == JTokenType.Raw)
        {
            // Generated oneOf models serialise themselves with WriteRawValue, so their payload
            // arrives as an opaque JRaw token that must be parsed to be masked.
            var parsed = ParseRaw(token.Value<string>());
            if (token.Parent is null) return parsed;
            token.Replace(parsed);
            return parsed;
        }
        else if (token.Type == JTokenType.String)
        {
            var text = token.Value<string>();
            JToken replacement = IsGeneratedResourceName(text)
                ? DynamicToken
                : NormalizeVolatileText(_masker.MaskText(text));
            // A root value has no parent, so Replace would throw: the caller uses the return value.
            if (token.Parent is null) return replacement;
            token.Replace(replacement);
            return replacement;
        }
        return token;
    }

    private static bool IsEnvironmentListing(string key) =>
        EnvironmentListings.Contains(Normalize(key), StringComparer.Ordinal);

    /// <summary>
    /// Normalises every leaf of a container while keeping its keys and its JSON types, so a model
    /// change is still visible but the recorded environment data is not.
    /// </summary>
    private static JToken Blank(JToken token)
    {
        switch (token)
        {
            case JObject obj:
                foreach (var property in obj.Properties().ToArray()) property.Value = Blank(property.Value);
                return obj;
            case JArray array:
                for (var index = 0; index < array.Count; index++) array[index] = Blank(array[index]);
                return array;
            default:
                return token.Type switch
                {
                    JTokenType.Null => JValue.CreateNull(),
                    JTokenType.Integer => 0,
                    JTokenType.Float => 0.0,
                    JTokenType.Boolean => token,
                    _ => DynamicToken
                };
        }
    }

    /// <summary>
    /// Normalises volatile parts of a plain string: the volatile query of an absolute URL - a
    /// pre-signed link carries a date and an expiry, a console link carries resource IDs - and the
    /// random suffix of a resource name the suite generated (for example
    /// <c>C# Auth E2E Environment 5ab1455f3f5f</c>).
    /// </summary>
    private string NormalizeVolatileText(string value)
    {
        if (string.IsNullOrEmpty(value)) return value;
        if (value.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
            value.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return NormalizeUri(value);

        // Only a name the suite generated carries a random token; anything else is left alone so a
        // stable identifier is still compared.
        return value.Contains("e2e", StringComparison.OrdinalIgnoreCase)
            ? GeneratedSuffixRegex().Replace(value, DynamicToken)
            : value;
    }

    /// <summary>
    /// Replaces a volatile value while preserving its JSON type, so a comparison never
    /// reports a spurious type change. Strings are always replaced - a field that is
    /// sometimes empty would otherwise drift between captures - while nulls are kept.
    /// </summary>
    private JToken DynamicValue(JToken value) => value.Type switch
    {
        JTokenType.Null => JValue.CreateNull(),
        JTokenType.String => DynamicToken,
        JTokenType.Integer => 0,
        JTokenType.Float => 0.0,
        // A boolean carries no volatile information and must stay a boolean, otherwise the
        // comparison would report a spurious type change against the Go and PHP snapshots.
        JTokenType.Boolean => value,
        JTokenType.Object => KeyAware(value),
        JTokenType.Array => DynamicArray((JArray)value),
        _ => DynamicToken
    };

    /// <summary>
    /// An array under a dynamic key holds volatile items: a nested container is normalised by
    /// its own keys, while a scalar - an identifier list such as <c>unit_ids</c> has no keys to
    /// go by - is replaced type-preservingly.
    /// </summary>
    private JToken DynamicArray(JArray array)
    {
        for (var index = 0; index < array.Count; index++)
        {
            array[index] = array[index] is JObject or JArray
                ? KeyAware(array[index])
                : DynamicValue(array[index]);
        }

        return array;
    }

    /// <summary>
    /// A container under a dynamic key keeps its shape and its children are normalised by
    /// their own names, so a stable nested field can still register a snapshot difference.
    /// This mirrors PHP's <c>normalizeDynamicValue</c>, which delegates back to the
    /// key-aware normaliser for arrays and objects.
    /// </summary>
    private JToken KeyAware(JToken value) => ProcessToken(value);

    /// <summary>
    /// Detects resource names generated by the test suite itself (e.g. <c>csharp-e2e-1699999</c>,
    /// <c>csharp_saas_auth_e2e_1deea5226e9a</c>). Each live run picks a new random suffix, so these
    /// values must be normalised or every capture would report a breaking change.
    /// </summary>
    private static bool IsGeneratedResourceName(string? value) =>
        !string.IsNullOrEmpty(value) && GeneratedResourceRegex().IsMatch(value);

    /// <summary>
    /// Parses an opaque raw payload and normalises it. A scalar or invalid payload is handled
    /// too, so capture never fails on one.
    /// </summary>
    private JToken ParseRaw(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return JValue.CreateNull();
        try
        {
            var parsed = ProcessToken(JToken.Parse(raw));
            Canonicalize(parsed);
            return parsed;
        }
        catch (Newtonsoft.Json.JsonReaderException)
        {
            return _masker.MaskText(raw);
        }
    }

    /// <summary>
    /// Normalises a logged request URI when it is not covered by the dynamic field set: the
    /// route and the query parameter names are kept so an endpoint or parameter change is
    /// still visible, while volatile path segments and query values become <c>[DYNAMIC]</c>.
    /// </summary>
    internal string NormalizeUri(string? value)
    {
        if (string.IsNullOrEmpty(value)) return value ?? string.Empty;
        var parts = value.Split('?', 2);
        var path = string.Join('/', parts[0].Split('/')
            .Select(segment => IsVolatileUriSegment(segment) ? DynamicToken : _masker.MaskText(segment)));
        if (parts.Length == 1) return path;

        var query = string.Join('&', parts[1].Split('&').Select(pair =>
        {
            var assignment = pair.Split('=', 2);
            if (assignment.Length == 1) return _masker.MaskText(pair);
            // A value the masker already replaced carries no information and must not be replaced
            // again: that would drop the [MASKED] marker the baselines record.
            if (assignment[1].StartsWith('[')) return pair;
            // Credentials can appear in a logged query string, so a sensitive parameter is
            // redacted and only then are volatile values normalised.
            if (_masker.IsSensitiveKey(assignment[0]))
                return $"{assignment[0]}={Masker.MaskValue(assignment[1])}";
            var volatileValue = IsDynamicKey(assignment[0]) ||
                                SignedUrlParameters.Contains(assignment[0], StringComparer.OrdinalIgnoreCase) ||
                                IsVolatileUriSegment(assignment[1]);
            return $"{assignment[0]}={(volatileValue ? DynamicToken : _masker.MaskText(assignment[1]))}";
        }));
        return $"{path}?{query}";
    }

    /// <summary>Query parameters of a signed URL that change on every request.</summary>
    private static readonly string[] SignedUrlParameters =
    {
        "X-Amz-Date", "X-Amz-Expires", "Expires", "X-Amz-Security-Token"
    };

    private static bool IsVolatileUriSegment(string segment) =>
        UuidRegex().IsMatch(segment) || IsGeneratedResourceName(segment);

    [GeneratedRegex("^csharp[-_](?:[a-z]+[-_])*(?:e2e|parity)[-_]|^dotnet[-_]e2e[-_]", RegexOptions.IgnoreCase)]
    private static partial Regex GeneratedResourceRegex();

    [GeneratedRegex("^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")]
    private static partial Regex UuidRegex();

    /// <summary>The random hexadecimal token the suite appends to a generated resource name.</summary>
    [GeneratedRegex(@"\b[0-9a-fA-F]{8,}\b")]
    private static partial Regex GeneratedSuffixRegex();
}
