using Newtonsoft.Json;

namespace SaasusSdk.Tests.TestLib;

public sealed class SnapshotStore
{
    public SnapshotStore(SnapshotConfig config) { Config = config; Config.Validate(); }
    public SnapshotConfig Config { get; }

    public string SnapshotDirectory => Path.Combine(Config.ModuleDirectory, "story_snapshots", "tags");
    public string ValidationDirectory => Path.Combine(Config.ModuleDirectory, "story_validations");

    public string SnapshotPath(string tag, string story) => Path.Combine(SnapshotDirectory, Config.FileNameFormat
        .Replace("{tag}", SnapshotConfig.Slug(tag), StringComparison.Ordinal)
        .Replace("{story_name}", SnapshotConfig.StorySlug(story), StringComparison.Ordinal));
    public static string ValidationFileName(string tag, string story) =>
        $"story_validation_{SnapshotConfig.StorySlug(story)}_{SnapshotConfig.Slug(tag)}.json";
    public string ValidationPath(string tag, string story) =>
        Path.Combine(ValidationDirectory, ValidationFileName(tag, story));
    public string ComparisonPath(string oldTag, string newTag, string story) => Path.Combine(Config.ModuleDirectory,
        "story_comparisons", $"{SnapshotConfig.Slug(oldTag)}_vs_{SnapshotConfig.Slug(newTag)}", $"{SnapshotConfig.StorySlug(story)}.json");
    public string ReportPath(string oldTag, string newTag, string story, string extension) => Path.Combine(Config.ModuleDirectory,
        "story_reports", $"{SnapshotConfig.Slug(oldTag)}_vs_{SnapshotConfig.Slug(newTag)}", $"{SnapshotConfig.StorySlug(story)}.{extension}");

    public async Task SaveSnapshotAsync(StorySnapshot snapshot, string tag, CancellationToken token = default)
        => await WriteJsonAsync(SnapshotPath(tag, snapshot.StoryName), snapshot, Config.Overwrite, token);

    public async Task<StorySnapshot> LoadSnapshotAsync(string tag, string story, CancellationToken token = default)
    {
        var snapshot = await ReadJsonAsync<StorySnapshot>(SnapshotPath(tag, story), token);
        return snapshot with
        {
            Steps = snapshot.Steps.Select(step => step with
            {
                Parameters = step.Parameters ?? new Newtonsoft.Json.Linq.JObject()
            }).ToArray()
        };
    }

    /// <summary>
    /// Persists a validation artifact, embedding the delta against the previous run and
    /// pruning older artifacts, mirroring Go's <c>SaveStoryValidation</c> and PHP's
    /// <c>retainLatestValidations</c>. Returns the artifact that was written.
    /// </summary>
    public async Task<SnapshotValidation> SaveValidationAsync(SnapshotValidation value, string tag, CancellationToken token = default)
    {
        var path = ValidationPath(tag, value.StoryName);
        var previous = LoadPreviousValidation(value.StoryName, path);
        var enriched = value with { Comparison = BuildHistory(previous.Validation, previous.FileName, value) };
        await WriteJsonAsync(path, enriched, true, token);
        RetainLatestValidations(value.StoryName, Config.ValidationHistoryLimit);
        return enriched;
    }

    public async Task<SnapshotValidation?> LoadValidationAsync(string tag, string story, CancellationToken token = default)
    {
        var path = ValidationPath(tag, story);
        return File.Exists(path) ? await ReadJsonAsync<SnapshotValidation>(path, token) : null;
    }

    public Task SaveComparisonAsync(SnapshotComparison value, CancellationToken token = default) =>
        WriteJsonAsync(ComparisonPath(value.OldTag, value.NewTag, value.StoryName), value, true, token);

    /// <summary>
    /// Writes the paired report documents. The JSON document carries the validation artifact
    /// alongside the comparison, so machine consumers see the same data as the HTML page.
    /// </summary>
    public Task SaveReportAsync(SnapshotComparison value, string json, string html, CancellationToken token = default) => Task.WhenAll(
        WriteTextAsync(ReportPath(value.OldTag, value.NewTag, value.StoryName, "json"), json, true, token),
        WriteTextAsync(ReportPath(value.OldTag, value.NewTag, value.StoryName, "html"), html, true, token));

