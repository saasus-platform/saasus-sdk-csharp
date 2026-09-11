using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

/// <summary>
/// Locks the snapshot wire format to the contract shared with saasus-sdk-go and
/// saasus-sdk-php. If any of these assertions fail, artifacts are no longer
/// comparable across languages.
/// </summary>
public sealed class SnapshotSchemaTests
{
    [Fact]
    public void StorySnapshotUsesTheSharedTopLevelContract()
    {
        var document = Serialize(Snapshot());

        Assert.Equal(
            new[] { "story_name", "description", "timestamp", "duration", "status", "variables", "steps", "summary", "metadata" },
            document.Properties().Select(property => property.Name).ToArray());
        Assert.Equal("passed", document["status"]);
        Assert.Equal(JTokenType.Integer, document["duration"]!.Type);
    }

    [Fact]
    public void SummaryAndMetadataUseTheSharedContract()
    {
        var document = Serialize(Snapshot());

        Assert.Equal(
            new[] { "total_steps", "successful_steps", "failed_steps", "skipped_steps", "total_duration", "average_step_duration" },
            ((JObject)document["summary"]!).Properties().Select(property => property.Name).ToArray());
        Assert.Equal(
            new[] { "sdk_version", "test_environment", "capture_level", "git_tag", "git_commit" },
            ((JObject)document["metadata"]!).Properties().Select(property => property.Name).ToArray());
        Assert.Equal("FULL", document["metadata"]!["capture_level"]);
        Assert.Equal("unknown", document["metadata"]!["sdk_version"]);
        Assert.Equal("dev", document["metadata"]!["test_environment"]);
    }

    [Fact]
    public void StepAndReturnValueUseTheSharedContract()
    {
        var step = (JObject)Serialize(Snapshot())["steps"]![0]!;

        // The shared prefix must appear in the same order as Go's StepSnapshot.
        Assert.Equal(
            new[] { "step_name", "method", "parameters", "return_value", "duration", "status_code", "success", "status", "timestamp" },
            step.Properties().Select(property => property.Name).Take(9).ToArray());
        // call_style is a .NET-only addition (four call styles per generated method).
        Assert.Equal(new[] { "call_style", "attempts" }, step.Properties().Select(p => p.Name).TakeLast(2).ToArray());
        Assert.Equal("passed", step["status"]);
        Assert.Equal(200, step["status_code"]);
        Assert.Equal(JTokenType.Integer, step["duration"]!.Type);

        var returnValue = (JObject)step["return_value"]!;
        Assert.Equal(
            new[] { "type", "status_code", "status", "http_response", "json_data", "body", "headers" },
            returnValue.Properties().Select(property => property.Name).ToArray());
        Assert.Equal("200 OK", returnValue["status"]);

        var httpResponse = (JObject)returnValue["http_response"]!;
        Assert.Equal(
            new[] { "status_code", "status", "headers", "content_length", "trace_id" },
            httpResponse.Properties().Select(property => property.Name).ToArray());
        Assert.Equal("abc-123", httpResponse["trace_id"]);
        Assert.Equal(11, httpResponse["content_length"]);
    }

    [Fact]
    public void OptionalStepFieldsAreOmittedWhenAbsent()
    {
        var snapshot = SnapshotFixtures.Story(steps: new[]
        {
            SnapshotFixtures.Step(returnValue: SnapshotFixtures.ReturnValue(new { ok = true }))
        });
        var step = (JObject)Serialize(snapshot)["steps"]![0]!;

        Assert.Null(step["skip_reason"]);
        Assert.Null(step["error"]);
        Assert.Null(step["state_changes"]);
    }

    [Fact]
    public void SkippedAndFailedStepsCarryTheOptionalFields()
    {
        var snapshot = SnapshotFixtures.Story(status: TestStatus.Failed, steps: new[]
        {
            SnapshotFixtures.Step("skipped", status: TestStatus.Skipped, statusCode: 0, skipReason: "unavailable"),
            SnapshotFixtures.Step("failed", status: TestStatus.Failed, statusCode: 500,
                error: new SnapshotError("System.InvalidOperationException", "boom", "{\"code\":1}"),
                stateChanges: new Dictionary<string, object?> { ["state"] = "broken" })
        }, summary: SnapshotFixtures.Summary(2, 0, 1, 1));
        var steps = (JArray)Serialize(snapshot)["steps"]!;

        Assert.Equal("skipped", steps[0]["status"]);
        Assert.Equal("unavailable", steps[0]["skip_reason"]);
        Assert.Equal("failed", steps[1]["status"]);
        Assert.Equal(
            new[] { "type", "message", "details" },
            ((JObject)steps[1]["error"]!).Properties().Select(property => property.Name).ToArray());
        Assert.Equal("broken", steps[1]["state_changes"]!["state"]);
    }

