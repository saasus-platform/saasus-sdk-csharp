using System.Net;
using billingapi.Client;
using billingapi.Model;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Billing;

public sealed class BillingStoryFactoryTests
{
    [Fact]
    public async Task ExecutesEveryBillingMethodAndLeavesStripeUnregistered()
    {
        var client = new FakeBillingStripeClient();
        var stories = BillingStoryFactory.Create(client, "sk_test_offline_fixture");
        var coverage = new CoverageTracker();
        foreach (var method in BillingStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);

        var results = await new E2EEngine(new Config
        {
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }, coverage).ExecuteAsync(stories);

        Assert.Equal(4, results.Count);
        Assert.Equal(20, results.Sum(result => result.Steps.Count));
        Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
        Assert.Empty(coverage.Untested);
        Assert.Equal(12, coverage.Entries.Count);
        Assert.False(client.IsRegistered);
    }

    [Fact]
    public async Task FailsWhenAVoidBillingCallReturnsAPayload()
    {
        // An empty string carries no JToken children, so only a strict null check rejects it.
        var client = new FakeBillingStripeClient(voidPayload: string.Empty);
        var results = await new E2EEngine(new Config
        {
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }).ExecuteAsync(BillingStoryFactory.Create(client, "sk_test_offline_fixture"));

        // Only the WithHttpInfo call styles carry the response payload; the object styles are void.
        var failed = results.Where(result => result.Status == TestStatus.Failed).ToArray();
        Assert.Equal(2, failed.Length);
        Assert.All(failed, result => Assert.Contains("HTTP responses", result.Name, StringComparison.Ordinal));
    }

    [Fact]
    public void DeclaresExactlyTheGeneratedStripeApiMethods()
    {
        var generatedMethods = typeof(billingapi.Api.IStripeApi)
            .GetInterfaces()
            .SelectMany(type => type.GetMethods())
            .Select(method => method.Name)
            .Where(name => name.Contains("StripeInfo", StringComparison.Ordinal))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        var declaredMethods = BillingStoryFactory.Methods
            .Select(method => method.Method)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(generatedMethods, declaredMethods);
    }

    private sealed class FakeBillingStripeClient : IBillingStripeClient
    {
        private readonly object? _voidPayload;

        public FakeBillingStripeClient(object? voidPayload = null) => _voidPayload = voidPayload;

        public bool IsRegistered { get; private set; }

        public StripeInfo GetStripeInfo() => new(IsRegistered);
        public ApiResponse<StripeInfo> GetStripeInfoWithHttpInfo() => Response(GetStripeInfo());
        public Task<StripeInfo> GetStripeInfoAsync(CancellationToken cancellationToken) =>
            Task.FromResult(GetStripeInfo());
        public Task<ApiResponse<StripeInfo>> GetStripeInfoWithHttpInfoAsync(
            CancellationToken cancellationToken) => Task.FromResult(GetStripeInfoWithHttpInfo());

        public void UpdateStripeInfo(UpdateStripeInfoParam parameter)
        {
            Assert.StartsWith("sk_test_", parameter.SecretKey, StringComparison.Ordinal);
            IsRegistered = true;
        }

        public ApiResponse<object> UpdateStripeInfoWithHttpInfo(UpdateStripeInfoParam parameter)
        {
            UpdateStripeInfo(parameter);
            return VoidResponse();
        }

        public Task UpdateStripeInfoAsync(
            UpdateStripeInfoParam parameter,
            CancellationToken cancellationToken)
        {
            UpdateStripeInfo(parameter);
            return Task.CompletedTask;
        }

        public Task<ApiResponse<object>> UpdateStripeInfoWithHttpInfoAsync(
            UpdateStripeInfoParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(UpdateStripeInfoWithHttpInfo(parameter));

        public void DeleteStripeInfo() => IsRegistered = false;
        public ApiResponse<object> DeleteStripeInfoWithHttpInfo()
        {
            DeleteStripeInfo();
            return VoidResponse();
        }
        public Task DeleteStripeInfoAsync(CancellationToken cancellationToken)
        {
            DeleteStripeInfo();
            return Task.CompletedTask;
        }
        public Task<ApiResponse<object>> DeleteStripeInfoWithHttpInfoAsync(
            CancellationToken cancellationToken) => Task.FromResult(DeleteStripeInfoWithHttpInfo());

        // The raw body is part of the contract the validators check, so the fake mirrors it.
        private static ApiResponse<StripeInfo> Response(StripeInfo data) =>
            new(HttpStatusCode.OK, new Multimap<string, string>(), data,
                $"{{\"is_registered\":{(data.IsRegistered ? "true" : "false")}}}");

        private ApiResponse<object> VoidResponse() =>
            new(HttpStatusCode.OK, new Multimap<string, string>(), _voidPayload!, string.Empty);
    }
}
