using Newtonsoft.Json.Linq;
using pricingapi.Api;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Pricing;

public sealed class PricingStoryFactoryTests
{
    [Fact]
    public async Task ExecutesEveryPricingMethodAndCleansUpAllResources()
    {
        var client = new FakePricingClient();
        var stories = PricingStoryFactory.Create(client);
        var coverage = new CoverageTracker();
        foreach (var method in PricingStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);

        var results = await new E2EEngine(new Config
        {
            SaasId = "saas",
            ApiKey = "api",
            SecretKey = "secret",
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }, coverage).ExecuteAsync(stories);

        Assert.Equal(4, results.Count);
        Assert.Equal(132, results.Sum(result => result.Steps.Count));
        Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
        Assert.Empty(coverage.Untested);
        Assert.Equal(120, coverage.Entries.Count);
        Assert.True(client.IsEmpty);
    }

    [Fact]
    public void DeclaresExactlyTheGeneratedPricingMethodsExceptExcludedOperations()
    {
        var apiInterfaces = new[]
        {
            typeof(IPricingUnitsApi),
            typeof(IPricingMenusApi),
            typeof(IPricingPlansApi),
            typeof(IMeteringApi),
            typeof(ITaxRateApi)
        };
        var generatedMethods = apiInterfaces
            .SelectMany(type => type.GetInterfaces().SelectMany(interfaceType => interfaceType.GetMethods()))
            .Where(method => IsGeneratedApiMethod(method.Name))
            .Where(method => method.Name != "GetBasePath")
            .Where(method => !PricingStoryFactory.IsExcluded(method.Name))
            .Select(method => method.Name)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        var declaredMethods = PricingStoryFactory.Methods
            .Select(method => method.Method)
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(generatedMethods, declaredMethods);
    }

    [Fact]
    public void StoriesCoverEveryDeclaredMethodAndCallStyle()
    {
        var covered = PricingStoryFactory.Create(new FakePricingClient())
            .SelectMany(story => story.Steps)
            .Select(step => (step.Method, step.CallStyle))
            .Distinct()
            .OrderBy(method => method.Method, StringComparer.Ordinal)
            .ThenBy(method => method.CallStyle)
            .ToArray();
        var declared = PricingStoryFactory.Methods
            .Distinct()
            .OrderBy(method => method.Method, StringComparer.Ordinal)
            .ThenBy(method => method.CallStyle)
            .ToArray();

        Assert.Equal(declared, covered);
    }

    [Fact]
    public async Task SnapshotParametersAreNamedAndNormalised()
    {
        var client = new FakePricingClient();
        var stories = PricingStoryFactory.Create(client);
        var story = stories.Single(value => value.Name == "Pricing API - synchronous responses");

        var results = await new E2EEngine(OfflineConfig()).ExecuteAsync(new[] { story });
        var snapshot = SnapshotFactory.Create(story, results.Single(), new SnapshotConfig());

        // Snapshot parameters use the shared object shape, never the positional argument
        // array, and the generated operationIndex argument stays out of the artifact.
        Assert.All(snapshot.Steps, step => Assert.Equal(JTokenType.Object, step.Parameters.Type));
        Assert.DoesNotContain("0", snapshot.Steps
            .SelectMany(step => ((JObject)step.Parameters).Properties())
            .Select(property => property.Name));

        var read = Parameters(snapshot, "GetPricingUnit");
        Assert.Equal(new[] { "pricing_unit_id" }, read.Properties().Select(property => property.Name));
        Assert.Equal(SnapshotMasker.DynamicToken, read["pricing_unit_id"]!.Value<string>());

        var tieredRead = Parameters(snapshot, "GetPricingUnit (tiered usage)");
        Assert.Equal(SnapshotMasker.DynamicToken, tieredRead["tiered_pricing_unit_id"]!.Value<string>());

        // Request bodies keep their shape while volatile values are normalised.
        var menu = (JObject)Parameters(snapshot, "CreatePricingMenu")["body"]!;
        Assert.Equal(SnapshotMasker.DynamicToken, menu["name"]!.Value<string>());
        Assert.Equal("C# E2E Menu", menu["display_name"]!.Value<string>());
        Assert.Equal(
            new[] { SnapshotMasker.DynamicToken, SnapshotMasker.DynamicToken },
            menu["unit_ids"]!.Values<string>());

        // The oneOf pricing unit body is serialised as a raw JSON payload, which must be
        // normalised too instead of being copied verbatim.
        var unit = (JObject)Parameters(snapshot, "CreatePricingUnit")["body"]!;
        Assert.Equal(SnapshotMasker.DynamicToken, unit["name"]!.Value<string>());
        Assert.Equal("fixed", unit["type"]!.Value<string>());
        Assert.Equal(1000, unit["unit_amount"]!.Value<int>());

        var period = Parameters(snapshot, "GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod");
        // Parameter names are canonicalised into ordinal order like every other payload.
        Assert.Equal(
            new[] { "end_timestamp", "metering_unit_name", "start_timestamp", "tenant_id" },
            period.Properties().Select(property => property.Name));
        Assert.Equal(0, period["start_timestamp"]!.Value<int>());
        Assert.Equal(0, period["end_timestamp"]!.Value<int>());
    }

    private static JObject Parameters(StorySnapshot snapshot, string stepName) =>
        (JObject)snapshot.Steps.Single(step => step.StepName == stepName).Parameters;

    private static Config OfflineConfig() => new()
    {
        SaasId = "saas",
        ApiKey = "api",
        SecretKey = "secret",
        Timeout = TimeSpan.FromSeconds(2),
        MaxRetries = 0,
        LogLevel = LogLevel.None
    };

    private static bool IsGeneratedApiMethod(string name) =>
        name.StartsWith("Create", StringComparison.Ordinal) ||
        name.StartsWith("Delete", StringComparison.Ordinal) ||
        name.StartsWith("Get", StringComparison.Ordinal) ||
        name.StartsWith("Update", StringComparison.Ordinal) ||
        name.StartsWith("Link", StringComparison.Ordinal);
}
