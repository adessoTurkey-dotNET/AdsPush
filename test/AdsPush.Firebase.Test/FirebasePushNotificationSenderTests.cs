using System.Net;
using System.Reflection;
using System.Text;
using AdsPush.Abstraction;
using FirebaseAdmin.Messaging;

namespace AdsPush.Firebase.Test;

public class FirebasePushNotificationSenderTests
{
    [Fact]
    public async Task SendToSingleAsyncReturnsSuccess()
    {
        var projectId = $"proj-{Guid.NewGuid():N}";
        var responses = new Queue<HttpResponseMessage>();
        responses.Enqueue(CreateSuccessResponse("projects/test/messages/123"));
        var requests = new List<CapturedRequest>();

        var (messaging, app) = FirebaseTestHelper.CreateMessaging(async (request, token) =>
        {
            requests.Add(await CapturedRequest.FromAsync(request));
            return responses.Dequeue();
        }, projectId);

        var sender = new FirebasePushNotificationSender(messaging);
        var result = await sender.SendToSingleAsync(new Message { Token = "device-token" });

        Assert.True(result.IsSuccess);
        Assert.Equal("projects/test/messages/123", result.MessageId);
        Assert.Single(requests);
        Assert.EndsWith($"projects/{projectId}/messages:send", requests[0].RequestUri, StringComparison.Ordinal);

        app.Delete();
    }

    [Fact]
    public void ConstructorWithMessagingThrowsWhenNull()
    {
        Assert.Throws<ArgumentNullException>(() => new FirebasePushNotificationSender((FirebaseMessaging)null!));
    }

    [Fact]
    public async Task SendToSingleAsyncReturnsFailedResultOnFirebaseError()
    {
        var projectId = $"proj-{Guid.NewGuid():N}";
        var responses = new Queue<HttpResponseMessage>();
        responses.Enqueue(CreateErrorResponse("INVALID_ARGUMENT"));

        var (messaging, app) = FirebaseTestHelper.CreateMessaging((request, token) =>
        {
            return Task.FromResult(responses.Dequeue());
        }, projectId);

        var sender = new FirebasePushNotificationSender(messaging);
        var result = await sender.SendToSingleAsync(new Message { Token = "device-token" });

        Assert.False(result.IsSuccess);
        Assert.NotNull(result.MessagingException);
        Assert.Equal(MessagingErrorCode.InvalidArgument, result.MessagingException!.MessagingErrorCode);

        app.Delete();
    }

    [Fact]
    public async Task SendAsyncWithBasicPayloadThrowsAdsPushException()
    {
        var projectId = $"proj-{Guid.NewGuid():N}";
        var responses = new Queue<HttpResponseMessage>();
        responses.Enqueue(CreateErrorResponse("UNREGISTERED"));

        var (messaging, app) =
            FirebaseTestHelper.CreateMessaging((request, token) => Task.FromResult(responses.Dequeue()), projectId);
        var sender = new FirebasePushNotificationSender(messaging);

        var payload = new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("title"),
            Detail = AdsPushText.CreateUsingString("detail"),
            PushType = AdsPushType.Alert
        };

        await Assert.ThrowsAsync<AdsPushException>(() =>
            sender.SendAsync(AdsPushTarget.Android, "device-token", payload));

