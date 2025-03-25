# integrationapi - the C# library for the SaaSus Eventbridge API Schema

SaaSus Eventbridge API Schema

<a id="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.saasus.io/v1/integration*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*ErrorApi* | [**ReturnInternalServerError**](docs/ErrorApi.md#returninternalservererror) | **GET** /errors/internal-server-error | Return Internal Server Error
*EventBridgeApi* | [**CreateEventBridgeEvent**](docs/EventBridgeApi.md#createeventbridgeevent) | **POST** /eventbridge/event | Send Events
*EventBridgeApi* | [**CreateEventBridgeTestEvent**](docs/EventBridgeApi.md#createeventbridgetestevent) | **POST** /eventbridge/test-event | Test EventBridge Connection
*EventBridgeApi* | [**DeleteEventBridgeSettings**](docs/EventBridgeApi.md#deleteeventbridgesettings) | **DELETE** /eventbridge/info | Delete EventBridge Settings
*EventBridgeApi* | [**GetEventBridgeSettings**](docs/EventBridgeApi.md#geteventbridgesettings) | **GET** /eventbridge/info | Get EventBridge Settings
*EventBridgeApi* | [**SaveEventBridgeSettings**](docs/EventBridgeApi.md#saveeventbridgesettings) | **PUT** /eventbridge/info | Update EventBridge Settings


<a id="documentation-for-models"></a>
## Documentation for Models

 - [Model.AwsRegion](docs/AwsRegion.md)
 - [Model.CreateEventBridgeEventParam](docs/CreateEventBridgeEventParam.md)
 - [Model.Error](docs/Error.md)
 - [Model.EventBridgeSettings](docs/EventBridgeSettings.md)
 - [Model.EventMessage](docs/EventMessage.md)


<a id="documentation-for-authorization"></a>
## Documentation for Authorization


Authentication schemes defined for the API:
<a id="Bearer"></a>
### Bearer

- **Type**: Bearer Authentication

**Note**:
This API automatically handles Bearer token authentication. You do not need to manually configure or include Bearer tokens in your requests.


