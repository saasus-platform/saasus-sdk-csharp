# authapi.Model.SignInResult
Result returned after a sign-in attempt 

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**ChallengeName** | **ChallengeName** |  | [optional] 
**ChallengeParameters** | **Dictionary&lt;string, string&gt;** | Parameters required to complete the challenge  | [optional] 
**Session** | **string** | Session identifier for the challenge. This session should be passed to the next call to RespondToSignInChallenge if another challenge is required.  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

