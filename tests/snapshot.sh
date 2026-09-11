#!/usr/bin/env bash
# Runs the snapshot compatibility suite. Must be executed from the repository root.
set -euo pipefail

usage() {
  cat <<'EOF'
Usage: bash tests/snapshot.sh --mode capture|compare|report|full [options]

Tags and stories
  --tag TAG                 snapshot tag for the current run (default: git describe)
  --old-tag TAG             baseline tag for comparison
  --new-tag TAG             candidate tag for comparison
  --comparison-mode MODE    release | manual | skip
  --stories FILTERS         comma-separated story names or slugs

Capture
  --capture-level LEVEL     FULL | STORY | STEP | RESPONSE
  --module NAME             snapshot module (apilog | billing | pricing); selects the
                            matching suite and its output directory. Without it every
                            snapshot suite runs and writes to its own module directory.
  --output DIR              snapshot output directory
  --file-name-format FMT    e.g. 'story_snapshot_{tag}_{story_name}.json'
  --capture-failed          capture stories that did not pass
  --overwrite               replace existing artifacts

Determinism
  --dynamic-fields LIST     extra field names normalised to [DYNAMIC]
  --dynamic-mode MODE       replace | exclude
  --no-default-dynamic      disable the built-in dynamic field list

Validation
  --no-validation           skip validation entirely
  --validate-timing         enable the timing rule (off by default)
  --no-validate-completion  disable the completion rule
  --no-validate-sequence    disable the sequence rule
  --no-validate-state       disable the state-transition rule
  --history-limit N         validation artifacts retained per story (default: 2)

Metadata and reporting
  --sdk-version VERSION     value recorded in metadata.sdk_version
  --test-environment NAME   value recorded in metadata.test_environment
  --config FILE             JSON snapshot configuration
  --allow-breaking          do not fail the run on breaking changes
  --verbose                 print the resolved configuration
  --log-level LEVEL         debug | info | warning | error | none
  --live                    allow live capture (sets SAASUS_E2E=true)
EOF
}

# With `set -u`, reading "$2" for a value option that was passed last would abort with an
# unbound-variable error instead of the script's own usage error.
require_value() {
  if [[ $# -lt 2 || -z "$2" ]]; then
    echo "Option $1 requires a value" >&2
    usage >&2
    exit 2
  fi
}

while [[ $# -gt 0 ]]; do
  case "$1" in
    --mode) require_value "$@"; export E2E_SNAPSHOT_MODE="$2"; shift 2 ;;
    --tag) require_value "$@"; export E2E_SNAPSHOT_TAG="$2"; shift 2 ;;
    --old-tag) require_value "$@"; export E2E_SNAPSHOT_OLD_TAG="$2"; shift 2 ;;
    --new-tag) require_value "$@"; export E2E_SNAPSHOT_NEW_TAG="$2"; shift 2 ;;
    --comparison-mode) require_value "$@"; export E2E_SNAPSHOT_COMPARISON_MODE="$2"; shift 2 ;;
    --stories) require_value "$@"; export E2E_SNAPSHOT_STORIES="$2"; shift 2 ;;
    --capture-level) require_value "$@"; export E2E_SNAPSHOT_CAPTURE_LEVEL="$2"; shift 2 ;;
    --module) require_value "$@"; export E2E_SNAPSHOT_MODULE="$2"; shift 2 ;;
    --output) require_value "$@"; export E2E_SNAPSHOT_OUTPUT="$2"; shift 2 ;;
    --file-name-format) require_value "$@"; export E2E_SNAPSHOT_FILE_NAME_FORMAT="$2"; shift 2 ;;
    --capture-failed) export E2E_SNAPSHOT_CAPTURE_FAILED=true; shift ;;
    --overwrite) export E2E_SNAPSHOT_OVERWRITE=true; shift ;;
    --dynamic-fields) require_value "$@"; export E2E_SNAPSHOT_DYNAMIC_FIELDS="$2"; shift 2 ;;
    --dynamic-mode) require_value "$@"; export E2E_SNAPSHOT_DYNAMIC_MODE="$2"; shift 2 ;;
    --no-default-dynamic) export E2E_SNAPSHOT_DEFAULT_DYNAMIC_FIELDS=false; shift ;;
    --no-validation) export E2E_SNAPSHOT_VALIDATION=false; shift ;;
    --validate-timing) export E2E_SNAPSHOT_VALIDATE_TIMING=true; shift ;;
    --no-validate-completion) export E2E_SNAPSHOT_VALIDATE_COMPLETION=false; shift ;;
    --no-validate-sequence) export E2E_SNAPSHOT_VALIDATE_SEQUENCE=false; shift ;;
    --no-validate-state) export E2E_SNAPSHOT_VALIDATE_STATE=false; shift ;;
    --history-limit) require_value "$@"; export E2E_SNAPSHOT_HISTORY_LIMIT="$2"; shift 2 ;;
    --sdk-version) require_value "$@"; export SDK_VERSION="$2"; shift 2 ;;
    --test-environment) require_value "$@"; export E2E_TEST_ENVIRONMENT="$2"; shift 2 ;;
    --config) require_value "$@"; export E2E_SNAPSHOT_CONFIG="$2"; shift 2 ;;
    --allow-breaking) export E2E_SNAPSHOT_FAIL_ON_BREAKING=false; shift ;;
    --verbose) export E2E_SNAPSHOT_VERBOSE=true; shift ;;
    --log-level) require_value "$@"; export E2E_LOG_LEVEL="$2"; shift 2 ;;
    --live) export SAASUS_E2E=true; shift ;;
    -h|--help) usage; exit 0 ;;
    *) echo "Unknown option: $1" >&2; usage >&2; exit 2 ;;
  esac
