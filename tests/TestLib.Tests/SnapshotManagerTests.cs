using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

public sealed class SnapshotManagerTests
{
    [Fact]
    public async Task CaptureCompareAndReportWorkOfflineWithTags()
    {
        var directory = TemporaryDirectory();
        try
        {
            await Capture(directory, "v1", SnapshotFixtures.WithResponse(new { kept = 1, removed = true }));
            await Capture(directory, "v2", SnapshotFixtures.WithResponse(new { kept = 1, added = true }));

            var compareConfig = Configuration(directory, SnapshotMode.Compare, oldTag: "v1", newTag: "v2");
            var comparison = await new SnapshotManager(compareConfig).ProcessAsync();
            Assert.Equal(CompatibilityLevel.Breaking, comparison.Compatibility);
            Assert.Contains(comparison.Comparisons.SelectMany(x => x.Issues), issue => issue.Type == "response");

            var reportConfig = Configuration(directory, SnapshotMode.Report, oldTag: "v1", newTag: "v2");
            var report = await new SnapshotManager(reportConfig).ProcessAsync();
            var store = new SnapshotStore(reportConfig);
            Assert.True(File.Exists(store.ReportPath("v1", "v2", "Billing story", "html")));
            Assert.True(File.Exists(store.ReportPath("v1", "v2", "Billing story", "json")));
            Assert.Equal(CompatibilityLevel.Breaking, report.Compatibility);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task RefusesFailedSnapshotsAndAccidentalOverwrite()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = Configuration(directory, SnapshotMode.Capture, currentTag: "v1");
            await Assert.ThrowsAsync<InvalidOperationException>(() => new SnapshotManager(config).ProcessAsync(new[] { Failed() }));
            await new SnapshotManager(config).ProcessAsync(new[] { SnapshotFixtures.WithResponse(new { ok = true }) });
            var validationPath = new SnapshotStore(config).ValidationPath("v1", "Billing story");
            var validationBefore = await File.ReadAllTextAsync(validationPath);
            await Assert.ThrowsAsync<IOException>(() => new SnapshotManager(config).ProcessAsync(new[] { SnapshotFixtures.WithResponse(new { ok = false }) }));
            Assert.Equal(validationBefore, await File.ReadAllTextAsync(validationPath));
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task CaptureFailedStoriesAllowsFailedCompletion()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = new SnapshotConfig
            {
                Mode = SnapshotMode.Capture,
                OutputDirectory = directory,
                ModuleName = "billing",
                CurrentTag = "v1",
                CaptureFailedStories = true
            };

            var report = await new SnapshotManager(config).ProcessAsync(new[] { Failed() });

            Assert.Single(new SnapshotStore(config).StoriesForTag("v1"));
            Assert.Single(report.Validations);
            Assert.True(report.Validations[0].IsValid);
            Assert.Equal("failed", report.Validations[0].CompletionStatus);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task PreflightsEntireCaptureBatchBeforeWriting()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = Configuration(directory, SnapshotMode.Capture, currentTag: "v1");
            var first = SnapshotFixtures.WithResponse(new { ok = true }, "first");
            var failed = Failed("second");

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                new SnapshotManager(config).ProcessAsync(new[] { first, failed }));

            var store = new SnapshotStore(config);
            Assert.False(File.Exists(store.SnapshotPath("v1", "first")));
            Assert.False(File.Exists(store.ValidationPath("v1", "first")));
            Assert.Empty(store.StoriesForTag("v1"));
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task RefusesStoriesThatShareASlug()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = Configuration(directory, SnapshotMode.Capture, currentTag: "v1");

            var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                new SnapshotManager(config).ProcessAsync(new[]
                {
                    SnapshotFixtures.WithResponse(new { ok = true }, "Billing API"),
                    SnapshotFixtures.WithResponse(new { ok = true }, "billing/api")
                }));

