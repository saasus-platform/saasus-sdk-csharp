# C# SDK E2E test library

`TestLib` is the shared test foundation for the SaaSus SDK module E2E tests. It is
module-agnostic: auth, billing, pricing, API log, communication, integration, and
AWS Marketplace tests can describe API lifecycles as `Story` and `Step` objects.

## Configuration

`Config.FromEnvironment()` reads the same credentials and endpoint override as the SDK:

```dotenv
SAASUS_SAAS_ID=...
SAASUS_API_KEY=...
SAASUS_SECRET_KEY=...
SAASUS_API_URL_BASE=https://api.saasus.io
```

Execution can be controlled with `E2E_TIMEOUT`, `E2E_MAX_RETRIES`, `E2E_DRY_RUN`,
`E2E_FAIL_FAST`, and `E2E_LOG_LEVEL`. Snapshot capture, comparison, reporting,
output, and breaking-change behavior use the `E2E_SNAPSHOT_*` variables parsed by
`Config`.

## Story example

```csharp
var story = new Story
{
    Name = "billing API lifecycle",
    Module = "billing",
    InitialVariables = new Dictionary<string, object?> { ["secret"] = stripeKey },
    Steps = new[]
    {
        new Step
        {
            Name = "get Stripe info",
            Method = "GetStripeInfoAsync",
            CallStyle = CallStyle.Async,
            ExpectedStatus = 200,
            ExecuteAsync = MethodExecutor.Async(async (_, cancellationToken) =>
                await stripeApi.GetStripeInfoAsync(cancellationToken: cancellationToken))
        },
        new Step
        {
            Name = "get Stripe info with HTTP metadata",
            Method = "GetStripeInfoWithHttpInfoAsync",
            CallStyle = CallStyle.WithHttpInfoAsync,
            ExecuteAsync = MethodExecutor.WithHttpInfoAsync(async (_, cancellationToken) =>
            {
                var response = await stripeApi.GetStripeInfoWithHttpInfoAsync(
                    cancellationToken: cancellationToken);
                return ExecutionResult.WithHttp(response.Data, (int)response.StatusCode);
            })
        }
    },
    CleanupAsync = (_, _) => DeleteCreatedResourcesAsync()
};

var config = Config.FromEnvironment();
config.Validate();
var engine = new E2EEngine(config);
var result = await engine.ExecuteStoryAsync(story);
```

Cleanup runs even if setup, a step, validation, or state update fails. HTTP 429,
5xx, transport errors, and timeouts are retried. Steps can validate responses and
store values in `TestContext.Variables` for later steps.

## Reporting, logging and coverage

`CoverageTracker` records method and call-style coverage and exposes `Coverage`
(covered / total / percentage), `IsFullyCovered` and per-method `Stats`
(executions, success rate, average duration).

`Reporter` renders three views over the same run:

- `Render` — console text with story/step counts, coverage percentage, HTTP status
  codes, retry counts, setup and cleanup errors, and untested calls.
- `Summarize` — a structured `ReportSummary` for assertions.
- `ToJson` / `WriteJsonAsync` — a machine-readable document using the same snake_case
  contract as the snapshot artifacts. Set `E2E_REPORT_JSON` to have the Billing E2E
  test write it automatically.

`Logger` emits a structured trace at `E2E_LOG_LEVEL=debug`: story and step
boundaries, validation outcomes, state updates, skips, retries and the final coverage
percentage. Every message passes through `Masker`, so credentials appear as
`[MASKED len=N]` and bearer tokens as `Bearer [MASKED]`.

## Snapshot compatibility testing

`SnapshotFactory`, `SnapshotStore`, `SnapshotMasker`, `SnapshotComparer`,
`SnapshotValidator`, `SnapshotReporter`, and `SnapshotManager` implement versioned
compatibility snapshots. They support capture/compare/report/full modes, Git-derived or
explicit tags, offline comparisons, capture levels, story filters, structured response
and error data, validation, atomic writes, and JSON/HTML reports.

### Wire format

