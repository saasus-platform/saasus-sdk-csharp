using Xunit;

namespace SaasusSdk.Tests.E2E.Billing;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class BillingE2ECollection
{
    public const string Name = "Billing E2E";
}
