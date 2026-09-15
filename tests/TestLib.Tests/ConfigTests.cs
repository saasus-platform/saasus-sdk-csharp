using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

public sealed class ConfigTests
{
    [Fact]
    public void FromEnvironmentLoadsCredentialsAndExecutionOptions()
    {
        var config = Config.FromEnvironment(new Dictionary<string, string?>
        {
            ["SAASUS_SAAS_ID"] = "saas",
            ["SAASUS_API_KEY"] = "api",
            ["SAASUS_SECRET_KEY"] = "secret",
            ["SAASUS_API_URL_BASE"] = "http://localhost:8080",
            ["E2E_TIMEOUT"] = "12",
            ["E2E_MAX_RETRIES"] = "4",
            ["E2E_DRY_RUN"] = "true",
            ["E2E_FAIL_FAST"] = "true"
        });

        Assert.True(config.IsValid);
        Assert.Equal("http://localhost:8080", config.BaseUrl);
        Assert.Equal(TimeSpan.FromSeconds(12), config.Timeout);
        Assert.Equal(4, config.MaxRetries);
        Assert.True(config.DryRun);
        Assert.True(config.FailFast);
    }

    [Fact]
    public void ValidateListsMissingCredentials()
    {
        var error = Assert.Throws<InvalidOperationException>(() => new Config().Validate());
        Assert.Contains("SAASUS_SAAS_ID", error.Message);
        Assert.Contains("SAASUS_API_KEY", error.Message);
        Assert.Contains("SAASUS_SECRET_KEY", error.Message);
    }

    [Fact]
    public void LoadsCompleteSnapshotConfiguration()
    {
        var config = Config.FromEnvironment(new Dictionary<string, string?>
        {
            ["E2E_SNAPSHOT_MODE"] = "full",
            ["E2E_SNAPSHOT_TAG"] = "v2",
            ["E2E_SNAPSHOT_OLD_TAG"] = "v1",
            ["E2E_SNAPSHOT_NEW_TAG"] = "v2",
            ["E2E_SNAPSHOT_COMPARISON_MODE"] = "manual",
            ["E2E_SNAPSHOT_CAPTURE_LEVEL"] = "response",
            ["E2E_SNAPSHOT_STORIES"] = "object,async",
            ["E2E_SNAPSHOT_OVERWRITE"] = "true"
        });
        Assert.NotNull(config.Snapshot);
        Assert.Equal(SnapshotMode.Full, config.Snapshot!.Mode);
        Assert.Equal(CaptureLevel.Response, config.Snapshot.CaptureLevel);
        Assert.Equal(new[] { "object", "async" }, config.Snapshot.StoryFilters);
        Assert.True(config.Snapshot.Overwrite);
    }

