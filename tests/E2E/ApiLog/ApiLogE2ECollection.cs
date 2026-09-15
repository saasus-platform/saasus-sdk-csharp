using Xunit;

namespace SaasusSdk.Tests.E2E.ApiLog;

[CollectionDefinition(Name, DisableParallelization = true)]
public sealed class ApiLogE2ECollection
{
    public const string Name = "ApiLog E2E";
}
