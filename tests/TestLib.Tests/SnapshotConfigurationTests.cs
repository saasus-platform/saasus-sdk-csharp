using billingapi.Model;
using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

public sealed class SnapshotConfigurationTests
{
    [Theory]
    [InlineData(CaptureLevel.Full, 1, true, true)]
    [InlineData(CaptureLevel.Story, 0, false, false)]
    [InlineData(CaptureLevel.Step, 1, true, false)]
    [InlineData(CaptureLevel.Response, 1, false, true)]
    public void AppliesCaptureLevel(
        CaptureLevel level,
        int expectedSteps,
        bool expectsParameters,
        bool expectsResponse)
    {
        var (story, result) = Execution();

        var snapshot = SnapshotFactory.Create(story, result, new SnapshotConfig
        {
            CaptureLevel = level,
            DynamicFields = new[] { "request_id" }
        });

        Assert.Equal(expectedSteps, snapshot.Steps.Count);
        // Go and PHP serialise story variables at every capture level.
        Assert.NotEmpty(snapshot.Variables);
        if (expectedSteps == 0) return;
        Assert.Equal(expectsParameters, snapshot.Steps[0].Parameters.HasValues);
        Assert.Equal(expectsResponse, snapshot.Steps[0].ReturnValue is not null);
        // state_changes carry old_value / new_value / timestamp, masked with the variable's key.
        var change = (JObject)JToken.FromObject(snapshot.Steps[0].StateChanges!["request_id"]!);
        Assert.Equal("[DYNAMIC]", change["new_value"]);
        Assert.Equal(new[] { "old_value", "new_value", "timestamp" },
            change.Properties().Select(property => property.Name).ToArray());
    }

    [Fact]
    public async Task CapturesAndMasksResolvedRequestParameters()
    {
        const string secret = "sk_test_must_not_appear";
        var story = new Story
        {
            Name = "parameters",
            InitialVariables = new Dictionary<string, object?> { ["secret"] = secret },
            Steps = new[]
            {
                new Step
                {
                    Name = "update",
                    Method = "UpdateStripeInfo",
                    Parameters = (Func<TestContext, object?>)(context =>
                        new UpdateStripeInfoParam(context.GetRequired<string>("secret"))),
                    ExecuteAsync = MethodExecutor.Sync(_ => { })
                }
            }
        };
        var result = await new E2EEngine(new Config { LogLevel = LogLevel.None }).ExecuteStoryAsync(story);

        var snapshot = SnapshotFactory.Create(story, result, new SnapshotConfig());
        var json = snapshot.Steps[0].Parameters.ToString();

        Assert.DoesNotContain(secret, json, StringComparison.Ordinal);
        Assert.Equal($"[MASKED len={secret.Length}]", snapshot.Steps[0].Parameters["secret_key"]);
        Assert.IsType<UpdateStripeInfoParam>(result.Steps[0].Parameters);
    }

    [Fact]
    public void ExcludesConfiguredDynamicFields()
    {
        var processed = JObject.FromObject(new SnapshotMasker(
            new[] { "id" }, DynamicFieldMode.Exclude, useDefaults: false).Process(new
            {
                id = "generated",
                nested = new { id = "nested", stable = true }
            })!);

        Assert.Null(processed["id"]);
        Assert.Null(processed["nested"]!["id"]);
        Assert.True(processed["nested"]!["stable"]!.Value<bool>());
    }

    [Fact]
    public void MatchesStoriesCaseInsensitively()
    {
        var config = new SnapshotConfig { StoryFilters = new[] { "async HTTP" } };

        Assert.True(config.MatchesStory("Billing API - AsYnC hTtP responses"));
        Assert.False(config.MatchesStory("Billing API - synchronous responses"));
    }

    [Fact]
    public void StoryLevelSnapshotsReportCompleteWhenTheStoryPassed()
    {
        var (story, result) = Execution();

        var snapshot = SnapshotFactory.Create(story, result, new SnapshotConfig { CaptureLevel = CaptureLevel.Story });
        var validation = new SnapshotValidator().Validate(snapshot, new SnapshotConfig());

        // Steps are empty at STORY level by design, so the story status decides.
        Assert.Equal("complete", validation.CompletionStatus);
        Assert.True(validation.IsValid);
    }

    [Fact]
    public void StoryLevelSummaryCountsOnlyTheCapturedSteps()
    {
        var (story, result) = Execution();

        var snapshot = SnapshotFactory.Create(story, result, new SnapshotConfig { CaptureLevel = CaptureLevel.Story });

        // The steps array is empty at STORY level, so the counts must be empty too.
        Assert.Empty(snapshot.Steps);
        Assert.Equal(0, snapshot.Summary.TotalSteps);
        Assert.Equal(0, snapshot.Summary.SuccessfulSteps);
        Assert.Equal(0, snapshot.Summary.AverageStepDurationNanoseconds);
    }

