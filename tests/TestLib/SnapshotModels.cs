using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SaasusSdk.Tests.TestLib;

public enum SnapshotMode { Capture, Compare, Report, Full }
public enum CaptureLevel { Full, Story, Step, Response }
public enum SnapshotComparisonMode { Release, Manual, Skip }
public enum DynamicFieldMode { Replace, Exclude }
public enum CompatibilityLevel { Compatible, Warning, Breaking }

/// <summary>Enabled flag plus severity for a single validation rule, mirroring Go/PHP <c>validation_rules</c>.</summary>
public sealed record ValidationRule(bool Enabled, string Severity);

public sealed class SnapshotConfig
{
    /// <summary>Rule defaults shared with the Go and PHP suites.</summary>
    public static readonly IReadOnlyDictionary<string, ValidationRule> DefaultValidationRules =
        new Dictionary<string, ValidationRule>(StringComparer.OrdinalIgnoreCase)
        {
            ["completion"] = new(true, "error"),
            ["sequence"] = new(true, "error"),
            ["state_transition"] = new(true, "warning"),
            ["timing"] = new(false, "info")
        };

    public const string SeverityError = "error";
    public const string SeverityWarning = "warning";
    public const string SeverityInfo = "info";

    public static readonly IReadOnlyList<string> Severities = new[] { SeverityError, SeverityWarning, SeverityInfo };

    public SnapshotMode Mode { get; init; } = SnapshotMode.Capture;
    public string OutputDirectory { get; init; } = "tests/E2E/Snapshots";
    public string ModuleName { get; init; } = "billing";
    public string FileNameFormat { get; init; } = "story_snapshot_{tag}_{story_name}.json";
    public CaptureLevel CaptureLevel { get; init; } = CaptureLevel.Full;
    public SnapshotComparisonMode ComparisonMode { get; init; } = SnapshotComparisonMode.Release;
    public string CurrentTag { get; init; } = string.Empty;
    public string OldTag { get; init; } = string.Empty;
    public string NewTag { get; init; } = string.Empty;
    public string SdkVersion { get; init; } = "unknown";
    public string TestEnvironment { get; init; } = "dev";
    public IReadOnlyList<string> StoryFilters { get; init; } = Array.Empty<string>();
    public bool Verbose { get; init; }
    public bool FailOnBreaking { get; init; } = true;
    public bool ValidationEnabled { get; init; } = true;
    public bool ValidateCompletion { get; init; } = true;
    public bool ValidateSequence { get; init; } = true;
    public bool ValidateStateTransitions { get; init; } = true;
    public bool ValidateTiming { get; init; }

    /// <summary>Per-rule overrides parsed from <c>validation_rules</c> in the JSON config file.</summary>
    public IReadOnlyDictionary<string, ValidationRule> ValidationRuleOverrides { get; init; } =
        new Dictionary<string, ValidationRule>(StringComparer.OrdinalIgnoreCase);

    public bool CaptureFailedStories { get; init; }
    public bool Overwrite { get; init; }
    public IReadOnlyCollection<string> DynamicFields { get; init; } = Array.Empty<string>();
    public bool UseDefaultDynamicFields { get; init; } = true;
    public DynamicFieldMode DynamicFieldMode { get; init; } = DynamicFieldMode.Replace;

    /// <summary>Number of validation artifacts retained per story, matching Go/PHP retention of the two newest.</summary>
    public int ValidationHistoryLimit { get; init; } = 2;

    public bool EnableCapture => Mode is SnapshotMode.Capture or SnapshotMode.Full;
    public bool EnableComparison => Mode is SnapshotMode.Compare or SnapshotMode.Full;
    public bool EnableReporting => Mode is SnapshotMode.Report or SnapshotMode.Full;
    public string ModuleDirectory => Path.Combine(OutputDirectory, Slug(ModuleName));

