# authapi.Api.RoleApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateRole**](RoleApi.md#createrole) | **POST** /roles | Create Role |
| [**DeleteRole**](RoleApi.md#deleterole) | **DELETE** /roles/{role_name} | Delete Role |
| [**GetRoles**](RoleApi.md#getroles) | **GET** /roles | Get Roles |

<a id="createrole"></a>
# **CreateRole**
> Role CreateRole (Role body = null)

Create Role

Create a role. By granting users the roles created here, it becomes easier to implement role-based authorization on the SaaS side. In addition, even the same user can have different roles for each tenant/environment to which they belong. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateRoleExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new RoleApi(config);
            var body = new Role();  // Role |  (optional) 

            try
            {
                // Create Role
                Role result = apiInstance.CreateRole(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RoleApi.CreateRole: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateRoleWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Role
    ApiResponse<Role> response = apiInstance.CreateRoleWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RoleApi.CreateRoleWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **Role** |  | [optional]  |

### Return type

[**Role**](Role.md)

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

<a id="deleterole"></a>
# **DeleteRole**
> void DeleteRole (string roleName)

Delete Role

Delete role. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class DeleteRoleExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new RoleApi(config);
            var roleName = admin;  // string | Role name

            try
            {
                // Delete Role
                apiInstance.DeleteRole(roleName);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RoleApi.DeleteRole: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteRoleWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Role
    apiInstance.DeleteRoleWithHttpInfo(roleName);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RoleApi.DeleteRoleWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
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
| **400** | Bad Request |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getroles"></a>
# **GetRoles**
> Roles GetRoles ()

Get Roles

Get registered roles list. Granting users the roles defined here makes it easy to implement role-based authorization on the SaaS side. In addition, even the same user can have different roles for each tenant/environment to which they belong. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetRolesExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new RoleApi(config);

            try
            {
                // Get Roles
                Roles result = apiInstance.GetRoles();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling RoleApi.GetRoles: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetRolesWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Roles
    ApiResponse<Roles> response = apiInstance.GetRolesWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling RoleApi.GetRolesWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**Roles**](Roles.md)

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

