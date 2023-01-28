using System;
using System.Collections.Generic;
using System.Linq;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using FirebaseAdmin.Messaging;
using AdsPush.Abstraction;

namespace AdsPush.Firebase.Extensions
{
    public static class MappingExtension
    {
        public static Message CreateFirebaseMessage(
            this AdsPushPayload payload,
            AdsPushTarget target,
            string deviceToken)
        {
            payload.Parameters ??= new Dictionary<string, object>();

            var message = new Message()
            {
                Token = deviceToken,
                Data = payload.Parameters.ToDictionary(x => x.Key, x => x.Value.ToString()),
            };

            switch (target)
            {
                case AdsPushTarget.Ios:
                    message.Apns = new ApnsConfig()
                    {
                        Aps = new Aps()
                        {
                            Badge = payload.Badge,
                            Sound = payload.Sound,
                            ThreadId = payload.GroupId,
                            MutableContent = true,
                            ContentAvailable = true,
                            CustomData = payload.Parameters,
                            Alert = new ApsAlert()
                            {
                                Title = payload.Title.Text,
                                TitleLocKey = payload.Title.LocalizationKey,
                                TitleLocArgs = payload.Title.LocalizationArgs,
                                Body = payload.Detail.Text,
                                LocKey = payload.Detail.LocalizationKey,
                                LocArgs = payload.Detail.LocalizationArgs,
                            }
                        },
                        Headers = new Dictionary<string, string>()
                        {
                            {
                                "apns-push-type", payload.PushType.ToString()
                            }
                        },
                    };

                    break;
                case AdsPushTarget.Android:
                    message.Android = new AndroidConfig()
                    {
                        CollapseKey = payload.GroupId,
                        Notification = new AndroidNotification()
                        {
                            Title = payload.Title.Text,
                            TitleLocKey = payload.Title.LocalizationKey,
                            TitleLocArgs = payload.Title.LocalizationArgs,
                            Body = payload.Detail.Text,
                            BodyLocKey = payload.Detail.LocalizationKey,
                            BodyLocArgs = payload.Detail.LocalizationArgs,
                            DefaultSound = string.IsNullOrEmpty(payload.Sound),
                            NotificationCount = payload.Badge,
                            Sound = payload.Sound,
                        },
                        Data = payload.Parameters.ToDictionary(x => x.Key, x => x.Value.ToString()),
                    };

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(target), target, null);
            }

            return message;
        }

        public static AdsPushException CreateException(
            this FirebaseException firebaseException)
        {
            if (firebaseException is FirebaseMessagingException firebaseMessagingException)
            {
                var errorType = firebaseMessagingException.MessagingErrorCode switch
                {
                    MessagingErrorCode.ThirdPartyAuthError => AdsPushErrorType.InvalidAuthConfiguration,
                    MessagingErrorCode.InvalidArgument => AdsPushErrorType.InvalidArgument,
                    MessagingErrorCode.Internal => AdsPushErrorType.ServiceUnavailable,
                    MessagingErrorCode.QuotaExceeded => AdsPushErrorType.Unknown,
                    MessagingErrorCode.SenderIdMismatch => AdsPushErrorType.InvalidAuthConfiguration,
                    MessagingErrorCode.Unavailable => AdsPushErrorType.ServiceUnavailable,
                    MessagingErrorCode.Unregistered => AdsPushErrorType.InvalidToken,
                    null => AdsPushErrorType.Unknown,
                    _ => AdsPushErrorType.Unknown
                };

                return new AdsPushException(
                    firebaseMessagingException.Message,
                    errorType,
                    firebaseException.HttpResponse);
            }
            else if (firebaseException is FirebaseAuthException firebaseAuthException)
            {
                return new AdsPushException(
                    firebaseAuthException.Message,
                    AdsPushErrorType.InvalidAuthConfiguration,
                    firebaseException.HttpResponse);
            }
            else
            {
                return new AdsPushException(
                    firebaseException.Message,
                    AdsPushErrorType.Unknown,
                    firebaseException.HttpResponse);
            }
        }
    }
}