# pricingapi.Api.MeteringApi

All URIs are relative to *https://api.saasus.io/v1/pricing*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateMeteringUnit**](MeteringApi.md#createmeteringunit) | **POST** /metering/units | Create Metering Unit |
| [**DeleteMeteringUnitByID**](MeteringApi.md#deletemeteringunitbyid) | **DELETE** /metering/units/{metering_unit_id} | Delete Metering Unit |
| [**DeleteMeteringUnitTimestampCount**](MeteringApi.md#deletemeteringunittimestampcount) | **DELETE** /metering/tenants/{tenant_id}/units/{metering_unit_name}/timestamp/{timestamp} | Delete Metering Unit Count for Specified Timestamp |
| [**GetMeteringUnitDateCountByTenantIdAndUnitNameAndDate**](MeteringApi.md#getmeteringunitdatecountbytenantidandunitnameanddate) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/date/{date} | Get Metering Unit Count for Specific Date |
| [**GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod**](MeteringApi.md#getmeteringunitdatecountbytenantidandunitnameanddateperiod) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/date-period | Obtain metering unit counts for a specified date/time period |
| [**GetMeteringUnitDateCountByTenantIdAndUnitNameToday**](MeteringApi.md#getmeteringunitdatecountbytenantidandunitnametoday) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/today | Get Metering Unit Count for the Current Day |
| [**GetMeteringUnitDateCountsByTenantIdAndDate**](MeteringApi.md#getmeteringunitdatecountsbytenantidanddate) | **GET** /metering/tenants/{tenant_id}/units/date/{date} | Get All Metering Unit Counts for a Specified Date |
| [**GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonth**](MeteringApi.md#getmeteringunitmonthcountbytenantidandunitnameandmonth) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/month/{month} | Get the Metering Unit Count for the Specified Month |
| [**GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonth**](MeteringApi.md#getmeteringunitmonthcountbytenantidandunitnamethismonth) | **GET** /metering/tenants/{tenant_id}/units/{metering_unit_name}/thismonth | Get Metering Unit Count for the Current Month |
| [**GetMeteringUnitMonthCountsByTenantIdAndMonth**](MeteringApi.md#getmeteringunitmonthcountsbytenantidandmonth) | **GET** /metering/tenants/{tenant_id}/units/month/{month} | Get All Metering Unit Counts for the Specified Month |
| [**GetMeteringUnits**](MeteringApi.md#getmeteringunits) | **GET** /metering/units | Get all metering units |
| [**UpdateMeteringUnitByID**](MeteringApi.md#updatemeteringunitbyid) | **PATCH** /metering/units/{metering_unit_id} | Update Metering Unit |
| [**UpdateMeteringUnitTimestampCount**](MeteringApi.md#updatemeteringunittimestampcount) | **PUT** /metering/tenants/{tenant_id}/units/{metering_unit_name}/timestamp/{timestamp} | Update Metering Unit Count for Specified Timestamp |
| [**UpdateMeteringUnitTimestampCountNow**](MeteringApi.md#updatemeteringunittimestampcountnow) | **PUT** /metering/tenants/{tenant_id}/units/{metering_unit_name}/now | Update Metering Unit Count for Current Time |

<a id="createmeteringunit"></a>
# **CreateMeteringUnit**
> MeteringUnit CreateMeteringUnit (MeteringUnitProps body = null)

Create Metering Unit

Create a metering unit. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class CreateMeteringUnitExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var body = new MeteringUnitProps();  // MeteringUnitProps |  (optional) 

            try
            {
                // Create Metering Unit
                MeteringUnit result = apiInstance.CreateMeteringUnit(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.CreateMeteringUnit: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateMeteringUnitWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Metering Unit
    ApiResponse<MeteringUnit> response = apiInstance.CreateMeteringUnitWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.CreateMeteringUnitWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **MeteringUnitProps** |  | [optional]  |

### Return type

[**MeteringUnit**](MeteringUnit.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | Created |  -  |
| **400** | Bad Request |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletemeteringunitbyid"></a>
# **DeleteMeteringUnitByID**
> void DeleteMeteringUnitByID (string meteringUnitId)

Delete Metering Unit

Delete metering unit. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class DeleteMeteringUnitByIDExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var meteringUnitId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Metering Unit ID

            try
            {
                // Delete Metering Unit
                apiInstance.DeleteMeteringUnitByID(meteringUnitId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.DeleteMeteringUnitByID: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteMeteringUnitByIDWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Metering Unit
    apiInstance.DeleteMeteringUnitByIDWithHttpInfo(meteringUnitId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.DeleteMeteringUnitByIDWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **meteringUnitId** | **string** | Metering Unit ID |  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletemeteringunittimestampcount"></a>
# **DeleteMeteringUnitTimestampCount**
> void DeleteMeteringUnitTimestampCount (string tenantId, string meteringUnitName, int timestamp)

Delete Metering Unit Count for Specified Timestamp

Deletes metering unit count for the specified timestamp. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class DeleteMeteringUnitTimestampCountExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var meteringUnitName = storage_unit;  // string | Metering Unit Name
            var timestamp = 1640995200;  // int | Timestamp

            try
            {
                // Delete Metering Unit Count for Specified Timestamp
                apiInstance.DeleteMeteringUnitTimestampCount(tenantId, meteringUnitName, timestamp);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.DeleteMeteringUnitTimestampCount: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteMeteringUnitTimestampCountWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Metering Unit Count for Specified Timestamp
    apiInstance.DeleteMeteringUnitTimestampCountWithHttpInfo(tenantId, meteringUnitName, timestamp);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.DeleteMeteringUnitTimestampCountWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **meteringUnitName** | **string** | Metering Unit Name |  |
| **timestamp** | **int** | Timestamp |  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmeteringunitdatecountbytenantidandunitnameanddate"></a>
# **GetMeteringUnitDateCountByTenantIdAndUnitNameAndDate**
> MeteringUnitDateCount GetMeteringUnitDateCountByTenantIdAndUnitNameAndDate (string tenantId, string meteringUnitName, string date)

Get Metering Unit Count for Specific Date

Gets the metering unit count for a specific date. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetMeteringUnitDateCountByTenantIdAndUnitNameAndDateExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var meteringUnitName = storage_unit;  // string | Metering Unit Name
            var date = 2022-01-01;  // string | Date

            try
            {
                // Get Metering Unit Count for Specific Date
                MeteringUnitDateCount result = apiInstance.GetMeteringUnitDateCountByTenantIdAndUnitNameAndDate(tenantId, meteringUnitName, date);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.GetMeteringUnitDateCountByTenantIdAndUnitNameAndDate: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeteringUnitDateCountByTenantIdAndUnitNameAndDateWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Metering Unit Count for Specific Date
    ApiResponse<MeteringUnitDateCount> response = apiInstance.GetMeteringUnitDateCountByTenantIdAndUnitNameAndDateWithHttpInfo(tenantId, meteringUnitName, date);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.GetMeteringUnitDateCountByTenantIdAndUnitNameAndDateWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **meteringUnitName** | **string** | Metering Unit Name |  |
| **date** | **string** | Date |  |

### Return type

[**MeteringUnitDateCount**](MeteringUnitDateCount.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmeteringunitdatecountbytenantidandunitnameanddateperiod"></a>
# **GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod**
> MeteringUnitDatePeriodCounts GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod (string tenantId, string meteringUnitName, int? startTimestamp = null, int? endTimestamp = null)

Obtain metering unit counts for a specified date/time period

Obtain metering unit counts for a specified date/time period. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriodExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var meteringUnitName = storage_unit;  // string | Metering Unit Name
            var startTimestamp = 1640995200;  // int? | Start Date-Time (optional) 
            var endTimestamp = 1640995200;  // int? | End Date-Time (optional) 

            try
            {
                // Obtain metering unit counts for a specified date/time period
                MeteringUnitDatePeriodCounts result = apiInstance.GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod(tenantId, meteringUnitName, startTimestamp, endTimestamp);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriod: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriodWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Obtain metering unit counts for a specified date/time period
    ApiResponse<MeteringUnitDatePeriodCounts> response = apiInstance.GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriodWithHttpInfo(tenantId, meteringUnitName, startTimestamp, endTimestamp);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.GetMeteringUnitDateCountByTenantIdAndUnitNameAndDatePeriodWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **meteringUnitName** | **string** | Metering Unit Name |  |
| **startTimestamp** | **int?** | Start Date-Time | [optional]  |
| **endTimestamp** | **int?** | End Date-Time | [optional]  |

### Return type

[**MeteringUnitDatePeriodCounts**](MeteringUnitDatePeriodCounts.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmeteringunitdatecountbytenantidandunitnametoday"></a>
# **GetMeteringUnitDateCountByTenantIdAndUnitNameToday**
> MeteringUnitDateCount GetMeteringUnitDateCountByTenantIdAndUnitNameToday (string tenantId, string meteringUnitName)

Get Metering Unit Count for the Current Day

Get the metering unit count for the current day. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetMeteringUnitDateCountByTenantIdAndUnitNameTodayExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var meteringUnitName = storage_unit;  // string | Metering Unit Name

            try
            {
                // Get Metering Unit Count for the Current Day
                MeteringUnitDateCount result = apiInstance.GetMeteringUnitDateCountByTenantIdAndUnitNameToday(tenantId, meteringUnitName);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.GetMeteringUnitDateCountByTenantIdAndUnitNameToday: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeteringUnitDateCountByTenantIdAndUnitNameTodayWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Metering Unit Count for the Current Day
    ApiResponse<MeteringUnitDateCount> response = apiInstance.GetMeteringUnitDateCountByTenantIdAndUnitNameTodayWithHttpInfo(tenantId, meteringUnitName);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.GetMeteringUnitDateCountByTenantIdAndUnitNameTodayWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **meteringUnitName** | **string** | Metering Unit Name |  |

### Return type

[**MeteringUnitDateCount**](MeteringUnitDateCount.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmeteringunitdatecountsbytenantidanddate"></a>
# **GetMeteringUnitDateCountsByTenantIdAndDate**
> MeteringUnitDateCounts GetMeteringUnitDateCountsByTenantIdAndDate (string tenantId, string date)

Get All Metering Unit Counts for a Specified Date

Gets the total metering unit count for the specified date. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetMeteringUnitDateCountsByTenantIdAndDateExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var date = 2022-01-01;  // string | Date

            try
            {
                // Get All Metering Unit Counts for a Specified Date
                MeteringUnitDateCounts result = apiInstance.GetMeteringUnitDateCountsByTenantIdAndDate(tenantId, date);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.GetMeteringUnitDateCountsByTenantIdAndDate: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeteringUnitDateCountsByTenantIdAndDateWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Metering Unit Counts for a Specified Date
    ApiResponse<MeteringUnitDateCounts> response = apiInstance.GetMeteringUnitDateCountsByTenantIdAndDateWithHttpInfo(tenantId, date);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.GetMeteringUnitDateCountsByTenantIdAndDateWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **date** | **string** | Date |  |

### Return type

[**MeteringUnitDateCounts**](MeteringUnitDateCounts.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmeteringunitmonthcountbytenantidandunitnameandmonth"></a>
# **GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonth**
> MeteringUnitMonthCount GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonth (string tenantId, string meteringUnitName, string month)

Get the Metering Unit Count for the Specified Month

Gets the metering unit count for the specified month. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonthExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var meteringUnitName = storage_unit;  // string | Metering Unit Name
            var month = 2022-01;  // string | Month

            try
            {
                // Get the Metering Unit Count for the Specified Month
                MeteringUnitMonthCount result = apiInstance.GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonth(tenantId, meteringUnitName, month);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonth: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonthWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get the Metering Unit Count for the Specified Month
    ApiResponse<MeteringUnitMonthCount> response = apiInstance.GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonthWithHttpInfo(tenantId, meteringUnitName, month);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.GetMeteringUnitMonthCountByTenantIdAndUnitNameAndMonthWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **meteringUnitName** | **string** | Metering Unit Name |  |
| **month** | **string** | Month |  |

### Return type

[**MeteringUnitMonthCount**](MeteringUnitMonthCount.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmeteringunitmonthcountbytenantidandunitnamethismonth"></a>
# **GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonth**
> MeteringUnitMonthCount GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonth (string tenantId, string meteringUnitName)

Get Metering Unit Count for the Current Month

Get the metering unit count for the current month. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonthExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var meteringUnitName = storage_unit;  // string | Metering Unit Name

            try
            {
                // Get Metering Unit Count for the Current Month
                MeteringUnitMonthCount result = apiInstance.GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonth(tenantId, meteringUnitName);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonth: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonthWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Metering Unit Count for the Current Month
    ApiResponse<MeteringUnitMonthCount> response = apiInstance.GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonthWithHttpInfo(tenantId, meteringUnitName);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.GetMeteringUnitMonthCountByTenantIdAndUnitNameThisMonthWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **meteringUnitName** | **string** | Metering Unit Name |  |

### Return type

[**MeteringUnitMonthCount**](MeteringUnitMonthCount.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmeteringunitmonthcountsbytenantidandmonth"></a>
# **GetMeteringUnitMonthCountsByTenantIdAndMonth**
> MeteringUnitMonthCounts GetMeteringUnitMonthCountsByTenantIdAndMonth (string tenantId, string month)

Get All Metering Unit Counts for the Specified Month

Gets all metering unit counts for the specified month. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetMeteringUnitMonthCountsByTenantIdAndMonthExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var month = 2022-01;  // string | Month

            try
            {
                // Get All Metering Unit Counts for the Specified Month
                MeteringUnitMonthCounts result = apiInstance.GetMeteringUnitMonthCountsByTenantIdAndMonth(tenantId, month);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.GetMeteringUnitMonthCountsByTenantIdAndMonth: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeteringUnitMonthCountsByTenantIdAndMonthWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get All Metering Unit Counts for the Specified Month
    ApiResponse<MeteringUnitMonthCounts> response = apiInstance.GetMeteringUnitMonthCountsByTenantIdAndMonthWithHttpInfo(tenantId, month);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.GetMeteringUnitMonthCountsByTenantIdAndMonthWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **month** | **string** | Month |  |

### Return type

[**MeteringUnitMonthCounts**](MeteringUnitMonthCounts.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getmeteringunits"></a>
# **GetMeteringUnits**
> MeteringUnits GetMeteringUnits ()

Get all metering units

Get all metering units. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetMeteringUnitsExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);

            try
            {
                // Get all metering units
                MeteringUnits result = apiInstance.GetMeteringUnits();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.GetMeteringUnits: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetMeteringUnitsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get all metering units
    ApiResponse<MeteringUnits> response = apiInstance.GetMeteringUnitsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.GetMeteringUnitsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**MeteringUnits**](MeteringUnits.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="updatemeteringunitbyid"></a>
# **UpdateMeteringUnitByID**
> void UpdateMeteringUnitByID (string meteringUnitId, MeteringUnitProps body = null)

Update Metering Unit

Update metering unit. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class UpdateMeteringUnitByIDExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var meteringUnitId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Metering Unit ID
            var body = new MeteringUnitProps();  // MeteringUnitProps |  (optional) 

            try
            {
                // Update Metering Unit
                apiInstance.UpdateMeteringUnitByID(meteringUnitId, body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.UpdateMeteringUnitByID: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateMeteringUnitByIDWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Metering Unit
    apiInstance.UpdateMeteringUnitByIDWithHttpInfo(meteringUnitId, body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.UpdateMeteringUnitByIDWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **meteringUnitId** | **string** | Metering Unit ID |  |
| **body** | **MeteringUnitProps** |  | [optional]  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="updatemeteringunittimestampcount"></a>
# **UpdateMeteringUnitTimestampCount**
> MeteringUnitTimestampCount UpdateMeteringUnitTimestampCount (string tenantId, string meteringUnitName, int timestamp, UpdateMeteringUnitTimestampCountParam updateMeteringUnitTimestampCountParam = null)

Update Metering Unit Count for Specified Timestamp

Update metering unit count for the specified timestamp. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class UpdateMeteringUnitTimestampCountExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var meteringUnitName = storage_unit;  // string | Metering Unit Name
            var timestamp = 1640995200;  // int | Timestamp
            var updateMeteringUnitTimestampCountParam = new UpdateMeteringUnitTimestampCountParam(); // UpdateMeteringUnitTimestampCountParam |  (optional) 

            try
            {
                // Update Metering Unit Count for Specified Timestamp
                MeteringUnitTimestampCount result = apiInstance.UpdateMeteringUnitTimestampCount(tenantId, meteringUnitName, timestamp, updateMeteringUnitTimestampCountParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.UpdateMeteringUnitTimestampCount: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateMeteringUnitTimestampCountWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Metering Unit Count for Specified Timestamp
    ApiResponse<MeteringUnitTimestampCount> response = apiInstance.UpdateMeteringUnitTimestampCountWithHttpInfo(tenantId, meteringUnitName, timestamp, updateMeteringUnitTimestampCountParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.UpdateMeteringUnitTimestampCountWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **meteringUnitName** | **string** | Metering Unit Name |  |
| **timestamp** | **int** | Timestamp |  |
| **updateMeteringUnitTimestampCountParam** | [**UpdateMeteringUnitTimestampCountParam**](UpdateMeteringUnitTimestampCountParam.md) |  | [optional]  |

### Return type

[**MeteringUnitTimestampCount**](MeteringUnitTimestampCount.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="updatemeteringunittimestampcountnow"></a>
# **UpdateMeteringUnitTimestampCountNow**
> MeteringUnitTimestampCount UpdateMeteringUnitTimestampCountNow (string tenantId, string meteringUnitName, UpdateMeteringUnitTimestampCountNowParam updateMeteringUnitTimestampCountNowParam = null)

Update Metering Unit Count for Current Time

Update the metering unit count for the current time. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class UpdateMeteringUnitTimestampCountNowExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new MeteringApi(config);
            var tenantId = 46af35b5-60de-4cd8-9412-19a3a5d1f838;  // string | Tenant ID
            var meteringUnitName = storage_unit;  // string | Metering Unit Name
            var updateMeteringUnitTimestampCountNowParam = new UpdateMeteringUnitTimestampCountNowParam(); // UpdateMeteringUnitTimestampCountNowParam |  (optional) 

            try
            {
                // Update Metering Unit Count for Current Time
                MeteringUnitTimestampCount result = apiInstance.UpdateMeteringUnitTimestampCountNow(tenantId, meteringUnitName, updateMeteringUnitTimestampCountNowParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling MeteringApi.UpdateMeteringUnitTimestampCountNow: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateMeteringUnitTimestampCountNowWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Metering Unit Count for Current Time
    ApiResponse<MeteringUnitTimestampCount> response = apiInstance.UpdateMeteringUnitTimestampCountNowWithHttpInfo(tenantId, meteringUnitName, updateMeteringUnitTimestampCountNowParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling MeteringApi.UpdateMeteringUnitTimestampCountNowWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **meteringUnitName** | **string** | Metering Unit Name |  |
| **updateMeteringUnitTimestampCountNowParam** | [**UpdateMeteringUnitTimestampCountNowParam**](UpdateMeteringUnitTimestampCountNowParam.md) |  | [optional]  |

### Return type

[**MeteringUnitTimestampCount**](MeteringUnitTimestampCount.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

