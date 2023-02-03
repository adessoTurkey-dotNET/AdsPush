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
builder.Services.AddAdsPush();
```
If you're sing .NET 5 or any .NET Core version in `Startup.cs`

```csharp
using AdsPush.Extensions;
...

 public override void ConfigureServices(IServiceCollection services)
 {
    //your code...
    services.AddAdsPush();
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


Now, you can easily use wia DI as the. following.

```csharp
private readonly IAdsPushSender _pushSender;
public MyService(
    IAdsPushSenderFactory adsPushSenderFactory)
{
    this._pushSender = adsPushSenderFactory.GetSender("MyApp");
}

```

