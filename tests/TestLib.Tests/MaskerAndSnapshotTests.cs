using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.TestLib.Tests;

public sealed class MaskerAndSnapshotTests
{
    [Fact]
    public void MasksNestedSecretsWithLengthPreservingToken()
    {
        var masked = JObject.FromObject(new Masker().Mask(new
        {
            api_key = "visible-secret",
            nested = new { authorization = "Bearer abc.def.ghi", name = "stable" }
        })!);

        Assert.Equal("[MASKED len=14]", masked["api_key"]);
        Assert.Equal("[MASKED len=18]", masked["nested"]!["authorization"]);
        Assert.Equal("stable", masked["nested"]!["name"]);
    }

    [Fact]
    public void MasksBearerTokensAndStripeKeysInFreeText()
    {
        var masker = new Masker();

        Assert.Equal("header: Bearer [MASKED]", masker.MaskText("header: Bearer abc.def-ghi"));
        Assert.Equal("key=[MASKED]", masker.MaskText("key=sk_test_51AbCdEf"));
        Assert.Equal("[MASKED]", Masker.MaskValue(null));
        Assert.Equal(string.Empty, Masker.MaskValue(string.Empty));
    }

    [Fact]
    public void ReplacesDynamicFieldsWithTypePreservingTokens()
    {
        var masked = JObject.FromObject(new SnapshotMasker().Process(new
        {
            id = "random",
            tiered_pricing_unit_id = "random-tiered-unit",
            created_at = "now",
            tenantId = "camel-case-twin",
            ttl = 42,
            name = "stable",
            blank = string.Empty
        })!);

        Assert.Equal("[DYNAMIC]", masked["id"]);
        Assert.Equal("[DYNAMIC]", masked["tiered_pricing_unit_id"]);
        Assert.Equal("[DYNAMIC]", masked["created_at"]);
        Assert.Equal("[DYNAMIC]", masked["tenantId"]);
        Assert.Equal(0, masked["ttl"]);
        Assert.Equal("stable", masked["name"]);
        Assert.Equal(string.Empty, masked["blank"]);
    }

    [Fact]
    public void NormalisesRawPayloadsIncludingScalarRoots()
    {
        var masker = new SnapshotMasker();

        // Generated oneOf models serialise themselves with WriteRawValue, so their payload
        // arrives as an opaque raw token that may hold an object, a scalar or invalid JSON.
        var masked = (JObject)masker.ProcessTokenCopy(new JObject
        {
            ["object_body"] = new JRaw("{\"name\":\"csharp_e2e_unit\",\"unit_amount\":1000}"),
            ["scalar_body"] = new JRaw("\"csharp_e2e_unit\""),
            ["number_body"] = new JRaw("42"),
            ["invalid_body"] = new JRaw("not json")
        })!;

        Assert.Equal(SnapshotMasker.DynamicToken, masked["object_body"]!["name"]!.Value<string>());
        Assert.Equal(1000, masked["object_body"]!["unit_amount"]!.Value<int>());
        Assert.Equal(SnapshotMasker.DynamicToken, masked["scalar_body"]!.Value<string>());
        Assert.Equal(42, masked["number_body"]!.Value<int>());
        Assert.Equal("not json", masked["invalid_body"]!.Value<string>());

        // The same payload can be the whole captured value.
        Assert.Equal(SnapshotMasker.DynamicToken,
            masker.ProcessTokenCopy(new JRaw("\"csharp_e2e_unit\""))!.Value<string>());
    }

    [Fact]
    public void NormalisesRecordedApiLogTrafficValues()
    {
        var masked = JObject.FromObject(new SnapshotMasker().Process(new
        {
            api_logs = new[]
            {
                new
                {
                    request_uri = "/v1/apilog/logs/2f47c6a1-55a6-40e5-bb85-2a705caa9fdd",
                    request_method = "GET",
                    response_status = "200",
                    response_body = "{\"api_logs\":[]}",
                    remote_address = "10.88.2.209:46278",
                    referer = string.Empty
                }
            }
        })!);

        // The field set stays intact so a model change is still detected, while the logged
        // values - which depend on whichever request was recorded - are normalised. Properties
        // are canonicalised into ordinal order.
        var log = (JObject)masked["api_logs"]![0]!;
        Assert.Equal(
            new[] { "referer", "remote_address", "request_method", "request_uri", "response_body", "response_status" },
            log.Properties().Select(property => property.Name));
        Assert.All(
            new[] { "request_uri", "request_method", "response_status", "response_body", "remote_address" },
            field => Assert.Equal(SnapshotMasker.DynamicToken, log[field]!.Value<string>()));
        Assert.Equal(string.Empty, log["referer"]!.Value<string>());
    }

