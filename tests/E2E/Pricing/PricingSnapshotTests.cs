using pricingapi.Api;
using SaasusSdk.Tests.TestLib;
using Xunit.Abstractions;

namespace SaasusSdk.Tests.E2E.Pricing;

[Collection(PricingE2ECollection.Name)]
public sealed class PricingSnapshotTests
{
    private readonly ITestOutputHelper _output;

    public PricingSnapshotTests(ITestOutputHelper output) => _output = output;

    [SnapshotFact]
    [Trait("Category", "Snapshot")]
    [Trait("Module", "pricing")]
    public async Task PricingSnapshots()
    {
        var executionConfig = Config.FromEnvironment();
        // Without this the shared default module (billing) would route Pricing captures and
        // comparisons at the Billing baselines.
        var snapshotConfig = (executionConfig.Snapshot
            ?? throw new InvalidOperationException("Snapshot configuration was not resolved.")).WithModule("pricing");
        if (snapshotConfig.Verbose)
        {
            _output.WriteLine(
                $"Snapshot configuration: mode={snapshotConfig.Mode}, " +
                $"captureLevel={snapshotConfig.CaptureLevel}, " +
                $"comparison={snapshotConfig.ComparisonMode}, " +
                $"output={snapshotConfig.ModuleDirectory}, " +
                $"stories={string.Join(",", snapshotConfig.StoryFilters)}");
        }

        var manager = new SnapshotManager(snapshotConfig);
        if (snapshotConfig.Mode is SnapshotMode.Compare or SnapshotMode.Report)
        {
            var offlineReport = await manager.ProcessAsync();
            WriteReport(offlineReport);
            Assert.NotEqual(0, offlineReport.Snapshots);
            return;
        }

        if (executionConfig.DryRun)
            throw new InvalidOperationException("Pricing snapshot capture requires real responses; unset E2E_DRY_RUN.");
        executionConfig.Validate();

        var moduleConfiguration = new modules.Configuration().GetPricingApiClientConfig();
        var client = new PricingApiClient(
            new PricingUnitsApi(moduleConfiguration),
            new PricingMenusApi(moduleConfiguration),
            new PricingPlansApi(moduleConfiguration),
            new MeteringApi(moduleConfiguration),
            new TaxRateApi(moduleConfiguration));
        var stories = PricingStoryFactory.Create(client)
            .Where(story => snapshotConfig.MatchesStory(story.Name))
            .ToArray();
        Assert.NotEmpty(stories);

        var coverage = new CoverageTracker();
        foreach (var method in PricingStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);
        var results = await new E2EEngine(executionConfig, coverage).ExecuteAsync(stories);
        _output.WriteLine(new Reporter().Render(results, coverage));

        // --capture-failed exists so failed stories still produce artifacts; asserting
        // before SnapshotManager ran would abort the test and discard them.
        void AssertStoriesPassed()
        {
            Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
            if (snapshotConfig.StoryFilters.Count == 0)
                Assert.Empty(coverage.Untested);
        }

        if (!snapshotConfig.CaptureFailedStories) AssertStoriesPassed();

        var snapshots = stories
            .Zip(results, (story, result) => SnapshotFactory.Create(story, result, snapshotConfig))
            .ToArray();
        var report = await manager.ProcessAsync(snapshots);
        WriteReport(report);
        if (snapshotConfig.CaptureFailedStories) AssertStoriesPassed();
    }

    private void WriteReport(SnapshotRunReport report)
    {
        _output.WriteLine(
            $"Mode: {report.Mode}\nTags: {report.OldTag} -> {report.NewTag}\n" +
            $"Snapshots: {report.Snapshots}\nCompatibility: {report.Compatibility}\n" +
            $"Differences: {report.Comparisons.Sum(comparison => comparison.Issues.Count)}");
        foreach (var validation in report.Validations)
        {
            _output.WriteLine(
                $"Validation {validation.StoryName}: {validation.CompletionStatus}, " +
                $"errors={validation.Summary.TotalErrors}, warnings={validation.Summary.TotalWarnings}");
            if (validation.Comparison is { } history)
            {
                _output.WriteLine(
                    $"  vs {history.PreviousFile}: new={history.NewFindings?.Count ?? 0}, " +
                    $"resolved={history.ResolvedFindings?.Count ?? 0}, " +
                    $"errorDelta={history.ErrorCountDelta:+0;-0;0}");
            }
        }
    }
}
