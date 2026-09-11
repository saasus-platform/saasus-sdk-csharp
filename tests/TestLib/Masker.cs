using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Redacts credentials before anything reaches a log line or a snapshot artifact.
/// The replacement tokens (<c>[MASKED len=N]</c> and <c>[MASKED]</c>) match the Go and
/// PHP SDK test suites so snapshots stay comparable across languages.
/// </summary>
public sealed partial class Masker
{
    public const string MaskedToken = "[MASKED]";

    /// <summary>Key substrings that mark a value as sensitive. Hyphens are normalised to underscores first.</summary>
    private static readonly string[] SensitiveFragments =
    {
        "api_key", "secret", "password", "authorization", "access_token",
        "refresh_token", "id_token", "token", "credential", "stripe_key", "cookie",
        "access_key", "client_secret", "confirmation_code", "mfa_code"
    };

    /// <summary>
    /// The same fragments without separators, so <c>accessKeyId</c> and <c>access_key_id</c>
    /// are both recognised.
    /// </summary>
    private static readonly string[] CompactSensitiveFragments =
        SensitiveFragments.Select(fragment => fragment.Replace("_", string.Empty, StringComparison.Ordinal))
            .Distinct(StringComparer.Ordinal).ToArray();

    /// <summary>
    /// Builds the length-preserving mask used for string values. Go's <c>len(str)</c> and PHP's
    /// <c>strlen</c> both count bytes, so the length is the UTF-8 byte count here too.
    /// </summary>
    public static string MaskValue(string? value) => value switch
    {
        null => MaskedToken,
        "" => string.Empty,
        _ => $"[MASKED len={System.Text.Encoding.UTF8.GetByteCount(value)}]"
    };

    /// <summary>
    /// Redacts free text that is about to be written to assertion messages or console output.
    /// Generated API exceptions and dependency probes can echo a raw HTTP body, so the text must be
    /// masked before it reaches xUnit and CI logs.
    /// </summary>
    public static string Redact(string? text, string fallback = "unknown error") =>
        string.IsNullOrWhiteSpace(text) ? fallback : new Masker().MaskText(text);

    public string MaskText(string? text)
    {
        if (string.IsNullOrEmpty(text)) return text ?? string.Empty;
        var masked = JsonSecretRegex().Replace(text, match =>
            $"{match.Groups[1].Value}\"{MaskValue(match.Groups[2].Value)}\"");
        // A JSON number, boolean or null value is not quoted, so it needs its own pattern or a
        // sensitive value such as {"mfa_code":123456} would pass through unchanged.
        masked = JsonScalarSecretRegex().Replace(masked, match => $"{match.Groups[1].Value}\"{MaskedToken}\"");
        masked = BearerRegex().Replace(masked, $"$1{MaskedToken}");
        // Environment-style assignments (AWS_ACCESS_KEY_ID=..., api_key=...) appear in error
        // messages and command echoes, where no key-based rule can reach them.
        masked = AssignmentRegex().Replace(masked, match => $"{match.Groups[1].Value}={MaskedToken}");
        // Presigned URLs carry the credential, session token and signature in the query
        // string, where no key-based rule can reach them.
        masked = SignedUrlRegex().Replace(masked, match => $"{match.Groups[1].Value}={MaskedToken}");
        return StripeKeyRegex().Replace(masked, MaskedToken);
    }

    public object? Mask(object? value)
    {
        if (value is null) return null;
        var token = value as JToken ?? JToken.FromObject(value);
        token = MaskToken(token);
        return token.ToObject<object>();
    }