    [Fact]
    public void LoadsSnapshotJsonConfigurationWithEnvironmentOverride()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snapshot-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path, "{\"mode\":\"compare\",\"old_tag\":\"file-old\",\"new_tag\":\"file-new\",\"comparison_mode\":\"manual\"}");
            var config = Config.FromEnvironment(new Dictionary<string, string?>
            {
                ["E2E_SNAPSHOT_CONFIG"] = path,
                ["E2E_SNAPSHOT_NEW_TAG"] = "env-new"
            });
            Assert.Equal(SnapshotMode.Compare, config.Snapshot!.Mode);
            Assert.Equal("file-old", config.Snapshot.OldTag);
            Assert.Equal("env-new", config.Snapshot.NewTag);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void EnvFileLoaderAppliesValuesBeforeFactDiscovery()
    {
        var directory = Path.Combine(Path.GetTempPath(), $"saasus-env-{Guid.NewGuid():N}");
        var key = $"SAASUS_TEST_{Guid.NewGuid():N}";
        Directory.CreateDirectory(directory);
        try
        {
            File.WriteAllText(Path.Combine(directory, ".env"), $"{key}=true\n");

            EnvFileLoader.LoadAndApply(directory);

            Assert.Equal("true", Environment.GetEnvironmentVariable(key));
        }
        finally
        {
            Environment.SetEnvironmentVariable(key, null);
            Directory.Delete(directory, true);
        }
    }

    [Fact]
    public void SnapshotFactResolvesConfigFileAndLegacyMode()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snapshot-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path, "{\"mode\":\"report\",\"old_tag\":\"v1\",\"new_tag\":\"v2\",\"comparison_mode\":\"manual\"}");

            Assert.Null(SnapshotFactAttribute.ResolveSkipReason(new Dictionary<string, string?>
            {
                ["E2E_SNAPSHOT_CONFIG"] = path
            }));
            Assert.Null(SnapshotFactAttribute.ResolveSkipReason(new Dictionary<string, string?>
            {
                ["E2E_SNAPSHOT_REPORTING"] = "true"
            }));
            Assert.Null(SnapshotFactAttribute.ResolveSkipReason(new Dictionary<string, string?>
            {
                ["SNAPSHOT_MODE"] = "report"
            }));
            Assert.Equal(SnapshotMode.Report, Config.FromEnvironment(new Dictionary<string, string?>
            {
                ["SNAPSHOT_MODE"] = "report"
            }).Snapshot!.Mode);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void FalseSnapshotEnableFlagDoesNotActivateCapture()
    {
        var environment = new Dictionary<string, string?>
        {
            ["E2E_SNAPSHOT_ENABLE"] = "false",
            ["SAASUS_E2E"] = "true"
        };

        Assert.Null(Config.FromEnvironment(environment).Snapshot);
        Assert.NotNull(SnapshotFactAttribute.ResolveSkipReason(environment));
    }

    [Fact]
    public void SnapshotFactRejectsInvalidConfiguration()
    {
        var error = Assert.Throws<InvalidOperationException>(() =>
            SnapshotFactAttribute.ResolveSkipReason(new Dictionary<string, string?>
            {
                ["E2E_SNAPSHOT_MODE"] = "capture",
                ["E2E_SNAPSHOT_CAPTURE_LEVEL"] = "misspelled"
            }));

        Assert.Contains("Unsupported snapshot capture level", error.Message);
    }

    [Theory]
    [InlineData("E2E_SNAPSHOT_CAPTURE_LEVEL")]
    [InlineData("E2E_SNAPSHOT_COMPARISON_MODE")]
    [InlineData("E2E_SNAPSHOT_DYNAMIC_MODE")]
    public void RejectsUnsupportedSnapshotEnumSettings(string setting)
    {
        var error = Assert.Throws<InvalidOperationException>(() => Config.FromEnvironment(
            new Dictionary<string, string?>
            {
                ["E2E_SNAPSHOT_MODE"] = "capture",
                [setting] = "misspelled"
            }));

        Assert.Contains("Unsupported snapshot", error.Message);
        Assert.Contains("misspelled", error.Message);
    }

    [Fact]
    public void RejectsUnsupportedSnapshotMode()
    {
        var error = Assert.Throws<InvalidOperationException>(() => Config.FromEnvironment(
            new Dictionary<string, string?> { ["E2E_SNAPSHOT_MODE"] = "unsupported" }));

        Assert.Contains("Unsupported snapshot mode", error.Message);
    }

    [Fact]
    public void LoadsMetadataDeterminismAndHistorySettings()
    {
        var config = Config.FromEnvironment(new Dictionary<string, string?>
        {
            ["E2E_SNAPSHOT_MODE"] = "capture",
            ["SDK_VERSION"] = "1.2.3",
            ["E2E_TEST_ENVIRONMENT"] = "staging",
            ["E2E_SNAPSHOT_FILE_NAME_FORMAT"] = "snap_{tag}_{story_name}.json",
            ["E2E_SNAPSHOT_DYNAMIC_FIELDS"] = "request_id, correlation",
            ["E2E_SNAPSHOT_DEFAULT_DYNAMIC_FIELDS"] = "false",
            ["E2E_SNAPSHOT_HISTORY_LIMIT"] = "5",
            ["E2E_REPORT_JSON"] = "artifacts/report.json"
        }).Snapshot;

        Assert.NotNull(config);
        Assert.Equal("1.2.3", config!.SdkVersion);
        Assert.Equal("staging", config.TestEnvironment);
        Assert.Equal("snap_{tag}_{story_name}.json", config.FileNameFormat);
        Assert.Equal(new[] { "request_id", "correlation" }, config.DynamicFields);
        Assert.False(config.UseDefaultDynamicFields);
        Assert.Equal(5, config.ValidationHistoryLimit);
    }

    [Theory]
    [InlineData("TEST_ENVIRONMENT")]
    [InlineData("APP_ENV")]
    public void FallsBackToStandardEnvironmentNames(string variable)
    {
        var config = Config.FromEnvironment(new Dictionary<string, string?>
        {
            ["E2E_SNAPSHOT_MODE"] = "capture",
            [variable] = "ci"
        }).Snapshot;

        Assert.Equal("ci", config!.TestEnvironment);
    }

    [Fact]
    public void ReportJsonPathIsReadFromTheEnvironment()
    {
        var config = Config.FromEnvironment(new Dictionary<string, string?>
        {
            ["E2E_REPORT_JSON"] = "artifacts/report.json"
        });

        Assert.Equal("artifacts/report.json", config.ReportJsonPath);
    }

    [Fact]
    public void LoadsPerRuleValidationSeveritiesFromJsonConfiguration()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snapshot-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path, """
                {
                  "mode": "capture",
                  "validation_rules": {
                    "completion": { "enabled": true, "severity": "warning" },
                    "timing": { "enabled": true }
                  }
                }
                """);

            var config = Config.FromEnvironment(new Dictionary<string, string?>
            {
                ["E2E_SNAPSHOT_CONFIG"] = path
            }).Snapshot;

            Assert.Equal(new ValidationRule(true, "warning"), config!.Rule("completion"));
            // severity falls back to the shared default when only "enabled" is given
            Assert.Equal(new ValidationRule(true, "info"), config.Rule("timing"));
            // rules without an override keep the boolean-flag behaviour
            Assert.Equal(new ValidationRule(true, "error"), config.Rule("sequence"));
            Assert.Equal(new ValidationRule(true, "warning"), config.Rule("state_transition"));
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void EmptySnapshotEnvironmentValuesDoNotActivateSnapshotHandling()
    {
        var config = Config.FromEnvironment(new Dictionary<string, string?>
        {
            ["E2E_SNAPSHOT_MODE"] = string.Empty,
            ["E2E_SNAPSHOT_TAG"] = string.Empty
        });

        Assert.Null(config.Snapshot);
    }

    [Fact]
    public void EmptySnapshotEnvironmentValuesDoNotOverrideTheJsonConfiguration()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snapshot-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path, "{\"mode\":\"compare\",\"module_name\":\"auth\",\"old_tag\":\"v1\",\"new_tag\":\"v2\"}");

            var snapshot = Config.FromEnvironment(new Dictionary<string, string?>
            {
                ["E2E_SNAPSHOT_CONFIG"] = path,
                ["E2E_SNAPSHOT_MODE"] = string.Empty,
                ["E2E_SNAPSHOT_MODULE"] = string.Empty
            }).Snapshot;

            Assert.NotNull(snapshot);
            Assert.Equal(SnapshotMode.Compare, snapshot!.Mode);
            Assert.Equal("auth", snapshot.ModuleName);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void EnvironmentAliasesOverrideTheJsonConfiguration()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snapshot-{Guid.NewGuid():N}.json");
        var output = Path.Combine(Path.GetTempPath(), $"snapshot-out-{Guid.NewGuid():N}");
        try
        {
            File.WriteAllText(path,
                "{\"mode\":\"capture\",\"output_directory\":\"/from-file\",\"test_environment\":\"file-env\"}");

            var snapshot = Config.FromEnvironment(new Dictionary<string, string?>
            {
                ["E2E_SNAPSHOT_CONFIG"] = path,
                // The alias must win over the JSON value, like the reference implementations.
                ["E2E_SNAPSHOT_OUTPUT_DIR"] = output,
                ["APP_ENV"] = "staging"
            }).Snapshot;

            Assert.NotNull(snapshot);
            Assert.Equal(output, snapshot!.OutputDirectory);
            Assert.Equal("staging", snapshot.TestEnvironment);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void ParsesLogLevelAliasesAndRejectsUndefinedValues()
    {
        LogLevel Parse(string value) =>
            Config.FromEnvironment(new Dictionary<string, string?> { ["E2E_LOG_LEVEL"] = value }).LogLevel;

        Assert.Equal(LogLevel.Warning, Parse("warn"));
        Assert.Equal(LogLevel.Warning, Parse("warning"));
        Assert.Equal(LogLevel.Debug, Parse("debug"));
        // An undefined numeric value would otherwise filter out every message.
        Assert.Equal(LogLevel.Info, Parse("99"));
        Assert.Equal(LogLevel.Info, Parse("nonsense"));
    }

    [Fact]
    public void RejectsUnknownValidationRuleInJsonConfiguration()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snapshot-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path, "{\"mode\":\"capture\",\"validation_rules\":{\"nonsense\":{\"enabled\":true}}}");

            var error = Assert.Throws<InvalidOperationException>(() => Config.FromEnvironment(
                new Dictionary<string, string?> { ["E2E_SNAPSHOT_CONFIG"] = path }));

            Assert.Contains("Unsupported validation rule: nonsense", error.Message);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    [Fact]
    public void ExampleSnapshotConfigurationIsValid()
    {
        var path = ExampleConfigPath();
        Assert.True(File.Exists(path), path);

        var config = Config.FromEnvironment(new Dictionary<string, string?>
        {
            ["E2E_SNAPSHOT_CONFIG"] = path
        }).Snapshot;

        Assert.NotNull(config);
        Assert.Equal(SnapshotMode.Full, config!.Mode);
        Assert.Equal("billing", config.ModuleName);
        Assert.Equal(CaptureLevel.Full, config.CaptureLevel);
        Assert.Equal(SnapshotComparisonMode.Release, config.ComparisonMode);
        Assert.Equal(2, config.ValidationHistoryLimit);
        Assert.Equal(new[] { "request_id" }, config.DynamicFields);
        Assert.Equal(new ValidationRule(false, "info"), config.Rule("timing"));
        Assert.Equal(new ValidationRule(true, "warning"), config.Rule("state_transition"));
    }

    private static string ExampleConfigPath()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "saasus-sdk-csharp.sln")))
                return Path.Combine(directory.FullName, "tests", "E2E", "snapshot-config.example.json");
        }
        throw new InvalidOperationException("Unable to locate the repository root.");
    }

    [Fact]
    public async Task SnapshotScriptOnlyExportsKnownVariables()
    {
        var script = await File.ReadAllTextAsync(RepositoryFile("tests", "snapshot.sh"));
        var exported = System.Text.RegularExpressions.Regex
            .Matches(script, @"export\s+([A-Z0-9_]+)=")
            .Select(match => match.Groups[1].Value)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(exported);
        var unknown = exported.Except(Config.KnownEnvironmentVariables, StringComparer.Ordinal).ToArray();
        Assert.Empty(unknown);
    }

    [Fact]
    public async Task SnapshotScriptDocumentsEveryFlagItAccepts()
    {
        var script = await File.ReadAllTextAsync(RepositoryFile("tests", "snapshot.sh"));
        var help = script[script.IndexOf("<<'EOF'", StringComparison.Ordinal)..script.IndexOf("\nEOF", StringComparison.Ordinal)];
        var flags = System.Text.RegularExpressions.Regex
            .Matches(script, @"^\s{4}(--[a-z-]+)\)", System.Text.RegularExpressions.RegexOptions.Multiline)
            .Select(match => match.Groups[1].Value)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(flags);
        var undocumented = flags.Where(flag => !help.Contains(flag, StringComparison.Ordinal)).ToArray();
        Assert.Empty(undocumented);
    }

    [Fact]
    public void BlankEnvironmentValuesDoNotOverrideTheConfigurationFile()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snapshot-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path,
                "{\"mode\":\"compare\",\"module_name\":\"pricing\",\"old_tag\":\"v1\",\"new_tag\":\"v2\",\"comparison_mode\":\"manual\"}");

            // `export E2E_SNAPSHOT_MODE=` must not silence the configured mode.
            var config = Config.FromEnvironment(new Dictionary<string, string?>
            {
                ["E2E_SNAPSHOT_CONFIG"] = path,
                ["E2E_SNAPSHOT_MODE"] = string.Empty,
                ["E2E_SNAPSHOT_MODULE"] = "   "
            }).Snapshot;

            Assert.Equal(SnapshotMode.Compare, config!.Mode);
            Assert.Equal("pricing", config.ModuleName);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    /// <summary>
    /// A configuration file is shared by every module, so a suite routes itself with
    /// <see cref="SnapshotConfig.WithModule"/> instead of inheriting the file's module.
    /// </summary>
    [Fact]
    public void SnapshotConfigurationIsRoutedByTheSuiteNotTheSharedFile()
    {
        var path = Path.Combine(Path.GetTempPath(), $"snapshot-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(path,
                "{\"mode\":\"capture\",\"module_name\":\"billing\",\"stories\":[\"Billing API - synchronous responses\"]}");
            var environment = new Dictionary<string, string?> { ["E2E_SNAPSHOT_CONFIG"] = path };

            var config = Config.FromEnvironment(environment).Snapshot;
            Assert.Equal("billing", config!.ModuleName);

            // Each suite overrides the module for its own artifacts and keeps everything else.
            var pricing = config.WithModule("pricing");
            Assert.Equal("pricing", pricing.ModuleName);
            Assert.Equal(config.Mode, pricing.Mode);
            Assert.Equal(config.StoryFilters, pricing.StoryFilters);
            Assert.EndsWith(Path.Combine("Snapshots", "pricing"), pricing.ModuleDirectory, StringComparison.Ordinal);
        }
        finally { if (File.Exists(path)) File.Delete(path); }
    }

    /// <summary>
    /// <c>tests/snapshot.sh --module NAME</c> filters on the <c>Module</c> trait, so a
    /// snapshot suite without that trait would be skipped silently.
    /// </summary>
    [Fact]
    public void EverySnapshotSuiteDeclaresItsModuleTrait()
    {
        var snapshotTests = typeof(ConfigTests).Assembly.GetTypes()
            .SelectMany(type => type.GetMethods())
            .Where(method => method.GetCustomAttributes(typeof(SnapshotFactAttribute), false).Length > 0)
            .ToArray();

        Assert.NotEmpty(snapshotTests);
        var modules = new List<string>();
        foreach (var method in snapshotTests)
        {
            var traits = method.GetCustomAttributesData()
                .Where(attribute => attribute.AttributeType == typeof(TraitAttribute))
                .Select(attribute => (
                    Name: attribute.ConstructorArguments[0].Value as string,
                    Value: attribute.ConstructorArguments[1].Value as string))
                .ToArray();

            var name = $"{method.DeclaringType?.Name}.{method.Name}";
            Assert.Contains(("Category", "Snapshot"), traits);
            var module = traits.SingleOrDefault(trait => trait.Name == "Module").Value;
            Assert.False(string.IsNullOrWhiteSpace(module), $"{name} does not declare a Module trait.");
            modules.Add(module!);
        }

        Assert.Equal(modules.Distinct(StringComparer.Ordinal).Count(), modules.Count);
    }

    /// <summary>
    /// <c>tests/snapshot.sh</c> rejects unknown module names, so its list must stay in sync
    /// with the modules the suites declare.
    /// </summary>
    [Fact]
    public async Task SnapshotScriptAcceptsExactlyTheDeclaredModules()
    {
        var script = await File.ReadAllTextAsync(RepositoryFile("tests", "snapshot.sh"));
        var accepted = System.Text.RegularExpressions.Regex
            .Match(script, @"^\s*""""\|(?<modules>[a-z|]+)\)", System.Text.RegularExpressions.RegexOptions.Multiline)
            .Groups["modules"].Value
            .Split('|', StringSplitOptions.RemoveEmptyEntries)
            .OrderBy(module => module, StringComparer.Ordinal)
            .ToArray();
        var declared = typeof(ConfigTests).Assembly.GetTypes()
            .SelectMany(type => type.GetMethods())
            .Where(method => method.GetCustomAttributes(typeof(SnapshotFactAttribute), false).Length > 0)
            .SelectMany(method => method.GetCustomAttributesData())
            .Where(attribute => attribute.AttributeType == typeof(TraitAttribute) &&
                                (attribute.ConstructorArguments[0].Value as string) == "Module")
            .Select(attribute => (string)attribute.ConstructorArguments[1].Value!)
            .OrderBy(module => module, StringComparer.Ordinal)
            .ToArray();

        Assert.NotEmpty(accepted);
        Assert.Equal(declared, accepted);
    }

    private static string RepositoryFile(params string[] parts)
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
        {
            if (File.Exists(Path.Combine(directory.FullName, "saasus-sdk-csharp.sln")))
                return Path.Combine(new[] { directory.FullName }.Concat(parts).ToArray());
        }
        throw new InvalidOperationException("Unable to locate the repository root.");
    }
}
