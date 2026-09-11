using Xunit;

namespace SaasusSdk.Tests.E2E.Communication;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class CommunicationE2ECollection
{
    public const string Name = "Communication E2E";
}
