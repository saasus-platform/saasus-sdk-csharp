using System.Net.Http;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

public sealed class E2EEngineTests
{
    private static Config TestConfig(bool failFast = false, int retries = 0) => new()
    {
        SaasId = "saas",
        ApiKey = "api",
        SecretKey = "secret",
        Timeout = TimeSpan.FromSeconds(2),
        MaxRetries = retries,
        FailFast = failFast,
        LogLevel = LogLevel.None
    };

    [Fact]
    public async Task SharesStateAndAlwaysRunsCleanup()
    {
        var cleanedUp = false;
        var story = new Story
        {
            Name = "state",
            Steps = new[]
            {
                new Step
                {
                    Name = "create",
                    Method = "Create",
                    ExecuteAsync = MethodExecutor.Async((_, _) => Task.FromResult<object?>("id-1")),
                    UpdateStateAsync = (result, context) =>
                    {
                        context.Variables["id"] = result.Response;
                        return Task.CompletedTask;
                    }
                },
                new Step
                {
                    Name = "read",
                    Method = "Read",
                    ExecuteAsync = MethodExecutor.Async((context, _) =>
                        Task.FromResult<object?>(context.GetRequired<string>("id"))),
                    ValidateAsync = (result, _) =>
                    {
                        Assert.Equal("id-1", result.Response);
                        return Task.CompletedTask;
                    }
                }
            },
            CleanupAsync = (_, _) =>
            {
                cleanedUp = true;
                return Task.CompletedTask;
            }
        };

        var result = await new E2EEngine(TestConfig()).ExecuteStoryAsync(story);

        Assert.Equal(TestStatus.Passed, result.Status);
        Assert.True(cleanedUp);
        Assert.All(result.Steps, step => Assert.Equal(TestStatus.Passed, step.Status));
    }

    [Fact]
    public async Task RetriesTransientFailures()
    {
        var calls = 0;
        var step = new Step
        {
            Name = "retry",
            Method = "Get",
            ExecuteAsync = async (_, _) =>
            {
                await Task.Yield();
                calls++;
                return calls < 3
                    ? ExecutionResult.Failure(new HttpRequestException("temporary"))
                    : ExecutionResult.WithHttp("ok", 200);
            }
        };

        var result = await new E2EEngine(TestConfig(retries: 2)).ExecuteStoryAsync(
            new Story { Name = "retry", Steps = new[] { step } });

        Assert.Equal(TestStatus.Passed, result.Status);
        Assert.Equal(3, calls);
        Assert.Equal(3, result.Steps[0].Attempts);
    }

    [Fact]
    public async Task FailFastStopsAfterTheFirstFailedStory()
    {
        var secondStoryRan = false;
        var engine = new E2EEngine(TestConfig(failFast: true));

        // FailFast is a multi-story policy, so it is only consulted by ExecuteAsync.
        var results = await engine.ExecuteAsync(new[]
        {
            new Story
            {
                Name = "failing",
                Steps = new[]
                {
                    new Step
                    {
                        Name = "fail", Method = "Fail",
                        ExecuteAsync = MethodExecutor.WithHttpInfo(_ => ExecutionResult.WithHttp(null, 500))
                    }
                }
            },
            new Story
            {
                Name = "never runs",
                Steps = new[]
                {
                    new Step
                    {
                        Name = "second", Method = "Second",
                        ExecuteAsync = MethodExecutor.Sync(_ => secondStoryRan = true)
                    }
                }
            }
        });

        Assert.Single(results);
        Assert.Equal(TestStatus.Failed, results[0].Status);
        Assert.False(secondStoryRan);
    }

