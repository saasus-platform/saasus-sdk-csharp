# authapi.Api.TenantUserApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateTenantUser**](TenantUserApi.md#createtenantuser) | **POST** /tenants/{tenant_id}/users | Create Tenant User |
| [**CreateTenantUserRoles**](TenantUserApi.md#createtenantuserroles) | **POST** /tenants/{tenant_id}/users/{user_id}/envs/{env_id}/roles | Create Tenant User Role |
| [**DeleteTenantUser**](TenantUserApi.md#deletetenantuser) | **DELETE** /tenants/{tenant_id}/users/{user_id} | Delete Tenant User |
| [**DeleteTenantUserRole**](TenantUserApi.md#deletetenantuserrole) | **DELETE** /tenants/{tenant_id}/users/{user_id}/envs/{env_id}/roles/{role_name} | Remove Role From Tenant User |
| [**GetAllTenantUser**](TenantUserApi.md#getalltenantuser) | **GET** /tenants/all/users/{user_id} | Get User Info |
| [**GetAllTenantUsers**](TenantUserApi.md#getalltenantusers) | **GET** /tenants/all/users | Get Users |
| [**GetTenantUser**](TenantUserApi.md#gettenantuser) | **GET** /tenants/{tenant_id}/users/{user_id} | Get Tenant User |
| [**GetTenantUsers**](TenantUserApi.md#gettenantusers) | **GET** /tenants/{tenant_id}/users | Get Tenant Users |
| [**UpdateTenantUser**](TenantUserApi.md#updatetenantuser) | **PATCH** /tenants/{tenant_id}/users/{user_id} | Update Tenant User Attribute |

<a id="createtenantuser"></a>
# **CreateTenantUser**
> User CreateTenantUser (string tenantId, CreateTenantUserParam createTenantUserParam = null)

Create Tenant User

Create a tenant user. If attributes is empty, the additional attributes will be created empty. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateTenantUserExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantUserApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var createTenantUserParam = new CreateTenantUserParam(); // CreateTenantUserParam |  (optional) 

            try
            {
                // Create Tenant User
                User result = apiInstance.CreateTenantUser(tenantId, createTenantUserParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantUserApi.CreateTenantUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateTenantUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Tenant User
    ApiResponse<User> response = apiInstance.CreateTenantUserWithHttpInfo(tenantId, createTenantUserParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantUserApi.CreateTenantUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **createTenantUserParam** | [**CreateTenantUserParam**](CreateTenantUserParam.md) |  | [optional]  |

### Return type

[**User**](User.md)

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

<a id="createtenantuserroles"></a>
# **CreateTenantUserRoles**
> void CreateTenantUserRoles (string tenantId, string userId, int envId, CreateTenantUserRolesParam createTenantUserRolesParam = null)

Create Tenant User Role

Create roles on tenant users. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateTenantUserRolesExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantUserApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var envId = 56;  // int | Env ID
            var createTenantUserRolesParam = new CreateTenantUserRolesParam(); // CreateTenantUserRolesParam |  (optional) 

            try
            {
                // Create Tenant User Role
                apiInstance.CreateTenantUserRoles(tenantId, userId, envId, createTenantUserRolesParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantUserApi.CreateTenantUserRoles: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateTenantUserRolesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Tenant User Role
    apiInstance.CreateTenantUserRolesWithHttpInfo(tenantId, userId, envId, createTenantUserRolesParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantUserApi.CreateTenantUserRolesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **userId** | **string** | User ID |  |
| **envId** | **int** | Env ID |  |
| **createTenantUserRolesParam** | [**CreateTenantUserRolesParam**](CreateTenantUserRolesParam.md) |  | [optional]  |

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

<a id="deletetenantuser"></a>
# **DeleteTenantUser**
> void DeleteTenantUser (string tenantId, string userId)

Delete Tenant User

Delete a user from the tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class DeleteTenantUserExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantUserApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID

            try
            {
                // Delete Tenant User
                apiInstance.DeleteTenantUser(tenantId, userId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantUserApi.DeleteTenantUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteTenantUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Tenant User
    apiInstance.DeleteTenantUserWithHttpInfo(tenantId, userId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantUserApi.DeleteTenantUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
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
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletetenantuserrole"></a>
# **DeleteTenantUserRole**
> void DeleteTenantUserRole (string tenantId, string userId, int envId, string roleName)

Remove Role From Tenant User

Remove a role from a tenant user. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class DeleteTenantUserRoleExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantUserApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var envId = 56;  // int | Env ID
            var roleName = admin;  // string | Role name

            try
            {
                // Remove Role From Tenant User
                apiInstance.DeleteTenantUserRole(tenantId, userId, envId, roleName);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantUserApi.DeleteTenantUserRole: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteTenantUserRoleWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Remove Role From Tenant User
    apiInstance.DeleteTenantUserRoleWithHttpInfo(tenantId, userId, envId, roleName);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantUserApi.DeleteTenantUserRoleWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **userId** | **string** | User ID |  |
| **envId** | **int** | Env ID |  |
| **roleName** | **string** | Role name |  |

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

<a id="getalltenantuser"></a>
# **GetAllTenantUser**
> Users GetAllTenantUser (string userId)

Get User Info

Get information on user belonging to the tenant from the user ID. If the user belongs to multiple tenants, it will be returned as another object. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetAllTenantUserExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantUserApi(config);
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID

            try
            {
                // Get User Info
                Users result = apiInstance.GetAllTenantUser(userId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantUserApi.GetAllTenantUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllTenantUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get User Info
    ApiResponse<Users> response = apiInstance.GetAllTenantUserWithHttpInfo(userId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantUserApi.GetAllTenantUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **userId** | **string** | User ID |  |

### Return type

[**Users**](Users.md)

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

<a id="getalltenantusers"></a>
# **GetAllTenantUsers**
> Users GetAllTenantUsers ()

Get Users

Get all users belonging to the tenant. The same user belonging to multiple tenants will be returned as a different object. Id is not unique. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetAllTenantUsersExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantUserApi(config);

            try
            {
                // Get Users
                Users result = apiInstance.GetAllTenantUsers();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantUserApi.GetAllTenantUsers: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetAllTenantUsersWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Users
    ApiResponse<Users> response = apiInstance.GetAllTenantUsersWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantUserApi.GetAllTenantUsersWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**Users**](Users.md)

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

<a id="gettenantuser"></a>
# **GetTenantUser**
> User GetTenantUser (string tenantId, string userId)

Get Tenant User

Get one tenant user by specific ID. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetTenantUserExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantUserApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID

            try
            {
                // Get Tenant User
                User result = apiInstance.GetTenantUser(tenantId, userId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantUserApi.GetTenantUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Tenant User
    ApiResponse<User> response = apiInstance.GetTenantUserWithHttpInfo(tenantId, userId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantUserApi.GetTenantUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **userId** | **string** | User ID |  |

### Return type

[**User**](User.md)

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

<a id="gettenantusers"></a>
# **GetTenantUsers**
> Users GetTenantUsers (string tenantId)

Get Tenant Users

Get all the users belonging to the tenant. Id is unique. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetTenantUsersExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantUserApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID

            try
            {
                // Get Tenant Users
                Users result = apiInstance.GetTenantUsers(tenantId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantUserApi.GetTenantUsers: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantUsersWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Tenant Users
    ApiResponse<Users> response = apiInstance.GetTenantUsersWithHttpInfo(tenantId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantUserApi.GetTenantUsersWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |

### Return type

[**Users**](Users.md)

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

<a id="updatetenantuser"></a>
# **UpdateTenantUser**
> void UpdateTenantUser (string tenantId, string userId, UpdateTenantUserParam updateTenantUserParam = null)

Update Tenant User Attribute

Update tenant user attributes. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateTenantUserExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantUserApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var userId = f94bfffc-8be2-11ec-b41a-0242ac120004;  // string | User ID
            var updateTenantUserParam = new UpdateTenantUserParam(); // UpdateTenantUserParam |  (optional) 

            try
            {
                // Update Tenant User Attribute
                apiInstance.UpdateTenantUser(tenantId, userId, updateTenantUserParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantUserApi.UpdateTenantUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateTenantUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Tenant User Attribute
    apiInstance.UpdateTenantUserWithHttpInfo(tenantId, userId, updateTenantUserParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantUserApi.UpdateTenantUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **userId** | **string** | User ID |  |
| **updateTenantUserParam** | [**UpdateTenantUserParam**](UpdateTenantUserParam.md) |  | [optional]  |

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

