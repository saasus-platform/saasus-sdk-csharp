# authapi.Model.CreateAuthCredentialsParam

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**IdToken** | **string** | ID token | 
**AccessToken** | **string** | Access token | 
**RefreshToken** | **string** | Refresh token | [optional] 
**CodeChallenge** | **string** | PKCE code challenge derived from the code verifier. It must be specified together with code_challenge_method. | [optional] 
**CodeChallengeMethod** | **string** | Method used to derive the PKCE code challenge. It must be specified together with code_challenge. | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

