// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using AdsPush;
using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;
using AdsPush.Abstraction.HMS;
using AdsPush.Abstraction.HMS.Android;
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

var hmsSettings = new AdsPushHMSSettings()
{
    //put your configurations hare.
    ApiBaseUrl = "https://push-api.cloud.huawei.com",
    IdentityUrl = "https://oauth-login.cloud.huawei.com/oauth2",
    ClientId = "110262941",
    ClientSecret = "7b88e9ac7b97a8bdb1beb8a5049ab0654ec0bdb7d8d1578e5aeb93521dec70af"
};

var sender = builder
    .ConfigureVapid(vapidSettings)
    .ConfigureApns(apnsSettings)
    .ConfigureFirebase(firebaseSettings, AdsPushTarget.Android)
    .ConfigureHMS(hmsSettings, AdsPushTarget.HarmonyOS)
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
    //AdsPushTarget.Ios,
    AdsPushTarget.HarmonyOS,
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
        Android = new FirebaseAdmin.Messaging.AndroidConfig()
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



//sample for HMS
var hmsResult = await sender
    .GetHMSSender()
    .SendAsync(new HMSRequest
    {
        HMSPayload = new()
        {
            PushToken = "hms push token",
            Notification = new()
            {
                Title = "",
                Body = "",
                Image = ""
            },
            Android = new()
            {
                CollapseKey = 1,
                TimeToLive = new TimeSpan(25412),
                Notification = new()
                {
                    Color = "red",
                    Sound = "",
                    ClickAction = new ClickAction
                    {
                        Type = 2,
                        Url = "",
                        Action = ""
                    },
                    UseDefaultLight = true,
                    UseDefaultVibrate = true,
                    TitleLocArgs =
                    {

                    },
                    Buttons = new List<Button>{
                        new Button
                        {
                            ActionType = 3,
                            Data = ""
                        }
                    }
                }
            }
        }
    },
    "deviceToken",
    Guid.NewGuid());
