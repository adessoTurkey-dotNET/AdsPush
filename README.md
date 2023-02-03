# AdsPush
AdsPush server-side library for sending notification by connecting to APNS & FCM.

## Get it started 

### Configuration
You have two easy options to be able configure AdsPush

1. Using Microsoft Dependency Injection (recommended)
2. Using direct sender instance.

#### Microsoft Dependency Injection

Microsoft Dependency Injection is Microsoft's IOC library coming with .NET Core. AdsPush supports using MDI to be able to manage your push configuration and sending operations.

If you're sing .NET 6 or newer version in `Program.cs`

```csharp
using AdsPush.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAdsPush(builder.Configuration);
```
If you're sing .NET 5 or any .NET Core version in `Startup.cs`

```csharp
using AdsPush.Extensions;
...

 public override void ConfigureServices(IServiceCollection services)
 {
    //your code...
    services.AddAdsPush(this.Congiguration);
}   
```

And put the following section in your in your `appsettings.[ENV].json`

```json
{
  "Logging": {
  ...
  },
  "AdsPush": {
    "MyApp": { 
      "TargetMappings": {
        "Ios": "Apns",
        "Android": "FirebaseCloudMessaging"
      },
      "Apns": {
        "P8PrivateKey": "<p8 certificate string without any space and start and end tags>",
        "P8PrivateKeyId": "<10 digit p8 certificate id. Usually a part of a downloadable certificate filename>",
        "TeamId": "<Apple 10 digit team id shown in Apple Developer Membership Page>",
        "AppBundleIdentifier": "<App slug / bundle name>",
        "EnvironmentType": "<Apns Env one of Development or Production>"
      },
      "FirebaseCloudMessaging": {
        "Type":"<type filed in service_account.json>",
        "ProjectId":"<project_id filed in service_account.json>",
        "PrivateKey": "<private_key filed in service_account.json>",
        "PrivateKeyId": "<private_key_id filed in service_account.json>",
        "ClientId": "<client_id filed in service_account.json>",
        "ClientEmail": "<client_email filed in service_account.json>",
        "AuthUri": "<auth_uri filed in service_account.json>",
        "AuthProviderX509CertUrl": "<auth_provider_x509_cert_url filed in service_account.json>",
        "TokenUri": "<client_x509_cert_url filed in service_account.json>",
        "ClientX509CertUrl": "<token_uri filed in service_account.json>"
      }
    }
  }
 ...
}
```
If you wish to use host/pod environment or any secret provider you can set the following environment variables.

```config
AdsPush__MyApp__Apns__AppBundleIdentifier=<App slug / bundle name>
AdsPush__MyApp__Apns__EnvironmentType=<Apns Env one of Development or Production>
AdsPush__MyApp__Apns__P8PrivateKey=<p8 certificate string without any space and start and end tags>
AdsPush__MyApp__Apns__P8PrivateKeyId=<10 digit p8 certificate id. Usually a part of a downloadable certificate filename>
AdsPush__MyApp__Apns__TeamId=<Apple 10 digit team id shown in Apple Developer Membership Page>
AdsPush__MyApp__FirebaseCloudMessaging__AuthProviderX509CertUrl=<auth_provider_x509_cert_url filed in service_account.json>
AdsPush__MyApp__FirebaseCloudMessaging__AuthUri=<auth_uri filed in service_account.json>
AdsPush__MyApp__FirebaseCloudMessaging__ClientEmail=<client_email filed in service_account.json>
AdsPush__MyApp__FirebaseCloudMessaging__ClientId=<client_id filed in service_account.json>
AdsPush__MyApp__FirebaseCloudMessaging__ClientX509CertUrl=<token_uri filed in service_account.json>
AdsPush__MyApp__FirebaseCloudMessaging__PrivateKey=<private_key filed in service_account.json>
AdsPush__MyApp__FirebaseCloudMessaging__PrivateKeyId=<private_key_id filed in service_account.json>
AdsPush__MyApp__FirebaseCloudMessaging__ProjectId=<project_id filed in service_account.json>
AdsPush__MyApp__FirebaseCloudMessaging__TokenUri=<client_x509_cert_url filed in service_account.json>
AdsPush__MyApp__FirebaseCloudMessaging__Type=<type filed in service_account.json>
AdsPush__MyApp__TargetMappings__Android=FirebaseCloudMessaging
AdsPush__MyApp__TargetMappings__Ios=Apns

```

Now, you can easily use wia DI as the. following.

```csharp
private readonly IAdsPushSender _pushSender;
public MyService(
    IAdsPushSenderFactory adsPushSenderFactory)
{
    this._pushSender = adsPushSenderFactory.GetSender("MyApp");
}

```

#### Using Sender Instance

The following lines of codes can be used without any DI configuration.

```csharp

using AdsPush;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Settings;

var builder = new AdsPushSenderBuilder();
var apnsSettings = new AdsPushAPNSSettings()
{
    //put your configurations hare.
};

var firebaseSettings = new AdsPushFirebaseSettings()
{
    //put your configurations hare.
};

var sender = builder
    .ConfigureApns(apnsSettings, null)
    .ConfigureFirebase(firebaseSettings, AdsPushTarget.Android)
    .BuildSender();
  
```

### Sending notifications

When you obtain `IAdsPushSender` instance by using one the methods shown above, you're ready to send notification. The following sample code can be used trigger a basic notification request.

```csharp

await sender.BasicSendAsync(
    AdsPushTarget.Ios,
    "79eb1b9e623bbca0d2b218f44a18d7b8ef59dac4da5baa9949c3e99a48eb259a",
    new ()
    {
        Title = AdsPushText.CreateUsingString("test"),
        Detail = AdsPushText.CreateUsingString("detail"),
        Badge = 52,
        Sound = "default",
        Parameters = new Dictionary<string, object>()
        {
            {"pushParam1","value1"},
            {"pushParam2","value2"},
        }
    });

```

If you wish to access whole supported parameters of the related platform, the following methods can be helpful. 

```csharp


//sample for Apns
var apnsResult = await sender
    .GetApnsSender()
    .SendAsync(
        new APNSRequest()
        {
            ApnsPayload = new()
            {
                Badge = 52,
                Sound = "",
                MutableContent = true,
                FilterCriteria = "",
                ThreadId = "",
                TargetContentId = "",
                Alert = new APNSAlert()
                {
                    Title = "",
                    Body = "",
                    Subtitle = ""
                }
                //more...
            },
            AdditionalParameters = new Dictionary<string, object>()
            {
                {"pushParam1", "value1" },
                {"pushParam2", "value2"},
                {"pushParam3", 52},
            }
        },
        "79eb1b9e623bbca0d2b218f44a18d7b8ef59dac4da5baa9949c3e99a48eb259a",
        Guid.NewGuid());

//sample for FCM
var firebaseResult = await sender
    .GetFirebaseSender()
    .SendToSingleAsync(new Message()
    {
        Token = "",
        Android = new AndroidConfig()
        {
            Priority = Priority.High,
        },
        Notification = new()
        {
            Title = "",
            Body = "",
            ImageUrl = ""
        }
    });
```