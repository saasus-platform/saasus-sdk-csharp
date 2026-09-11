using System.Net;
using Newtonsoft.Json;
using pricingapi.Client;
using pricingapi.Model;

namespace SaasusSdk.Tests.E2E.Pricing;

/// <summary>
/// In-memory Pricing API used by the story tests. It intentionally implements
/// the same lifecycle semantics as the live story, including the currently
/// unimplemented 501 UpdatePricingPlansUsed endpoint.
/// </summary>
internal sealed class FakePricingClient : IPricingClientInvoker
{
    private readonly Dictionary<string, MeteringUnit> _meteringUnits = new(StringComparer.Ordinal);
    private readonly Dictionary<string, PricingUnit> _pricingUnits = new(StringComparer.Ordinal);
    private readonly Dictionary<string, PricingMenu> _pricingMenus = new(StringComparer.Ordinal);
    private readonly Dictionary<string, PricingPlan> _pricingPlans = new(StringComparer.Ordinal);
    private readonly Dictionary<string, TaxRate> _taxRates = new(StringComparer.Ordinal);
    private readonly Dictionary<(string Tenant, string Unit, int Timestamp), int> _counts = new();
    private int _nextId;

    public bool IsEmpty => _meteringUnits.Count == 0 &&
                           _pricingUnits.Count == 0 &&
                           _pricingMenus.Count == 0 &&
                           _pricingPlans.Count == 0 &&
                           _taxRates.Count == 0 &&
                           _counts.Count == 0;

    public object? Invoke(string methodName, object?[] arguments)
    {
        var baseMethod = BaseMethod(methodName);
        if (baseMethod == "UpdatePricingPlansUsed")
            throw new ApiException(501, "UpdatePricingPlansUsed is not implemented in the fixture.");

        return baseMethod switch
        {
            "CreateMeteringUnit" => Result(methodName, CreateMeteringUnit((MeteringUnitProps)arguments[0]!), 201),
            "GetMeteringUnits" => Result(methodName, new MeteringUnits(_meteringUnits.Values.ToList()), 200),
            "UpdateMeteringUnitByID" => UpdateMeteringUnitById(methodName, arguments),
            "UpdateMeteringUnitTimestampCount" => UpdateTimestampCount(methodName, arguments),
            "UpdateMeteringUnitTimestampCountNow" => UpdateTimestampCountNow(methodName, arguments),
            "GetMeteringUnitDateCountByTenantIdAndUnitNameAndDate" => Result(
                methodName,
                DateCount((string)arguments[0]!, (string)arguments[1]!, (string)arguments[2]!),
                200),
            "GetMeteringUnitDateCountByTenantIdAndUnitNameToday" => Result(
                methodName,
                DateCount((string)arguments[0]!, (string)arguments[1]!, DateTime.UtcNow.ToString("yyyy-MM-dd")),
                200),
            "GetMeteringUnitDateCountsByTenantIdAndDate" => Result(
                methodName,
                DateCounts((string)arguments[0]!, (string)arguments[1]!),
                200),
            "GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod" => Result(
                methodName,
                DatePeriodCounts((string)arguments[0]!, (string)arguments[1]!,
                    (int?)arguments[2], (int?)arguments[3]),
                200),
            "GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonth" => Result(
                methodName,
                MonthCount((string)arguments[0]!, (string)arguments[1]!, (string)arguments[2]!),
                200),
            "GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonth" => Result(
                methodName,
                MonthCount((string)arguments[0]!, (string)arguments[1]!, DateTime.UtcNow.ToString("yyyy-MM")),
                200),
            "GetMeteringUnitMonthCountsByTenantIdAndMonth" => Result(
                methodName,
                MonthCounts((string)arguments[0]!, (string)arguments[1]!),
                200),
            "DeleteMeteringUnitTimestampCount" => DeleteTimestampCount(methodName, arguments),
            "CreatePricingUnit" => Result(methodName, CreatePricingUnit((PricingUnitForSave)arguments[0]!), 201),
            "GetPricingUnits" => Result(methodName, new PricingUnits(_pricingUnits.Values.ToList()), 200),
            "GetPricingUnit" => Result(methodName, _pricingUnits[(string)arguments[0]!], 200),
            "UpdatePricingUnit" => UpdatePricingUnit(methodName, arguments),
            "CreatePricingMenu" => Result(methodName, CreatePricingMenu((SavePricingMenuParam)arguments[0]!), 201),
            "GetPricingMenus" => Result(methodName, new PricingMenus(_pricingMenus.Values.ToList()), 200),
            "GetPricingMenu" => Result(methodName, _pricingMenus[(string)arguments[0]!], 200),
            "UpdatePricingMenu" => UpdatePricingMenu(methodName, arguments),
            "CreatePricingPlan" => Result(methodName, CreatePricingPlan((SavePricingPlanParam)arguments[0]!), 201),
            "GetPricingPlans" => Result(methodName, new PricingPlans(_pricingPlans.Values.ToList()), 200),
            "GetPricingPlan" => Result(methodName, _pricingPlans[(string)arguments[0]!], 200),
            "UpdatePricingPlan" => UpdatePricingPlan(methodName, arguments),
            "GetTaxRates" => Result(methodName, new TaxRates(_taxRates.Values.ToList()), 200),
            "CreateTaxRate" => Result(methodName, CreateTaxRate((TaxRateProps)arguments[0]!), 201),
            "UpdateTaxRate" => UpdateTaxRate(methodName, arguments),
            "DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRates" => DeleteAll(methodName),
            _ => throw new MissingMethodException("Fixture method was not implemented: " + methodName)
        };
    }

