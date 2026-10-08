# authapi.Model.CreateSaasUserParam
Either email or sign_in_id must be specified, but not both. - If email is specified: Email authentication user will be created.   When password is not specified, a temporary password will be sent by email. - If sign_in_id is specified: Sign-in ID authentication user will be created.   When password is not specified, it will be auto-generated and returned in the response. 

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Email** | **string** | E-mail | [optional] 
**SignInId** | **string** | Sign-in ID (alphanumeric and symbols -_ only, max 50 characters)  | [optional] 
**Password** | **string** | Password. For email authentication, if not specified, a temporary password will be sent by email. For sign-in ID authentication, if not specified, password will be auto-generated and returned.  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

