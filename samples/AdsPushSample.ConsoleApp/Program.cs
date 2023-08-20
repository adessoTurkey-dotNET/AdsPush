// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using AdsPush;
using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;
using AdsPush.Abstraction.Settings;
using AdsPush.Abstraction.Vapid;
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


var vapidSettings = new AdsPushVapidSettings()
{
    //put your configurations hare.
};

var sender = builder
    .ConfigureVapid(vapidSettings)
    .ConfigureApns(apnsSettings)
    .ConfigureFirebase(firebaseSettings, AdsPushTarget.Android)
    .BuildSender();


var basicPayload = new AdsPushBasicSendPayload()
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
};

var apnDeviceToken = "15f6fdd0f34a7e0f46301a817536f0fb1b2ab05b09b3fae02beba2854a1a2a16";
//var apnDeviceTokenVapid = "{"endpoint:"...", "keys": {"auth":"...","p256dh":"..."}}";
await sender.BasicSendAsync(
    AdsPushTarget.Ios,
    apnDeviceToken,
    basicPayload);

//For VAPID WebPush
string
    endpoint = "https://fcm.googleapis.com/fcm/send/cIo6QJ4MMtQ:APA91bEGHCpZdHaUS7otb5_xU1zNWe6TAqria9phFm7M_9ZIiEyr0vXj3gRHbeIJMYvp2-SAVbgNrVvl7uBvU_VTLpIA0CLBcmqXuuEktGr0U4LVLvwWBibO68spJk7D-lr8R9zPyAXE",
    p256dh = "BIjydse4Rij892SJN10xx1qbxDM6GrYXSfg7TGu90CVM1WmlTYzn_79psRqseyWdER969LGLjZmnXIhHPaKTyGE",
    auth = "TkLGLzFeUU3C9SJJN6dLAA";

var subscription = VapidSubscription.FromParameters(endpoint, p256dh, auth);
await sender.BasicSendAsync(
    AdsPushTarget.BrowserAndPwa,
    subscription.ToAdsPushToken(),
    basicPayload);


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

//Sample for VAPID WebPush
var vapidResult = await sender
    .GetVapidSender()
    .SendAsync(
        subscription,
        new VapidRequest()
        {
            Title = "",
            Badge = "",
            Message = "",
            Sound = "",
            Icon = "",
            Image = "",
            Language = "",
            Silent = false,
            Tag = "",
            ClickAction = "",
            VibratePattern = "",
            Data = new Dictionary<string, string>()
            {
                {
                    "param1", "value1"
                }
            }
        });
