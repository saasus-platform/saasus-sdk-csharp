using Xunit;

namespace SaasusSdk.Tests.E2E.Pricing;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class PricingE2ECollection
{
    public const string Name = "Pricing E2E";
}