    /// <summary>Resolves the effective enabled flag and severity for a validation rule.</summary>
    public ValidationRule Rule(string name)
    {
        if (ValidationRuleOverrides.TryGetValue(name, out var overridden))
            // The configuration accepts any casing, but findings are counted by exact value, so
            // the severity must be canonicalised here or an error would be counted as nothing.
            return overridden with { Severity = NormalizeSeverity(overridden.Severity) };
        var severity = DefaultValidationRules.TryGetValue(name, out var fallback) ? fallback.Severity : SeverityError;
        var enabled = name.ToLowerInvariant() switch
        {
            "completion" => ValidateCompletion,
            "sequence" => ValidateSequence,
            "state_transition" => ValidateStateTransitions,
            "timing" => ValidateTiming,
            _ => DefaultValidationRules.TryGetValue(name, out var rule) && rule.Enabled
        };
        return new(enabled, severity);
    }

    /// <summary>Maps a configured severity onto its canonical lower-case spelling.</summary>
    public static string NormalizeSeverity(string severity) =>
        Severities.FirstOrDefault(known => string.Equals(known, severity, StringComparison.OrdinalIgnoreCase))
        ?? throw new InvalidOperationException($"Unsupported validation severity: {severity}.");

    public string ResolveCurrentTag() => Slug(!string.IsNullOrWhiteSpace(CurrentTag)
        ? CurrentTag
        // The fallback must be Gregorian with ASCII digits, or the generated tag would not
        // resolve the intended snapshot files under a non-Gregorian culture. The layout matches
        // Go's "20060102-150405" and PHP's gmdate('Ymd-His').
        : GitInfo.ExactTag() ?? GitInfo.Describe()
            ?? $"dev-{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture)}");