    [Fact]
    public void MatchesStoriesBySlug()
    {
        var config = new SnapshotConfig { StoryFilters = new[] { "billing_api_-_asynchronous_responses" } };

        Assert.True(config.MatchesStory("Billing API - asynchronous responses"));
        Assert.False(config.MatchesStory("Billing API - synchronous responses"));
    }

    [Fact]
    public void ValidationRulesCanBeDisabledIndependently()
    {
        var validation = new SnapshotValidator().Validate(InvalidSnapshot(), new SnapshotConfig
        {
            ValidateCompletion = false,
            ValidateSequence = false,
            ValidateStateTransitions = false,
            ValidateTiming = false
        });

        // The always-on incomplete-execution detector still reports the missing step failure.
        Assert.False(validation.IsValid);
        var finding = Assert.Single(validation.SequenceErrors!);
        Assert.Contains("no step reported a failure", finding.Message, StringComparison.Ordinal);
        Assert.Null(validation.StateTransitionErrors);
        Assert.Null(validation.TimingErrors);
    }

    [Fact]
    public void ValidationRuleSeverityIsConfigurable()
    {
        var snapshot = SnapshotFixtures.Story(status: TestStatus.Failed,
            steps: new[] { SnapshotFixtures.Step(status: TestStatus.Failed, statusCode: 500,
                returnValue: SnapshotFixtures.ReturnValue(new { ok = false }, statusCode: 500)) },
            summary: SnapshotFixtures.Summary(1, 0, 1, 0));

        var strict = new SnapshotValidator().Validate(snapshot, new SnapshotConfig());
        Assert.False(strict.IsValid);
        Assert.Contains(strict.Findings, finding =>
            finding.Type == "sequence" && finding.Message == "Story execution is not complete" &&
            finding.Severity == SnapshotConfig.SeverityError);

        var lenient = new SnapshotValidator().Validate(snapshot, new SnapshotConfig
        {
            ValidationRuleOverrides = new Dictionary<string, ValidationRule>(StringComparer.OrdinalIgnoreCase)
            {
                ["completion"] = new(true, SnapshotConfig.SeverityWarning),
                ["state_transition"] = new(true, SnapshotConfig.SeverityWarning)
            }
        });
        Assert.True(lenient.IsValid);
        Assert.Contains(lenient.Findings, finding =>
            finding.Type == "sequence" && finding.Message == "Story execution is not complete" &&
            finding.Severity == SnapshotConfig.SeverityWarning);
        Assert.Equal(0, lenient.Summary.TotalErrors);
    }

    [Fact]
    public void ReportsAFailedStepThatRecordsASuccessStatus()
    {
        var snapshot = SnapshotFixtures.Story(status: TestStatus.Failed, steps: new[]
        {
            SnapshotFixtures.Step(status: TestStatus.Failed, statusCode: 200,
                returnValue: SnapshotFixtures.ReturnValue(new { ok = true }),
                error: new SnapshotError("System.InvalidOperationException", "boom", null))
        }, summary: SnapshotFixtures.Summary(1, 0, 1, 0));

        var validation = new SnapshotValidator().Validate(snapshot, new SnapshotConfig
        {
            ValidationRuleOverrides = new Dictionary<string, ValidationRule>(StringComparer.OrdinalIgnoreCase)
            {
                ["state_transition"] = new(true, SnapshotConfig.SeverityError)
            }
        });

        var finding = Assert.Single(validation.StateTransitionErrors!);
        Assert.Equal("Success flag does not match HTTP status", finding.Message);
        // The configured severity must be honoured instead of a hardcoded warning.
        Assert.Equal(SnapshotConfig.SeverityError, finding.Severity);
    }

    [Fact]
    public void AcceptsAPassedStepWithANegativePathStatus()
    {
        var snapshot = SnapshotFixtures.Story(steps: new[]
        {
            SnapshotFixtures.Step(statusCode: 501, returnValue: SnapshotFixtures.ReturnValue(new { ok = false }, statusCode: 501))
        });

        var validation = new SnapshotValidator().Validate(snapshot, new SnapshotConfig());

        Assert.Null(validation.StateTransitionErrors);
        Assert.True(validation.IsValid);
    }

    [Fact]
    public void CaptureFailedDoesNotSuppressCompletionForPassedStories()
    {
        var config = new SnapshotConfig { CaptureFailedStories = true };
        var emptyPassed = SnapshotFixtures.Story(
            steps: Array.Empty<StepSnapshot>(), summary: SnapshotFixtures.Summary(0, 0, 0, 0));

        // A passed story with no steps is a framework bug and must still be rejected.
        Assert.False(new SnapshotValidator().Validate(emptyPassed, config).IsValid);

        var failed = SnapshotFixtures.Story(status: TestStatus.Failed,
            steps: new[] { SnapshotFixtures.Step(status: TestStatus.Failed, statusCode: 500,
                returnValue: SnapshotFixtures.ReturnValue(new { ok = false }, statusCode: 500),
                error: new SnapshotError("System.InvalidOperationException", "boom")) },
            summary: SnapshotFixtures.Summary(1, 0, 1, 0));
        Assert.DoesNotContain(new SnapshotValidator().Validate(failed, config).Findings,
            finding => finding.Message == "Story execution is not complete");
    }

