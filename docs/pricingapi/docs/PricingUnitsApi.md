# pricingapi.Api.PricingUnitsApi

All URIs are relative to *https://api.saasus.io/v1/pricing*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreatePricingUnit**](PricingUnitsApi.md#createpricingunit) | **POST** /units | Create Pricing Unit |
| [**DeletePricingUnit**](PricingUnitsApi.md#deletepricingunit) | **DELETE** /units/{pricing_unit_id} | Delete Pricing Unit |
| [**GetPricingUnit**](PricingUnitsApi.md#getpricingunit) | **GET** /units/{pricing_unit_id} | Get Pricing Unit |
| [**GetPricingUnits**](PricingUnitsApi.md#getpricingunits) | **GET** /units | Get Pricing Units |
| [**UpdatePricingUnit**](PricingUnitsApi.md#updatepricingunit) | **PATCH** /units/{pricing_unit_id} | Update Pricing Unit |

<a id="createpricingunit"></a>
# **CreatePricingUnit**
> PricingUnit CreatePricingUnit (PricingUnitForSave body = null)

Create Pricing Unit

Create a pricing unit. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class CreatePricingUnitExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingUnitsApi(config);
            var body = new PricingUnitForSave();  // PricingUnitForSave |  (optional) 

            try
            {
                // Create Pricing Unit
                PricingUnit result = apiInstance.CreatePricingUnit(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingUnitsApi.CreatePricingUnit: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreatePricingUnitWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Pricing Unit
    ApiResponse<PricingUnit> response = apiInstance.CreatePricingUnitWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingUnitsApi.CreatePricingUnitWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **PricingUnitForSave** |  | [optional]  |

### Return type

[**PricingUnit**](PricingUnit.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | Created |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletepricingunit"></a>
# **DeletePricingUnit**
> void DeletePricingUnit (string pricingUnitId)

Delete Pricing Unit

Delete a pricing unit. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class DeletePricingUnitExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingUnitsApi(config);
            var pricingUnitId = "pricingUnitId_example";  // string | Unit ID

            try
            {
                // Delete Pricing Unit
                apiInstance.DeletePricingUnit(pricingUnitId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingUnitsApi.DeletePricingUnit: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeletePricingUnitWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Pricing Unit
    apiInstance.DeletePricingUnitWithHttpInfo(pricingUnitId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingUnitsApi.DeletePricingUnitWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **pricingUnitId** | **string** | Unit ID |  |

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

<a id="getpricingunit"></a>
# **GetPricingUnit**
> PricingUnit GetPricingUnit (string pricingUnitId)

Get Pricing Unit

Get a pricing unit. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetPricingUnitExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingUnitsApi(config);
            var pricingUnitId = "pricingUnitId_example";  // string | Unit ID

            try
            {
                // Get Pricing Unit
                PricingUnit result = apiInstance.GetPricingUnit(pricingUnitId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingUnitsApi.GetPricingUnit: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetPricingUnitWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Pricing Unit
    ApiResponse<PricingUnit> response = apiInstance.GetPricingUnitWithHttpInfo(pricingUnitId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingUnitsApi.GetPricingUnitWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **pricingUnitId** | **string** | Unit ID |  |

### Return type

[**PricingUnit**](PricingUnit.md)

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

<a id="getpricingunits"></a>
# **GetPricingUnits**
> PricingUnits GetPricingUnits ()

Get Pricing Units

Gets the smallest unit of measure on which the charges are based. \"Fixed Unit\" (type=fixed) is a unit of a monthly fixed charge such as a basic charge, \"Usage Unit\" (type=usage) is a unit in which a charge is generated per unit such as billing for the number of users, \"Tiered Unit\" (type=tiered) is a fixed charge unit for each tier of usage, such as the tiered packet charge for mobile phones, \"Tiered Usage Unit\" (type=tiered_usage) is a unit where the charge per unit changes according to the usage amount, such as a volume discount. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetPricingUnitsExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingUnitsApi(config);

            try
            {
                // Get Pricing Units
                PricingUnits result = apiInstance.GetPricingUnits();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingUnitsApi.GetPricingUnits: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetPricingUnitsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Pricing Units
    ApiResponse<PricingUnits> response = apiInstance.GetPricingUnitsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingUnitsApi.GetPricingUnitsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**PricingUnits**](PricingUnits.md)

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

<a id="updatepricingunit"></a>
# **UpdatePricingUnit**
> void UpdatePricingUnit (string pricingUnitId, PricingUnitForSave body = null)

Update Pricing Unit

Update pricing unit. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class UpdatePricingUnitExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingUnitsApi(config);
            var pricingUnitId = "pricingUnitId_example";  // string | Unit ID
            var body = new PricingUnitForSave();  // PricingUnitForSave |  (optional) 

            try
            {
                // Update Pricing Unit
                apiInstance.UpdatePricingUnit(pricingUnitId, body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingUnitsApi.UpdatePricingUnit: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdatePricingUnitWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Pricing Unit
    apiInstance.UpdatePricingUnitWithHttpInfo(pricingUnitId, body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingUnitsApi.UpdatePricingUnitWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **pricingUnitId** | **string** | Unit ID |  |
| **body** | **PricingUnitForSave** |  | [optional]  |

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
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

