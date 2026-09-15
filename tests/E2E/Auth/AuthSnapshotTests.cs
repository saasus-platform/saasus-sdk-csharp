using SaasusSdk.Tests.TestLib;
using Xunit.Abstractions;

namespace SaasusSdk.Tests.E2E.Auth;

[Collection(AuthE2ECollection.Name)]
public sealed class AuthSnapshotTests
{
    private readonly ITestOutputHelper _output;

    public AuthSnapshotTests(ITestOutputHelper output) => _output = output;

    [SnapshotFact]
    [Trait("Category", "Snapshot")]
    [Trait("Module", "auth")]
    public async Task AuthSnapshots()
    {
        var executionConfig = Config.FromEnvironment();
        // Without this the shared default module (billing) would route Auth captures and
        // comparisons at the Billing baselines.
        var snapshotConfig = (executionConfig.Snapshot
            ?? throw new InvalidOperationException("Snapshot configuration was not resolved.")).WithModule("auth");
        if (snapshotConfig.Verbose)
        {
            _output.WriteLine(
                $"Snapshot configuration: mode={snapshotConfig.Mode}, captureLevel={snapshotConfig.CaptureLevel}, " +
                $"comparison={snapshotConfig.ComparisonMode}, output={snapshotConfig.ModuleDirectory}, " +
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
            throw new InvalidOperationException("Snapshot capture requires real responses; unset E2E_DRY_RUN.");
        executionConfig.Validate();
        var cognitoResult = await CognitoTokenProvider.TryCreateAsync(executionConfig, CancellationToken.None);
        // The provider owns an HttpClient, so its lifetime ends with this test.
        using var cognito = cognitoResult.Provider;
        if (cognito is null && !string.IsNullOrWhiteSpace(cognitoResult.Reason))
            _output.WriteLine($"Cognito-dependent Auth operations will be skipped: {Masker.Redact(cognitoResult.Reason, "unavailable")}");
        var stripe = await AuthStripeIntegration.TryPrepareAsync(executionConfig, CancellationToken.None);
        if (!stripe.Enabled)
            _output.WriteLine($"Stripe-dependent Auth operations will be skipped: {Masker.Redact(stripe.Reason, "unavailable")}");

        // The Stripe registration is left in place, like the Go and PHP suites: removing it would
        // break the next Auth run, and every story re-registers the key in its own setup.
        await CaptureAsync(executionConfig, snapshotConfig, manager, cognito, stripe);
    }

    private async Task CaptureAsync(
        Config executionConfig,
        SnapshotConfig snapshotConfig,
        SnapshotManager manager,
        CognitoTokenProvider? cognito,
        AuthStripePreparationResult stripe)
    {
        var options = AuthStoryOptions.Live(executionConfig, cognito, stripe.Enabled);
        var client = new AuthApiClient(
            new modules.Configuration().GetAuthApiClientConfig(), cognito, executionConfig);
        var stories = AuthStoryFactory.Create(client, options)
            .Concat(AuthRawResponseStoryFactory.Create(client, options))
            .Where(story => snapshotConfig.MatchesStory(story.Name))
            .ToArray();
        Assert.NotEmpty(stories);

        var coverage = new CoverageTracker();
        foreach (var method in AuthStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);
        var results = await new E2EEngine(executionConfig, coverage).ExecuteAsync(stories);
        // With --capture-failed the manager is expected to record failed stories, so the
        // pass-only assertion would abort before any snapshot is created.
        if (!snapshotConfig.CaptureFailedStories)
            Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
        if (snapshotConfig.StoryFilters.Count == 0)
        {
            var skippedOperations = AuthStoryFactory.SkippedOperations(options).ToHashSet(StringComparer.Ordinal);
            var expectedSkipped = AuthStoryFactory.Methods
                .Where(method => skippedOperations.Contains(AuthStoryFactory.BaseOperation(method.Method)))
                .ToHashSet();
            Assert.All(coverage.Untested, method => Assert.Contains(method, expectedSkipped));
        }

        var snapshots = stories
            .Zip(results, (story, result) => SnapshotFactory.Create(story, result, snapshotConfig))
            .ToArray();
        var report = await manager.ProcessAsync(snapshots);
        WriteReport(report);
    }

    private void WriteReport(SnapshotRunReport report)
    {
        _output.WriteLine(
            $"Mode: {report.Mode}\nTags: {report.OldTag} -> {report.NewTag}\nSnapshots: {report.Snapshots}\n" +
            $"Compatibility: {report.Compatibility}\nDifferences: {report.Comparisons.Sum(x => x.Issues.Count)}");
        foreach (var validation in report.Validations)
        {
            _output.WriteLine(
                $"Validation {validation.StoryName}: {validation.CompletionStatus}, " +
                $"errors={validation.Summary.TotalErrors}, warnings={validation.Summary.TotalWarnings}");
        }
    }
}
