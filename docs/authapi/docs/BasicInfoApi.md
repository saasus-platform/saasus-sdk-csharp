# authapi.Api.BasicInfoApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**FindNotificationMessages**](BasicInfoApi.md#findnotificationmessages) | **GET** /notification-messages | Get Notification Email Templates |
| [**GetBasicInfo**](BasicInfoApi.md#getbasicinfo) | **GET** /basic-info | Get Basic Configurations |
| [**GetCustomizePageSettings**](BasicInfoApi.md#getcustomizepagesettings) | **GET** /customize-page-settings | Get Authentication Authorization Basic Information |
| [**GetCustomizePages**](BasicInfoApi.md#getcustomizepages) | **GET** /customize-pages | Get Authentication Page Setting |
| [**UpdateBasicInfo**](BasicInfoApi.md#updatebasicinfo) | **PUT** /basic-info | Update Basic Configurations |
| [**UpdateCustomizePageSettings**](BasicInfoApi.md#updatecustomizepagesettings) | **PATCH** /customize-page-settings | Update Authentication Authorization Basic Information |
| [**UpdateCustomizePages**](BasicInfoApi.md#updatecustomizepages) | **PATCH** /customize-pages | Authentication Page Setting |
| [**UpdateNotificationMessages**](BasicInfoApi.md#updatenotificationmessages) | **PUT** /notification-messages | Update Notification Email Template |

<a id="findnotificationmessages"></a>
# **FindNotificationMessages**
> NotificationMessages FindNotificationMessages ()

Get Notification Email Templates

Get notification email templates. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class FindNotificationMessagesExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new BasicInfoApi(config);

            try
            {
                // Get Notification Email Templates
                NotificationMessages result = apiInstance.FindNotificationMessages();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BasicInfoApi.FindNotificationMessages: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the FindNotificationMessagesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Notification Email Templates
    ApiResponse<NotificationMessages> response = apiInstance.FindNotificationMessagesWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BasicInfoApi.FindNotificationMessagesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**NotificationMessages**](NotificationMessages.md)

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

<a id="getbasicinfo"></a>
# **GetBasicInfo**
> BasicInfo GetBasicInfo ()

Get Basic Configurations

Get the domain name and CNAME record based on the SaaS ID. By setting the CNAME record on the DNS the login screen will be generated. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetBasicInfoExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new BasicInfoApi(config);

            try
            {
                // Get Basic Configurations
                BasicInfo result = apiInstance.GetBasicInfo();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BasicInfoApi.GetBasicInfo: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetBasicInfoWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Basic Configurations
    ApiResponse<BasicInfo> response = apiInstance.GetBasicInfoWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BasicInfoApi.GetBasicInfoWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**BasicInfo**](BasicInfo.md)

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

<a id="getcustomizepagesettings"></a>
# **GetCustomizePageSettings**
> CustomizePageSettings GetCustomizePageSettings ()

Get Authentication Authorization Basic Information

Get authentication authorization basic information. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetCustomizePageSettingsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new BasicInfoApi(config);

            try
            {
                // Get Authentication Authorization Basic Information
                CustomizePageSettings result = apiInstance.GetCustomizePageSettings();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BasicInfoApi.GetCustomizePageSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCustomizePageSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Authentication Authorization Basic Information
    ApiResponse<CustomizePageSettings> response = apiInstance.GetCustomizePageSettingsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BasicInfoApi.GetCustomizePageSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**CustomizePageSettings**](CustomizePageSettings.md)

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

<a id="getcustomizepages"></a>
# **GetCustomizePages**
> CustomizePages GetCustomizePages ()

Get Authentication Page Setting

Get the authentication screen setting information (new registration, login, password reset, etc.). 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetCustomizePagesExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new BasicInfoApi(config);

            try
            {
                // Get Authentication Page Setting
                CustomizePages result = apiInstance.GetCustomizePages();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BasicInfoApi.GetCustomizePages: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCustomizePagesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Authentication Page Setting
    ApiResponse<CustomizePages> response = apiInstance.GetCustomizePagesWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BasicInfoApi.GetCustomizePagesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**CustomizePages**](CustomizePages.md)

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

<a id="updatebasicinfo"></a>
# **UpdateBasicInfo**
> void UpdateBasicInfo (UpdateBasicInfoParam updateBasicInfoParam = null)

Update Basic Configurations

Update the domain name that was set as a parameter based on the SaaS ID. After the CNAME record is generated, set it in your DNS. If it is set on a SaaS application that is already running, it will affect the behavior. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateBasicInfoExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new BasicInfoApi(config);
            var updateBasicInfoParam = new UpdateBasicInfoParam(); // UpdateBasicInfoParam |  (optional) 

            try
            {
                // Update Basic Configurations
                apiInstance.UpdateBasicInfo(updateBasicInfoParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BasicInfoApi.UpdateBasicInfo: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateBasicInfoWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Basic Configurations
    apiInstance.UpdateBasicInfoWithHttpInfo(updateBasicInfoParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BasicInfoApi.UpdateBasicInfoWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateBasicInfoParam** | [**UpdateBasicInfoParam**](UpdateBasicInfoParam.md) |  | [optional]  |

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

<a id="updatecustomizepagesettings"></a>
# **UpdateCustomizePageSettings**
> void UpdateCustomizePageSettings (UpdateCustomizePageSettingsParam updateCustomizePageSettingsParam = null)

Update Authentication Authorization Basic Information

Update authentication authorization basic information. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateCustomizePageSettingsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new BasicInfoApi(config);
            var updateCustomizePageSettingsParam = new UpdateCustomizePageSettingsParam(); // UpdateCustomizePageSettingsParam |  (optional) 

            try
            {
                // Update Authentication Authorization Basic Information
                apiInstance.UpdateCustomizePageSettings(updateCustomizePageSettingsParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BasicInfoApi.UpdateCustomizePageSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateCustomizePageSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Authentication Authorization Basic Information
    apiInstance.UpdateCustomizePageSettingsWithHttpInfo(updateCustomizePageSettingsParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BasicInfoApi.UpdateCustomizePageSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateCustomizePageSettingsParam** | [**UpdateCustomizePageSettingsParam**](UpdateCustomizePageSettingsParam.md) |  | [optional]  |

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

<a id="updatecustomizepages"></a>
# **UpdateCustomizePages**
> void UpdateCustomizePages (UpdateCustomizePagesParam updateCustomizePagesParam = null)

Authentication Page Setting

Update the authentication page setting information (new registration, login, password reset, etc.). 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateCustomizePagesExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new BasicInfoApi(config);
            var updateCustomizePagesParam = new UpdateCustomizePagesParam(); // UpdateCustomizePagesParam |  (optional) 

            try
            {
                // Authentication Page Setting
                apiInstance.UpdateCustomizePages(updateCustomizePagesParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BasicInfoApi.UpdateCustomizePages: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateCustomizePagesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Authentication Page Setting
    apiInstance.UpdateCustomizePagesWithHttpInfo(updateCustomizePagesParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BasicInfoApi.UpdateCustomizePagesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateCustomizePagesParam** | [**UpdateCustomizePagesParam**](UpdateCustomizePagesParam.md) |  | [optional]  |

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

<a id="updatenotificationmessages"></a>
# **UpdateNotificationMessages**
> void UpdateNotificationMessages (UpdateNotificationMessagesParam updateNotificationMessagesParam = null)

Update Notification Email Template

Update notification email template. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateNotificationMessagesExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new BasicInfoApi(config);
            var updateNotificationMessagesParam = new UpdateNotificationMessagesParam(); // UpdateNotificationMessagesParam |  (optional) 

            try
            {
                // Update Notification Email Template
                apiInstance.UpdateNotificationMessages(updateNotificationMessagesParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling BasicInfoApi.UpdateNotificationMessages: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateNotificationMessagesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Notification Email Template
    apiInstance.UpdateNotificationMessagesWithHttpInfo(updateNotificationMessagesParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling BasicInfoApi.UpdateNotificationMessagesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateNotificationMessagesParam** | [**UpdateNotificationMessagesParam**](UpdateNotificationMessagesParam.md) |  | [optional]  |

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

