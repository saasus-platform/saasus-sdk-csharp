# authapi.Api.TenantAttributeApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateTenantAttribute**](TenantAttributeApi.md#createtenantattribute) | **POST** /tenant-attributes | Create Tenant Attribute |
| [**DeleteTenantAttribute**](TenantAttributeApi.md#deletetenantattribute) | **DELETE** /tenant-attributes/{attribute_name} | Delete Tenant Attribute |
| [**GetTenantAttributes**](TenantAttributeApi.md#gettenantattributes) | **GET** /tenant-attributes | Get Tenant Attributes |

<a id="createtenantattribute"></a>
# **CreateTenantAttribute**
> Attribute CreateTenantAttribute (Attribute body = null)

Create Tenant Attribute

Register additional tenant attributes to be managed by SaaSus Platform. For example, tenant name, memo, etc., then get the attributes from SaaS using the SaaSus SDK/API. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateTenantAttributeExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantAttributeApi(config);
            var body = new Attribute();  // Attribute |  (optional) 

            try
            {
                // Create Tenant Attribute
                Attribute result = apiInstance.CreateTenantAttribute(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantAttributeApi.CreateTenantAttribute: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateTenantAttributeWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Tenant Attribute
    ApiResponse<Attribute> response = apiInstance.CreateTenantAttributeWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantAttributeApi.CreateTenantAttributeWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Attribute** |  | [optional]  |

### Return type

[**Attribute**](Attribute.md)

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

<a id="deletetenantattribute"></a>
# **DeleteTenantAttribute**
> void DeleteTenantAttribute (string attributeName)

Delete Tenant Attribute

Deletes tenant attributes managed by SaaSus Platform. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class DeleteTenantAttributeExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantAttributeApi(config);
            var attributeName = birthday;  // string | Attribute Name

            try
            {
                // Delete Tenant Attribute
                apiInstance.DeleteTenantAttribute(attributeName);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantAttributeApi.DeleteTenantAttribute: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteTenantAttributeWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Tenant Attribute
    apiInstance.DeleteTenantAttributeWithHttpInfo(attributeName);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantAttributeApi.DeleteTenantAttributeWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **attributeName** | **string** | Attribute Name |  |

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

<a id="gettenantattributes"></a>
# **GetTenantAttributes**
> TenantAttributes GetTenantAttributes ()

Get Tenant Attributes

Get definitions for additional tenant attributes managed by the SaaSus Platform. For example, tenant name, memo, etc., then get the attributes from SaaS using the SaaSus SDK/API. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetTenantAttributesExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantAttributeApi(config);

            try
            {
                // Get Tenant Attributes
                TenantAttributes result = apiInstance.GetTenantAttributes();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantAttributeApi.GetTenantAttributes: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantAttributesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Tenant Attributes
    ApiResponse<TenantAttributes> response = apiInstance.GetTenantAttributesWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantAttributeApi.GetTenantAttributesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**TenantAttributes**](TenantAttributes.md)

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

