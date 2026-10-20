using System.Net;
using System.Reflection;
using Newtonsoft.Json;

namespace SaasusSdk.Contract;

// Reflective engine behind ContractTest.
//
// Only WithHttpInfo / WithHttpInfoAsync are exercised: Op / OpAsync just call
// them and return .Data, and RestSharp reports a response that fails to
// deserialize as ErrorText with a null Data rather than as an exception, which
// only the WithHttpInfo result exposes.
internal static class ContractEngine
{
    // Declared once, in the shared fetch-openapi-spec action, and passed from
    // there to both the Prism setup and here, so the specs fetched and the
    // modules enumerated cannot drift apart. No default: it would be a second
    // copy of the list, and would let the suite test fewer modules than were
    // fetched. Empty is reported by ContractTest.ModuleListIsProvided.
    internal static readonly string[] Modules =
        (Environment.GetEnvironmentVariable("CONTRACT_MODULES") ?? string.Empty)
            .Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

    private static readonly Assembly SdkAssembly = typeof(authapi.Client.Configuration).Assembly;

    private static readonly Lazy<IReadOnlyDictionary<string, ContractCase>> LazyCases =
        new(BuildCases);

    static ContractEngine()
    {
        // ExpansionApiClient signs every request with SaaSus SigV1 from these
        // variables and throws when any is unset, so the mock is unreachable
        // without them; it does not validate the signature. Overwritten rather
        // than defaulted so a developer's real credentials are never signed
        // into a request sent to the mock.
        Environment.SetEnvironmentVariable("SAASUS_SECRET_KEY", "synthetic-secret-key");
        Environment.SetEnvironmentVariable("SAASUS_API_KEY", "synthetic-api-key");
        Environment.SetEnvironmentVariable("SAASUS_SAAS_ID", "synthetic-saas-id");
    }

    internal static IReadOnlyDictionary<string, ContractCase> Cases => LazyCases.Value;

    internal static IEnumerable<string> UnmappedOperations(string module)
    {
        var spec = SpecDocument.LoadOrNull(SpecDir, module);
        if (spec == null)
        {
            yield break;
        }
        foreach (var operation in spec.Operations)
        {
            if (FindApiType(module, operation.OperationId) == null)
            {
                yield return module + "." + operation.OperationId;
            }
        }
    }

    // No other spec is substituted for a missing one: a mock must be exercised
    // with the document it was started from, or a request the mock rejects and
    // one the spec never declared become indistinguishable.
    internal static IEnumerable<string> ModulesMissingSpec() =>
        Modules.Where(module => PrismEndpoints.UrlForOrNull(module) != null
            && SpecDocument.LoadOrNull(SpecDir, module) == null);

    // Read from the compiled code so that comparing it against Modules does not
    // require a second copy of the list.
    internal static IEnumerable<string> GeneratedModules()
    {
        const string suffix = "api.Api";
        return SdkAssembly.GetTypes()
            .Select(type => type.Namespace)
            .Where(ns => ns != null && ns.EndsWith(suffix, StringComparison.Ordinal))
            .Select(ns => ns!.Substring(0, ns!.Length - suffix.Length))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(module => module, StringComparer.Ordinal);
    }

    private static string? SpecDir => Environment.GetEnvironmentVariable("CONTRACT_SPEC_DIR");

    private static IReadOnlyDictionary<string, ContractCase> BuildCases()
    {
        var cases = new Dictionary<string, ContractCase>(StringComparer.Ordinal);
        foreach (var module in Modules)
        {
            var baseUrl = PrismEndpoints.UrlForOrNull(module);
            var spec = SpecDocument.LoadOrNull(SpecDir, module);
            if (baseUrl == null || spec == null)
            {
                continue;
            }
            foreach (var operation in spec.Operations)
            {
                var apiType = FindApiType(module, operation.OperationId);
                if (apiType == null)
                {
                    continue;
                }
                var labels = spec.BodyVariantLabels(operation);
                for (var index = 0; index < Math.Max(labels.Count, 1); index++)
                {
                    var label = index < labels.Count ? labels[index] : null;
                    foreach (var form in new[] { CallForm.Sync, CallForm.Async })
                    {
                        var contractCase = new ContractCase(module, apiType, spec, operation, index, label, form, baseUrl);
                        if (!cases.TryAdd(contractCase.Id, contractCase))
                        {
                            throw new ContractFailure($"duplicate contract case id: {contractCase.Id}");
                        }
                    }
                }
            }
        }
        return cases;
    }

    // Null when this SDK lacks the operation, which is what lets a previously
    // released SDK run the same suite. Matched on the method name rather than
    // the spec's tag to stay independent of the generator's tag-to-class
    // naming; the WithHttpInfo suffix makes that unambiguous.
    private static Type? FindApiType(string module, string operationId)
    {
        foreach (var type in ApiTypes(module))
        {
            if (SyncMethod(type, operationId) != null && AsyncMethod(type, operationId) != null)
            {
                return type;
            }
        }
        return null;
    }

