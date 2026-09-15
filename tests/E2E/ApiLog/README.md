# ApiLog API E2E tests

These tests follow the `GetLogs` / `GetLog` flow covered by the Go and PHP SDK
E2E suites:

1. Retrieve the API log list.
2. Extract the first log's ID, date, timestamp, and cursor.
3. Retrieve logs again with the extracted query parameters.
4. Retrieve the extracted log by ID.

The generated C# client exposes four call styles, so the story runs with
synchronous, synchronous `WithHttpInfo`, asynchronous, and asynchronous
`WithHttpInfo` methods. The offline story test verifies all eight generated
ApiLog methods remain covered.

The live test is disabled unless explicitly enabled:

```bash
SAASUS_E2E=true \
SAASUS_SAAS_ID=... \
SAASUS_API_KEY=... \
SAASUS_SECRET_KEY=... \
dotnet test tests/SaaSusSdk.Tests.csproj --filter 'Category=E2E&FullyQualifiedName~ApiLog'
```

`SAASUS_API_URL_BASE` can override the default `https://api.saasus.io` host.
The test is read-only, but it requires an environment containing at least one
API log so that the individual `GetLog` step can use a real ID.

## Snapshots

Capture and compare ApiLog snapshots with the `apilog` module directory:

```bash
SAASUS_E2E=true E2E_SNAPSHOT_MODE=capture E2E_SNAPSHOT_MODULE=apilog \
E2E_SNAPSHOT_TAG=apilog-v1 \
dotnet test tests/SaaSusSdk.Tests.csproj --filter 'Category=Snapshot&FullyQualifiedName~ApiLog'

E2E_SNAPSHOT_MODE=compare E2E_SNAPSHOT_MODULE=apilog \
E2E_SNAPSHOT_COMPARISON_MODE=manual \
E2E_SNAPSHOT_OLD_TAG=apilog-v1 E2E_SNAPSHOT_NEW_TAG=apilog-v2 \
dotnet test tests/SaaSusSdk.Tests.csproj --filter 'Category=Snapshot&FullyQualifiedName~ApiLog'
```

Capture requests a single log per list call and retries until one is visible, so
the snapshot does not depend on how much history the environment holds; a page
that stays empty fails the step instead of being captured. Which request happens
to be logged still differs between runs, so the recorded traffic values
(`request_uri`, `request_method`, `response_status`, `request_body`,
`response_body`, `remote_address`) and the IDs and timestamps are normalised to
`[DYNAMIC]`; the field set itself stays in the artifact so model changes are
still detected.
