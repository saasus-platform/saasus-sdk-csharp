# Auth API E2E tests

These tests follow the Auth resource and configuration flows covered by
`saasus-sdk-go/tests/e2e/authapi` and `saasus-sdk-php/test/e2e`.  The generated C# Auth
client exposes four call styles, so each operation is executed with synchronous,
synchronous `WithHttpInfo`, asynchronous, and asynchronous `WithHttpInfo` methods.
The offline story test checks that all generated successful Auth operations (85 base
operations, 340 call-style methods) remain represented in the stories.

The live test changes Auth API data and is disabled unless explicitly enabled:

```bash
SAASUS_E2E=true \
SAASUS_SAAS_ID=... \
SAASUS_API_KEY=... \
SAASUS_SECRET_KEY=... \
dotnet test tests/SaaSusSdk.Tests.csproj --filter 'Category=E2E'
```

Use a dedicated SaaSus environment. `SAASUS_API_URL_BASE` can override the default
`https://api.saasus.io` host.  The stories use unique resource names and clean up
 created users, tenants, environments, roles, attributes, and tenant users after a
 partial failure. Every successful response is checked for its generated model type,
 resource ID, expected email/name, collection membership, and custom-attribute value.

External integrations are conditional. If Cognito is configured, the test provider
authenticates the configured user and, when AWS credentials are available, provisions
the per-story SaaS user and prepares MFA/TOTP tokens. If `STRIPE_SECRET_KEY` is set,
the Billing API is used to register Stripe before Stripe-dependent Auth operations
run. Missing or unavailable dependencies are recorded as skipped with an explicit
reason.

Optional Auth dependency variables:

| Variable | Purpose |
|---|---|
| `E2E_COGNITO_USER_POOL_ID`, `E2E_COGNITO_CLIENT_ID` | Cognito app configuration |
| `E2E_COGNITO_USERNAME`, `E2E_COGNITO_PASSWORD` | Initial Cognito user for token acquisition |
| `E2E_COGNITO_REGION`, `E2E_COGNITO_ENDPOINT` | Region and optional Cognito endpoint override |
| `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `AWS_SESSION_TOKEN` | Signed Cognito admin/MFA operations |
| `AUTH_E2E_EMAIL_CONFIRMATION_CODE` | Enables email update confirmation steps |
| `AUTH_E2E_EXTERNAL_PROVIDER_NAME`, `AUTH_E2E_EXTERNAL_PROVIDER_ACCESS_TOKEN`, `AUTH_E2E_EXTERNAL_PROVIDER_CODE` | Enables external-provider steps; the access-token override is optional |
| `AUTH_E2E_SKIP_SIGNUP_ON_COGNITO_EMAIL_LIMIT=0` | Enables sign-up steps (disabled by default) |

The dedicated Raw response stories execute the 16 read-only Auth operations through
`WithHttpInfo` and `WithHttpInfoAsync`, checking headers, a non-empty body, valid JSON,
and deserialized content.

## Snapshots

Set `E2E_SNAPSHOT_MODULE=auth` when capturing or comparing Auth snapshots so they are
written below `tests/E2E/Snapshots/auth/`. The `Module=auth` trait filter is part of every
command: `Category=Snapshot` alone would also execute the Billing, Communication and
Integration snapshot tests against the live environment.

```bash
SAASUS_E2E=true E2E_SNAPSHOT_MODE=capture E2E_SNAPSHOT_MODULE=auth \
E2E_SNAPSHOT_TAG=auth-v1 \
dotnet test tests/SaaSusSdk.Tests.csproj --filter 'Category=Snapshot&Module=auth'

E2E_SNAPSHOT_MODE=compare E2E_SNAPSHOT_MODULE=auth \
E2E_SNAPSHOT_COMPARISON_MODE=manual \
E2E_SNAPSHOT_OLD_TAG=auth-v1 E2E_SNAPSHOT_NEW_TAG=auth-v2 \
dotnet test tests/SaaSusSdk.Tests.csproj --filter 'Category=Snapshot&Module=auth'
```