    public bool MatchesStory(string storyName) => StoryFilters.Count == 0 ||
        StoryFilters.Any(filter => storyName.Contains(filter, StringComparison.OrdinalIgnoreCase) ||
            string.Equals(StorySlug(storyName), StorySlug(filter), StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Copies this configuration for a specific snapshot module. Every module test needs its own
    /// module directory, otherwise the shared default (<c>billing</c>) would route another
    /// module's captures and comparisons at the Billing baselines.
    /// </summary>
    public SnapshotConfig WithModule(string moduleName) => new()
    {
        Mode = Mode,
        OutputDirectory = OutputDirectory,
        ModuleName = moduleName,
        FileNameFormat = FileNameFormat,
        CaptureLevel = CaptureLevel,
        ComparisonMode = ComparisonMode,
        CurrentTag = CurrentTag,
        OldTag = OldTag,
        NewTag = NewTag,
        SdkVersion = SdkVersion,
        TestEnvironment = TestEnvironment,
        StoryFilters = StoryFilters,
        Verbose = Verbose,
        FailOnBreaking = FailOnBreaking,
        ValidationEnabled = ValidationEnabled,
        ValidateCompletion = ValidateCompletion,
        ValidateSequence = ValidateSequence,
        ValidateStateTransitions = ValidateStateTransitions,
        ValidateTiming = ValidateTiming,
        ValidationRuleOverrides = ValidationRuleOverrides,
        ValidationHistoryLimit = ValidationHistoryLimit,
        CaptureFailedStories = CaptureFailedStories,
        Overwrite = Overwrite,
        DynamicFields = DynamicFields,
        UseDefaultDynamicFields = UseDefaultDynamicFields,
        DynamicFieldMode = DynamicFieldMode
    };

    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(OutputDirectory)) throw new InvalidOperationException("Snapshot output directory is required.");
        if (string.IsNullOrWhiteSpace(ModuleName)) throw new InvalidOperationException("Snapshot module name is required.");
        // Slug keeps '.' and '-', so a module name such as ".." would resolve outside the output
        // directory once combined into ModuleDirectory.
        var moduleSlug = Slug(ModuleName);
        if (moduleSlug.Trim('.').Length == 0 || moduleSlug.Contains(Path.DirectorySeparatorChar) ||
            moduleSlug.Contains(Path.AltDirectorySeparatorChar))
            throw new InvalidOperationException($"Unsupported snapshot module name: {ModuleName}.");
        // A programmatically built configuration can hold an out-of-range enum, which would silently
        // disable capture or comparison instead of failing fast.
        if (!Enum.IsDefined(Mode)) throw new InvalidOperationException($"Unsupported snapshot mode: {Mode}.");
        if (!Enum.IsDefined(CaptureLevel)) throw new InvalidOperationException($"Unsupported snapshot capture level: {CaptureLevel}.");
        if (!Enum.IsDefined(ComparisonMode)) throw new InvalidOperationException($"Unsupported snapshot comparison mode: {ComparisonMode}.");
        if (!Enum.IsDefined(DynamicFieldMode)) throw new InvalidOperationException($"Unsupported snapshot dynamic field mode: {DynamicFieldMode}.");
        if (string.IsNullOrWhiteSpace(FileNameFormat)) throw new InvalidOperationException("Snapshot file name format is required.");
        // Without both placeholders several tags or stories map to one path, which silently
        // overwrites baselines when overwrite is enabled.
        if (!FileNameFormat.Contains("{tag}", StringComparison.Ordinal) ||
            !FileNameFormat.Contains("{story_name}", StringComparison.Ordinal))
            throw new InvalidOperationException(
                "Snapshot file name format must contain both {tag} and {story_name}.");
        // A format carrying a directory would write outside the snapshot directory that discovery
        // searches, making the captured baseline undiscoverable.
        if (Path.IsPathRooted(FileNameFormat) ||
            FileNameFormat.Contains(Path.DirectorySeparatorChar) ||
            FileNameFormat.Contains(Path.AltDirectorySeparatorChar) ||
            FileNameFormat.Contains("..", StringComparison.Ordinal))
            throw new InvalidOperationException(
                "Snapshot file name format must be a file name, not a path.");
        if (ValidationHistoryLimit < 1) throw new InvalidOperationException("Snapshot validation history limit must be at least 1.");
        foreach (var (name, rule) in ValidationRuleOverrides)
        {
            if (!DefaultValidationRules.ContainsKey(name))
                throw new InvalidOperationException($"Unsupported validation rule: {name}.");
            if (!Severities.Contains(rule.Severity, StringComparer.OrdinalIgnoreCase))
                throw new InvalidOperationException($"Unsupported validation severity: {rule.Severity}.");
        }
        // Full mode captures the candidate itself, so ResolveTags can use the current tag; only a
        // pure compare or report run needs both tags supplied up front.
        if (ComparisonMode == SnapshotComparisonMode.Manual &&
            Mode is SnapshotMode.Compare or SnapshotMode.Report &&
            (string.IsNullOrWhiteSpace(OldTag) || string.IsNullOrWhiteSpace(NewTag)))
            throw new InvalidOperationException("Manual comparison requires E2E_SNAPSHOT_OLD_TAG and E2E_SNAPSHOT_NEW_TAG.");
        if (ComparisonMode == SnapshotComparisonMode.Manual && Mode == SnapshotMode.Full &&
            string.IsNullOrWhiteSpace(OldTag))
            throw new InvalidOperationException("Manual comparison requires E2E_SNAPSHOT_OLD_TAG.");
    }

    /// <summary>
    /// Slug for tags and module names: <c>.</c>, <c>-</c> and <c>_</c> survive so a tag such as
    /// <c>v1.0.0-8-gbcd96cd</c> stays readable. Mirrors PHP's <c>SnapshotConfig::slug</c>.
    /// </summary>
    public static string Slug(string value) => Slug(value, keepPunctuation: true);

    /// <summary>
    /// Slug for story names: every non-alphanumeric character collapses to <c>_</c>, matching
    /// PHP's <c>storySlug</c> and Go's <c>sanitizeFileName</c>, so artifact names stay
    /// comparable across languages.
    /// </summary>
    public static string StorySlug(string value) => Slug(value, keepPunctuation: false);