Artifacts use the **same schema as `saasus-sdk-go` and `saasus-sdk-php`** so they can be
diffed across languages: snake_case keys, lower-case status strings (`passed`),
upper-case capture levels (`FULL`), nanosecond durations, and HTTP metadata nested under
`return_value.http_response`.

```json
{
  "story_name": "Billing API - synchronous HTTP responses",
  "description": "...",
  "timestamp": "2026-08-05T00:28:34.340164+00:00",
  "duration": 1013000000,
  "status": "passed",
  "variables": { "billing_state": "unregistered" },
  "steps": [
    {
      "step_name": "Pre_GetStripeConnectionInformation",
      "method": "GetStripeInfoWithHttpInfo",
      "parameters": {},
      "return_value": {
        "type": "billingapi.Model.StripeInfo",
        "status_code": 200,
        "status": "200 OK",
        "http_response": {
          "status_code": 200,
          "status": "200 OK",
          "headers": { "Content-Type": "application/json; charset=UTF-8" },
          "content_length": 29,
          "trace_id": "811dbb54-..."
        },
        "json_data": { "is_registered": false },
        "body": "{\n  \"is_registered\": false\n}\n",
        "headers": { "Content-Type": "application/json; charset=UTF-8" }
      },
      "duration": 99000000,
      "status_code": 200,
      "success": true,
      "status": "passed",
      "timestamp": "2026-08-05T00:28:34.340164+00:00",
      "call_style": "with_http_info",
      "attempts": 1
    }
  ],
  "summary": { "total_steps": 5, "successful_steps": 5, "failed_steps": 0,
               "skipped_steps": 0, "total_duration": 1013000000,
               "average_step_duration": 202600000 },
  "metadata": { "sdk_version": "unknown", "test_environment": "dev",
                "capture_level": "FULL", "git_tag": "v1.0.0-8-gbcd96cd",
                "git_commit": "d569141..." }
}
```

`call_style` and `attempts` are the only .NET-specific additions: the generated C# SDK
exposes four call styles per method, and the engine retries transient failures.
`skip_reason`, `error` and `state_changes` are omitted when empty, matching Go's
`omitempty`. `SnapshotSchemaTests` locks this contract.

### Determinism

`SnapshotMasker` redacts credentials as `[MASKED len=N]` and normalises volatile values
to `[DYNAMIC]` while preserving the JSON type (integers become `0`, floats `0.0`, empty
strings and nulls are left alone). A built-in field list covers identifiers, e-mail
addresses, timestamps, TTLs and cursors; extend it with `E2E_SNAPSHOT_DYNAMIC_FIELDS` or
disable it with `E2E_SNAPSHOT_DEFAULT_DYNAMIC_FIELDS=false`.

### Comparison rules

Removed fields, JSON type changes, shrinking arrays, and changes to `status`,
`status_code`, `method`, `step_name`, `return_value.type` or `return_value.json_data`
are **breaking**, as is any `passed` → non-`passed` transition. Added fields, growing
arrays, header changes, `state_changes` and `variables` changes are **warnings**.

Ignored: everything under `metadata`, all `timestamp` and `trace_id` values, `attempts`,
and the volatile headers `Date`, `Server`, `X-Saasus-Trace-Id`, `X-Request-Id`,
`X-Correlation-Id` and `X-Runtime`. Durations are only reported when they change by more
than 50%, and then as a `timing` warning. Two redacted values compare as equal, so
rotating a test credential does not produce a diff.

### Validation

Four configurable rules — `completion`, `sequence`, `state_transition` and `timing` —
each with an enabled flag and a severity (`error`, `warning`, `info`). Defaults match
Go and PHP: completion and sequence are errors, state transitions are warnings, timing
is off. Override them per rule through `validation_rules` in the JSON configuration
file.

Findings are bucketed into `sequence_errors`, `state_transition_errors` and
`timing_errors`, with `skipped_steps` and a severity `summary`. Each artifact embeds a
`comparison` block against the previous run (`previous_file`, `new_findings`,
`resolved_findings` and severity deltas), and only the newest
`E2E_SNAPSHOT_HISTORY_LIMIT` artifacts per story are retained.