    public Task<object?> InvokeAsync(
        string methodName,
        object?[] arguments,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Invoke(methodName, arguments));
    }

    private MeteringUnit CreateMeteringUnit(MeteringUnitProps body)
    {
        var id = NextId("meter");
        var value = new MeteringUnit(body.UnitName, body.AggregateUsage, body.DisplayName, body.Description, id);
        _meteringUnits[id] = value;
        return value;
    }

    private object? UpdateMeteringUnitById(string methodName, object?[] arguments)
    {
        var id = (string)arguments[0]!;
        var body = (MeteringUnitProps)arguments[1]!;
        _meteringUnits[id] = new MeteringUnit(
            body.UnitName, body.AggregateUsage, body.DisplayName, body.Description, id);
        return VoidOrResponse(methodName, 200);
    }

    private object? UpdateTimestampCount(string methodName, object?[] arguments)
    {
        var tenant = (string)arguments[0]!;
        var unit = (string)arguments[1]!;
        var timestamp = (int)arguments[2]!;
        var parameter = (UpdateMeteringUnitTimestampCountParam)arguments[3]!;
        var count = ApplyCount(tenant, unit, timestamp, parameter.Method, parameter.Count);
        return Result(methodName, new MeteringUnitTimestampCount(unit, timestamp, count), 200);
    }

    private object? UpdateTimestampCountNow(string methodName, object?[] arguments)
    {
        var tenant = (string)arguments[0]!;
        var unit = (string)arguments[1]!;
        var timestamp = checked((int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        var parameter = (UpdateMeteringUnitTimestampCountNowParam)arguments[2]!;
        var count = ApplyCount(tenant, unit, timestamp, parameter.Method, parameter.Count);
        return Result(methodName, new MeteringUnitTimestampCount(unit, timestamp, count), 200);
    }

    private object? DeleteTimestampCount(string methodName, object?[] arguments)
    {
        _counts.Remove(((string)arguments[0]!, (string)arguments[1]!, (int)arguments[2]!));
        return VoidOrResponse(methodName, 200);
    }

    private MeteringUnitDateCount DateCount(string tenant, string unit, string date)
    {
        var count = _counts
            .Where(pair => pair.Key.Tenant == tenant && pair.Key.Unit == unit)
            .Where(pair => DateTimeOffset.FromUnixTimeSeconds(pair.Key.Timestamp).UtcDateTime.ToString("yyyy-MM-dd") == date)
            .Sum(pair => pair.Value);
        return new MeteringUnitDateCount(unit, date, count);
    }

    private MeteringUnitDateCounts DateCounts(string tenant, string date) =>
        new(_counts
            .Where(pair => pair.Key.Tenant == tenant)
            .GroupBy(pair => pair.Key.Unit, StringComparer.Ordinal)
            .Select(group => new MeteringUnitDateCount(
                group.Key,
                date,
                group.Where(pair => DateTimeOffset.FromUnixTimeSeconds(pair.Key.Timestamp).UtcDateTime.ToString("yyyy-MM-dd") == date)
                    .Sum(pair => pair.Value)))
            .ToList());

    private MeteringUnitDatePeriodCounts DatePeriodCounts(
        string tenant,
        string unit,
        int? startTimestamp,
        int? endTimestamp)
    {
        var counts = _counts
            .Where(pair => pair.Key.Tenant == tenant && pair.Key.Unit == unit)
            .Where(pair => !startTimestamp.HasValue || pair.Key.Timestamp >= startTimestamp.Value)
            .Where(pair => !endTimestamp.HasValue || pair.Key.Timestamp <= endTimestamp.Value)
            .Select(pair => new MeteringUnitCount(pair.Key.Timestamp, pair.Value))
            .ToList();
        return new MeteringUnitDatePeriodCounts(unit, counts);
    }

    private MeteringUnitMonthCount MonthCount(string tenant, string unit, string month)
    {
        var count = _counts
            .Where(pair => pair.Key.Tenant == tenant && pair.Key.Unit == unit)
            .Where(pair => DateTimeOffset.FromUnixTimeSeconds(pair.Key.Timestamp).UtcDateTime.ToString("yyyy-MM") == month)
            .Sum(pair => pair.Value);
        return new MeteringUnitMonthCount(unit, month, count);
    }

    private MeteringUnitMonthCounts MonthCounts(string tenant, string month) =>
        new(_counts
            .Where(pair => pair.Key.Tenant == tenant)
            .GroupBy(pair => pair.Key.Unit, StringComparer.Ordinal)
            .Select(group => new MeteringUnitMonthCount(
                group.Key,
                month,
                group.Where(pair => DateTimeOffset.FromUnixTimeSeconds(pair.Key.Timestamp).UtcDateTime.ToString("yyyy-MM") == month)
                    .Sum(pair => pair.Value)))
            .ToList());

    private int ApplyCount(
        string tenant,
        string unit,
        int timestamp,
        UpdateMeteringUnitTimestampCountMethod method,
        int value)
    {
        var key = (tenant, unit, timestamp);
        var current = _counts.TryGetValue(key, out var existing) ? existing : 0;
        var updated = method switch
        {
            UpdateMeteringUnitTimestampCountMethod.Add => current + value,
            UpdateMeteringUnitTimestampCountMethod.Sub => current - value,
            UpdateMeteringUnitTimestampCountMethod.Direct => value,
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
        };
        _counts[key] = updated;
        return updated;
    }

    private PricingUnit CreatePricingUnit(PricingUnitForSave body)
    {
        var id = NextId("unit");
        var value = ToPricingUnit(body.ActualInstance, id);
        _pricingUnits[id] = value;
        return value;
    }

    private object? UpdatePricingUnit(string methodName, object?[] arguments)
    {
        var id = (string)arguments[0]!;
        var body = (PricingUnitForSave)arguments[1]!;
        _pricingUnits[id] = ToPricingUnit(body.ActualInstance, id);
        return VoidOrResponse(methodName, 200);
    }

    private PricingUnit ToPricingUnit(object actualInstance, string id) => actualInstance switch
    {
        PricingFixedUnitForSave unit => new PricingUnit(new PricingFixedUnit(
            unit.UnitAmount, unit.RecurringInterval, unit.Name, unit.DisplayName, unit.Description,
            unit.Type, unit.Currency, id)),
        PricingTieredUsageUnitForSave unit => new PricingUnit(new PricingTieredUsageUnit(
            unit.UpperCount, unit.MeteringUnitName, unit.AggregateUsage, unit.Name, unit.DisplayName,
            unit.Description, unit.Type, unit.Currency, unit.Tiers, id,
            MeteringUnitId(unit.MeteringUnitName), RecurringInterval.Month)),
        PricingTieredUnitForSave unit => new PricingUnit(new PricingTieredUnit(
            unit.UpperCount, unit.MeteringUnitName, unit.AggregateUsage, unit.Name, unit.DisplayName,
            unit.Description, unit.Type, unit.Currency, unit.Tiers, id,
            MeteringUnitId(unit.MeteringUnitName), RecurringInterval.Month)),
        PricingUsageUnitForSave unit => new PricingUnit(new PricingUsageUnit(
            unit.UpperCount, unit.UnitAmount, unit.MeteringUnitName, unit.AggregateUsage, unit.Name,
            unit.DisplayName, unit.Description, unit.Type, unit.Currency, id,
            MeteringUnitId(unit.MeteringUnitName), RecurringInterval.Month)),
        _ => throw new ArgumentException("Unsupported Pricing Unit fixture payload.", nameof(actualInstance))
    };

    private string MeteringUnitId(string unitName) =>
        _meteringUnits.Values.SingleOrDefault(unit => unit.UnitName == unitName)?.Id
        ?? throw new InvalidOperationException("Metering unit fixture was not created: " + unitName);

    private PricingMenu CreatePricingMenu(SavePricingMenuParam body)
    {
        var id = NextId("menu");
        var value = new PricingMenu(
            body.Name,
            body.DisplayName,
            body.Description,
            false,
            body.UnitIds.Select(unitId => _pricingUnits[unitId]).ToList(),
            id);
        _pricingMenus[id] = value;
        return value;
    }

    private object? UpdatePricingMenu(string methodName, object?[] arguments)
    {
        var id = (string)arguments[0]!;
        var body = (SavePricingMenuParam)arguments[1]!;
        _pricingMenus[id] = new PricingMenu(
            body.Name,
            body.DisplayName,
            body.Description,
            false,
            body.UnitIds.Select(unitId => _pricingUnits[unitId]).ToList(),
            id);
        return VoidOrResponse(methodName, 200);
    }

    private PricingPlan CreatePricingPlan(SavePricingPlanParam body)
    {
        var id = NextId("plan");
        var value = new PricingPlan(
            body.Name,
            body.DisplayName,
            body.Description,
            false,
            body.MenuIds.Select(menuId => _pricingMenus[menuId]).ToList(),
            id);
        _pricingPlans[id] = value;
        return value;
    }

    private object? UpdatePricingPlan(string methodName, object?[] arguments)
    {
        var id = (string)arguments[0]!;
        var body = (SavePricingPlanParam)arguments[1]!;
        _pricingPlans[id] = new PricingPlan(
            body.Name,
            body.DisplayName,
            body.Description,
            false,
            body.MenuIds.Select(menuId => _pricingMenus[menuId]).ToList(),
            id);
        return VoidOrResponse(methodName, 200);
    }

    private TaxRate CreateTaxRate(TaxRateProps body)
    {
        var id = NextId("tax");
        var value = new TaxRate(
            body.Name, body.DisplayName, body.Percentage, body.Inclusive, body.Country,
            body.Description, id);
        _taxRates[id] = value;
        return value;
    }

    private object? UpdateTaxRate(string methodName, object?[] arguments)
    {
        var id = (string)arguments[0]!;
        var body = (UpdateTaxRateParam)arguments[1]!;
        var previous = _taxRates[id];
        _taxRates[id] = new TaxRate(
            previous.Name,
            body.DisplayName,
            previous.Percentage,
            previous.Inclusive,
            previous.Country,
            body.Description,
            id);
        return VoidOrResponse(methodName, 200);
    }

    private object? DeleteAll(string methodName)
    {
        _meteringUnits.Clear();
        _pricingUnits.Clear();
        _pricingMenus.Clear();
        _pricingPlans.Clear();
        _taxRates.Clear();
        _counts.Clear();
        return VoidOrResponse(methodName, 200);
    }

    private string NextId(string prefix) => prefix + "-" + (++_nextId).ToString();

    private static string BaseMethod(string methodName)
    {
        foreach (var suffix in new[] { "WithHttpInfoAsync", "WithHttpInfo", "Async" })
        {
            if (methodName.EndsWith(suffix, StringComparison.Ordinal))
                return methodName[..^suffix.Length];
        }

        return methodName;
    }

    private static bool IsWithHttpInfo(string methodName) =>
        methodName.Contains("WithHttpInfo", StringComparison.Ordinal);

    private static object? Result(string methodName, object? value, int statusCode)
    {
        if (!IsWithHttpInfo(methodName)) return value;
        return value switch
        {
            MeteringUnit item => Response(item, statusCode),
            MeteringUnits items => Response(items, statusCode),
            MeteringUnitDateCount item => Response(item, statusCode),
            MeteringUnitDateCounts items => Response(items, statusCode),
            MeteringUnitDatePeriodCounts items => Response(items, statusCode),
            MeteringUnitMonthCount item => Response(item, statusCode),
            MeteringUnitMonthCounts items => Response(items, statusCode),
            MeteringUnitTimestampCount item => Response(item, statusCode),
            PricingUnit item => Response(item, statusCode),
            PricingUnits items => Response(items, statusCode),
            PricingMenu item => Response(item, statusCode),
            PricingMenus items => Response(items, statusCode),
            PricingPlan item => Response(item, statusCode),
            PricingPlans items => Response(items, statusCode),
            TaxRate item => Response(item, statusCode),
            TaxRates items => Response(items, statusCode),
            _ => Response(value, statusCode)
        };
    }

    private static object Response<T>(T value, int statusCode) =>
        new ApiResponse<T>(
            (HttpStatusCode)statusCode,
            new Multimap<string, string>(),
            value,
            JsonConvert.SerializeObject(value));

    private static object? VoidOrResponse(string methodName, int statusCode) =>
        IsWithHttpInfo(methodName)
            ? new ApiResponse<object>(
                (HttpStatusCode)statusCode,
                new Multimap<string, string>(),
                null!,
                string.Empty)
            : null;
}
