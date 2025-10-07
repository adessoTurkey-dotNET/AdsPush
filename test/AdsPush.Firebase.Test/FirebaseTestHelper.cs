using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using AdsPush.Abstraction.Settings;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Http;
using Newtonsoft.Json;

namespace AdsPush.Firebase.Test;

internal static class FirebaseTestHelper
{
    internal static AdsPushFirebaseSettings CreateSettings(
        string? projectId = null)
    {
        using var rsa = RSA.Create(2048);
        var pkcs8 = rsa.ExportPkcs8PrivateKey();
        var base64 = Convert.ToBase64String(pkcs8);
        var formattedKey = FormatPem(base64);

        projectId ??= $"test-project-{Guid.NewGuid():N}";

        return new AdsPushFirebaseSettings
        {
            Type = "service_account",
            ProjectId = projectId,
            PrivateKeyId = Guid.NewGuid().ToString("N"),
            PrivateKey = formattedKey,
            ClientEmail = $"firebase-adminsdk@{projectId}.iam.gserviceaccount.com",
            ClientId = new Random().NextInt64(1_000_000_000_000, 9_999_999_999_999).ToString(),
            AuthUri = "https://accounts.google.com/o/oauth2/auth",
            TokenUri = "https://oauth2.googleapis.com/token",
            AuthProviderX509CertUrl = "https://www.googleapis.com/oauth2/v1/certs",
            ClientX509CertUrl =
                $"https://www.googleapis.com/robot/v1/metadata/x509/firebase-adminsdk%40{projectId}.iam.gserviceaccount.com"
        };
    }

    internal static string CreateServiceAccountJson(
        AdsPushFirebaseSettings settings)
    {
        var payload = new
        {
            type = settings.Type,
            project_id = settings.ProjectId,
            private_key_id = settings.PrivateKeyId,
            private_key = settings.PrivateKey,
            client_email = settings.ClientEmail,
            client_id = settings.ClientId,
            auth_uri = settings.AuthUri,
            token_uri = settings.TokenUri,
            auth_provider_x509_cert_url = settings.AuthProviderX509CertUrl,
            client_x509_cert_url = settings.ClientX509CertUrl
        };

        return JsonConvert.SerializeObject(payload);
    }

    internal static Stream CreateServiceAccountStream(
        AdsPushFirebaseSettings settings)
    {
        var json = CreateServiceAccountJson(settings);
        return new MemoryStream(Encoding.UTF8.GetBytes(json));
    }

    internal static void DeleteAppIfExists(
        string appName)
    {
        try
        {
            FirebaseApp.GetInstance(appName).Delete();
        }
        catch (ArgumentException)
        {
        }
    }

    internal static void DeleteDefaultApp()
    {
        try
        {
            FirebaseApp.DefaultInstance?.Delete();
        }
        catch (InvalidOperationException)
        {
        }
    }

    internal static (FirebaseMessaging Messaging, FirebaseApp App) CreateMessaging(
        Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responder,
        string projectId)
    {
        var appName = $"app-{Guid.NewGuid():N}";
        var app = FirebaseApp.Create(
            new AppOptions { ProjectId = projectId, Credential = GoogleCredential.FromAccessToken("ya29.test-token") },
            appName);

        var messaging = (FirebaseMessaging)Activator.CreateInstance(
            typeof(FirebaseMessaging),
            BindingFlags.NonPublic | BindingFlags.Instance,
            binder: null,
            args: new object[] { app },
            culture: null)!;
        var clientField =
            typeof(FirebaseMessaging).GetField("messagingClient", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var clientType = clientField.FieldType;

        var handler = new DelegateHandler(responder);
        var factory = new StubHttpClientFactory(handler);

        var retryOptionsType = Type.GetType("FirebaseAdmin.Util.RetryOptions, FirebaseAdmin")!;
        var retryOptionsValue = retryOptionsType.GetProperty("NoBackOff",
                BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic)!
            .GetValue(null);

        var argsType = clientType.GetNestedType("Args", BindingFlags.NonPublic)!;
        var args = Activator.CreateInstance(argsType)!;
        argsType.GetProperty("ClientFactory", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(args, factory);
        argsType.GetProperty("Credential", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(args, app.Options.Credential);
        argsType.GetProperty("ProjectId", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(args, projectId);
        argsType.GetProperty("RetryOptions", BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic)!
            .SetValue(args, retryOptionsValue);

        var messagingClient = Activator.CreateInstance(
            clientType,
            BindingFlags.NonPublic | BindingFlags.Instance,
            binder: null,
            args: new[] { args },
            culture: null)!;

        clientField.SetValue(messaging, messagingClient);

        return (messaging, app);
    }

    private static string FormatPem(
        string base64)
    {
        var builder = new StringBuilder();
        builder.AppendLine("-----BEGIN PRIVATE KEY-----");
        for (var i = 0; i < base64.Length; i += 64)
        {
            var segment = base64.Substring(i, Math.Min(64, base64.Length - i));
            builder.AppendLine(segment);
        }

        builder.AppendLine("-----END PRIVATE KEY-----");
        return builder.ToString();
    }

    private sealed class StubHttpClientFactory : HttpClientFactory
    {
        private readonly HttpMessageHandler _handler;

        public StubHttpClientFactory(
            HttpMessageHandler handler)
        {
            this._handler = handler;
        }

        protected override HttpMessageHandler CreateHandler(
            CreateHttpClientArgs args)
        {
            return this._handler;
        }
    }

    private sealed class DelegateHandler : HttpMessageHandler
    {
        private readonly Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> _responder;

        public DelegateHandler(
            Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> responder)
        {
            this._responder = responder;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            return this._responder(request, cancellationToken);
        }
    }
}
