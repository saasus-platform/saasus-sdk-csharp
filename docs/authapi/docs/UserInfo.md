# authapi.Model.UserInfo

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **string** |  | 
**Email** | **string** | E-mail. For sign-in ID authentication users, this field is an empty string.  | 
**SignInId** | **string** | Sign-in ID. For email authentication users, this field is an empty string.  | 
**UserAttribute** | **Dictionary&lt;string, Object&gt;** | user additional attributes | 
**Tenants** | [**List&lt;UserAvailableTenant&gt;**](UserAvailableTenant.md) | Tenant Info | 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