    public bool IsSensitiveKey(string key)
    {
        var normalized = key.Replace("-", string.Empty, StringComparison.Ordinal)
                            .Replace("_", string.Empty, StringComparison.Ordinal);
        return CompactSensitiveFragments.Any(fragment =>
            normalized.Contains(fragment, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>Produces the redacted replacement for a sensitive JSON value.</summary>
    public static JToken MaskSensitiveToken(JToken value) => value.Type switch
    {
        JTokenType.String => MaskValue(value.Value<string>()),
        JTokenType.Null => JValue.CreateNull(),
        _ => MaskedToken
    };

    /// <summary>
    /// Redacts a token in place and returns it. A root scalar has no parent and cannot be replaced
    /// in place, so callers that own the root must use the return value.
    /// </summary>
    private JToken MaskToken(JToken token)
    {
        if (token is JObject obj)
        {
            foreach (var property in obj.Properties().ToArray())
            {
                if (IsSensitiveKey(property.Name))
                {
                    property.Value = MaskSensitiveToken(property.Value);
                }
                else
                {
                    MaskToken(property.Value);
                }
            }
        }
        else if (token is JArray array)
        {
            foreach (var item in array.ToArray()) MaskToken(item);
        }
        else if (token.Type == JTokenType.String)
        {
            JToken replacement = MaskText(token.Value<string>());
            if (token.Parent is null) return replacement;
            token.Replace(replacement);
            return replacement;
        }
        return token;
    }

    /// <summary>
    /// A JSON <c>"key": "value"</c> pair whose key names a credential. The value group is
    /// escape-aware, so a secret containing <c>\"</c> or <c>\\</c> is still redacted. The
    /// alternation is kept in sync with <see cref="SensitiveFragments"/>.
    /// </summary>
    [GeneratedRegex("(\\\"[A-Za-z0-9_-]*(?:api[_-]?key|secret|password|authorization|token|credential|cookie|access[_-]?key|stripe[_-]?key|confirmation[_-]?code|mfa[_-]?code)[A-Za-z0-9_-]*\\\"\\s*:\\s*)\\\"((?:[^\\\"\\\\]|\\\\.)*)\\\"", RegexOptions.IgnoreCase)]
    private static partial Regex JsonSecretRegex();

    /// <summary>The same key set with an unquoted JSON value (number, boolean or null).</summary>
    [GeneratedRegex("(\\\"[A-Za-z0-9_-]*(?:api[_-]?key|secret|password|authorization|token|credential|cookie|access[_-]?key|stripe[_-]?key|confirmation[_-]?code|mfa[_-]?code)[A-Za-z0-9_-]*\\\"\\s*:\\s*)(?:-?\\d+(?:\\.\\d+)?(?:[eE][-+]?\\d+)?|true|false|null)", RegexOptions.IgnoreCase)]
    private static partial Regex JsonScalarSecretRegex();

    [GeneratedRegex("(Bearer\\s+)[A-Za-z0-9._~+/-]+=*", RegexOptions.IgnoreCase)]
    private static partial Regex BearerRegex();

    /// <summary>Signed query-string parameters of AWS presigned URLs (SigV4 and SigV2).</summary>
    [GeneratedRegex("(?<=[?&])(X-Amz-Credential|X-Amz-Security-Token|X-Amz-Signature|AWSAccessKeyId|Signature)=[^&\\s\"\\\\]+", RegexOptions.IgnoreCase)]
    private static partial Regex SignedUrlRegex();

    /// <summary>
    /// An environment-style or query-style assignment of a credential. The alternation is kept in
    /// sync with <see cref="SensitiveFragments"/>.
    /// </summary>
    [GeneratedRegex("\\b([A-Za-z0-9_-]*(?:secret|password|authorization|api[_-]?key|access[_-]?key(?:[_-]?id)?|stripe[_-]?key|access[_-]?token|refresh[_-]?token|id[_-]?token|token|credential|cookie|confirmation[_-]?code|mfa[_-]?code)[A-Za-z0-9_-]*)\\s*=\\s*([^\\s&\"';]+)", RegexOptions.IgnoreCase)]
    private static partial Regex AssignmentRegex();

    [GeneratedRegex("sk_(?:test|live)_[A-Za-z0-9]+")]
    private static partial Regex StripeKeyRegex();
}
