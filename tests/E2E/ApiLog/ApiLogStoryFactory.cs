using System.Globalization;
using apilogapi.Api;
using apilogapi.Client;
using apilogapi.Model;
using SaasusSdk.Tests.TestLib;
using ApiLogModel = apilogapi.Model.ApiLog;

namespace SaasusSdk.Tests.E2E.ApiLog;

internal interface IApiLogClient
{
    ApiLogs GetLogs(
        DateTime? createdDate = null,
        DateTime? createdAt = null,
        long? limit = null,
        string? cursor = null);

    ApiResponse<ApiLogs> GetLogsWithHttpInfo(
        DateTime? createdDate = null,
        DateTime? createdAt = null,
        long? limit = null,
        string? cursor = null);

    Task<ApiLogs> GetLogsAsync(
        DateTime? createdDate = null,
        DateTime? createdAt = null,
        long? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);

    Task<ApiResponse<ApiLogs>> GetLogsWithHttpInfoAsync(
        DateTime? createdDate = null,
        DateTime? createdAt = null,
        long? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default);

    ApiLogModel GetLog(string apiLogId);
    ApiResponse<ApiLogModel> GetLogWithHttpInfo(string apiLogId);
    Task<ApiLogModel> GetLogAsync(string apiLogId, CancellationToken cancellationToken = default);
    Task<ApiResponse<ApiLogModel>> GetLogWithHttpInfoAsync(
        string apiLogId,
        CancellationToken cancellationToken = default);
}

internal sealed class ApiLogClient : IApiLogClient
{
    private readonly IApiLogApi _api;

    public ApiLogClient(IApiLogApi api) => _api = api;

    public ApiLogs GetLogs(
        DateTime? createdDate = null,
        DateTime? createdAt = null,
        long? limit = null,
        string? cursor = null) =>
        _api.GetLogs(createdDate, createdAt, limit, cursor);

    public ApiResponse<ApiLogs> GetLogsWithHttpInfo(
        DateTime? createdDate = null,
        DateTime? createdAt = null,
        long? limit = null,
        string? cursor = null) =>
        _api.GetLogsWithHttpInfo(createdDate, createdAt, limit, cursor);

    public Task<ApiLogs> GetLogsAsync(
        DateTime? createdDate = null,
        DateTime? createdAt = null,
        long? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default) =>
        _api.GetLogsAsync(createdDate, createdAt, limit, cursor, cancellationToken: cancellationToken);

    public Task<ApiResponse<ApiLogs>> GetLogsWithHttpInfoAsync(
        DateTime? createdDate = null,
        DateTime? createdAt = null,
        long? limit = null,
        string? cursor = null,
        CancellationToken cancellationToken = default) =>
        _api.GetLogsWithHttpInfoAsync(
            createdDate, createdAt, limit, cursor, cancellationToken: cancellationToken);

    public ApiLogModel GetLog(string apiLogId) => _api.GetLog(apiLogId);
    public ApiResponse<ApiLogModel> GetLogWithHttpInfo(string apiLogId) => _api.GetLogWithHttpInfo(apiLogId);
    public Task<ApiLogModel> GetLogAsync(string apiLogId, CancellationToken cancellationToken = default) =>
        _api.GetLogAsync(apiLogId, cancellationToken: cancellationToken);

    public Task<ApiResponse<ApiLogModel>> GetLogWithHttpInfoAsync(
        string apiLogId,
        CancellationToken cancellationToken = default) =>
        _api.GetLogWithHttpInfoAsync(apiLogId, cancellationToken: cancellationToken);
}

internal static class ApiLogStoryFactory
{
    private const int EmptyResponseRetries = 5;
    private static readonly TimeSpan EmptyResponseDelay = TimeSpan.FromMilliseconds(200);

