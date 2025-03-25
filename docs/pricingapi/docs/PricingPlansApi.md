# pricingapi.Api.PricingPlansApi

All URIs are relative to *https://api.saasus.io/v1/pricing*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreatePricingPlan**](PricingPlansApi.md#createpricingplan) | **POST** /plans | Create Pricing Plan |
| [**DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRates**](PricingPlansApi.md#deleteallplansandmenusandunitsandmetersandtaxrates) | **DELETE** /plans-initialization | Delete all Plans, Menus, Units, Meters and Tax Rates |
| [**DeletePricingPlan**](PricingPlansApi.md#deletepricingplan) | **DELETE** /plans/{plan_id} | Delete Pricing Plan |
| [**DeleteStripePlan**](PricingPlansApi.md#deletestripeplan) | **DELETE** /stripe | Delete Product Data from Stripe |
| [**GetPricingPlan**](PricingPlansApi.md#getpricingplan) | **GET** /plans/{plan_id} | Get Pricing Plan |
| [**GetPricingPlans**](PricingPlansApi.md#getpricingplans) | **GET** /plans | Get Pricing Plans |
| [**LinkPlanToStripe**](PricingPlansApi.md#linkplantostripe) | **PATCH** /stripe/init | Connect to Stripe |
| [**UpdatePricingPlan**](PricingPlansApi.md#updatepricingplan) | **PATCH** /plans/{plan_id} | Update Pricing Plan |
| [**UpdatePricingPlansUsed**](PricingPlansApi.md#updatepricingplansused) | **PATCH** /plans/used | Update Used Flag |

<a id="createpricingplan"></a>
# **CreatePricingPlan**
> PricingPlan CreatePricingPlan (SavePricingPlanParam body = null)

Create Pricing Plan

Create a pricing plan. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class CreatePricingPlanExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingPlansApi(config);
            var body = new SavePricingPlanParam();  // SavePricingPlanParam |  (optional) 

            try
            {
                // Create Pricing Plan
                PricingPlan result = apiInstance.CreatePricingPlan(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingPlansApi.CreatePricingPlan: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreatePricingPlanWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Pricing Plan
    ApiResponse<PricingPlan> response = apiInstance.CreatePricingPlanWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingPlansApi.CreatePricingPlanWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **SavePricingPlanParam** |  | [optional]  |

### Return type

[**PricingPlan**](PricingPlan.md)

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

<a id="deleteallplansandmenusandunitsandmetersandtaxrates"></a>
# **DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRates**
> void DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRates ()

Delete all Plans, Menus, Units, Meters and Tax Rates

Unconditionally remove all rate plans, menus, units, meters and tax rates. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRatesExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingPlansApi(config);

            try
            {
                // Delete all Plans, Menus, Units, Meters and Tax Rates
                apiInstance.DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRates();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingPlansApi.DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRates: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRatesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete all Plans, Menus, Units, Meters and Tax Rates
    apiInstance.DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRatesWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingPlansApi.DeleteAllPlansAndMenusAndUnitsAndMetersAndTaxRatesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
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

<a id="deletepricingplan"></a>
# **DeletePricingPlan**
> void DeletePricingPlan (string planId)

Delete Pricing Plan

Delete a pricing plan. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class DeletePricingPlanExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingPlansApi(config);
            var planId = "planId_example";  // string | Pricing Plan ID

            try
            {
                // Delete Pricing Plan
                apiInstance.DeletePricingPlan(planId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingPlansApi.DeletePricingPlan: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeletePricingPlanWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Pricing Plan
    apiInstance.DeletePricingPlanWithHttpInfo(planId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingPlansApi.DeletePricingPlanWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **planId** | **string** | Pricing Plan ID |  |

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

<a id="deletestripeplan"></a>
# **DeleteStripePlan**
> void DeleteStripePlan ()

Delete Product Data from Stripe

Delete product data from Stripe. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class DeleteStripePlanExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingPlansApi(config);

            try
            {
                // Delete Product Data from Stripe
                apiInstance.DeleteStripePlan();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingPlansApi.DeleteStripePlan: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteStripePlanWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Product Data from Stripe
    apiInstance.DeleteStripePlanWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingPlansApi.DeleteStripePlanWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
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

<a id="getpricingplan"></a>
# **GetPricingPlan**
> PricingPlan GetPricingPlan (string planId)

Get Pricing Plan

Get a pricing plan. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetPricingPlanExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingPlansApi(config);
            var planId = "planId_example";  // string | Pricing Plan ID

            try
            {
                // Get Pricing Plan
                PricingPlan result = apiInstance.GetPricingPlan(planId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingPlansApi.GetPricingPlan: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetPricingPlanWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Pricing Plan
    ApiResponse<PricingPlan> response = apiInstance.GetPricingPlanWithHttpInfo(planId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingPlansApi.GetPricingPlanWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **planId** | **string** | Pricing Plan ID |  |

### Return type

[**PricingPlan**](PricingPlan.md)

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

<a id="getpricingplans"></a>
# **GetPricingPlans**
> PricingPlans GetPricingPlans ()

Get Pricing Plans

Get pricing plans. Multiple feature menus are grouped together and defined as one pricing plan. Each tenant can choose a pricing plan defined here. If you have a specific tenant-specific rate (private pricing), create and connect the pricing plan specifically for that tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetPricingPlansExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingPlansApi(config);

            try
            {
                // Get Pricing Plans
                PricingPlans result = apiInstance.GetPricingPlans();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingPlansApi.GetPricingPlans: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetPricingPlansWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Pricing Plans
    ApiResponse<PricingPlans> response = apiInstance.GetPricingPlansWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingPlansApi.GetPricingPlansWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**PricingPlans**](PricingPlans.md)

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

<a id="linkplantostripe"></a>
# **LinkPlanToStripe**
> void LinkPlanToStripe ()

Connect to Stripe

Connect information to Stripe. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class LinkPlanToStripeExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingPlansApi(config);

            try
            {
                // Connect to Stripe
                apiInstance.LinkPlanToStripe();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingPlansApi.LinkPlanToStripe: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the LinkPlanToStripeWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Connect to Stripe
    apiInstance.LinkPlanToStripeWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingPlansApi.LinkPlanToStripeWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
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

<a id="updatepricingplan"></a>
# **UpdatePricingPlan**
> void UpdatePricingPlan (string planId, SavePricingPlanParam body = null)

Update Pricing Plan

Update a pricing plan. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class UpdatePricingPlanExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingPlansApi(config);
            var planId = "planId_example";  // string | Pricing Plan ID
            var body = new SavePricingPlanParam();  // SavePricingPlanParam |  (optional) 

            try
            {
                // Update Pricing Plan
                apiInstance.UpdatePricingPlan(planId, body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingPlansApi.UpdatePricingPlan: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdatePricingPlanWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Pricing Plan
    apiInstance.UpdatePricingPlanWithHttpInfo(planId, body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingPlansApi.UpdatePricingPlanWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **planId** | **string** | Pricing Plan ID |  |
| **body** | **SavePricingPlanParam** |  | [optional]  |

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

<a id="updatepricingplansused"></a>
# **UpdatePricingPlansUsed**
> void UpdatePricingPlansUsed (UpdatePricingPlansUsedParam updatePricingPlansUsedParam = null)

Update Used Flag

Update price plan and feature menu/pricing unit to used. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class UpdatePricingPlansUsedExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingPlansApi(config);
            var updatePricingPlansUsedParam = new UpdatePricingPlansUsedParam(); // UpdatePricingPlansUsedParam |  (optional) 

            try
            {
                // Update Used Flag
                apiInstance.UpdatePricingPlansUsed(updatePricingPlansUsedParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingPlansApi.UpdatePricingPlansUsed: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdatePricingPlansUsedWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Used Flag
    apiInstance.UpdatePricingPlansUsedWithHttpInfo(updatePricingPlansUsedParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingPlansApi.UpdatePricingPlansUsedWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updatePricingPlansUsedParam** | [**UpdatePricingPlansUsedParam**](UpdatePricingPlansUsedParam.md) |  | [optional]  |

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

