# awsmarketplaceapi - the C# library for the SaaSus AWS Marketplace API Schema

SaaSus AWS Marketplace API Schema

<a id="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.saasus.io/v1/awsmarketplace*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*AwsMarketplaceApi* | [**CreateCustomer**](docs/AwsMarketplaceApi.md#createcustomer) | **POST** /customers | Create customer information to be linked to AWS Marketplace
*AwsMarketplaceApi* | [**GetCatalogEntityVisibility**](docs/AwsMarketplaceApi.md#getcatalogentityvisibility) | **GET** /catalog-entity/visibility | Obtain product publication status from AWS Marketplace
*AwsMarketplaceApi* | [**GetCloudFormationLaunchStackLink**](docs/AwsMarketplaceApi.md#getcloudformationlaunchstacklink) | **GET** /cloudformation-launch-stack-link | Get the link to create the AWS CloudFormation stack
*AwsMarketplaceApi* | [**GetCustomer**](docs/AwsMarketplaceApi.md#getcustomer) | **GET** /customers/{customer_identifier} | Get customer information to be linked to AWS Marketplace
*AwsMarketplaceApi* | [**GetCustomers**](docs/AwsMarketplaceApi.md#getcustomers) | **GET** /customers | Get a list of customer information to be linked to AWS Marketplace
*AwsMarketplaceApi* | [**GetListingStatus**](docs/AwsMarketplaceApi.md#getlistingstatus) | **GET** /listing-status | Get AWS Marketplace Listing Status
*AwsMarketplaceApi* | [**GetPlanByPlanName**](docs/AwsMarketplaceApi.md#getplanbyplanname) | **GET** /plans/{plan_name} | Obtain plan information to link to AWS Marketplace
*AwsMarketplaceApi* | [**GetPlans**](docs/AwsMarketplaceApi.md#getplans) | **GET** /plans | Obtain plan information to link to AWS Marketplace
*AwsMarketplaceApi* | [**GetSettings**](docs/AwsMarketplaceApi.md#getsettings) | **GET** /settings | Get AWS Marketplace Settings
*AwsMarketplaceApi* | [**SavePlan**](docs/AwsMarketplaceApi.md#saveplan) | **PUT** /plans | Save plan information to be linked to AWSMarketplace
*AwsMarketplaceApi* | [**SyncCustomer**](docs/AwsMarketplaceApi.md#synccustomer) | **POST** /customers/{customer_identifier}/sync | Sync AWS Marketplace customer information to SaaSus
*AwsMarketplaceApi* | [**UpdateListingStatus**](docs/AwsMarketplaceApi.md#updatelistingstatus) | **PUT** /listing-status | Update AWS Marketplace Listing Status
*AwsMarketplaceApi* | [**UpdateSettings**](docs/AwsMarketplaceApi.md#updatesettings) | **PUT** /settings | Update AWS Marketplace Settings
*AwsMarketplaceApi* | [**VerifyRegistrationToken**](docs/AwsMarketplaceApi.md#verifyregistrationtoken) | **POST** /registration-token/verify | Verify Registration Token
*ErrorApi* | [**ReturnInternalServerError**](docs/ErrorApi.md#returninternalservererror) | **GET** /errors/internal-server-error | Return Internal Server Error


<a id="documentation-for-models"></a>
## Documentation for Models

 - [Model.CatalogEntityVisibility](docs/CatalogEntityVisibility.md)
 - [Model.CloudFormationLaunchStackLink](docs/CloudFormationLaunchStackLink.md)
 - [Model.CreateCustomerParam](docs/CreateCustomerParam.md)
 - [Model.Customer](docs/Customer.md)
 - [Model.Customers](docs/Customers.md)
 - [Model.Error](docs/Error.md)
 - [Model.GetListingStatusResult](docs/GetListingStatusResult.md)
 - [Model.ListingStatus](docs/ListingStatus.md)
 - [Model.Plan](docs/Plan.md)
 - [Model.Plans](docs/Plans.md)
 - [Model.SavePlanParam](docs/SavePlanParam.md)
 - [Model.Settings](docs/Settings.md)
 - [Model.UpdateListingStatusParam](docs/UpdateListingStatusParam.md)
 - [Model.UpdateSettingsParam](docs/UpdateSettingsParam.md)
 - [Model.VerifyRegistrationTokenParam](docs/VerifyRegistrationTokenParam.md)
 - [Model.VisibilityStatus](docs/VisibilityStatus.md)


<a id="documentation-for-authorization"></a>
## Documentation for Authorization


Authentication schemes defined for the API:
<a id="Bearer"></a>
### Bearer

- **Type**: Bearer Authentication

**Note**:
This API automatically handles Bearer token authentication. You do not need to manually configure or include Bearer tokens in your requests.


