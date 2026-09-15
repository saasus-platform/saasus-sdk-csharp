using System.Diagnostics;

namespace SaasusSdk.Tests.TestLib;

internal static class GitInfo
{
    private const int TimeoutMilliseconds = 3000;

    public static string? ExactTag() => Run("describe --tags --exact-match");
    public static string? Describe() => Run("describe --tags --always");
    public static string Commit() => Run("rev-parse HEAD") ?? string.Empty;

    private static string? Run(string arguments)
    {
        try
        {
            using var process = Process.Start(new ProcessStartInfo("git", arguments)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            });
            if (process is null) return null;

            // Both pipes must be drained while the process runs: reading one to the end first
            // deadlocks when the other fills its buffer, and the timeout would never be reached.
            var standardOutput = process.StandardOutput.ReadToEndAsync();
            var standardError = process.StandardError.ReadToEndAsync();
            if (!process.WaitForExit(TimeoutMilliseconds))
            {
                try { process.Kill(entireProcessTree: true); } catch (InvalidOperationException) { }
                return null;
            }
            if (!Task.WaitAll(new Task[] { standardOutput, standardError }, TimeoutMilliseconds)) return null;

            var output = standardOutput.Result.Trim();
            return process.ExitCode == 0 && output.Length > 0 ? output : null;
        }
        catch { return null; }
    }
}