    [Fact]
    public void FullModeAcceptsAManualBaselineWithoutANewTag()
    {
        // --mode full --comparison-mode manual --old-tag baseline: the candidate is the tag being
        // captured, so only the baseline has to be supplied.
        new SnapshotConfig
        {
            Mode = SnapshotMode.Full,
            ComparisonMode = SnapshotComparisonMode.Manual,
            OldTag = "baseline",
            CurrentTag = "candidate"
        }.Validate();

        Assert.Throws<InvalidOperationException>(() => new SnapshotConfig
        {
            Mode = SnapshotMode.Compare,
            ComparisonMode = SnapshotComparisonMode.Manual,
            OldTag = "baseline"
        }.Validate());
    }

    [Fact]
    public void WrapsNonObjectParametersInTheSharedValueProperty()
    {
        var result = new StoryResult("story", TestStatus.Passed, TimeSpan.Zero, new[]
        {
            new StepResult("get", "Get", CallStyle.Sync, TestStatus.Passed, TimeSpan.Zero,
                Parameters: new[] { "a", "b" })
        });

        var parameters = SnapshotFactory
            .Create(new Story { Name = "story" }, result, new SnapshotConfig()).Steps[0].Parameters;

        // PHP's snapshotParameters stores a scalar or list under "value".
        Assert.Equal(JTokenType.Object, parameters.Type);
        Assert.Equal(new[] { "a", "b" }, parameters["value"]!.Select(item => item.Value<string>()));
    }

    [Fact]
    public void RejectsModuleNamesThatEscapeTheOutputDirectory()
    {
        foreach (var module in new[] { "..", "../", "." })
            Assert.Throws<InvalidOperationException>(
                () => new SnapshotConfig { ModuleName = module }.Validate());

        new SnapshotConfig { ModuleName = "billing" }.Validate();
    }

    [Fact]
    public void RejectsUndefinedSnapshotEnumValues()
    {
        Assert.Throws<InvalidOperationException>(
            () => new SnapshotConfig { Mode = (SnapshotMode)99 }.Validate());
        Assert.Throws<InvalidOperationException>(
            () => new SnapshotConfig { CaptureLevel = (CaptureLevel)99 }.Validate());
        Assert.Throws<InvalidOperationException>(
            () => new SnapshotConfig { ComparisonMode = (SnapshotComparisonMode)99 }.Validate());
        Assert.Throws<InvalidOperationException>(
            () => new SnapshotConfig { DynamicFieldMode = (DynamicFieldMode)99 }.Validate());
    }

    [Fact]
    public void UsesTheNestedReturnValueStatusForTheStateRule()
    {
        // The top-level status is missing while the nested response reports a server error.
        var snapshot = SnapshotFixtures.Story(steps: new[]
        {
            SnapshotFixtures.Step(statusCode: 0,
                returnValue: SnapshotFixtures.ReturnValue(new { ok = false }, statusCode: 503))
        });

        var validation = new SnapshotValidator().Validate(snapshot, new SnapshotConfig());

        // A passed step with a 5xx response is still an accepted negative path...
        Assert.Null(validation.StateTransitionErrors);

        // ...but a failed step reporting 2xx is a contradiction.
        var contradiction = SnapshotFixtures.Story(status: TestStatus.Failed, steps: new[]
        {
            SnapshotFixtures.Step(status: TestStatus.Failed, statusCode: 0,
                returnValue: SnapshotFixtures.ReturnValue(new { ok = true }, statusCode: 200),
                error: new SnapshotError("System.InvalidOperationException", "boom"))
        }, summary: SnapshotFixtures.Summary(1, 0, 1, 0));
        Assert.Contains(new SnapshotValidator().Validate(contradiction, new SnapshotConfig()).Findings,
            finding => finding.Message == "Success flag does not match HTTP status");
    }

    [Fact]
    public void ClassifiesCompletionStatusLikeTheReference()
    {
        string Status(StorySnapshot snapshot) =>
            new SnapshotValidator().Validate(snapshot, new SnapshotConfig()).CompletionStatus;

        // A failed setup captured with no steps is "failed", not "incomplete".
        Assert.Equal("failed", Status(SnapshotFixtures.Story(status: TestStatus.Failed,
            steps: Array.Empty<StepSnapshot>(), summary: SnapshotFixtures.Summary(0, 0, 0, 0))));
        Assert.Equal("partial", Status(SnapshotFixtures.Story(status: TestStatus.Failed,
            steps: Array.Empty<StepSnapshot>(), summary: SnapshotFixtures.Summary(2, 1, 1, 0))));
        Assert.Equal("incomplete", Status(SnapshotFixtures.Story(status: TestStatus.Skipped,
            steps: Array.Empty<StepSnapshot>(), summary: SnapshotFixtures.Summary(0, 0, 0, 0))));
    }