    private static string Slug(string value, bool keepPunctuation)
    {
        var chars = value.ToLowerInvariant().Select(character =>
            char.IsLetterOrDigit(character) || (keepPunctuation && character is '.' or '-')
                ? character
                : '_').ToArray();
        var slug = new string(chars).Trim('_');
        while (slug.Contains("__", StringComparison.Ordinal)) slug = slug.Replace("__", "_", StringComparison.Ordinal);
        return string.IsNullOrEmpty(slug) ? "snapshot" : slug;
    }
}

// ---------------------------------------------------------------------------
// Snapshot artifact (schema shared with saasus-sdk-go and saasus-sdk-php)
// ---------------------------------------------------------------------------

public sealed record SnapshotMetadata(
    string SdkVersion,
    string TestEnvironment,
    CaptureLevel CaptureLevel,
    string GitTag,
    string GitCommit);

public sealed record SnapshotError(
    string Type,
    string Message,
    [property: JsonProperty("details", NullValueHandling = NullValueHandling.Ignore)] string? Details = null);

public sealed record SnapshotHttpResponse(
    int StatusCode,
    string Status,
    IReadOnlyDictionary<string, string> Headers,
    long ContentLength,
    [property: JsonProperty("trace_id", NullValueHandling = NullValueHandling.Ignore)] string? TraceId = null);

public sealed record SnapshotReturnValue(
    string Type,
    int StatusCode,
    string Status,
    SnapshotHttpResponse? HttpResponse,
    JToken? JsonData,
    string Body,
    IReadOnlyDictionary<string, string> Headers);

public sealed record SnapshotSummary(
    int TotalSteps,
    int SuccessfulSteps,
    int FailedSteps,
    int SkippedSteps,
    [property: JsonProperty("total_duration")] long TotalDurationNanoseconds,
    [property: JsonProperty("average_step_duration")] long AverageStepDurationNanoseconds);

public sealed record StepSnapshot(
    string StepName,
    string Method,
    JToken Parameters,
    SnapshotReturnValue? ReturnValue,
    [property: JsonProperty("duration")] long DurationNanoseconds,
    // 0 means "no HTTP status was observable", matching Go's int field.
    int StatusCode,
    bool Success,
    TestStatus Status,
    [property: JsonProperty("skip_reason", NullValueHandling = NullValueHandling.Ignore)] string? SkipReason,
    [property: JsonProperty("error", NullValueHandling = NullValueHandling.Ignore)] SnapshotError? Error,
    DateTimeOffset Timestamp,
    [property: JsonProperty("state_changes", NullValueHandling = NullValueHandling.Ignore)] IReadOnlyDictionary<string, object?>? StateChanges,
    // C#-specific extras: the .NET SDK exposes four call styles per method.
    CallStyle CallStyle = CallStyle.Async,
    int Attempts = 1);

public sealed record StorySnapshot(
    string StoryName,
    string Description,
    DateTimeOffset Timestamp,
    [property: JsonProperty("duration")] long DurationNanoseconds,
    TestStatus Status,
    IReadOnlyDictionary<string, object?> Variables,
    IReadOnlyList<StepSnapshot> Steps,
    SnapshotSummary Summary,
    SnapshotMetadata Metadata);

// ---------------------------------------------------------------------------
// Comparison artifact
// ---------------------------------------------------------------------------

public sealed record CompatibilityIssue(
    string Type,
    string Path,
    string Description,
    JToken? OldValue,
    JToken? NewValue,
    [property: JsonProperty("impact")] CompatibilityLevel Level);

public sealed record CompatibilityVerdict(CompatibilityLevel Level, bool Passed);

public sealed record ComparisonSummary(int Differences, int Warnings, int BreakingChanges);

