using System.Reflection;
using System.Runtime.ExceptionServices;
using Newtonsoft.Json.Linq;
using pricingapi.Model;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Pricing;

/// <summary>
/// The generated Pricing API is split across five API classes.  Keeping the
/// small invocation abstraction here lets the stories exercise the exact
/// generated method names without duplicating a 120-method adapter interface.
/// </summary>
internal interface IPricingClientInvoker
{
    object? Invoke(string methodName, object?[] arguments);
    Task<object?> InvokeAsync(string methodName, object?[] arguments, CancellationToken cancellationToken);
}

internal sealed class PricingApiClient : IPricingClientInvoker
{
    private readonly IReadOnlyDictionary<string, (object Target, MethodInfo Method)> _methods;

    public PricingApiClient(params object[] apiClients)
    {
        var methods = new Dictionary<string, (object Target, MethodInfo Method)>(StringComparer.Ordinal);
        foreach (var apiClient in apiClients)
        {
            foreach (var method in apiClient.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public))
            {
                if (method.IsSpecialName || method.DeclaringType == typeof(object) ||
                    method.Name == "GetBasePath") continue;
                if (methods.ContainsKey(method.Name))
                    throw new ArgumentException($"Duplicate Pricing API method: {method.Name}.", nameof(apiClients));
                methods[method.Name] = (apiClient, method);
            }
        }

        _methods = methods;
    }

    public object? Invoke(string methodName, object?[] arguments)
    {
        var (target, method) = Resolve(methodName);
        try
        {
            return method.Invoke(target, arguments);
        }
        catch (TargetInvocationException error) when (error.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(error.InnerException).Throw();
            throw;
        }
    }

    public async Task<object?> InvokeAsync(
        string methodName,
        object?[] arguments,
        CancellationToken cancellationToken)
    {
        var asyncArguments = arguments.Append(cancellationToken).ToArray();
        var pending = Invoke(methodName, asyncArguments);
        if (pending is not Task task) return pending;

        await task.ConfigureAwait(false);
        return task.GetType().GetProperty("Result")?.GetValue(task);
    }

    /// <summary>Converts a generated <c>ApiResponse&lt;T&gt;</c> into the shared result shape.</summary>
    public static ExecutionResult ToExecutionResult(object? response)
    {
        if (response is not pricingapi.Client.IApiResponse apiResponse)
            return ExecutionResult.Success(response);

        var headers = apiResponse.Headers?.ToDictionary(
            pair => pair.Key,
            pair => (IReadOnlyList<string>)pair.Value.ToArray(),
            StringComparer.OrdinalIgnoreCase);
        return new ExecutionResult(
            apiResponse.Content,
            (int)apiResponse.StatusCode,
            headers,
            Body: apiResponse.RawContent);
    }

    private (object Target, MethodInfo Method) Resolve(string methodName) =>
        _methods.TryGetValue(methodName, out var method)
            ? method
            : throw new MissingMethodException("Pricing API method was not found: " + methodName);
}

internal sealed record PricingOperation(
    string Method,
    Func<TestContext, PricingArguments> Arguments,
    int ExpectedStatus,
    Func<ExecutionResult, TestContext, Task>? Validate = null,
    Func<ExecutionResult, TestContext, Task>? UpdateState = null)
{
    public string WithHttpInfoMethod => Method + "WithHttpInfo";
    public string AsyncMethod => Method + "Async";
    public string WithHttpInfoAsyncMethod => Method + "WithHttpInfoAsync";

    public string MethodFor(CallStyle style) => style switch
    {
        CallStyle.Sync => Method,
        CallStyle.WithHttpInfo => WithHttpInfoMethod,
        CallStyle.Async => AsyncMethod,
        CallStyle.WithHttpInfoAsync => WithHttpInfoAsyncMethod,
        _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
    };
}

/// <summary>
/// A resolved call: <see cref="Values"/> is the positional array handed to the generated
/// method, while <see cref="Named"/> is the object-shaped view recorded as the snapshot
/// step parameters. Naming the arguments keeps the cross-language snapshot contract and
/// lets <see cref="SnapshotMasker"/> normalise volatile values such as resource IDs.
/// </summary>
internal sealed record PricingArguments(
    object?[] Values,
    IReadOnlyDictionary<string, object?> Named);

internal static class PricingStoryFactory
{
    /// <summary>The generated clients take a trailing <c>operationIndex</c> argument.</summary>
    private const int OperationIndex = 0;

