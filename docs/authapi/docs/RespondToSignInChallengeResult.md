# authapi.Model.RespondToSignInChallengeResult
Result returned after responding to a sign-in challenge 

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Credentials** | [**Credentials**](Credentials.md) |  | [optional] 
**ChallengeName** | **ChallengeName** |  | [optional] 
**ChallengeParameters** | **Dictionary&lt;string, string&gt;** | Parameters required for the next challenge.  | [optional] 
**Session** | **string** | Session identifier for the challenge. This session should be passed to the next call to RespondToSignInChallenge if another challenge is required.  | [optional] 
**NewDeviceMetadata** | [**NewDeviceMetadata**](NewDeviceMetadata.md) |  | [optional] 

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)