    [Fact]
    public void NormalisesLoggedRequestUrisWithoutLosingTheRoute()
    {
        // With the recorded-traffic defaults disabled, a URI is normalised structurally:
        // the route and the query parameter names survive for comparison.
        var masked = JObject.FromObject(new SnapshotMasker(
            new[] { "created_date", "cursor" }, useDefaults: false).Process(new
        {
            request_uri = "/v1/apilog/logs/2f47c6a1-55a6-40e5-bb85-2a705caa9fdd" +
                          "?created_date=2024-01-01&cursor=abc123&limit=1" +
                          "&token=sk_test_51AbCdEf&api_key=plain-secret"
        })!);

        Assert.Equal(
            "/v1/apilog/logs/[DYNAMIC]?created_date=[DYNAMIC]&cursor=[DYNAMIC]&limit=1" +
            "&token=[MASKED len=16]&api_key=[MASKED len=12]",
            masked["request_uri"]!.Value<string>());
    }

    [Fact]
    public void DefaultDynamicFieldsCanBeDisabled()
    {
        var masked = JObject.FromObject(new SnapshotMasker(useDefaults: false).Process(new { id = "kept" })!);
        Assert.Equal("kept", masked["id"]);
    }

    [Fact]
    public void PreservesTimestampLookingPayloadStrings()
    {
        const string value = "2026-08-05T00:28:34.340164+09:00";
        var json = SnapshotJson.Serialize(new { stable_marker = value });

        // Converting the string to a date would change its offset on re-serialization and hide a
        // string-format change from the comparer.
        Assert.Contains(value, json, StringComparison.Ordinal);
        Assert.Empty(new SnapshotComparer().Compare(
            JObject.Parse(json), JObject.Parse(SnapshotJson.Serialize(new { stable_marker = value }))).Issues);
    }

    [Fact]
    public void MaskLengthsAndMultiValueHeadersMatchTheReference()
    {
        // Go's len() and PHP's strlen() both count bytes.
        Assert.Equal("[MASKED len=9]", Masker.MaskValue("日本語"));

        var headers = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["Set-Cookie"] = new[] { "a=1", "b=2" }
        };
        var result = new StoryResult("story", TestStatus.Passed, TimeSpan.Zero, new[]
        {
            new StepResult("get", "Get", CallStyle.WithHttpInfo, TestStatus.Passed,
                TimeSpan.Zero, 200, new { ok = true }, headers)
        });

        var snapshot = SnapshotFactory.Create(new Story { Name = "story" }, result, new SnapshotConfig());

