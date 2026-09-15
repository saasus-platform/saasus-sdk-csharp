namespace SaasusSdk.Tests.TestLib;

public static class MethodExecutor
{
    public static Func<TestContext, CancellationToken, Task<ExecutionResult>> Sync(
        Action<TestContext> call) =>
        (context, token) => Invoke(() =>
        {
            call(context);
            return ExecutionResult.Success();
        }, token);

    public static Func<TestContext, CancellationToken, Task<ExecutionResult>> Sync(
        Func<TestContext, object?> call) =>
        (context, token) => Invoke(() => ExecutionResult.Success(call(context)), token);

    public static Func<TestContext, CancellationToken, Task<ExecutionResult>> WithHttpInfo(
        Func<TestContext, ExecutionResult> call) =>
        (context, token) => Invoke(call, context, token);

    public static Func<TestContext, CancellationToken, Task<ExecutionResult>> Async(
        Func<TestContext, CancellationToken, Task> call) =>
        async (context, token) =>
        {
            try
            {
                await call(context, token).ConfigureAwait(false);
                return ExecutionResult.Success();
            }
            catch (Exception error)
            {
                return FromException(error);
            }
        };

    public static Func<TestContext, CancellationToken, Task<ExecutionResult>> Async(
        Func<TestContext, CancellationToken, Task<object?>> call) =>
        async (context, token) =>
        {
            try
            {
                return ExecutionResult.Success(await call(context, token).ConfigureAwait(false));
            }
            catch (Exception error)
            {
                return FromException(error);
            }
        };

    public static Func<TestContext, CancellationToken, Task<ExecutionResult>> WithHttpInfoAsync(
        Func<TestContext, CancellationToken, Task<ExecutionResult>> call) =>
        async (context, token) =>
        {
            try
            {
                return await call(context, token).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                return FromException(error);
            }
        };

    private static Task<ExecutionResult> Invoke(
        Func<TestContext, ExecutionResult> call,
        TestContext context,
        CancellationToken token) =>
        Task.Run(() =>
        {
            try
            {
                return call(context);
            }
            catch (Exception error)
            {
                return FromException(error);
            }
        }, token);

    private static Task<ExecutionResult> Invoke(Func<ExecutionResult> call, CancellationToken token) =>
        Task.Run(() =>
        {
            try
            {
                return call();
            }
            catch (Exception error)
            {
                return FromException(error);
            }
        }, token);

    /// <summary>
    /// Converts a generated-client exception into a result that keeps the HTTP status code,
    /// error content and response headers, so a failed step still records the response.
    /// </summary>
    public static ExecutionResult FromException(Exception error)
    {
        var type = error.GetType();
        var status = type.GetProperty("ErrorCode")?.GetValue(error) as int?;
        var content = type.GetProperty("ErrorContent")?.GetValue(error);
        var headers = ExtractHeaders(type.GetProperty("Headers")?.GetValue(error));
        return ExecutionResult.Failure(error, status, headers, content?.ToString());
    }

    private static IReadOnlyDictionary<string, IReadOnlyList<string>>? ExtractHeaders(object? value)
    {
        if (value is not System.Collections.IEnumerable entries) return null;
        var result = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase);
        foreach (var entry in entries)
        {
            var type = entry?.GetType();
            var key = type?.GetProperty("Key")?.GetValue(entry)?.ToString();
            var values = type?.GetProperty("Value")?.GetValue(entry) as System.Collections.IEnumerable;
            if (key is null || values is null) continue;
            result[key] = values.Cast<object?>().Select(item => item?.ToString() ?? string.Empty).ToArray();
        }
        return result.Count == 0 ? null : result;
    }
}
