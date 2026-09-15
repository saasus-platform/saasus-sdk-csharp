using Xunit;

namespace SaasusSdk.Tests.E2E.Auth;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class AuthE2ECollection
{
    public const string Name = "Auth E2E";
}
