# billingapi - the C# library for the SaaSus Billing API Schema

SaaSus Billing API Schema

<a id="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.saasus.io/v1/billing*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*ErrorApi* | [**ReturnInternalServerError**](docs/ErrorApi.md#returninternalservererror) | **GET** /errors/internal-server-error | Return Internal Server Error
*StripeApi* | [**DeleteStripeInfo**](docs/StripeApi.md#deletestripeinfo) | **DELETE** /stripe/info | Delete Stripe Connection
*StripeApi* | [**GetStripeInfo**](docs/StripeApi.md#getstripeinfo) | **GET** /stripe/info | Get Stripe Connection information
*StripeApi* | [**UpdateStripeInfo**](docs/StripeApi.md#updatestripeinfo) | **PUT** /stripe/info | Update Stripe Connection Info


<a id="documentation-for-models"></a>
## Documentation for Models

 - [Model.Error](docs/Error.md)
 - [Model.StripeInfo](docs/StripeInfo.md)
 - [Model.UpdateStripeInfoParam](docs/UpdateStripeInfoParam.md)


<a id="documentation-for-authorization"></a>
## Documentation for Authorization


Authentication schemes defined for the API:
<a id="Bearer"></a>
### Bearer

- **Type**: Bearer Authentication

**Note**:
This API automatically handles Bearer token authentication. You do not need to manually configure or include Bearer tokens in your requests.


