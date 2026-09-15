using System.Net;
using communicationapi.Api;
using communicationapi.Client;
using communicationapi.Model;
using Newtonsoft.Json;
using SaasusSdk.Tests.TestLib;

namespace SaasusSdk.Tests.E2E.Communication;

public sealed class CommunicationStoryFactoryTests
{
    [Fact]
    public async Task ExecutesEveryCommunicationMethodAndLeavesFeedbackDeleted()
    {
        var client = new FakeCommunicationClient();
        var stories = CommunicationStoryFactory.Create(client)
            .Concat(CommunicationStoryFactory.CreateRaw(client))
            .ToArray();
        var coverage = new CoverageTracker();
        foreach (var method in CommunicationStoryFactory.Methods)
            coverage.Register(method.Method, method.CallStyle);

        var results = await new E2EEngine(new Config
        {
            Timeout = TimeSpan.FromSeconds(2),
            MaxRetries = 0,
            LogLevel = LogLevel.None
        }, coverage).ExecuteAsync(stories);

        Assert.Equal(6, results.Count);
        Assert.Equal(90, results.Sum(result => result.Steps.Count));
        Assert.All(results, result => Assert.Equal(TestStatus.Passed, result.Status));
        var rawResults = results.Where(result => result.Name.Contains("Raw responses", StringComparison.Ordinal));
        Assert.Equal(2, rawResults.Count());
        Assert.All(rawResults.SelectMany(result => result.Steps), step =>
        {
            Assert.NotNull(step.StatusCode);
            Assert.False(string.IsNullOrWhiteSpace(step.Body));
        });
        Assert.Empty(coverage.Untested);
        Assert.Equal(48, coverage.Entries.Count);
        Assert.Null(client.Feedback);
        Assert.Null(client.Comment);
    }

    [Fact]
    public async Task CleanupRemovesTheFixtureFeedbackWithoutARecordedIdentifier()
    {
        var client = new FakeCommunicationClient();
        var story = CommunicationStoryFactory.Create(client).First();
        // A create that committed remotely but whose response never reached the state capture: the
        // title is spelled out independently of the factory's own fixture constant.
        client.CreateFeedback(new CreateFeedbackParam(
            "C# Communication E2E Feedback",
            "Feedback created by the C# SDK E2E test.",
            "00000000-0000-0000-0000-000000000000"));
        Assert.NotNull(client.Feedback);
        Assert.NotNull(story.CleanupAsync);

        await story.CleanupAsync!(
            new TestContext(new Dictionary<string, object?>(story.InitialVariables)),
            CancellationToken.None);

        Assert.Null(client.Feedback);
    }

