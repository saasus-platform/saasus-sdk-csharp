namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Drives the capture / compare / report / full snapshot workflow.
/// Capture is preflighted as a batch so a late failure never leaves partial artifacts.
/// </summary>
public sealed class SnapshotManager
{
    private readonly SnapshotConfig _config;
    private readonly SnapshotStore _store;
    private readonly SnapshotComparer _comparer = new();
    private readonly SnapshotValidator _validator = new();
    private readonly SnapshotReporter _reporter = new();
    public SnapshotManager(SnapshotConfig config) { _config = config; _config.Validate(); _store = new(config); }

    public async Task<SnapshotRunReport> ProcessAsync(
        IReadOnlyList<StorySnapshot>? currentSnapshots = null,
        CancellationToken token = default)
    {
        var tag = _config.ResolveCurrentTag();
        var validations = new List<SnapshotValidation>();
        if (_config.EnableCapture)
        {
            if (currentSnapshots is null || currentSnapshots.Count == 0) throw new InvalidOperationException("Capture mode requires executed story snapshots.");
            var selected = currentSnapshots.Where(snapshot => _config.MatchesStory(snapshot.StoryName)).ToArray();
            // A typo in --stories would otherwise write nothing and still report success.
            if (selected.Length == 0)
                throw new InvalidOperationException(
                    "No story matched the configured filters, so there is nothing to capture: " +
                    $"{string.Join(", ", _config.StoryFilters)}");
            EnsureDistinctSlugs(selected);

            var captures = new List<(StorySnapshot Snapshot, SnapshotValidation? Validation)>();
            foreach (var snapshot in selected)
            {
                var snapshotPath = _store.SnapshotPath(tag, snapshot.StoryName);
                if (!_config.Overwrite && File.Exists(snapshotPath))
                    throw new IOException($"Artifact already exists: {snapshotPath}. Set E2E_SNAPSHOT_OVERWRITE=true to replace it.");
                if (!_config.CaptureFailedStories && snapshot.Status != TestStatus.Passed)
                    throw new InvalidOperationException($"Refusing to capture failed story '{snapshot.StoryName}'.");
                // E2E_SNAPSHOT_VALIDATION=false must skip the validator, not merely ignore it.
                if (!_config.ValidationEnabled)
                {
                    captures.Add((snapshot, null));
                    continue;
                }
                var validation = _validator.Validate(snapshot, _config);
                // A failed story is captured for diagnosis, so its findings must not block the
                // write; an ordinary capture is still rejected when it does not validate.
                if (!validation.IsValid &&
                    !(_config.CaptureFailedStories && snapshot.Status == TestStatus.Failed))
                    throw new InvalidOperationException($"Snapshot validation failed for '{snapshot.StoryName}'.");
                captures.Add((snapshot, validation));
            }

            // The artifacts of a batch belong together, so files this run creates are removed again
            // when a later write fails. An artifact that replaced an existing one is kept: its
            // previous content is already gone and deleting it would remove a committed baseline.
            var written = new List<string>();
            try
            {
                foreach (var (snapshot, validation) in captures)
                {
                    // The snapshot is written first: a validation artifact without its baseline would
                    // make later history and reporting observe a run that has no snapshot.
                    var tagged = snapshot with { Metadata = snapshot.Metadata with { GitTag = tag } };
                    var snapshotPath = _store.SnapshotPath(tag, snapshot.StoryName);
                    var snapshotExisted = File.Exists(snapshotPath);
                    await _store.SaveSnapshotAsync(tagged, tag, token);
                    if (!snapshotExisted) written.Add(snapshotPath);
                    if (validation is null) continue;
                    // SaveValidationAsync attaches the run-over-run delta and prunes old artifacts.
                    var validationPath = _store.ValidationPath(tag, validation.StoryName);
                    var validationExisted = File.Exists(validationPath);
                    validations.Add(await _store.SaveValidationAsync(validation, tag, token));
                    if (!validationExisted) written.Add(validationPath);
                }
            }
            catch
            {
                foreach (var path in written)
                {
                    try { File.Delete(path); }
                    catch (IOException) { /* Reported through the original failure. */ }
                    catch (UnauthorizedAccessException) { /* Reported through the original failure. */ }
                }

                throw;
            }
        }

        var comparisons = new List<SnapshotComparison>();
        var oldTag = string.Empty;
        var newTag = tag;
        if (_config.EnableComparison || _config.EnableReporting)
        {
            if (_config.ComparisonMode == SnapshotComparisonMode.Skip)
                return new(_config.Mode.ToString().ToLowerInvariant(), oldTag, newTag,
                    SnapshotCount(newTag, currentSnapshots),
                    CompatibilityLevel.Compatible, comparisons, validations);
            (oldTag, newTag) = ResolveTags(tag);
            if (string.IsNullOrEmpty(oldTag))
            {
                // The first release has no predecessor. Go and PHP still emit a compatible baseline
                // comparison (and report) so the run produces the expected artifacts.
                foreach (var story in _store.StoriesForTag(newTag).Where(_config.MatchesStory))
                {
                    var baseline = new SnapshotComparison(story, newTag, newTag, Array.Empty<CompatibilityIssue>());
                    comparisons.Add(baseline);
                    await _store.SaveComparisonAsync(baseline, token);
                    if (!_config.EnableReporting) continue;
                    var snapshot = await _store.LoadSnapshotAsync(newTag, story, token);
                    var validation = validations.FirstOrDefault(x => x.StoryName == story)
                        ?? await ResolveValidationAsync(newTag, story, snapshot, token);
                    if (validation is not null && validations.All(x => x.StoryName != story))
                        validations.Add(validation);
                    await _store.SaveReportAsync(baseline,
                        _reporter.ToReportJson(baseline, validation),
                        _reporter.ToHtml(baseline, validation), token);
                }
                return new(_config.Mode.ToString().ToLowerInvariant(), oldTag, newTag,
                    SnapshotCount(newTag, currentSnapshots),
                    CompatibilityLevel.Compatible, comparisons, validations);
            }
            // Stories are addressed by slug in every artifact path, so two display names that
            // normalise to the same slug are the same story and must be compared, not reported as
            // one removed plus one added.
            var oldStories = BySlug(_store.StoriesForTag(oldTag));
            var newStories = BySlug(_store.StoriesForTag(newTag));
            var slugs = oldStories.Keys.Union(newStories.Keys, StringComparer.Ordinal)
                .OrderBy(slug => slug, StringComparer.Ordinal).ToArray();
            foreach (var slug in slugs)
            {
                var story = newStories.TryGetValue(slug, out var newName) ? newName : oldStories[slug];
                if (!_config.MatchesStory(story)) continue;
                if (!oldStories.ContainsKey(slug) || !newStories.ContainsKey(slug))
                {
                    var removed = !newStories.ContainsKey(slug);
                    var missing = new SnapshotComparison(story, oldTag, newTag, new[] { new CompatibilityIssue(
                        removed ? "story_removed" : "story_added", "$", "Story set changed.", null, null,
                        removed ? CompatibilityLevel.Breaking : CompatibilityLevel.Warning) });
                    comparisons.Add(missing);
                    await _store.SaveComparisonAsync(missing, token);
                    var missingValidation = _config.ValidationEnabled
                        ? await _store.LoadValidationAsync(newTag, story, token)
                        : null;
                    if (missingValidation is not null && validations.All(x => x.StoryName != story))
                        validations.Add(missingValidation);
                    if (_config.EnableReporting)
                        await _store.SaveReportAsync(missing,
                            _reporter.ToReportJson(missing, missingValidation),
                            _reporter.ToHtml(missing, missingValidation), token);
                    continue;
                }
                var oldSnapshot = await _store.LoadSnapshotAsync(oldTag, oldStories[slug], token);
                var newSnapshot = await _store.LoadSnapshotAsync(newTag, newStories[slug], token);
                var comparison = _comparer.Compare(story, oldSnapshot, newSnapshot, oldTag, newTag);
                comparisons.Add(comparison);
                await _store.SaveComparisonAsync(comparison, token);
                // The run report must expose the validation summaries too, not only the HTML page.
                var storyValidation = validations.FirstOrDefault(x => x.StoryName == story)
                    ?? await ResolveValidationAsync(newTag, story, newSnapshot, token);
                if (storyValidation is not null && validations.All(x => x.StoryName != story))
                    validations.Add(storyValidation);
                if (_config.EnableReporting)
                    await _store.SaveReportAsync(comparison,
                        _reporter.ToReportJson(comparison, storyValidation),
                        _reporter.ToHtml(comparison, storyValidation), token);
            }
        }
        var level = comparisons.Any(x => x.Level == CompatibilityLevel.Breaking) ? CompatibilityLevel.Breaking :
            comparisons.Any(x => x.Level == CompatibilityLevel.Warning) ? CompatibilityLevel.Warning : CompatibilityLevel.Compatible;
        if (_config.FailOnBreaking && level == CompatibilityLevel.Breaking && _config.Mode != SnapshotMode.Report)
            throw new InvalidOperationException("Breaking snapshot changes detected:\n" + string.Join("\n", comparisons.SelectMany(x => x.Issues).Where(x => x.Level == CompatibilityLevel.Breaking).Select(x => $"{x.Path}: {x.Description}")));
        return new(_config.Mode.ToString().ToLowerInvariant(), oldTag, newTag, SnapshotCount(newTag, currentSnapshots), level, comparisons, validations);
    }

