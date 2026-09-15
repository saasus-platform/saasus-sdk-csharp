using SaasusSdk.Tests.TestLib;
using Xunit.Abstractions;

namespace SaasusSdk.Tests.E2E.Auth;

[Collection(AuthE2ECollection.Name)]
public sealed class AuthApiTests
{
    private readonly ITestOutputHelper _output;

    public AuthApiTests(ITestOutputHelper output) => _output = output;

    [E2EFact]
    [Trait("Category", "E2E")]
    public async Task AuthApiStories()
    {
        var config = Config.FromEnvironment();
        config.Validate();
        CognitoTokenProvider? cognito = null;
        if (!config.DryRun)
        {
            var cognitoResult = await CognitoTokenProvider.TryCreateAsync(config, CancellationToken.None);
            cognito = cognitoResult.Provider;
            if (cognito is null && !string.IsNullOrWhiteSpace(cognitoResult.Reason))
                _output.WriteLine($"Cognito-dependent Auth operations will be skipped: {Masker.Redact(cognitoResult.Reason, "unavailable")}");
        }
        // The provider owns an HttpClient, so its lifetime ends with this test.
        using var ownedCognito = cognito;

        var stripe = await AuthStripeIntegration.TryPrepareAsync(config, CancellationToken.None);
        if (!stripe.Enabled)
            _output.WriteLine($"Stripe-dependent Auth operations will be skipped: {Masker.Redact(stripe.Reason, "unavailable")}");

        // The Stripe registration is left in place, like the Go and PHP suites: removing it would
        // break the next Auth run, and every story re-registers the key in its own setup.
        await RunStoriesAsync(config, cognito, stripe);
    }

    private async Task RunStoriesAsync(
        Config config,
        CognitoTokenProvider? cognito,
        AuthStripePreparationResult stripe)
    {
        var moduleConfiguration = new modules.Configuration().GetAuthApiClientConfig();
        var options = AuthStoryOptions.Live(config, cognito, stripe.Enabled);
        var client = new AuthApiClient(moduleConfiguration, cognito, config);
        var stories = AuthStoryFactory.Create(client, options)
            .Concat(AuthRawResponseStoryFactory.Create(client, options))
            .ToArray();
        var coverage = new CoverageTracker();
        foreach (var method in AuthStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);

        var results = await new E2EEngine(config, coverage).ExecuteAsync(stories);
        var reporter = new Reporter();
        _output.WriteLine(reporter.Render(results, coverage));
        if (!string.IsNullOrWhiteSpace(config.ReportJsonPath))
        {
            await reporter.WriteJsonAsync(config.ReportJsonPath, results, coverage);
            _output.WriteLine($"JSON report written to {config.ReportJsonPath}");
        }

        var failures = results.Where(result => result.Status == TestStatus.Failed).ToArray();
        Assert.True(failures.Length == 0,
            string.Join(Environment.NewLine, failures.Select(DescribeFailure)));

        var skippedOperations = AuthStoryFactory.SkippedOperations(options).ToHashSet(StringComparer.Ordinal);
        var expectedSkipped = AuthStoryFactory.Methods
            .Where(method => skippedOperations.Contains(AuthStoryFactory.BaseOperation(method.Method)))
            .ToHashSet();
        var untested = coverage.Untested
            .Where(method => !expectedSkipped.Contains(method))
            .ToArray();
        Assert.True(untested.Length == 0,
            "Untested Auth API calls: " + string.Join(", ", untested.Select(FormatMethod)));
    }

    private static string DescribeFailure(StoryResult result)
    {
        var errors = result.Steps
            .Where(step => step.Status == TestStatus.Failed)
            .Select(step => $"{step.Name}: {Masker.Redact(step.Error?.Message)}")
            .ToList();
        if (result.SetupError is not null) errors.Insert(0, $"setup: {Masker.Redact(result.SetupError.Message)}");
        if (result.CleanupError is not null) errors.Add($"cleanup: {Masker.Redact(result.CleanupError.Message)}");
        return $"{result.Name}: {string.Join("; ", errors)}";
    }

    private static string FormatMethod((string Method, CallStyle CallStyle) method) =>
        $"{method.Method} ({method.CallStyle})";

}