    [Fact]
    public async Task FailFastStopsAfterFirstFailedStepButRunsCleanup()
    {
        var secondRan = false;
        var cleanupRan = false;
        var result = await new E2EEngine(TestConfig(failFast: true)).ExecuteStoryAsync(new Story
        {
            Name = "fail-fast",
            Steps = new[]
            {
                new Step
                {
                    Name = "fail",
                    Method = "Fail",
                    ExecuteAsync = MethodExecutor.WithHttpInfo(_ =>
                        ExecutionResult.WithHttp(null, 500))
                },
                new Step
                {
                    Name = "second",
                    Method = "Second",
                    ExecuteAsync = MethodExecutor.Sync(_ => secondRan = true)
                }
            },
            CleanupAsync = (_, _) =>
            {
                cleanupRan = true;
                return Task.CompletedTask;
            }
        });

        Assert.Equal(TestStatus.Failed, result.Status);
        Assert.False(secondRan);
        Assert.True(cleanupRan);
        Assert.Single(result.Steps);
    }

    [Fact]
    public async Task StopsStoryAfterFailedStepByDefaultAndRunsCleanup()
    {
        var secondRan = false;
        var cleanupRan = false;
        var result = await new E2EEngine(TestConfig()).ExecuteStoryAsync(new Story
        {
            Name = "stop-on-failure",
            Steps = new[]
            {
                new Step
                {
                    Name = "fail",
                    Method = "Fail",
                    ExecuteAsync = MethodExecutor.WithHttpInfo(_ =>
                        ExecutionResult.WithHttp(null, 500))
                },
                new Step
                {
                    Name = "must not run",
                    Method = "MustNotRun",
                    ExecuteAsync = MethodExecutor.Sync(_ => secondRan = true)
                }
            },
            CleanupAsync = (_, _) =>
            {
                cleanupRan = true;
                return Task.CompletedTask;
            }
        });

        Assert.Equal(TestStatus.Failed, result.Status);
        Assert.Single(result.Steps);
        Assert.False(secondRan);
        Assert.True(cleanupRan);
    }

    [Fact]
    public async Task SupportsEveryCallStyleThroughCommonResult()
    {
        var steps = new[]
        {
            new Step { Name = "sync", Method = "Sync", CallStyle = CallStyle.Sync,
                ExecuteAsync = MethodExecutor.Sync(_ => "sync") },
            new Step { Name = "http", Method = "Http", CallStyle = CallStyle.WithHttpInfo,
                ExecuteAsync = MethodExecutor.WithHttpInfo(_ => ExecutionResult.WithHttp("http", 200)) },
            new Step { Name = "async", Method = "Async", CallStyle = CallStyle.Async,
                ExecuteAsync = MethodExecutor.Async((_, _) => Task.FromResult<object?>("async")) },
            new Step { Name = "http async", Method = "HttpAsync", CallStyle = CallStyle.WithHttpInfoAsync,
                ExecuteAsync = MethodExecutor.WithHttpInfoAsync((_, _) =>
                    Task.FromResult(ExecutionResult.WithHttp("http async", 204))) }
        };

        var result = await new E2EEngine(TestConfig()).ExecuteStoryAsync(
            new Story { Name = "styles", Steps = steps });

        Assert.Equal(TestStatus.Passed, result.Status);
        Assert.Equal(4, result.Steps.Count);
        Assert.All(result.Steps, step => Assert.Equal(TestStatus.Passed, step.Status));
    }

    [Fact]
    public async Task DryRunDoesNotCallSetupStepsOrCleanup()
    {
        var calls = 0;
        var config = new Config
        {
            DryRun = true,
            LogLevel = LogLevel.None
        };
        var result = await new E2EEngine(config).ExecuteStoryAsync(new Story
        {
            Name = "dry-run",
            SetupAsync = (_, _) => { calls++; return Task.CompletedTask; },
            Steps = new[]
            {
                new Step
                {
                    Name = "call",
                    Method = "Call",
                    ExecuteAsync = MethodExecutor.Sync(_ => calls++)
                }
            },
            CleanupAsync = (_, _) => { calls++; return Task.CompletedTask; }
        });

        Assert.Equal(TestStatus.Passed, result.Status);
        Assert.Equal(0, calls);
    }

