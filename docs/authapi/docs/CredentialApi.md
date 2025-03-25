# authapi.Api.CredentialApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateAuthCredentials**](CredentialApi.md#createauthcredentials) | **POST** /credentials | Save Authentication/Authorization Information |
| [**GetAuthCredentials**](CredentialApi.md#getauthcredentials) | **GET** /credentials | Get Authentication/Authorization Information |

<a id="createauthcredentials"></a>
# **CreateAuthCredentials**
> AuthorizationTempCode CreateAuthCredentials (Credentials body = null)

Save Authentication/Authorization Information

Temporarily save the parameter for the ID token, access token, and refresh token and return a temporary code for obtaining. Temporary codes are valid for 10 seconds from issuance. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateAuthCredentialsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new CredentialApi(config);
            var body = new Credentials();  // Credentials |  (optional) 

            try
            {
                // Save Authentication/Authorization Information
                AuthorizationTempCode result = apiInstance.CreateAuthCredentials(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CredentialApi.CreateAuthCredentials: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateAuthCredentialsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Save Authentication/Authorization Information
    ApiResponse<AuthorizationTempCode> response = apiInstance.CreateAuthCredentialsWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CredentialApi.CreateAuthCredentialsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Credentials** |  | [optional]  |

### Return type

[**AuthorizationTempCode**](AuthorizationTempCode.md)

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
| **401** | Unauthorized |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getauthcredentials"></a>
# **GetAuthCredentials**
> Credentials GetAuthCredentials (string code = null, string authFlow = null, string refreshToken = null)

Get Authentication/Authorization Information

Get ID token, access token, and refresh token using a temporary code or a refresh token. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetAuthCredentialsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new CredentialApi(config);
            var code = "code_example";  // string | Temp Code (optional) 
            var authFlow = "tempCodeAuth";  // string | Authentication Flow tempCodeAuth: Getting authentication information using a temporary code refreshTokenAuth: Getting authentication information using a refresh token If not specified, it will be tempCodeAuth  (optional) 
            var refreshToken = "refreshToken_example";  // string | Refresh Token (optional) 

            try
            {
                // Get Authentication/Authorization Information
                Credentials result = apiInstance.GetAuthCredentials(code, authFlow, refreshToken);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling CredentialApi.GetAuthCredentials: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAuthCredentialsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Authentication/Authorization Information
    ApiResponse<Credentials> response = apiInstance.GetAuthCredentialsWithHttpInfo(code, authFlow, refreshToken);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling CredentialApi.GetAuthCredentialsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **code** | **string** | Temp Code | [optional]  |
| **authFlow** | **string** | Authentication Flow tempCodeAuth: Getting authentication information using a temporary code refreshTokenAuth: Getting authentication information using a refresh token If not specified, it will be tempCodeAuth  | [optional]  |
| **refreshToken** | **string** | Refresh Token | [optional]  |

### Return type

[**Credentials**](Credentials.md)

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

