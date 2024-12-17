# authapi.Api.AuthInfoApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**GetAuthInfo**](AuthInfoApi.md#getauthinfo) | **GET** /auth-info | Get Authentication Info |
| [**GetIdentityProviders**](AuthInfoApi.md#getidentityproviders) | **GET** /identity-providers | Get Sign-In Information Via External Provider |
| [**GetSignInSettings**](AuthInfoApi.md#getsigninsettings) | **GET** /sign-in-settings | Get Password Requirements |
| [**UpdateAuthInfo**](AuthInfoApi.md#updateauthinfo) | **PUT** /auth-info | Update Authentication Info |
| [**UpdateIdentityProvider**](AuthInfoApi.md#updateidentityprovider) | **PUT** /identity-providers | Update Sign-In Information |
| [**UpdateSignInSettings**](AuthInfoApi.md#updatesigninsettings) | **PUT** /sign-in-settings | Update Password Requirements |

<a id="getauthinfo"></a>
# **GetAuthInfo**
> AuthInfo GetAuthInfo ()

Get Authentication Info

Get the post-login SaaS URL that contains authentication information. You can pass authentication information to the URL obtained here and implement this Callback using the SaaSus SDK. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetAuthInfoExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new AuthInfoApi(config);

            try
            {
                // Get Authentication Info
                AuthInfo result = apiInstance.GetAuthInfo();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AuthInfoApi.GetAuthInfo: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAuthInfoWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Authentication Info
    ApiResponse<AuthInfo> response = apiInstance.GetAuthInfoWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AuthInfoApi.GetAuthInfoWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**AuthInfo**](AuthInfo.md)

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

<a id="getidentityproviders"></a>
# **GetIdentityProviders**
> IdentityProviders GetIdentityProviders ()

Get Sign-In Information Via External Provider

Get sign-in information via external provider set in cognito. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetIdentityProvidersExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new AuthInfoApi(config);

            try
            {
                // Get Sign-In Information Via External Provider
                IdentityProviders result = apiInstance.GetIdentityProviders();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AuthInfoApi.GetIdentityProviders: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetIdentityProvidersWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Sign-In Information Via External Provider
    ApiResponse<IdentityProviders> response = apiInstance.GetIdentityProvidersWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AuthInfoApi.GetIdentityProvidersWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**IdentityProviders**](IdentityProviders.md)

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

<a id="getsigninsettings"></a>
# **GetSignInSettings**
> SignInSettings GetSignInSettings ()

Get Password Requirements

Get user password requirements. Set a secure password that is difficult to decipher by increasing the number of digits by combining alphabets, numbers, and symbols. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetSignInSettingsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new AuthInfoApi(config);

            try
            {
                // Get Password Requirements
                SignInSettings result = apiInstance.GetSignInSettings();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AuthInfoApi.GetSignInSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSignInSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Password Requirements
    ApiResponse<SignInSettings> response = apiInstance.GetSignInSettingsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AuthInfoApi.GetSignInSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**SignInSettings**](SignInSettings.md)

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

<a id="updateauthinfo"></a>
# **UpdateAuthInfo**
> void UpdateAuthInfo (AuthInfo body = null)

Update Authentication Info

Register post-login SaaS URL for authentication information. It is possible to pass authentication information to the URL registered here and implement this Callback using the SaaSus SDK. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateAuthInfoExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new AuthInfoApi(config);
            var body = new AuthInfo();  // AuthInfo |  (optional) 

            try
            {
                // Update Authentication Info
                apiInstance.UpdateAuthInfo(body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AuthInfoApi.UpdateAuthInfo: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateAuthInfoWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Authentication Info
    apiInstance.UpdateAuthInfoWithHttpInfo(body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AuthInfoApi.UpdateAuthInfoWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **AuthInfo** |  | [optional]  |

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

<a id="updateidentityprovider"></a>
# **UpdateIdentityProvider**
> void UpdateIdentityProvider (UpdateIdentityProviderParam updateIdentityProviderParam = null)

Update Sign-In Information

Update the sign-in information for the external ID provider

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateIdentityProviderExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new AuthInfoApi(config);
            var updateIdentityProviderParam = new UpdateIdentityProviderParam(); // UpdateIdentityProviderParam |  (optional) 

            try
            {
                // Update Sign-In Information
                apiInstance.UpdateIdentityProvider(updateIdentityProviderParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AuthInfoApi.UpdateIdentityProvider: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateIdentityProviderWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Sign-In Information
    apiInstance.UpdateIdentityProviderWithHttpInfo(updateIdentityProviderParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AuthInfoApi.UpdateIdentityProviderWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateIdentityProviderParam** | [**UpdateIdentityProviderParam**](UpdateIdentityProviderParam.md) |  | [optional]  |

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

<a id="updatesigninsettings"></a>
# **UpdateSignInSettings**
> void UpdateSignInSettings (UpdateSignInSettingsParam updateSignInSettingsParam = null)

Update Password Requirements

Update user password requirements. Set a secure password that is difficult to decipher by increasing the number of digits by combining alphabets, numbers, and symbols. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateSignInSettingsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new AuthInfoApi(config);
            var updateSignInSettingsParam = new UpdateSignInSettingsParam(); // UpdateSignInSettingsParam |  (optional) 

            try
            {
                // Update Password Requirements
                apiInstance.UpdateSignInSettings(updateSignInSettingsParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AuthInfoApi.UpdateSignInSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSignInSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Password Requirements
    apiInstance.UpdateSignInSettingsWithHttpInfo(updateSignInSettingsParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AuthInfoApi.UpdateSignInSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateSignInSettingsParam** | [**UpdateSignInSettingsParam**](UpdateSignInSettingsParam.md) |  | [optional]  |

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