    [Fact]
    public async Task DryRunBypassesResponseValidationAndStateUpdates()
    {
        var validations = 0;
        var updates = 0;
        var result = await new E2EEngine(new Config
        {
            DryRun = true,
            LogLevel = LogLevel.None
        }).ExecuteStoryAsync(new Story
        {
            Name = "validated dry-run",
            Steps = new[]
            {
                new Step
                {
                    Name = "get",
                    Method = "Get",
                    ExecuteAsync = MethodExecutor.Sync(_ => throw new InvalidOperationException("must not execute")),
                    ValidateAsync = (_, _) =>
                    {
                        validations++;
                        throw new InvalidOperationException("must not validate synthetic response");
                    },
                    UpdateStateAsync = (_, _) =>
                    {
                        updates++;
                        return Task.CompletedTask;
                    }
                }
            }
        });

        Assert.Equal(TestStatus.Passed, result.Status);
        Assert.Equal(0, validations);
        Assert.Equal(0, updates);
    }

    [Fact]
    public async Task AppliesConfiguredTimeoutToSetup()
    {
        var config = new Config
        {
            Timeout = TimeSpan.FromMilliseconds(20),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        };

        var result = await new E2EEngine(config).ExecuteStoryAsync(new Story
        {
            Name = "setup timeout",
            SetupAsync = async (_, token) =>
                await Task.Delay(Timeout.InfiniteTimeSpan, token)
        }).WaitAsync(TimeSpan.FromSeconds(1));

        Assert.Equal(TestStatus.Failed, result.Status);
        Assert.IsType<TimeoutException>(result.SetupError);
    }

