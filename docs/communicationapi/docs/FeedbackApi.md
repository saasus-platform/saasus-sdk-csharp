# communicationapi.Api.FeedbackApi

All URIs are relative to *https://api.saasus.io/v1/communication*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**CreateFeedback**](FeedbackApi.md#createfeedback) | **POST** /feedbacks | Create Feedback |
| [**CreateFeedbackComment**](FeedbackApi.md#createfeedbackcomment) | **POST** /feedbacks/{feedback_id}/comments | Create Feedback Comment |
| [**CreateVoteUser**](FeedbackApi.md#createvoteuser) | **POST** /feedbacks/{feedback_id}/votes/users | Create Vote User |
| [**DeleteFeedback**](FeedbackApi.md#deletefeedback) | **DELETE** /feedbacks/{feedback_id} | Delete Feedback |
| [**DeleteFeedbackComment**](FeedbackApi.md#deletefeedbackcomment) | **DELETE** /feedbacks/{feedback_id}/comments/{comment_id} | Delete Feedback Comment |
| [**DeleteVoteForFeedback**](FeedbackApi.md#deletevoteforfeedback) | **DELETE** /feedbacks/{feedback_id}/votes/users/{user_id} | Delete Vote For Feedback |
| [**GetFeedback**](FeedbackApi.md#getfeedback) | **GET** /feedbacks/{feedback_id} | Get Feedback |
| [**GetFeedbackComment**](FeedbackApi.md#getfeedbackcomment) | **GET** /feedbacks/{feedback_id}/comments/{comment_id} | Get Feedback Comment |
| [**GetFeedbacks**](FeedbackApi.md#getfeedbacks) | **GET** /feedbacks | Get Feedbacks |
| [**UpdateFeedback**](FeedbackApi.md#updatefeedback) | **PATCH** /feedbacks/{feedback_id} | Update Feedback |
| [**UpdateFeedbackComment**](FeedbackApi.md#updatefeedbackcomment) | **PATCH** /feedbacks/{feedback_id}/comments/{comment_id} | Update Feedback Comment |
| [**UpdateFeedbackStatus**](FeedbackApi.md#updatefeedbackstatus) | **PATCH** /feedbacks/{feedback_id}/status | Update Feedback Status |

<a id="createfeedback"></a>
# **CreateFeedback**
> Feedback CreateFeedback (CreateFeedbackParam createFeedbackParam = null)

Create Feedback

Create Feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class CreateFeedbackExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var createFeedbackParam = new CreateFeedbackParam(); // CreateFeedbackParam |  (optional) 

            try
            {
                // Create Feedback
                Feedback result = apiInstance.CreateFeedback(createFeedbackParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.CreateFeedback: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateFeedbackWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Feedback
    ApiResponse<Feedback> response = apiInstance.CreateFeedbackWithHttpInfo(createFeedbackParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.CreateFeedbackWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **createFeedbackParam** | [**CreateFeedbackParam**](CreateFeedbackParam.md) |  | [optional]  |

### Return type

[**Feedback**](Feedback.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | Created |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="createfeedbackcomment"></a>
# **CreateFeedbackComment**
> Comment CreateFeedbackComment (string feedbackId, CreateFeedbackCommentParam createFeedbackCommentParam = null)

Create Feedback Comment

Post comment to feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class CreateFeedbackCommentExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 
            var createFeedbackCommentParam = new CreateFeedbackCommentParam(); // CreateFeedbackCommentParam |  (optional) 

            try
            {
                // Create Feedback Comment
                Comment result = apiInstance.CreateFeedbackComment(feedbackId, createFeedbackCommentParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.CreateFeedbackComment: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateFeedbackCommentWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Feedback Comment
    ApiResponse<Comment> response = apiInstance.CreateFeedbackCommentWithHttpInfo(feedbackId, createFeedbackCommentParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.CreateFeedbackCommentWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |
| **createFeedbackCommentParam** | [**CreateFeedbackCommentParam**](CreateFeedbackCommentParam.md) |  | [optional]  |

### Return type

[**Comment**](Comment.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | Created |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="createvoteuser"></a>
# **CreateVoteUser**
> Votes CreateVoteUser (string feedbackId, CreateVoteUserParam createVoteUserParam = null)

Create Vote User

Vote for feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class CreateVoteUserExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 
            var createVoteUserParam = new CreateVoteUserParam(); // CreateVoteUserParam |  (optional) 

            try
            {
                // Create Vote User
                Votes result = apiInstance.CreateVoteUser(feedbackId, createVoteUserParam);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.CreateVoteUser: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the CreateVoteUserWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Create Vote User
    ApiResponse<Votes> response = apiInstance.CreateVoteUserWithHttpInfo(feedbackId, createVoteUserParam);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.CreateVoteUserWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |
| **createVoteUserParam** | [**CreateVoteUserParam**](CreateVoteUserParam.md) |  | [optional]  |

### Return type

[**Votes**](Votes.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **201** | Created |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletefeedback"></a>
# **DeleteFeedback**
> void DeleteFeedback (string feedbackId)

Delete Feedback

Delete Feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class DeleteFeedbackExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 

            try
            {
                // Delete Feedback
                apiInstance.DeleteFeedback(feedbackId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.DeleteFeedback: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteFeedbackWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Feedback
    apiInstance.DeleteFeedbackWithHttpInfo(feedbackId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.DeleteFeedbackWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletefeedbackcomment"></a>
# **DeleteFeedbackComment**
> void DeleteFeedbackComment (string feedbackId, string commentId)

Delete Feedback Comment

Delete comment for feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class DeleteFeedbackCommentExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 
            var commentId = "commentId_example";  // string | 

            try
            {
                // Delete Feedback Comment
                apiInstance.DeleteFeedbackComment(feedbackId, commentId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.DeleteFeedbackComment: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteFeedbackCommentWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Feedback Comment
    apiInstance.DeleteFeedbackCommentWithHttpInfo(feedbackId, commentId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.DeleteFeedbackCommentWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |
| **commentId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="deletevoteforfeedback"></a>
# **DeleteVoteForFeedback**
> void DeleteVoteForFeedback (string feedbackId, string userId)

Delete Vote For Feedback

Cancel vote for feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class DeleteVoteForFeedbackExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 
            var userId = "userId_example";  // string | 

            try
            {
                // Delete Vote For Feedback
                apiInstance.DeleteVoteForFeedback(feedbackId, userId);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.DeleteVoteForFeedback: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the DeleteVoteForFeedbackWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Delete Vote For Feedback
    apiInstance.DeleteVoteForFeedbackWithHttpInfo(feedbackId, userId);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.DeleteVoteForFeedbackWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |
| **userId** | **string** |  |  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getfeedback"></a>
# **GetFeedback**
> Feedback GetFeedback (string feedbackId)

Get Feedback

Retrieve feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class GetFeedbackExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 

            try
            {
                // Get Feedback
                Feedback result = apiInstance.GetFeedback(feedbackId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.GetFeedback: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetFeedbackWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Feedback
    ApiResponse<Feedback> response = apiInstance.GetFeedbackWithHttpInfo(feedbackId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.GetFeedbackWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |

### Return type

[**Feedback**](Feedback.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getfeedbackcomment"></a>
# **GetFeedbackComment**
> Comment GetFeedbackComment (string feedbackId, string commentId)

Get Feedback Comment

Retrieve comment from feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class GetFeedbackCommentExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 
            var commentId = "commentId_example";  // string | 

            try
            {
                // Get Feedback Comment
                Comment result = apiInstance.GetFeedbackComment(feedbackId, commentId);
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.GetFeedbackComment: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetFeedbackCommentWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Feedback Comment
    ApiResponse<Comment> response = apiInstance.GetFeedbackCommentWithHttpInfo(feedbackId, commentId);
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.GetFeedbackCommentWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |
| **commentId** | **string** |  |  |

### Return type

[**Comment**](Comment.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Ok |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="getfeedbacks"></a>
# **GetFeedbacks**
> Feedbacks GetFeedbacks ()

Get Feedbacks

Get the list of feedbacks.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class GetFeedbacksExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);

            try
            {
                // Get Feedbacks
                Feedbacks result = apiInstance.GetFeedbacks();
                Debug.WriteLine(result);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.GetFeedbacks: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the GetFeedbacksWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Get Feedbacks
    ApiResponse<Feedbacks> response = apiInstance.GetFeedbacksWithHttpInfo();
    Debug.Write("Status Code: " + response.StatusCode);
    Debug.Write("Response Headers: " + response.Headers);
    Debug.Write("Response Body: " + response.Data);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.GetFeedbacksWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters
This endpoint does not need any parameter.
### Return type

[**Feedbacks**](Feedbacks.md)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="updatefeedback"></a>
# **UpdateFeedback**
> void UpdateFeedback (string feedbackId, UpdateFeedbackParam updateFeedbackParam = null)

Update Feedback

Edit feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class UpdateFeedbackExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 
            var updateFeedbackParam = new UpdateFeedbackParam(); // UpdateFeedbackParam |  (optional) 

            try
            {
                // Update Feedback
                apiInstance.UpdateFeedback(feedbackId, updateFeedbackParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.UpdateFeedback: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateFeedbackWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Feedback
    apiInstance.UpdateFeedbackWithHttpInfo(feedbackId, updateFeedbackParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.UpdateFeedbackWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |
| **updateFeedbackParam** | [**UpdateFeedbackParam**](UpdateFeedbackParam.md) |  | [optional]  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="updatefeedbackcomment"></a>
# **UpdateFeedbackComment**
> void UpdateFeedbackComment (string feedbackId, string commentId, UpdateFeedbackCommentParam updateFeedbackCommentParam = null)

Update Feedback Comment

Edit comment for feedback.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class UpdateFeedbackCommentExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 
            var commentId = "commentId_example";  // string | 
            var updateFeedbackCommentParam = new UpdateFeedbackCommentParam(); // UpdateFeedbackCommentParam |  (optional) 

            try
            {
                // Update Feedback Comment
                apiInstance.UpdateFeedbackComment(feedbackId, commentId, updateFeedbackCommentParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.UpdateFeedbackComment: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateFeedbackCommentWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Feedback Comment
    apiInstance.UpdateFeedbackCommentWithHttpInfo(feedbackId, commentId, updateFeedbackCommentParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.UpdateFeedbackCommentWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |
| **commentId** | **string** |  |  |
| **updateFeedbackCommentParam** | [**UpdateFeedbackCommentParam**](UpdateFeedbackCommentParam.md) |  | [optional]  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

<a id="updatefeedbackstatus"></a>
# **UpdateFeedbackStatus**
> void UpdateFeedbackStatus (string feedbackId, UpdateFeedbackStatusParam updateFeedbackStatusParam = null)

Update Feedback Status

Update Feedback Status.

### Example
```csharp
using System.Collections.Generic;
using System.Diagnostics;
using communicationapi.Api;
using communicationapi.Model;
using modules;

namespace Example
{
    public class UpdateFeedbackStatusExample
    {
        public static void Main()
        {
            CommunicationApiClientConfig config = new Configuration().GetCommunicationApiClientConfig();
            var apiInstance = new FeedbackApi(config);
            var feedbackId = "feedbackId_example";  // string | 
            var updateFeedbackStatusParam = new UpdateFeedbackStatusParam(); // UpdateFeedbackStatusParam |  (optional) 

            try
            {
                // Update Feedback Status
                apiInstance.UpdateFeedbackStatus(feedbackId, updateFeedbackStatusParam);
            }
            catch (ApiException  e)
            {
                Debug.Print("Exception when calling FeedbackApi.UpdateFeedbackStatus: " + e.Message);
                Debug.Print("Status Code: " + e.ErrorCode);
                Debug.Print(e.StackTrace);
            }
        }
    }
}
```

#### Using the UpdateFeedbackStatusWithHttpInfo variant
This returns an ApiResponse object which contains the response data, status code and headers.

```csharp
try
{
    // Update Feedback Status
    apiInstance.UpdateFeedbackStatusWithHttpInfo(feedbackId, updateFeedbackStatusParam);
}
catch (ApiException e)
{
    Debug.Print("Exception when calling FeedbackApi.UpdateFeedbackStatusWithHttpInfo: " + e.Message);
    Debug.Print("Status Code: " + e.ErrorCode);
    Debug.Print(e.StackTrace);
}
```

### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **feedbackId** | **string** |  |  |
| **updateFeedbackStatusParam** | [**UpdateFeedbackStatusParam**](UpdateFeedbackStatusParam.md) |  | [optional]  |

### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | OK |  -  |
| **404** | Not Found |  -  |
| **500** | Internal Server Error |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

