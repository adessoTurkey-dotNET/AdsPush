// See https://aka.ms/new-console-template for more information

using AdsPush;
using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;
using AdsPush.Abstraction.Settings;
using FirebaseAdmin.Messaging;

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

await sender.BasicSendAsync(
    AdsPushTarget.Ios,
    "79eb1b9e623bbca0d2b218f44a18d7b8ef59dac4da5baa9949c3e99a48eb259a",
    new()
    {
        Title = AdsPushText.CreateUsingString("test"),
        Detail = AdsPushText.CreateUsingString("detail"),
        Badge = 52,
        Sound = "default",
        Parameters = new Dictionary<string, object>()
        {
            {
                "pushParam1", "value1"
            },
            {
                "pushParam2", "value2"
            },
        }
    });

//for whole platform options
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
                {
                    "pushParam1", "value1"
                },
                {
                    "pushParam2", "value2"
                },
                {
                    "pushParam3", 52
                },
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