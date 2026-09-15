using System.Net;
using communicationapi.Client;
using communicationapi.Model;
using Newtonsoft.Json.Linq;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Communication;

internal interface ICommunicationClient
{
    Feedbacks GetFeedbacks();
    ApiResponse<Feedbacks> GetFeedbacksWithHttpInfo();
    Task<Feedbacks> GetFeedbacksAsync(CancellationToken cancellationToken);
    Task<ApiResponse<Feedbacks>> GetFeedbacksWithHttpInfoAsync(CancellationToken cancellationToken);

    Feedback CreateFeedback(CreateFeedbackParam parameter);
    ApiResponse<Feedback> CreateFeedbackWithHttpInfo(CreateFeedbackParam parameter);
    Task<Feedback> CreateFeedbackAsync(CreateFeedbackParam parameter, CancellationToken cancellationToken);
    Task<ApiResponse<Feedback>> CreateFeedbackWithHttpInfoAsync(
        CreateFeedbackParam parameter,
        CancellationToken cancellationToken);

    void DeleteFeedback(string feedbackId);
    ApiResponse<object> DeleteFeedbackWithHttpInfo(string feedbackId);
    Task DeleteFeedbackAsync(string feedbackId, CancellationToken cancellationToken);
    Task<ApiResponse<object>> DeleteFeedbackWithHttpInfoAsync(
        string feedbackId,
        CancellationToken cancellationToken);

    Feedback GetFeedback(string feedbackId);
    ApiResponse<Feedback> GetFeedbackWithHttpInfo(string feedbackId);
    Task<Feedback> GetFeedbackAsync(string feedbackId, CancellationToken cancellationToken);
    Task<ApiResponse<Feedback>> GetFeedbackWithHttpInfoAsync(
        string feedbackId,
        CancellationToken cancellationToken);

    void UpdateFeedback(string feedbackId, UpdateFeedbackParam parameter);
    ApiResponse<object> UpdateFeedbackWithHttpInfo(string feedbackId, UpdateFeedbackParam parameter);
    Task UpdateFeedbackAsync(
        string feedbackId,
        UpdateFeedbackParam parameter,
        CancellationToken cancellationToken);
    Task<ApiResponse<object>> UpdateFeedbackWithHttpInfoAsync(
        string feedbackId,
        UpdateFeedbackParam parameter,
        CancellationToken cancellationToken);

    void UpdateFeedbackStatus(string feedbackId, UpdateFeedbackStatusParam parameter);
    ApiResponse<object> UpdateFeedbackStatusWithHttpInfo(
        string feedbackId,
        UpdateFeedbackStatusParam parameter);
    Task UpdateFeedbackStatusAsync(
        string feedbackId,
        UpdateFeedbackStatusParam parameter,
        CancellationToken cancellationToken);
    Task<ApiResponse<object>> UpdateFeedbackStatusWithHttpInfoAsync(
        string feedbackId,
        UpdateFeedbackStatusParam parameter,
        CancellationToken cancellationToken);

    Comment CreateFeedbackComment(string feedbackId, CreateFeedbackCommentParam parameter);
    ApiResponse<Comment> CreateFeedbackCommentWithHttpInfo(
        string feedbackId,
        CreateFeedbackCommentParam parameter);
    Task<Comment> CreateFeedbackCommentAsync(
        string feedbackId,
        CreateFeedbackCommentParam parameter,
        CancellationToken cancellationToken);
    Task<ApiResponse<Comment>> CreateFeedbackCommentWithHttpInfoAsync(
        string feedbackId,
        CreateFeedbackCommentParam parameter,
        CancellationToken cancellationToken);

    Comment GetFeedbackComment(string feedbackId, string commentId);
    ApiResponse<Comment> GetFeedbackCommentWithHttpInfo(string feedbackId, string commentId);
    Task<Comment> GetFeedbackCommentAsync(
        string feedbackId,
        string commentId,
        CancellationToken cancellationToken);
    Task<ApiResponse<Comment>> GetFeedbackCommentWithHttpInfoAsync(
        string feedbackId,
        string commentId,
        CancellationToken cancellationToken);

    void UpdateFeedbackComment(
        string feedbackId,
        string commentId,
        UpdateFeedbackCommentParam parameter);
    ApiResponse<object> UpdateFeedbackCommentWithHttpInfo(
        string feedbackId,
        string commentId,
        UpdateFeedbackCommentParam parameter);
    Task UpdateFeedbackCommentAsync(
        string feedbackId,
        string commentId,
        UpdateFeedbackCommentParam parameter,
        CancellationToken cancellationToken);
    Task<ApiResponse<object>> UpdateFeedbackCommentWithHttpInfoAsync(
        string feedbackId,
        string commentId,
        UpdateFeedbackCommentParam parameter,
        CancellationToken cancellationToken);

    Votes CreateVoteUser(string feedbackId, CreateVoteUserParam parameter);
    ApiResponse<Votes> CreateVoteUserWithHttpInfo(string feedbackId, CreateVoteUserParam parameter);
    Task<Votes> CreateVoteUserAsync(
        string feedbackId,
        CreateVoteUserParam parameter,
        CancellationToken cancellationToken);
    Task<ApiResponse<Votes>> CreateVoteUserWithHttpInfoAsync(
        string feedbackId,
        CreateVoteUserParam parameter,
        CancellationToken cancellationToken);

    void DeleteVoteForFeedback(string feedbackId, string userId);
    ApiResponse<object> DeleteVoteForFeedbackWithHttpInfo(string feedbackId, string userId);
    Task DeleteVoteForFeedbackAsync(
        string feedbackId,
        string userId,
        CancellationToken cancellationToken);
    Task<ApiResponse<object>> DeleteVoteForFeedbackWithHttpInfoAsync(
        string feedbackId,
        string userId,
        CancellationToken cancellationToken);

    void DeleteFeedbackComment(string feedbackId, string commentId);
    ApiResponse<object> DeleteFeedbackCommentWithHttpInfo(string feedbackId, string commentId);
    Task DeleteFeedbackCommentAsync(
        string feedbackId,
        string commentId,
        CancellationToken cancellationToken);
    Task<ApiResponse<object>> DeleteFeedbackCommentWithHttpInfoAsync(
        string feedbackId,
        string commentId,
        CancellationToken cancellationToken);
}

