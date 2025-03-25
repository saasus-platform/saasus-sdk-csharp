# awsmarketplaceapi.Api.AwsMarketplaceApi

All URIs are relative to *https://api.saasus.io/v1/awsmarketplace*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateCustomer**](AwsMarketplaceApi.md#createcustomer) | **POST** /customers | Create customer information to be linked to AWS Marketplace |
| [**GetCatalogEntityVisibility**](AwsMarketplaceApi.md#getcatalogentityvisibility) | **GET** /catalog-entity/visibility | Obtain product publication status from AWS Marketplace |
| [**GetCloudFormationLaunchStackLink**](AwsMarketplaceApi.md#getcloudformationlaunchstacklink) | **GET** /cloudformation-launch-stack-link | Get the link to create the AWS CloudFormation stack |
| [**GetCustomer**](AwsMarketplaceApi.md#getcustomer) | **GET** /customers/{customer_identifier} | Get customer information to be linked to AWS Marketplace |
| [**GetCustomers**](AwsMarketplaceApi.md#getcustomers) | **GET** /customers | Get a list of customer information to be linked to AWS Marketplace |
| [**GetListingStatus**](AwsMarketplaceApi.md#getlistingstatus) | **GET** /listing-status | Get AWS Marketplace Listing Status |
| [**GetPlanByPlanName**](AwsMarketplaceApi.md#getplanbyplanname) | **GET** /plans/{plan_name} | Obtain plan information to link to AWS Marketplace |
| [**GetPlans**](AwsMarketplaceApi.md#getplans) | **GET** /plans | Obtain plan information to link to AWS Marketplace |
| [**GetSettings**](AwsMarketplaceApi.md#getsettings) | **GET** /settings | Get AWS Marketplace Settings |
| [**SavePlan**](AwsMarketplaceApi.md#saveplan) | **PUT** /plans | Save plan information to be linked to AWSMarketplace |
| [**SyncCustomer**](AwsMarketplaceApi.md#synccustomer) | **POST** /customers/{customer_identifier}/sync | Sync AWS Marketplace customer information to SaaSus |
| [**UpdateListingStatus**](AwsMarketplaceApi.md#updatelistingstatus) | **PUT** /listing-status | Update AWS Marketplace Listing Status |
| [**UpdateSettings**](AwsMarketplaceApi.md#updatesettings) | **PUT** /settings | Update AWS Marketplace Settings |
| [**VerifyRegistrationToken**](AwsMarketplaceApi.md#verifyregistrationtoken) | **POST** /registration-token/verify | Verify Registration Token |

<a id="createcustomer"></a>
# **CreateCustomer**
> Customer CreateCustomer (CreateCustomerParam createCustomerParam = null)

Create customer information to be linked to AWS Marketplace

Create customer information to be linked to AWS Marketplace. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class CreateCustomerExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);
            var createCustomerParam = new CreateCustomerParam(); // CreateCustomerParam |  (optional) 

            try
            {
                // Create customer information to be linked to AWS Marketplace
                Customer result = apiInstance.CreateCustomer(createCustomerParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.CreateCustomer: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateCustomerWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create customer information to be linked to AWS Marketplace
    ApiResponse<Customer> response = apiInstance.CreateCustomerWithHttpInfo(createCustomerParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.CreateCustomerWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createCustomerParam** | [**CreateCustomerParam**](CreateCustomerParam.md) |  | [optional]  |

### Return type

[**Customer**](Customer.md)

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

<a id="getcatalogentityvisibility"></a>
# **GetCatalogEntityVisibility**
> CatalogEntityVisibility GetCatalogEntityVisibility ()

Obtain product publication status from AWS Marketplace

Retrieve the product's publication status from AWS Marketplace. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class GetCatalogEntityVisibilityExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);

            try
            {
                // Obtain product publication status from AWS Marketplace
                CatalogEntityVisibility result = apiInstance.GetCatalogEntityVisibility();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.GetCatalogEntityVisibility: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCatalogEntityVisibilityWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Obtain product publication status from AWS Marketplace
    ApiResponse<CatalogEntityVisibility> response = apiInstance.GetCatalogEntityVisibilityWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.GetCatalogEntityVisibilityWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**CatalogEntityVisibility**](CatalogEntityVisibility.md)

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

<a id="getcloudformationlaunchstacklink"></a>
# **GetCloudFormationLaunchStackLink**
> CloudFormationLaunchStackLink GetCloudFormationLaunchStackLink ()

Get the link to create the AWS CloudFormation stack

Get the CloudFormation Quick Create link. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class GetCloudFormationLaunchStackLinkExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);

            try
            {
                // Get the link to create the AWS CloudFormation stack
                CloudFormationLaunchStackLink result = apiInstance.GetCloudFormationLaunchStackLink();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.GetCloudFormationLaunchStackLink: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCloudFormationLaunchStackLinkWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get the link to create the AWS CloudFormation stack
    ApiResponse<CloudFormationLaunchStackLink> response = apiInstance.GetCloudFormationLaunchStackLinkWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.GetCloudFormationLaunchStackLinkWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**CloudFormationLaunchStackLink**](CloudFormationLaunchStackLink.md)

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

<a id="getcustomer"></a>
# **GetCustomer**
> Customer GetCustomer (string customerIdentifier)

Get customer information to be linked to AWS Marketplace

Get customer information to be linked to AWS Marketplace. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class GetCustomerExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);
            var customerIdentifier = 123456789012;  // string | Customer ID

            try
            {
                // Get customer information to be linked to AWS Marketplace
                Customer result = apiInstance.GetCustomer(customerIdentifier);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.GetCustomer: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCustomerWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get customer information to be linked to AWS Marketplace
    ApiResponse<Customer> response = apiInstance.GetCustomerWithHttpInfo(customerIdentifier);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.GetCustomerWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **customerIdentifier** | **string** | Customer ID |  |

### Return type

[**Customer**](Customer.md)

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

<a id="getcustomers"></a>
# **GetCustomers**
> Customers GetCustomers (List<string> tenantIds = null)

Get a list of customer information to be linked to AWS Marketplace

Get a list of customer information to be linked to AWS Marketplace. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class GetCustomersExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);
            var tenantIds = new List<string>(); // List<string> | 指定したテナントIDの顧客を取得する(Get customers with the specified tenant ID) (optional) 

            try
            {
                // Get a list of customer information to be linked to AWS Marketplace
                Customers result = apiInstance.GetCustomers(tenantIds);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.GetCustomers: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetCustomersWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get a list of customer information to be linked to AWS Marketplace
    ApiResponse<Customers> response = apiInstance.GetCustomersWithHttpInfo(tenantIds);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.GetCustomersWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **tenantIds** | [**List&lt;string&gt;**](string.md) | 指定したテナントIDの顧客を取得する(Get customers with the specified tenant ID) | [optional]  |

### Return type

[**Customers**](Customers.md)

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

<a id="getlistingstatus"></a>
# **GetListingStatus**
> GetListingStatusResult GetListingStatus ()

Get AWS Marketplace Listing Status

Get AWS Marketplace Listing Status. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class GetListingStatusExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);

            try
            {
                // Get AWS Marketplace Listing Status
                GetListingStatusResult result = apiInstance.GetListingStatus();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.GetListingStatus: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetListingStatusWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get AWS Marketplace Listing Status
    ApiResponse<GetListingStatusResult> response = apiInstance.GetListingStatusWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.GetListingStatusWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**GetListingStatusResult**](GetListingStatusResult.md)

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

<a id="getplanbyplanname"></a>
# **GetPlanByPlanName**
> Plan GetPlanByPlanName (string planName)

Obtain plan information to link to AWS Marketplace

Obtain plan information to link to AWS Marketplace. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class GetPlanByPlanNameExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);
            var planName = normal_plan_name_month;  // string | AWS Marketplace linked plan name

            try
            {
                // Obtain plan information to link to AWS Marketplace
                Plan result = apiInstance.GetPlanByPlanName(planName);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.GetPlanByPlanName: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetPlanByPlanNameWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Obtain plan information to link to AWS Marketplace
    ApiResponse<Plan> response = apiInstance.GetPlanByPlanNameWithHttpInfo(planName);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.GetPlanByPlanNameWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **planName** | **string** | AWS Marketplace linked plan name |  |

### Return type

[**Plan**](Plan.md)

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

<a id="getplans"></a>
# **GetPlans**
> Plans GetPlans ()

Obtain plan information to link to AWS Marketplace

Obtain plan information to link to AWS Marketplace. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class GetPlansExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);

            try
            {
                // Obtain plan information to link to AWS Marketplace
                Plans result = apiInstance.GetPlans();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.GetPlans: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetPlansWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Obtain plan information to link to AWS Marketplace
    ApiResponse<Plans> response = apiInstance.GetPlansWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.GetPlansWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**Plans**](Plans.md)

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

<a id="getsettings"></a>
# **GetSettings**
> Settings GetSettings ()

Get AWS Marketplace Settings

Get AWS Marketplace Settings. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class GetSettingsExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);

            try
            {
                // Get AWS Marketplace Settings
                Settings result = apiInstance.GetSettings();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.GetSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get AWS Marketplace Settings
    ApiResponse<Settings> response = apiInstance.GetSettingsWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.GetSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**Settings**](Settings.md)

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

<a id="saveplan"></a>
# **SavePlan**
> void SavePlan (SavePlanParam savePlanParam = null)

Save plan information to be linked to AWSMarketplace

Save plan information to be linked to AWSMarketplace. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class SavePlanExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);
            var savePlanParam = new SavePlanParam(); // SavePlanParam |  (optional) 

            try
            {
                // Save plan information to be linked to AWSMarketplace
                apiInstance.SavePlan(savePlanParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.SavePlan: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SavePlanWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Save plan information to be linked to AWSMarketplace
    apiInstance.SavePlanWithHttpInfo(savePlanParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.SavePlanWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **savePlanParam** | [**SavePlanParam**](SavePlanParam.md) |  | [optional]  |

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

<a id="synccustomer"></a>
# **SyncCustomer**
> void SyncCustomer (string customerIdentifier)

Sync AWS Marketplace customer information to SaaSus

Sync AWS Marketplace customer information to SaaSus. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class SyncCustomerExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);
            var customerIdentifier = 123456789012;  // string | Customer ID

            try
            {
                // Sync AWS Marketplace customer information to SaaSus
                apiInstance.SyncCustomer(customerIdentifier);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.SyncCustomer: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the SyncCustomerWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Sync AWS Marketplace customer information to SaaSus
    apiInstance.SyncCustomerWithHttpInfo(customerIdentifier);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.SyncCustomerWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **customerIdentifier** | **string** | Customer ID |  |

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

<a id="updatelistingstatus"></a>
# **UpdateListingStatus**
> void UpdateListingStatus (UpdateListingStatusParam updateListingStatusParam = null)

Update AWS Marketplace Listing Status

Update AWS Marketplace Listing Status. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class UpdateListingStatusExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);
            var updateListingStatusParam = new UpdateListingStatusParam(); // UpdateListingStatusParam |  (optional) 

            try
            {
                // Update AWS Marketplace Listing Status
                apiInstance.UpdateListingStatus(updateListingStatusParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.UpdateListingStatus: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateListingStatusWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update AWS Marketplace Listing Status
    apiInstance.UpdateListingStatusWithHttpInfo(updateListingStatusParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.UpdateListingStatusWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateListingStatusParam** | [**UpdateListingStatusParam**](UpdateListingStatusParam.md) |  | [optional]  |

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

<a id="updatesettings"></a>
# **UpdateSettings**
> void UpdateSettings (UpdateSettingsParam updateSettingsParam = null)

Update AWS Marketplace Settings

Update AWS Marketplace Settings. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class UpdateSettingsExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);
            var updateSettingsParam = new UpdateSettingsParam(); // UpdateSettingsParam |  (optional) 

            try
            {
                // Update AWS Marketplace Settings
                apiInstance.UpdateSettings(updateSettingsParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.UpdateSettings: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateSettingsWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update AWS Marketplace Settings
    apiInstance.UpdateSettingsWithHttpInfo(updateSettingsParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.UpdateSettingsWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **updateSettingsParam** | [**UpdateSettingsParam**](UpdateSettingsParam.md) |  | [optional]  |

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

<a id="verifyregistrationtoken"></a>
# **VerifyRegistrationToken**
> void VerifyRegistrationToken (VerifyRegistrationTokenParam verifyRegistrationTokenParam = null)

Verify Registration Token

Verify Registration Token. 

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using awsmarketplaceapi.Api;
using awsmarketplaceapi.Model;
using modules;

namespace Example
{
    public class VerifyRegistrationTokenExample
    {
        public static void Main()
        {
            AwsMarketplaceApiClientConfig config = new Configuration().GetAwsMarketplaceApiClientConfig();
            var apiInstance = new AwsMarketplaceApi(config);
            var verifyRegistrationTokenParam = new VerifyRegistrationTokenParam(); // VerifyRegistrationTokenParam |  (optional) 

            try
            {
                // Verify Registration Token
                apiInstance.VerifyRegistrationToken(verifyRegistrationTokenParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling AwsMarketplaceApi.VerifyRegistrationToken: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the VerifyRegistrationTokenWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Verify Registration Token
    apiInstance.VerifyRegistrationTokenWithHttpInfo(verifyRegistrationTokenParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling AwsMarketplaceApi.VerifyRegistrationTokenWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **verifyRegistrationTokenParam** | [**VerifyRegistrationTokenParam**](VerifyRegistrationTokenParam.md) |  | [optional]  |

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

