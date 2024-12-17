# billingapi.Api.StripeApi

All URIs are relative to *https://api.saasus.io/v1/billing*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**DeleteStripeInfo**](StripeApi.md#deletestripeinfo) | **DELETE** /stripe/info | Delete Stripe Connection |
| [**GetStripeInfo**](StripeApi.md#getstripeinfo) | **GET** /stripe/info | Get Stripe Connection information |
| [**UpdateStripeInfo**](StripeApi.md#updatestripeinfo) | **PUT** /stripe/info | Update Stripe Connection Info |

<a id="deletestripeinfo"></a>
# **DeleteStripeInfo**
> void DeleteStripeInfo ()

Delete Stripe Connection

Delete connection with external billing SaaS 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using billingapi.Api;
using billingapi.Model;
using modules;

namespace Example
{
    public class DeleteStripeInfoExample
    {
        public static void Main()
        {
            BillingApiClientConfig config = new Configuration().GetBillingApiClientConfig();
            var apiInstance = new StripeApi(config);

            try
            {
                // Delete Stripe Connection
                apiInstance.DeleteStripeInfo();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling StripeApi.DeleteStripeInfo: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteStripeInfoWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Stripe Connection
    apiInstance.DeleteStripeInfoWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling StripeApi.DeleteStripeInfoWithHttpInfo: " + e.Message);
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

<a id="getstripeinfo"></a>
# **GetStripeInfo**
> StripeInfo GetStripeInfo ()

Get Stripe Connection information

Get information on connnections with external billing SaaS. Currently possible to integrate with Stripe. Without integration, you will need to implement billing using the SaaSus SDK/API. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using billingapi.Api;
using billingapi.Model;
using modules;

namespace Example
{
    public class GetStripeInfoExample
    {
        public static void Main()
        {
            BillingApiClientConfig config = new Configuration().GetBillingApiClientConfig();
            var apiInstance = new StripeApi(config);

            try
            {
                // Get Stripe Connection information
                StripeInfo result = apiInstance.GetStripeInfo();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling StripeApi.GetStripeInfo: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetStripeInfoWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Stripe Connection information
    ApiResponse<StripeInfo> response = apiInstance.GetStripeInfoWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling StripeApi.GetStripeInfoWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**StripeInfo**](StripeInfo.md)

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

<a id="updatestripeinfo"></a>
# **UpdateStripeInfo**
> void UpdateStripeInfo (UpdateStripeInfoParam updateStripeInfoParam = null)

Update Stripe Connection Info

Updates information on connection with external billing SaaS. Currently possible to connect to Stripe. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using billingapi.Api;
using billingapi.Model;
using modules;

namespace Example
{
    public class UpdateStripeInfoExample
    {
        public static void Main()
        {
            BillingApiClientConfig config = new Configuration().GetBillingApiClientConfig();
            var apiInstance = new StripeApi(config);
            var updateStripeInfoParam = new UpdateStripeInfoParam(); // UpdateStripeInfoParam |  (optional) 

            try
            {
                // Update Stripe Connection Info
                apiInstance.UpdateStripeInfo(updateStripeInfoParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling StripeApi.UpdateStripeInfo: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateStripeInfoWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Stripe Connection Info
    apiInstance.UpdateStripeInfoWithHttpInfo(updateStripeInfoParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling StripeApi.UpdateStripeInfoWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateStripeInfoParam** | [**UpdateStripeInfoParam**](UpdateStripeInfoParam.md) |  | [optional]  |

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

