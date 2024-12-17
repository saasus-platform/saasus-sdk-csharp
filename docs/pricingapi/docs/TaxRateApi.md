# pricingapi.Api.TaxRateApi

All URIs are relative to *https://api.saasus.io/v1/pricing*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateTaxRate**](TaxRateApi.md#createtaxrate) | **POST** /tax-rates | Create Tax Rate |
| [**GetTaxRates**](TaxRateApi.md#gettaxrates) | **GET** /tax-rates | Get Tax Rates |
| [**UpdateTaxRate**](TaxRateApi.md#updatetaxrate) | **PATCH** /tax-rates/{tax_rate_id} | Update Tax Rate |

<a id="createtaxrate"></a>
# **CreateTaxRate**
> TaxRate CreateTaxRate (TaxRateProps body = null)

Create Tax Rate

Creates a tax rate. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class CreateTaxRateExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new TaxRateApi(config);
            var body = new TaxRateProps();  // TaxRateProps |  (optional) 

            try
            {
                // Create Tax Rate
                TaxRate result = apiInstance.CreateTaxRate(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TaxRateApi.CreateTaxRate: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateTaxRateWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Tax Rate
    ApiResponse<TaxRate> response = apiInstance.CreateTaxRateWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TaxRateApi.CreateTaxRateWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **TaxRateProps** |  | [optional]  |

### Return type

[**TaxRate**](TaxRate.md)

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

<a id="gettaxrates"></a>
# **GetTaxRates**
> TaxRates GetTaxRates ()

Get Tax Rates

Get all Tax Rates 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetTaxRatesExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new TaxRateApi(config);

            try
            {
                // Get Tax Rates
                TaxRates result = apiInstance.GetTaxRates();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TaxRateApi.GetTaxRates: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTaxRatesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Tax Rates
    ApiResponse<TaxRates> response = apiInstance.GetTaxRatesWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TaxRateApi.GetTaxRatesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**TaxRates**](TaxRates.md)

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

<a id="updatetaxrate"></a>
# **UpdateTaxRate**
> void UpdateTaxRate (string taxRateId, UpdateTaxRateParam updateTaxRateParam = null)

Update Tax Rate

Update tax rate. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class UpdateTaxRateExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new TaxRateApi(config);
            var taxRateId = "taxRateId_example";  // string | Tax Rate ID
            var updateTaxRateParam = new UpdateTaxRateParam(); // UpdateTaxRateParam |  (optional) 

            try
            {
                // Update Tax Rate
                apiInstance.UpdateTaxRate(taxRateId, updateTaxRateParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TaxRateApi.UpdateTaxRate: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateTaxRateWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Tax Rate
    apiInstance.UpdateTaxRateWithHttpInfo(taxRateId, updateTaxRateParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TaxRateApi.UpdateTaxRateWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **taxRateId** | **string** | Tax Rate ID |  |
| **updateTaxRateParam** | [**UpdateTaxRateParam**](UpdateTaxRateParam.md) |  | [optional]  |

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

