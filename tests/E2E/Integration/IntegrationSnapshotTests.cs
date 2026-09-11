using integrationapi.Api;
using SaasusSdk.Tests.TestLib;
using Xunit.Abstractions;

namespace SaasusSdk.Tests.E2E.Integration;

[Collection(IntegrationE2ECollection.Name)]
public sealed class IntegrationSnapshotTests
{
    private readonly ITestOutputHelper _output;

    public IntegrationSnapshotTests(ITestOutputHelper output) => _output = output;

    [SnapshotFact]
    [Trait("Category", "Snapshot")]
    [Trait("Module", "integration")]
    public async Task IntegrationSnapshots()
    {
        var executionConfig = Config.FromEnvironment();
        var sourceSnapshotConfig = executionConfig.Snapshot ??
            throw new InvalidOperationException("Snapshot configuration was not resolved.");
        var snapshotConfig = sourceSnapshotConfig.WithModule("integration");
        if (snapshotConfig.Verbose)
        {
            _output.WriteLine(
                $"Snapshot configuration: mode={snapshotConfig.Mode}, " +
                $"captureLevel={snapshotConfig.CaptureLevel}, comparison={snapshotConfig.ComparisonMode}, " +
                $"output={snapshotConfig.ModuleDirectory}, stories={string.Join(",", snapshotConfig.StoryFilters)}");
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
            throw new InvalidOperationException("Integration snapshot capture requires real responses; unset E2E_DRY_RUN.");
        executionConfig.Validate();
        var client = new IntegrationEventBridgeClient(
            new EventBridgeApi(new modules.Configuration().GetIntegrationApiClientConfig()));
        var stories = IntegrationStoryFactory.Create(client)
            .Where(story => snapshotConfig.MatchesStory(story.Name))
            .ToArray();
        Assert.NotEmpty(stories);

        var coverage = new CoverageTracker();
        foreach (var method in IntegrationStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);
        var results = await new E2EEngine(executionConfig, coverage).ExecuteAsync(stories);
        // With --capture-failed the manager is expected to record failed stories, so the
        // pass-only assertion would abort before any snapshot is created.
        if (!snapshotConfig.CaptureFailedStories)
            Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
        if (snapshotConfig.StoryFilters.Count == 0)
            Assert.Empty(coverage.Untested);

        var snapshots = stories
            .Zip(results, (story, result) => SnapshotFactory.Create(story, result, snapshotConfig))
            .ToArray();
        var report = await manager.ProcessAsync(snapshots);
        WriteReport(report);
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