    private static IEnumerable<Type> ApiTypes(string module) =>
        SdkAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Namespace == module + "api.Api")
            .OrderBy(t => t.Name, StringComparer.Ordinal);

    internal static MethodInfo? SyncMethod(Type apiType, string operationId) =>
        DeclaredMethod(apiType, operationId + "WithHttpInfo");

    internal static MethodInfo? AsyncMethod(Type apiType, string operationId) =>
        DeclaredMethod(apiType, operationId + "WithHttpInfoAsync");

    private static MethodInfo? DeclaredMethod(Type apiType, string name) =>
        apiType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(m => m.Name == name && !m.IsSpecialName);

    // The hand-written configuration wrappers (generated/modules) are bypassed:
    // what this suite verifies against the spec is the generated client.
    internal static object CreateApi(string module, Type apiType, string baseUrl)
    {
        var configurationType = SdkAssembly.GetType(module + "api.Client.Configuration")
            ?? throw new InvalidOperationException($"{module}: no generated Configuration type");
        var configuration = Activator.CreateInstance(configurationType)!;
        configurationType.GetProperty("BasePath")!.SetValue(configuration, baseUrl);
        return Activator.CreateInstance(apiType, configuration)!;
    }

    // Bound by name, not by position, so an SDK generated from another spec
    // revision (a parameter inserted in the middle) still gets each value in
    // the right argument.
    //
    // The request body is the only argument with no spec parameter, hence the
    // unbound count. Any other unbound argument cannot be explained from the
    // spec, and a constant would send a value the spec never declared, so
    // binding fails instead.
    internal static object?[] BuildArgs(MethodInfo method, SpecDocument spec, SpecOperation operation, int variantIndex)
    {
        var parameters = method.GetParameters();
        var bound = new SpecParam?[parameters.Length];
        var unbound = new List<int>();
        for (var i = 0; i < parameters.Length; i++)
        {
            bound[i] = operation.Parameters.FirstOrDefault(p => Matches(p.Name, parameters[i].Name));
            if (bound[i] == null && !IsGeneratorArgument(parameters[i]))
            {
                unbound.Add(i);
            }
        }

        var expected = operation.BodySchema != null ? 1 : 0;
        if (unbound.Count != expected)
        {
            throw new ContractFailure($"{operation.OperationId}: parameter binding failed — expected {expected}"
                + $" unbound argument(s) (the request body), got {unbound.Count} of {parameters.Length}");
        }

        var args = new object?[parameters.Length];
        for (var i = 0; i < parameters.Length; i++)
        {
            var parameter = parameters[i];
            if (bound[i] is { } specParam)
            {
                args[i] = Coerce(spec.ParamValue(specParam), parameter.ParameterType);
            }
            else if (unbound.Count > 0 && i == unbound[0])
            {
                args[i] = Coerce(spec.BodyValue(operation, variantIndex), parameter.ParameterType);
            }
            else
            {
                args[i] = parameter.HasDefaultValue ? parameter.DefaultValue : Default(parameter.ParameterType);
            }
        }
        return args;
    }

    private static bool IsGeneratorArgument(ParameterInfo parameter) =>
        parameter.ParameterType == typeof(CancellationToken)
        || (parameter.Name == "operationIndex" && parameter.ParameterType == typeof(int));

    // created_date -> createdDate, X-Saasus-Referer -> xSaasusReferer.
    private static bool Matches(string specName, string? argumentName) =>
        argumentName != null && Flatten(specName) == Flatten(argumentName);

    private static string Flatten(string name) =>
        new string(name.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();

    // Through JSON, the same conversion the SDK applies to a request body: the
    // generated models tag each property with its spec name, so a value keyed
    // by spec names binds by name rather than by position.
    private static object? Coerce(object? value, Type target)
    {
        if (value == null)
        {
            return Default(target);
        }
        return JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value), target);
    }

    private static object? Default(Type target) =>
        target.IsValueType ? Activator.CreateInstance(target) : null;

    // 0 when the error is not a generated ApiException.
    internal static int HttpStatusOf(Exception error) =>
        error.GetType().GetProperty("ErrorCode")?.GetValue(error) as int? ?? 0;
}

internal enum CallForm
{
    Sync,
    Async
}

// Carries only what this harness composed itself, never a response body.
internal sealed class ContractFailure : Xunit.Sdk.XunitException
{
    internal ContractFailure(string message) : base(message)
    {
    }
}

internal sealed class ContractCase
{
    private readonly string _module;
    private readonly Type _apiType;
    private readonly SpecDocument _spec;
    private readonly SpecOperation _operation;
    private readonly int _variantIndex;
    private readonly string _baseUrl;