internal sealed class CommunicationClientAdapter : ICommunicationClient
{
    private readonly communicationapi.Api.IFeedbackApi _api;

    public CommunicationClientAdapter(communicationapi.Api.IFeedbackApi api) => _api = api;

    public Feedbacks GetFeedbacks() => _api.GetFeedbacks();
    public ApiResponse<Feedbacks> GetFeedbacksWithHttpInfo() => _api.GetFeedbacksWithHttpInfo();
    public Task<Feedbacks> GetFeedbacksAsync(CancellationToken cancellationToken) =>
        _api.GetFeedbacksAsync(cancellationToken: cancellationToken);
    public Task<ApiResponse<Feedbacks>> GetFeedbacksWithHttpInfoAsync(CancellationToken cancellationToken) =>
        _api.GetFeedbacksWithHttpInfoAsync(cancellationToken: cancellationToken);

    public Feedback CreateFeedback(CreateFeedbackParam parameter) => _api.CreateFeedback(parameter);
    public ApiResponse<Feedback> CreateFeedbackWithHttpInfo(CreateFeedbackParam parameter) =>
        _api.CreateFeedbackWithHttpInfo(parameter);
    public Task<Feedback> CreateFeedbackAsync(CreateFeedbackParam parameter, CancellationToken cancellationToken) =>
        _api.CreateFeedbackAsync(parameter, cancellationToken: cancellationToken);
    public Task<ApiResponse<Feedback>> CreateFeedbackWithHttpInfoAsync(
        CreateFeedbackParam parameter,
        CancellationToken cancellationToken) =>
        _api.CreateFeedbackWithHttpInfoAsync(parameter, cancellationToken: cancellationToken);

