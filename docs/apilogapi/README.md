# apilogapi - the C# library for the SaaSus ApiLog API Schema

SaaSus ApiLog API Schema

<a id="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.saasus.io/v1/apilog*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*ApiLogApi* | [**GetLog**](docs/ApiLogApi.md#getlog) | **GET** /logs/{api_log_id} | Get API execution log
*ApiLogApi* | [**GetLogs**](docs/ApiLogApi.md#getlogs) | **GET** /logs | Get API execution log list
*ErrorApi* | [**ReturnInternalServerError**](docs/ErrorApi.md#returninternalservererror) | **GET** /errors/internal-server-error | Returns a server error with status code 500


<a id="documentation-for-models"></a>
## Documentation for Models

 - [Model.ApiLog](docs/ApiLog.md)
 - [Model.ApiLogs](docs/ApiLogs.md)
 - [Model.Error](docs/Error.md)


<a id="documentation-for-authorization"></a>
## Documentation for Authorization


Authentication schemes defined for the API:
<a id="Bearer"></a>
### Bearer

- **Type**: Bearer Authentication

**Note**:
This API automatically handles Bearer token authentication. You do not need to manually configure or include Bearer tokens in your requests.