public sealed record SnapshotComparison(
    string StoryName,
    string OldTag,
    string NewTag,
    [property: JsonIgnore] IReadOnlyList<CompatibilityIssue> Issues)
{
    [JsonProperty("compatibility")]
    public CompatibilityVerdict Compatibility => new(Level, IsCompatible);

    [JsonProperty("summary")]
    public ComparisonSummary Summary => new(
        Issues.Count,
        Issues.Count(x => x.Level == CompatibilityLevel.Warning),
        Issues.Count(x => x.Level == CompatibilityLevel.Breaking));

    [JsonProperty("differences")]
    public IReadOnlyList<CompatibilityIssue> Differences => Issues;

    [JsonIgnore]
    public CompatibilityLevel Level => Issues.Any(x => x.Level == CompatibilityLevel.Breaking)
        ? CompatibilityLevel.Breaking : Issues.Any(x => x.Level == CompatibilityLevel.Warning)
            ? CompatibilityLevel.Warning : CompatibilityLevel.Compatible;

    [JsonIgnore]
    public bool IsCompatible => Level != CompatibilityLevel.Breaking;

    [JsonConstructor]
    private SnapshotComparison(string storyName, string oldTag, string newTag,
        IReadOnlyList<CompatibilityIssue>? differences, IReadOnlyList<CompatibilityIssue>? issues)
        : this(storyName, oldTag, newTag, differences ?? issues ?? Array.Empty<CompatibilityIssue>()) { }
}

// ---------------------------------------------------------------------------
// Validation artifact
// ---------------------------------------------------------------------------

public sealed record ValidationFinding(
    string Type,
    string StepName,
    string Message,
    string Severity,
    [property: JsonProperty("expected_value", NullValueHandling = NullValueHandling.Ignore)] object? ExpectedValue = null,
    [property: JsonProperty("actual_value", NullValueHandling = NullValueHandling.Ignore)] object? ActualValue = null);

public sealed record SkippedStepInfo(
    string StepName,
    string Method,
    [property: JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)] string? Reason = null);

public sealed record ValidationSummary(int TotalErrors, int TotalWarnings, int TotalInfo, bool IsValid);

/// <summary>Run-over-run validation delta, mirroring Go's <c>ValidationComparison</c>.</summary>
public sealed record ValidationHistory(
    [property: JsonProperty("previous_file", NullValueHandling = NullValueHandling.Ignore)] string? PreviousFile,
    [property: JsonProperty("previous_validation_time", NullValueHandling = NullValueHandling.Ignore)] DateTimeOffset? PreviousValidationTime,
    [property: JsonProperty("new_findings", NullValueHandling = NullValueHandling.Ignore)] IReadOnlyList<ValidationFinding>? NewFindings,
    [property: JsonProperty("resolved_findings", NullValueHandling = NullValueHandling.Ignore)] IReadOnlyList<ValidationFinding>? ResolvedFindings,
    int ErrorCountDelta,
    int WarningCountDelta,
    int InfoCountDelta);

public sealed record SnapshotValidation(
    string StoryName,
    DateTimeOffset ValidationTime,
    bool IsValid,
    string CompletionStatus,
    IReadOnlyList<ValidationFinding>? SequenceErrors,
    IReadOnlyList<ValidationFinding>? StateTransitionErrors,
    IReadOnlyList<ValidationFinding>? TimingErrors,
    [property: JsonProperty("skipped_steps", NullValueHandling = NullValueHandling.Ignore)] IReadOnlyList<SkippedStepInfo>? SkippedSteps,
    [property: JsonProperty("comparison", NullValueHandling = NullValueHandling.Ignore)] ValidationHistory? Comparison,
    ValidationSummary Summary)
{
    /// <summary>Flattened view over the three finding buckets.</summary>
    [JsonIgnore]
    public IReadOnlyList<ValidationFinding> Findings =>
        (SequenceErrors ?? Array.Empty<ValidationFinding>())
        .Concat(StateTransitionErrors ?? Array.Empty<ValidationFinding>())
        .Concat(TimingErrors ?? Array.Empty<ValidationFinding>())
        .ToArray();
}

public sealed record SnapshotRunReport(string Mode, string OldTag, string NewTag, int Snapshots,
    CompatibilityLevel Compatibility, IReadOnlyList<SnapshotComparison> Comparisons,
    IReadOnlyList<SnapshotValidation> Validations);
