# authapi.Api.InvitationApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateTenantInvitation**](InvitationApi.md#createtenantinvitation) | **POST** /tenants/{tenant_id}/invitations | Create Tenant Invitation |
| [**DeleteTenantInvitation**](InvitationApi.md#deletetenantinvitation) | **DELETE** /tenants/{tenant_id}/invitations/{invitation_id} | Delete Tenant Invitation |
| [**GetInvitationValidity**](InvitationApi.md#getinvitationvalidity) | **GET** /invitations/{invitation_id}/validity | Get Invitation Validity |
| [**GetTenantInvitation**](InvitationApi.md#gettenantinvitation) | **GET** /tenants/{tenant_id}/invitations/{invitation_id} | Get Tenant Invitation |
| [**GetTenantInvitations**](InvitationApi.md#gettenantinvitations) | **GET** /tenants/{tenant_id}/invitations | Get Tenant Invitations |
| [**ValidateInvitation**](InvitationApi.md#validateinvitation) | **PATCH** /invitations/{invitation_id}/validate | Validate Invitation |

<a id="createtenantinvitation"></a>
# **CreateTenantInvitation**
> Invitation CreateTenantInvitation (string tenantId, CreateTenantInvitationParam createTenantInvitationParam = null)

Create Tenant Invitation

Create an invitation to the tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateTenantInvitationExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new InvitationApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var createTenantInvitationParam = new CreateTenantInvitationParam(); // CreateTenantInvitationParam |  (optional) 

            try
            {
                // Create Tenant Invitation
                Invitation result = apiInstance.CreateTenantInvitation(tenantId, createTenantInvitationParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InvitationApi.CreateTenantInvitation: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateTenantInvitationWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Tenant Invitation
    ApiResponse<Invitation> response = apiInstance.CreateTenantInvitationWithHttpInfo(tenantId, createTenantInvitationParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InvitationApi.CreateTenantInvitationWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **createTenantInvitationParam** | [**CreateTenantInvitationParam**](CreateTenantInvitationParam.md) |  | [optional]  |

### Return type

[**Invitation**](Invitation.md)

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
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletetenantinvitation"></a>
# **DeleteTenantInvitation**
> void DeleteTenantInvitation (string tenantId, string invitationId)

Delete Tenant Invitation

Delete an invitation for the tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class DeleteTenantInvitationExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new InvitationApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var invitationId = "invitationId_example";  // string | Invitation ID

            try
            {
                // Delete Tenant Invitation
                apiInstance.DeleteTenantInvitation(tenantId, invitationId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InvitationApi.DeleteTenantInvitation: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteTenantInvitationWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Tenant Invitation
    apiInstance.DeleteTenantInvitationWithHttpInfo(tenantId, invitationId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InvitationApi.DeleteTenantInvitationWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **invitationId** | **string** | Invitation ID |  |

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

<a id="getinvitationvalidity"></a>
# **GetInvitationValidity**
> InvitationValidity GetInvitationValidity (string invitationId)

Get Invitation Validity

Get the validity of an invitation to the tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetInvitationValidityExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new InvitationApi(config);
            var invitationId = "invitationId_example";  // string | Invitation ID

            try
            {
                // Get Invitation Validity
                InvitationValidity result = apiInstance.GetInvitationValidity(invitationId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InvitationApi.GetInvitationValidity: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetInvitationValidityWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Invitation Validity
    ApiResponse<InvitationValidity> response = apiInstance.GetInvitationValidityWithHttpInfo(invitationId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InvitationApi.GetInvitationValidityWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **invitationId** | **string** | Invitation ID |  |

### Return type

[**InvitationValidity**](InvitationValidity.md)

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

<a id="gettenantinvitation"></a>
# **GetTenantInvitation**
> Invitation GetTenantInvitation (string tenantId, string invitationId)

Get Tenant Invitation

Get invitation information for the tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetTenantInvitationExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new InvitationApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var invitationId = "invitationId_example";  // string | Invitation ID

            try
            {
                // Get Tenant Invitation
                Invitation result = apiInstance.GetTenantInvitation(tenantId, invitationId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InvitationApi.GetTenantInvitation: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantInvitationWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Tenant Invitation
    ApiResponse<Invitation> response = apiInstance.GetTenantInvitationWithHttpInfo(tenantId, invitationId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InvitationApi.GetTenantInvitationWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **invitationId** | **string** | Invitation ID |  |

### Return type

[**Invitation**](Invitation.md)

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

<a id="gettenantinvitations"></a>
# **GetTenantInvitations**
> Invitations GetTenantInvitations (string tenantId)

Get Tenant Invitations

Get a list of invitations to the tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetTenantInvitationsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new InvitationApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID

            try
            {
                // Get Tenant Invitations
                Invitations result = apiInstance.GetTenantInvitations(tenantId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InvitationApi.GetTenantInvitations: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantInvitationsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Tenant Invitations
    ApiResponse<Invitations> response = apiInstance.GetTenantInvitationsWithHttpInfo(tenantId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InvitationApi.GetTenantInvitationsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |

### Return type

[**Invitations**](Invitations.md)

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

<a id="validateinvitation"></a>
# **ValidateInvitation**
> void ValidateInvitation (string invitationId, ValidateInvitationParam validateInvitationParam = null)

Validate Invitation

Validate an invitation to the tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class ValidateInvitationExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new InvitationApi(config);
            var invitationId = "invitationId_example";  // string | Invitation ID
            var validateInvitationParam = new ValidateInvitationParam(); // ValidateInvitationParam |  (optional) 

            try
            {
                // Validate Invitation
                apiInstance.ValidateInvitation(invitationId, validateInvitationParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling InvitationApi.ValidateInvitation: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ValidateInvitationWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Validate Invitation
    apiInstance.ValidateInvitationWithHttpInfo(invitationId, validateInvitationParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling InvitationApi.ValidateInvitationWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **invitationId** | **string** | Invitation ID |  |
| **validateInvitationParam** | [**ValidateInvitationParam**](ValidateInvitationParam.md) |  | [optional]  |

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
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