    internal static readonly IReadOnlyList<(string Method, CallStyle CallStyle)> Methods = new[]
    {
        ("GetLogs", CallStyle.Sync),
        ("GetLog", CallStyle.Sync),
        ("GetLogsWithHttpInfo", CallStyle.WithHttpInfo),
        ("GetLogWithHttpInfo", CallStyle.WithHttpInfo),
        ("GetLogsAsync", CallStyle.Async),
        ("GetLogAsync", CallStyle.Async),
        ("GetLogsWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("GetLogWithHttpInfoAsync", CallStyle.WithHttpInfoAsync)
    };

    public static IReadOnlyList<Story> Create(IApiLogClient client, long? logLimit = null) =>
        Enum.GetValues<CallStyle>()
            .Select(callStyle => CreateStory(client, callStyle, logLimit))
            .ToArray();

    private static Story CreateStory(IApiLogClient client, CallStyle callStyle, long? logLimit)
    {
        var responseDescription = callStyle switch
        {
            CallStyle.Sync => "synchronous responses",
            CallStyle.WithHttpInfo => "synchronous HTTP responses",
            CallStyle.Async => "asynchronous responses",
            CallStyle.WithHttpInfoAsync => "asynchronous HTTP responses",
            _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
        };

        var variables = new Dictionary<string, object?>
        {
            ["api_log_id"] = string.Empty,
            ["created_date"] = string.Empty,
            ["created_at"] = string.Empty,
            ["cursor"] = string.Empty,
            ["timestamp"] = 0
        };
        if (logLimit.HasValue) variables["limit"] = logLimit.Value;

        return new Story
        {
            Name = $"ApiLog API - {responseDescription}",
            Description = "Retrieves API logs, filters by extracted values, and retrieves a log by ID.",
            Module = "apilog",
            InitialVariables = variables,
            Steps = new[]
            {
                GetLogsStep(
                    "Pre_GetApiLogs", client, callStyle, useQueryParameters: false, updateState: false, logLimit),
                GetLogsStep(
                    "GetApiLogs", client, callStyle, useQueryParameters: false, updateState: true, logLimit),
                GetLogsStep(
                    "GetApiLogs_WithQueryParameters",
                    client,
                    callStyle,
                    useQueryParameters: true,
                    updateState: false,
                    logLimit),
                GetLogStep("GetApiLog", client, callStyle)
            }
        };
    }

    private static Step GetLogsStep(
        string name,
        IApiLogClient client,
        CallStyle callStyle,
        bool useQueryParameters,
        bool updateState,
        long? logLimit) => new()
        {
            Name = name,
            Method = MethodName("GetLogs", callStyle),
            CallStyle = callStyle,
            Parameters = (Func<TestContext, object?>)(context =>
                QueryParameters(context, useQueryParameters, logLimit)),
            ExpectedStatus = IsHttpInfo(callStyle) ? 200 : null,
            // A bounded query is only deterministic when the page is filled, so every list
            // step retries while it is empty. Only the state-seeding step needs data: a
            // cursor-filtered page can legitimately be terminal and therefore empty.
            ExecuteAsync = GetLogsExecutor(
                client, callStyle, useQueryParameters, updateState || logLimit.HasValue, logLimit),
            ValidateAsync = ValidateLogs(requireLogs: updateState),
            UpdateStateAsync = updateState ? UpdateLogVariablesAsync : null
        };

    private static Step GetLogStep(string name, IApiLogClient client, CallStyle callStyle) => new()
    {
        Name = name,
        Method = MethodName("GetLog", callStyle),
        CallStyle = callStyle,
        Parameters = (Func<TestContext, object?>)(context =>
            new Dictionary<string, object?>
            {
                ["api_log_id"] = context.GetRequired<string>("api_log_id")
            }),
        ExpectedStatus = IsHttpInfo(callStyle) ? 200 : null,
        ExecuteAsync = GetLogExecutor(client, callStyle),
        ValidateAsync = ValidateLogAsync
    };

    private static Func<TestContext, CancellationToken, Task<ExecutionResult>> GetLogsExecutor(
        IApiLogClient client,
        CallStyle callStyle,
        bool useQueryParameters,
        bool retryEmpty,
        long? logLimit) => callStyle switch
        {
            CallStyle.Sync => MethodExecutor.Sync(context =>
            {
                var query = Query(context, useQueryParameters, logLimit);
                return retryEmpty
                    ? FetchWithRetry(
                        () => client.GetLogs(query.CreatedDate, query.CreatedAt, query.Limit, query.Cursor),
                        IsEmpty)
                    : client.GetLogs(query.CreatedDate, query.CreatedAt, query.Limit, query.Cursor);
            }),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(context =>
            {
                var query = Query(context, useQueryParameters, logLimit);
                var response = retryEmpty
                    ? FetchWithRetry(
                        () => client.GetLogsWithHttpInfo(
                            query.CreatedDate, query.CreatedAt, query.Limit, query.Cursor),
                        IsEmpty)
                    : client.GetLogsWithHttpInfo(
                        query.CreatedDate, query.CreatedAt, query.Limit, query.Cursor);
                return ToExecutionResult(response);
            }),
            CallStyle.Async => MethodExecutor.Async(async (context, cancellationToken) =>
            {
                var query = Query(context, useQueryParameters, logLimit);
                return retryEmpty
                    ? await FetchWithRetryAsync(
                        () => client.GetLogsAsync(
                            query.CreatedDate,
                            query.CreatedAt,
                            query.Limit,
                            query.Cursor,
                            cancellationToken),
                        IsEmpty,
                        cancellationToken).ConfigureAwait(false)
                    : await client.GetLogsAsync(
                        query.CreatedDate,
                        query.CreatedAt,
                        query.Limit,
                        query.Cursor,
                        cancellationToken).ConfigureAwait(false);
            }),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(async (context, cancellationToken) =>
            {
                var query = Query(context, useQueryParameters, logLimit);
                var response = retryEmpty
                    ? await FetchWithRetryAsync(
                        () => client.GetLogsWithHttpInfoAsync(
                            query.CreatedDate,
                            query.CreatedAt,
                            query.Limit,
                            query.Cursor,
                            cancellationToken),
                        IsEmpty,
                        cancellationToken).ConfigureAwait(false)
                    : await client.GetLogsWithHttpInfoAsync(
                        query.CreatedDate,
                        query.CreatedAt,
                        query.Limit,
                        query.Cursor,
                        cancellationToken).ConfigureAwait(false);
                return ToExecutionResult(response);
            }),
            _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
        };

    private static Func<TestContext, CancellationToken, Task<ExecutionResult>> GetLogExecutor(
        IApiLogClient client,
        CallStyle callStyle) => callStyle switch
        {
            CallStyle.Sync => MethodExecutor.Sync(context =>
                client.GetLog(context.GetRequired<string>("api_log_id"))),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(context =>
                ToExecutionResult(client.GetLogWithHttpInfo(
                    context.GetRequired<string>("api_log_id")))),
            CallStyle.Async => MethodExecutor.Async(async (context, cancellationToken) =>
                await client.GetLogAsync(
                    context.GetRequired<string>("api_log_id"),
                    cancellationToken).ConfigureAwait(false)),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(async (context, cancellationToken) =>
                ToExecutionResult(await client.GetLogWithHttpInfoAsync(
                    context.GetRequired<string>("api_log_id"),
                    cancellationToken).ConfigureAwait(false))),
            _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
        };

    private static object QueryParameters(TestContext context, bool useQueryParameters, long? logLimit)
    {
        if (!useQueryParameters)
        {
            return logLimit is null
                ? new Dictionary<string, object?>()
                : new Dictionary<string, object?> { ["limit"] = logLimit };
        }

        var query = Query(context, useQueryParameters: true, logLimit);
        return new Dictionary<string, object?>
        {
            ["created_date"] = query.CreatedDate?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            ["created_at"] = query.CreatedAt?.ToString("O", CultureInfo.InvariantCulture),
            ["limit"] = query.Limit,
            ["cursor"] = query.Cursor
        };
    }

    private static LogQuery Query(TestContext context, bool useQueryParameters, long? logLimit)
    {
        if (!useQueryParameters)
            return new LogQuery(null, null, logLimit, null);

        var createdDateText = context.GetRequired<string>("created_date");
        if (!DateTime.TryParseExact(
                createdDateText,
                "yyyy-MM-dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out var createdDate))
        {
            throw new InvalidOperationException(
                $"API log created_date is not a valid YYYY-MM-DD value: {createdDateText}");
        }

        var createdAtText = context.GetRequired<string>("created_at");
        if (!DateTimeOffset.TryParse(
                createdAtText,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out var createdAt))
        {
            throw new InvalidOperationException(
                $"API log created_at is not a valid ISO 8601 value: {createdAtText}");
        }

        var cursor = context.GetRequired<string>("cursor");
        var limit = ParseLimit(context);
        return new LogQuery(
            createdDate,
            createdAt.UtcDateTime,
            limit,
            string.IsNullOrWhiteSpace(cursor) ? null : cursor);
    }

    private static long? ParseLimit(TestContext context)
    {
        if (!context.Variables.TryGetValue("limit", out var value) || value is null)
            return null;

        if (value is long longValue && longValue > 0)
            return longValue;
        if (value is int intValue && intValue > 0)
            return intValue;
        if (value is string text &&
            long.TryParse(text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) &&
            parsed > 0)
            return parsed;

        throw new InvalidOperationException(
            $"API log limit must be a positive integer: {value}");
    }

    /// <summary>
    /// Validates a log page. Steps that retry until the page is filled must not accept an
    /// empty result, otherwise a query that never returned data is recorded as a success.
    /// </summary>
    private static Func<ExecutionResult, TestContext, Task> ValidateLogs(bool requireLogs) => (result, _) =>
    {
        if (result.Response is not ApiLogs logs || logs.VarApiLogs is null)
            throw new InvalidOperationException("Expected ApiLog API to return ApiLogs with an api_logs array.");
        if (requireLogs && logs.VarApiLogs.Count == 0)
            throw new InvalidOperationException(
                $"ApiLog API returned no logs after {EmptyResponseRetries} retries.");

        return Task.CompletedTask;
    };

    private static Task ValidateLogAsync(ExecutionResult result, TestContext _)
    {
        if (result.Response is not ApiLogModel log || string.IsNullOrWhiteSpace(log.ApiLogId))
            throw new InvalidOperationException("Expected ApiLog API to return a log with a valid api_log_id.");

        return Task.CompletedTask;
    }

    private static Task UpdateLogVariablesAsync(ExecutionResult result, TestContext context)
    {
        if (result.Response is not ApiLogs logs || logs.VarApiLogs is null || logs.VarApiLogs.Count == 0)
        {
            throw new InvalidOperationException(
                "ApiLog API returned no logs from which to build the GetApiLog request.");
        }

        var log = logs.VarApiLogs[0];
        if (string.IsNullOrWhiteSpace(log.ApiLogId) || string.IsNullOrWhiteSpace(log.CreatedDate))
            throw new InvalidOperationException("The first API log is missing api_log_id or created_date.");

        context.Variables["api_log_id"] = log.ApiLogId;
        context.Variables["created_date"] = log.CreatedDate;
        context.Variables["timestamp"] = log.CreatedAt;
        context.Variables["created_at"] = DateTimeOffset
            .FromUnixTimeSeconds(log.CreatedAt)
            .ToUniversalTime()
            .ToString("yyyy-MM-dd'T'HH:mm:ss'Z'", CultureInfo.InvariantCulture);
        context.Variables["cursor"] = logs.Cursor ?? string.Empty;
        return Task.CompletedTask;
    }

    private static T FetchWithRetry<T>(
        Func<T> fetch,
        Func<T, bool> isEmpty)
    {
        var result = fetch();
        for (var attempt = 0; attempt < EmptyResponseRetries && isEmpty(result); attempt++)
        {
            // The live API can lag briefly before a newly-created log becomes visible.
            // The synchronous executor runs on a worker thread, so waiting here does not
            // block the test's caller thread.
            Thread.Sleep(EmptyResponseDelay);
            result = fetch();
        }

        return result;
    }

    private static async Task<T> FetchWithRetryAsync<T>(
        Func<Task<T>> fetch,
        Func<T, bool> isEmpty,
        CancellationToken cancellationToken)
    {
        var result = await fetch().ConfigureAwait(false);
        for (var attempt = 0; attempt < EmptyResponseRetries && isEmpty(result); attempt++)
        {
            await Task.Delay(EmptyResponseDelay, cancellationToken).ConfigureAwait(false);
            result = await fetch().ConfigureAwait(false);
        }

        return result;
    }

    private static bool IsEmpty(ApiLogs logs) =>
        logs is null || logs.VarApiLogs is null || logs.VarApiLogs.Count == 0;

    private static bool IsEmpty(ApiResponse<ApiLogs> response) =>
        response is null || IsEmpty(response.Data);

    private static ExecutionResult ToExecutionResult<T>(ApiResponse<T> response) =>
        new(
            response.Data,
            (int)response.StatusCode,
            ToHeaders(response.Headers),
            Body: response.RawContent);

    private static IReadOnlyDictionary<string, IReadOnlyList<string>>? ToHeaders(
        Multimap<string, string>? headers) =>
        headers is null || headers.Count == 0
            ? null
            : headers.ToDictionary(
                pair => pair.Key,
                pair => (IReadOnlyList<string>)pair.Value.ToArray(),
                StringComparer.OrdinalIgnoreCase);

    private static string MethodName(string operation, CallStyle callStyle) => callStyle switch
    {
        CallStyle.Sync => operation,
        CallStyle.WithHttpInfo => $"{operation}WithHttpInfo",
        CallStyle.Async => $"{operation}Async",
        CallStyle.WithHttpInfoAsync => $"{operation}WithHttpInfoAsync",
        _ => throw new ArgumentOutOfRangeException(nameof(callStyle), callStyle, null)
    };

    private static bool IsHttpInfo(CallStyle callStyle) =>
        callStyle is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync;

    private sealed record LogQuery(
        DateTime? CreatedDate,
        DateTime? CreatedAt,
        long? Limit,
        string? Cursor);
}