done

# A JSON configuration file can supply "mode", so --mode is only required without --config.
if [[ -z "${E2E_SNAPSHOT_MODE:-}" && -z "${E2E_SNAPSHOT_CONFIG:-}" ]]; then
  echo "--mode is required unless --config supplies it" >&2
  usage >&2
  exit 2
fi

# Story filters only match the stories of one module, so an unscoped run would fail the
# other modules' snapshot tests. Require the module whenever the story filter is used.
if [[ -n "${E2E_SNAPSHOT_STORIES:-}" && -z "${E2E_SNAPSHOT_MODULE:-}" ]]; then
  echo "--stories requires --module so unrelated modules are not executed" >&2
  usage >&2
  exit 2
fi

# The mode is only validated when it is given on the command line; a configuration file
# supplies it otherwise.
if [[ -n "${E2E_SNAPSHOT_MODE:-}" ]]; then
  case "${E2E_SNAPSHOT_MODE}" in
    capture|compare|report|full) ;;
    *)
      echo "Unsupported --mode: ${E2E_SNAPSHOT_MODE} (expected capture, compare, report or full)" >&2
      usage >&2
      exit 2
      ;;
  esac
fi

# Every snapshot test carries a Module trait, so --module selects exactly one suite and
# the modules never write into each other's output directory. Without it all suites run,
# each resolving its own module directory. An unknown name would match no test at all, so
# it is rejected instead of reporting a successful but empty run.
case "${E2E_SNAPSHOT_MODULE:-}" in
  ""|apilog|auth|billing|communication|integration|pricing) ;;
  *)
    echo "--module must be one of: apilog, auth, billing, communication, integration, pricing" >&2
    usage >&2
    exit 2
    ;;
esac

filter='Category=Snapshot'
if [[ -n "${E2E_SNAPSHOT_MODULE:-}" ]]; then
  filter="${filter}&Module=${E2E_SNAPSHOT_MODULE}"
fi

dotnet test tests/SaaSusSdk.Tests.csproj --configuration Release --filter "${filter}"
