using billingapi.Api;
using SaasusSdk.Tests.TestLib;
using Xunit.Abstractions;

namespace SaasusSdk.Tests.E2E.Billing;

[Collection(BillingE2ECollection.Name)]
public sealed class BillingSnapshotTests
{
    private readonly ITestOutputHelper _output;
    public BillingSnapshotTests(ITestOutputHelper output) => _output = output;

    [SnapshotFact]
    [Trait("Category", "Snapshot")]
    [Trait("Module", "billing")]
    public async Task BillingSnapshots()
    {
        var executionConfig = Config.FromEnvironment();
        // Each module test routes itself, so --module only selects which tests run.
        var snapshotConfig = (executionConfig.Snapshot
            ?? throw new InvalidOperationException("Snapshot configuration was not resolved.")).WithModule("billing");
        if (snapshotConfig.Verbose)
            _output.WriteLine($"Snapshot configuration: mode={snapshotConfig.Mode}, captureLevel={snapshotConfig.CaptureLevel}, comparison={snapshotConfig.ComparisonMode}, output={snapshotConfig.ModuleDirectory}, stories={string.Join(",", snapshotConfig.StoryFilters)}");
        var manager = new SnapshotManager(snapshotConfig);
        if (snapshotConfig.Mode is SnapshotMode.Compare or SnapshotMode.Report)
        {
            var offlineReport = await manager.ProcessAsync();
            WriteReport(offlineReport);
            Assert.NotEqual(0, offlineReport.Snapshots);
            return;
        }

        if (executionConfig.DryRun) throw new InvalidOperationException("Snapshot capture requires real responses; unset E2E_DRY_RUN.");
        executionConfig.Validate();
        if (string.IsNullOrWhiteSpace(executionConfig.StripeSecretKey))
            throw new InvalidOperationException("STRIPE_SECRET_KEY is required for Billing snapshot capture.");
        var client = new BillingStripeClient(new StripeApi(new modules.Configuration().GetBillingApiClientConfig()));
        var stories = BillingStoryFactory.Create(client, executionConfig.StripeSecretKey)
            .Where(story => snapshotConfig.MatchesStory(story.Name)).ToArray();
        Assert.NotEmpty(stories);
        var coverage = new CoverageTracker();
        foreach (var method in BillingStoryFactory.Methods) coverage.Register(method.Method, method.CallStyle);
        var results = await new E2EEngine(executionConfig, coverage).ExecuteAsync(stories);
        // With --capture-failed the manager is expected to record failed stories, so the
        // assertions run after the artifacts were written instead of aborting the run.
        void AssertStoriesPassed()
        {
            Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
            if (snapshotConfig.StoryFilters.Count == 0) Assert.Empty(coverage.Untested);
        }

        if (!snapshotConfig.CaptureFailedStories) AssertStoriesPassed();
        var snapshots = stories.Zip(results, (story, result) => SnapshotFactory.Create(story, result, snapshotConfig)).ToArray();
        var report = await manager.ProcessAsync(snapshots);
        WriteReport(report);
        if (snapshotConfig.CaptureFailedStories) AssertStoriesPassed();
    }

    private void WriteReport(SnapshotRunReport report)
    {
        _output.WriteLine(
            $"Mode: {report.Mode}\nTags: {report.OldTag} -> {report.NewTag}\nSnapshots: {report.Snapshots}\n" +
            $"Compatibility: {report.Compatibility}\nDifferences: {report.Comparisons.Sum(x => x.Issues.Count)}");
        foreach (var validation in report.Validations)
        {
            _output.WriteLine($"Validation {validation.StoryName}: {validation.CompletionStatus}, " +
                              $"errors={validation.Summary.TotalErrors}, warnings={validation.Summary.TotalWarnings}");
            if (validation.Comparison is { } history)
                _output.WriteLine($"  vs {history.PreviousFile}: new={history.NewFindings?.Count ?? 0}, " +
                                  $"resolved={history.ResolvedFindings?.Count ?? 0}, " +
                                  $"errorDelta={history.ErrorCountDelta:+0;-0;0}");
        }
    }
}
