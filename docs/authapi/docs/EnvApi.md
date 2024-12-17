# authapi.Api.EnvApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateEnv**](EnvApi.md#createenv) | **POST** /envs | Create Env Info |
| [**DeleteEnv**](EnvApi.md#deleteenv) | **DELETE** /envs/{env_id} | Delete Env Info |
| [**GetEnv**](EnvApi.md#getenv) | **GET** /envs/{env_id} | Get Env Details |
| [**GetEnvs**](EnvApi.md#getenvs) | **GET** /envs | Get Env Info |
| [**UpdateEnv**](EnvApi.md#updateenv) | **PATCH** /envs/{env_id} | Update Env Info |

<a id="createenv"></a>
# **CreateEnv**
> Env CreateEnv (Env body = null)

Create Env Info

Create environment information. Multiple environments can be defined, such as an environment for testing linkage, an environment for development, and an environment for actual operation. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateEnvExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new EnvApi(config);
            var body = new Env();  // Env |  (optional) 

            try
            {
                // Create Env Info
                Env result = apiInstance.CreateEnv(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EnvApi.CreateEnv: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateEnvWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Env Info
    ApiResponse<Env> response = apiInstance.CreateEnvWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EnvApi.CreateEnvWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Env** |  | [optional]  |

### Return type

[**Env**](Env.md)

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

<a id="deleteenv"></a>
# **DeleteEnv**
> void DeleteEnv (int envId)

Delete Env Info

Delete env info. Env with id 3 cannot be deleted. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class DeleteEnvExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new EnvApi(config);
            var envId = 56;  // int | Env ID

            try
            {
                // Delete Env Info
                apiInstance.DeleteEnv(envId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EnvApi.DeleteEnv: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteEnvWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Env Info
    apiInstance.DeleteEnvWithHttpInfo(envId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EnvApi.DeleteEnvWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **envId** | **int** | Env ID |  |

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

<a id="getenv"></a>
# **GetEnv**
> Env GetEnv (int envId)

Get Env Details

Get environment details. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetEnvExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new EnvApi(config);
            var envId = 56;  // int | Env ID

            try
            {
                // Get Env Details
                Env result = apiInstance.GetEnv(envId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EnvApi.GetEnv: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetEnvWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Env Details
    ApiResponse<Env> response = apiInstance.GetEnvWithHttpInfo(envId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EnvApi.GetEnvWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **envId** | **int** | Env ID |  |

### Return type

[**Env**](Env.md)

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

<a id="getenvs"></a>
# **GetEnvs**
> Envs GetEnvs ()

Get Env Info

Get registered environment information. Multiple environments can be defined, such as an environment for testing linkage, an environment for development, and an environment for actual operation. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetEnvsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new EnvApi(config);

            try
            {
                // Get Env Info
                Envs result = apiInstance.GetEnvs();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EnvApi.GetEnvs: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetEnvsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Env Info
    ApiResponse<Envs> response = apiInstance.GetEnvsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EnvApi.GetEnvsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**Envs**](Envs.md)

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

<a id="updateenv"></a>
# **UpdateEnv**
> void UpdateEnv (int envId, UpdateEnvParam updateEnvParam = null)

Update Env Info

Update env info. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateEnvExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new EnvApi(config);
            var envId = 56;  // int | Env ID
            var updateEnvParam = new UpdateEnvParam(); // UpdateEnvParam |  (optional) 

            try
            {
                // Update Env Info
                apiInstance.UpdateEnv(envId, updateEnvParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling EnvApi.UpdateEnv: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateEnvWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Env Info
    apiInstance.UpdateEnvWithHttpInfo(envId, updateEnvParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling EnvApi.UpdateEnvWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **envId** | **int** | Env ID |  |
| **updateEnvParam** | [**UpdateEnvParam**](UpdateEnvParam.md) |  | [optional]  |

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
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

