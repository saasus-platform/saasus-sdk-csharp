using communicationapi.Api;
using SaasusSdk.Tests.TestLib;
using Xunit.Abstractions;

namespace SaasusSdk.Tests.E2E.Communication;

[Collection(CommunicationE2ECollection.Name)]
public sealed class CommunicationApiTests
{
    private readonly ITestOutputHelper _output;

    public CommunicationApiTests(ITestOutputHelper output) => _output = output;

    [E2EFact]
    [Trait("Category", "E2E")]
    public async Task CommunicationApiStories()
    {
        var config = Config.FromEnvironment();
        config.Validate();

        var moduleConfiguration = new modules.Configuration().GetCommunicationApiClientConfig();
        var client = new CommunicationClientAdapter(new FeedbackApi(moduleConfiguration));
        var stories = CommunicationStoryFactory.Create(client)
            .Concat(CommunicationStoryFactory.CreateRaw(client))
            .ToArray();
        var coverage = new CoverageTracker();
        foreach (var method in CommunicationStoryFactory.Methods)
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
            $"Untested Communication API calls ({coverage.Coverage.Percentage:F1}% covered): " +
            $"{string.Join(", ", coverage.Untested)}");
    }

    private static string DescribeFailure(StoryResult result)
    {
        var details = result.Steps
            .Where(step => step.Status == TestStatus.Failed)
            .Select(step => $"{step.Name}: {Masker.Redact(step.Error?.Message)}");
        if (result.SetupError is not null)
            details = new[] { $"setup: {Masker.Redact(result.SetupError.Message)}" }.Concat(details);
        if (result.CleanupError is not null)
            details = details.Concat(new[] { $"cleanup: {Masker.Redact(result.CleanupError.Message)}" });
        return $"{result.Name}: {string.Join("; ", details)}";
    }

}