    [Fact]
    public void RejectsFileNameFormatsMissingAPlaceholder()
    {
        foreach (var format in new[] { "story_snapshot_{tag}.json", "story_snapshot_{story_name}.json" })
        {
            var error = Assert.Throws<InvalidOperationException>(
                () => new SnapshotConfig { FileNameFormat = format }.Validate());
            Assert.Contains("{tag} and {story_name}", error.Message, StringComparison.Ordinal);
        }

        new SnapshotConfig { FileNameFormat = "snap_{tag}_{story_name}.json" }.Validate();

        // A format carrying a directory would write outside the directory discovery searches.
        foreach (var format in new[] { "../snap_{tag}_{story_name}.json", "nested/snap_{tag}_{story_name}.json" })
        {
            var error = Assert.Throws<InvalidOperationException>(
                () => new SnapshotConfig { FileNameFormat = format }.Validate());
            Assert.Contains("not a path", error.Message, StringComparison.Ordinal);
        }
    }

    [Fact]
    public void RejectsUnknownValidationRulesAndSeverities()
    {
        Assert.Throws<InvalidOperationException>(() => new SnapshotConfig
        {
            ValidationRuleOverrides = new Dictionary<string, ValidationRule> { ["nope"] = new(true, "error") }
        }.Validate());

        Assert.Throws<InvalidOperationException>(() => new SnapshotConfig
        {
            ValidationRuleOverrides = new Dictionary<string, ValidationRule> { ["timing"] = new(true, "fatal") }
        }.Validate());
    }

    [Fact]
    public void ReportsSkippedStepsAndTimingFindings()
    {
        var snapshot = SnapshotFixtures.Story(steps: new[]
        {
            SnapshotFixtures.Step("slow", returnValue: SnapshotFixtures.ReturnValue(new { ok = true }),
                durationNanoseconds: SnapshotValidator.SlowStepThresholdNanoseconds + 1),
            SnapshotFixtures.Step("skipped", status: TestStatus.Skipped, statusCode: 0,
                skipReason: "not available in CI")
        }, summary: SnapshotFixtures.Summary(2, 1, 0, 1));

        var validation = new SnapshotValidator().Validate(snapshot, new SnapshotConfig { ValidateTiming = true });

        var skipped = Assert.Single(validation.SkippedSteps!);
        Assert.Equal("skipped", skipped.StepName);
        Assert.Equal("not available in CI", skipped.Reason);
        var timing = Assert.Single(validation.TimingErrors!);
        Assert.Equal("slow", timing.StepName);
        Assert.Equal(SnapshotConfig.SeverityInfo, timing.Severity);
    }

    /// <summary>
    /// A failed story whose only step passed and carries no error: the always-on
    /// incomplete-execution detector must report it whatever the configurable rules say.
    /// </summary>
    private static StorySnapshot InvalidSnapshot() =>
        SnapshotFixtures.Story(status: TestStatus.Failed,
            steps: new[] { SnapshotFixtures.Step(durationNanoseconds: 0,
                returnValue: SnapshotFixtures.ReturnValue(new { ok = true })) },
            summary: SnapshotFixtures.Summary(1, 1, 0, 0, 0));

    private static (Story Story, StoryResult Result) Execution()
    {
        var timestamp = DateTimeOffset.UtcNow;
        var story = new Story
        {
            Name = "Billing story",
            Description = "capture levels",
            InitialVariables = new Dictionary<string, object?> { ["tenant_id"] = "tenant" },
            Steps = new[]
            {
                new Step
                {
                    Name = "Get",
                    Method = "Get",
                    ExecuteAsync = MethodExecutor.Sync(_ => new { ok = true })
                }
            }
        };
        var step = new StepResult("Get", "Get", CallStyle.Sync, TestStatus.Passed,
            TimeSpan.FromMilliseconds(1), 200, new { ok = true }, Body: "{\"ok\":true}",
            Timestamp: timestamp,
            StateChanges: new Dictionary<string, StateChange>
            {
                ["request_id"] = new(null, "generated", timestamp)
            },
            Parameters: new Dictionary<string, object?> { ["tenant_id"] = "tenant" });
        var result = new StoryResult(story.Name, TestStatus.Passed, TimeSpan.FromMilliseconds(1),
            new[] { step }, Variables: story.InitialVariables);
        return (story, result);
    }
}
