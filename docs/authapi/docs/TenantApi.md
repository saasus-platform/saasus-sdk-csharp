# authapi.Api.TenantApi

All URIs are relative to *https://api.saasus.io/v1/auth*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateTenant**](TenantApi.md#createtenant) | **POST** /tenants | Create Tenant |
| [**CreateTenantAndPricing**](TenantApi.md#createtenantandpricing) | **PATCH** /stripe/init | Stripe Initial Setting |
| [**DeleteStripeTenantAndPricing**](TenantApi.md#deletestripetenantandpricing) | **DELETE** /stripe | Delete Customer and Product From Stripe |
| [**DeleteTenant**](TenantApi.md#deletetenant) | **DELETE** /tenants/{tenant_id} | Delete Tenant |
| [**GetStripeCustomer**](TenantApi.md#getstripecustomer) | **GET** /tenants/{tenant_id}/stripe-customer | Get Stripe Customer |
| [**GetTenant**](TenantApi.md#gettenant) | **GET** /tenants/{tenant_id} | Get Tenant Details |
| [**GetTenantIdentityProviders**](TenantApi.md#gettenantidentityproviders) | **GET** /tenants/{tenant_id}/identity-providers | Get identity provider per tenant |
| [**GetTenants**](TenantApi.md#gettenants) | **GET** /tenants | Get Tenants |
| [**ResetPlan**](TenantApi.md#resetplan) | **PUT** /plans/reset | Delete all information related to rate plans |
| [**UpdateTenant**](TenantApi.md#updatetenant) | **PATCH** /tenants/{tenant_id} | Update Tenant Details |
| [**UpdateTenantBillingInfo**](TenantApi.md#updatetenantbillinginfo) | **PUT** /tenants/{tenant_id}/billing-info | Update Tenant Billing Information |
| [**UpdateTenantIdentityProvider**](TenantApi.md#updatetenantidentityprovider) | **PUT** /tenants/{tenant_id}/identity-providers | Update identity provider per tenant |
| [**UpdateTenantPlan**](TenantApi.md#updatetenantplan) | **PUT** /tenants/{tenant_id}/plans | Update Tenant Plan Information |

<a id="createtenant"></a>
# **CreateTenant**
> Tenant CreateTenant (TenantProps body = null)

Create Tenant

Create a tenant managed by the SaaSus Platform. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateTenantExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);
            var body = new TenantProps();  // TenantProps |  (optional) 

            try
            {
                // Create Tenant
                Tenant result = apiInstance.CreateTenant(body);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.CreateTenant: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateTenantWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Tenant
    ApiResponse<Tenant> response = apiInstance.CreateTenantWithHttpInfo(body);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.CreateTenantWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **body** | **TenantProps** |  | [optional]  |

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

<a id="createtenantandpricing"></a>
# **CreateTenantAndPricing**
> void CreateTenantAndPricing ()

Stripe Initial Setting

Set Stripe initial information via billing 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class CreateTenantAndPricingExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);

            try
            {
                // Stripe Initial Setting
                apiInstance.CreateTenantAndPricing();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.CreateTenantAndPricing: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateTenantAndPricingWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Stripe Initial Setting
    apiInstance.CreateTenantAndPricingWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.CreateTenantAndPricingWithHttpInfo: " + e.Message);
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

<a id="deletestripetenantandpricing"></a>
# **DeleteStripeTenantAndPricing**
> void DeleteStripeTenantAndPricing ()

Delete Customer and Product From Stripe

Delete customer and product from Stripe. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class DeleteStripeTenantAndPricingExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);

            try
            {
                // Delete Customer and Product From Stripe
                apiInstance.DeleteStripeTenantAndPricing();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.DeleteStripeTenantAndPricing: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteStripeTenantAndPricingWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Customer and Product From Stripe
    apiInstance.DeleteStripeTenantAndPricingWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.DeleteStripeTenantAndPricingWithHttpInfo: " + e.Message);
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

<a id="deletetenant"></a>
# **DeleteTenant**
> void DeleteTenant (string tenantId)

Delete Tenant

Delete SaaSus Platform tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class DeleteTenantExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID

            try
            {
                // Delete Tenant
                apiInstance.DeleteTenant(tenantId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.DeleteTenant: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteTenantWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Tenant
    apiInstance.DeleteTenantWithHttpInfo(tenantId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.DeleteTenantWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |

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

<a id="getstripecustomer"></a>
# **GetStripeCustomer**
> StripeCustomer GetStripeCustomer (string tenantId)

Get Stripe Customer

Get the Stripe Customer information associated with the tenant, including their subscriptions. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetStripeCustomerExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID

            try
            {
                // Get Stripe Customer
                StripeCustomer result = apiInstance.GetStripeCustomer(tenantId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.GetStripeCustomer: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetStripeCustomerWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Stripe Customer
    ApiResponse<StripeCustomer> response = apiInstance.GetStripeCustomerWithHttpInfo(tenantId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.GetStripeCustomerWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |

### Return type

[**StripeCustomer**](StripeCustomer.md)

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

<a id="gettenant"></a>
# **GetTenant**
> TenantDetail GetTenant (string tenantId)

Get Tenant Details

Get the details of tenant managed on the SaaSus Platform. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetTenantExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID

            try
            {
                // Get Tenant Details
                TenantDetail result = apiInstance.GetTenant(tenantId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.GetTenant: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Tenant Details
    ApiResponse<TenantDetail> response = apiInstance.GetTenantWithHttpInfo(tenantId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.GetTenantWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |

### Return type

[**TenantDetail**](TenantDetail.md)

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

<a id="gettenantidentityproviders"></a>
# **GetTenantIdentityProviders**
> TenantIdentityProviders GetTenantIdentityProviders (string tenantId)

Get identity provider per tenant

Get sign-in information via external identity provider per tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetTenantIdentityProvidersExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID

            try
            {
                // Get identity provider per tenant
                TenantIdentityProviders result = apiInstance.GetTenantIdentityProviders(tenantId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.GetTenantIdentityProviders: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantIdentityProvidersWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get identity provider per tenant
    ApiResponse<TenantIdentityProviders> response = apiInstance.GetTenantIdentityProvidersWithHttpInfo(tenantId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.GetTenantIdentityProvidersWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |

### Return type

[**TenantIdentityProviders**](TenantIdentityProviders.md)

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

<a id="gettenants"></a>
# **GetTenants**
> Tenants GetTenants ()

Get Tenants

Get tenants managed by SaaSus Platform. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class GetTenantsExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);

            try
            {
                // Get Tenants
                Tenants result = apiInstance.GetTenants();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.GetTenants: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetTenantsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Tenants
    ApiResponse<Tenants> response = apiInstance.GetTenantsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.GetTenantsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**Tenants**](Tenants.md)

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

<a id="resetplan"></a>
# **ResetPlan**
> void ResetPlan ()

Delete all information related to rate plans

Delete all information related to rate plans. Delete plans linked to tenants and plan definitions. If you are using the Stripe linkage, the linkage will be removed. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class ResetPlanExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);

            try
            {
                // Delete all information related to rate plans
                apiInstance.ResetPlan();
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.ResetPlan: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the ResetPlanWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete all information related to rate plans
    apiInstance.ResetPlanWithHttpInfo();
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.ResetPlanWithHttpInfo: " + e.Message);
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

<a id="updatetenant"></a>
# **UpdateTenant**
> void UpdateTenant (string tenantId, TenantProps body = null)

Update Tenant Details

Update SaaSus Platform tenant details. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateTenantExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var body = new TenantProps();  // TenantProps |  (optional) 

            try
            {
                // Update Tenant Details
                apiInstance.UpdateTenant(tenantId, body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.UpdateTenant: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateTenantWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Tenant Details
    apiInstance.UpdateTenantWithHttpInfo(tenantId, body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.UpdateTenantWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **body** | **TenantProps** |  | [optional]  |

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

<a id="updatetenantbillinginfo"></a>
# **UpdateTenantBillingInfo**
> void UpdateTenantBillingInfo (string tenantId, BillingInfo body = null)

Update Tenant Billing Information

Update SaaSus Platform tenant billing information. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateTenantBillingInfoExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var body = new BillingInfo();  // BillingInfo |  (optional) 

            try
            {
                // Update Tenant Billing Information
                apiInstance.UpdateTenantBillingInfo(tenantId, body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.UpdateTenantBillingInfo: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateTenantBillingInfoWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Tenant Billing Information
    apiInstance.UpdateTenantBillingInfoWithHttpInfo(tenantId, body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.UpdateTenantBillingInfoWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **body** | **BillingInfo** |  | [optional]  |

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

<a id="updatetenantidentityprovider"></a>
# **UpdateTenantIdentityProvider**
> void UpdateTenantIdentityProvider (string tenantId, UpdateTenantIdentityProviderParam updateTenantIdentityProviderParam = null)

Update identity provider per tenant

Update sign-in information via external identity provider per tenant. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateTenantIdentityProviderExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var updateTenantIdentityProviderParam = new UpdateTenantIdentityProviderParam(); // UpdateTenantIdentityProviderParam |  (optional) 

            try
            {
                // Update identity provider per tenant
                apiInstance.UpdateTenantIdentityProvider(tenantId, updateTenantIdentityProviderParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.UpdateTenantIdentityProvider: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateTenantIdentityProviderWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update identity provider per tenant
    apiInstance.UpdateTenantIdentityProviderWithHttpInfo(tenantId, updateTenantIdentityProviderParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.UpdateTenantIdentityProviderWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **updateTenantIdentityProviderParam** | [**UpdateTenantIdentityProviderParam**](UpdateTenantIdentityProviderParam.md) |  | [optional]  |

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

<a id="updatetenantplan"></a>
# **UpdateTenantPlan**
> void UpdateTenantPlan (string tenantId, PlanReservation body = null)

Update Tenant Plan Information

Update SaaSus Platform tenant plan information. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using authapi.Api;
using authapi.Model;
using modules;

namespace Example
{
    public class UpdateTenantPlanExample
    {
        public static void Main()
        {
            AuthApiClientConfig config = new Configuration().GetAuthApiClientConfig();
            var apiInstance = new TenantApi(config);
            var tenantId = "tenantId_example";  // string | Tenant ID
            var body = new PlanReservation();  // PlanReservation |  (optional) 

            try
            {
                // Update Tenant Plan Information
                apiInstance.UpdateTenantPlan(tenantId, body);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling TenantApi.UpdateTenantPlan: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateTenantPlanWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Tenant Plan Information
    apiInstance.UpdateTenantPlanWithHttpInfo(tenantId, body);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling TenantApi.UpdateTenantPlanWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantId** | **string** | Tenant ID |  |
| **body** | **PlanReservation** |  | [optional]  |

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

