using integrationapi.Api;
using SaasusSdk.Tests.TestLib;
using Xunit.Abstractions;

namespace SaasusSdk.Tests.E2E.Integration;

[Collection(IntegrationE2ECollection.Name)]
public sealed class IntegrationApiTests
{
    private readonly ITestOutputHelper _output;

    public IntegrationApiTests(ITestOutputHelper output) => _output = output;

    [E2EFact]
    [Trait("Category", "E2E")]
    public async Task IntegrationApiStories()
    {
        var config = Config.FromEnvironment();
        config.Validate();
        var moduleConfiguration = new modules.Configuration().GetIntegrationApiClientConfig();
        var client = new IntegrationEventBridgeClient(new EventBridgeApi(moduleConfiguration));
        var stories = IntegrationStoryFactory.Create(client);
        var coverage = new CoverageTracker();
        foreach (var method in IntegrationStoryFactory.Methods)
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
            $"Untested Integration API calls ({coverage.Coverage.Percentage:F1}% covered): " +
            $"{string.Join(", ", coverage.Untested.Select(entry => $"{entry.Method} [{entry.CallStyle}]"))}");
    }

    private static string DescribeFailure(StoryResult result)
    {
        var details = result.Steps
            .Where(step => step.Status == TestStatus.Failed)
            .Select(step => $"{step.Name}: {Masker.Redact(step.Error?.Message)}");
        return $"{result.Name}: {string.Join("; ", details)}";
    }

}
