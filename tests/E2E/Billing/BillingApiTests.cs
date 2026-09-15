using billingapi.Api;
using SaasusSdk.Tests.TestLib;
using Xunit.Abstractions;

namespace SaasusSdk.Tests.E2E.Billing;

[Collection(BillingE2ECollection.Name)]
public sealed class BillingApiTests
{
    private readonly ITestOutputHelper _output;

    public BillingApiTests(ITestOutputHelper output) => _output = output;

    [E2EFact]
    [Trait("Category", "E2E")]
    public async Task BillingApiStories()
    {
        var config = Config.FromEnvironment();
        config.Validate();
        if (string.IsNullOrWhiteSpace(config.StripeSecretKey))
            throw new InvalidOperationException(
                "STRIPE_SECRET_KEY is required to run the Billing API E2E test.");

        var moduleConfiguration = new modules.Configuration().GetBillingApiClientConfig();
        var client = new BillingStripeClient(new StripeApi(moduleConfiguration));
        var stories = BillingStoryFactory.Create(client, config.StripeSecretKey);
        var coverage = new CoverageTracker();
        foreach (var method in BillingStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);

        var engine = new E2EEngine(config, coverage);
        var results = await engine.ExecuteAsync(stories);
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
        Assert.True(coverage.IsFullyCovered,
            $"Untested Billing API calls ({coverage.Coverage.Percentage:F1}% covered): " +
            $"{string.Join(", ", coverage.Untested)}");
    }

    private static string DescribeFailure(StoryResult result)
    {
        var details = result.Steps
            .Where(step => step.Status == TestStatus.Failed)
            .Select(step => $"{step.Name}: {Masker.Redact(step.Error?.Message)}");
        return $"{result.Name}: {string.Join("; ", details)}";
    }

}
