using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

public sealed class CoverageAndReporterTests
{
    [Fact]
    public void TracksCallStyleCoverageAndUntestedMethods()
    {
        var coverage = new CoverageTracker();
        coverage.Register("Get", CallStyle.Async);
        coverage.Register("Get", CallStyle.WithHttpInfoAsync);
        coverage.Record("Get", CallStyle.Async, true, TimeSpan.FromMilliseconds(5));

        var entry = Assert.Single(coverage.Entries);
        Assert.Equal(1, entry.Executions);
        Assert.Equal(1, entry.Successes);
        Assert.Equal(("Get", CallStyle.WithHttpInfoAsync), Assert.Single(coverage.Untested));

        var report = new Reporter().Render(Array.Empty<StoryResult>(), coverage);
        Assert.Contains("UNTESTED Get [WithHttpInfoAsync]", report);
    }
}