    internal ContractCase(string module, Type apiType, SpecDocument spec, SpecOperation operation,
        int variantIndex, string? variantLabel, CallForm form, string baseUrl)
    {
        _module = module;
        _apiType = apiType;
        _spec = spec;
        _operation = operation;
        _variantIndex = variantIndex;
        _baseUrl = baseUrl;
        Form = form;
        // The variant index is in the id because two oneOf/anyOf members can
        // share a label (inline members have no $ref to name them) and cases
        // are keyed by id, so a shared id would leave one variant unsent.
        Id = $"{module}.{apiType.Name}.{operation.OperationId}"
            + (variantLabel == null ? string.Empty : $"(#{variantIndex} {variantLabel})")
            + $" [{form}]";
    }

    // e.g. pricing.PricingUnitsApi.CreatePricingUnit(#0 fixed) [Sync]
    internal string Id { get; }

    internal CallForm Form { get; }

    internal async Task InvokeAsync()
    {
        try
        {
            var api = ContractEngine.CreateApi(_module, _apiType, _baseUrl);
            var method = Form == CallForm.Sync
                ? ContractEngine.SyncMethod(_apiType, _operation.OperationId)!
                : ContractEngine.AsyncMethod(_apiType, _operation.OperationId)!;
            var args = ContractEngine.BuildArgs(method, _spec, _operation, _variantIndex);

            var result = method.Invoke(api, args);
            if (Form == CallForm.Async)
            {
                var task = (Task)result!;
                await task.ConfigureAwait(false);
                result = task.GetType().GetProperty("Result")!.GetValue(task);
            }
            AssertRoundTrip(new ApiResponseView(result!));
        }
        catch (ContractFailure)
        {
            throw;
        }
        catch (Exception exception)
        {
            // Unwrapped so the declared-status check below sees the real
            // ApiException.
            var error = exception is TargetInvocationException { InnerException: { } inner } ? inner : exception;
            // For an operation declaring no 2xx (ReturnInternalServerError),
            // surfacing the declared error IS the check. Everything else fails:
            // a 4xx means the mock rejected a request built from the same spec
            // it validates against, so the SDK or the spec is wrong; a 5xx means
            // Prism could not produce the declared response, a spec defect.
            // Skipping either would leave that operation's response
            // deserialization unverified.
            var status = ContractEngine.HttpStatusOf(error);
            if (_operation.HasSuccess || status == 0 || !_operation.DeclaredStatuses.Contains(status))
            {
                throw Failure(error);
            }
        }
    }

    private void AssertRoundTrip(ApiResponseView response)
    {
        if (response.Status is < 200 or >= 300)
        {
            // Reached only if the generated exception factory was disabled or
            // the status is 1xx/3xx.
            if (_operation.HasSuccess || !_operation.DeclaredStatuses.Contains(response.Status))
            {
                throw new ContractFailure($"{Id} failed: unexpected HTTP {response.Status}");
            }
            return;
        }
        // RestSharp puts a response it could not deserialize here instead of
        // throwing. Not quoted: the generated codec copies the body into it.
        if (!string.IsNullOrEmpty(response.ErrorText))
        {
            throw new ContractFailure($"{Id} failed: HTTP {response.Status} response could not be deserialized"
                + " — response bodies are deliberately not reported; re-run locally for the detail");
        }
        // A body-less operation is typed ApiResponse<Object>, so a null payload
        // is only a failure for the rest.
        if (response.PayloadType != typeof(object) && response.Data == null)
        {
            throw new ContractFailure($"{Id} failed: HTTP {response.Status} response deserialized to null");
        }
    }

    // The original exception is neither chained nor quoted: generated models put
    // the response body in their deserialization message, and the runner prints
    // that whole chain into the uploaded report. Diagnosis needs a local re-run.
    private ContractFailure Failure(Exception error)
    {
        var status = ContractEngine.HttpStatusOf(error);
        return new ContractFailure($"{Id} failed: {error.GetType().FullName}"
            + (status != 0 ? $" (HTTP {status})" : string.Empty)
            + " — response bodies are deliberately not reported; re-run locally for the detail");
    }
}

// Reads the generated ApiResponse<T>, whose T differs per operation.
internal sealed class ApiResponseView
{
    internal ApiResponseView(object response)
    {
        var type = response.GetType();
        Status = (int)(HttpStatusCode)type.GetProperty("StatusCode")!.GetValue(response)!;
        ErrorText = type.GetProperty("ErrorText")!.GetValue(response) as string;
        Data = type.GetProperty("Data")!.GetValue(response);
        PayloadType = type.GetGenericArguments()[0];
    }

    internal int Status { get; }

    internal string? ErrorText { get; }

    internal object? Data { get; }

    internal Type PayloadType { get; }
}