    [Fact]
    public void DeclaresExactlyTheGeneratedFeedbackApiMethods()
    {
        var generatedMethods = typeof(IFeedbackApi)
            .GetInterfaces()
            .SelectMany(type => type.GetMethods())
            .Select(method => method.Name)
            .Where(name => name != "GetBasePath" &&
                           !name.StartsWith("get_", StringComparison.Ordinal) &&
                           !name.StartsWith("set_", StringComparison.Ordinal))
            .Distinct(StringComparer.Ordinal)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();
        var declaredMethods = CommunicationStoryFactory.Methods
            .Select(method => method.Method)
            .OrderBy(name => name, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(generatedMethods, declaredMethods);
    }

    private sealed class FakeCommunicationClient : ICommunicationClient
    {
        private int _nextId = 1;

        public Feedback? Feedback { get; private set; }
        public Comment? Comment { get; private set; }

        public Feedbacks GetFeedbacks() => new(CopyFeedbacks());
        public ApiResponse<Feedbacks> GetFeedbacksWithHttpInfo() => Response(GetFeedbacks());
        public Task<Feedbacks> GetFeedbacksAsync(CancellationToken cancellationToken) =>
            Task.FromResult(GetFeedbacks());
        public Task<ApiResponse<Feedbacks>> GetFeedbacksWithHttpInfoAsync(CancellationToken cancellationToken) =>
            Task.FromResult(GetFeedbacksWithHttpInfo());

        public Feedback CreateFeedback(CreateFeedbackParam parameter)
        {
            Assert.False(string.IsNullOrWhiteSpace(parameter.UserId));
            Assert.False(string.IsNullOrWhiteSpace(parameter.FeedbackTitle));
            Assert.False(string.IsNullOrWhiteSpace(parameter.FeedbackDescription));
            Feedback = new Feedback(
                parameter.FeedbackTitle,
                parameter.FeedbackDescription,
                new List<Comment>(),
                0,
                new List<User>(),
                $"feedback-{_nextId++}",
                parameter.UserId,
                1,
                0);
            Comment = null;
            return Copy(Feedback);
        }
        public ApiResponse<Feedback> CreateFeedbackWithHttpInfo(CreateFeedbackParam parameter) =>
            Response(CreateFeedback(parameter), HttpStatusCode.Created);
        public Task<Feedback> CreateFeedbackAsync(
            CreateFeedbackParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(CreateFeedback(parameter));
        public Task<ApiResponse<Feedback>> CreateFeedbackWithHttpInfoAsync(
            CreateFeedbackParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(CreateFeedbackWithHttpInfo(parameter));

        public void DeleteFeedback(string feedbackId)
        {
            Assert.Equal(Feedback?.Id, feedbackId);
            Feedback = null;
            Comment = null;
        }
        public ApiResponse<object> DeleteFeedbackWithHttpInfo(string feedbackId)
        {
            DeleteFeedback(feedbackId);
            return EmptyResponse();
        }
        public Task DeleteFeedbackAsync(string feedbackId, CancellationToken cancellationToken)
        {
            DeleteFeedback(feedbackId);
            return Task.CompletedTask;
        }
        public Task<ApiResponse<object>> DeleteFeedbackWithHttpInfoAsync(
            string feedbackId,
            CancellationToken cancellationToken) =>
            Task.FromResult(DeleteFeedbackWithHttpInfo(feedbackId));

        public Feedback GetFeedback(string feedbackId) => Copy(FindFeedback(feedbackId));
        public ApiResponse<Feedback> GetFeedbackWithHttpInfo(string feedbackId) =>
            Response(GetFeedback(feedbackId));
        public Task<Feedback> GetFeedbackAsync(string feedbackId, CancellationToken cancellationToken) =>
            Task.FromResult(GetFeedback(feedbackId));
        public Task<ApiResponse<Feedback>> GetFeedbackWithHttpInfoAsync(
            string feedbackId,
            CancellationToken cancellationToken) =>
            Task.FromResult(GetFeedbackWithHttpInfo(feedbackId));

        public void UpdateFeedback(string feedbackId, UpdateFeedbackParam parameter)
        {
            var current = FindFeedback(feedbackId);
            Feedback = new Feedback(
                parameter.FeedbackTitle,
                parameter.FeedbackDescription,
                current.Comments,
                current.Count,
                current.Users,
                current.Id,
                current.UserId,
                current.CreatedAt,
                current.Status);
        }
        public ApiResponse<object> UpdateFeedbackWithHttpInfo(
            string feedbackId,
            UpdateFeedbackParam parameter)
        {
            UpdateFeedback(feedbackId, parameter);
            return EmptyResponse();
        }
        public Task UpdateFeedbackAsync(
            string feedbackId,
            UpdateFeedbackParam parameter,
            CancellationToken cancellationToken)
        {
            UpdateFeedback(feedbackId, parameter);
            return Task.CompletedTask;
        }
        public Task<ApiResponse<object>> UpdateFeedbackWithHttpInfoAsync(
            string feedbackId,
            UpdateFeedbackParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(UpdateFeedbackWithHttpInfo(feedbackId, parameter));

        public void UpdateFeedbackStatus(string feedbackId, UpdateFeedbackStatusParam parameter)
        {
            var current = FindFeedback(feedbackId);
            current.Status = parameter.Status;
        }
        public ApiResponse<object> UpdateFeedbackStatusWithHttpInfo(
            string feedbackId,
            UpdateFeedbackStatusParam parameter)
        {
            UpdateFeedbackStatus(feedbackId, parameter);
            return EmptyResponse();
        }
        public Task UpdateFeedbackStatusAsync(
            string feedbackId,
            UpdateFeedbackStatusParam parameter,
            CancellationToken cancellationToken)
        {
            UpdateFeedbackStatus(feedbackId, parameter);
            return Task.CompletedTask;
        }
        public Task<ApiResponse<object>> UpdateFeedbackStatusWithHttpInfoAsync(
            string feedbackId,
            UpdateFeedbackStatusParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(UpdateFeedbackStatusWithHttpInfo(feedbackId, parameter));

        public Comment CreateFeedbackComment(string feedbackId, CreateFeedbackCommentParam parameter)
        {
            FindFeedback(feedbackId);
            Assert.False(string.IsNullOrWhiteSpace(parameter.Body));
            Comment = new Comment($"comment-{_nextId++}", 1, parameter.Body);
            return Copy(Comment);
        }
        public ApiResponse<Comment> CreateFeedbackCommentWithHttpInfo(
            string feedbackId,
            CreateFeedbackCommentParam parameter) =>
            Response(CreateFeedbackComment(feedbackId, parameter), HttpStatusCode.Created);
        public Task<Comment> CreateFeedbackCommentAsync(
            string feedbackId,
            CreateFeedbackCommentParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(CreateFeedbackComment(feedbackId, parameter));
        public Task<ApiResponse<Comment>> CreateFeedbackCommentWithHttpInfoAsync(
            string feedbackId,
            CreateFeedbackCommentParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(CreateFeedbackCommentWithHttpInfo(feedbackId, parameter));

        public Comment GetFeedbackComment(string feedbackId, string commentId)
        {
            FindFeedback(feedbackId);
            Assert.Equal(Comment?.Id, commentId);
            return Copy(Comment!);
        }
        public ApiResponse<Comment> GetFeedbackCommentWithHttpInfo(string feedbackId, string commentId) =>
            Response(GetFeedbackComment(feedbackId, commentId));
        public Task<Comment> GetFeedbackCommentAsync(
            string feedbackId,
            string commentId,
            CancellationToken cancellationToken) =>
            Task.FromResult(GetFeedbackComment(feedbackId, commentId));
        public Task<ApiResponse<Comment>> GetFeedbackCommentWithHttpInfoAsync(
            string feedbackId,
            string commentId,
            CancellationToken cancellationToken) =>
            Task.FromResult(GetFeedbackCommentWithHttpInfo(feedbackId, commentId));

        public void UpdateFeedbackComment(
            string feedbackId,
            string commentId,
            UpdateFeedbackCommentParam parameter)
        {
            GetFeedbackComment(feedbackId, commentId);
            Comment = new Comment(commentId, 1, parameter.Body);
        }
        public ApiResponse<object> UpdateFeedbackCommentWithHttpInfo(
            string feedbackId,
            string commentId,
            UpdateFeedbackCommentParam parameter)
        {
            UpdateFeedbackComment(feedbackId, commentId, parameter);
            return EmptyResponse();
        }
        public Task UpdateFeedbackCommentAsync(
            string feedbackId,
            string commentId,
            UpdateFeedbackCommentParam parameter,
            CancellationToken cancellationToken)
        {
            UpdateFeedbackComment(feedbackId, commentId, parameter);
            return Task.CompletedTask;
        }
        public Task<ApiResponse<object>> UpdateFeedbackCommentWithHttpInfoAsync(
            string feedbackId,
            string commentId,
            UpdateFeedbackCommentParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(UpdateFeedbackCommentWithHttpInfo(feedbackId, commentId, parameter));

        public Votes CreateVoteUser(string feedbackId, CreateVoteUserParam parameter)
        {
            FindFeedback(feedbackId);
            return new Votes(new List<User> { new(parameter.UserId) }, 1);
        }
        public ApiResponse<Votes> CreateVoteUserWithHttpInfo(
            string feedbackId,
            CreateVoteUserParam parameter) =>
            Response(CreateVoteUser(feedbackId, parameter), HttpStatusCode.Created);
        public Task<Votes> CreateVoteUserAsync(
            string feedbackId,
            CreateVoteUserParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(CreateVoteUser(feedbackId, parameter));
        public Task<ApiResponse<Votes>> CreateVoteUserWithHttpInfoAsync(
            string feedbackId,
            CreateVoteUserParam parameter,
            CancellationToken cancellationToken) =>
            Task.FromResult(CreateVoteUserWithHttpInfo(feedbackId, parameter));

        public void DeleteVoteForFeedback(string feedbackId, string userId) => FindFeedback(feedbackId);
        public ApiResponse<object> DeleteVoteForFeedbackWithHttpInfo(string feedbackId, string userId)
        {
            DeleteVoteForFeedback(feedbackId, userId);
            return EmptyResponse();
        }
        public Task DeleteVoteForFeedbackAsync(
            string feedbackId,
            string userId,
            CancellationToken cancellationToken)
        {
            DeleteVoteForFeedback(feedbackId, userId);
            return Task.CompletedTask;
        }
        public Task<ApiResponse<object>> DeleteVoteForFeedbackWithHttpInfoAsync(
            string feedbackId,
            string userId,
            CancellationToken cancellationToken) =>
            Task.FromResult(DeleteVoteForFeedbackWithHttpInfo(feedbackId, userId));

        public void DeleteFeedbackComment(string feedbackId, string commentId)
        {
            GetFeedbackComment(feedbackId, commentId);
            Comment = null;
        }
        public ApiResponse<object> DeleteFeedbackCommentWithHttpInfo(string feedbackId, string commentId)
        {
            DeleteFeedbackComment(feedbackId, commentId);
            return EmptyResponse();
        }
        public Task DeleteFeedbackCommentAsync(
            string feedbackId,
            string commentId,
            CancellationToken cancellationToken)
        {
            DeleteFeedbackComment(feedbackId, commentId);
            return Task.CompletedTask;
        }
        public Task<ApiResponse<object>> DeleteFeedbackCommentWithHttpInfoAsync(
            string feedbackId,
            string commentId,
            CancellationToken cancellationToken) =>
            Task.FromResult(DeleteFeedbackCommentWithHttpInfo(feedbackId, commentId));

        private Feedback FindFeedback(string feedbackId)
        {
            Assert.NotNull(Feedback);
            Assert.Equal(Feedback!.Id, feedbackId);
            return Feedback;
        }

        private List<Feedback> CopyFeedbacks() =>
            Feedback is null ? new List<Feedback>() : new List<Feedback> { Copy(Feedback) };

        private static Feedback Copy(Feedback feedback) => new(
            feedback.FeedbackTitle,
            feedback.FeedbackDescription,
            feedback.Comments?.Select(Copy).ToList() ?? new List<Comment>(),
            feedback.Count,
            feedback.Users?.Select(user => new User(user.UserId)).ToList() ?? new List<User>(),
            feedback.Id,
            feedback.UserId,
            feedback.CreatedAt,
            feedback.Status);

        private static Comment Copy(Comment comment) => new(comment.Id, comment.CreatedAt, comment.Body);

        private static ApiResponse<T> Response<T>(
            T data,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var headers = new Multimap<string, string>();
            headers.Add("Content-Type", "application/json");
            return new(statusCode, headers, data, JsonConvert.SerializeObject(data));
        }

        private static ApiResponse<object> EmptyResponse() => Response<object>(null!);
    }
}