            Assert.Contains("slug collision", error.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Contains("billing_api", error.Message, StringComparison.Ordinal);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public void ComparatorDetectsPrimitiveResponseChanges()
    {
        var comparison = new SnapshotComparer().Compare(new { response = new { enabled = false } }, new { response = new { enabled = true } });
        Assert.Contains(comparison.Issues, issue => issue.Path == "$.response.enabled" && issue.Type == "value");
    }

    [Fact]
    public void ValidatorDetectsIncompleteExecution()
    {
        var validation = new SnapshotValidator().Validate(Failed());
        Assert.False(validation.IsValid);
        Assert.NotNull(validation.SequenceErrors);
        Assert.Contains(validation.SequenceErrors!, finding =>
            finding.Type == "sequence" && finding.Message == "Story execution is not complete");
        Assert.Equal(1, validation.Summary.TotalErrors);
    }

    [Fact]
    public async Task FullModeUsesPreviousReleaseTagAndWritesReports()
    {
        var directory = TemporaryDirectory();
        try
        {
            await Capture(directory, "v1", SnapshotFixtures.WithResponse(new { enabled = true }));
            var config = Configuration(directory, SnapshotMode.Full, currentTag: "v2",
                comparisonMode: SnapshotComparisonMode.Release);

            var report = await new SnapshotManager(config).ProcessAsync(new[] { SnapshotFixtures.WithResponse(new { enabled = true }) });
            var store = new SnapshotStore(config);

            Assert.Equal("v1", report.OldTag);
            Assert.Equal("v2", report.NewTag);
            Assert.Equal(CompatibilityLevel.Compatible, report.Compatibility);
            Assert.True(File.Exists(store.ReportPath("v1", "v2", "Billing story", "html")));
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task FullModeCreatesFirstBaselineWithoutPreviousTag()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = Configuration(directory, SnapshotMode.Full, currentTag: "v1",
                comparisonMode: SnapshotComparisonMode.Release);

            var report = await new SnapshotManager(config).ProcessAsync(new[] { SnapshotFixtures.WithResponse(new { enabled = true }) });

            Assert.Equal(string.Empty, report.OldTag);
            Assert.Equal("v1", report.NewTag);
            // Go and PHP emit a zero-difference baseline comparison and report for a first release.
            var baseline = Assert.Single(report.Comparisons);
            Assert.Empty(baseline.Issues);
            Assert.Equal(CompatibilityLevel.Compatible, report.Compatibility);
            var store = new SnapshotStore(config);
            Assert.True(File.Exists(store.ComparisonPath("v1", "v1", "Billing story")));
            Assert.True(File.Exists(store.ReportPath("v1", "v1", "Billing story", "html")));
            Assert.True(File.Exists(store.ReportPath("v1", "v1", "Billing story", "json")));
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task ReportModeRequiresPreviousReleaseTag()
    {
        var directory = TemporaryDirectory();
        try
        {
            await Capture(directory, "v1", SnapshotFixtures.WithResponse(new { enabled = true }));
            var config = Configuration(directory, SnapshotMode.Report,
                comparisonMode: SnapshotComparisonMode.Release);

            var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                new SnapshotManager(config).ProcessAsync());

            Assert.Contains("No previous snapshot tag", error.Message);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task OfflineReportCountsOnlyMatchingStories()
    {
        var directory = TemporaryDirectory();
        try
        {
            await Capture(directory, "v1", SnapshotFixtures.WithResponse(new { enabled = true }));
            await Capture(directory, "v2", SnapshotFixtures.WithResponse(new { enabled = false }));
            var config = new SnapshotConfig
            {
                Mode = SnapshotMode.Report,
                OutputDirectory = directory,
                ModuleName = "billing",
                OldTag = "v1",
                NewTag = "v2",
                ComparisonMode = SnapshotComparisonMode.Manual,
                StoryFilters = new[] { "missing story" },
                FailOnBreaking = false
            };

            var report = await new SnapshotManager(config).ProcessAsync();

            Assert.Equal(0, report.Snapshots);
            Assert.Empty(report.Comparisons);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task SkipComparisonModeCapturesWithoutComparisonTags()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = Configuration(directory, SnapshotMode.Full, currentTag: "v1",
                comparisonMode: SnapshotComparisonMode.Skip);

            var report = await new SnapshotManager(config).ProcessAsync(new[] { SnapshotFixtures.WithResponse(new { enabled = true }) });

            Assert.Single(new SnapshotStore(config).StoriesForTag("v1"));
            Assert.Empty(report.Comparisons);
            Assert.Equal(CompatibilityLevel.Compatible, report.Compatibility);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public void ManualComparisonRequiresBothTags()
    {
        var directory = TemporaryDirectory();
        try
        {
            Assert.Throws<InvalidOperationException>(() => new SnapshotManager(
                Configuration(directory, SnapshotMode.Compare, oldTag: "v1")));
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task ExplicitMissingComparisonTagDoesNotFallback()
    {
        var directory = TemporaryDirectory();
        try
        {
            await Capture(directory, "v1", SnapshotFixtures.WithResponse(new { enabled = true }));
            var config = Configuration(directory, SnapshotMode.Compare, oldTag: "missing", newTag: "v1");

            var error = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                new SnapshotManager(config).ProcessAsync());

            Assert.Contains("missing", error.Message);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task DiscoveryNormalisesMetadataTagsAndFiltersStaleArtifacts()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = Configuration(directory, SnapshotMode.Capture, currentTag: "release/1.0", overwrite: true);
            var store = new SnapshotStore(config);
            var snapshot = SnapshotFixtures.WithResponse(new { ok = true }) with
            {
                Metadata = SnapshotFixtures.Metadata(gitTag: "release/1.0")
            };
            await store.SaveSnapshotAsync(snapshot, "release/1.0");

            // Tags are addressed through their slug, so discovery must report the slug.
            Assert.Equal(new[] { "release_1.0" }, store.AvailableTags());
            Assert.Equal(new[] { snapshot.StoryName }, store.StoriesForTag("release/1.0"));

            // A stale artifact copied under a different tag's file name must not be returned.
            var stale = snapshot with { StoryName = "Stale story", Metadata = SnapshotFixtures.Metadata(gitTag: "v0.9") };
            await File.WriteAllTextAsync(store.SnapshotPath("release/1.0", "Stale story"), SnapshotJson.Serialize(stale));

            Assert.Equal(new[] { snapshot.StoryName }, store.StoriesForTag("release/1.0"));
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task DisablingValidationSkipsTheValidatorEntirely()
    {
        var directory = TemporaryDirectory();
        try
        {
            var relaxed = Configuration(directory, SnapshotMode.Capture, currentTag: "v1", overwrite: true,
                validationEnabled: false);
            var store = new SnapshotStore(relaxed);

            // A story that would fail validation is still captured when validation is off.
            var report = await new SnapshotManager(relaxed).ProcessAsync(new[]
            {
                SnapshotFixtures.Story(steps: Array.Empty<StepSnapshot>(), summary: SnapshotFixtures.Summary(0, 0, 0, 0))
            });

            Assert.Empty(report.Validations);
            Assert.False(File.Exists(store.ValidationPath("v1", "Billing story")));
            Assert.True(File.Exists(store.SnapshotPath("v1", "Billing story")));
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task ReleaseComparisonInfersThePreviousTagFromNewTagAlone()
    {
        var directory = TemporaryDirectory();
        try
        {
            await Capture(directory, "v1", SnapshotFixtures.WithResponse(new { kept = 1 }));
            await Task.Delay(15);
            await Capture(directory, "v2", SnapshotFixtures.WithResponse(new { kept = 2 }));

            var config = Configuration(directory, SnapshotMode.Compare, newTag: "v2",
                comparisonMode: SnapshotComparisonMode.Release);
            var report = await new SnapshotManager(config).ProcessAsync();

            Assert.Equal("v1", report.OldTag);
            Assert.Equal("v2", report.NewTag);
            Assert.NotEmpty(report.Comparisons);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task MalformedSnapshotArtifactsAreReportedInsteadOfIgnored()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = Configuration(directory, SnapshotMode.Capture, currentTag: "v1", overwrite: true);
            var store = new SnapshotStore(config);
            await store.SaveSnapshotAsync(SnapshotFixtures.WithResponse(new { ok = true }), "v1");
            await File.WriteAllTextAsync(store.SnapshotPath("v1", "Broken story"), "{ not json");

            var error = Assert.Throws<InvalidDataException>(() => store.AvailableTags());

            Assert.Contains("Malformed snapshot artifact", error.Message, StringComparison.Ordinal);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public void ArtifactNamesUseTheSharedStorySlug()
    {
        var store = new SnapshotStore(Configuration(Path.GetTempPath(), SnapshotMode.Capture));

        Assert.Equal("story_snapshot_v1.0.0-8-gbcd96cd_billing_api_synchronous_http_responses.json",
            Path.GetFileName(store.SnapshotPath("v1.0.0-8-gbcd96cd", "Billing API - synchronous HTTP responses")));
        Assert.Equal("story_validation_billing_api_synchronous_http_responses_v1.0.0-8-gbcd96cd.json",
            SnapshotStore.ValidationFileName("v1.0.0-8-gbcd96cd", "Billing API - synchronous HTTP responses"));
    }

    [Fact]
    public async Task CaptureFailedPersistsTheDiagnosticArtifactsAndValidation()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = Configuration(directory, SnapshotMode.Capture, currentTag: "v1", overwrite: true);
            var withFailed = new SnapshotConfig
            {
                Mode = config.Mode,
                OutputDirectory = config.OutputDirectory,
                ModuleName = config.ModuleName,
                CurrentTag = config.CurrentTag,
                Overwrite = true,
                FailOnBreaking = false,
                CaptureFailedStories = true
            };
            var store = new SnapshotStore(withFailed);

            // A failed story whose steps do not name the failure is invalid, yet --capture-failed
            // must still persist the diagnostic artifacts.
            var invalidFailed = SnapshotFixtures.Story(status: TestStatus.Failed,
                steps: new[] { SnapshotFixtures.Step(returnValue: SnapshotFixtures.ReturnValue(new { ok = true })) },
                summary: SnapshotFixtures.Summary(1, 1, 0, 0));
            var report = await new SnapshotManager(withFailed).ProcessAsync(new[] { invalidFailed });

            Assert.False(Assert.Single(report.Validations).IsValid);
            Assert.True(File.Exists(store.SnapshotPath("v1", "Billing story")));
            Assert.True(File.Exists(store.ValidationPath("v1", "Billing story")));
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task ReportRunsExposeValidationInJsonAndInTheRunReport()
    {
        var directory = TemporaryDirectory();
        try
        {
            await Capture(directory, "v1", SnapshotFixtures.WithResponse(new { kept = 1 }));
            await Task.Delay(15);
            await Capture(directory, "v2", SnapshotFixtures.WithResponse(new { kept = 2 }));

            var config = Configuration(directory, SnapshotMode.Report, oldTag: "v1", newTag: "v2");
            var report = await new SnapshotManager(config).ProcessAsync();
            var store = new SnapshotStore(config);

            Assert.NotEmpty(report.Validations);
            var document = JObject.Parse(
                await File.ReadAllTextAsync(store.ReportPath("v1", "v2", "Billing story", "json")));
            Assert.NotNull(document["comparison"]);
            Assert.NotNull(document["validation"]);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task BreakingChangesFailTheRunUnderTheDefaultPolicy()
    {
        var directory = TemporaryDirectory();
        try
        {
            await Capture(directory, "v1", SnapshotFixtures.WithResponse(new { kept = 1, removed = true }));
            await Task.Delay(15);
            await Capture(directory, "v2", SnapshotFixtures.WithResponse(new { kept = 1 }));

            // Every other case opts out of the policy, so the default has to be asserted here.
            var strict = new SnapshotConfig
            {
                Mode = SnapshotMode.Compare,
                OutputDirectory = directory,
                ModuleName = "billing",
                OldTag = "v1",
                NewTag = "v2",
                ComparisonMode = SnapshotComparisonMode.Manual
            };
            Assert.True(strict.FailOnBreaking);

            var error = await Assert.ThrowsAsync<InvalidOperationException>(
                () => new SnapshotManager(strict).ProcessAsync());
            Assert.Contains("Breaking snapshot changes detected", error.Message, StringComparison.Ordinal);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task CaptureFailsWhenTheStoryFilterMatchesNothing()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = new SnapshotConfig
            {
                Mode = SnapshotMode.Capture,
                OutputDirectory = directory,
                ModuleName = "billing",
                CurrentTag = "v1",
                Overwrite = true,
                FailOnBreaking = false,
                StoryFilters = new[] { "typo" }
            };

            // A typo in --stories would otherwise write nothing and still report success.
            var error = await Assert.ThrowsAsync<InvalidOperationException>(
                () => new SnapshotManager(config).ProcessAsync(new[] { SnapshotFixtures.WithResponse(new { ok = true }) }));

            Assert.Contains("No story matched", error.Message, StringComparison.Ordinal);
        }
        finally { Directory.Delete(directory, true); }
    }

    private static StorySnapshot Failed(string name = "Billing story") =>
        SnapshotFixtures.Story(name, status: TestStatus.Failed,
            steps: new[] { SnapshotFixtures.Step(status: TestStatus.Failed, statusCode: 500,
                returnValue: SnapshotFixtures.ReturnValue(new { ok = false }, statusCode: 500),
                error: new SnapshotError("System.InvalidOperationException", "boom")) },
            summary: SnapshotFixtures.Summary(1, 0, 1, 0));

    private static async Task Capture(string directory, string tag, StorySnapshot snapshot) =>
        await new SnapshotManager(Configuration(directory, SnapshotMode.Capture, currentTag: tag, overwrite: true)).ProcessAsync(new[] { snapshot });

    private static SnapshotConfig Configuration(string directory, SnapshotMode mode, string currentTag = "", string oldTag = "", string newTag = "", bool overwrite = false,
        SnapshotComparisonMode comparisonMode = SnapshotComparisonMode.Manual, bool validationEnabled = true) => new()
        {
            Mode = mode,
            OutputDirectory = directory,
            ModuleName = "billing",
            CurrentTag = currentTag,
            OldTag = oldTag,
            NewTag = newTag,
            ComparisonMode = comparisonMode,
            Overwrite = overwrite,
            ValidationEnabled = validationEnabled,
            FailOnBreaking = false
        };

    private static string TemporaryDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), $"saasus-snapshot-{Guid.NewGuid():N}");
        Directory.CreateDirectory(path); return path;
    }
}
