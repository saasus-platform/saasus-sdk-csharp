using System.Collections.Concurrent;

namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Tracks which generated SDK methods were exercised, per call style, and exposes the
/// percentage plus per-method statistics reported by the Go and PHP suites.
/// </summary>
public sealed class CoverageTracker
{
    private sealed class MutableEntry
    {
        public int Executions;
        public int Successes;
        public int Failures;
        public long DurationTicks;
    }

    private readonly ConcurrentDictionary<(string Method, CallStyle Style), MutableEntry> _entries = new();
    private readonly HashSet<(string Method, CallStyle Style)> _expected = new();
    private readonly object _expectedLock = new();

    public void Register(string method, CallStyle callStyle)
    {
        lock (_expectedLock)
        {
            _expected.Add((method, callStyle));
        }
    }

    public void Record(string method, CallStyle callStyle, bool success, TimeSpan duration)
    {
        Register(method, callStyle);
        var entry = _entries.GetOrAdd((method, callStyle), _ => new MutableEntry());
        Interlocked.Increment(ref entry.Executions);
        Interlocked.Add(ref entry.DurationTicks, duration.Ticks);
        if (success)
            Interlocked.Increment(ref entry.Successes);
        else
            Interlocked.Increment(ref entry.Failures);
    }

    public IReadOnlyList<CoverageEntry> Entries => _entries
        .OrderBy(pair => pair.Key.Method, StringComparer.Ordinal)
        .ThenBy(pair => pair.Key.Style)
        .Select(pair => new CoverageEntry(
            pair.Key.Method,
            pair.Key.Style,
            pair.Value.Executions,
            pair.Value.Successes,
            pair.Value.Failures,
            TimeSpan.FromTicks(pair.Value.DurationTicks)))
        .ToArray();

    public IReadOnlyList<(string Method, CallStyle CallStyle)> Untested
    {
        get
        {
            lock (_expectedLock)
            {
                return _expected
                    .Where(key => !_entries.TryGetValue(key, out var value) || value.Executions == 0)
                    .OrderBy(key => key.Method, StringComparer.Ordinal)
                    .ThenBy(key => key.Style)
                    .ToArray();
            }
        }
    }

    /// <summary>Number of registered method/call-style pairs, whether executed or not.</summary>
    public int Registered
    {
        get { lock (_expectedLock) { return _expected.Count; } }
    }

    /// <summary>
    /// Covered and total registered calls with the percentage. An empty registration set
    /// counts as fully covered, matching PHP's <c>getCoverage</c>.
    /// </summary>
    public CoverageSummary Coverage
    {
        get
        {
            var total = Registered;
            var covered = total - Untested.Count;
            return new CoverageSummary(covered, total, total == 0 ? 100d : Math.Round(covered * 100d / total, 1));
        }
    }

    public bool IsFullyCovered => Untested.Count == 0;

    /// <summary>Execution count, success rate and average duration for one method/call-style pair.</summary>
    public MethodStats Stats(string method, CallStyle callStyle)
    {
        if (!_entries.TryGetValue((method, callStyle), out var entry) || entry.Executions == 0)
            return new MethodStats(method, callStyle, 0, 0d, TimeSpan.Zero);
        return new MethodStats(method, callStyle, entry.Executions,
            Math.Round(entry.Successes * 100d / entry.Executions, 1),
            TimeSpan.FromTicks(entry.DurationTicks / entry.Executions));
    }

    /// <summary>Statistics for every executed method/call-style pair.</summary>
    public IReadOnlyList<MethodStats> AllStats => Entries.Select(entry => Stats(entry.Method, entry.CallStyle)).ToArray();
}

public sealed record CoverageSummary(int Covered, int Total, double Percentage);

public sealed record MethodStats(string Method, CallStyle CallStyle, int Executions, double SuccessRate, TimeSpan AverageDuration);
