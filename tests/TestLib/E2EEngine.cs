using System.Diagnostics;
using System.Net.Http;

namespace SaasusSdk.Tests.TestLib;

public sealed class E2EEngine
{
    private readonly Config _config;
    private readonly Logger _logger;

    public E2EEngine(Config config, CoverageTracker? coverage = null, Logger? logger = null)
    {
        _config = config;
        Coverage = coverage ?? new CoverageTracker();
        _logger = logger ?? new Logger(config.LogLevel);
    }

    public CoverageTracker Coverage { get; }

    /// <summary>The logger used for the execution trace; exposed so callers can reuse the level and masking.</summary>
    public Logger Logger => _logger;

    public async Task<IReadOnlyList<StoryResult>> ExecuteAsync(
        IEnumerable<Story> stories,
        CancellationToken cancellationToken = default)
    {
        var results = new List<StoryResult>();
        foreach (var story in stories)
        {
            // A pre-cancelled token must stop the run even when a story has no executable steps.
            cancellationToken.ThrowIfCancellationRequested();
            var result = await ExecuteStoryAsync(story, cancellationToken).ConfigureAwait(false);
            results.Add(result);
            if (_config.FailFast && result.Status == TestStatus.Failed) break;
        }

        if (_logger.IsEnabled(LogLevel.Debug)) _logger.LogCoverage(Coverage);
        return results;
    }

