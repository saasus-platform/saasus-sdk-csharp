# authapi.Api.SaasUserApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**ConfirmDevice**](SaasUserApi.md#confirmdevice) | **POST** /device/confirm | Confirm Device |
| [**ConfirmEmailUpdate**](SaasUserApi.md#confirmemailupdate) | **POST** /users/{user_id}/email/confirm | Confirm User Email Update |
| [**ConfirmExternalUserLink**](SaasUserApi.md#confirmexternaluserlink) | **POST** /external-users/confirm | Confirm External User Account Link |
| [**ConfirmSignUpWithAwsMarketplace**](SaasUserApi.md#confirmsignupwithawsmarketplace) | **POST** /aws-marketplace/sign-up-confirm | Confirm Sign Up with AWS Marketplace |
| [**CreateSaasUser**](SaasUserApi.md#createsaasuser) | **POST** /users | Create SaaS User |
| [**CreateSecretCode**](SaasUserApi.md#createsecretcode) | **POST** /users/{user_id}/mfa/software-token/secret-code | Create secret code for authentication application registration |
| [**DeleteSaasUser**](SaasUserApi.md#deletesaasuser) | **DELETE** /users/{user_id} | Delete User |
| [**GetSaasUser**](SaasUserApi.md#getsaasuser) | **GET** /users/{user_id} | Get User |
| [**GetSaasUsers**](SaasUserApi.md#getsaasusers) | **GET** /users | Get Users |
| [**GetSaasUsersCount**](SaasUserApi.md#getsaasuserscount) | **GET** /users/count | Get SaaS Users Count |
| [**GetUserMfaPreference**](SaasUserApi.md#getusermfapreference) | **GET** /users/{user_id}/mfa/preference | Get User&#39;s MFA Settings |
| [**LinkAwsMarketplace**](SaasUserApi.md#linkawsmarketplace) | **PATCH** /aws-marketplace/link | Link an existing tenant with AWS Marketplace |
| [**RequestEmailUpdate**](SaasUserApi.md#requestemailupdate) | **POST** /users/{user_id}/email/request | Request User Email Update |
| [**RequestExternalUserLink**](SaasUserApi.md#requestexternaluserlink) | **POST** /external-users/request | Request External User Account Link |
| [**ResendSignUpConfirmationEmail**](SaasUserApi.md#resendsignupconfirmationemail) | **POST** /sign-up/resend | Resend Sign Up Confirmation Email |
| [**ResetSaasUserPassword**](SaasUserApi.md#resetsaasuserpassword) | **POST** /users/{user_id}/password/reset | Reset Password |
| [**RespondToSignInChallenge**](SaasUserApi.md#respondtosigninchallenge) | **POST** /sign-in/challenge | Respond to Sign In Challenge |
| [**SaveSaasUsersCount**](SaasUserApi.md#savesaasuserscount) | **POST** /users/count | Save SaaS Users Count |
| [**SearchSaasUsers**](SaasUserApi.md#searchsaasusers) | **GET** /users/search | Search SaaS Users |
| [**SignIn**](SaasUserApi.md#signin) | **POST** /sign-in | Sign In |
| [**SignUp**](SaasUserApi.md#signup) | **POST** /sign-up | Sign Up |
| [**SignUpWithAwsMarketplace**](SaasUserApi.md#signupwithawsmarketplace) | **POST** /aws-marketplace/sign-up | Sign Up with AWS Marketplace |
| [**UnlinkProvider**](SaasUserApi.md#unlinkprovider) | **DELETE** /users/{user_id}/providers/{provider_name} | Unlink external identity providers |
| [**UpdateDeviceStatus**](SaasUserApi.md#updatedevicestatus) | **POST** /device/status | Update Device Status |
| [**UpdateSaasUserAttributes**](SaasUserApi.md#updatesaasuserattributes) | **PATCH** /users/{user_id}/attributes | Update SaaS User Attributes |
| [**UpdateSaasUserEmail**](SaasUserApi.md#updatesaasuseremail) | **PATCH** /users/{user_id}/email | Change Email |
| [**UpdateSaasUserPassword**](SaasUserApi.md#updatesaasuserpassword) | **PATCH** /users/{user_id}/password | Change Password |
| [**UpdateSaasUserSignInId**](SaasUserApi.md#updatesaasusersigninid) | **PATCH** /users/{user_id}/sign-in-id | Change Sign-in ID |
| [**UpdateSoftwareToken**](SaasUserApi.md#updatesoftwaretoken) | **PUT** /users/{user_id}/mfa/software-token | Register Authentication Application |
| [**UpdateUserMfaPreference**](SaasUserApi.md#updateusermfapreference) | **PATCH** /users/{user_id}/mfa/preference | Update User&#39;s MFA Settings |

<a id="confirmdevice"></a>
# **ConfirmDevice**
> ConfirmDeviceResult ConfirmDevice (ConfirmDeviceParam confirmDeviceParam = null)

Confirm Device

Confirms a device for remembering. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class ConfirmDeviceExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var confirmDeviceParam = new ConfirmDeviceParam(); // ConfirmDeviceParam |  (optional) 

            try
            {
                // Confirm Device
                ConfirmDeviceResult result = apiInstance.ConfirmDevice(confirmDeviceParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.ConfirmDevice: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ConfirmDeviceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Confirm Device
    ApiResponse<ConfirmDeviceResult> response = apiInstance.ConfirmDeviceWithHttpInfo(confirmDeviceParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.ConfirmDeviceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **confirmDeviceParam** | [**ConfirmDeviceParam**](ConfirmDeviceParam.md) |  | [optional]  |

### Return type

[**ConfirmDeviceResult**](ConfirmDeviceResult.md)

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
| **401** | Unauthorized |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="confirmemailupdate"></a>
# **ConfirmEmailUpdate**
> void ConfirmEmailUpdate (string userId, ConfirmEmailUpdateParam confirmEmailUpdateParam = null)

Confirm User Email Update

Verify the code to confirm the user's email address update. Requires the user's access token. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class ConfirmEmailUpdateExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var confirmEmailUpdateParam = new ConfirmEmailUpdateParam(); // ConfirmEmailUpdateParam |  (optional) 

            try
            {
                // Confirm User Email Update
                apiInstance.ConfirmEmailUpdate(userId, confirmEmailUpdateParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.ConfirmEmailUpdate: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ConfirmEmailUpdateWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Confirm User Email Update
    apiInstance.ConfirmEmailUpdateWithHttpInfo(userId, confirmEmailUpdateParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.ConfirmEmailUpdateWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |
| **confirmEmailUpdateParam** | [**ConfirmEmailUpdateParam**](ConfirmEmailUpdateParam.md) |  | [optional]  |

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

<a id="confirmexternaluserlink"></a>
# **ConfirmExternalUserLink**
> void ConfirmExternalUserLink (ConfirmExternalUserLinkParam confirmExternalUserLinkParam = null)

Confirm External User Account Link

Verify the code for external account user link confirmation. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class ConfirmExternalUserLinkExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var confirmExternalUserLinkParam = new ConfirmExternalUserLinkParam(); // ConfirmExternalUserLinkParam |  (optional) 

            try
            {
                // Confirm External User Account Link
                apiInstance.ConfirmExternalUserLink(confirmExternalUserLinkParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.ConfirmExternalUserLink: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ConfirmExternalUserLinkWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Confirm External User Account Link
    apiInstance.ConfirmExternalUserLinkWithHttpInfo(confirmExternalUserLinkParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.ConfirmExternalUserLinkWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **confirmExternalUserLinkParam** | [**ConfirmExternalUserLinkParam**](ConfirmExternalUserLinkParam.md) |  | [optional]  |

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

<a id="confirmsignupwithawsmarketplace"></a>
# **ConfirmSignUpWithAwsMarketplace**
> Tenant ConfirmSignUpWithAwsMarketplace (ConfirmSignUpWithAwsMarketplaceParam confirmSignUpWithAwsMarketplaceParam = null)

Confirm Sign Up with AWS Marketplace

Confirm a new use registeration linked to AWS Marketplace. Create a new tenant linked to AWS Marketplace. If the Registration Token is not valid, an error is returned. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class ConfirmSignUpWithAwsMarketplaceExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var confirmSignUpWithAwsMarketplaceParam = new ConfirmSignUpWithAwsMarketplaceParam(); // ConfirmSignUpWithAwsMarketplaceParam |  (optional) 

            try
            {
                // Confirm Sign Up with AWS Marketplace
                Tenant result = apiInstance.ConfirmSignUpWithAwsMarketplace(confirmSignUpWithAwsMarketplaceParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.ConfirmSignUpWithAwsMarketplace: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ConfirmSignUpWithAwsMarketplaceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Confirm Sign Up with AWS Marketplace
    ApiResponse<Tenant> response = apiInstance.ConfirmSignUpWithAwsMarketplaceWithHttpInfo(confirmSignUpWithAwsMarketplaceParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.ConfirmSignUpWithAwsMarketplaceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **confirmSignUpWithAwsMarketplaceParam** | [**ConfirmSignUpWithAwsMarketplaceParam**](ConfirmSignUpWithAwsMarketplaceParam.md) |  | [optional]  |

### Return type

[**Tenant**](Tenant.md)

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

<a id="createsaasuser"></a>
# **CreateSaasUser**
> CreatedSaasUser CreateSaasUser (CreateSaasUserParam createSaasUserParam = null)

Create SaaS User

Create SaaS User. If attributes is empty, a temporary password will be sent to the registered email. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateSaasUserExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var createSaasUserParam = new CreateSaasUserParam(); // CreateSaasUserParam |  (optional) 

            try
            {
                // Create SaaS User
                CreatedSaasUser result = apiInstance.CreateSaasUser(createSaasUserParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.CreateSaasUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateSaasUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create SaaS User
    ApiResponse<CreatedSaasUser> response = apiInstance.CreateSaasUserWithHttpInfo(createSaasUserParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.CreateSaasUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createSaasUserParam** | [**CreateSaasUserParam**](CreateSaasUserParam.md) |  | [optional]  |

### Return type

[**CreatedSaasUser**](CreatedSaasUser.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | OK |  -  |
| **400** | Bad Request |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="createsecretcode"></a>
# **CreateSecretCode**
> SoftwareTokenSecretCode CreateSecretCode (string userId, CreateSecretCodeParam createSecretCodeParam = null)

Create secret code for authentication application registration

Create a secret code for authentication application registration. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateSecretCodeExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var createSecretCodeParam = new CreateSecretCodeParam(); // CreateSecretCodeParam |  (optional) 

            try
            {
                // Create secret code for authentication application registration
                SoftwareTokenSecretCode result = apiInstance.CreateSecretCode(userId, createSecretCodeParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.CreateSecretCode: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateSecretCodeWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create secret code for authentication application registration
    ApiResponse<SoftwareTokenSecretCode> response = apiInstance.CreateSecretCodeWithHttpInfo(userId, createSecretCodeParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.CreateSecretCodeWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |
| **createSecretCodeParam** | [**CreateSecretCodeParam**](CreateSecretCodeParam.md) |  | [optional]  |

### Return type

[**SoftwareTokenSecretCode**](SoftwareTokenSecretCode.md)

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

<a id="deletesaasuser"></a>
# **DeleteSaasUser**
> UserInfo DeleteSaasUser (string userId)

Delete User

Delete all users with matching user ID from the tenant and SaaS. Returns user information before deletion. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class DeleteSaasUserExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID

            try
            {
                // Delete User
                UserInfo result = apiInstance.DeleteSaasUser(userId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.DeleteSaasUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteSaasUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete User
    ApiResponse<UserInfo> response = apiInstance.DeleteSaasUserWithHttpInfo(userId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.DeleteSaasUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |

### Return type

[**UserInfo**](UserInfo.md)

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

<a id="getsaasuser"></a>
# **GetSaasUser**
> SaasUser GetSaasUser (string userId)

Get User

Get user information based on user ID. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetSaasUserExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID

            try
            {
                // Get User
                SaasUser result = apiInstance.GetSaasUser(userId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.GetSaasUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSaasUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get User
    ApiResponse<SaasUser> response = apiInstance.GetSaasUserWithHttpInfo(userId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.GetSaasUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |

### Return type

[**SaasUser**](SaasUser.md)

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

<a id="getsaasusers"></a>
# **GetSaasUsers**
> SaasUsers GetSaasUsers ()

Get Users

Get all SaaS users. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetSaasUsersExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);

            try
            {
                // Get Users
                SaasUsers result = apiInstance.GetSaasUsers();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.GetSaasUsers: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSaasUsersWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Users
    ApiResponse<SaasUsers> response = apiInstance.GetSaasUsersWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.GetSaasUsersWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**SaasUsers**](SaasUsers.md)

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

<a id="getsaasuserscount"></a>
# **GetSaasUsersCount**
> SaasUsersCount GetSaasUsersCount ()

Get SaaS Users Count

Get the count of SaaS users. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetSaasUsersCountExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);

            try
            {
                // Get SaaS Users Count
                SaasUsersCount result = apiInstance.GetSaasUsersCount();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.GetSaasUsersCount: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSaasUsersCountWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get SaaS Users Count
    ApiResponse<SaasUsersCount> response = apiInstance.GetSaasUsersCountWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.GetSaasUsersCountWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**SaasUsersCount**](SaasUsersCount.md)

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

<a id="getusermfapreference"></a>
# **GetUserMfaPreference**
> MfaPreference GetUserMfaPreference (string userId)

Get User's MFA Settings

Get the user's MFA settings. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetUserMfaPreferenceExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID

            try
            {
                // Get User's MFA Settings
                MfaPreference result = apiInstance.GetUserMfaPreference(userId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.GetUserMfaPreference: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetUserMfaPreferenceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get User's MFA Settings
    ApiResponse<MfaPreference> response = apiInstance.GetUserMfaPreferenceWithHttpInfo(userId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.GetUserMfaPreferenceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |

### Return type

[**MfaPreference**](MfaPreference.md)

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

<a id="linkawsmarketplace"></a>
# **LinkAwsMarketplace**
> void LinkAwsMarketplace (LinkAwsMarketplaceParam linkAwsMarketplaceParam = null)

Link an existing tenant with AWS Marketplace

Link an existing tenant with AWS Marketplace. If the Registration Token is not valid, an error is returned. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class LinkAwsMarketplaceExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var linkAwsMarketplaceParam = new LinkAwsMarketplaceParam(); // LinkAwsMarketplaceParam |  (optional) 

            try
            {
                // Link an existing tenant with AWS Marketplace
                apiInstance.LinkAwsMarketplace(linkAwsMarketplaceParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.LinkAwsMarketplace: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the LinkAwsMarketplaceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Link an existing tenant with AWS Marketplace
    apiInstance.LinkAwsMarketplaceWithHttpInfo(linkAwsMarketplaceParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.LinkAwsMarketplaceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **linkAwsMarketplaceParam** | [**LinkAwsMarketplaceParam**](LinkAwsMarketplaceParam.md) |  | [optional]  |

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

<a id="requestemailupdate"></a>
# **RequestEmailUpdate**
> void RequestEmailUpdate (string userId, RequestEmailUpdateParam requestEmailUpdateParam = null)

Request User Email Update

Request to update the user's email address. Sends a verification code to the requested email address. Requires the user's access token. The verification code is valid for 24 hours. This API is only available for email-authenticated users. Sign-in ID authentication users cannot use this API. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class RequestEmailUpdateExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var requestEmailUpdateParam = new RequestEmailUpdateParam(); // RequestEmailUpdateParam |  (optional) 

            try
            {
                // Request User Email Update
                apiInstance.RequestEmailUpdate(userId, requestEmailUpdateParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.RequestEmailUpdate: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RequestEmailUpdateWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Request User Email Update
    apiInstance.RequestEmailUpdateWithHttpInfo(userId, requestEmailUpdateParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.RequestEmailUpdateWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |
| **requestEmailUpdateParam** | [**RequestEmailUpdateParam**](RequestEmailUpdateParam.md) |  | [optional]  |

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

<a id="requestexternaluserlink"></a>
# **RequestExternalUserLink**
> void RequestExternalUserLink (RequestExternalUserLinkParam requestExternalUserLinkParam = null)

Request External User Account Link

Request to link an external account user. Get the email address of the user to be linked from the access token and send a verification code to that email address. The verification code is valid for 24 hours. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class RequestExternalUserLinkExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var requestExternalUserLinkParam = new RequestExternalUserLinkParam(); // RequestExternalUserLinkParam |  (optional) 

            try
            {
                // Request External User Account Link
                apiInstance.RequestExternalUserLink(requestExternalUserLinkParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.RequestExternalUserLink: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RequestExternalUserLinkWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Request External User Account Link
    apiInstance.RequestExternalUserLinkWithHttpInfo(requestExternalUserLinkParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.RequestExternalUserLinkWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **requestExternalUserLinkParam** | [**RequestExternalUserLinkParam**](RequestExternalUserLinkParam.md) |  | [optional]  |

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

<a id="resendsignupconfirmationemail"></a>
# **ResendSignUpConfirmationEmail**
> void ResendSignUpConfirmationEmail (ResendSignUpConfirmationEmailParam resendSignUpConfirmationEmailParam = null)

Resend Sign Up Confirmation Email

Resend temporary password for the new registered user. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class ResendSignUpConfirmationEmailExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var resendSignUpConfirmationEmailParam = new ResendSignUpConfirmationEmailParam(); // ResendSignUpConfirmationEmailParam |  (optional) 

            try
            {
                // Resend Sign Up Confirmation Email
                apiInstance.ResendSignUpConfirmationEmail(resendSignUpConfirmationEmailParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.ResendSignUpConfirmationEmail: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ResendSignUpConfirmationEmailWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Resend Sign Up Confirmation Email
    apiInstance.ResendSignUpConfirmationEmailWithHttpInfo(resendSignUpConfirmationEmailParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.ResendSignUpConfirmationEmailWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **resendSignUpConfirmationEmailParam** | [**ResendSignUpConfirmationEmailParam**](ResendSignUpConfirmationEmailParam.md) |  | [optional]  |

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

<a id="resetsaasuserpassword"></a>
# **ResetSaasUserPassword**
> SaasUserResetPasswordResult ResetSaasUserPassword (string userId)

Reset Password

Reset user's login password. The current password will be invalidated and a temporary password will be issued. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class ResetSaasUserPasswordExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID

            try
            {
                // Reset Password
                SaasUserResetPasswordResult result = apiInstance.ResetSaasUserPassword(userId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.ResetSaasUserPassword: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ResetSaasUserPasswordWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Reset Password
    ApiResponse<SaasUserResetPasswordResult> response = apiInstance.ResetSaasUserPasswordWithHttpInfo(userId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.ResetSaasUserPasswordWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |

### Return type

[**SaasUserResetPasswordResult**](SaasUserResetPasswordResult.md)

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

<a id="respondtosigninchallenge"></a>
# **RespondToSignInChallenge**
> RespondToSignInChallengeResult RespondToSignInChallenge (RespondToSignInChallengeParam respondToSignInChallengeParam = null)

Respond to Sign In Challenge

Respond to a sign-in challenge. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class RespondToSignInChallengeExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var respondToSignInChallengeParam = new RespondToSignInChallengeParam(); // RespondToSignInChallengeParam |  (optional) 

            try
            {
                // Respond to Sign In Challenge
                RespondToSignInChallengeResult result = apiInstance.RespondToSignInChallenge(respondToSignInChallengeParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.RespondToSignInChallenge: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the RespondToSignInChallengeWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Respond to Sign In Challenge
    ApiResponse<RespondToSignInChallengeResult> response = apiInstance.RespondToSignInChallengeWithHttpInfo(respondToSignInChallengeParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.RespondToSignInChallengeWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **respondToSignInChallengeParam** | [**RespondToSignInChallengeParam**](RespondToSignInChallengeParam.md) |  | [optional]  |

### Return type

[**RespondToSignInChallengeResult**](RespondToSignInChallengeResult.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **401** | Unauthorized |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="savesaasuserscount"></a>
# **SaveSaasUsersCount**
> void SaveSaasUsersCount (SaveSaasUsersCountParam saveSaasUsersCountParam)

Save SaaS Users Count

Save the count of SaaS users. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class SaveSaasUsersCountExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var saveSaasUsersCountParam = new SaveSaasUsersCountParam(); // SaveSaasUsersCountParam | 

            try
            {
                // Save SaaS Users Count
                apiInstance.SaveSaasUsersCount(saveSaasUsersCountParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.SaveSaasUsersCount: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SaveSaasUsersCountWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Save SaaS Users Count
    apiInstance.SaveSaasUsersCountWithHttpInfo(saveSaasUsersCountParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.SaveSaasUsersCountWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **saveSaasUsersCountParam** | [**SaveSaasUsersCountParam**](SaveSaasUsersCountParam.md) |  |  |

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
| **501** | Not Implemented |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="searchsaasusers"></a>
# **SearchSaasUsers**
> SearchSaasUsersResult SearchSaasUsers (string id = null, string email = null, string signInId = null, long? limit = null, string cursor = null)

Search SaaS Users

Search SaaS users by user ID, email, or sign-in ID. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class SearchSaasUsersExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var id = "id_example";  // string | User ID (optional) 
            var email = "email_example";  // string | Email prefix (optional) 
            var signInId = "signInId_example";  // string | Sign-in ID prefix (optional) 
            var limit = 789L;  // long? | Maximum number of items to retrieve (optional) 
            var cursor = "cursor_example";  // string | Cursor for cursor pagination (optional) 

            try
            {
                // Search SaaS Users
                SearchSaasUsersResult result = apiInstance.SearchSaasUsers(id, email, signInId, limit, cursor);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.SearchSaasUsers: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SearchSaasUsersWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Search SaaS Users
    ApiResponse<SearchSaasUsersResult> response = apiInstance.SearchSaasUsersWithHttpInfo(id, email, signInId, limit, cursor);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.SearchSaasUsersWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **id** | **string** | User ID | [optional]  |
| **email** | **string** | Email prefix | [optional]  |
| **signInId** | **string** | Sign-in ID prefix | [optional]  |
| **limit** | **long?** | Maximum number of items to retrieve | [optional]  |
| **cursor** | **string** | Cursor for cursor pagination | [optional]  |

### Return type

[**SearchSaasUsersResult**](SearchSaasUsersResult.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **400** | Bad Request |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="signin"></a>
# **SignIn**
> SignInResult SignIn (SignInParam signInParam = null)

Sign In

A user attempts to sign in. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class SignInExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var signInParam = new SignInParam(); // SignInParam |  (optional) 

            try
            {
                // Sign In
                SignInResult result = apiInstance.SignIn(signInParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.SignIn: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SignInWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Sign In
    ApiResponse<SignInResult> response = apiInstance.SignInWithHttpInfo(signInParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.SignInWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **signInParam** | [**SignInParam**](SignInParam.md) |  | [optional]  |

### Return type

[**SignInResult**](SignInResult.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **401** | Unauthorized |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="signup"></a>
# **SignUp**
> SaasUser SignUp (SignUpParam signUpParam = null)

Sign Up

Register a new user. A temporary password will be sent to the registered email. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class SignUpExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var signUpParam = new SignUpParam(); // SignUpParam |  (optional) 

            try
            {
                // Sign Up
                SaasUser result = apiInstance.SignUp(signUpParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.SignUp: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SignUpWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Sign Up
    ApiResponse<SaasUser> response = apiInstance.SignUpWithHttpInfo(signUpParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.SignUpWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **signUpParam** | [**SignUpParam**](SignUpParam.md) |  | [optional]  |

### Return type

[**SaasUser**](SaasUser.md)

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

<a id="signupwithawsmarketplace"></a>
# **SignUpWithAwsMarketplace**
> SaasUser SignUpWithAwsMarketplace (SignUpWithAwsMarketplaceParam signUpWithAwsMarketplaceParam = null)

Sign Up with AWS Marketplace

Register a new user linked to AWS Marketplace. A temporary password will be sent to the registered email. If the Registration Token is not valid, an error is returned. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class SignUpWithAwsMarketplaceExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var signUpWithAwsMarketplaceParam = new SignUpWithAwsMarketplaceParam(); // SignUpWithAwsMarketplaceParam |  (optional) 

            try
            {
                // Sign Up with AWS Marketplace
                SaasUser result = apiInstance.SignUpWithAwsMarketplace(signUpWithAwsMarketplaceParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.SignUpWithAwsMarketplace: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SignUpWithAwsMarketplaceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Sign Up with AWS Marketplace
    ApiResponse<SaasUser> response = apiInstance.SignUpWithAwsMarketplaceWithHttpInfo(signUpWithAwsMarketplaceParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.SignUpWithAwsMarketplaceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **signUpWithAwsMarketplaceParam** | [**SignUpWithAwsMarketplaceParam**](SignUpWithAwsMarketplaceParam.md) |  | [optional]  |

### Return type

[**SaasUser**](SaasUser.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="unlinkprovider"></a>
# **UnlinkProvider**
> void UnlinkProvider (string providerName, string userId)

Unlink external identity providers

Unlink external identity providers. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UnlinkProviderExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var providerName = Google;  // string | 
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID

            try
            {
                // Unlink external identity providers
                apiInstance.UnlinkProvider(providerName, userId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.UnlinkProvider: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UnlinkProviderWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Unlink external identity providers
    apiInstance.UnlinkProviderWithHttpInfo(providerName, userId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.UnlinkProviderWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **providerName** | **string** |  |  |
| **userId** | **string** | User ID |  |

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

<a id="updatedevicestatus"></a>
# **UpdateDeviceStatus**
> void UpdateDeviceStatus (UpdateDeviceStatusParam updateDeviceStatusParam = null)

Update Device Status

Updates the device status. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateDeviceStatusExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var updateDeviceStatusParam = new UpdateDeviceStatusParam(); // UpdateDeviceStatusParam |  (optional) 

            try
            {
                // Update Device Status
                apiInstance.UpdateDeviceStatus(updateDeviceStatusParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.UpdateDeviceStatus: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateDeviceStatusWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Device Status
    apiInstance.UpdateDeviceStatusWithHttpInfo(updateDeviceStatusParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.UpdateDeviceStatusWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateDeviceStatusParam** | [**UpdateDeviceStatusParam**](UpdateDeviceStatusParam.md) |  | [optional]  |

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
| **401** | Unauthorized |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="updatesaasuserattributes"></a>
# **UpdateSaasUserAttributes**
> void UpdateSaasUserAttributes (string userId, UpdateSaasUserAttributesParam updateSaasUserAttributesParam = null)

Update SaaS User Attributes

Update the additional attributes of the SaaS user. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateSaasUserAttributesExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var updateSaasUserAttributesParam = new UpdateSaasUserAttributesParam(); // UpdateSaasUserAttributesParam |  (optional) 

            try
            {
                // Update SaaS User Attributes
                apiInstance.UpdateSaasUserAttributes(userId, updateSaasUserAttributesParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.UpdateSaasUserAttributes: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSaasUserAttributesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update SaaS User Attributes
    apiInstance.UpdateSaasUserAttributesWithHttpInfo(userId, updateSaasUserAttributesParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.UpdateSaasUserAttributesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |
| **updateSaasUserAttributesParam** | [**UpdateSaasUserAttributesParam**](UpdateSaasUserAttributesParam.md) |  | [optional]  |

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

<a id="updatesaasuseremail"></a>
# **UpdateSaasUserEmail**
> void UpdateSaasUserEmail (string userId, UpdateSaasUserEmailParam updateSaasUserEmailParam = null)

Change Email

Change user's email. The user must be an email authentication user. Sign-in ID authentication users cannot change their email. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateSaasUserEmailExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var updateSaasUserEmailParam = new UpdateSaasUserEmailParam(); // UpdateSaasUserEmailParam |  (optional) 

            try
            {
                // Change Email
                apiInstance.UpdateSaasUserEmail(userId, updateSaasUserEmailParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.UpdateSaasUserEmail: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSaasUserEmailWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Change Email
    apiInstance.UpdateSaasUserEmailWithHttpInfo(userId, updateSaasUserEmailParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.UpdateSaasUserEmailWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |
| **updateSaasUserEmailParam** | [**UpdateSaasUserEmailParam**](UpdateSaasUserEmailParam.md) |  | [optional]  |

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

<a id="updatesaasuserpassword"></a>
# **UpdateSaasUserPassword**
> void UpdateSaasUserPassword (string userId, UpdateSaasUserPasswordParam updateSaasUserPasswordParam = null)

Change Password

Change user's login password. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateSaasUserPasswordExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var updateSaasUserPasswordParam = new UpdateSaasUserPasswordParam(); // UpdateSaasUserPasswordParam |  (optional) 

            try
            {
                // Change Password
                apiInstance.UpdateSaasUserPassword(userId, updateSaasUserPasswordParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.UpdateSaasUserPassword: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSaasUserPasswordWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Change Password
    apiInstance.UpdateSaasUserPasswordWithHttpInfo(userId, updateSaasUserPasswordParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.UpdateSaasUserPasswordWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |
| **updateSaasUserPasswordParam** | [**UpdateSaasUserPasswordParam**](UpdateSaasUserPasswordParam.md) |  | [optional]  |

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

<a id="updatesaasusersigninid"></a>
# **UpdateSaasUserSignInId**
> void UpdateSaasUserSignInId (string userId, UpdateSaasUserSignInIdParam updateSaasUserSignInIdParam = null)

Change Sign-in ID

Change user's sign-in ID. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateSaasUserSignInIdExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var updateSaasUserSignInIdParam = new UpdateSaasUserSignInIdParam(); // UpdateSaasUserSignInIdParam |  (optional) 

            try
            {
                // Change Sign-in ID
                apiInstance.UpdateSaasUserSignInId(userId, updateSaasUserSignInIdParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.UpdateSaasUserSignInId: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSaasUserSignInIdWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Change Sign-in ID
    apiInstance.UpdateSaasUserSignInIdWithHttpInfo(userId, updateSaasUserSignInIdParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.UpdateSaasUserSignInIdWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |
| **updateSaasUserSignInIdParam** | [**UpdateSaasUserSignInIdParam**](UpdateSaasUserSignInIdParam.md) |  | [optional]  |

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

<a id="updatesoftwaretoken"></a>
# **UpdateSoftwareToken**
> void UpdateSoftwareToken (string userId, UpdateSoftwareTokenParam updateSoftwareTokenParam = null)

Register Authentication Application

Register an authentication application. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateSoftwareTokenExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var updateSoftwareTokenParam = new UpdateSoftwareTokenParam(); // UpdateSoftwareTokenParam |  (optional) 

            try
            {
                // Register Authentication Application
                apiInstance.UpdateSoftwareToken(userId, updateSoftwareTokenParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.UpdateSoftwareToken: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSoftwareTokenWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Register Authentication Application
    apiInstance.UpdateSoftwareTokenWithHttpInfo(userId, updateSoftwareTokenParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.UpdateSoftwareTokenWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |
| **updateSoftwareTokenParam** | [**UpdateSoftwareTokenParam**](UpdateSoftwareTokenParam.md) |  | [optional]  |

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

<a id="updateusermfapreference"></a>
# **UpdateUserMfaPreference**
> void UpdateUserMfaPreference (string userId, MfaPreference body = null)

Update User's MFA Settings

Update user's MFA settings. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateUserMfaPreferenceExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new SaasUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var body = new MfaPreference();  // MfaPreference |  (optional) 

            try
            {
                // Update User's MFA Settings
                apiInstance.UpdateUserMfaPreference(userId, body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling SaasUserApi.UpdateUserMfaPreference: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateUserMfaPreferenceWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update User's MFA Settings
    apiInstance.UpdateUserMfaPreferenceWithHttpInfo(userId, body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling SaasUserApi.UpdateUserMfaPreferenceWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |
| **body** | **MfaPreference** |  | [optional]  |

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

