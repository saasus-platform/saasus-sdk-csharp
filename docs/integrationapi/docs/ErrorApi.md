# integrationapi.Api.ErrorApi

All URIs are relative to *https://api.saasus.io/v1/integration*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ReturnInternalServerError**](ErrorApi.md#returninternalservererror) | **GET** /errors/internal-server-error | Return Internal Server Error |

<a id="returninternalservererror"></a>
# **ReturnInternalServerError**
> void ReturnInternalServerError ()

Return Internal Server Error

This endpoint is used for testing purposes. Returns a server error with status code 500. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using integrationapi.Api;
using integrationapi.Model;
using modules;

namespace Example
{
    public class ReturnInternalServerErrorExample
    {
        public static void Main()
        {
            IntegrationApiClientConfig config = new Configuration().GetIntegrationApiClientConfig();
            var apiInstance = new ErrorApi(config);

            try
            {
                // Return Internal Server Error
                apiInstance.ReturnInternalServerError();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ErrorApi.ReturnInternalServerError: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ReturnInternalServerErrorWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Return Internal Server Error
    apiInstance.ReturnInternalServerErrorWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ErrorApi.ReturnInternalServerErrorWithHttpInfo: " + e.Message);
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
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