    public async Task<StoryResult> ExecuteStoryAsync(
        Story story,
        CancellationToken cancellationToken = default)
    {
        // Honoured here too: ExecuteStoryAsync is public and a story may have no executable steps.
        cancellationToken.ThrowIfCancellationRequested();
        var stopwatch = Stopwatch.StartNew();
        var steps = new List<StepResult>();
        var context = new TestContext(new Dictionary<string, object?>(story.InitialVariables));
        Exception? setupError = null;
        Exception? cleanupError = null;
        _logger.LogStoryStart(story);
        using var storyTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        if (story.Timeout.HasValue)
            storyTimeout.CancelAfter(story.Timeout.Value);

        try
        {
            if (!_config.DryRun && story.SetupAsync is not null)
            {
                using var setupTimeout = CancellationTokenSource.CreateLinkedTokenSource(storyTimeout.Token);
                setupTimeout.CancelAfter(_config.Timeout);
                Task? setupTask = null;
                try
                {
                    setupTask = story.SetupAsync(context, setupTimeout.Token);
                    await setupTask.WaitAsync(_config.Timeout, storyTimeout.Token).ConfigureAwait(false);
                }
                catch (OperationCanceledException error) when (
                    setupTimeout.IsCancellationRequested && !storyTimeout.IsCancellationRequested)
                {
                    // A delegate that ignores its token keeps running; cancel it, give it a bounded
                    // grace period to unwind, then observe it. Proceeding immediately would let
                    // cleanup run while setup still mutates the context or live resources.
                    await AbandonAsync(setupTimeout, setupTask).ConfigureAwait(false);
                    throw new TimeoutException(
                        $"Setup for '{story.Name}' exceeded the {_config.Timeout} timeout.", error);
                }
                catch
                {
                    // Caller cancellation and the story timeout cancel both linked tokens, so the
                    // filtered handler above does not run: the task still has to be settled.
                    await AbandonAsync(setupTimeout, setupTask).ConfigureAwait(false);
                    throw;
                }
            }

            foreach (var step in story.Steps)
            {
                Coverage.Register(step.Method, step.CallStyle);
                _logger.LogStepStart(story, step, steps.Count);
                var result = await ExecuteStepAsync(step, context, _config.Timeout, storyTimeout.Token)
                    .ConfigureAwait(false);
                steps.Add(result);
                _logger.LogStepResult(result);
                if (result.Status == TestStatus.Failed) break;
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception error)
        {
            setupError = error;
            _logger.Write(LogLevel.Error, $"Story '{story.Name}' failed before completing its steps: {error}");
        }
        finally
        {
            if (!_config.DryRun && story.CleanupAsync is not null)
            {
                using var cleanupTimeout = new CancellationTokenSource(_config.Timeout);
                Task? cleanupTask = null;
                try
                {
                    cleanupTask = story.CleanupAsync(context, cleanupTimeout.Token);
                    await cleanupTask.WaitAsync(_config.Timeout).ConfigureAwait(false);
                }
                catch (Exception error)
                {
                    // The delegate may ignore its token: cancel it, wait a bounded grace period and
                    // observe it, so a late completion cannot mutate shared resources unnoticed.
                    await AbandonAsync(cleanupTimeout, cleanupTask).ConfigureAwait(false);
                    cleanupError = error;
                    _logger.Write(LogLevel.Error, $"Cleanup for '{story.Name}' failed: {error}");
                }
            }
        }

        stopwatch.Stop();
        var failed = setupError is not null || cleanupError is not null ||
                     steps.Any(step => step.Status == TestStatus.Failed);
        var skipped = steps.Count > 0 && steps.All(step => step.Status == TestStatus.Skipped);
        var storyResult = new StoryResult(
            story.Name,
            failed ? TestStatus.Failed : skipped ? TestStatus.Skipped : TestStatus.Passed,
            stopwatch.Elapsed,
            steps,
            setupError,
            cleanupError,
            new Dictionary<string, object?>(context.Variables));
        _logger.LogStoryEnd(storyResult);
        return storyResult;
    }

    private async Task<StepResult> ExecuteStepAsync(
        Step step,
        TestContext context,
        TimeSpan timeout,
        CancellationToken cancellationToken)
    {
        if (step.Skip)
        {
            _logger.LogSkip(step);
            return new StepResult(step.Name, step.Method, step.CallStyle, TestStatus.Skipped,
                TimeSpan.Zero, SkipReason: step.SkipReason, Attempts: 0);
        }

        var stopwatch = Stopwatch.StartNew();
        var timestamp = DateTimeOffset.UtcNow;
        var variablesBefore = new Dictionary<string, object?>(context.Variables);
        ExecutionResult execution = ExecutionResult.DryRun();
        object? parameters = new Dictionary<string, object?>();
        var parameterResolutionFailed = false;
        var attempts = 0;
        try
        {
            parameters = step.Parameters is Func<TestContext, object?> factory
                ? factory(context)
                : step.Parameters ?? parameters;
        }
        catch (Exception error)
        {
            parameterResolutionFailed = true;
            execution = ExecutionResult.Failure(error);
        }

        for (; !parameterResolutionFailed && attempts <= _config.MaxRetries; attempts++)
        {
            if (_config.DryRun) break;
            using var stepTimeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            stepTimeout.CancelAfter(timeout);
            Task<ExecutionResult>? executionTask = null;
            try
            {
                executionTask = step.ExecuteAsync(context, stepTimeout.Token);
                execution = await executionTask
                    .WaitAsync(timeout, cancellationToken)
                    .ConfigureAwait(false);
                if (execution.Error is OperationCanceledException && cancellationToken.IsCancellationRequested)
                    cancellationToken.ThrowIfCancellationRequested();
                if (execution.Error is OperationCanceledException &&
                    stepTimeout.IsCancellationRequested &&
                    !cancellationToken.IsCancellationRequested)
                {
                    execution = ExecutionResult.Failure(new TimeoutException(
                        $"Step '{step.Name}' exceeded the {timeout} timeout.", execution.Error));
                }
            }
            catch (TimeoutException error)
            {
                await AwaitSynchronousCompletionAsync(step, executionTask).ConfigureAwait(false);
                execution = ExecutionResult.Failure(error);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                await AwaitSynchronousCompletionAsync(step, executionTask).ConfigureAwait(false);
                throw;
            }
            catch (OperationCanceledException error) when (!cancellationToken.IsCancellationRequested)
            {
                await AwaitSynchronousCompletionAsync(step, executionTask).ConfigureAwait(false);
                execution = ExecutionResult.Failure(new TimeoutException(
                    $"Step '{step.Name}' exceeded the {timeout} timeout.", error));
            }
            catch (Exception error)
            {
                execution = ExecutionResult.Failure(error);
            }

            if (!ShouldRetry(step, execution) || attempts == _config.MaxRetries) break;
            _logger.LogRetry(step.Method, attempts + 1, _config.MaxRetries + 1,
                execution.Error?.Message ?? $"HTTP {execution.StatusCode}");
            await Task.Delay(RetryDelay(attempts), cancellationToken).ConfigureAwait(false);
        }

        var expectedErrorResponse = execution.Error is not null && execution.StatusCode is { } errorStatus &&
                                    AcceptsStatus(step, errorStatus);
        Exception? validationError = expectedErrorResponse ? null : execution.Error;
        if (!_config.DryRun && validationError is null)
        {
            try
            {
                ValidateStatus(step, execution);
                if (step.ValidateAsync is not null)
                    await step.ValidateAsync(execution, context).ConfigureAwait(false);
                _logger.LogValidation(step.Name, true);
                if (step.UpdateStateAsync is not null)
                    await step.UpdateStateAsync(execution, context).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                validationError = error;
                _logger.LogValidation(step.Name, false, error.Message);
            }
        }

        stopwatch.Stop();
        var success = validationError is null;
        var changedAt = DateTimeOffset.UtcNow;
        // Go and PHP record every transition as old_value / new_value / timestamp, including
        // removals, so a snapshot shows what the step changed rather than only the end state.
        var stateChanges = new Dictionary<string, StateChange>(StringComparer.Ordinal);
        foreach (var (key, value) in context.Variables)
        {
            var existed = variablesBefore.TryGetValue(key, out var previous);
            if (existed && Equals(previous, value)) continue;
            stateChanges[key] = new StateChange(existed ? previous : null, value, changedAt);
        }
        foreach (var removed in variablesBefore.Keys.Where(key => !context.Variables.ContainsKey(key)))
            stateChanges[removed] = new StateChange(variablesBefore[removed], null, changedAt);
        _logger.LogStateUpdate(step.Name, stateChanges);
        Coverage.Record(step.Method, step.CallStyle, success, stopwatch.Elapsed);
        return new StepResult(
            step.Name,
            step.Method,
            step.CallStyle,
            success ? TestStatus.Passed : TestStatus.Failed,
            stopwatch.Elapsed,
            execution.StatusCode,
            execution.Response,
            execution.Headers,
            validationError,
            Attempts: attempts + 1,
            Timestamp: timestamp,
            Body: execution.Body,
            StateChanges: stateChanges,
            Parameters: parameters);
    }

    private static void ValidateStatus(Step step, ExecutionResult execution)
    {
        if (!execution.HasStatusCode)
        {
            // A plain SDK call surfaces no status. If the step asserts one, accepting the call
            // silently would turn a negative-path assertion into a test that cannot fail.
            if (step.ExpectedStatus.HasValue || step.AllowedStatuses.Count > 0)
                throw new InvalidOperationException(
                    $"Step '{step.Name}' declares a status constraint but the call reported no HTTP status.");
            return;
        }
        var status = execution.StatusCode!.Value;
        if (!AcceptsStatus(step, status))
        {
            throw new InvalidOperationException(
                $"Step '{step.Name}' returned HTTP {status}, which is not an accepted status.");
        }
    }

    private static bool AcceptsStatus(Step step, int status) => step.ExpectedStatus.HasValue
        ? status == step.ExpectedStatus.Value
        : step.AllowedStatuses.Count > 0
            ? step.AllowedStatuses.Contains(status)
            : status is >= 200 and < 300;

    /// <summary>
    /// A synchronous SDK call runs on a pool thread that cannot be cancelled, so the engine gives
    /// it a short grace period to unwind and then abandons it. Awaiting it without a deadline would
    /// turn the per-step timeout into an unbounded hang and stop cleanup from running.
    /// </summary>
    private static readonly TimeSpan AbandonGrace = TimeSpan.FromSeconds(5);

    private static async Task AwaitSynchronousCompletionAsync(
        Step step,
        Task<ExecutionResult>? executionTask)
    {
        if (executionTask is null) return;
        try
        {
            // Every call style gets the bounded grace period: an asynchronous step that ignores
            // cancellation could otherwise still be mutating shared resources while cleanup and
            // snapshot capture run.
            await executionTask.WaitAsync(AbandonGrace).ConfigureAwait(false);
        }
        catch
        {
            // The original timeout or cancellation remains the reported failure.
            Observe(executionTask);
        }
    }

    /// <summary>
    /// Consumes the exception of an abandoned task. An unobserved faulted task otherwise surfaces
    /// on the finalizer thread, unrelated to the test that started it.
    /// </summary>
    private static void Observe(Task? task) => task?.ContinueWith(
        static completed => _ = completed.Exception,
        CancellationToken.None,
        TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
        TaskScheduler.Default);

    /// <summary>
    /// Cancels a delegate that overran its deadline, waits a bounded grace period for it to unwind
    /// so the next phase does not race it, and then observes whatever is left.
    /// </summary>
    private static async Task AbandonAsync(CancellationTokenSource source, Task? task)
    {
        source.Cancel();
        if (task is null) return;
        try { await task.WaitAsync(AbandonGrace).ConfigureAwait(false); }
        catch { Observe(task); }
    }

    private static bool ShouldRetry(Step step, ExecutionResult result)
    {
        if (result.StatusCode is { } status && AcceptsStatus(step, status))
            return false;
        // A timeout returns while the underlying request may still be in flight, for every call
        // style. Retrying would issue a duplicate side-effecting request and race with cleanup.
        if (result.Error is TimeoutException) return false;
        return result.StatusCode == 429 || result.StatusCode is >= 500 and <= 599 ||
               result.Error is HttpRequestException;
    }

    private static TimeSpan RetryDelay(int retry) =>
        TimeSpan.FromMilliseconds(Math.Min(100 * Math.Pow(2, retry), 2_000));
}
