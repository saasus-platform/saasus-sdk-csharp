# authapi.Model.SignInParam
Parameters required for user sign-in The required parameters vary depending on the sign_in_flow. 

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**SignInFlow** | **string** | The sign-in flow to use for authentication. Currently, only USER_SRP_AUTH is supported.  | 
**SignInParameters** | **Dictionary&lt;string, string&gt;** | The required parameters vary depending on the sign_in_flow. USER_SRP_AUTH:   USERNAME: email address   SRP_A: SRP A value  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

