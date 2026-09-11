using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Auth;

/// <summary>
/// Covers the response contract of <see cref="AuthResponseValidator"/> directly. The offline story
/// tests drive a fake invoker, so without these the void and typed contracts would be unexercised
/// and a removed check would stay green.
/// </summary>
public sealed class AuthResponseValidatorTests
{
    [Fact]
    public void AVoidContractRejectsAPayload()
    {
        var context = new TestContext(new Dictionary<string, object?>());

        // The generated WithHttpInfo method of a void operation returns ApiResponse<object>, so
        // typeof(object) is a void contract rather than "anything goes".
        foreach (var expected in new[] { null, typeof(object) })
        {
            var error = Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.Validate(
                "DeleteTenant", expected, ExecutionResult.WithHttp(new { unexpected = true }, 200), context));
            Assert.Contains("expected a void response", error.Message, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void AVoidContractAcceptsNoPayload()
    {
        var context = new TestContext(new Dictionary<string, object?>());

        AuthResponseValidator.Validate("DeleteTenant", null, ExecutionResult.WithHttp(null, 200), context);
        AuthResponseValidator.Validate("DeleteTenant", typeof(object), ExecutionResult.WithHttp(null, 200), context);
    }

    [Fact]
    public void AVoidContractAcceptsTheBodyOfAnOperationThatAnswersWithOne()
    {
        var context = new TestContext(new Dictionary<string, object?>());

        // DeleteSaasUser declares no response schema but answers with the removed user, so the
        // generated ApiResponse<object> carries a deserialized body on a successful call.
        AuthResponseValidator.Validate(
            "DeleteSaasUser", typeof(object), ExecutionResult.WithHttp(new { id = "user-1" }, 200), context);
    }

    [Fact]
    public void ATypedContractRejectsAMissingOrMismatchedResponse()
    {
        var context = new TestContext(new Dictionary<string, object?>());

        var missing = Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.Validate(
            "GetBasicInfo", typeof(authapi.Model.BasicInfo), ExecutionResult.WithHttp(null, 200), context));
        Assert.Contains("returned no response", missing.Message, StringComparison.Ordinal);

        var mismatched = Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.Validate(
            "GetBasicInfo", typeof(authapi.Model.BasicInfo), ExecutionResult.WithHttp("not a model", 200), context));
        Assert.Contains("expected BasicInfo", mismatched.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void RawValidationRequiresAJsonEnvelope()
    {
        // No headers at all.
        Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.ValidateRaw(
            "GetBasicInfo", CallStyle.WithHttpInfo, ExecutionResult.WithHttp(new object(), 200)));

        var json = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Content-Type"] = new[] { "application/json" }
        };

        // A JSON content type but an empty body.
        Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.ValidateRaw(
            "GetBasicInfo", CallStyle.WithHttpInfo,
            ExecutionResult.WithHttp(new object(), 200, json) with { Body = string.Empty }));

        // A malformed body.
        Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.ValidateRaw(
            "GetBasicInfo", CallStyle.WithHttpInfo,
            ExecutionResult.WithHttp(new object(), 200, json) with { Body = "{ not json" }));

        // The object call styles expose no HTTP envelope at all.
        Assert.Throws<InvalidOperationException>(() => AuthResponseValidator.ValidateRaw(
            "GetBasicInfo", CallStyle.Sync,
            ExecutionResult.WithHttp(new object(), 200, json) with { Body = "{}" }));

        AuthResponseValidator.ValidateRaw("GetBasicInfo", CallStyle.WithHttpInfo,
            ExecutionResult.WithHttp(new object(), 200, json) with { Body = "{\"ok\":true}" });
    }
}
