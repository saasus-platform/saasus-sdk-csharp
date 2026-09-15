namespace SaasusSdk.Tests.TestLib;

public sealed class TestContext
{
    public TestContext(IDictionary<string, object?>? variables = null)
    {
        Variables = variables ?? new Dictionary<string, object?>();
    }

    public IDictionary<string, object?> Variables { get; }

    public T GetRequired<T>(string key)
    {
        if (!Variables.TryGetValue(key, out var value) || value is not T typed)
        {
            throw new KeyNotFoundException($"Story variable '{key}' is missing or is not {typeof(T).Name}.");
        }

        return typed;
    }
}

public sealed class Step
{
    public required string Name { get; init; }
    public required string Method { get; init; }
    public CallStyle CallStyle { get; init; } = CallStyle.Async;
    public required Func<TestContext, CancellationToken, Task<ExecutionResult>> ExecuteAsync { get; init; }
    public object? Parameters { get; init; }
    public int? ExpectedStatus { get; init; }
    public IReadOnlyCollection<int> AllowedStatuses { get; init; } = Array.Empty<int>();
    public Func<ExecutionResult, TestContext, Task>? ValidateAsync { get; init; }
    public Func<ExecutionResult, TestContext, Task>? UpdateStateAsync { get; init; }
    public bool Skip { get; init; }
    public string SkipReason { get; init; } = string.Empty;
}

public sealed class Story
{
    public required string Name { get; init; }
    public string Description { get; init; } = string.Empty;
    public string Module { get; init; } = string.Empty;
    public IReadOnlyList<Step> Steps { get; init; } = Array.Empty<Step>();
    public IReadOnlyDictionary<string, object?> InitialVariables { get; init; } =
        new Dictionary<string, object?>();
    public Func<TestContext, CancellationToken, Task>? SetupAsync { get; init; }
    public Func<TestContext, CancellationToken, Task>? CleanupAsync { get; init; }
    public TimeSpan? Timeout { get; init; }
}
