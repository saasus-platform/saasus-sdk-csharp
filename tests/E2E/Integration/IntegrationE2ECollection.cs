using Xunit;

namespace SaasusSdk.Tests.E2E.Integration;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class IntegrationE2ECollection
{
    public const string Name = "Integration E2E";
}