    [Fact]
    public async Task CallerCancellationStopsRemainingStories()
    {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(20));
        var secondStoryRan = false;
        var stories = new[]
        {
            new Story
            {
                Name = "cancelled",
                Steps = new[]
                {
                    new Step
                    {
                        Name = "wait",
                        Method = "Wait",
                        ExecuteAsync = MethodExecutor.Async(async (_, token) =>
                            await Task.Delay(Timeout.InfiniteTimeSpan, token))
                    }
                }
            },
            new Story
            {
                Name = "must not run",
                Steps = new[]
                {
                    new Step
                    {
                        Name = "call",
                        Method = "Call",
                        ExecuteAsync = MethodExecutor.Sync(_ => secondStoryRan = true)
                    }
                }
            }
        };

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            new E2EEngine(TestConfig()).ExecuteAsync(stories, cancellation.Token));

        Assert.False(secondStoryRan);
    }

    [Fact]
    public async Task BoundsCleanupAfterCallerCancellation()
    {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMilliseconds(20));
        var cleanupStarted = false;
        var cleanupWasInitiallyCanceled = true;
        var config = new Config
        {
            Timeout = TimeSpan.FromMilliseconds(100),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        };
        var story = new Story
        {
            Name = "cancel with cleanup",
            Steps = new[]
            {
                new Step
                {
                    Name = "wait",
                    Method = "Wait",
                    ExecuteAsync = MethodExecutor.Async(async (_, token) =>
                        await Task.Delay(Timeout.InfiniteTimeSpan, token))
                }
            },
            CleanupAsync = async (_, token) =>
            {
                cleanupStarted = true;
                cleanupWasInitiallyCanceled = token.IsCancellationRequested;
                await Task.Delay(Timeout.InfiniteTimeSpan, token);
            }
        };

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            new E2EEngine(config).ExecuteStoryAsync(story, cancellation.Token).WaitAsync(TimeSpan.FromSeconds(1)));

        Assert.True(cleanupStarted);
        Assert.False(cleanupWasInitiallyCanceled);
    }

    [Fact]
    public async Task ConvertsStepCancellationToTimeout()
    {
        var config = new Config
        {
            Timeout = TimeSpan.FromMilliseconds(20),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        };
        var result = await new E2EEngine(config).ExecuteStoryAsync(new Story
        {
            Name = "timeout",
            Steps = new[]
            {
                new Step
                {
                    Name = "slow",
                    Method = "SlowAsync",
                    ExecuteAsync = MethodExecutor.Async(async (_, token) =>
                        await Task.Delay(Timeout.InfiniteTimeSpan, token))
                }
            }
        });

        Assert.Equal(TestStatus.Failed, result.Status);
        Assert.IsType<TimeoutException>(result.Steps[0].Error);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task AppliesTimeoutToSynchronousCalls(bool withHttpInfo)
    {
        using var gate = new ManualResetEventSlim();
        var calls = 0;
        var callCompleted = false;
        var cleanupSawCallCompleted = false;
        var config = new Config
        {
            Timeout = TimeSpan.FromMilliseconds(20),
            MaxRetries = 2,
            LogLevel = LogLevel.None
        };
        var execute = withHttpInfo
            ? MethodExecutor.WithHttpInfo(_ =>
            {
                Interlocked.Increment(ref calls);
                gate.Wait();
                callCompleted = true;
                return ExecutionResult.WithHttp("ok", 200);
            })
            : MethodExecutor.Sync(_ =>
            {
                Interlocked.Increment(ref calls);
                gate.Wait();
                callCompleted = true;
                return (object?)"ok";
            });

        try
        {
            var resultTask = new E2EEngine(config).ExecuteStoryAsync(new Story
            {
                Name = "sync timeout",
                Steps = new[]
                {
                    new Step
                    {
                        Name = "slow",
                        Method = "Slow",
                        CallStyle = withHttpInfo ? CallStyle.WithHttpInfo : CallStyle.Sync,
                        ExecuteAsync = execute
                    }
                },
                CleanupAsync = (_, _) =>
                {
                    cleanupSawCallCompleted = callCompleted;
                    return Task.CompletedTask;
                }
            });

            Assert.True(SpinWait.SpinUntil(() => Volatile.Read(ref calls) == 1, 1_000));
            await Task.Delay(50);
            Assert.False(resultTask.IsCompleted);
            gate.Set();
            var result = await resultTask.WaitAsync(TimeSpan.FromSeconds(1));

            Assert.Equal(TestStatus.Failed, result.Status);
            Assert.IsType<TimeoutException>(result.Steps[0].Error);
            Assert.Equal(1, Volatile.Read(ref calls));
            Assert.True(cleanupSawCallCompleted);
        }
        finally
        {
            gate.Set();
        }
    }

    [Fact]
    public async Task AcceptsExplicitAllowedStatus()
    {
        var result = await new E2EEngine(TestConfig()).ExecuteStoryAsync(new Story
        {
            Name = "not-found",
            Steps = new[]
            {
                new Step
                {
                    Name = "get missing",
                    Method = "Get",
                    AllowedStatuses = new[] { 404 },
                    ExecuteAsync = MethodExecutor.WithHttpInfo(_ =>
                        ExecutionResult.WithHttp(null, 404))
                }
            }
        });

        Assert.Equal(TestStatus.Passed, result.Status);
    }

    [Fact]
    public async Task DoesNotRetryExplicitlyAcceptedTransientStatus()
    {
        var calls = 0;
        var result = await new E2EEngine(TestConfig(retries: 2)).ExecuteStoryAsync(new Story
        {
            Name = "accepted transient",
            Steps = new[]
            {
                new Step
                {
                    Name = "expected failure",
                    Method = "Call",
                    AllowedStatuses = new[] { 500 },
                    ExecuteAsync = MethodExecutor.WithHttpInfo(_ =>
                    {
                        calls++;
                        return ExecutionResult.WithHttp(null, 500);
                    })
                }
            }
        });

        Assert.Equal(TestStatus.Passed, result.Status);
        Assert.Equal(1, calls);
        Assert.Equal(1, result.Steps[0].Attempts);
    }

    [Fact]
    public async Task ExtractsGeneratedApiExceptionMetadata()
    {
        var result = await new E2EEngine(TestConfig()).ExecuteStoryAsync(new Story
        {
            Name = "api-error",
            Steps = new[]
            {
                new Step
                {
                    Name = "call",
                    Method = "Call",
                    ExecuteAsync = MethodExecutor.Sync(_ => throw new billingapi.Client.ApiException(401, "unauthorized", "body"))
                }
            }
        });
        Assert.Equal(401, result.Steps[0].StatusCode);
        Assert.Equal("body", result.Steps[0].Body);
    }

    [Fact]
    public async Task AcceptsAllowedStatusFromGeneratedApiException()
    {
        var result = await new E2EEngine(TestConfig()).ExecuteStoryAsync(new Story
        {
            Name = "expected-api-error",
            Steps = new[]
            {
                new Step
                {
                    Name = "missing resource",
                    Method = "Get",
                    AllowedStatuses = new[] { 404 },
                    ExecuteAsync = MethodExecutor.Sync(_ =>
                        throw new billingapi.Client.ApiException(404, "not found", "{\"message\":\"not found\"}"))
                }
            }
        });

        Assert.Equal(TestStatus.Passed, result.Status);
        Assert.Equal(TestStatus.Passed, result.Steps[0].Status);
        Assert.Equal(404, result.Steps[0].StatusCode);
        Assert.Null(result.Steps[0].Error);
        Assert.Contains("not found", result.Steps[0].Body);
    }

    [Fact]
    public async Task APreCancelledTokenStopsTheRunEvenWithoutExecutableSteps()
    {
        using var cancelled = new CancellationTokenSource();
        cancelled.Cancel();
        var story = new Story
        {
            Name = "skipped only",
            Steps = new[]
            {
                new Step { Name = "skipped", Method = "Get", Skip = true, SkipReason = "unavailable",
                    ExecuteAsync = MethodExecutor.Async((_, _) => Task.FromResult<object?>(null)) }
            }
        };

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => new E2EEngine(new Config { MaxRetries = 0 }).ExecuteAsync(new[] { story }, cancelled.Token));
    }

    [Fact]
    public async Task AStatusConstraintFailsWhenTheCallReportsNoStatus()
    {
        var story = new Story
        {
            Name = "status constraint",
            Steps = new[]
            {
                new Step
                {
                    Name = "expects 501",
                    Method = "GetSomething",
                    // A plain SDK call surfaces no status, so the assertion could never fail before.
                    ExpectedStatus = 501,
                    ExecuteAsync = MethodExecutor.Async((_, _) => Task.FromResult<object?>(new { ok = true }))
                }
            }
        };

        var result = await new E2EEngine(new Config { MaxRetries = 0 }).ExecuteStoryAsync(story);

        Assert.Equal(TestStatus.Failed, result.Status);
        Assert.Contains("declares a status constraint", result.Steps[0].Error!.Message, StringComparison.Ordinal);
    }

    [Fact]
    public async Task StateChangesRecordTheTransitionIncludingRemovals()
    {
        var story = new Story
        {
            Name = "transitions",
            InitialVariables = new Dictionary<string, object?> { ["kept"] = "before", ["dropped"] = "gone" },
            Steps = new[]
            {
                new Step
                {
                    Name = "update",
                    Method = "Update",
                    ExecuteAsync = MethodExecutor.Async((_, _) => Task.FromResult<object?>(null)),
                    UpdateStateAsync = (_, context) =>
                    {
                        context.Variables["kept"] = "after";
                        context.Variables.Remove("dropped");
                        return Task.CompletedTask;
                    }
                }
            }
        };

        var result = await new E2EEngine(new Config { MaxRetries = 0 }).ExecuteStoryAsync(story);
        var changes = result.Steps[0].StateChanges!;

        Assert.Equal("before", changes["kept"].OldValue);
        Assert.Equal("after", changes["kept"].NewValue);
        Assert.Equal("gone", changes["dropped"].OldValue);
        Assert.Null(changes["dropped"].NewValue);
    }
}