    public void DeleteFeedback(string feedbackId) => _api.DeleteFeedback(feedbackId);
    public ApiResponse<object> DeleteFeedbackWithHttpInfo(string feedbackId) =>
        _api.DeleteFeedbackWithHttpInfo(feedbackId);
    public Task DeleteFeedbackAsync(string feedbackId, CancellationToken cancellationToken) =>
        _api.DeleteFeedbackAsync(feedbackId, cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> DeleteFeedbackWithHttpInfoAsync(
        string feedbackId,
        CancellationToken cancellationToken) =>
        _api.DeleteFeedbackWithHttpInfoAsync(feedbackId, cancellationToken: cancellationToken);

    public Feedback GetFeedback(string feedbackId) => _api.GetFeedback(feedbackId);
    public ApiResponse<Feedback> GetFeedbackWithHttpInfo(string feedbackId) =>
        _api.GetFeedbackWithHttpInfo(feedbackId);
    public Task<Feedback> GetFeedbackAsync(string feedbackId, CancellationToken cancellationToken) =>
        _api.GetFeedbackAsync(feedbackId, cancellationToken: cancellationToken);
    public Task<ApiResponse<Feedback>> GetFeedbackWithHttpInfoAsync(
        string feedbackId,
        CancellationToken cancellationToken) =>
        _api.GetFeedbackWithHttpInfoAsync(feedbackId, cancellationToken: cancellationToken);

    public void UpdateFeedback(string feedbackId, UpdateFeedbackParam parameter) =>
        _api.UpdateFeedback(feedbackId, parameter);
    public ApiResponse<object> UpdateFeedbackWithHttpInfo(string feedbackId, UpdateFeedbackParam parameter) =>
        _api.UpdateFeedbackWithHttpInfo(feedbackId, parameter);
    public Task UpdateFeedbackAsync(
        string feedbackId,
        UpdateFeedbackParam parameter,
        CancellationToken cancellationToken) =>
        _api.UpdateFeedbackAsync(feedbackId, parameter, cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> UpdateFeedbackWithHttpInfoAsync(
        string feedbackId,
        UpdateFeedbackParam parameter,
        CancellationToken cancellationToken) =>
        _api.UpdateFeedbackWithHttpInfoAsync(feedbackId, parameter, cancellationToken: cancellationToken);

    public void UpdateFeedbackStatus(string feedbackId, UpdateFeedbackStatusParam parameter) =>
        _api.UpdateFeedbackStatus(feedbackId, parameter);
    public ApiResponse<object> UpdateFeedbackStatusWithHttpInfo(
        string feedbackId,
        UpdateFeedbackStatusParam parameter) =>
        _api.UpdateFeedbackStatusWithHttpInfo(feedbackId, parameter);
    public Task UpdateFeedbackStatusAsync(
        string feedbackId,
        UpdateFeedbackStatusParam parameter,
        CancellationToken cancellationToken) =>
        _api.UpdateFeedbackStatusAsync(feedbackId, parameter, cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> UpdateFeedbackStatusWithHttpInfoAsync(
        string feedbackId,
        UpdateFeedbackStatusParam parameter,
        CancellationToken cancellationToken) =>
        _api.UpdateFeedbackStatusWithHttpInfoAsync(feedbackId, parameter, cancellationToken: cancellationToken);

    public Comment CreateFeedbackComment(string feedbackId, CreateFeedbackCommentParam parameter) =>
        _api.CreateFeedbackComment(feedbackId, parameter);
    public ApiResponse<Comment> CreateFeedbackCommentWithHttpInfo(
        string feedbackId,
        CreateFeedbackCommentParam parameter) =>
        _api.CreateFeedbackCommentWithHttpInfo(feedbackId, parameter);
    public Task<Comment> CreateFeedbackCommentAsync(
        string feedbackId,
        CreateFeedbackCommentParam parameter,
        CancellationToken cancellationToken) =>
        _api.CreateFeedbackCommentAsync(feedbackId, parameter, cancellationToken: cancellationToken);
    public Task<ApiResponse<Comment>> CreateFeedbackCommentWithHttpInfoAsync(
        string feedbackId,
        CreateFeedbackCommentParam parameter,
        CancellationToken cancellationToken) =>
        _api.CreateFeedbackCommentWithHttpInfoAsync(feedbackId, parameter, cancellationToken: cancellationToken);

    public Comment GetFeedbackComment(string feedbackId, string commentId) =>
        _api.GetFeedbackComment(feedbackId, commentId);
    public ApiResponse<Comment> GetFeedbackCommentWithHttpInfo(string feedbackId, string commentId) =>
        _api.GetFeedbackCommentWithHttpInfo(feedbackId, commentId);
    public Task<Comment> GetFeedbackCommentAsync(
        string feedbackId,
        string commentId,
        CancellationToken cancellationToken) =>
        _api.GetFeedbackCommentAsync(feedbackId, commentId, cancellationToken: cancellationToken);
    public Task<ApiResponse<Comment>> GetFeedbackCommentWithHttpInfoAsync(
        string feedbackId,
        string commentId,
        CancellationToken cancellationToken) =>
        _api.GetFeedbackCommentWithHttpInfoAsync(feedbackId, commentId, cancellationToken: cancellationToken);

    public void UpdateFeedbackComment(
        string feedbackId,
        string commentId,
        UpdateFeedbackCommentParam parameter) =>
        _api.UpdateFeedbackComment(feedbackId, commentId, parameter);
    public ApiResponse<object> UpdateFeedbackCommentWithHttpInfo(
        string feedbackId,
        string commentId,
        UpdateFeedbackCommentParam parameter) =>
        _api.UpdateFeedbackCommentWithHttpInfo(feedbackId, commentId, parameter);
    public Task UpdateFeedbackCommentAsync(
        string feedbackId,
        string commentId,
        UpdateFeedbackCommentParam parameter,
        CancellationToken cancellationToken) =>
        _api.UpdateFeedbackCommentAsync(feedbackId, commentId, parameter, cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> UpdateFeedbackCommentWithHttpInfoAsync(
        string feedbackId,
        string commentId,
        UpdateFeedbackCommentParam parameter,
        CancellationToken cancellationToken) =>
        _api.UpdateFeedbackCommentWithHttpInfoAsync(
            feedbackId,
            commentId,
            parameter,
            cancellationToken: cancellationToken);

    public Votes CreateVoteUser(string feedbackId, CreateVoteUserParam parameter) =>
        _api.CreateVoteUser(feedbackId, parameter);
    public ApiResponse<Votes> CreateVoteUserWithHttpInfo(string feedbackId, CreateVoteUserParam parameter) =>
        _api.CreateVoteUserWithHttpInfo(feedbackId, parameter);
    public Task<Votes> CreateVoteUserAsync(
        string feedbackId,
        CreateVoteUserParam parameter,
        CancellationToken cancellationToken) =>
        _api.CreateVoteUserAsync(feedbackId, parameter, cancellationToken: cancellationToken);
    public Task<ApiResponse<Votes>> CreateVoteUserWithHttpInfoAsync(
        string feedbackId,
        CreateVoteUserParam parameter,
        CancellationToken cancellationToken) =>
        _api.CreateVoteUserWithHttpInfoAsync(feedbackId, parameter, cancellationToken: cancellationToken);

    public void DeleteVoteForFeedback(string feedbackId, string userId) =>
        _api.DeleteVoteForFeedback(feedbackId, userId);
    public ApiResponse<object> DeleteVoteForFeedbackWithHttpInfo(string feedbackId, string userId) =>
        _api.DeleteVoteForFeedbackWithHttpInfo(feedbackId, userId);
    public Task DeleteVoteForFeedbackAsync(
        string feedbackId,
        string userId,
        CancellationToken cancellationToken) =>
        _api.DeleteVoteForFeedbackAsync(feedbackId, userId, cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> DeleteVoteForFeedbackWithHttpInfoAsync(
        string feedbackId,
        string userId,
        CancellationToken cancellationToken) =>
        _api.DeleteVoteForFeedbackWithHttpInfoAsync(feedbackId, userId, cancellationToken: cancellationToken);

    public void DeleteFeedbackComment(string feedbackId, string commentId) =>
        _api.DeleteFeedbackComment(feedbackId, commentId);
    public ApiResponse<object> DeleteFeedbackCommentWithHttpInfo(string feedbackId, string commentId) =>
        _api.DeleteFeedbackCommentWithHttpInfo(feedbackId, commentId);
    public Task DeleteFeedbackCommentAsync(
        string feedbackId,
        string commentId,
        CancellationToken cancellationToken) =>
        _api.DeleteFeedbackCommentAsync(feedbackId, commentId, cancellationToken: cancellationToken);
    public Task<ApiResponse<object>> DeleteFeedbackCommentWithHttpInfoAsync(
        string feedbackId,
        string commentId,
        CancellationToken cancellationToken) =>
        _api.DeleteFeedbackCommentWithHttpInfoAsync(feedbackId, commentId, cancellationToken: cancellationToken);
}

internal static class CommunicationStoryFactory
{
    internal static readonly IReadOnlyList<(string Method, CallStyle CallStyle)> Methods = new[]
    {
        ("GetFeedbacks", CallStyle.Sync),
        ("CreateFeedback", CallStyle.Sync),
        ("DeleteFeedback", CallStyle.Sync),
        ("GetFeedback", CallStyle.Sync),
        ("UpdateFeedback", CallStyle.Sync),
        ("UpdateFeedbackStatus", CallStyle.Sync),
        ("CreateVoteUser", CallStyle.Sync),
        ("DeleteVoteForFeedback", CallStyle.Sync),
        ("CreateFeedbackComment", CallStyle.Sync),
        ("DeleteFeedbackComment", CallStyle.Sync),
        ("GetFeedbackComment", CallStyle.Sync),
        ("UpdateFeedbackComment", CallStyle.Sync),

        ("GetFeedbacksWithHttpInfo", CallStyle.WithHttpInfo),
        ("CreateFeedbackWithHttpInfo", CallStyle.WithHttpInfo),
        ("DeleteFeedbackWithHttpInfo", CallStyle.WithHttpInfo),
        ("GetFeedbackWithHttpInfo", CallStyle.WithHttpInfo),
        ("UpdateFeedbackWithHttpInfo", CallStyle.WithHttpInfo),
        ("UpdateFeedbackStatusWithHttpInfo", CallStyle.WithHttpInfo),
        ("CreateVoteUserWithHttpInfo", CallStyle.WithHttpInfo),
        ("DeleteVoteForFeedbackWithHttpInfo", CallStyle.WithHttpInfo),
        ("CreateFeedbackCommentWithHttpInfo", CallStyle.WithHttpInfo),
        ("DeleteFeedbackCommentWithHttpInfo", CallStyle.WithHttpInfo),
        ("GetFeedbackCommentWithHttpInfo", CallStyle.WithHttpInfo),
        ("UpdateFeedbackCommentWithHttpInfo", CallStyle.WithHttpInfo),

        ("GetFeedbacksAsync", CallStyle.Async),
        ("CreateFeedbackAsync", CallStyle.Async),
        ("DeleteFeedbackAsync", CallStyle.Async),
        ("GetFeedbackAsync", CallStyle.Async),
        ("UpdateFeedbackAsync", CallStyle.Async),
        ("UpdateFeedbackStatusAsync", CallStyle.Async),
        ("CreateVoteUserAsync", CallStyle.Async),
        ("DeleteVoteForFeedbackAsync", CallStyle.Async),
        ("CreateFeedbackCommentAsync", CallStyle.Async),
        ("DeleteFeedbackCommentAsync", CallStyle.Async),
        ("GetFeedbackCommentAsync", CallStyle.Async),
        ("UpdateFeedbackCommentAsync", CallStyle.Async),

        ("GetFeedbacksWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("CreateFeedbackWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("DeleteFeedbackWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("GetFeedbackWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("UpdateFeedbackWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("UpdateFeedbackStatusWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("CreateVoteUserWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("DeleteVoteForFeedbackWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("CreateFeedbackCommentWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("DeleteFeedbackCommentWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("GetFeedbackCommentWithHttpInfoAsync", CallStyle.WithHttpInfoAsync),
        ("UpdateFeedbackCommentWithHttpInfoAsync", CallStyle.WithHttpInfoAsync)
    };

    private static readonly IReadOnlyList<string> FeedbackJsonProperties = new[]
    {
        "id", "user_id", "feedback_title", "feedback_description"
    };

    private static readonly IReadOnlyList<string> CommentJsonProperties = new[]
    {
        "id", "created_at", "body"
    };

    private static readonly IReadOnlyList<string> VoteJsonProperties = new[]
    {
        "count", "users"
    };

    public static IReadOnlyList<Story> Create(ICommunicationClient client) => new[]
    {
        CreateStory(client, CallStyle.Sync, rawResponse: false),
        CreateStory(client, CallStyle.WithHttpInfo, rawResponse: false),
        CreateStory(client, CallStyle.Async, rawResponse: false),
        CreateStory(client, CallStyle.WithHttpInfoAsync, rawResponse: false)
    };

    public static IReadOnlyList<Story> CreateRaw(ICommunicationClient client) => new[]
    {
        CreateStory(client, CallStyle.WithHttpInfo, rawResponse: true),
        CreateStory(client, CallStyle.WithHttpInfoAsync, rawResponse: true)
    };

    private static Story CreateStory(ICommunicationClient client, CallStyle style, bool rawResponse)
    {
        var state = new CommunicationState();
        var styleName = style switch
        {
            CallStyle.Sync => "synchronous responses",
            CallStyle.WithHttpInfo => "synchronous HTTP responses",
            CallStyle.Async => "asynchronous responses",
            CallStyle.WithHttpInfoAsync => "asynchronous HTTP responses",
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        };

        var responseName = rawResponse
            ? $"Raw responses ({(style is CallStyle.WithHttpInfo ? "synchronous" : "asynchronous")})"
            : styleName;
        return new Story
        {
            Name = $"Communication API - {responseName}",
            Description = rawResponse
                ? "Validates Communication HTTP response wrappers, raw JSON bodies, and deserialized content."
                : "Exercises the complete feedback, comment, and vote lifecycle.",
            Module = "communication",
            InitialVariables = Variables(),
            SetupAsync = (_, cancellationToken) => CleanupAsync(client, state, cancellationToken),
            CleanupAsync = (_, cancellationToken) => CleanupAsync(client, state, cancellationToken),
            Steps = BuildSteps(client, state, style, rawResponse)
        };
    }

    private static IReadOnlyList<Step> BuildSteps(
        ICommunicationClient client,
        CommunicationState state,
        CallStyle style,
        bool rawResponse) => new[]
    {
        ValueStep(
            "GetFeedbacks",
            "GetFeedbacks",
            style,
            _ => client.GetFeedbacks(),
            _ => client.GetFeedbacksWithHttpInfo(),
            (_, cancellationToken) => client.GetFeedbacksAsync(cancellationToken),
            (_, cancellationToken) => client.GetFeedbacksWithHttpInfoAsync(cancellationToken),
            ValidateFeedbacks,
            _ => new Dictionary<string, object?>(),
            rawJsonProperties: rawResponse ? new[] { "feedbacks" } : null),

        ValueStep(
            "CreateFeedback",
            "CreateFeedback",
            style,
            context => client.CreateFeedback(CreateFeedbackParameter(context)),
            context => client.CreateFeedbackWithHttpInfo(CreateFeedbackParameter(context)),
            (context, cancellationToken) => client.CreateFeedbackAsync(
                CreateFeedbackParameter(context), cancellationToken),
            (context, cancellationToken) => client.CreateFeedbackWithHttpInfoAsync(
                CreateFeedbackParameter(context), cancellationToken),
            ValidateFeedback,
            context => CreateFeedbackParameter(context),
            (result, context) => CaptureFeedback(result, context, state),
            expectedStatus: 201,
            rawJsonProperties: rawResponse ? FeedbackJsonProperties : null),

        ValueStep(
            "GetFeedback",
            "GetFeedback",
            style,
            context => client.GetFeedback(FeedbackId(context)),
            context => client.GetFeedbackWithHttpInfo(FeedbackId(context)),
            (context, cancellationToken) => client.GetFeedbackAsync(FeedbackId(context), cancellationToken),
            (context, cancellationToken) => client.GetFeedbackWithHttpInfoAsync(
                FeedbackId(context), cancellationToken),
            ValidateFeedback,
            context => new { feedbackId = FeedbackId(context) },
            rawJsonProperties: rawResponse ? FeedbackJsonProperties : null),

        VoidStep(
            "UpdateFeedback",
            "UpdateFeedback",
            style,
            context => client.UpdateFeedback(FeedbackId(context), UpdateFeedbackParameter()),
            context => client.UpdateFeedbackWithHttpInfo(FeedbackId(context), UpdateFeedbackParameter()),
            (context, cancellationToken) => client.UpdateFeedbackAsync(
                FeedbackId(context), UpdateFeedbackParameter(), cancellationToken),
            (context, cancellationToken) => client.UpdateFeedbackWithHttpInfoAsync(
                FeedbackId(context), UpdateFeedbackParameter(), cancellationToken),
            context => new
            {
                feedbackId = FeedbackId(context),
                requestBody = UpdateFeedbackParameter()
            },
            rawResponse: rawResponse),

        ValueStep(
            "GetFeedbackAfterUpdate",
            "GetFeedback",
            style,
            context => client.GetFeedback(FeedbackId(context)),
            context => client.GetFeedbackWithHttpInfo(FeedbackId(context)),
            (context, cancellationToken) => client.GetFeedbackAsync(FeedbackId(context), cancellationToken),
            (context, cancellationToken) => client.GetFeedbackWithHttpInfoAsync(
                FeedbackId(context), cancellationToken),
            ValidateUpdatedFeedback,
            context => new { feedbackId = FeedbackId(context) },
            rawJsonProperties: rawResponse ? FeedbackJsonProperties : null),

        VoidStep(
            "UpdateFeedbackStatus",
            "UpdateFeedbackStatus",
            style,
            context => client.UpdateFeedbackStatus(FeedbackId(context), UpdateFeedbackStatusParameter()),
            context => client.UpdateFeedbackStatusWithHttpInfo(
                FeedbackId(context), UpdateFeedbackStatusParameter()),
            (context, cancellationToken) => client.UpdateFeedbackStatusAsync(
                FeedbackId(context), UpdateFeedbackStatusParameter(), cancellationToken),
            (context, cancellationToken) => client.UpdateFeedbackStatusWithHttpInfoAsync(
                FeedbackId(context), UpdateFeedbackStatusParameter(), cancellationToken),
            context => new
            {
                feedbackId = FeedbackId(context),
                requestBody = UpdateFeedbackStatusParameter()
            },
            rawResponse: rawResponse),

        ValueStep(
            "GetFeedbackAfterStatusUpdate",
            "GetFeedback",
            style,
            context => client.GetFeedback(FeedbackId(context)),
            context => client.GetFeedbackWithHttpInfo(FeedbackId(context)),
            (context, cancellationToken) => client.GetFeedbackAsync(FeedbackId(context), cancellationToken),
            (context, cancellationToken) => client.GetFeedbackWithHttpInfoAsync(
                FeedbackId(context), cancellationToken),
            ValidateUpdatedFeedbackStatus,
            context => new { feedbackId = FeedbackId(context) },
            rawJsonProperties: rawResponse ? FeedbackJsonProperties : null),

        ValueStep(
            "CreateFeedbackComment",
            "CreateFeedbackComment",
            style,
            context => client.CreateFeedbackComment(FeedbackId(context), CreateCommentParameter()),
            context => client.CreateFeedbackCommentWithHttpInfo(
                FeedbackId(context), CreateCommentParameter()),
            (context, cancellationToken) => client.CreateFeedbackCommentAsync(
                FeedbackId(context), CreateCommentParameter(), cancellationToken),
            (context, cancellationToken) => client.CreateFeedbackCommentWithHttpInfoAsync(
                FeedbackId(context), CreateCommentParameter(), cancellationToken),
            ValidateComment,
            context => new
            {
                feedbackId = FeedbackId(context),
                requestBody = CreateCommentParameter()
            },
            (result, context) => CaptureComment(result, context, state),
            expectedStatus: 201,
            rawJsonProperties: rawResponse ? CommentJsonProperties : null),

        ValueStep(
            "GetFeedbackComment",
            "GetFeedbackComment",
            style,
            context => client.GetFeedbackComment(FeedbackId(context), CommentId(context)),
            context => client.GetFeedbackCommentWithHttpInfo(
                FeedbackId(context), CommentId(context)),
            (context, cancellationToken) => client.GetFeedbackCommentAsync(
                FeedbackId(context), CommentId(context), cancellationToken),
            (context, cancellationToken) => client.GetFeedbackCommentWithHttpInfoAsync(
                FeedbackId(context), CommentId(context), cancellationToken),
            ValidateComment,
            context => new
            {
                feedbackId = FeedbackId(context),
                commentId = CommentId(context)
            },
            rawJsonProperties: rawResponse ? CommentJsonProperties : null),

        VoidStep(
            "UpdateFeedbackComment",
            "UpdateFeedbackComment",
            style,
            context => client.UpdateFeedbackComment(
                FeedbackId(context), CommentId(context), UpdateCommentParameter()),
            context => client.UpdateFeedbackCommentWithHttpInfo(
                FeedbackId(context), CommentId(context), UpdateCommentParameter()),
            (context, cancellationToken) => client.UpdateFeedbackCommentAsync(
                FeedbackId(context), CommentId(context), UpdateCommentParameter(), cancellationToken),
            (context, cancellationToken) => client.UpdateFeedbackCommentWithHttpInfoAsync(
                FeedbackId(context), CommentId(context), UpdateCommentParameter(), cancellationToken),
            context => new
            {
                feedbackId = FeedbackId(context),
                commentId = CommentId(context),
                requestBody = UpdateCommentParameter()
            },
            rawResponse: rawResponse),

        ValueStep(
            "GetFeedbackCommentAfterUpdate",
            "GetFeedbackComment",
            style,
            context => client.GetFeedbackComment(FeedbackId(context), CommentId(context)),
            context => client.GetFeedbackCommentWithHttpInfo(
                FeedbackId(context), CommentId(context)),
            (context, cancellationToken) => client.GetFeedbackCommentAsync(
                FeedbackId(context), CommentId(context), cancellationToken),
            (context, cancellationToken) => client.GetFeedbackCommentWithHttpInfoAsync(
                FeedbackId(context), CommentId(context), cancellationToken),
            ValidateUpdatedComment,
            context => new
            {
                feedbackId = FeedbackId(context),
                commentId = CommentId(context)
            },
            rawJsonProperties: rawResponse ? CommentJsonProperties : null),

        ValueStep(
            "CreateVoteUser",
            "CreateVoteUser",
            style,
            context => client.CreateVoteUser(FeedbackId(context), CreateVoteParameter(context)),
            context => client.CreateVoteUserWithHttpInfo(
                FeedbackId(context), CreateVoteParameter(context)),
            (context, cancellationToken) => client.CreateVoteUserAsync(
                FeedbackId(context), CreateVoteParameter(context), cancellationToken),
            (context, cancellationToken) => client.CreateVoteUserWithHttpInfoAsync(
                FeedbackId(context), CreateVoteParameter(context), cancellationToken),
            ValidateVotes,
            context => new
            {
                feedbackId = FeedbackId(context),
                requestBody = CreateVoteParameter(context)
            },
            expectedStatus: 201,
            rawJsonProperties: rawResponse ? VoteJsonProperties : null),

        VoidStep(
            "DeleteVoteForFeedback",
            "DeleteVoteForFeedback",
            style,
            context => client.DeleteVoteForFeedback(FeedbackId(context), UserId(context)),
            context => client.DeleteVoteForFeedbackWithHttpInfo(
                FeedbackId(context), UserId(context)),
            (context, cancellationToken) => client.DeleteVoteForFeedbackAsync(
                FeedbackId(context), UserId(context), cancellationToken),
            (context, cancellationToken) => client.DeleteVoteForFeedbackWithHttpInfoAsync(
                FeedbackId(context), UserId(context), cancellationToken),
            context => new
            {
                feedbackId = FeedbackId(context),
                userId = UserId(context)
            },
            rawResponse: rawResponse),

        VoidStep(
            "DeleteFeedbackComment",
            "DeleteFeedbackComment",
            style,
            context => client.DeleteFeedbackComment(FeedbackId(context), CommentId(context)),
            context => client.DeleteFeedbackCommentWithHttpInfo(
                FeedbackId(context), CommentId(context)),
            (context, cancellationToken) => client.DeleteFeedbackCommentAsync(
                FeedbackId(context), CommentId(context), cancellationToken),
            (context, cancellationToken) => client.DeleteFeedbackCommentWithHttpInfoAsync(
                FeedbackId(context), CommentId(context), cancellationToken),
            context => new
            {
                feedbackId = FeedbackId(context),
                commentId = CommentId(context)
            },
            (result, context) => ClearComment(context, state),
            rawResponse: rawResponse),

        VoidStep(
            "DeleteFeedback",
            "DeleteFeedback",
            style,
            context => client.DeleteFeedback(FeedbackId(context)),
            context => client.DeleteFeedbackWithHttpInfo(FeedbackId(context)),
            (context, cancellationToken) => client.DeleteFeedbackAsync(
                FeedbackId(context), cancellationToken),
            (context, cancellationToken) => client.DeleteFeedbackWithHttpInfoAsync(
                FeedbackId(context), cancellationToken),
            context => new { feedbackId = FeedbackId(context) },
            (result, context) => ClearFeedback(context, state),
            rawResponse: rawResponse)
    };

    private static Step ValueStep<T>(
        string name,
        string method,
        CallStyle style,
        Func<TestContext, T> syncCall,
        Func<TestContext, ApiResponse<T>> syncHttpCall,
        Func<TestContext, CancellationToken, Task<T>> asyncCall,
        Func<TestContext, CancellationToken, Task<ApiResponse<T>>> asyncHttpCall,
        Func<ExecutionResult, TestContext, Task> validate,
        Func<TestContext, object?>? parameters = null,
        Func<ExecutionResult, TestContext, Task>? updateState = null,
        int expectedStatus = 200,
        IReadOnlyList<string>? rawJsonProperties = null) => new()
    {
        Name = name,
        Method = MethodName(method, style),
        CallStyle = style,
        Parameters = parameters,
        ExpectedStatus = ExpectedStatusFor(style, expectedStatus),
        ExecuteAsync = style switch
        {
            CallStyle.Sync => MethodExecutor.Sync(context => (object?)syncCall(context)),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(
                context => ToExecutionResult(syncHttpCall(context))),
            CallStyle.Async => MethodExecutor.Async(async (context, cancellationToken) =>
                (object?)await asyncCall(context, cancellationToken).ConfigureAwait(false)),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(
                async (context, cancellationToken) => ToExecutionResult(
                    await asyncHttpCall(context, cancellationToken).ConfigureAwait(false))),
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        },
        ValidateAsync = (result, context) => ValidateValue(
            result,
            context,
            validate,
            rawJsonProperties),
        UpdateStateAsync = updateState
    };

    private static Step VoidStep(
        string name,
        string method,
        CallStyle style,
        Action<TestContext> syncCall,
        Func<TestContext, ApiResponse<object>> syncHttpCall,
        Func<TestContext, CancellationToken, Task> asyncCall,
        Func<TestContext, CancellationToken, Task<ApiResponse<object>>> asyncHttpCall,
        Func<TestContext, object?>? parameters = null,
        Func<ExecutionResult, TestContext, Task>? updateState = null,
        int expectedStatus = 200,
        bool rawResponse = false) => new()
    {
        Name = name,
        Method = MethodName(method, style),
        CallStyle = style,
        Parameters = parameters,
        ExpectedStatus = ExpectedStatusFor(style, expectedStatus),
        ExecuteAsync = style switch
        {
            CallStyle.Sync => MethodExecutor.Sync(context => syncCall(context)),
            CallStyle.WithHttpInfo => MethodExecutor.WithHttpInfo(
                context => ToExecutionResult(syncHttpCall(context))),
            CallStyle.Async => MethodExecutor.Async((context, cancellationToken) =>
                asyncCall(context, cancellationToken)),
            CallStyle.WithHttpInfoAsync => MethodExecutor.WithHttpInfoAsync(
                async (context, cancellationToken) => ToExecutionResult(
                    await asyncHttpCall(context, cancellationToken).ConfigureAwait(false))),
            _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
        },
        ValidateAsync = (result, context) => ValidateVoidResponse(result, context, rawResponse),
        UpdateStateAsync = updateState
    };

    private static int? ExpectedStatusFor(CallStyle style, int status) =>
        style is CallStyle.WithHttpInfo or CallStyle.WithHttpInfoAsync ? status : null;

    private static string MethodName(string baseName, CallStyle style) => style switch
    {
        CallStyle.Sync => baseName,
        CallStyle.WithHttpInfo => $"{baseName}WithHttpInfo",
        CallStyle.Async => $"{baseName}Async",
        CallStyle.WithHttpInfoAsync => $"{baseName}WithHttpInfoAsync",
        _ => throw new ArgumentOutOfRangeException(nameof(style), style, null)
    };

    private static ExecutionResult ToExecutionResult<T>(ApiResponse<T> response) =>
        new(
            response.Data,
            (int)response.StatusCode,
            Headers(response.Headers),
            Body: response.RawContent,
            RawResponse: response);

    private static IReadOnlyDictionary<string, IReadOnlyList<string>>? Headers(
        Multimap<string, string>? headers) =>
        headers?.ToDictionary(
            pair => pair.Key,
            pair => (IReadOnlyList<string>)pair.Value.ToArray(),
            StringComparer.OrdinalIgnoreCase);

    private static Task ValidateFeedbacks(ExecutionResult result, TestContext _)
    {
        if (result.Response is not Feedbacks feedbacks || feedbacks.VarFeedbacks is null)
            throw new InvalidOperationException("Expected Communication API to return a feedback list.");
        return Task.CompletedTask;
    }

    private static async Task ValidateValue(
        ExecutionResult result,
        TestContext context,
        Func<ExecutionResult, TestContext, Task> typedValidator,
        IReadOnlyList<string>? rawJsonProperties)
    {
        await typedValidator(result, context).ConfigureAwait(false);
        if (rawJsonProperties is not null)
            ValidateRawJsonResponse(result, rawJsonProperties);
    }

    private static async Task ValidateVoidResponse(
        ExecutionResult result,
        TestContext context,
        bool validateRawResponse)
    {
        await ValidateVoid(result, context).ConfigureAwait(false);
        if (validateRawResponse)
            ValidateRawResponseEnvelope(result);
    }

    private static Task ValidateFeedback(ExecutionResult result, TestContext _)
    {
        if (result.Response is not Feedback feedback)
            throw new InvalidOperationException("Expected Communication API to return a feedback.");
        if (string.IsNullOrWhiteSpace(feedback.Id) ||
            string.IsNullOrWhiteSpace(feedback.UserId) ||
            string.IsNullOrWhiteSpace(feedback.FeedbackTitle) ||
            string.IsNullOrWhiteSpace(feedback.FeedbackDescription))
            throw new InvalidOperationException("Communication feedback response is missing required fields.");
        return Task.CompletedTask;
    }

    private static Task ValidateComment(ExecutionResult result, TestContext _)
    {
        if (result.Response is not Comment comment ||
            string.IsNullOrWhiteSpace(comment.Id) ||
            comment.CreatedAt == 0 ||
            string.IsNullOrWhiteSpace(comment.Body))
            throw new InvalidOperationException("Communication comment response is missing required fields.");
        return Task.CompletedTask;
    }

    private static Task ValidateUpdatedFeedback(ExecutionResult result, TestContext context)
    {
        ValidateFeedback(result, context);
        if (result.Response is not Feedback feedback ||
            !string.Equals(feedback.FeedbackTitle, UpdatedFeedbackTitle, StringComparison.Ordinal) ||
            !string.Equals(feedback.FeedbackDescription, UpdatedFeedbackDescription, StringComparison.Ordinal))
            throw new InvalidOperationException("Communication feedback update was not persisted.");
        return Task.CompletedTask;
    }

    private static Task ValidateUpdatedFeedbackStatus(ExecutionResult result, TestContext context)
    {
        ValidateFeedback(result, context);
        if (result.Response is not Feedback feedback || feedback.Status != UpdatedFeedbackStatus)
            throw new InvalidOperationException("Communication feedback status update was not persisted.");
        return Task.CompletedTask;
    }

    private static Task ValidateUpdatedComment(ExecutionResult result, TestContext context)
    {
        ValidateComment(result, context);
        if (result.Response is not Comment comment ||
            !string.Equals(comment.Body, UpdatedCommentBody, StringComparison.Ordinal))
            throw new InvalidOperationException("Communication comment update was not persisted.");
        return Task.CompletedTask;
    }

    private static Task ValidateVotes(ExecutionResult result, TestContext _)
    {
        if (result.Response is not Votes votes || votes.Users is null || votes.Count < 1)
            throw new InvalidOperationException("Communication vote response is missing vote information.");
        return Task.CompletedTask;
    }

    private static Task ValidateVoid(ExecutionResult result, TestContext _)
    {
        if (result.Response is not null)
            throw new InvalidOperationException("Expected an empty Communication API response.");
        return Task.CompletedTask;
    }

    private static void ValidateRawResponseEnvelope(ExecutionResult result)
    {
        if (result.RawResponse is not IApiResponse)
            throw new InvalidOperationException(
                "Expected Communication WithHttpInfo call to preserve its ApiResponse wrapper.");
        if (result.StatusCode is not > 0)
            throw new InvalidOperationException("Communication raw response did not expose an HTTP status.");
    }

    private static void ValidateRawJsonResponse(
        ExecutionResult result,
        IReadOnlyList<string> requiredProperties)
    {
        ValidateRawResponseEnvelope(result);
        if (string.IsNullOrWhiteSpace(result.Body))
            throw new InvalidOperationException("Communication raw response body was empty.");

        JToken payload;
        try
        {
            payload = JToken.Parse(result.Body);
        }
        catch (Newtonsoft.Json.JsonException error)
        {
            throw new InvalidOperationException("Communication raw response body was not valid JSON.", error);
        }

        if (payload is not JObject json)
            throw new InvalidOperationException("Communication raw response was not a JSON object.");

        foreach (var property in requiredProperties)
        {
            var value = json[property];
            if (value is null || value.Type == JTokenType.Null)
                throw new InvalidOperationException(
                    $"Communication raw response did not contain '{property}'.");

            switch (property)
            {
                case "feedbacks":
                case "users":
                    if (value is not JArray)
                        throw new InvalidOperationException(
                            $"Communication raw response property '{property}' was not an array.");
                    break;
                case "count":
                    if (value.Type != JTokenType.Integer || value.Value<int>() < 1)
                        throw new InvalidOperationException(
                            "Communication raw response contained an invalid vote count.");
                    break;
                case "created_at":
                    if (value.Type != JTokenType.Integer || value.Value<long>() == 0)
                        throw new InvalidOperationException(
                            "Communication raw response contained an invalid comment created_at.");
                    break;
                default:
                    if (value.Type != JTokenType.String || string.IsNullOrWhiteSpace(value.Value<string>()))
                        throw new InvalidOperationException(
                            $"Communication raw response property '{property}' was not a non-empty string.");
                    break;
            }
        }
    }

    private static Task CaptureFeedback(
        ExecutionResult result,
        TestContext context,
        CommunicationState state)
    {
        if (result.Response is not Feedback feedback || string.IsNullOrWhiteSpace(feedback.Id))
            throw new InvalidOperationException("Could not capture the created Communication feedback ID.");
        state.FeedbackId = feedback.Id;
        context.Variables["feedback_id"] = feedback.Id;
        return Task.CompletedTask;
    }

    private static Task CaptureComment(
        ExecutionResult result,
        TestContext context,
        CommunicationState state)
    {
        if (result.Response is not Comment comment || string.IsNullOrWhiteSpace(comment.Id))
            throw new InvalidOperationException("Could not capture the created Communication comment ID.");
        state.CommentId = comment.Id;
        context.Variables["comment_id"] = comment.Id;
        return Task.CompletedTask;
    }

    private static Task ClearComment(TestContext context, CommunicationState state)
    {
        state.CommentId = string.Empty;
        context.Variables["comment_id"] = string.Empty;
        return Task.CompletedTask;
    }

    private static Task ClearFeedback(TestContext context, CommunicationState state)
    {
        state.FeedbackId = string.Empty;
        state.CommentId = string.Empty;
        context.Variables["feedback_id"] = string.Empty;
        context.Variables["comment_id"] = string.Empty;
        return Task.CompletedTask;
    }

    private const string CreatedFeedbackTitle = "C# Communication E2E Feedback";
    private const string UpdatedFeedbackTitle = "C# Communication E2E Feedback Updated";
    private const string UpdatedFeedbackDescription = "Updated by the C# SDK E2E test.";
    private const int UpdatedFeedbackStatus = 1;
    private const string UpdatedCommentBody = "C# Communication E2E test comment updated.";

    private static IReadOnlyDictionary<string, object?> Variables() => new Dictionary<string, object?>
    {
        ["feedback_id"] = string.Empty,
        ["comment_id"] = string.Empty,
        ["user_id"] = TestUserId()
    };

    private static string TestUserId() =>
        string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("TEST_USER_ID"))
            ? "00000000-0000-0000-0000-000000000000"
            : Environment.GetEnvironmentVariable("TEST_USER_ID")!;

    private static string UserId(TestContext context) => context.GetRequired<string>("user_id");
    private static string FeedbackId(TestContext context) => context.GetRequired<string>("feedback_id");
    private static string CommentId(TestContext context) => context.GetRequired<string>("comment_id");

    private static CreateFeedbackParam CreateFeedbackParameter(TestContext context) => new(
        CreatedFeedbackTitle,
        "Feedback created by the C# SDK E2E test.",
        UserId(context));

    private static UpdateFeedbackParam UpdateFeedbackParameter() => new(
        UpdatedFeedbackTitle,
        UpdatedFeedbackDescription);

    private static UpdateFeedbackStatusParam UpdateFeedbackStatusParameter() => new(UpdatedFeedbackStatus);

    private static CreateFeedbackCommentParam CreateCommentParameter() => new(
        "C# Communication E2E test comment.");

    private static UpdateFeedbackCommentParam UpdateCommentParameter() => new(UpdatedCommentBody);

    private static CreateVoteUserParam CreateVoteParameter(TestContext context) => new(UserId(context));

    private static async Task CleanupAsync(
        ICommunicationClient client,
        CommunicationState state,
        CancellationToken cancellationToken)
    {
        var feedbackId = state.FeedbackId;
        if (string.IsNullOrWhiteSpace(feedbackId))
        {
            // A create that committed remotely without a usable response records no identifier, so
            // the story's own fixtures are located by their titles instead of being left behind.
            await DeleteFixtureFeedbacksAsync(client, cancellationToken).ConfigureAwait(false);
            return;
        }

        try
        {
            await client.DeleteFeedbackAsync(feedbackId, cancellationToken).ConfigureAwait(false);
        }
        catch (ApiException error) when (error.ErrorCode == (int)HttpStatusCode.NotFound)
        {
            // A normal delete may already have removed the feedback.
        }

        // The state is cleared only once the resource is really gone. Clearing it in a finally
        // block would lose the identifier needed to retry a failed cleanup.
        state.FeedbackId = string.Empty;
        state.CommentId = string.Empty;
    }

    /// <summary>
    /// Deletes the feedbacks this suite creates, identified by the fixture titles combined with the
    /// configured test user. It is the only way back to a clean environment when a create succeeded
    /// remotely but its identifier was never recorded. Stories run one after another, so the only
    /// owner of such a fixture is this suite.
    /// </summary>
    private static async Task DeleteFixtureFeedbacksAsync(
        ICommunicationClient client,
        CancellationToken cancellationToken)
    {
        var userId = TestUserId();
        var feedbacks = await client.GetFeedbacksAsync(cancellationToken).ConfigureAwait(false);
        foreach (var feedback in feedbacks.VarFeedbacks ?? new List<Feedback>())
        {
            if (string.IsNullOrWhiteSpace(feedback.Id) ||
                !IsFixtureTitle(feedback.FeedbackTitle) ||
                !string.Equals(feedback.UserId, userId, StringComparison.Ordinal))
                continue;
            try
            {
                await client.DeleteFeedbackAsync(feedback.Id, cancellationToken).ConfigureAwait(false);
            }
            catch (ApiException error) when (error.ErrorCode == (int)HttpStatusCode.NotFound)
            {
                // Another cleanup pass already removed it.
            }
        }
    }

    private static bool IsFixtureTitle(string? title) =>
        string.Equals(title, CreatedFeedbackTitle, StringComparison.Ordinal) ||
        string.Equals(title, UpdatedFeedbackTitle, StringComparison.Ordinal);

    private sealed class CommunicationState
    {
        public string FeedbackId { get; set; } = string.Empty;
        public string CommentId { get; set; } = string.Empty;
    }
}
