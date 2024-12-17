# communicationapi - the C# library for the SaaSus Communication API Schema

SaaSus Communication API Schema

<a id="documentation-for-api-endpoints"></a>
## Documentation for API Endpoints

All URIs are relative to *https://api.saasus.io/v1/communication*

Class | Method | HTTP request | Description
------------ | ------------- | ------------- | -------------
*ErrorApi* | [**ReturnInternalServerError**](docs/ErrorApi.md#returninternalservererror) | **GET** /errors/internal-server-error | Return Internal Server Error
*FeedbackApi* | [**CreateFeedback**](docs/FeedbackApi.md#createfeedback) | **POST** /feedbacks | Create Feedback
*FeedbackApi* | [**CreateFeedbackComment**](docs/FeedbackApi.md#createfeedbackcomment) | **POST** /feedbacks/{feedback_id}/comments | Create Feedback Comment
*FeedbackApi* | [**CreateVoteUser**](docs/FeedbackApi.md#createvoteuser) | **POST** /feedbacks/{feedback_id}/votes/users | Create Vote User
*FeedbackApi* | [**DeleteFeedback**](docs/FeedbackApi.md#deletefeedback) | **DELETE** /feedbacks/{feedback_id} | Delete Feedback
*FeedbackApi* | [**DeleteFeedbackComment**](docs/FeedbackApi.md#deletefeedbackcomment) | **DELETE** /feedbacks/{feedback_id}/comments/{comment_id} | Delete Feedback Comment
*FeedbackApi* | [**DeleteVoteForFeedback**](docs/FeedbackApi.md#deletevoteforfeedback) | **DELETE** /feedbacks/{feedback_id}/votes/users/{user_id} | Delete Vote For Feedback
*FeedbackApi* | [**GetFeedback**](docs/FeedbackApi.md#getfeedback) | **GET** /feedbacks/{feedback_id} | Get Feedback
*FeedbackApi* | [**GetFeedbackComment**](docs/FeedbackApi.md#getfeedbackcomment) | **GET** /feedbacks/{feedback_id}/comments/{comment_id} | Get Feedback Comment
*FeedbackApi* | [**GetFeedbacks**](docs/FeedbackApi.md#getfeedbacks) | **GET** /feedbacks | Get Feedbacks
*FeedbackApi* | [**UpdateFeedback**](docs/FeedbackApi.md#updatefeedback) | **PATCH** /feedbacks/{feedback_id} | Update Feedback
*FeedbackApi* | [**UpdateFeedbackComment**](docs/FeedbackApi.md#updatefeedbackcomment) | **PATCH** /feedbacks/{feedback_id}/comments/{comment_id} | Update Feedback Comment
*FeedbackApi* | [**UpdateFeedbackStatus**](docs/FeedbackApi.md#updatefeedbackstatus) | **PATCH** /feedbacks/{feedback_id}/status | Update Feedback Status


<a id="documentation-for-models"></a>
## Documentation for Models

 - [Model.Comment](docs/Comment.md)
 - [Model.Comments](docs/Comments.md)
 - [Model.CreateFeedbackCommentParam](docs/CreateFeedbackCommentParam.md)
 - [Model.CreateFeedbackParam](docs/CreateFeedbackParam.md)
 - [Model.CreateVoteUserParam](docs/CreateVoteUserParam.md)
 - [Model.Error](docs/Error.md)
 - [Model.Feedback](docs/Feedback.md)
 - [Model.FeedbackSaveProps](docs/FeedbackSaveProps.md)
 - [Model.Feedbacks](docs/Feedbacks.md)
 - [Model.UpdateFeedbackCommentParam](docs/UpdateFeedbackCommentParam.md)
 - [Model.UpdateFeedbackParam](docs/UpdateFeedbackParam.md)
 - [Model.UpdateFeedbackStatusParam](docs/UpdateFeedbackStatusParam.md)
 - [Model.User](docs/User.md)
 - [Model.Users](docs/Users.md)
 - [Model.Votes](docs/Votes.md)


<a id="documentation-for-authorization"></a>
## Documentation for Authorization


Authentication schemes defined for the API:
<a id="Bearer"></a>
### Bearer

- **Type**: Bearer Authentication

**Note**:
This API automatically handles Bearer token authentication. You do not need to manually configure or include Bearer tokens in your requests.


