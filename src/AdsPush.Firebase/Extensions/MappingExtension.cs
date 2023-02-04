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
            this AdsPushBasicSendPayload payload,
            AdsPushTarget target,
            string deviceToken)
        {
            if (payload.Parameters == null)
            {
                payload.Parameters = new Dictionary<string, object>();
            }

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
                AdsPushErrorType errorType;
                switch (firebaseMessagingException.MessagingErrorCode)
                {
                    case MessagingErrorCode.ThirdPartyAuthError:
                        errorType = AdsPushErrorType.InvalidAuthConfiguration;
                        break;
                    case MessagingErrorCode.InvalidArgument:
                        errorType = AdsPushErrorType.InvalidArgument;
                        break;
                    case MessagingErrorCode.Internal:
                        errorType = AdsPushErrorType.ServiceUnavailable;
                        break;
                    case MessagingErrorCode.QuotaExceeded:
                        errorType = AdsPushErrorType.Unknown;
                        break;
                    case MessagingErrorCode.SenderIdMismatch:
                        errorType = AdsPushErrorType.InvalidAuthConfiguration;
                        break;
                    case MessagingErrorCode.Unavailable:
                        errorType = AdsPushErrorType.ServiceUnavailable;
                        break;
                    case MessagingErrorCode.Unregistered:
                        errorType = AdsPushErrorType.InvalidToken;
                        break;
                    default:
                        errorType = AdsPushErrorType.Unknown;
                        break;
                }

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