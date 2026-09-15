namespace SaasusSdk.Tests.TestLib;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class SnapshotFactAttribute : FactAttribute
{
    public SnapshotFactAttribute() => Skip = ResolveSkipReason();

    internal static string? ResolveSkipReason(
        IReadOnlyDictionary<string, string?>? environment = null)
    {
        var snapshot = Config.FromEnvironment(environment).Snapshot;
        if (snapshot is null)
            return "Configure snapshot mode to capture, compare, report, or full.";

        string? Get(string key) => environment is null
            ? Environment.GetEnvironmentVariable(key)
            : environment.TryGetValue(key, out var value) ? value : null;
        if (snapshot.Mode is SnapshotMode.Capture or SnapshotMode.Full &&
            (!bool.TryParse(Get("SAASUS_E2E"), out var enabled) || !enabled))
            return "Set SAASUS_E2E=true for snapshot modes that call the live API.";

        return null;
    }
}
