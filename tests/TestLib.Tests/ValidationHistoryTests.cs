using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

/// <summary>
/// Covers the run-over-run validation delta and the retention policy that keeps only
/// the newest artifacts, matching Go's SaveStoryValidation and PHP's retainLatestValidations.
/// </summary>
public sealed class ValidationHistoryTests
{
    [Fact]
    public async Task FirstCaptureHasNoHistoryBlock()
    {
        var directory = TemporaryDirectory();
        try
        {
            var store = Store(directory);
            var written = await store.SaveValidationAsync(Validation(), "v1");

            Assert.Null(written.Comparison);
            var document = JObject.Parse(await File.ReadAllTextAsync(store.ValidationPath("v1", "story")));
            Assert.Null(document["comparison"]);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task SecondCaptureRecordsNewAndResolvedFindings()
    {
        var directory = TemporaryDirectory();
        try
        {
            var store = Store(directory);
            await store.SaveValidationAsync(Validation(
                new ValidationFinding("sequence", "a", "Missing return_value", "error")), "v1");
            await Task.Delay(15);

            var second = await store.SaveValidationAsync(Validation(
                new ValidationFinding("state_transition", "b", "Step marked as failed but no error information provided", "warning")), "v2");

            var history = Assert.IsType<ValidationHistory>(second.Comparison);
            Assert.Equal(SnapshotStore.ValidationFileName("v1", "story"), history.PreviousFile);
            Assert.NotNull(history.PreviousValidationTime);
            Assert.Equal("b", Assert.Single(history.NewFindings!).StepName);
            Assert.Equal("a", Assert.Single(history.ResolvedFindings!).StepName);
            Assert.Equal(-1, history.ErrorCountDelta);
            Assert.Equal(1, history.WarningCountDelta);
            Assert.Equal(0, history.InfoCountDelta);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task UnchangedFindingsProduceZeroDeltasAndNoLists()
    {
        var directory = TemporaryDirectory();
        try
        {
            var store = Store(directory);
            var finding = new ValidationFinding("sequence", "a", "Missing return_value", "error");
            await store.SaveValidationAsync(Validation(finding), "v1");
            await Task.Delay(15);

            var second = await store.SaveValidationAsync(Validation(finding), "v2");

            var history = Assert.IsType<ValidationHistory>(second.Comparison);
            Assert.Null(history.NewFindings);
            Assert.Null(history.ResolvedFindings);
            Assert.Equal(0, history.ErrorCountDelta);
            Assert.Equal(0, history.WarningCountDelta);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task HistoryBlockMatchesTheSharedContract()
    {
        var directory = TemporaryDirectory();
        try
        {
            var store = Store(directory);
            await store.SaveValidationAsync(Validation(
                new ValidationFinding("sequence", "a", "Missing return_value", "error")), "v1");
            await Task.Delay(15);
            await store.SaveValidationAsync(Validation(), "v2");

            var document = JObject.Parse(await File.ReadAllTextAsync(store.ValidationPath("v2", "story")));
            var comparison = (JObject)document["comparison"]!;

            Assert.Equal(
                new[] { "previous_file", "previous_validation_time", "resolved_findings", "error_count_delta", "warning_count_delta", "info_count_delta" },
                comparison.Properties().Select(property => property.Name).ToArray());
            // comparison must sit between the finding buckets and the summary, as in Go.
            Assert.Equal(
                new[] { "story_name", "validation_time", "is_valid", "completion_status", "sequence_errors", "state_transition_errors", "timing_errors", "comparison", "summary" },
                document.Properties().Select(property => property.Name).ToArray());
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task RetainsOnlyTheTwoNewestValidationArtifacts()
    {
        var directory = TemporaryDirectory();
        try
        {
            var store = Store(directory);
            foreach (var tag in new[] { "v1", "v2", "v3", "v4" })
            {
                await store.SaveValidationAsync(Validation(), tag);
                await Task.Delay(15);
            }

            var files = store.ValidationFiles("story").Select(Path.GetFileName).ToArray();

            Assert.Equal(2, files.Length);
            Assert.Contains(SnapshotStore.ValidationFileName("v4", "story"), files);
            Assert.Contains(SnapshotStore.ValidationFileName("v3", "story"), files);
            Assert.False(File.Exists(store.ValidationPath("v1", "story")));
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task RetentionLimitIsConfigurable()
    {
        var directory = TemporaryDirectory();
        try
        {
            var store = new SnapshotStore(new SnapshotConfig
            {
                OutputDirectory = directory,
                ModuleName = "billing",
                Overwrite = true,
                ValidationHistoryLimit = 3
            });
            foreach (var tag in new[] { "v1", "v2", "v3", "v4" })
            {
                await store.SaveValidationAsync(Validation(), tag);
                await Task.Delay(15);
            }

            Assert.Equal(3, store.ValidationFiles("story").Count);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public void RetentionLimitMustBePositive() =>
        Assert.Throws<InvalidOperationException>(() =>
            new SnapshotConfig { ValidationHistoryLimit = 0 }.Validate());

    [Fact]
    public async Task RetentionKeepsArtifactsPerStory()
    {
        var directory = TemporaryDirectory();
        try
        {
            var store = Store(directory);
            foreach (var tag in new[] { "v1", "v2", "v3" })
            {
                await store.SaveValidationAsync(Validation(storyName: "first"), tag);
                await store.SaveValidationAsync(Validation(storyName: "second"), tag);
                await Task.Delay(15);
            }

            Assert.Equal(2, store.ValidationFiles("first").Count);
            Assert.Equal(2, store.ValidationFiles("second").Count);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task CaptureAttachesHistoryToTheReportedValidation()
    {
        var directory = TemporaryDirectory();
        try
        {
            var snapshot = SnapshotFixtures.WithResponse(new { ok = true });
            await Capture(directory, "v1", snapshot);
            await Task.Delay(15);

            var report = await Capture(directory, "v2", snapshot);

            var validation = Assert.Single(report.Validations);
            var history = Assert.IsType<ValidationHistory>(validation.Comparison);
            Assert.Equal(SnapshotStore.ValidationFileName("v1", snapshot.StoryName), history.PreviousFile);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public void DuplicateFindingsAreComparedAsAMultiset()
    {
        var duplicate = new ValidationFinding("sequence", "a", "Missing return_value", "error");
        var previous = WithSequence(duplicate, duplicate);
        var current = WithSequence(duplicate, duplicate, duplicate);

        var history = SnapshotStore.BuildHistory(previous, "previous.json", current);

        Assert.NotNull(history);
        // Two findings can share story, step, message and severity, so only the extra one is new.
        Assert.Single(history!.NewFindings!);
        Assert.Null(history.ResolvedFindings);
    }

    private static SnapshotValidation WithSequence(params ValidationFinding[] findings) =>
        new("story", DateTimeOffset.UtcNow, false, "partial", findings, null, null, null, null,
            new ValidationSummary(findings.Count(x => x.Severity == "error"), 0, 0, false));

    private static Task<SnapshotRunReport> Capture(string directory, string tag, StorySnapshot snapshot) =>
        new SnapshotManager(new SnapshotConfig
        {
            Mode = SnapshotMode.Capture,
            OutputDirectory = directory,
            ModuleName = "billing",
            CurrentTag = tag,
            Overwrite = true,
            FailOnBreaking = false
        }).ProcessAsync(new[] { snapshot });

    private static SnapshotStore Store(string directory) => new(new SnapshotConfig
    {
        OutputDirectory = directory,
        ModuleName = "billing",
        Overwrite = true
    });

    private static SnapshotValidation Validation(ValidationFinding? finding = null, string storyName = "story")
    {
        var sequence = finding is { Type: "sequence" } ? new[] { finding } : null;
        var state = finding is { Type: "state_transition" } ? new[] { finding } : null;
        var findings = new[] { sequence, state }.Where(x => x is not null).SelectMany(x => x!).ToArray();
        return new SnapshotValidation(storyName, DateTimeOffset.UtcNow,
            findings.All(x => x.Severity != "error"), "complete", sequence, state, null, null, null,
            new ValidationSummary(
                findings.Count(x => x.Severity == "error"),
                findings.Count(x => x.Severity == "warning"),
                findings.Count(x => x.Severity == "info"),
                findings.All(x => x.Severity != "error")));
    }

    private static string TemporaryDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), $"saasus-history-{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        return path;
    }
}
