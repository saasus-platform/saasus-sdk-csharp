# pricingapi - the C# library for the SaaSus Pricing API Schema

SaaSus Pricing API Schema

<a id="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.saasus.io/v1/pricing*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*ErrorApi* | [**ReturnInternalServerError**](docs/ErrorApi.md#returninternalservererror) | **GET** /errors/internal-server-error | Return Internal Server Error
*MeteringApi* | [**CreateMeteringUnit**](docs/MeteringApi.md#createmeteringunit) | **POST** /metering/units | Create Metering Unit
*MeteringApi* | [**DeleteMeteringUnitByID**](docs/MeteringApi.md#deletemeteringunitbyid) | **DELETE** /metering/units/{metering_unit_id} | Delete Metering Unit
*MeteringApi* | [**DeleteMeteringUnitTimestampCount**](docs/MeteringApi.md#deletemeteringunittimestampcount) | **DELETE** /metering/tenants/{tenant_id}/units/{metering_unit_name}/timestamp/{timestamp} | Delete Metering Unit Count for Specified Timestamp
*MeteringApi* | [**GetMeteringUnitDateCountByTenantIdAndUnitNameAndDate**](docs/MeteringApi.md#getmeteringunitdatecountbytenantidandunitnameanddate) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/date/{date} | Get Metering Unit Count for Specific Date
*MeteringApi* | [**GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod**](docs/MeteringApi.md#getmeteringunitdatecountbytenantidandunitnameanddateperiod) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/date-period | Obtain metering unit counts for a specified date/time period
*MeteringApi* | [**GetMeteringUnitDateCountByTenantIdAndUnitNameToday**](docs/MeteringApi.md#getmeteringunitdatecountbytenantidandunitnametoday) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/today | Get Metering Unit Count for the Current Day
*MeteringApi* | [**GetMeteringUnitDateCountsByTenantIdAndDate**](docs/MeteringApi.md#getmeteringunitdatecountsbytenantidanddate) | **GET** /metering/tenants/{tenant_id}/units/date/{date} | Get All Metering Unit Counts for a Specified Date
*MeteringApi* | [**GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonth**](docs/MeteringApi.md#getmeteringunitmonthcountbytenantidandunitnameandmonth) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/month/{month} | Get the Metering Unit Count for the Specified Month
*MeteringApi* | [**GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonth**](docs/MeteringApi.md#getmeteringunitmonthcountbytenantidandunitnamethismonth) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/thismonth | Get Metering Unit Count for the Current Month
*MeteringApi* | [**GetMeteringUnitMonthCountsByTenantIdAndMonth**](docs/MeteringApi.md#getmeteringunitmonthcountsbytenantidandmonth) | **GET** /metering/tenants/{tenant_id}/units/month/{month} | Get All Metering Unit Counts for the Specified Month
*MeteringApi* | [**GetMeteringUnits**](docs/MeteringApi.md#getmeteringunits) | **GET** /metering/units | Get all metering units
*MeteringApi* | [**UpdateMeteringUnitByID**](docs/MeteringApi.md#updatemeteringunitbyid) | **PATCH** /metering/units/{metering_unit_id} | Update Metering Unit
*MeteringApi* | [**UpdateMeteringUnitTimestampCount**](docs/MeteringApi.md#updatemeteringunittimestampcount) | **PUT** /metering/tenants/{tenant_id}/units/{metering_unit_name}/timestamp/{timestamp} | Update Metering Unit Count for Specified Timestamp
*MeteringApi* | [**UpdateMeteringUnitTimestampCountNow**](docs/MeteringApi.md#updatemeteringunittimestampcountnow) | **PUT** /metering/tenants/{tenant_id}/units/{metering_unit_name}/now | Update Metering Unit Count for Current Time
*PricingMenusApi* | [**CreatePricingMenu**](docs/PricingMenusApi.md#createpricingmenu) | **POST** /menus | Create a Pricing Feature Menu
*PricingMenusApi* | [**DeletePricingMenu**](docs/PricingMenusApi.md#deletepricingmenu) | **DELETE** /menus/{menu_id} | Delete Pricing Feature Menu
*PricingMenusApi* | [**GetPricingMenu**](docs/PricingMenusApi.md#getpricingmenu) | **GET** /menus/{menu_id} | Get Pricing Feature Menu
*PricingMenusApi* | [**GetPricingMenus**](docs/PricingMenusApi.md#getpricingmenus) | **GET** /menus | Get Pricing Feature Menus
*PricingMenusApi* | [**UpdatePricingMenu**](docs/PricingMenusApi.md#updatepricingmenu) | **PATCH** /menus/{menu_id} | Update Pricing Feature Menu
*PricingPlansApi* | [**CreatePricingPlan**](docs/PricingPlansApi.md#createpricingplan) | **POST** /plans | Create Pricing Plan
*PricingPlansApi* | [**DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRates**](docs/PricingPlansApi.md#deleteallplansandmenusandunitsandmetersandtaxrates) | **DELETE** /plans-initialization | Delete all Plans, Menus, Units, Meters and Tax Rates
*PricingPlansApi* | [**DeletePricingPlan**](docs/PricingPlansApi.md#deletepricingplan) | **DELETE** /plans/{plan_id} | Delete Pricing Plan
*PricingPlansApi* | [**DeleteStripePlan**](docs/PricingPlansApi.md#deletestripeplan) | **DELETE** /stripe | Delete Product Data from Stripe
*PricingPlansApi* | [**GetPricingPlan**](docs/PricingPlansApi.md#getpricingplan) | **GET** /plans/{plan_id} | Get Pricing Plan
*PricingPlansApi* | [**GetPricingPlans**](docs/PricingPlansApi.md#getpricingplans) | **GET** /plans | Get Pricing Plans
*PricingPlansApi* | [**LinkPlanToStripe**](docs/PricingPlansApi.md#linkplantostripe) | **PATCH** /stripe/init | Connect to Stripe
*PricingPlansApi* | [**UpdatePricingPlan**](docs/PricingPlansApi.md#updatepricingplan) | **PATCH** /plans/{plan_id} | Update Pricing Plan
*PricingPlansApi* | [**UpdatePricingPlansUsed**](docs/PricingPlansApi.md#updatepricingplansused) | **PATCH** /plans/used | Update Used Flag
*PricingUnitsApi* | [**CreatePricingUnit**](docs/PricingUnitsApi.md#createpricingunit) | **POST** /units | Create Pricing Unit
*PricingUnitsApi* | [**DeletePricingUnit**](docs/PricingUnitsApi.md#deletepricingunit) | **DELETE** /units/{pricing_unit_id} | Delete Pricing Unit
*PricingUnitsApi* | [**GetPricingUnit**](docs/PricingUnitsApi.md#getpricingunit) | **GET** /units/{pricing_unit_id} | Get Pricing Unit
*PricingUnitsApi* | [**GetPricingUnits**](docs/PricingUnitsApi.md#getpricingunits) | **GET** /units | Get Pricing Units
*PricingUnitsApi* | [**UpdatePricingUnit**](docs/PricingUnitsApi.md#updatepricingunit) | **PATCH** /units/{pricing_unit_id} | Update Pricing Unit
*TaxRateApi* | [**CreateTaxRate**](docs/TaxRateApi.md#createtaxrate) | **POST** /tax-rates | Create Tax Rate
*TaxRateApi* | [**GetTaxRates**](docs/TaxRateApi.md#gettaxrates) | **GET** /tax-rates | Get Tax Rates
*TaxRateApi* | [**UpdateTaxRate**](docs/TaxRateApi.md#updatetaxrate) | **PATCH** /tax-rates/{tax_rate_id} | Update Tax Rate


<a id="documentation-for-models"></a>
## Documentation for Models

 - [Model.AggregateUsage](docs/AggregateUsage.md)
 - [Model.Currency](docs/Currency.md)
 - [Model.Error](docs/Error.md)
 - [Model.MeteringUnit](docs/MeteringUnit.md)
 - [Model.MeteringUnitCount](docs/MeteringUnitCount.md)
 - [Model.MeteringUnitDateCount](docs/MeteringUnitDateCount.md)
 - [Model.MeteringUnitDateCounts](docs/MeteringUnitDateCounts.md)
 - [Model.MeteringUnitDatePeriodCounts](docs/MeteringUnitDatePeriodCounts.md)
 - [Model.MeteringUnitMonthCount](docs/MeteringUnitMonthCount.md)
 - [Model.MeteringUnitMonthCounts](docs/MeteringUnitMonthCounts.md)
 - [Model.MeteringUnitProps](docs/MeteringUnitProps.md)
 - [Model.MeteringUnitTimestampCount](docs/MeteringUnitTimestampCount.md)
 - [Model.MeteringUnits](docs/MeteringUnits.md)
 - [Model.PricingFixedUnit](docs/PricingFixedUnit.md)
 - [Model.PricingFixedUnitForSave](docs/PricingFixedUnitForSave.md)
 - [Model.PricingMenu](docs/PricingMenu.md)
 - [Model.PricingMenuProps](docs/PricingMenuProps.md)
 - [Model.PricingMenus](docs/PricingMenus.md)
 - [Model.PricingPlan](docs/PricingPlan.md)
 - [Model.PricingPlanProps](docs/PricingPlanProps.md)
 - [Model.PricingPlans](docs/PricingPlans.md)
 - [Model.PricingTier](docs/PricingTier.md)
 - [Model.PricingTieredUnit](docs/PricingTieredUnit.md)
 - [Model.PricingTieredUnitForSave](docs/PricingTieredUnitForSave.md)
 - [Model.PricingTieredUsageUnit](docs/PricingTieredUsageUnit.md)
 - [Model.PricingTieredUsageUnitForSave](docs/PricingTieredUsageUnitForSave.md)
 - [Model.PricingTiers](docs/PricingTiers.md)
 - [Model.PricingUnit](docs/PricingUnit.md)
 - [Model.PricingUnitBaseProps](docs/PricingUnitBaseProps.md)
 - [Model.PricingUnitForSave](docs/PricingUnitForSave.md)
 - [Model.PricingUnits](docs/PricingUnits.md)
 - [Model.PricingUsageUnit](docs/PricingUsageUnit.md)
 - [Model.PricingUsageUnitForSave](docs/PricingUsageUnitForSave.md)
 - [Model.RecurringInterval](docs/RecurringInterval.md)
 - [Model.SavePricingMenuParam](docs/SavePricingMenuParam.md)
 - [Model.SavePricingPlanParam](docs/SavePricingPlanParam.md)
 - [Model.TaxRate](docs/TaxRate.md)
 - [Model.TaxRateProps](docs/TaxRateProps.md)
 - [Model.TaxRates](docs/TaxRates.md)
 - [Model.UnitType](docs/UnitType.md)
 - [Model.UpdateMeteringUnitTimestampCountMethod](docs/UpdateMeteringUnitTimestampCountMethod.md)
 - [Model.UpdateMeteringUnitTimestampCountNowParam](docs/UpdateMeteringUnitTimestampCountNowParam.md)
 - [Model.UpdateMeteringUnitTimestampCountParam](docs/UpdateMeteringUnitTimestampCountParam.md)
 - [Model.UpdatePricingPlansUsedParam](docs/UpdatePricingPlansUsedParam.md)
 - [Model.UpdateTaxRateParam](docs/UpdateTaxRateParam.md)


<a id="documentation-for-authorization"></a>
## Documentation for Authorization


Authentication schemes defined for the API:
<a id="Bearer"></a>
### Bearer

- **Type**: Bearer Authentication

**Note**:
This API automatically handles Bearer token authentication. You do not need to manually configure or include Bearer tokens in your requests.


