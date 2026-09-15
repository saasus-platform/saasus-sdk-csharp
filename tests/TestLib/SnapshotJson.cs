using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Shared JSON contract for every snapshot artifact.
/// The wire format intentionally mirrors the Go and PHP SDK test suites
/// (snake_case keys, lower-case status strings, upper-case capture levels and
/// nanosecond durations) so artifacts can be diffed across languages.
/// </summary>
public static class SnapshotJson
{
    /// <summary>One CLR tick is 100 nanoseconds.</summary>
    public const long NanosecondsPerTick = 100;

    public static long ToNanoseconds(TimeSpan duration) => duration.Ticks * NanosecondsPerTick;

    public static TimeSpan FromNanoseconds(long nanoseconds) => TimeSpan.FromTicks(nanoseconds / NanosecondsPerTick);

    public static readonly JsonSerializerSettings Settings = Create();

    /// <summary>
    /// A fresh serializer per conversion: <see cref="JsonSerializer"/> is not thread-safe and the
    /// test classes run in parallel, so a shared instance could produce flaky tokens.
    /// </summary>
    public static JsonSerializer CreateSerializer() => JsonSerializer.Create(Settings);

    private static JsonSerializerSettings Create()
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include,
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            // An API payload string that merely looks like a date must stay a string: converting it
            // would change the offset or fractional precision on re-serialization and hide a
            // string-format change from the comparer. Declared DateTimeOffset members are still
            // converted, because their contract type drives the conversion.
            DateParseHandling = DateParseHandling.None,
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(processDictionaryKeys: false, overrideSpecifiedNames: false)
            }
        };
        // The upper-case converter must be registered first: StringEnumConverter matches every enum.
        settings.Converters.Add(new UpperCaseEnumConverter<CaptureLevel>());
        settings.Converters.Add(new StringEnumConverter(new SnakeCaseNamingStrategy()));
        // Dictionary enumeration order is not stable, so keys are emitted in ordinal order.
        settings.Converters.Add(new OrderedDictionaryConverter());
        return settings;
    }

    /// <summary>Serializes an artifact using the cross-language contract, with a trailing newline like the PHP SDK.</summary>
    public static string Serialize(object value) =>
        JsonConvert.SerializeObject(value, Settings) + "\n";

    public static T? Deserialize<T>(string json) => JsonConvert.DeserializeObject<T>(json, Settings);
}

/// <summary>Serializes an enum using upper-case names, matching Go's <c>CaptureLevel</c> constants.</summary>
public sealed class UpperCaseEnumConverter<T> : JsonConverter where T : struct, Enum
{
    public override bool CanConvert(Type objectType) =>
        objectType == typeof(T) || objectType == typeof(T?);

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null) writer.WriteNull();
        else writer.WriteValue(value.ToString()!.ToUpperInvariant());
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var text = reader.Value?.ToString();
        if (string.IsNullOrWhiteSpace(text)) return default(T);
        if (!Enum.TryParse<T>(text, true, out var parsed))
            throw new JsonSerializationException($"Unsupported {typeof(T).Name}: {text}.");
        return parsed;
    }
}

/// <summary>
/// Serializes string-keyed dictionaries with ordinal-ordered keys. Dictionary
/// enumeration order is not stable across runs, so two equivalent captures would
/// otherwise produce different snapshot bytes and noisy baseline diffs. Records keep
/// their declaration order because only dictionary types are handled here.
/// </summary>
public sealed class OrderedDictionaryConverter : JsonConverter
{
    public override bool CanRead => false;

    public override bool CanConvert(Type objectType) =>
        !typeof(Newtonsoft.Json.Linq.JToken).IsAssignableFrom(objectType) &&
        objectType.GetInterfaces().Append(objectType).Any(IsStringKeyedDictionary);

    private static bool IsStringKeyedDictionary(Type type) =>
        type.IsGenericType &&
        (type.GetGenericTypeDefinition() == typeof(IDictionary<,>) ||
         type.GetGenericTypeDefinition() == typeof(IReadOnlyDictionary<,>)) &&
        type.GetGenericArguments()[0] == typeof(string);

    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
    {
        if (value is null) { writer.WriteNull(); return; }
        var entries = new List<KeyValuePair<string, object?>>();
        foreach (var entry in (System.Collections.IEnumerable)value)
        {
            var type = entry?.GetType();
            var key = type?.GetProperty("Key")?.GetValue(entry)?.ToString();
            if (key is null) continue;
            entries.Add(new KeyValuePair<string, object?>(key, type!.GetProperty("Value")?.GetValue(entry)));
        }

        writer.WriteStartObject();
        foreach (var entry in entries.OrderBy(entry => entry.Key, StringComparer.Ordinal))
        {
            writer.WritePropertyName(entry.Key);
            serializer.Serialize(writer, entry.Value);
        }
        writer.WriteEndObject();
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) =>
        throw new NotSupportedException("Deserialization uses the default dictionary contract.");
}
