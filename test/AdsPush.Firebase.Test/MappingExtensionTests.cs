using System.Net;
using System.Reflection;
using AdsPush.Abstraction;
using AdsPush.Firebase.Extensions;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;

namespace AdsPush.Firebase.Test;

public class MappingExtensionTests
{
    [Fact]
    public void CreateFirebaseMessageForIosMapsPayload()
    {
        var payload = new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("Title"),
            Detail = AdsPushText.CreateUsingString("Body"),
            PushType = AdsPushType.Alert,
            Badge = 3,
            Sound = "default",
            GroupId = "group",
            Ttl = TimeSpan.FromMinutes(2)
        };
        payload.Parameters["traceId"] = "abc";

        var message = payload.CreateFirebaseMessage(AdsPushTarget.Ios, "token");

        Assert.Equal("token", message.Token);
        Assert.Equal("abc", message.Data["traceId"]);
        Assert.Equal("Alert", message.Apns.Headers["apns-push-type"]);
        Assert.True(message.Apns.Aps.MutableContent);
        Assert.True(message.Apns.Aps.ContentAvailable);
        Assert.Equal("group", message.Apns.Aps.ThreadId);
        Assert.Equal("Title", message.Apns.Aps.Alert.Title);
        Assert.Equal("Body", message.Apns.Aps.Alert.Body);
        Assert.Contains("apns-expiration", message.Apns.Headers.Keys);
    }

    [Fact]
    public void CreateFirebaseMessageForAndroidMapsPayload()
    {
        var payload = new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("Title"),
            Detail = AdsPushText.CreateUsingString("Body"),
            PushType = AdsPushType.Alert,
            Badge = 4,
            Sound = "chime",
            GroupId = "collapse",
            Ttl = TimeSpan.FromSeconds(30)
        };
        payload.Parameters["flag"] = true;

        var message = payload.CreateFirebaseMessage(AdsPushTarget.Android, "token");

        Assert.Equal("token", message.Token);
        Assert.Equal("collapse", message.Android.CollapseKey);
        Assert.Equal("Title", message.Android.Notification.Title);
        Assert.Equal("Body", message.Android.Notification.Body);
        Assert.Equal("chime", message.Android.Notification.Sound);
        Assert.Equal(4, message.Android.Notification.NotificationCount);
        Assert.Equal(TimeSpan.FromSeconds(30), message.Android.TimeToLive);
        Assert.Equal("True", message.Android.Data["flag"]);
    }

    [Fact]
    public void CreateFirebaseMessageWithUnknownTargetThrows()
    {
        var payload = new AdsPushBasicSendPayload
        {
            Title = AdsPushText.CreateUsingString("Title"), Detail = AdsPushText.CreateUsingString("Body")
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => payload.CreateFirebaseMessage((AdsPushTarget)999, "token"));
    }

    [Fact]
    public void CreateFirebaseMessageInitializesParametersWhenNull()
    {
        var payload = new AdsPushBasicSendPayload
        {
            Parameters = null,
            Title = AdsPushText.CreateUsingString("Title"),
            Detail = AdsPushText.CreateUsingString("Body")
        };

        var message = payload.CreateFirebaseMessage(AdsPushTarget.Ios, "token");

        Assert.NotNull(message.Data);
    }

    public static TheoryData<MessagingErrorCode?, AdsPushErrorType> ErrorMappings => new()
    {
        { MessagingErrorCode.ThirdPartyAuthError, AdsPushErrorType.InvalidAuthConfiguration },
        { MessagingErrorCode.InvalidArgument, AdsPushErrorType.InvalidArgument },
        { MessagingErrorCode.Internal, AdsPushErrorType.ServiceUnavailable },
        { MessagingErrorCode.QuotaExceeded, AdsPushErrorType.Unknown },
        { MessagingErrorCode.SenderIdMismatch, AdsPushErrorType.InvalidAuthConfiguration },
        { MessagingErrorCode.Unavailable, AdsPushErrorType.ServiceUnavailable },
        { MessagingErrorCode.Unregistered, AdsPushErrorType.InvalidToken },
        { null, AdsPushErrorType.Unknown }
    };

    [Theory]
    [MemberData(nameof(ErrorMappings))]
    public void CreateExceptionMapsMessagingErrors(
        MessagingErrorCode? messagingError,
        AdsPushErrorType expectedErrorType)
    {
        using var response = new HttpResponseMessage(HttpStatusCode.BadRequest);
        var ctor = typeof(FirebaseMessagingException).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            binder: null,
            types: new[]
            {
                typeof(ErrorCode), typeof(string), typeof(MessagingErrorCode?), typeof(Exception),
                typeof(HttpResponseMessage)
            },
            modifiers: null)!;

#pragma warning disable CS8601
        var exception = (FirebaseMessagingException)ctor.Invoke(new object[]
        {
            ErrorCode.Internal, "error", messagingError, null!, response
        });
#pragma warning restore CS8601

        var adsPushException = exception.CreateException();

        Assert.Equal(expectedErrorType, adsPushException.ErrorType);
        Assert.Same(response, adsPushException.HttpResponse!);
    }

    [Fact]
    public void CreateExceptionMapsAuthExceptionToInvalidAuthConfiguration()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
        var ctor = typeof(FirebaseAuthException).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            binder: null,
            types: new[]
            {
                typeof(ErrorCode), typeof(string), typeof(AuthErrorCode?), typeof(Exception),
                typeof(HttpResponseMessage)
            },
            modifiers: null)!;

        var exception = (FirebaseAuthException)ctor.Invoke(new object[]
        {
            ErrorCode.Internal, "auth", AuthErrorCode.EmailAlreadyExists, null!, response
        });

        var adsPushException = exception.CreateException();

        Assert.Equal(AdsPushErrorType.InvalidAuthConfiguration, adsPushException.ErrorType);
        Assert.Same(response, adsPushException.HttpResponse!);
    }

    [Fact]
    public void CreateExceptionWithUnknownFirebaseExceptionDefaultsToUnknownError()
    {
        using var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        var ctor = typeof(FirebaseException).GetConstructor(
            BindingFlags.NonPublic | BindingFlags.Instance,
            binder: null,
            types: new[] { typeof(ErrorCode), typeof(string), typeof(Exception), typeof(HttpResponseMessage) },
            modifiers: null)!;

        var exception = (FirebaseException)ctor.Invoke(new object[] { ErrorCode.Internal, "general", null!, response });

        var adsPushException = exception.CreateException();

        Assert.Equal(AdsPushErrorType.Unknown, adsPushException.ErrorType);
        Assert.Same(response, adsPushException.HttpResponse!);
    }
}
