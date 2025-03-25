# apilogapi.Api.ApiLogApi

All URIs are relative to *https://api.saasus.io/v1/apilog*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetLog**](ApiLogApi.md#getlog) | **GET** /logs/{api_log_id} | Get API execution log |
| [**GetLogs**](ApiLogApi.md#getlogs) | **GET** /logs | Get API execution log list |

<a id="getlog"></a>
# **GetLog**
> ApiLog GetLog (string apiLogId)

Get API execution log

Retrieve the log of the API execution with the specified ID.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using apilogapi.Api;
using apilogapi.Model;
using modules;

namespace Example
{
    public class GetLogExample
    {
        public static void Main()
        {
            ApiLogApiClientConfig config = new Configuration().GetApiLogApiClientConfig();
            var apiInstance = new ApiLogApi(config);
            var apiLogId = "apiLogId_example";  // string | API Log ID

            try
            {
                // Get API execution log
                ApiLog result = apiInstance.GetLog(apiLogId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApiLogApi.GetLog: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetLogWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get API execution log
    ApiResponse<ApiLog> response = apiInstance.GetLogWithHttpInfo(apiLogId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApiLogApi.GetLogWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **apiLogId** | **string** | API Log ID |  |

### Return type

[**ApiLog**](ApiLog.md)

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

<a id="getlogs"></a>
# **GetLogs**
> ApiLogs GetLogs (DateTime? createdDate = null, DateTime? createdAt = null, long? limit = null, string cursor = null)

Get API execution log list

Retrieve the log of all API executions.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using apilogapi.Api;
using apilogapi.Model;
using modules;

namespace Example
{
    public class GetLogsExample
    {
        public static void Main()
        {
            ApiLogApiClientConfig config = new Configuration().GetApiLogApiClientConfig();
            var apiInstance = new ApiLogApi(config);
            var createdDate = DateTime.Parse("2013-10-20");  // DateTime? | The date, in format of YYYY-MM-DD, to retrieve the log. (optional) 
            var createdAt = DateTime.Parse("2013-10-20T19:20:30+01:00");  // DateTime? | The datetime, in ISO 8601 format, to retrieve the log. (optional) 
            var limit = 789L;  // long? | Maximum number of logs to retrieve. (optional) 
            var cursor = "cursor_example";  // string | Cursor for cursor pagination. (optional) 

            try
            {
                // Get API execution log list
                ApiLogs result = apiInstance.GetLogs(createdDate, createdAt, limit, cursor);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling ApiLogApi.GetLogs: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetLogsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get API execution log list
    ApiResponse<ApiLogs> response = apiInstance.GetLogsWithHttpInfo(createdDate, createdAt, limit, cursor);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling ApiLogApi.GetLogsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createdDate** | **DateTime?** | The date, in format of YYYY-MM-DD, to retrieve the log. | [optional]  |
| **createdAt** | **DateTime?** | The datetime, in ISO 8601 format, to retrieve the log. | [optional]  |
| **limit** | **long?** | Maximum number of logs to retrieve. | [optional]  |
| **cursor** | **string** | Cursor for cursor pagination. | [optional]  |

### Return type

[**ApiLogs**](ApiLogs.md)

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