    public IReadOnlyList<string> AvailableTags()
    {
        if (!Directory.Exists(SnapshotDirectory)) return Array.Empty<string>();
        return Directory.GetFiles(SnapshotDirectory, SnapshotSearchPattern("*"))
            .Select(path => new { Path = path, Snapshot = ReadSnapshot(path) })
            .Where(item => !string.IsNullOrWhiteSpace(item.Snapshot.Metadata.GitTag))
            // A stale artifact filed under another tag's name is not discoverable through
            // StoriesForTag, so advertising its recorded tag would yield an empty comparison.
            .Where(item => IsFiledUnder(item.Path, item.Snapshot.Metadata.GitTag, item.Snapshot.StoryName))
            // Tags are addressed through their slug everywhere else (file names, comparisons),
            // so a tag such as release/1.0 must be grouped as release_1.0 here too.
            .GroupBy(item => SnapshotConfig.Slug(item.Snapshot.Metadata.GitTag), StringComparer.Ordinal)
            // A legacy or partially written artifact deserializes to the default timestamp, which
            // would order tags lexicographically instead of by capture time.
            .Select(group => new
            {
                Tag = group.Key,
                Time = group.Max(item => item.Snapshot.Timestamp == default
                    ? new DateTimeOffset(File.GetLastWriteTimeUtc(item.Path), TimeSpan.Zero)
                    : item.Snapshot.Timestamp)
            })
            // The tag is the tie-breaker: file enumeration order is unspecified, so equal or
            // missing timestamps would otherwise select a different baseline on each run.
            .OrderBy(item => item.Time).ThenBy(item => item.Tag, StringComparer.Ordinal)
            .Select(item => item.Tag).ToArray();
    }

    /// <summary>True when the artifact lives at the path its own tag and story name imply.</summary>
    private bool IsFiledUnder(string path, string gitTag, string storyName) =>
        string.Equals(Path.GetFullPath(path), Path.GetFullPath(SnapshotPath(gitTag, storyName)), StringComparison.Ordinal);

    public IReadOnlyList<string> StoriesForTag(string tag)
    {
        if (!Directory.Exists(SnapshotDirectory)) return Array.Empty<string>();
        var slug = SnapshotConfig.Slug(tag);
        return Directory.GetFiles(SnapshotDirectory, SnapshotSearchPattern(slug))
            .Select(ReadSnapshot)
            // A blank recorded tag is not a wildcard, and Slug maps it to "snapshot", so the
            // emptiness has to be rejected explicitly before the slugs are compared.
            .Where(snapshot => !string.IsNullOrWhiteSpace(snapshot.StoryName) &&
                               !string.IsNullOrWhiteSpace(snapshot.Metadata.GitTag) &&
                               string.Equals(SnapshotConfig.Slug(snapshot.Metadata.GitTag), slug, StringComparison.Ordinal))
            .Select(snapshot => snapshot.StoryName)
            // Directory enumeration order is unspecified, so the comparison list and the serialized
            // run report would otherwise differ between filesystems.
            .Distinct(StringComparer.Ordinal)
            .OrderBy(SnapshotConfig.StorySlug, StringComparer.Ordinal)
            .ToArray();
    }

    /// <summary>
    /// Reads a snapshot for discovery. A malformed or semantically empty artifact is reported
    /// instead of being treated as absent, which would silently select the wrong baseline.
    /// </summary>
    private static StorySnapshot ReadSnapshot(string path)
    {
        try
        {
            var snapshot = SnapshotJson.Deserialize<StorySnapshot>(File.ReadAllText(path))
                ?? throw new InvalidDataException($"Snapshot artifact contains no document: {path}.");
            // Incomplete JSON such as "{}" deserializes with null members, which would then be
            // dereferenced during discovery instead of reporting the corrupt artifact.
            if (snapshot.Metadata is null || snapshot.Summary is null || snapshot.Steps is null)
                throw new InvalidDataException($"Snapshot artifact is missing required members: {path}.");
            return snapshot;
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException($"Malformed snapshot artifact: {path}.", exception);
        }
    }

    /// <summary>Validation artifacts for a story, newest first.</summary>
    public IReadOnlyList<string> ValidationFiles(string story)
    {
        if (!Directory.Exists(ValidationDirectory)) return Array.Empty<string>();
        var slug = SnapshotConfig.StorySlug(story);
        return Directory.GetFiles(ValidationDirectory, $"story_validation_{slug}_*.json")
            // The glob for story "a" also matches "a_b", so the recorded story name decides.
            // Otherwise another story's baseline could be compared against, or pruned.
            .Where(path => IsValidationFileFor(path, slug))
            .OrderByDescending(File.GetLastWriteTimeUtc)
            .ThenByDescending(path => path, StringComparer.Ordinal)
            .ToArray();
    }