    [Fact]
    public void ValidationArtifactUsesTheSharedContract()
    {
        var validation = new SnapshotValidator().Validate(
            SnapshotFixtures.Story(status: TestStatus.Failed,
                steps: new[] { SnapshotFixtures.Step(status: TestStatus.Failed, statusCode: 500,
                    returnValue: SnapshotFixtures.ReturnValue(new { ok = false }, statusCode: 500),
                    skipReason: null) },
                summary: SnapshotFixtures.Summary(1, 0, 1, 0)));
        var document = Serialize(validation);

        Assert.Equal(
            new[] { "story_name", "validation_time", "is_valid", "completion_status", "sequence_errors", "state_transition_errors", "timing_errors", "summary" },
            document.Properties().Select(property => property.Name).ToArray());
        Assert.Equal(
            new[] { "type", "step_name", "message", "severity" },
            ((JObject)document["sequence_errors"]![0]!).Properties().Select(property => property.Name).ToArray());
        Assert.Equal(
            new[] { "total_errors", "total_warnings", "total_info", "is_valid" },
            ((JObject)document["summary"]!).Properties().Select(property => property.Name).ToArray());
        Assert.Equal(JTokenType.Null, document["timing_errors"]!.Type);
    }

    [Fact]
    public void ComparisonArtifactUsesTheSharedContract()
    {
        var comparison = new SnapshotComparer().Compare("story",
            new { steps = new[] { new { status_code = 200 } } },
            new { steps = new[] { new { status_code = 500 } } }, "v1", "v2");
        var document = Serialize(comparison);

        Assert.Equal(
            new[] { "story_name", "old_tag", "new_tag", "compatibility", "summary", "differences" },
            document.Properties().Select(property => property.Name).ToArray());
        Assert.Equal(new[] { "level", "passed" },
            ((JObject)document["compatibility"]!).Properties().Select(property => property.Name).ToArray());
        Assert.Equal("breaking", document["compatibility"]!["level"]);
        Assert.False(document["compatibility"]!["passed"]!.Value<bool>());
        Assert.Equal(new[] { "differences", "warnings", "breaking_changes" },
            ((JObject)document["summary"]!).Properties().Select(property => property.Name).ToArray());
        Assert.Equal(
            new[] { "type", "path", "description", "old_value", "new_value", "impact" },
            ((JObject)document["differences"]![0]!).Properties().Select(property => property.Name).ToArray());
        Assert.Equal("breaking", document["differences"]![0]!["impact"]);
    }

    [Fact]
    public void ArtifactsAreTwoSpaceIndentedAndNewlineTerminated()
    {
        var text = SnapshotJson.Serialize(SnapshotFixtures.Story());

        Assert.EndsWith("}\n", text, StringComparison.Ordinal);
        Assert.Contains("\n  \"story_name\"", text, StringComparison.Ordinal);
        // Forward slashes must not be escaped, matching Go and PHP's JSON_UNESCAPED_SLASHES.
        Assert.DoesNotContain("\\/", SnapshotJson.Serialize(new { path = "a/b" }), StringComparison.Ordinal);
    }

    [Fact]
    public void DurationsAreExpressedInNanoseconds()
    {
        var result = new StoryResult("story", TestStatus.Passed, TimeSpan.FromMilliseconds(1500),
            new[] { new StepResult("s", "M", CallStyle.Async, TestStatus.Passed, TimeSpan.FromMilliseconds(250)) });
        var snapshot = SnapshotFactory.Create(new Story { Name = "story" }, result, new SnapshotConfig());

        Assert.Equal(1_500_000_000L, snapshot.DurationNanoseconds);
        Assert.Equal(250_000_000L, snapshot.Steps[0].DurationNanoseconds);
        // Go and PHP both sum the captured step durations, so setup, cleanup and the gaps
        // between steps are excluded from the summary.
        Assert.Equal(250_000_000L, snapshot.Summary.TotalDurationNanoseconds);
        Assert.Equal(250_000_000L, snapshot.Summary.AverageStepDurationNanoseconds);
        Assert.Equal(TimeSpan.FromMilliseconds(1500), SnapshotJson.FromNanoseconds(snapshot.DurationNanoseconds));
    }

    [Fact]
    public async Task SnapshotsRoundTripThroughDiskWithoutLosingFields()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"saasus-schema-{Guid.NewGuid():N}");
        try
        {
            var config = new SnapshotConfig { OutputDirectory = directory, ModuleName = "billing", Overwrite = true };
            var store = new SnapshotStore(config);
            var original = Snapshot();

            await store.SaveSnapshotAsync(original, "v1");
            var loaded = await store.LoadSnapshotAsync("v1", original.StoryName);

            Assert.Equal(SnapshotJson.Serialize(original), SnapshotJson.Serialize(loaded));
        }
        finally { if (Directory.Exists(directory)) Directory.Delete(directory, true); }
    }

    private static StorySnapshot Snapshot()
    {
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Content-Type"] = "application/json",
            ["Content-Length"] = "11"
        };
        var returnValue = new SnapshotReturnValue(
            "billingapi.Model.StripeInfo", 200, "200 OK",
            new SnapshotHttpResponse(200, "200 OK", headers, 11, "abc-123"),
            JToken.FromObject(new { is_registered = false }),
            "{\"ok\":true}", headers);
        return SnapshotFixtures.Story(steps: new[] { SnapshotFixtures.Step(returnValue: returnValue) });
    }

    private static JObject Serialize(object value) => JObject.Parse(SnapshotJson.Serialize(value));
}
