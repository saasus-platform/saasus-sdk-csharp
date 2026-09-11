using pricingapi.Api;
using SaasusSdk.Tests.TestLib;
using Xunit.Abstractions;

namespace SaasusSdk.Tests.E2E.Pricing;

[Collection(PricingE2ECollection.Name)]
public sealed class PricingApiTests
{
    private readonly ITestOutputHelper _output;

    public PricingApiTests(ITestOutputHelper output) => _output = output;

    [E2EFact]
    [Trait("Category", "E2E")]
    public async Task PricingApiStories()
    {
        var config = Config.FromEnvironment();
        config.Validate();

        var moduleConfiguration = new modules.Configuration().GetPricingApiClientConfig();
        var client = new PricingApiClient(
            new PricingUnitsApi(moduleConfiguration),
            new PricingMenusApi(moduleConfiguration),
            new PricingPlansApi(moduleConfiguration),
            new MeteringApi(moduleConfiguration),
            new TaxRateApi(moduleConfiguration));
        var coverage = new CoverageTracker();
        foreach (var method in PricingStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);

        var results = await new E2EEngine(config, coverage).ExecuteAsync(PricingStoryFactory.Create(client));
        _output.WriteLine(new Reporter().Render(results, coverage));
        if (!string.IsNullOrWhiteSpace(config.ReportJsonPath))
        {
            await new Reporter().WriteJsonAsync(config.ReportJsonPath, results, coverage);
            _output.WriteLine($"JSON report written to {config.ReportJsonPath}");
        }

        var failures = results.Where(result => result.Status == TestStatus.Failed).ToArray();
        Assert.True(failures.Length == 0,
            string.Join(Environment.NewLine, failures.Select(DescribeFailure)));
        Assert.True(coverage.IsFullyCovered,
            $"Untested Pricing API calls ({coverage.Coverage.Percentage:F1}% covered): " +
            string.Join(", ", coverage.Untested));
    }

    private static string DescribeFailure(StoryResult result)
    {
        var details = result.Steps
            .Where(step => step.Status == TestStatus.Failed)
            .Select(step => $"{step.Name}: {step.Error?.Message ?? "unknown error"}");
        return $"{result.Name}: {string.Join("; ", details)}";
    }
}