    private static bool IsValidationFileFor(string path, string storySlug)
    {
        try
        {
            var validation = SnapshotJson.Deserialize<SnapshotValidation>(File.ReadAllText(path))
                ?? throw new InvalidDataException($"Validation artifact contains no document: {path}.");
            if (validation.Summary is null || validation.StoryName is null)
                throw new InvalidDataException($"Validation artifact is missing required members: {path}.");
            return string.Equals(SnapshotConfig.StorySlug(validation.StoryName), storySlug, StringComparison.Ordinal);
        }
        catch (JsonException exception)
        {
            throw new InvalidDataException($"Malformed validation artifact: {path}.", exception);
        }
    }

    private (SnapshotValidation? Validation, string? FileName) LoadPreviousValidation(string story, string currentPath)
    {
        foreach (var path in ValidationFiles(story))
        {
            if (string.Equals(Path.GetFullPath(path), Path.GetFullPath(currentPath), StringComparison.Ordinal)) continue;
            SnapshotValidation? validation;
            try
            {
                validation = SnapshotJson.Deserialize<SnapshotValidation>(File.ReadAllText(path));
            }
            catch (JsonException exception)
            {
                // Silently skipping a corrupt artifact would compute the history against an older
                // baseline and hide the corruption.
                throw new InvalidDataException($"Malformed validation artifact: {path}.", exception);
            }
            if (validation is not null) return (validation, Path.GetFileName(path));
        }
        return (null, null);
    }

    /// <summary>Computes the run-over-run finding delta keyed by type, step, message and severity.</summary>
    internal static ValidationHistory? BuildHistory(SnapshotValidation? previous, string? previousFile, SnapshotValidation current)
    {
        if (previous is null) return null;
        static string Key(ValidationFinding finding) =>
            $"{finding.Type}|{finding.StepName}|{finding.Message}|{finding.Severity}";

        // Two findings can legitimately share a story, step, message and severity (a repeated
        // step name, or the same failure twice), so the delta is computed as a multiset.
        var previousFindings = previous.Findings;
        var currentFindings = current.Findings;
        var previousCounts = CountByKey(previousFindings, Key);
        var currentCounts = CountByKey(currentFindings, Key);
        var added = currentFindings.Where(finding => !Consume(previousCounts, Key(finding))).ToArray();
        var resolved = previousFindings.Where(finding => !Consume(currentCounts, Key(finding))).ToArray();
        return new ValidationHistory(
            previousFile,
            previous.ValidationTime,
            added.Length == 0 ? null : added,
            resolved.Length == 0 ? null : resolved,
            current.Summary.TotalErrors - previous.Summary.TotalErrors,
            current.Summary.TotalWarnings - previous.Summary.TotalWarnings,
            current.Summary.TotalInfo - previous.Summary.TotalInfo);
    }

    private static Dictionary<string, int> CountByKey(
        IReadOnlyList<ValidationFinding> findings, Func<ValidationFinding, string> key)
    {
        var counts = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var finding in findings)
            counts[key(finding)] = counts.TryGetValue(key(finding), out var count) ? count + 1 : 1;
        return counts;
    }

    /// <summary>Takes one occurrence of <paramref name="key"/>; false when none is left.</summary>
    private static bool Consume(Dictionary<string, int> counts, string key)
    {
        if (!counts.TryGetValue(key, out var count) || count == 0) return false;
        counts[key] = count - 1;
        return true;
    }

    private void RetainLatestValidations(string story, int keep)
    {
        foreach (var path in ValidationFiles(story).Skip(keep))
        {
            try { File.Delete(path); }
            catch (IOException exception)
            { throw new IOException($"Unable to prune stale validation artifact: {path}", exception); }
        }
    }

    private string SnapshotSearchPattern(string tag) => Config.FileNameFormat
        .Replace("{tag}", tag, StringComparison.Ordinal)
        .Replace("{story_name}", "*", StringComparison.Ordinal);

    private static async Task<T> ReadJsonAsync<T>(string path, CancellationToken token)
    {
        if (!File.Exists(path)) throw new FileNotFoundException("Snapshot artifact not found.", path);
        var json = await File.ReadAllTextAsync(path, token);
        return SnapshotJson.Deserialize<T>(json) ?? throw new InvalidDataException($"Invalid JSON: {path}");
    }

    private static async Task WriteJsonAsync(string path, object value, bool overwrite, CancellationToken token) =>
        await WriteTextAsync(path, SnapshotJson.Serialize(value), overwrite, token);

    private static async Task WriteTextAsync(string path, string text, bool overwrite, CancellationToken token)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        if (!overwrite && File.Exists(path)) throw new IOException($"Artifact already exists: {path}. Set E2E_SNAPSHOT_OVERWRITE=true to replace it.");
        var temporary = path + $".{Guid.NewGuid():N}.tmp";
        try { await File.WriteAllTextAsync(temporary, text, token); File.Move(temporary, path, true); }
        finally { if (File.Exists(temporary)) File.Delete(temporary); }
    }
}