        // PHP records $values[0]; joining them would produce a spurious difference. Set-Cookie is a
        // sensitive key, so the first value is length-masked.
        Assert.Equal(Masker.MaskValue("a=1"), snapshot.Steps[0].ReturnValue!.Headers["Set-Cookie"]);
    }

    [Fact]
    public void MaskerHandlesRootScalarsAndUnquotedJsonValues()
    {
        var masker = new Masker();

        // A root string has no parent token, so masking must not throw.
        Assert.Equal("Bearer [MASKED]", masker.Mask("Bearer abc.def-ghi"));
        // An unquoted JSON value still has to be redacted.
        Assert.DoesNotContain("123456", masker.MaskText("{\"mfa_code\":123456}"), StringComparison.Ordinal);
    }

    [Fact]
    public void NormalizesGeneratedAttributeNamesAsValuesAndKeys()
    {
        var masked = (JObject)new SnapshotMasker().ProcessTokenCopy(JObject.Parse(
            "{\"attributes\":{\"csharp_saas_auth_e2e_1deea5226e9a\":\"csharp_tenant_auth_e2e_1deea5226e9a\"}}"))!;

        // A new random suffix on every live run would otherwise be a breaking change.
        var attributes = (JObject)masked["attributes"]!;
        Assert.Equal(new[] { "[DYNAMIC]" }, attributes.Properties().Select(property => property.Name).ToArray());
        Assert.Equal("[DYNAMIC]", attributes["[DYNAMIC]"]);
    }

    [Fact]
    public void ClassifiesAStepSuccessRegressionAsBreaking()
    {
        var regression = new SnapshotComparer().Compare(
            new { steps = new[] { new { success = true } } },
            new { steps = new[] { new { success = false } } });
        Assert.Equal(CompatibilityLevel.Breaking, Assert.Single(regression.Issues).Level);

        var recovery = new SnapshotComparer().Compare(
            new { steps = new[] { new { success = false } } },
            new { steps = new[] { new { success = true } } });
        Assert.Equal(CompatibilityLevel.Warning, Assert.Single(recovery.Issues).Level);
    }

    [Fact]
    public void ReportsPayloadDurationsAsContractChanges()
    {
        var comparison = new SnapshotComparer().Compare(
            new { steps = new[] { new { return_value = new { json_data = new { items = new[] { new { duration = 10 } } } } } } },
            new { steps = new[] { new { return_value = new { json_data = new { items = new[] { new { duration = 900 } } } } } } });

        // Only the framework's own $.steps[n].duration gets the 50% tolerance.
        var issue = Assert.Single(comparison.Issues);
        Assert.Equal("response", issue.Type);
        Assert.Equal(CompatibilityLevel.Breaking, issue.Level);
    }

    [Fact]
    public void FreeTextRedactionCoversEverySensitiveFragment()
    {
        var masker = new Masker();

        Assert.DoesNotContain("123456", masker.MaskText("mfa_code=123456"), StringComparison.Ordinal);
        Assert.DoesNotContain("AKIAEXAMPLE", masker.MaskText("{\"AWS_ACCESS_KEY_ID\":\"AKIAEXAMPLE\"}"), StringComparison.Ordinal);
        Assert.DoesNotContain("999111", masker.MaskText("{\"confirmation_code\":\"999111\"}"), StringComparison.Ordinal);
    }

    [Fact]
    public void ReportsADurationTypeChangeInsteadOfApplyingTolerance()
    {
        var comparison = new SnapshotComparer().Compare(
            new { duration = 1_000_000L }, new { duration = "1ms" });

        // The tolerance must not swallow a schema change.
        Assert.Equal(CompatibilityLevel.Breaking, Assert.Single(comparison.Issues).Level);
    }

    [Fact]
    public void ComparesResponseFieldsThatShareAFrameworkFieldName()
    {
        var comparison = new SnapshotComparer().Compare(
            new { steps = new[] { new { return_value = new { json_data = new { attempts = 1, timestamp = "a" } } } } },
            new { steps = new[] { new { return_value = new { json_data = new { attempts = 2, timestamp = "b" } } } } });

        // Only the framework's own metadata is ignored; nested response fields are contract.
        Assert.Equal(2, comparison.Issues.Count);
        Assert.All(comparison.Issues, issue => Assert.Equal("response", issue.Type));
    }

    [Fact]
    public void IgnoresTheFrameworksOwnStepMetadata()
    {
        var comparison = new SnapshotComparer().Compare(
            new { steps = new[] { new { attempts = 1, timestamp = "a" } } },
            new { steps = new[] { new { attempts = 3, timestamp = "b" } } });

        Assert.Empty(comparison.Issues);
    }

    [Fact]
    public void MasksARootStringResponseWithoutThrowing()
    {
        var masker = new SnapshotMasker();

        // A primitive string response has no parent token, so masking must not throw.
        Assert.Equal("\"plain\"", masker.ProcessTokenCopy(JToken.Parse("\"plain\""))!.ToString(Newtonsoft.Json.Formatting.None));
        Assert.Equal("plain", masker.ProcessBody("plain"));
    }

    [Fact]
    public void NormalizesConfiguredSeverityCasing()
    {
        var config = new SnapshotConfig
        {
            ValidationRuleOverrides = new Dictionary<string, ValidationRule>(StringComparer.OrdinalIgnoreCase)
            {
                ["completion"] = new(true, "ERROR")
            }
        };

        // Findings are counted by exact value, so the severity must be canonicalised.
        Assert.Equal(SnapshotConfig.SeverityError, config.Rule("completion").Severity);
        var validation = new SnapshotValidator().Validate(
            SnapshotFixtures.Story(steps: Array.Empty<StepSnapshot>(), summary: SnapshotFixtures.Summary(0, 0, 0, 0)),
            config);
        Assert.Equal(1, validation.Summary.TotalErrors);
        Assert.False(validation.IsValid);
    }

    [Fact]
    public void MasksAccessKeyIdentifiersAndAssignments()
    {
        var masker = new Masker();

        Assert.True(masker.IsSensitiveKey("AccessKeyId"));
        Assert.True(masker.IsSensitiveKey("AWSAccessKeyId"));
        Assert.True(masker.IsSensitiveKey("aws_secret_access_key"));
        Assert.True(masker.IsSensitiveKey("confirmationCode"));
        Assert.True(masker.IsSensitiveKey("mfa_code"));

        var masked = masker.MaskText("AWS_ACCESS_KEY_ID=AKIAIOSFODNN7EXAMPLE failed");
        Assert.DoesNotContain("AKIAIOSFODNN7EXAMPLE", masked, StringComparison.Ordinal);
        Assert.Contains("AWS_ACCESS_KEY_ID=[MASKED]", masked, StringComparison.Ordinal);
    }

    [Fact]
    public void EmitsAnEmptyObjectForVoidHttpResponses()
    {
        var result = new StoryResult("story", TestStatus.Passed, TimeSpan.FromMilliseconds(2), new[]
        {
            new StepResult("delete", "DeleteStripeInfo", CallStyle.WithHttpInfo, TestStatus.Passed,
                TimeSpan.FromMilliseconds(2), StatusCode: 204)
        });

        var snapshot = SnapshotFactory.Create(new Story { Name = "story" }, result, new SnapshotConfig());
        var jsonData = snapshot.Steps[0].ReturnValue!.JsonData;

        // Go and PHP emit {} so a later typed response is an added field, not a type change.
        Assert.NotNull(jsonData);
        Assert.Equal(JTokenType.Object, jsonData!.Type);
        Assert.False(jsonData.HasValues);
    }

    [Fact]
    public void RedactsSignedQueryParametersOfPresignedUrls()
    {
        const string url = "https://bucket.s3.amazonaws.com/template?X-Amz-Algorithm=AWS4-HMAC-SHA256" +
                           "&X-Amz-Credential=ASIAEXAMPLE%2F20260101%2Fap-northeast-1%2Fs3%2Faws4_request" +
                           "&X-Amz-Security-Token=IQoJb3JpZ2luX2VjEXAMPLE&X-Amz-Signature=deadbeefcafe";

        var masked = new Masker().MaskText(url);

        Assert.DoesNotContain("ASIAEXAMPLE", masked, StringComparison.Ordinal);
        Assert.DoesNotContain("IQoJb3JpZ2luX2Vj", masked, StringComparison.Ordinal);
        Assert.DoesNotContain("deadbeefcafe", masked, StringComparison.Ordinal);
        // The non-secret parameters stay so the snapshot still shows the URL shape.
        Assert.Contains("X-Amz-Algorithm=AWS4-HMAC-SHA256", masked, StringComparison.Ordinal);
        Assert.Contains("X-Amz-Signature=[MASKED]", masked, StringComparison.Ordinal);
    }

    [Fact]
    public void MasksPresignedUrlsCarriedInsideResponseBodies()
    {
        var masked = new SnapshotMasker().ProcessBody(
            "{\"cloudformation_template_url\":\"https://b.s3.amazonaws.com/t?X-Amz-Signature=abc123\"}");

        Assert.DoesNotContain("abc123", masked, StringComparison.Ordinal);
    }

    [Fact]
    public void MasksMenuAndPlanIdentifierAliases()
    {
        var masker = new SnapshotMasker();

        Assert.True(masker.IsDynamicKey("menuId"));
        Assert.True(masker.IsDynamicKey("menu_id"));
        Assert.True(masker.IsDynamicKey("planId"));
        Assert.True(masker.IsDynamicKey("plan_id"));
    }

    [Fact]
    public void NestedFieldsUnderADynamicKeyKeepTheirOwnMasking()
    {
        var masked = (JObject)new SnapshotMasker().ProcessTokenCopy(JObject.Parse(
            "{\"id\":{\"value\":\"opaque\",\"stable\":\"keep-me\",\"user_id\":\"volatile\"}}"))!;

        // The container keeps its shape and each child is judged by its own name, so a
        // stable nested field can still register a snapshot difference.
        Assert.Equal("keep-me", masked["id"]!["stable"]);
        Assert.Equal("opaque", masked["id"]!["value"]);
        Assert.Equal("[DYNAMIC]", masked["id"]!["user_id"]);
    }

    [Fact]
    public void StorySlugMatchesTheSharedNamingWhileTagsKeepPunctuation()
    {
        // Story names collapse every non-alphanumeric character, like PHP's storySlug and
        // Go's sanitizeFileName; tags keep dots and dashes, like PHP's slug.
        Assert.Equal("auth_api_synchronous_http_responses",
            SnapshotConfig.StorySlug("Auth API - synchronous HTTP responses"));
        Assert.Equal("v1.0.0-8-gbcd96cd", SnapshotConfig.Slug("v1.0.0-8-gbcd96cd"));
    }

    [Fact]
    public void KeepsBooleanDynamicFieldsAsBooleans()
    {
        var masked = (JObject)new SnapshotMasker(new[] { "is_registered" }).ProcessTokenCopy(
            JObject.Parse("{\"is_registered\":true}"))!;

        Assert.Equal(JTokenType.Boolean, masked["is_registered"]!.Type);
        Assert.True(masked["is_registered"]!.Value<bool>());
    }

    [Fact]
    public void CanonicalizesJsonBodyPropertyOrder()
    {
        var masker = new SnapshotMasker();

        var first = masker.ProcessBody("{\"b\":1,\"a\":{\"d\":2,\"c\":3}}");
        var second = masker.ProcessBody("{\"a\":{\"c\":3,\"d\":2},\"b\":1}");

        Assert.Equal(first, second);
        Assert.Equal("{\"a\":{\"c\":3,\"d\":2},\"b\":1}", masker.ProcessText("{\"b\":1,\"a\":{\"d\":2,\"c\":3}}"));
    }

    [Fact]
    public void KeepsTheHttpEnvelopeWhenHeadersAreEmptyAndMeasuresUtf8Bytes()
    {
        const string body = "{\"name\":\"日本語\"}";
        var result = new StoryResult("story", TestStatus.Passed, TimeSpan.FromMilliseconds(5), new[]
        {
            new StepResult("step", "GetStripeInfo", CallStyle.WithHttpInfo, TestStatus.Passed,
                TimeSpan.FromMilliseconds(5), StatusCode: 200, Body: body)
        });

        var snapshot = SnapshotFactory.Create(new Story { Name = "story" }, result, new SnapshotConfig());
        var returnValue = snapshot.Steps[0].ReturnValue!;
        var http = returnValue.HttpResponse;

        // A response without headers still carries a status and a body worth recording.
        Assert.NotNull(http);
        Assert.Equal(200, http!.StatusCode);
        Assert.Empty(http.Headers);
        // The length describes the body the artifact persists, measured in UTF-8 bytes.
        Assert.Equal(System.Text.Encoding.UTF8.GetByteCount(returnValue.Body), http.ContentLength);
        Assert.NotEqual(returnValue.Body.Length, http.ContentLength);
    }

    [Fact]
    public void SerializesDictionaryKeysInOrdinalOrder()
    {
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["X-Zeta"] = "z",
            ["Content-Type"] = "application/json",
            ["Accept"] = "*/*"
        };
        var snapshot = SnapshotFixtures.Story(steps: new[]
        {
            SnapshotFixtures.Step(returnValue: SnapshotFixtures.ReturnValue(new { ok = true }, headers: headers))
        });

        var serialized = (JObject)JObject.Parse(SnapshotJson.Serialize(snapshot))["steps"]![0]!["return_value"]!["headers"]!;

        Assert.Equal(new[] { "Accept", "Content-Type", "X-Zeta" },
            serialized.Properties().Select(property => property.Name).ToArray());
    }

    [Fact]
    public void DetectsRemovedAndAddedFields()
    {
        var comparison = new SnapshotComparer().Compare(
            new { kept = 1, removed = true },
            new { kept = 1, added = true });

        Assert.Equal(CompatibilityLevel.Breaking, comparison.Level);
        Assert.Contains(comparison.Issues, issue =>
            issue.Level == CompatibilityLevel.Breaking && issue.Path == "$.removed");
        Assert.Contains(comparison.Issues, issue =>
            issue.Level == CompatibilityLevel.Warning && issue.Path == "$.added");
    }

    [Fact]
    public void IgnoresNormalisedValuesAndTransportHeaders()
    {
        var comparison = new SnapshotComparer().Compare(
            new
            {
                steps = new[]
                {
                    new
                    {
                        return_value = new
                        {
                            json_data = new { request_body = string.Empty },
                            headers = new Dictionary<string, string> { ["Transfer-Encoding"] = "chunked" }
                        }
                    }
                }
            },
            new
            {
                steps = new[]
                {
                    new
                    {
                        return_value = new
                        {
                            json_data = new { request_body = SnapshotMasker.DynamicToken },
                            headers = new Dictionary<string, string>()
                        }
                    }
                }
            });

        // A normalised value and transport framing headers carry no contract information.
        Assert.Empty(comparison.Issues);
        Assert.Equal(CompatibilityLevel.Compatible, comparison.Level);
    }

    [Fact]
    public void IgnoresCollectionReorderingButNotContentChanges()
    {
        var comparer = new SnapshotComparer();
        var fixedUnit = new { type = "fixed", unit_amount = 1000 };
        var tieredUnit = new { type = "tiered_usage", unit_amount = 0 };

        // The collection endpoints do not guarantee ordering.
        var reordered = comparer.Compare(
            new { units = new[] { fixedUnit, tieredUnit } },
            new { units = new[] { tieredUnit, fixedUnit } });
        Assert.Empty(reordered.Issues);

        // A content change inside a reordered collection is still reported.
        var changed = comparer.Compare(
            new { units = new[] { fixedUnit, tieredUnit } },
            new { units = new[] { tieredUnit, new { type = "fixed", unit_amount = 2000 } } });
        Assert.NotEmpty(changed.Issues);

        // Scalar lists keep their positional comparison.
        var scalars = comparer.Compare(new { ids = new[] { "a", "b" } }, new { ids = new[] { "b", "a" } });
        Assert.NotEmpty(scalars.Issues);

        // So do ordered arrays: the story steps and payload lists such as pricing tiers.
        var steps = comparer.Compare(
            new { steps = new[] { new { step_name = "CreatePricingUnit" }, new { step_name = "GetPricingUnit" } } },
            new { steps = new[] { new { step_name = "GetPricingUnit" }, new { step_name = "CreatePricingUnit" } } });
        Assert.Contains(steps.Issues, issue => issue.Type == "step_sequence");
        Assert.Equal(CompatibilityLevel.Breaking, steps.Level);

        var tiers = comparer.Compare(
            new { tiers = new[] { new { up_to = 10 }, new { up_to = 20 } } },
            new { tiers = new[] { new { up_to = 20 }, new { up_to = 10 } } });
        Assert.NotEmpty(tiers.Issues);

        // The raw body on the framework's envelope is JSON text, so the same rules apply to it.
        var body = comparer.Compare(
            new { steps = new[] { new { return_value = new { body = "{\"units\":[{\"type\":\"fixed\"},{\"type\":\"tiered_usage\"}]}" } } } },
            new { steps = new[] { new { return_value = new { body = "{\n  \"units\": [{\"type\": \"tiered_usage\"}, {\"type\": \"fixed\"}]\n}" } } } });
        Assert.Empty(body.Issues);

        var changedBody = comparer.Compare(
            new { steps = new[] { new { return_value = new { body = "{\"units\":[{\"type\":\"fixed\"},{\"type\":\"tiered_usage\"}]}" } } } },
            new { steps = new[] { new { return_value = new { body = "{\"units\":[{\"type\":\"tiered_usage\"},{\"type\":\"usage\"}]}" } } } });
        Assert.NotEmpty(changedBody.Issues);
    }

    [Fact]
    public void NormalisesEnvironmentListingsAndVolatileText()
    {
        var masked = JObject.FromObject(new SnapshotMasker().Process(new
        {
            // Whatever the shared environment happens to contain.
            tenants = new object[]
            {
                new { id = "t-1", name = "test_tenant", envs = new[] { new { name = "dev", id = 3 } }, back_office_staff_email = "a@example.com" },
                new { id = "t-2", name = "Moore and Sons", envs = Array.Empty<object>(), back_office_staff_email = "b@example.com" }
            },
            // A pre-signed URL: host, path and the stable parameters stay, the signature,
            // the request date and the volatile identifiers do not.
            link = "https://us-east-1.console.aws.amazon.com/cloudformation/home" +
                   "?templateURL=x&X-Amz-Credential=AKIAEXAMPLE/20260813/us-east-1/s3/aws4_request" +
                   "&X-Amz-Date=20260813T050000Z&param_ExternalID=02ae083b-6201-4fb7-a7fc-6392fc122e02" +
                   "&region=us-east-1",
            // A name the suite generated for this run.
            env_display_name = "C# Auth E2E Environment 5ab1455f3f5f",
            // A stable name that only looks similar.
            display_name = "C# E2E Meter"
        })!);

        var tenant = (JObject)masked["tenants"]![0]!;
        Assert.Equal(new[] { "back_office_staff_email", "envs", "id", "name" },
            tenant.Properties().Select(property => property.Name));
        Assert.Equal(SnapshotMasker.DynamicToken, tenant["name"]!.Value<string>());
        Assert.Equal(SnapshotMasker.DynamicToken, ((JObject)tenant["envs"]![0]!)["name"]!.Value<string>());
        Assert.Equal(0, ((JObject)tenant["envs"]![0]!)["id"]!.Value<int>());

        Assert.Equal(
            "https://us-east-1.console.aws.amazon.com/cloudformation/home" +
            "?templateURL=x&X-Amz-Credential=[MASKED]&X-Amz-Date=[DYNAMIC]" +
            "&param_ExternalID=[DYNAMIC]&region=us-east-1",
            masked["link"]!.Value<string>());
        Assert.Equal("C# Auth E2E Environment [DYNAMIC]", masked["env_display_name"]!.Value<string>());
        Assert.Equal("C# E2E Meter", masked["display_name"]!.Value<string>());
    }

    [Fact]
    public void IgnoresEnvironmentListingLengthButNotItsShape()
    {
        var comparer = new SnapshotComparer();
        var tenant = new { id = SnapshotMasker.DynamicToken, name = SnapshotMasker.DynamicToken };

        // A tenant added to the shared environment between runs is not a contract change.
        var grown = comparer.Compare(
            new { tenants = new[] { tenant } },
            new { tenants = new[] { tenant, tenant } });
        Assert.Empty(grown.Issues);

        // A field that disappears from the model still is.
        var reshaped = comparer.Compare(
            new { tenants = new[] { tenant } },
            new { tenants = new[] { new { id = SnapshotMasker.DynamicToken } } });
        Assert.Contains(reshaped.Issues, issue => issue.Level == CompatibilityLevel.Breaking);

        // Other collections keep reporting a length change.
        var shrunk = comparer.Compare(
            new { steps = new[] { tenant, tenant } },
            new { steps = new[] { tenant } });
        Assert.Contains(shrunk.Issues, issue => issue.Type == "array_length");
    }

    [Fact]
    public void DetectsHttpStatusChangesAsBreaking()
    {
        var comparison = new SnapshotComparer().Compare(
            new { steps = new[] { new { status_code = 200 } } },
            new { steps = new[] { new { status_code = 500 } } });

        var issue = Assert.Single(comparison.Issues);
        Assert.Equal("$.steps[0].status_code", issue.Path);
        Assert.Equal("status_code", issue.Type);
        Assert.Equal(CompatibilityLevel.Breaking, issue.Level);
    }

    [Fact]
    public void TreatsResponsePayloadChangesAsBreaking()
    {
        var comparison = new SnapshotComparer().Compare(
            new { return_value = new { type = "model", json_data = new { enabled = false } } },
            new { return_value = new { type = "model", json_data = new { enabled = true } } });

        var issue = Assert.Single(comparison.Issues);
        Assert.Equal("$.return_value.json_data.enabled", issue.Path);
        Assert.Equal("response", issue.Type);
        Assert.Equal(CompatibilityLevel.Breaking, issue.Level);
    }

    [Fact]
    public void TreatsStateAndHeaderChangesAsWarnings()
    {
        var comparison = new SnapshotComparer().Compare(
            new { state_changes = new { tenant = "a" }, variables = new { seen = 1 } },
            new { state_changes = new { tenant = "b" }, variables = new { seen = 2 } });

        Assert.Equal(CompatibilityLevel.Warning, comparison.Level);
        Assert.All(comparison.Issues, issue => Assert.Equal("state_transition", issue.Type));
    }

    [Fact]
    public void TreatsPassedToFailedTransitionAsBreaking()
    {
        var comparison = new SnapshotComparer().Compare(
            new { steps = new[] { new { detail = "passed" } } },
            new { steps = new[] { new { detail = "failed" } } });

        Assert.Equal(CompatibilityLevel.Breaking, Assert.Single(comparison.Issues).Level);
    }

    [Fact]
    public void IgnoresTimingWhenEitherDurationIsUnmeasured()
    {
        var reported = new SnapshotComparer().Compare(
            new { duration = 1_000_000L }, new { duration = 10_000_000L });
        Assert.Equal("timing", Assert.Single(reported.Issues).Type);

        // A zero duration is unmeasured, not a 100% regression.
        Assert.Empty(new SnapshotComparer().Compare(
            new { duration = 1_000_000L }, new { duration = 0L }).Issues);
        Assert.Empty(new SnapshotComparer().Compare(
            new { duration = 0L }, new { duration = 1_000_000L }).Issues);
    }

    [Fact]
    public void MasksSensitiveResponseHeaders()
    {
        var headers = new Dictionary<string, IReadOnlyList<string>>(StringComparer.OrdinalIgnoreCase)
        {
            ["X-Api-Key"] = new[] { "opaque-api-key" },
            ["Authorization"] = new[] { "Basic opaque-credential" },
            ["Set-Cookie"] = new[] { "session=opaque-session; Secure; HttpOnly" },
            ["Content-Type"] = new[] { "application/json" }
        };
        var step = new StepResult("get", "Get", CallStyle.WithHttpInfo, TestStatus.Passed,
            TimeSpan.Zero, 200, new { ok = true }, headers);
        var result = new StoryResult("story", TestStatus.Passed, TimeSpan.Zero, new[] { step });

        var snapshot = SnapshotFactory.Create(new Story { Name = "story" }, result, new SnapshotConfig());
        var returnValue = Assert.IsType<SnapshotReturnValue>(snapshot.Steps[0].ReturnValue);

        Assert.Equal("[MASKED len=14]", returnValue.Headers["X-Api-Key"]);
        Assert.Equal("[MASKED len=23]", returnValue.Headers["Authorization"]);
        Assert.Equal("[MASKED len=40]", returnValue.Headers["Set-Cookie"]);
        Assert.Equal("application/json", returnValue.Headers["Content-Type"]);
        Assert.NotNull(returnValue.HttpResponse);
        Assert.Equal(200, returnValue.HttpResponse!.StatusCode);
        Assert.Equal("200 OK", returnValue.HttpResponse.Status);
    }

    [Fact]
    public void MasksSensitiveFieldsInRawJsonBody()
    {
        const string body = "{\"client_secret\":\"secret-value\",\"cookie\":\"session-value\",\"credential\":\"credential-value\",\"stable\":\"visible\"}";
        var step = new StepResult("get", "Get", CallStyle.WithHttpInfo, TestStatus.Passed,
            TimeSpan.Zero, 200, new { ok = true }, Body: body);
        var snapshot = SnapshotFactory.Create(
            new Story { Name = "story" },
            new StoryResult("story", TestStatus.Passed, TimeSpan.Zero, new[] { step }),
            new SnapshotConfig());

        var returnValue = Assert.IsType<SnapshotReturnValue>(snapshot.Steps[0].ReturnValue);
        var raw = JObject.Parse(returnValue.Body);
        Assert.Equal("[MASKED len=12]", raw["client_secret"]);
        Assert.Equal("[MASKED len=13]", raw["cookie"]);
        Assert.Equal("[MASKED len=16]", raw["credential"]);
        Assert.Equal("visible", raw["stable"]);
        Assert.DoesNotContain("secret-value", returnValue.Body, StringComparison.Ordinal);
    }

    [Fact]
    public void IgnoresVolatileHeadersAndTraceIdentifiers()
    {
        var comparison = new SnapshotComparer().Compare(
            new
            {
                steps = new[]
                {
                    new
                    {
                        return_value = new
                        {
                            headers = new Dictionary<string, string> { ["Date"] = "old", ["X-Runtime"] = "1" },
                            http_response = new { trace_id = "old-trace" }
                        }
                    }
                }
            },
            new
            {
                steps = new[]
                {
                    new
                    {
                        return_value = new
                        {
                            headers = new Dictionary<string, string> { ["Server"] = "new", ["X-Correlation-Id"] = "2" },
                            http_response = new { trace_id = "new-trace" }
                        }
                    }
                }
            });

        Assert.Empty(comparison.Issues);

        // The same names inside an API payload are contract, not framework metadata.
        var payload = new SnapshotComparer().Compare(
            new { steps = new[] { new { return_value = new { json_data = new { headers = new { Server = "old" } } } } } },
            new { steps = new[] { new { return_value = new { json_data = new { headers = new { Server = "new" } } } } } });
        Assert.Single(payload.Issues);
    }

    [Fact]
    public void ToleratesSmallDurationDriftButReportsLargeRegressions()
    {
        var tolerated = new SnapshotComparer().Compare(
            new { duration = 1_000_000L, summary = new { total_duration = 1_000_000L } },
            new { duration = 1_400_000L, summary = new { total_duration = 1_400_000L } });
        Assert.Empty(tolerated.Issues);

        var regression = new SnapshotComparer().Compare(
            new { duration = 1_000_000L },
            new { duration = 3_000_000L });
        var issue = Assert.Single(regression.Issues);
        Assert.Equal("timing", issue.Type);
        Assert.Equal(CompatibilityLevel.Warning, issue.Level);
        Assert.Contains("200.0%", issue.Description, StringComparison.Ordinal);
    }

    [Fact]
    public async Task SnapshotStoreRoundTripsWithTaggedPath()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = new SnapshotConfig { OutputDirectory = directory, ModuleName = "billing", CurrentTag = "v1", Overwrite = true };
            var store = new SnapshotStore(config);
            Assert.Contains("story_snapshot_v1_get_user.json", store.SnapshotPath("v1", "Get User"));
            var snapshot = SnapshotFixtures.Story("Get User", steps: Array.Empty<StepSnapshot>(),
                summary: SnapshotFixtures.Summary(0, 0), metadata: SnapshotFixtures.Metadata(gitTag: "v1"));

            await store.SaveSnapshotAsync(snapshot, "v1");
            var loaded = await store.LoadSnapshotAsync("v1", "Get User");

            Assert.Equal(snapshot.StoryName, loaded.StoryName);
            Assert.Equal(snapshot.Status, loaded.Status);
            Assert.Equal(snapshot.DurationNanoseconds, loaded.DurationNanoseconds);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task DiscoversSnapshotsWithCustomFileNameFormat()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = new SnapshotConfig
            {
                OutputDirectory = directory,
                ModuleName = "billing",
                FileNameFormat = "custom-{story_name}-at-{tag}.snapshot",
                Overwrite = true
            };
            var store = new SnapshotStore(config);
            var snapshot = SnapshotFixtures.Story("Get User", steps: Array.Empty<StepSnapshot>(),
                summary: SnapshotFixtures.Summary(0, 0), metadata: SnapshotFixtures.Metadata(gitTag: "v1"));

            await store.SaveSnapshotAsync(snapshot, "v1");

            Assert.Equal(new[] { "v1" }, store.AvailableTags());
            Assert.Equal(new[] { "Get User" }, store.StoriesForTag("v1"));
            Assert.Equal("Get User", (await store.LoadSnapshotAsync("v1", "Get User")).StoryName);
        }
        finally { Directory.Delete(directory, true); }
    }

    [Fact]
    public async Task LoadsSnapshotsWithoutParametersAsEmptyObjects()
    {
        var directory = TemporaryDirectory();
        try
        {
            var config = new SnapshotConfig
            {
                OutputDirectory = directory,
                ModuleName = "billing",
                CurrentTag = "v1",
                Overwrite = true
            };
            var store = new SnapshotStore(config);
            var path = store.SnapshotPath("v1", "legacy");
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            await File.WriteAllTextAsync(path,
                """
                {
                  "story_name": "legacy",
                  "description": "",
                  "timestamp": "2026-01-01T00:00:00+00:00",
                  "duration": 1000000,
                  "status": "passed",
                  "variables": {},
                  "steps": [
                    {
                      "step_name": "get",
                      "method": "Get",
                      "return_value": null,
                      "duration": 1000000,
                      "status_code": 200,
                      "success": true,
                      "status": "passed",
                      "timestamp": "2026-01-01T00:00:00+00:00"
                    }
                  ],
                  "summary": {
                    "total_steps": 1,
                    "successful_steps": 1,
                    "failed_steps": 0,
                    "skipped_steps": 0,
                    "total_duration": 1000000,
                    "average_step_duration": 1000000
                  },
                  "metadata": {
                    "sdk_version": "unknown",
                    "test_environment": "dev",
                    "capture_level": "FULL",
                    "git_tag": "v1"
                  }
                }
                """);

            var loaded = await store.LoadSnapshotAsync("v1", "legacy");

            Assert.IsType<JObject>(loaded.Steps[0].Parameters);
            Assert.False(loaded.Steps[0].Parameters.HasValues);
            Assert.Equal(TestStatus.Passed, loaded.Status);
            Assert.Equal(CaptureLevel.Full, loaded.Metadata.CaptureLevel);
        }
        finally { Directory.Delete(directory, true); }
    }

    private static string TemporaryDirectory()
    {
        var path = Path.Combine(Path.GetTempPath(), $"saasus-csharp-{Guid.NewGuid():N}");
        Directory.CreateDirectory(path);
        return path;
    }
}
