# authapi.Api.SingleTenantApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetCloudFormationLaunchStackLinkForSingleTenant**](SingleTenantApi.md#getcloudformationlaunchstacklinkforsingletenant) | **GET** /single-tenant/cloudformation-launch-stack-link | Get CloudFormation Stack Launch Link For Single Tenant |
| [**GetSingleTenantSettings**](SingleTenantApi.md#getsingletenantsettings) | **GET** /single-tenant/settings | Retrieve the settings of the single tenant. |
| [**UpdateSingleTenantSettings**](SingleTenantApi.md#updatesingletenantsettings) | **PATCH** /single-tenant/settings | Update configuration information for single-tenant functionality |

<a id="getcloudformationlaunchstacklinkforsingletenant"></a>
# **GetCloudFormationLaunchStackLinkForSingleTenant**
> CloudFormationLaunchStackLink GetCloudFormationLaunchStackLinkForSingleTenant ()

Get CloudFormation Stack Launch Link For Single Tenant

Get the CloudFormation stack activation link for Single Tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetCloudFormationLaunchStackLinkForSingleTenantExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SingleTenantApi(config);

            try
            {
                // Get CloudFormation Stack Launch Link For Single Tenant
                CloudFormationLaunchStackLink result = apiInstance.GetCloudFormationLaunchStackLinkForSingleTenant();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SingleTenantApi.GetCloudFormationLaunchStackLinkForSingleTenant: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCloudFormationLaunchStackLinkForSingleTenantWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get CloudFormation Stack Launch Link For Single Tenant
    ApiResponse<CloudFormationLaunchStackLink> response = apiInstance.GetCloudFormationLaunchStackLinkForSingleTenantWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SingleTenantApi.GetCloudFormationLaunchStackLinkForSingleTenantWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**CloudFormationLaunchStackLink**](CloudFormationLaunchStackLink.md)

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

<a id="getsingletenantsettings"></a>
# **GetSingleTenantSettings**
> SingleTenantSettings GetSingleTenantSettings ()

Retrieve the settings of the single tenant.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetSingleTenantSettingsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SingleTenantApi(config);

            try
            {
                // Retrieve the settings of the single tenant.
                SingleTenantSettings result = apiInstance.GetSingleTenantSettings();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SingleTenantApi.GetSingleTenantSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSingleTenantSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Retrieve the settings of the single tenant.
    ApiResponse<SingleTenantSettings> response = apiInstance.GetSingleTenantSettingsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SingleTenantApi.GetSingleTenantSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**SingleTenantSettings**](SingleTenantSettings.md)

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

<a id="updatesingletenantsettings"></a>
# **UpdateSingleTenantSettings**
> void UpdateSingleTenantSettings (UpdateSingleTenantSettingsParam updateSingleTenantSettingsParam = null)

Update configuration information for single-tenant functionality

Updates configuration information for single-tenant functionality Returns error if single tenant feature cannot be enabled. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateSingleTenantSettingsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SingleTenantApi(config);
            var updateSingleTenantSettingsParam = new UpdateSingleTenantSettingsParam(); // UpdateSingleTenantSettingsParam |  (optional) 

            try
            {
                // Update configuration information for single-tenant functionality
                apiInstance.UpdateSingleTenantSettings(updateSingleTenantSettingsParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SingleTenantApi.UpdateSingleTenantSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSingleTenantSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update configuration information for single-tenant functionality
    apiInstance.UpdateSingleTenantSettingsWithHttpInfo(updateSingleTenantSettingsParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SingleTenantApi.UpdateSingleTenantSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateSingleTenantSettingsParam** | [**UpdateSingleTenantSettingsParam**](UpdateSingleTenantSettingsParam.md) |  | [optional]  |

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

