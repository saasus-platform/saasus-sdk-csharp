using Xunit;
using Xunit.Abstractions;

namespace SaasusSdk.Contract;

// Entry point of the contract test: dotnet test tests/Contract.
//
// Each (operation, call form) is its own theory case so that failures are
// counted and named individually in the report. A module with no Prism endpoint
// contributes no case, which is how one module can be run locally and all of
// them in CI; with no case at all xunit fails the theory for missing data,
// which is intended — the run proved nothing.
public class ContractTest
{
    private readonly ITestOutputHelper _output;

    public ContractTest(ITestOutputHelper output) => _output = output;

    public static IEnumerable<object[]> Cases() =>
        ContractEngine.Cases.Keys.OrderBy(id => id, StringComparer.Ordinal).Select(id => new object[] { id });

    [Theory]
    [MemberData(nameof(Cases))]
    public Task Contract(string caseId) => ContractEngine.Cases[caseId].InvokeAsync();

    [Fact]
    public void ModuleListIsProvided()
    {
        Assert.True(ContractEngine.Modules.Length > 0,
            "CONTRACT_MODULES is empty: it carries the module list from the fetch-openapi-spec action"
            + " (locally: CONTRACT_MODULES=\"auth\" dotnet test tests/Contract)");
    }

    // A module added to generate.sh but not to the shared module list would
    // contribute no case, so all of its operations would go unverified.
    [Fact]
    public void EveryGeneratedModuleIsUnderTest()
    {
        var untested = ContractEngine.GeneratedModules()
            .Except(ContractEngine.Modules, StringComparer.Ordinal)
            .ToList();
        Assert.True(untested.Count == 0, "modules generated into the SDK but absent from CONTRACT_MODULES"
            + " (their operations would go unverified): " + string.Join(", ", untested));
    }

    [Fact]
    public void ConfiguredModulesHaveSpec()
    {
        var missing = ContractEngine.ModulesMissingSpec().ToList();
        Assert.True(missing.Count == 0, "modules with a Prism mock but no spec in CONTRACT_SPEC_DIR"
            + " (their operations would go unverified): " + string.Join(", ", missing));
    }

    // Enforced only for the latest SDK (B): an older SDK (C) legitimately lacks
    // operations added to the spec after it was released.
    [Fact]
    public void AllOperationsEnumerated()
    {
        if (!string.Equals(Environment.GetEnvironmentVariable("CONTRACT_REQUIRE_COMPLETE"), "true",
                StringComparison.OrdinalIgnoreCase))
        {
            _output.WriteLine("CONTRACT_REQUIRE_COMPLETE is not set:"
                + " completeness is only enforced for the latest SDK");
            return;
        }
        var missing = ContractEngine.Modules
            .Where(module => PrismEndpoints.UrlForOrNull(module) != null)
            .SelectMany(ContractEngine.UnmappedOperations)
            .ToList();
        Assert.True(missing.Count == 0, "spec operations not enumerated in SDK: " + string.Join(", ", missing));
    }
}