    internal static readonly IReadOnlyCollection<string> ExcludedBaseMethods = new[]
    {
        // Individual deletes are unreliable after resources are linked. The
        // bulk initialization endpoint is the cleanup operation covered below.
        "DeletePricingUnit",
        "DeletePricingMenu",
        "DeletePricingPlan",
        "DeleteMeteringUnitByID",
        // These operations require Billing/Stripe configuration and are outside
        // the Pricing lifecycle, matching the Go and PHP E2E suites.
        "LinkPlanToStripe",
        "DeleteStripePlan"
    };

    internal static readonly IReadOnlyList<PricingOperation> Operations = new[]
    {
        Operation("GetPricingUnits", NoArguments(), 200, ValidatePricingUnits),
        Operation("CreatePricingUnit", context => Arguments(("body", FixedPricingUnit(context))), 201,
            ValidatePricingUnit("fixed"), StorePricingUnit("pricing_unit_id")),
        Operation("GetPricingUnit",
            context => Arguments(("pricing_unit_id", context.GetRequired<string>("pricing_unit_id"))), 200,
            ValidatePricingUnit("fixed")),
        Operation("UpdatePricingUnit", context => Arguments(
                ("pricing_unit_id", context.GetRequired<string>("pricing_unit_id")),
                ("body", FixedPricingUnit(context, updated: true))),
            200, ValidateOptionalJsonBody),

        Operation("GetPricingMenus", NoArguments(), 200, ValidatePricingMenus),
        Operation("CreatePricingMenu", context => Arguments(("body", Menu(context))), 201,
            ValidatePricingMenu, StorePricingMenu),
        Operation("GetPricingMenu",
            context => Arguments(("pricing_menu_id", context.GetRequired<string>("pricing_menu_id"))), 200,
            ValidatePricingMenu),
        Operation("UpdatePricingMenu", context => Arguments(
                ("pricing_menu_id", context.GetRequired<string>("pricing_menu_id")),
                ("body", Menu(context, updated: true))),
            200, ValidateOptionalJsonBody),

        Operation("GetPricingPlans", NoArguments(), 200, ValidatePricingPlans),
        Operation("CreatePricingPlan", context => Arguments(("body", Plan(context))), 201,
            ValidatePricingPlan, StorePricingPlan),
        Operation("GetPricingPlan",
            context => Arguments(("pricing_plan_id", context.GetRequired<string>("pricing_plan_id"))), 200,
            ValidatePricingPlan),
        Operation("UpdatePricingPlan", context => Arguments(
                ("pricing_plan_id", context.GetRequired<string>("pricing_plan_id")),
                ("body", Plan(context, updated: true))),
            200, ValidateOptionalJsonBody),
        Operation("UpdatePricingPlansUsed", context => Arguments(
                ("body", new UpdatePricingPlansUsedParam(new List<string>
                {
                    context.GetRequired<string>("pricing_plan_id")
                }))), 501, RequireNotImplemented),

        Operation("GetMeteringUnitDateCountByTenantIdAndUnitNameAndDate", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("metering_unit_name", context.GetRequired<string>("metering_unit_name")),
                ("date", context.GetRequired<string>("date"))), 200, ValidateMeteringUnitDateCount),
        Operation("DeleteMeteringUnitTimestampCount", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("metering_unit_name", context.GetRequired<string>("metering_unit_name")),
                ("timestamp", context.GetRequired<int>("timestamp"))), 200, ValidateOptionalJsonBody),
        Operation("UpdateMeteringUnitTimestampCount", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("metering_unit_name", context.GetRequired<string>("metering_unit_name")),
                ("timestamp", context.GetRequired<int>("timestamp")),
                ("body", new UpdateMeteringUnitTimestampCountParam(
                    UpdateMeteringUnitTimestampCountMethod.Add, 10))), 200, ValidateMeteringUnitTimestampCount),
        Operation("GetMeteringUnitDateCountByTenantIdAndUnitNameToday", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("metering_unit_name", context.GetRequired<string>("metering_unit_name"))),
            200, ValidateMeteringUnitDateCount),
        Operation("UpdateMeteringUnitTimestampCountNow", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("metering_unit_name", context.GetRequired<string>("metering_unit_name")),
                ("body", new UpdateMeteringUnitTimestampCountNowParam(
                    UpdateMeteringUnitTimestampCountMethod.Add, 5))), 200, ValidateMeteringUnitTimestampCount),
        Operation("GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonth", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("metering_unit_name", context.GetRequired<string>("metering_unit_name"))),
            200, ValidateMeteringUnitMonthCount),
        Operation("GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonth", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("metering_unit_name", context.GetRequired<string>("metering_unit_name")),
                ("month", context.GetRequired<string>("month"))), 200, ValidateMeteringUnitMonthCount),
        Operation("GetMeteringUnitDateCountsByTenantIdAndDate", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("date", context.GetRequired<string>("date"))), 200, ValidateMeteringUnitDateCounts),
        Operation("GetMeteringUnitMonthCountsByTenantIdAndMonth", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("month", context.GetRequired<string>("month"))), 200, ValidateMeteringUnitMonthCounts),
        Operation("DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRates", NoArguments(), 200,
            ValidateOptionalJsonBody),
        Operation("GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod", context => Arguments(
                ("tenant_id", context.GetRequired<string>("tenant_id")),
                ("metering_unit_name", context.GetRequired<string>("metering_unit_name")),
                ("start_timestamp", context.GetRequired<int>("start_timestamp")),
                ("end_timestamp", context.GetRequired<int>("end_timestamp"))),
            200, ValidateMeteringUnitDatePeriodCounts),
        Operation("GetMeteringUnits", NoArguments(), 200, ValidateMeteringUnits),
        Operation("CreateMeteringUnit", context => Arguments(("body", MeteringUnit(context))), 201,
            ValidateMeteringUnit, StoreMeteringUnit),
        Operation("UpdateMeteringUnitByID", context => Arguments(
                ("metering_unit_id", context.GetRequired<string>("metering_unit_id")),
                ("body", MeteringUnit(context, updated: true))),
            200, ValidateOptionalJsonBody),

        Operation("GetTaxRates", NoArguments(), 200, ValidateTaxRates),
        Operation("CreateTaxRate", context => Arguments(("body", TaxRate(context))), 201,
            ValidateTaxRate, StoreTaxRate),
        Operation("UpdateTaxRate", context => Arguments(
                ("tax_rate_id", context.GetRequired<string>("tax_rate_id")),
                ("body", new UpdateTaxRateParam("C# E2E Tax Updated", "Updated by the C# SDK E2E test."))),
            200, ValidateOptionalJsonBody)
    };

    internal static readonly IReadOnlyList<(string Method, CallStyle CallStyle)> Methods =
        Operations.SelectMany(operation => new[]
        {
            (operation.Method, CallStyle.Sync),
            (operation.WithHttpInfoMethod, CallStyle.WithHttpInfo),
            (operation.AsyncMethod, CallStyle.Async),
            (operation.WithHttpInfoAsyncMethod, CallStyle.WithHttpInfoAsync)
        }).ToArray();

    private static readonly IReadOnlyDictionary<string, PricingOperation> OperationByMethod =
        Operations.ToDictionary(operation => operation.Method, StringComparer.Ordinal);

    private static readonly string[] Lifecycle =
    {
        "CreateMeteringUnit",
        "GetMeteringUnits",
        "CreatePricingUnit",
        "CreatePricingUnit",
        "GetPricingUnits",
        "GetPricingUnit",
        "GetPricingUnit",
        "CreatePricingMenu",
        "GetPricingMenus",
        "GetPricingMenu",
        "CreatePricingPlan",
        "GetPricingPlans",
        "GetPricingPlan",
        "CreateTaxRate",
        "GetTaxRates",
        "UpdatePricingUnit",
        "UpdatePricingUnit",
        "UpdatePricingMenu",
        "UpdatePricingPlan",
        "UpdatePricingPlansUsed",
        "UpdateTaxRate",
        "UpdateMeteringUnitByID",
        "UpdateMeteringUnitTimestampCount",
        "GetMeteringUnitDateCountByTenantIdAndUnitNameAndDate",
        "UpdateMeteringUnitTimestampCountNow",
        "GetMeteringUnitDateCountByTenantIdAndUnitNameToday",
        "GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonth",
        "GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonth",
        "GetMeteringUnitDateCountsByTenantIdAndDate",
        "GetMeteringUnitMonthCountsByTenantIdAndMonth",
        "GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod",
        "DeleteMeteringUnitTimestampCount",
        "DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRates"
    };

    public static IReadOnlyList<Story> Create(IPricingClientInvoker client) => new[]
    {
        CreateStory(client, CallStyle.Sync, "synchronous responses"),
        CreateStory(client, CallStyle.WithHttpInfo, "synchronous HTTP responses"),
        CreateStory(client, CallStyle.Async, "asynchronous responses"),
        CreateStory(client, CallStyle.WithHttpInfoAsync, "asynchronous HTTP responses")
    };

    internal static bool IsExcluded(string methodName)
    {
        var baseName = methodName;
        foreach (var suffix in new[] { "WithHttpInfoAsync", "WithHttpInfo", "Async" })
        {
            if (baseName.EndsWith(suffix, StringComparison.Ordinal))
            {
                baseName = baseName[..^suffix.Length];
                break;
            }
        }

        return ExcludedBaseMethods.Contains(baseName, StringComparer.Ordinal);
    }

    private static Story CreateStory(IPricingClientInvoker client, CallStyle style, string styleDescription) => new()
    {
        Name = "Pricing API - " + styleDescription,
        Description = "Exercises the Pricing and metering lifecycle from the Go and PHP SDK E2E tests.",
        Module = "pricing",
        InitialVariables = Variables(),
        SetupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
        CleanupAsync = (_, cancellationToken) => CleanupAsync(client, cancellationToken),
        Steps = CreateSteps(client, style)
    };

    private static IReadOnlyList<Step> CreateSteps(IPricingClientInvoker client, CallStyle style)
    {
        var specs = new List<StepSpec>();

        foreach (var method in Lifecycle)
        {
            specs.Add(new StepSpec(method, method));
        }

        // The lifecycle creates and reads both a fixed and a tiered-usage unit.
        // They use the same generated operation, so override only the affected
        // argument and validation functions for the second occurrence.
        specs[3] = new StepSpec(
            "CreatePricingUnit",
            "CreatePricingUnit (tiered usage)",
            context => Arguments(("body", TieredUsagePricingUnit(context))),
            ValidatePricingUnit("tiered_usage"),
            StorePricingUnit("tiered_pricing_unit_id"));
        specs[6] = new StepSpec(
            "GetPricingUnit",
            "GetPricingUnit (tiered usage)",
            context => Arguments(
                ("tiered_pricing_unit_id", context.GetRequired<string>("tiered_pricing_unit_id"))),
            ValidatePricingUnit("tiered_usage"));
        specs[16] = new StepSpec(
            "UpdatePricingUnit",
            "UpdatePricingUnit (tiered usage)",
            context => Arguments(
                ("tiered_pricing_unit_id", context.GetRequired<string>("tiered_pricing_unit_id")),
                ("body", TieredUsagePricingUnit(context, updated: true))));

        return specs.Select(spec => CreateStep(client, style, spec)).ToArray();
    }

    private static Step CreateStep(IPricingClientInvoker client, CallStyle style, StepSpec spec)
    {
        var operation = OperationByMethod[spec.Method];
        var methodName = operation.MethodFor(style);
        var arguments = spec.Arguments ?? operation.Arguments;
        var validation = spec.Validate ?? operation.Validate;
        var updateState = spec.UpdateState ?? operation.UpdateState;

        Func<TestContext, CancellationToken, Task<ExecutionResult>> execute = style switch
        {
            CallStyle.Sync => MethodExecutor.Sync(context => client.Invoke(methodName, arguments(context).Values)),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(context =>
                PricingApiClient.ToExecutionResult(client.Invoke(methodName, arguments(context).Values))),
            CallStyle.Async => MethodExecutor.Async((context, cancellationToken) =>
                client.InvokeAsync(methodName, arguments(context).Values, cancellationToken)),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(async (context, cancellationToken) =>
                PricingApiClient.ToExecutionResult(
                    await client.InvokeAsync(methodName, arguments(context).Values, cancellationToken)
                        .ConfigureAwait(false))),
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        };

        // A plain model or void call discards the HTTP status, so only the HTTP-info styles
        // can check a success code. Expected error statuses stay in place because the
        // generated clients surface them through ApiException in every call style.
        var expectsErrorStatus = operation.ExpectedStatus is < 200 or >= 300;

        return new Step
        {
            Name = spec.Name,
            Method = methodName,
            CallStyle = style,
            Parameters = (Func<TestContext, object?>)(context => arguments(context).Named),
            ExpectedStatus = IsHttpInfo(style) || expectsErrorStatus ? operation.ExpectedStatus : null,
            ExecuteAsync = execute,
            ValidateAsync = validation,
            UpdateStateAsync = updateState
        };
    }

    private static async Task CleanupAsync(IPricingClientInvoker client, CancellationToken cancellationToken)
    {
        await client.InvokeAsync(
                "DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRatesAsync",
                Arguments().Values,
                cancellationToken)
            .ConfigureAwait(false);
    }

    private static PricingOperation Operation(
        string method,
        Func<TestContext, PricingArguments> arguments,
        int expectedStatus,
        Func<ExecutionResult, TestContext, Task>? validate = null,
        Func<ExecutionResult, TestContext, Task>? updateState = null) =>
        new(method, arguments, expectedStatus, validate, updateState);

    /// <summary>
    /// Builds the positional argument array plus its named view. The trailing zero is the
    /// generated <c>operationIndex</c> parameter, which is an implementation detail of the
    /// client and therefore stays out of the snapshot parameters.
    /// </summary>
    private static PricingArguments Arguments(params (string Name, object? Value)[] arguments) =>
        new(
            arguments.Select(argument => argument.Value).Append(OperationIndex).ToArray(),
            arguments.ToDictionary(
                argument => argument.Name, argument => argument.Value, StringComparer.Ordinal));

    private static Func<TestContext, PricingArguments> NoArguments() => _ => Arguments();

    private static IReadOnlyDictionary<string, object?> Variables()
    {
        var now = DateTimeOffset.UtcNow;
        var dayStart = new DateTimeOffset(now.UtcDateTime.Date, TimeSpan.Zero);
        var dayEnd = dayStart.AddDays(1).AddSeconds(-1);
        var suffix = Guid.NewGuid().ToString("N");

        return new Dictionary<string, object?>
        {
            ["tenant_id"] = Environment.GetEnvironmentVariable("TEST_TENANT_ID") ?? "test-tenant-id",
            ["metering_unit_name"] = "csharp_e2e_meter_" + suffix,
            ["pricing_unit_name"] = "csharp_e2e_unit_" + suffix,
            ["tiered_pricing_unit_name"] = "csharp_e2e_tiered_usage_unit_" + suffix,
            ["pricing_menu_name"] = "csharp_e2e_menu_" + suffix,
            ["pricing_plan_name"] = "csharp_e2e_plan_" + suffix,
            ["tax_rate_name"] = "csharp_e2e_tax_" + suffix,
            ["timestamp"] = checked((int)now.ToUnixTimeSeconds()),
            ["date"] = now.ToString("yyyy-MM-dd"),
            ["month"] = now.ToString("yyyy-MM"),
            // Named after the generated query parameters so snapshots normalise them.
            ["start_timestamp"] = checked((int)dayStart.ToUnixTimeSeconds()),
            ["end_timestamp"] = checked((int)dayEnd.ToUnixTimeSeconds())
        };
    }

    private static PricingUnitForSave FixedPricingUnit(TestContext context, bool updated = false) =>
        new(new PricingFixedUnitForSave(
            context.GetRequired<string>("pricing_unit_name"),
            updated ? "C# E2E Unit Updated" : "C# E2E Unit",
            updated ? "Updated by the C# SDK E2E test." : "Created by the C# SDK E2E test.",
            UnitType.Fixed,
            Currency.JPY,
            1000,
            RecurringInterval.Month));

    private static PricingUnitForSave TieredUsagePricingUnit(TestContext context, bool updated = false) =>
        new(new PricingTieredUsageUnitForSave(
            context.GetRequired<string>("tiered_pricing_unit_name"),
            updated ? "C# E2E Tiered Usage Unit Updated" : "C# E2E Tiered Usage Unit",
            updated
                ? "Tiered usage unit updated by the C# SDK E2E test."
                : "Tiered usage unit created by the C# SDK E2E test.",
            UnitType.TieredUsage,
            Currency.JPY,
            new List<PricingTier> { new(0, 500, 400, true) },
            5000,
            context.GetRequired<string>("metering_unit_name"),
            AggregateUsage.Max));

    private static SavePricingMenuParam Menu(TestContext context, bool updated = false) =>
        new(
            context.GetRequired<string>("pricing_menu_name"),
            updated ? "C# E2E Menu Updated" : "C# E2E Menu",
            updated ? "Updated by the C# SDK E2E test." : "Created by the C# SDK E2E test.",
            new List<string>
            {
                context.GetRequired<string>("pricing_unit_id"),
                context.GetRequired<string>("tiered_pricing_unit_id")
            });

    private static SavePricingPlanParam Plan(TestContext context, bool updated = false) =>
        new(
            context.GetRequired<string>("pricing_plan_name"),
            updated ? "C# E2E Plan Updated" : "C# E2E Plan",
            updated ? "Updated by the C# SDK E2E test." : "Created by the C# SDK E2E test.",
            new List<string> { context.GetRequired<string>("pricing_menu_id") });

    private static MeteringUnitProps MeteringUnit(TestContext context, bool updated = false) =>
        new(
            context.GetRequired<string>("metering_unit_name"),
            AggregateUsage.Max,
            updated ? "C# E2E Meter Updated" : "C# E2E Meter",
            updated ? "Updated by the C# SDK E2E test." : "Created by the C# SDK E2E test.");

    private static TaxRateProps TaxRate(TestContext context) =>
        new(
            context.GetRequired<string>("tax_rate_name"),
            "C# E2E Tax",
            10m,
            true,
            "JP",
            "Tax rate created by the C# SDK E2E test.");

    private static Func<ExecutionResult, TestContext, Task> ValidatePricingUnit(string expectedType) =>
        (result, _) =>
        {
            RequireModel<PricingUnit>(result, "PricingUnit", pricingUnit =>
                ValidatePricingUnitShape(pricingUnit, expectedType));
            return Task.CompletedTask;
        };

    private static Task ValidatePricingUnits(ExecutionResult result, TestContext _)
    {
        RequireModel<PricingUnits>(result, "PricingUnits", response =>
            RequireCollection(response.Units, "PricingUnits.units", ValidatePricingUnitShape));
        return Task.CompletedTask;
    }

    private static Task ValidatePricingMenu(ExecutionResult result, TestContext _)
    {
        RequireModel<PricingMenu>(result, "PricingMenu", ValidatePricingMenuShape);
        return Task.CompletedTask;
    }

    private static Task ValidatePricingMenus(ExecutionResult result, TestContext _)
    {
        RequireModel<PricingMenus>(result, "PricingMenus", response =>
            RequireCollection(response.VarPricingMenus, "PricingMenus.pricing_menus", ValidatePricingMenuShape));
        return Task.CompletedTask;
    }

    private static Task ValidatePricingPlan(ExecutionResult result, TestContext _)
    {
        RequireModel<PricingPlan>(result, "PricingPlan", ValidatePricingPlanShape);
        return Task.CompletedTask;
    }

    private static Task ValidatePricingPlans(ExecutionResult result, TestContext _)
    {
        RequireModel<PricingPlans>(result, "PricingPlans", response =>
            RequireCollection(response.VarPricingPlans, "PricingPlans.pricing_plans", ValidatePricingPlanShape));
        return Task.CompletedTask;
    }

    private static Task ValidateMeteringUnit(ExecutionResult result, TestContext _)
    {
        RequireModel<MeteringUnit>(result, "MeteringUnit", ValidateMeteringUnitShape);
        return Task.CompletedTask;
    }

    private static Task ValidateMeteringUnits(ExecutionResult result, TestContext _)
    {
        RequireModel<MeteringUnits>(result, "MeteringUnits", response =>
            RequireCollection(response.Units, "MeteringUnits.units", ValidateMeteringUnitShape));
        return Task.CompletedTask;
    }

    private static Task ValidateTaxRate(ExecutionResult result, TestContext _)
    {
        RequireModel<TaxRate>(result, "TaxRate", ValidateTaxRateShape);
        return Task.CompletedTask;
    }

    private static Task ValidateTaxRates(ExecutionResult result, TestContext _)
    {
        RequireModel<TaxRates>(result, "TaxRates", response =>
            RequireCollection(response.VarTaxRates, "TaxRates.tax_rates", ValidateTaxRateShape));
        return Task.CompletedTask;
    }

    private static Task ValidateMeteringUnitDateCount(ExecutionResult result, TestContext _)
    {
        RequireModel<MeteringUnitDateCount>(result, "MeteringUnitDateCount", ValidateMeteringUnitDateCountShape);
        return Task.CompletedTask;
    }

    private static Task ValidateMeteringUnitTimestampCount(ExecutionResult result, TestContext _)
    {
        RequireModel<MeteringUnitTimestampCount>(result, "MeteringUnitTimestampCount", ValidateMeteringUnitTimestampCountShape);
        return Task.CompletedTask;
    }

    private static Task ValidateMeteringUnitMonthCount(ExecutionResult result, TestContext _)
    {
        RequireModel<MeteringUnitMonthCount>(result, "MeteringUnitMonthCount", ValidateMeteringUnitMonthCountShape);
        return Task.CompletedTask;
    }

    private static Task ValidateMeteringUnitDateCounts(ExecutionResult result, TestContext _)
    {
        RequireModel<MeteringUnitDateCounts>(result, "MeteringUnitDateCounts", response =>
            RequireCollection(response.Counts, "MeteringUnitDateCounts.counts", ValidateMeteringUnitDateCountShape));
        return Task.CompletedTask;
    }

    private static Task ValidateMeteringUnitMonthCounts(ExecutionResult result, TestContext _)
    {
        RequireModel<MeteringUnitMonthCounts>(result, "MeteringUnitMonthCounts", response =>
            RequireCollection(response.Counts, "MeteringUnitMonthCounts.counts", ValidateMeteringUnitMonthCountShape));
        return Task.CompletedTask;
    }

    private static Task ValidateMeteringUnitDatePeriodCounts(ExecutionResult result, TestContext _)
    {
        RequireModel<MeteringUnitDatePeriodCounts>(result, "MeteringUnitDatePeriodCounts", response =>
        {
            RequireText(response.MeteringUnitName, "metering_unit_name", "MeteringUnitDatePeriodCounts");
            RequireCollection(response.Counts, "MeteringUnitDatePeriodCounts.counts", ValidateMeteringUnitCountShape);
        });
        return Task.CompletedTask;
    }

    private static void ValidatePricingUnitShape(PricingUnit pricingUnit) =>
        ValidatePricingUnitShape(pricingUnit, expectedType: null);

    private static void ValidatePricingUnitShape(PricingUnit pricingUnit, string? expectedType)
    {
        if (pricingUnit.ActualInstance is not { } actualInstance)
            throw new InvalidOperationException("Pricing response did not contain a Pricing Unit payload.");

        var actualType = actualInstance switch
        {
            PricingFixedUnit => "fixed",
            PricingTieredUsageUnit => "tiered_usage",
            PricingTieredUnit => "tiered",
            PricingUsageUnit => "usage",
            _ => string.Empty
        };
        if (string.IsNullOrWhiteSpace(actualType))
            throw new InvalidOperationException("Pricing response contained an unknown Pricing Unit type.");
        if (expectedType is not null && !string.Equals(actualType, expectedType, StringComparison.Ordinal))
            throw new InvalidOperationException(
                $"Expected Pricing Unit type {expectedType}, but received {actualType}.");

        switch (actualInstance)
        {
            case PricingFixedUnit fixedUnit:
                ValidatePricingUnitFields(fixedUnit.Id, fixedUnit.Name, fixedUnit.DisplayName,
                    fixedUnit.Description, actualType);
                break;
            case PricingTieredUsageUnit tieredUsageUnit:
                ValidatePricingUnitFields(tieredUsageUnit.Id, tieredUsageUnit.Name, tieredUsageUnit.DisplayName,
                    tieredUsageUnit.Description, actualType);
                break;
            case PricingTieredUnit tieredUnit:
                ValidatePricingUnitFields(tieredUnit.Id, tieredUnit.Name, tieredUnit.DisplayName,
                    tieredUnit.Description, actualType);
                break;
            case PricingUsageUnit usageUnit:
                ValidatePricingUnitFields(usageUnit.Id, usageUnit.Name, usageUnit.DisplayName,
                    usageUnit.Description, actualType);
                break;
        }
    }

    private static void ValidatePricingUnitFields(
        string? id,
        string? name,
        string? displayName,
        string? description,
        string resource)
    {
        RequireId(id, resource + " Pricing Unit");
        RequireText(name, "name", resource + " Pricing Unit");
        RequireText(displayName, "display_name", resource + " Pricing Unit");
        RequireText(description, "description", resource + " Pricing Unit");
    }

    private static void ValidatePricingMenuShape(PricingMenu menu)
    {
        ValidateResourceFields(menu.Id, menu.Name, menu.DisplayName, menu.Description, "Pricing Menu");
        RequireCollection(menu.Units, "PricingMenu.units", ValidatePricingUnitShape);
    }

    private static void ValidatePricingPlanShape(PricingPlan plan)
    {
        ValidateResourceFields(plan.Id, plan.Name, plan.DisplayName, plan.Description, "Pricing Plan");
        RequireCollection(plan.PricingMenus, "PricingPlan.pricing_menus", ValidatePricingMenuShape);
    }

    private static void ValidateMeteringUnitShape(MeteringUnit unit)
    {
        ValidateResourceFields(unit.Id, unit.UnitName, unit.DisplayName, unit.Description, "Metering Unit");
    }

    private static void ValidateTaxRateShape(TaxRate taxRate)
    {
        ValidateResourceFields(taxRate.Id, taxRate.Name, taxRate.DisplayName, taxRate.Description, "Tax Rate");
        RequireText(taxRate.Country, "country", "Tax Rate");
    }

    private static void ValidateMeteringUnitDateCountShape(MeteringUnitDateCount count)
    {
        RequireText(count.MeteringUnitName, "metering_unit_name", "MeteringUnitDateCount");
        RequireText(count.Date, "date", "MeteringUnitDateCount");
    }

    private static void ValidateMeteringUnitTimestampCountShape(MeteringUnitTimestampCount count)
    {
        RequireText(count.MeteringUnitName, "metering_unit_name", "MeteringUnitTimestampCount");
    }

    private static void ValidateMeteringUnitMonthCountShape(MeteringUnitMonthCount count)
    {
        RequireText(count.MeteringUnitName, "metering_unit_name", "MeteringUnitMonthCount");
        RequireText(count.Month, "month", "MeteringUnitMonthCount");
    }

    private static void ValidateMeteringUnitCountShape(MeteringUnitCount count)
    {
        // Timestamp zero and count zero are valid values, so only the model
        // presence is checked for this leaf object.
    }

    private static void ValidateResourceFields(
        string? id,
        string? name,
        string? displayName,
        string? description,
        string resource)
    {
        RequireId(id, resource);
        RequireText(name, "name", resource);
        RequireText(displayName, "display_name", resource);
        RequireText(description, "description", resource);
    }

    private static void RequireModel<T>(
        ExecutionResult result,
        string resource,
        Action<T>? validate = null)
        where T : class
    {
        if (result.Response is not T model)
            throw new InvalidOperationException($"Expected Pricing API to return {resource}.");

        if (result.StatusCode.HasValue)
            RequireJsonBody(result.Body, resource);
        validate?.Invoke(model);
    }

    private static void RequireCollection<T>(
        IEnumerable<T>? values,
        string resource,
        Action<T>? validate = null)
        where T : class
    {
        if (values is null)
            throw new InvalidOperationException($"Pricing response did not contain {resource}.");

        foreach (var value in values)
        {
            if (value is null)
                throw new InvalidOperationException($"Pricing response contained a null item in {resource}.");
            validate?.Invoke(value);
        }
    }

    private static void RequireJsonBody(string? body, string resource)
    {
        if (string.IsNullOrWhiteSpace(body))
            throw new InvalidOperationException($"Pricing {resource} response did not contain a JSON body.");

        JToken token;
        try
        {
            token = JToken.Parse(body);
        }
        catch (Newtonsoft.Json.JsonException error)
        {
            throw new InvalidOperationException($"Pricing {resource} response was not valid JSON.", error);
        }

        if (token.Type is not (JTokenType.Object or JTokenType.Array))
            throw new InvalidOperationException($"Pricing {resource} response was not a JSON object or array.");
    }

    private static Task ValidateOptionalJsonBody(ExecutionResult result, TestContext _)
    {
        if (!string.IsNullOrWhiteSpace(result.Body))
            RequireJsonBody(result.Body, "API");
        return Task.CompletedTask;
    }

    private static void RequireText(string? value, string property, string resource)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException(
                $"{resource} response did not contain a non-empty {property}.");
    }

    private static Func<ExecutionResult, TestContext, Task> StorePricingUnit(string variableName) =>
        (result, context) =>
        {
            if (result.Response is not PricingUnit pricingUnit ||
                pricingUnit.ActualInstance is not { } actualInstance)
                throw new InvalidOperationException("Pricing response did not contain a Pricing Unit.");

            var id = actualInstance switch
            {
                PricingFixedUnit unit => unit.Id,
                PricingTieredUsageUnit unit => unit.Id,
                PricingTieredUnit unit => unit.Id,
                PricingUsageUnit unit => unit.Id,
                _ => string.Empty
            };
            context.Variables[variableName] = RequireId(id, "Pricing Unit");
            return Task.CompletedTask;
        };

    private static Task StoreMeteringUnit(ExecutionResult result, TestContext context)
    {
        if (result.Response is not MeteringUnit meteringUnit)
            throw new InvalidOperationException("Pricing response did not contain a Metering Unit.");
        context.Variables["metering_unit_id"] = RequireId(meteringUnit.Id, "Metering Unit");
        return Task.CompletedTask;
    }

    private static Task StorePricingMenu(ExecutionResult result, TestContext context)
    {
        if (result.Response is not PricingMenu menu)
            throw new InvalidOperationException("Pricing response did not contain a Pricing Menu.");
        context.Variables["pricing_menu_id"] = RequireId(menu.Id, "Pricing Menu");
        return Task.CompletedTask;
    }

    private static Task StorePricingPlan(ExecutionResult result, TestContext context)
    {
        if (result.Response is not PricingPlan plan)
            throw new InvalidOperationException("Pricing response did not contain a Pricing Plan.");
        context.Variables["pricing_plan_id"] = RequireId(plan.Id, "Pricing Plan");
        return Task.CompletedTask;
    }

    private static Task StoreTaxRate(ExecutionResult result, TestContext context)
    {
        if (result.Response is not TaxRate taxRate)
            throw new InvalidOperationException("Pricing response did not contain a Tax Rate.");
        context.Variables["tax_rate_id"] = RequireId(taxRate.Id, "Tax Rate");
        return Task.CompletedTask;
    }

    private static Task RequireNotImplemented(ExecutionResult result, TestContext _)
    {
        if (result.StatusCode != 501 || result.Error is null)
            throw new InvalidOperationException("Expected UpdatePricingPlansUsed to return HTTP 501.");
        return Task.CompletedTask;
    }

    private static string RequireId(string? id, string resource)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new InvalidOperationException($"{resource} response did not contain a non-empty id.");
        return id;
    }

    private static bool IsHttpInfo(CallStyle style) =>
        style is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync;

    private sealed record StepSpec(
        string Method,
        string Name,
        Func<TestContext, PricingArguments>? Arguments = null,
        Func<ExecutionResult, TestContext, Task>? Validate = null,
        Func<ExecutionResult, TestContext, Task>? UpdateState = null);
}
