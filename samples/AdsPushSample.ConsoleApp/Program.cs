// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using AdsPush;
using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;
using AdsPush.Abstraction.Settings;
using AdsPush.Vapid;
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


var publicKey = "BF59A9jkMtVqs0Gzef1o6xhcB8SBHjhufCLikJhtNY9YGl_Zm2PwLMYbQs_RvD3T0yUFUlcFBt6nqSVOdoU05IM";
var privateKey = "jYJABdhwbgAOiQkz97LK39FjA5YF4WXPxcgDX7bdRcQ";
var subject = @"mailto:example@example.com";
var vapidSettings = new AdsPushVapidSettings()
{
    //put your configurations hare.
    PublicKey = publicKey,
    PrivateKey = privateKey,
    Subject = subject
};

var sender = builder
    .ConfigureVapid(vapidSettings, null)
    .ConfigureApns(apnsSettings, null)
    .ConfigureFirebase(firebaseSettings, AdsPushTarget.Android)
    .BuildSender();


// string
//     endpoint = "https://fcm.googleapis.com/fcm/send/cIo6QJ4MMtQ:APA91bEGHCpZdHaUS7otb5_xU1zNWe6TAqria9phFm7M_9ZIiEyr0vXj3gRHbeIJMYvp2-SAVbgNrVvl7uBvU_VTLpIA0CLBcmqXuuEktGr0U4LVLvwWBibO68spJk7D-lr8R9zPyAXE",
//     p256dh = "BIjydse4Rij892SJN10xx1qbxDM6GrYXSfg7TGu90CVM1WmlTYzn_79psRqseyWdER969LGLjZmnXIhHPaKTyGE",
//     auth = "TkLGLzFeUU3C9SJJN6dLAA";



//Safari mobile
string
    endpoint = "",
    p256dh = "",
    auth = "";


var subs = VapidSubscription.FromParameters(endpoint, p256dh, auth);
await sender.BasicSendAsync(
    AdsPushTarget.BrowserAndPwa,
    subs.ToAdsPushToken(),
    new AdsPushBasicSendPayload()
    {
        Title = AdsPushText.CreateUsingString("Title"),
        Detail = AdsPushText.CreateUsingString("Detail"),
        
    });

var apnDeviceToken = "15f6fdd0f34a7e0f46301a817536f0fb1b2ab05b09b3fae02beba2854a1a2a16";

await sender.BasicSendAsync(
    AdsPushTarget.Ios,
    apnDeviceToken,
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
