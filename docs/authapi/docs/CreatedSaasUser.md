# authapi.Model.CreatedSaasUser

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Id** | **string** |  | 
**Email** | **string** | E-mail. For sign-in ID authentication users, this field is an empty string.  | 
**SignInId** | **string** | Sign-in ID. For email authentication users, this field is an empty string.  | 
**Attributes** | **Dictionary&lt;string, Object&gt;** | Attribute information  | 
**LastLoginAt** | **int?** | Last login date and time (unix timestamp). Null if the user has never logged in.  | [optional] 
**Password** | **string** | Auto-generated password (only when sign_in_id authentication and password not specified)  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

