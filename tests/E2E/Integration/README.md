# Integration API E2E tests

The Integration tests reproduce the EventBridge settings lifecycle covered by the
Go and PHP SDK E2E suites. Four stories exercise the C# generated client's four
call styles:

- synchronous object responses;
- synchronous `WithHttpInfo` responses;
- asynchronous object responses; and
- asynchronous `WithHttpInfo` responses.

Together they cover the five EventBridge operations in all 20 generated methods.
The generated `ErrorApi.ReturnInternalServerError` endpoint is intentionally not
included because its successful test behaviour is an HTTP 500 response.

## Live E2E execution

Live tests are skipped unless `SAASUS_E2E=true` is set. The SDK credentials are
read from the same environment variables as the other module tests:

```bash
cd saasus-sdk-csharp
SAASUS_E2E=true \
SAASUS_SAAS_ID=your-saas-id \
SAASUS_API_KEY=your-api-key \
SAASUS_SECRET_KEY=your-secret-key \
dotnet test tests/SaaSusSdk.Tests.csproj --filter Category=E2E
```

Optional test values are `TEST_AWS_ACCOUNT_ID` (default `267185063265`) and
`TEST_AWS_REGION` (default `ap-northeast-1`). The EventBridge settings are
captured before each story and restored after it when a previous configuration
exists.

The `CreateEventBridgeEvent` step expects HTTP 501, matching the Go/PHP E2E
fixtures where that endpoint is not implemented by the API yet.

## Snapshot execution

Use the shared snapshot script with the Integration module selected:

```bash
E2E_SNAPSHOT_MODULE=integration \
bash tests/snapshot.sh --mode full --live --overwrite
```

For offline comparison/reporting, the Integration snapshot artifacts are read from
`tests/E2E/Snapshots/integration/`.