    /// <summary>Indexes discovered story names by the slug used in every artifact path.</summary>
    private static Dictionary<string, string> BySlug(IReadOnlyList<string> stories)
    {
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var story in stories) result[SnapshotConfig.StorySlug(story)] = story;
        return result;
    }

    /// <summary>
    /// Story names are slugified into file names, so two stories that normalise to the same slug
    /// would silently overwrite each other. Duplicate display names collide the same way.
    /// </summary>
    private static void EnsureDistinctSlugs(IReadOnlyList<StorySnapshot> snapshots)
    {
        var seen = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (var snapshot in snapshots)
        {
            var slug = SnapshotConfig.StorySlug(snapshot.StoryName);
            if (seen.TryGetValue(slug, out var existing))
                throw new InvalidOperationException(
                    $"Story slug collision: \"{existing}\" and \"{snapshot.StoryName}\" both normalize to \"{slug}\".");
            seen[slug] = snapshot.StoryName;
        }
    }

    /// <summary>
    /// Returns the validation artifact for a story. A disabled validation setting suppresses it
    /// entirely; a reporting run persists a missing artifact, while a pure comparison validates in
    /// memory so the run report is complete without writing new files.
    /// </summary>
    private async Task<SnapshotValidation?> ResolveValidationAsync(
        string tag, string story, StorySnapshot snapshot, CancellationToken token)
    {
        if (!_config.ValidationEnabled) return null;
        var existing = await _store.LoadValidationAsync(tag, story, token);
        if (existing is not null) return existing;
        var validation = _validator.Validate(snapshot, _config);
        return _config.EnableReporting
            ? await _store.SaveValidationAsync(validation, tag, token)
            : validation;
    }

    private int SnapshotCount(string tag, IReadOnlyList<StorySnapshot>? currentSnapshots) =>
        currentSnapshots is null
            ? _store.StoriesForTag(tag).Count(_config.MatchesStory)
            : currentSnapshots.Count(snapshot => _config.MatchesStory(snapshot.StoryName));

    private (string Old, string New) ResolveTags(string current)
    {
        if (_config.ComparisonMode == SnapshotComparisonMode.Manual || !string.IsNullOrWhiteSpace(_config.OldTag))
        {
            var oldTag = SnapshotConfig.Slug(_config.OldTag);
            var available = _store.AvailableTags();
            // A capture-enabled run compares the artifacts it just wrote; otherwise an explicit
            // --new-tag wins and, failing that, the newest stored tag is the candidate.
            var newTag = _config.EnableCapture
                ? current
                : !string.IsNullOrWhiteSpace(_config.NewTag)
                    ? SnapshotConfig.Slug(_config.NewTag)
                    : available.LastOrDefault()
                      ?? throw new InvalidOperationException("No snapshot tag is available.");
            if (!available.Contains(oldTag)) throw new InvalidOperationException($"Snapshot tag does not exist: {oldTag}");
            if (!available.Contains(newTag)) throw new InvalidOperationException($"Snapshot tag does not exist: {newTag}");
            return (oldTag, newTag);
        }
        var tags = _store.AvailableTags().ToList();
        // Full mode captures under the resolved current tag, so that is the candidate. Only a pure
        // compare or report run may point --new-tag at a different, already captured tag.
        var newest = _config.EnableCapture
            ? current
            : !string.IsNullOrWhiteSpace(_config.NewTag)
                ? SnapshotConfig.Slug(_config.NewTag)
                : tags.LastOrDefault() ?? throw new InvalidOperationException("No snapshot tag is available.");
        var index = tags.FindIndex(tag => tag == newest);
        if (index < 0) throw new InvalidOperationException($"Snapshot tag does not exist: {newest}");
        if (index == 0)
        {
            if (_config.Mode == SnapshotMode.Full) return (string.Empty, newest);
            throw new InvalidOperationException("No previous snapshot tag is available. Specify E2E_SNAPSHOT_OLD_TAG.");
        }
        return (tags[index - 1], newest);
    }
}