        app.Delete();
    }

    [Fact]
    public async Task SendAsyncWithBasicPayloadSucceeds()
    {
        var projectId = $"proj-{Guid.NewGuid():N}";
        var responses = new Queue<HttpResponseMessage>();
        responses.Enqueue(CreateSuccessResponse("projects/test/messages/456"));

        var (messaging, app) = FirebaseTestHelper.CreateMessaging(
            (request, token) => Task.FromResult(responses.Count > 0
                ? responses.Dequeue()
                : CreateSuccessResponse("projects/test/messages/fallback")), projectId);
        var sender = new FirebasePushNotificationSender(messaging);

        var payload = new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("title"),
            Detail = AdsPushText.CreateUsingString("detail"),
            PushType = AdsPushType.Alert
        };

        await sender.SendAsync(AdsPushTarget.Android, "device-token", payload);

        app.Delete();
    }

    [Fact]
    public async Task SendToMultiDeviceAsyncReturnsBatchResult()
    {
        var projectId = $"proj-{Guid.NewGuid():N}";
        var responses = new Queue<HttpResponseMessage>();
        responses.Enqueue(CreateSuccessResponse("projects/test/messages/1"));
        responses.Enqueue(CreateSuccessResponse("projects/test/messages/2"));

        var (messaging, app) = FirebaseTestHelper.CreateMessaging(
            (request, token) => Task.FromResult(responses.Count > 0
                ? responses.Dequeue()
                : CreateSuccessResponse("projects/test/messages/fallback")), projectId);
        var sender = new FirebasePushNotificationSender(messaging);

        var multicast = new MulticastMessage { Tokens = new List<string> { "tokenA", "tokenB" } };

        var result = await sender.SendToMultiDeviceAsync(multicast);

        Assert.NotNull(result);

        app.Delete();
    }

    [Fact]
    public async Task SendBatchNotificationAsyncReturnsBatchResult()
    {
        var projectId = $"proj-{Guid.NewGuid():N}";
        var responses = new Queue<HttpResponseMessage>();
        responses.Enqueue(CreateSuccessResponse("projects/test/messages/1"));
        responses.Enqueue(CreateSuccessResponse("projects/test/messages/2"));

        var (messaging, app) = FirebaseTestHelper.CreateMessaging(
            (request, token) => Task.FromResult(responses.Count > 0
                ? responses.Dequeue()
                : CreateSuccessResponse("projects/test/messages/fallback")), projectId);
        var sender = new FirebasePushNotificationSender(messaging);

        var batch = await sender.SendBatchNotificationAsync(new[]
        {
            new Message { Token = "token1" }, new Message { Token = "token2" }
        });

        Assert.NotNull(batch);

        app.Delete();
    }

    [Fact]
    public void ConstructorWithSettingsCreatesMessagingInstance()
    {
        var settings = FirebaseTestHelper.CreateSettings();
        var sender = new FirebasePushNotificationSender(settings);

        var field = typeof(FirebasePushNotificationSender).GetField("_firebaseMessaging",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.NotNull(field.GetValue(sender));

        FirebaseTestHelper.DeleteAppIfExists(settings.ProjectId);
    }

    [Fact]
    public void ConstructorWithServiceAccountJsonCreatesDefaultApp()
    {
        FirebaseTestHelper.DeleteDefaultApp();
        var settings = FirebaseTestHelper.CreateSettings();
        var json = FirebaseTestHelper.CreateServiceAccountJson(settings);

        var sender = new FirebasePushNotificationSender(json);

        var field = typeof(FirebasePushNotificationSender).GetField("_firebaseMessaging",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.NotNull(field.GetValue(sender));

        FirebaseTestHelper.DeleteDefaultApp();
    }

    [Fact]
    public void ConstructorWithStreamCreatesDefaultApp()
    {
        FirebaseTestHelper.DeleteDefaultApp();
        var settings = FirebaseTestHelper.CreateSettings();
        using var stream = FirebaseTestHelper.CreateServiceAccountStream(settings);

        var sender = new FirebasePushNotificationSender(stream);

        var field = typeof(FirebasePushNotificationSender).GetField("_firebaseMessaging",
            BindingFlags.NonPublic | BindingFlags.Instance)!;
        Assert.NotNull(field.GetValue(sender));

        FirebaseTestHelper.DeleteDefaultApp();
    }

    private static HttpResponseMessage CreateSuccessResponse(
        string name)
    {
        return new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent($"{{\"name\":\"{name}\"}}", Encoding.UTF8, "application/json")
        };
    }

    private static HttpResponseMessage CreateErrorResponse(
        string status)
    {
        var json =
            $"{{\"error\":{{\"code\":400,\"message\":\"error\",\"status\":\"{status}\",\"details\":[{{\"@type\":\"type.googleapis.com/google.firebase.fcm.v1.FcmError\",\"errorCode\":\"{status}\"}}]}}}}";
        return new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json")
        };
    }

    private sealed record CapturedRequest(string Method, string RequestUri, string? Content)
    {
        public static async Task<CapturedRequest> FromAsync(HttpRequestMessage request)
        {
            var body = request.Content is null ? null : await request.Content.ReadAsStringAsync();
            return new CapturedRequest(request.Method.Method, request.RequestUri!.ToString(), body);
        }
    }
}
