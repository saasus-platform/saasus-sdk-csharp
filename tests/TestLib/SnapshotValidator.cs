namespace SaasusSdk.Tests.TestLib;

/// <summary>
/// Applies the four configurable validation rules (completion, sequence,
/// state transition, timing) to a captured snapshot. Rule severities are
/// configurable and findings are bucketed exactly like the Go and PHP suites.
/// </summary>
public sealed class SnapshotValidator
{
    /// <summary>Five minutes expressed in nanoseconds, the timing threshold shared with Go and PHP.</summary>
    public const long SlowStepThresholdNanoseconds = 300_000_000_000L;

    public SnapshotValidation Validate(StorySnapshot snapshot, SnapshotConfig? config = null)
    {
        config ??= new SnapshotConfig();
        var captureLevel = snapshot.Metadata.CaptureLevel;
        var sequence = new List<ValidationFinding>();
        var stateTransitions = new List<ValidationFinding>();
        var timings = new List<ValidationFinding>();

        var completionRule = config.Rule("completion");
        // Steps are intentionally empty at STORY level, so completeness cannot be judged. The
        // capture-failed bypass applies only to stories that actually failed: a passed story with
        // no steps is a framework bug and must still be rejected.
        if (completionRule.Enabled && captureLevel != CaptureLevel.Story &&
            !(config.CaptureFailedStories && snapshot.Status == TestStatus.Failed) &&
            !IsComplete(snapshot))
        {
            // Go and PHP both file this finding as a "sequence" error while keeping the
            // completion rule's severity. Validation history keys findings by type, so the
            // type must match or sibling baselines report spurious new/resolved findings.
            sequence.Add(new ValidationFinding("sequence", string.Empty,
                "Story execution is not complete", completionRule.Severity));
        }

        var sequenceRule = config.Rule("sequence");
        var stateRule = config.Rule("state_transition");
        var timingRule = config.Rule("timing");
        // return_value is intentionally null at STEP level and absent at STORY level.
        var canInspectResponses = captureLevel is not (CaptureLevel.Story or CaptureLevel.Step);

        DateTimeOffset? previousTimestamp = null;
        foreach (var step in snapshot.Steps)
        {
            if (sequenceRule.Enabled && canInspectResponses)
            {
                if (previousTimestamp.HasValue && step.Timestamp < previousTimestamp)
                    sequence.Add(new ValidationFinding("sequence", step.StepName,
                        "Step timestamp is before the previous step", SnapshotConfig.SeverityWarning));

                // A void SDK method legitimately has no return value and no status code.
                var voidReturn = step.Success && step.StatusCode == 0;
                if (step.ReturnValue is null && step.Status != TestStatus.Skipped && !voidReturn)
                    sequence.Add(new ValidationFinding("sequence", step.StepName,
                        "Missing return_value", sequenceRule.Severity));
            }
            previousTimestamp = step.Timestamp;

            if (stateRule.Enabled)
            {
                if (step.Status == TestStatus.Failed && step.Error is null)
                    stateTransitions.Add(new ValidationFinding("state_transition", step.StepName,
                        "Step marked as failed but no error information provided", stateRule.Severity));
                if (step.Status == TestStatus.Passed && step.Error is not null)
                    stateTransitions.Add(new ValidationFinding("state_transition", step.StepName,
                        "Step marked as successful but carries error information", SnapshotConfig.SeverityWarning));
                // Go and PHP read the status from the nested return value and compare it with the
                // success flag; the top-level field is only a fallback.
                var statusCode = step.ReturnValue?.StatusCode is > 0 ? step.ReturnValue.StatusCode : step.StatusCode;
                if (statusCode > 0 && !(step.Status == TestStatus.Passed && statusCode >= 400) &&
                    (step.Status == TestStatus.Passed) != IsSuccessStatus(statusCode))
                    stateTransitions.Add(new ValidationFinding("state_transition", step.StepName,
                        "Success flag does not match HTTP status", stateRule.Severity));
            }

            if (timingRule.Enabled && step.Status != TestStatus.Skipped)
            {
                if (step.DurationNanoseconds == 0)
                    timings.Add(new ValidationFinding("timing", step.StepName,
                        "Step duration is zero", SnapshotConfig.SeverityInfo));
                else if (step.DurationNanoseconds > SlowStepThresholdNanoseconds)
                    timings.Add(new ValidationFinding("timing", step.StepName,
                        "Step duration exceeds five minutes", timingRule.Severity));
            }
        }

        // Always on, like Go's DetectIncompleteExecution: a failed story must name a failed step.
        // The steps themselves are inspected, because a stale summary could otherwise mask it.
        if (snapshot.Status == TestStatus.Failed &&
            !snapshot.Steps.Any(step => step.Status == TestStatus.Failed || step.Error is not null))
            sequence.Add(new ValidationFinding("sequence", string.Empty,
                "Story failed but no step reported a failure", SnapshotConfig.SeverityError));

        var all = sequence.Concat(stateTransitions).Concat(timings).ToArray();
        var errors = all.Count(x => x.Severity == SnapshotConfig.SeverityError);
        var warnings = all.Count(x => x.Severity == SnapshotConfig.SeverityWarning);
        var info = all.Count(x => x.Severity == SnapshotConfig.SeverityInfo);
        var skipped = snapshot.Steps
            .Where(step => step.Status == TestStatus.Skipped)
            .Select(step => new SkippedStepInfo(step.StepName, step.Method,
                string.IsNullOrEmpty(step.SkipReason) ? null : step.SkipReason))
            .ToArray();

        return new SnapshotValidation(
            snapshot.StoryName,
            DateTimeOffset.UtcNow,
            errors == 0,
            CompletionStatus(snapshot),
            sequence.Count == 0 ? null : sequence,
            stateTransitions.Count == 0 ? null : stateTransitions,
            timings.Count == 0 ? null : timings,
            skipped.Length == 0 ? null : skipped,
            null,
            new ValidationSummary(errors, warnings, info, errors == 0));
    }

    private static bool IsSuccessStatus(int statusCode) => statusCode is >= 200 and < 400;

    private static bool IsComplete(StorySnapshot snapshot) =>
        snapshot.Status == TestStatus.Passed && snapshot.Steps.Count > 0 && snapshot.Summary.FailedSteps == 0;

    private static string CompletionStatus(StorySnapshot snapshot)
    {
        // Mirrors PHP's completionStatus: a passing story is complete, a failed story is partial or
        // failed depending on progress, and only a story that neither passed nor failed can be
        // incomplete. A failed setup captured with no steps is therefore reported as failed.
        if (snapshot.Status == TestStatus.Passed && snapshot.Summary.FailedSteps == 0) return "complete";
        if (snapshot.Status == TestStatus.Failed)
            return snapshot.Summary.SuccessfulSteps > 0 ? "partial" : "failed";
        return snapshot.Summary.SuccessfulSteps > 0 && snapshot.Summary.FailedSteps > 0
            ? "partial"
            : "incomplete";
    }
}
