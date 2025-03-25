# pricingapi.Api.PricingMenusApi

All URIs are relative to *https://api.saasus.io/v1/pricing*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreatePricingMenu**](PricingMenusApi.md#createpricingmenu) | **POST** /menus | Create a Pricing Feature Menu |
| [**DeletePricingMenu**](PricingMenusApi.md#deletepricingmenu) | **DELETE** /menus/{menu_id} | Delete Pricing Feature Menu |
| [**GetPricingMenu**](PricingMenusApi.md#getpricingmenu) | **GET** /menus/{menu_id} | Get Pricing Feature Menu |
| [**GetPricingMenus**](PricingMenusApi.md#getpricingmenus) | **GET** /menus | Get Pricing Feature Menus |
| [**UpdatePricingMenu**](PricingMenusApi.md#updatepricingmenu) | **PATCH** /menus/{menu_id} | Update Pricing Feature Menu |

<a id="createpricingmenu"></a>
# **CreatePricingMenu**
> PricingMenu CreatePricingMenu (SavePricingMenuParam body = null)

Create a Pricing Feature Menu

Create a pricing feature menu. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class CreatePricingMenuExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingMenusApi(config);
            var body = new SavePricingMenuParam();  // SavePricingMenuParam |  (optional) 

            try
            {
                // Create a Pricing Feature Menu
                PricingMenu result = apiInstance.CreatePricingMenu(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingMenusApi.CreatePricingMenu: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreatePricingMenuWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create a Pricing Feature Menu
    ApiResponse<PricingMenu> response = apiInstance.CreatePricingMenuWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingMenusApi.CreatePricingMenuWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **SavePricingMenuParam** |  | [optional]  |

### Return type

[**PricingMenu**](PricingMenu.md)

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

<a id="deletepricingmenu"></a>
# **DeletePricingMenu**
> void DeletePricingMenu (string menuId)

Delete Pricing Feature Menu

Delete pricing feature menu. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class DeletePricingMenuExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingMenusApi(config);
            var menuId = "menuId_example";  // string | Menu ID

            try
            {
                // Delete Pricing Feature Menu
                apiInstance.DeletePricingMenu(menuId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingMenusApi.DeletePricingMenu: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeletePricingMenuWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Pricing Feature Menu
    apiInstance.DeletePricingMenuWithHttpInfo(menuId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingMenusApi.DeletePricingMenuWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **menuId** | **string** | Menu ID |  |

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

<a id="getpricingmenu"></a>
# **GetPricingMenu**
> PricingMenu GetPricingMenu (string menuId)

Get Pricing Feature Menu

Get a pricing feature menu. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetPricingMenuExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingMenusApi(config);
            var menuId = "menuId_example";  // string | Menu ID

            try
            {
                // Get Pricing Feature Menu
                PricingMenu result = apiInstance.GetPricingMenu(menuId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingMenusApi.GetPricingMenu: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetPricingMenuWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Pricing Feature Menu
    ApiResponse<PricingMenu> response = apiInstance.GetPricingMenuWithHttpInfo(menuId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingMenusApi.GetPricingMenuWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **menuId** | **string** | Menu ID |  |

### Return type

[**PricingMenu**](PricingMenu.md)

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

<a id="getpricingmenus"></a>
# **GetPricingMenus**
> PricingMenus GetPricingMenus ()

Get Pricing Feature Menus

Get the feature menu list. Multiple measurement units are grouped together and defined as one feature menu. Multiple feature menus defined here are combined into one billing plan. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class GetPricingMenusExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingMenusApi(config);

            try
            {
                // Get Pricing Feature Menus
                PricingMenus result = apiInstance.GetPricingMenus();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingMenusApi.GetPricingMenus: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetPricingMenusWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Pricing Feature Menus
    ApiResponse<PricingMenus> response = apiInstance.GetPricingMenusWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingMenusApi.GetPricingMenusWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**PricingMenus**](PricingMenus.md)

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

<a id="updatepricingmenu"></a>
# **UpdatePricingMenu**
> void UpdatePricingMenu (string menuId, SavePricingMenuParam body = null)

Update Pricing Feature Menu

Update pricing feature menu. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using pricingapi.Api;
using pricingapi.Model;
using modules;

namespace Example
{
    public class UpdatePricingMenuExample
    {
        public static void Main()
        {
            PricingApiClientConfig config = new Configuration().GetPricingApiClientConfig();
            var apiInstance = new PricingMenusApi(config);
            var menuId = "menuId_example";  // string | Menu ID
            var body = new SavePricingMenuParam();  // SavePricingMenuParam |  (optional) 

            try
            {
                // Update Pricing Feature Menu
                apiInstance.UpdatePricingMenu(menuId, body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling PricingMenusApi.UpdatePricingMenu: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdatePricingMenuWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Pricing Feature Menu
    apiInstance.UpdatePricingMenuWithHttpInfo(menuId, body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling PricingMenusApi.UpdatePricingMenuWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **menuId** | **string** | Menu ID |  |
| **body** | **SavePricingMenuParam** |  | [optional]  |

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

