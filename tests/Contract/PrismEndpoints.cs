namespace SaasusSdk.Contract;

// PRISM_URL_<MODULE> is exported by the setup-prism action in CI and set by
// hand for a local run:
//   CONTRACT_MODULES=auth PRISM_URL_AUTH=http://127.0.0.1:4010 dotnet test tests/Contract
internal static class PrismEndpoints
{
    // Null when the module has no mock configured; it then contributes no case.
    internal static string? UrlForOrNull(string module)
    {
        var url = Environment.GetEnvironmentVariable("PRISM_URL_" + module.ToUpperInvariant());
        return string.IsNullOrEmpty(url) ? null : url;
    }
}
