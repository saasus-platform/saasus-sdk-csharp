namespace SaasusSdk.Tests.TestLib;

public sealed class Config
{
    public string SaasId { get; init; } = string.Empty;
    public string ApiKey { get; init; } = string.Empty;
    public string SecretKey { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = string.Empty;
    public string StripeSecretKey { get; init; } = string.Empty;
    public string CognitoUserPoolId { get; init; } = string.Empty;
    public string CognitoClientId { get; init; } = string.Empty;
    public string CognitoUsername { get; init; } = string.Empty;
    public string CognitoPassword { get; init; } = string.Empty;
    public string CognitoRegion { get; init; } = "ap-northeast-1";
    public string CognitoEndpoint { get; init; } = string.Empty;
    public string AwsAccessKeyId { get; init; } = string.Empty;
    public string AwsSecretAccessKey { get; init; } = string.Empty;
    public string AwsSessionToken { get; init; } = string.Empty;
    public string AuthDefaultPassword { get; init; } = "Passw0rd!";
    public string AuthUpdatedPassword { get; init; } = "UpdatedPassw0rd!";
    public string AuthEmailConfirmationCode { get; init; } = string.Empty;
    public string AuthExternalProviderName { get; init; } = string.Empty;
    public string AuthExternalProviderAccessToken { get; init; } = string.Empty;
    public string AuthExternalProviderCode { get; init; } = string.Empty;
    public bool AuthSignUpEnabled { get; init; }
    public LogLevel LogLevel { get; init; } = LogLevel.Info;
    public bool DryRun { get; init; }
    public bool FailFast { get; init; }
    public TimeSpan Timeout { get; init; } = TimeSpan.FromSeconds(300);
    public int MaxRetries { get; init; } = 3;

    /// <summary>Optional path for the machine-readable run report (<c>E2E_REPORT_JSON</c>).</summary>
    public string ReportJsonPath { get; init; } = string.Empty;

    public SnapshotConfig? Snapshot { get; init; }

    public static Config FromEnvironment(IReadOnlyDictionary<string, string?>? environment = null)
    {
        if (environment is null) EnvFileLoader.LoadAndApply();
        string? Get(string key) => environment is null
            ? Environment.GetEnvironmentVariable(key)
            : environment.TryGetValue(key, out var value) ? value : null;

        var snapshotEnabled = HasAny(Get,
            "E2E_SNAPSHOT_MODE",
            "SNAPSHOT_MODE",
            "E2E_SNAPSHOT_CONFIG",
            "E2E_SNAPSHOT_TAG",
            "E2E_SNAPSHOT_OLD_TAG",
            "E2E_SNAPSHOT_NEW_TAG",
            // The parser and tests/snapshot.sh both accept these, so they must activate snapshots
            // too or a direct invocation silently skips every snapshot test.
            "E2E_SNAPSHOT_OUTPUT",
            "E2E_SNAPSHOT_OUTPUT_DIR",
            "E2E_SNAPSHOT_CAPTURE_LEVEL") ||
            ParseBoolean(Get("E2E_SNAPSHOT_ENABLE")) ||
            ParseBoolean(Get("E2E_SNAPSHOT_COMPARISON")) ||
            ParseBoolean(Get("E2E_SNAPSHOT_REPORTING"));

        return new Config
        {
            SaasId = Get("SAASUS_SAAS_ID") ?? string.Empty,
            ApiKey = Get("SAASUS_API_KEY") ?? string.Empty,
            SecretKey = Get("SAASUS_SECRET_KEY") ?? string.Empty,
            BaseUrl = Get("SAASUS_API_URL_BASE") ?? string.Empty,
            StripeSecretKey = Get("STRIPE_SECRET_KEY") ?? string.Empty,
            CognitoUserPoolId = Get("E2E_COGNITO_USER_POOL_ID") ?? string.Empty,
            CognitoClientId = Get("E2E_COGNITO_CLIENT_ID") ?? string.Empty,
            CognitoUsername = Get("E2E_COGNITO_USERNAME") ?? string.Empty,
            CognitoPassword = Get("E2E_COGNITO_PASSWORD") ?? string.Empty,
            CognitoRegion = Get("E2E_COGNITO_REGION") ?? "ap-northeast-1",
            CognitoEndpoint = Get("E2E_COGNITO_ENDPOINT") ?? string.Empty,
            AwsAccessKeyId = Get("AWS_ACCESS_KEY_ID") ?? string.Empty,
            AwsSecretAccessKey = Get("AWS_SECRET_ACCESS_KEY") ?? string.Empty,
            AwsSessionToken = Get("AWS_SESSION_TOKEN") ?? string.Empty,
            AuthDefaultPassword = Get("AUTH_E2E_DEFAULT_PASSWORD") ?? "Passw0rd!",
            AuthUpdatedPassword = Get("AUTH_E2E_UPDATED_PASSWORD") ?? "UpdatedPassw0rd!",
            AuthEmailConfirmationCode = Get("AUTH_E2E_EMAIL_CONFIRMATION_CODE") ?? string.Empty,
            AuthExternalProviderName = Get("AUTH_E2E_EXTERNAL_PROVIDER_NAME") ?? string.Empty,
            AuthExternalProviderAccessToken = Get("AUTH_E2E_EXTERNAL_PROVIDER_ACCESS_TOKEN") ?? string.Empty,
            AuthExternalProviderCode = Get("AUTH_E2E_EXTERNAL_PROVIDER_CODE") ?? string.Empty,
            AuthSignUpEnabled = IsSignUpEnabled(Get("AUTH_E2E_SKIP_SIGNUP_ON_COGNITO_EMAIL_LIMIT")),
            LogLevel = ParseLogLevel(Get("E2E_LOG_LEVEL") ?? Get("LOG_LEVEL")),
            DryRun = ParseBoolean(Get("E2E_DRY_RUN") ?? Get("DRY_RUN")),
            FailFast = ParseBoolean(Get("E2E_FAIL_FAST") ?? Get("FAIL_FAST")),
            Timeout = TimeSpan.FromSeconds(ParsePositiveInt(Get("E2E_TIMEOUT"), 300)),
            MaxRetries = ParseNonNegativeInt(Get("E2E_MAX_RETRIES"), 3),
            ReportJsonPath = Get("E2E_REPORT_JSON") ?? string.Empty,
            Snapshot = snapshotEnabled
                ? SnapshotFromEnvironment(Get)
                : null
        };
    }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(SaasId) &&
        !string.IsNullOrWhiteSpace(ApiKey) &&
        !string.IsNullOrWhiteSpace(SecretKey);

    /// <summary>
    /// Every environment variable this class understands. Kept in sync with
    /// <c>tests/snapshot.sh</c> by <c>ConfigTests.SnapshotScriptOnlyExportsKnownVariables</c>.
    /// </summary>
    public static IReadOnlyList<string> KnownEnvironmentVariables { get; } = new[]
    {
        // credentials and endpoint
        "SAASUS_SAAS_ID", "SAASUS_API_KEY", "SAASUS_SECRET_KEY", "SAASUS_API_URL_BASE",
        "STRIPE_SECRET_KEY", "SAASUS_E2E",
        // Auth external dependencies
        "E2E_COGNITO_USER_POOL_ID", "E2E_COGNITO_CLIENT_ID", "E2E_COGNITO_USERNAME",
        "E2E_COGNITO_PASSWORD", "E2E_COGNITO_REGION", "E2E_COGNITO_ENDPOINT",
        "AWS_ACCESS_KEY_ID", "AWS_SECRET_ACCESS_KEY", "AWS_SESSION_TOKEN",
        "AUTH_E2E_DEFAULT_PASSWORD", "AUTH_E2E_UPDATED_PASSWORD",
        "AUTH_E2E_EMAIL_CONFIRMATION_CODE", "AUTH_E2E_EXTERNAL_PROVIDER_NAME",
        "AUTH_E2E_EXTERNAL_PROVIDER_ACCESS_TOKEN", "AUTH_E2E_EXTERNAL_PROVIDER_CODE",
        "AUTH_E2E_SKIP_SIGNUP_ON_COGNITO_EMAIL_LIMIT",
        // execution
        "E2E_LOG_LEVEL", "LOG_LEVEL", "E2E_DRY_RUN", "DRY_RUN", "E2E_FAIL_FAST", "FAIL_FAST",
        "E2E_TIMEOUT", "E2E_MAX_RETRIES", "E2E_REPORT_JSON",
        // snapshot activation
        "E2E_SNAPSHOT_MODE", "SNAPSHOT_MODE", "E2E_SNAPSHOT_CONFIG",
        "E2E_SNAPSHOT_ENABLE", "E2E_SNAPSHOT_COMPARISON", "E2E_SNAPSHOT_REPORTING",
        // snapshot settings
        "E2E_SNAPSHOT_OUTPUT", "E2E_SNAPSHOT_OUTPUT_DIR", "E2E_SNAPSHOT_MODULE",
        "E2E_SNAPSHOT_FILE_NAME_FORMAT", "E2E_SNAPSHOT_TAG", "E2E_SNAPSHOT_OLD_TAG",
        "E2E_SNAPSHOT_NEW_TAG", "E2E_SNAPSHOT_CAPTURE_LEVEL", "E2E_SNAPSHOT_COMPARISON_MODE",
        "E2E_SNAPSHOT_STORIES", "E2E_SNAPSHOT_VERBOSE", "E2E_SNAPSHOT_FAIL_ON_BREAKING",
        "E2E_SNAPSHOT_OVERWRITE", "E2E_SNAPSHOT_CAPTURE_FAILED",
        // validation
        "E2E_SNAPSHOT_VALIDATION", "E2E_SNAPSHOT_VALIDATE_COMPLETION",
        "E2E_SNAPSHOT_VALIDATE_SEQUENCE", "E2E_SNAPSHOT_VALIDATE_STATE",
        "E2E_SNAPSHOT_VALIDATE_TIMING", "E2E_SNAPSHOT_HISTORY_LIMIT",
        // determinism and metadata
        "E2E_SNAPSHOT_DYNAMIC_FIELDS", "E2E_SNAPSHOT_DYNAMIC_MODE",
        "E2E_SNAPSHOT_DEFAULT_DYNAMIC_FIELDS", "SDK_VERSION",
        "E2E_TEST_ENVIRONMENT", "TEST_ENVIRONMENT", "APP_ENV"
    };

    public void Validate()
    {
        var missing = new List<string>();
        if (string.IsNullOrWhiteSpace(SaasId)) missing.Add("SAASUS_SAAS_ID");
        if (string.IsNullOrWhiteSpace(ApiKey)) missing.Add("SAASUS_API_KEY");
        if (string.IsNullOrWhiteSpace(SecretKey)) missing.Add("SAASUS_SECRET_KEY");
        if (missing.Count > 0)
        {
            throw new InvalidOperationException(
                $"Missing required environment variables: {string.Join(", ", missing)}");
        }
    }

    /// <summary>
    /// True when at least one key holds a non-empty value. An empty value (for example
    /// <c>E2E_SNAPSHOT_MODE=</c> exported by CI) carries no configuration and must not
    /// activate snapshot handling.
    /// </summary>
    private static bool HasAny(Func<string, string?> get, params string[] keys) =>
        keys.Any(key => !string.IsNullOrWhiteSpace(get(key)));

    private static bool ParseBoolean(string? value, bool fallback = false) =>
        bool.TryParse(value, out var result) ? result : fallback;

    private static bool IsSignUpEnabled(string? value)
    {
        var normalized = value?.Trim().ToLowerInvariant();
        return normalized is "0" or "false" or "no" or "off";
    }

    private static int ParsePositiveInt(string? value, int fallback) =>
        int.TryParse(value, out var result) && result > 0 ? result : fallback;

    private static int ParseNonNegativeInt(string? value, int fallback) =>
        int.TryParse(value, out var result) && result >= 0 ? result : fallback;

    /// <summary>
    /// Parses a log level by name. <c>Enum.TryParse</c> accepts any number, so an undefined value
    /// such as <c>99</c> would filter out every message; <c>warn</c> is accepted as an alias like
    /// the Go and PHP parsers do.
    /// </summary>
    private static LogLevel ParseLogLevel(string? value)
    {
        var normalized = value?.Trim();
        if (string.IsNullOrEmpty(normalized)) return LogLevel.Info;
        if (string.Equals(normalized, "warn", StringComparison.OrdinalIgnoreCase)) return LogLevel.Warning;
        return Enum.TryParse<LogLevel>(normalized, true, out var level) && Enum.IsDefined(level)
            ? level
            : LogLevel.Info;
    }

    private static T ParseEnum<T>(string? value, T fallback, string name) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(value)) return fallback;
        if (Enum.TryParse<T>(value, true, out var result) && Enum.IsDefined(result)) return result;
        throw new InvalidOperationException($"Unsupported {name}: {value}.");
    }

    private static SnapshotConfig SnapshotFromEnvironment(Func<string, string?> get)
    {
        Newtonsoft.Json.Linq.JObject? file = null;
        var configPath = get("E2E_SNAPSHOT_CONFIG");
        if (!string.IsNullOrWhiteSpace(configPath))
        {
            if (!File.Exists(configPath)) throw new FileNotFoundException("Snapshot config file not found.", configPath);
            file = Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(configPath));
        }
        string? Value(string environmentName, string jsonName)
        {
            // An empty environment value is not an override: the JSON configuration still wins.
            if (get(environmentName) is { } environmentValue && !string.IsNullOrWhiteSpace(environmentValue))
                return environmentValue;
            var token = file?[jsonName];
            return token is Newtonsoft.Json.Linq.JArray array
                ? string.Join(",", array.Values<string>())
                : token?.ToString();
        }
        // Every alias of a setting must be resolved before the JSON configuration, otherwise a
        // configured run silently ignores the alternative environment variable.
        string? EnvironmentValue(params string[] names) =>
            names.Select(get).FirstOrDefault(value => !string.IsNullOrWhiteSpace(value));

        var rawMode = EnvironmentValue("E2E_SNAPSHOT_MODE", "SNAPSHOT_MODE") ?? Value("E2E_SNAPSHOT_MODE", "mode");
        SnapshotMode mode;
        if (!string.IsNullOrWhiteSpace(rawMode))
        {
            if (!Enum.TryParse(rawMode, true, out mode) || !Enum.IsDefined(mode))
                throw new InvalidOperationException($"Unsupported snapshot mode: {rawMode}.");
        }
        else
        {
            var capture = ParseBoolean(get("E2E_SNAPSHOT_ENABLE"));
            var compare = ParseBoolean(get("E2E_SNAPSHOT_COMPARISON"));
            var report = ParseBoolean(get("E2E_SNAPSHOT_REPORTING"));
            mode = capture && (compare || report) ? SnapshotMode.Full : compare ? SnapshotMode.Compare : report ? SnapshotMode.Report : SnapshotMode.Capture;
        }
        var filters = (Value("E2E_SNAPSHOT_STORIES", "stories") ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var dynamicFields = (Value("E2E_SNAPSHOT_DYNAMIC_FIELDS", "dynamic_fields") ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var config = new SnapshotConfig
        {
            Mode = mode,
            OutputDirectory = EnvironmentValue("E2E_SNAPSHOT_OUTPUT", "E2E_SNAPSHOT_OUTPUT_DIR")
                ?? Value("E2E_SNAPSHOT_OUTPUT", "output_directory") ?? ResolveDefaultSnapshotOutput(),
            ModuleName = Value("E2E_SNAPSHOT_MODULE", "module_name") ?? "billing",
            FileNameFormat = Value("E2E_SNAPSHOT_FILE_NAME_FORMAT", "file_name_format") ?? "story_snapshot_{tag}_{story_name}.json",
            CurrentTag = Value("E2E_SNAPSHOT_TAG", "current_tag") ?? string.Empty,
            OldTag = Value("E2E_SNAPSHOT_OLD_TAG", "old_tag") ?? string.Empty,
            NewTag = Value("E2E_SNAPSHOT_NEW_TAG", "new_tag") ?? string.Empty,
            SdkVersion = Value("SDK_VERSION", "sdk_version") ?? "unknown",
            TestEnvironment = EnvironmentValue("E2E_TEST_ENVIRONMENT", "TEST_ENVIRONMENT", "APP_ENV")
                ?? Value("E2E_TEST_ENVIRONMENT", "test_environment") ?? "dev",
            CaptureLevel = ParseEnum(Value("E2E_SNAPSHOT_CAPTURE_LEVEL", "capture_level"), CaptureLevel.Full, "snapshot capture level"),
            ComparisonMode = ParseEnum(Value("E2E_SNAPSHOT_COMPARISON_MODE", "comparison_mode"), SnapshotComparisonMode.Release, "snapshot comparison mode"),
            StoryFilters = filters,
            Verbose = ParseBoolean(Value("E2E_SNAPSHOT_VERBOSE", "verbose")),
            FailOnBreaking = ParseBoolean(Value("E2E_SNAPSHOT_FAIL_ON_BREAKING", "fail_on_breaking"), true),
            ValidationEnabled = ParseBoolean(Value("E2E_SNAPSHOT_VALIDATION", "validation_enabled"), true),
            ValidateCompletion = ParseBoolean(Value("E2E_SNAPSHOT_VALIDATE_COMPLETION", "validate_completion"), true),
            ValidateSequence = ParseBoolean(Value("E2E_SNAPSHOT_VALIDATE_SEQUENCE", "validate_sequence"), true),
            ValidateStateTransitions = ParseBoolean(Value("E2E_SNAPSHOT_VALIDATE_STATE", "validate_state_transitions"), true),
            ValidateTiming = ParseBoolean(Value("E2E_SNAPSHOT_VALIDATE_TIMING", "validate_timing")),
            ValidationRuleOverrides = ParseValidationRules(file),
            ValidationHistoryLimit = ParsePositiveInt(Value("E2E_SNAPSHOT_HISTORY_LIMIT", "validation_history_limit"), 2),
            CaptureFailedStories = ParseBoolean(Value("E2E_SNAPSHOT_CAPTURE_FAILED", "capture_failed_stories")),
            Overwrite = ParseBoolean(Value("E2E_SNAPSHOT_OVERWRITE", "overwrite")),
            DynamicFields = dynamicFields,
            UseDefaultDynamicFields = ParseBoolean(Value("E2E_SNAPSHOT_DEFAULT_DYNAMIC_FIELDS", "default_dynamic_fields"), true),
            DynamicFieldMode = ParseEnum(Value("E2E_SNAPSHOT_DYNAMIC_MODE", "dynamic_mode"), DynamicFieldMode.Replace, "snapshot dynamic field mode")
        };
        config.Validate();
        return config;
    }

    /// <summary>
    /// Reads the <c>validation_rules</c> block, which allows per-rule enable flags and
    /// severities, matching Go's and PHP's JSON snapshot configuration.
    /// </summary>
    private static IReadOnlyDictionary<string, ValidationRule> ParseValidationRules(Newtonsoft.Json.Linq.JObject? file)
    {
        var overrides = new Dictionary<string, ValidationRule>(StringComparer.OrdinalIgnoreCase);
        if (file?["validation_rules"] is not Newtonsoft.Json.Linq.JObject rules) return overrides;
        foreach (var property in rules.Properties())
        {
            if (property.Value is not Newtonsoft.Json.Linq.JObject rule)
                throw new InvalidOperationException($"Validation rule '{property.Name}' must be an object.");
            var fallback = SnapshotConfig.DefaultValidationRules.TryGetValue(property.Name, out var known)
                ? known
                : new ValidationRule(true, SnapshotConfig.SeverityError);
            overrides[property.Name] = new ValidationRule(
                rule["enabled"] is { } enabled ? enabled.ToObject<bool>() : fallback.Enabled,
                rule["severity"] is { } severity ? severity.ToObject<string>() ?? fallback.Severity : fallback.Severity);
        }
        return overrides;
    }

    private static string ResolveDefaultSnapshotOutput()
    {
        foreach (var start in new[] { Directory.GetCurrentDirectory(), AppContext.BaseDirectory })
        {
            for (var directory = new DirectoryInfo(start); directory is not null; directory = directory.Parent)
            {
                if (File.Exists(Path.Combine(directory.FullName, "saasus-sdk-csharp.sln")))
                    return Path.Combine(directory.FullName, "tests", "E2E", "Snapshots");
            }
        }

        return Path.GetFullPath(Path.Combine("tests", "E2E", "Snapshots"));
    }
}
