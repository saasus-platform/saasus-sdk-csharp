# integrationapi.Api.EventBridgeApi

All URIs are relative to *https://api.saasus.io/v1/integration*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateEventBridgeEvent**](EventBridgeApi.md#createeventbridgeevent) | **POST** /eventbridge/event | Send Events |
| [**CreateEventBridgeTestEvent**](EventBridgeApi.md#createeventbridgetestevent) | **POST** /eventbridge/test-event | Test EventBridge Connection |
| [**DeleteEventBridgeSettings**](EventBridgeApi.md#deleteeventbridgesettings) | **DELETE** /eventbridge/info | Delete EventBridge Settings |
| [**GetEventBridgeSettings**](EventBridgeApi.md#geteventbridgesettings) | **GET** /eventbridge/info | Get EventBridge Settings |
| [**SaveEventBridgeSettings**](EventBridgeApi.md#saveeventbridgesettings) | **PUT** /eventbridge/info | Update EventBridge Settings |

<a id="createeventbridgeevent"></a>
# **CreateEventBridgeEvent**
> void CreateEventBridgeEvent (CreateEventBridgeEventParam createEventBridgeEventParam = null)

Send Events

Send events to Amazon EventBridge. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using integrationapi.Api;
using integrationapi.Model;
using modules;

namespace Example
{
    public class CreateEventBridgeEventExample
    {
        public static void Main()
        {
            IntegrationApiClientConfig config = new Configuration().GetIntegrationApiClientConfig();
            var apiInstance = new EventBridgeApi(config);
            var createEventBridgeEventParam = new CreateEventBridgeEventParam(); // CreateEventBridgeEventParam |  (optional) 

            try
            {
                // Send Events
                apiInstance.CreateEventBridgeEvent(createEventBridgeEventParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventBridgeApi.CreateEventBridgeEvent: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateEventBridgeEventWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Send Events
    apiInstance.CreateEventBridgeEventWithHttpInfo(createEventBridgeEventParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventBridgeApi.CreateEventBridgeEventWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createEventBridgeEventParam** | [**CreateEventBridgeEventParam**](CreateEventBridgeEventParam.md) |  | [optional]  |

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
| **201** | Created |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="createeventbridgetestevent"></a>
# **CreateEventBridgeTestEvent**
> void CreateEventBridgeTestEvent ()

Test EventBridge Connection

Send events to test the connection with Amazon EventBridge. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using integrationapi.Api;
using integrationapi.Model;
using modules;

namespace Example
{
    public class CreateEventBridgeTestEventExample
    {
        public static void Main()
        {
            IntegrationApiClientConfig config = new Configuration().GetIntegrationApiClientConfig();
            var apiInstance = new EventBridgeApi(config);

            try
            {
                // Test EventBridge Connection
                apiInstance.CreateEventBridgeTestEvent();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventBridgeApi.CreateEventBridgeTestEvent: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateEventBridgeTestEventWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Test EventBridge Connection
    apiInstance.CreateEventBridgeTestEventWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventBridgeApi.CreateEventBridgeTestEventWithHttpInfo: " + e.Message);
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
| **201** | Created |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deleteeventbridgesettings"></a>
# **DeleteEventBridgeSettings**
> void DeleteEventBridgeSettings ()

Delete EventBridge Settings

Delete settings used to provide host state via Amazon EventBridge. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using integrationapi.Api;
using integrationapi.Model;
using modules;

namespace Example
{
    public class DeleteEventBridgeSettingsExample
    {
        public static void Main()
        {
            IntegrationApiClientConfig config = new Configuration().GetIntegrationApiClientConfig();
            var apiInstance = new EventBridgeApi(config);

            try
            {
                // Delete EventBridge Settings
                apiInstance.DeleteEventBridgeSettings();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventBridgeApi.DeleteEventBridgeSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteEventBridgeSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete EventBridge Settings
    apiInstance.DeleteEventBridgeSettingsWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventBridgeApi.DeleteEventBridgeSettingsWithHttpInfo: " + e.Message);
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

<a id="geteventbridgesettings"></a>
# **GetEventBridgeSettings**
> EventBridgeSettings GetEventBridgeSettings ()

Get EventBridge Settings

Gets the settings for providing real-time status of all monitored hosts via Amazon EventBridge. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using integrationapi.Api;
using integrationapi.Model;
using modules;

namespace Example
{
    public class GetEventBridgeSettingsExample
    {
        public static void Main()
        {
            IntegrationApiClientConfig config = new Configuration().GetIntegrationApiClientConfig();
            var apiInstance = new EventBridgeApi(config);

            try
            {
                // Get EventBridge Settings
                EventBridgeSettings result = apiInstance.GetEventBridgeSettings();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventBridgeApi.GetEventBridgeSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetEventBridgeSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get EventBridge Settings
    ApiResponse<EventBridgeSettings> response = apiInstance.GetEventBridgeSettingsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventBridgeApi.GetEventBridgeSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**EventBridgeSettings**](EventBridgeSettings.md)

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

<a id="saveeventbridgesettings"></a>
# **SaveEventBridgeSettings**
> void SaveEventBridgeSettings (EventBridgeSettings body = null)

Update EventBridge Settings

Update configuration used to provide the host state via Amazon EventBridge. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using integrationapi.Api;
using integrationapi.Model;
using modules;

namespace Example
{
    public class SaveEventBridgeSettingsExample
    {
        public static void Main()
        {
            IntegrationApiClientConfig config = new Configuration().GetIntegrationApiClientConfig();
            var apiInstance = new EventBridgeApi(config);
            var body = new EventBridgeSettings();  // EventBridgeSettings |  (optional) 

            try
            {
                // Update EventBridge Settings
                apiInstance.SaveEventBridgeSettings(body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EventBridgeApi.SaveEventBridgeSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SaveEventBridgeSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update EventBridge Settings
    apiInstance.SaveEventBridgeSettingsWithHttpInfo(body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EventBridgeApi.SaveEventBridgeSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **EventBridgeSettings** |  | [optional]  |

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

