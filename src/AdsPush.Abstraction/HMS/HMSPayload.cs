using AdsPush.Abstraction.HMS.Android;
using AdsPush.Abstraction.HMS.APNS;
using AdsPush.Abstraction.HMS.WebPush;
using Newtonsoft.Json;

namespace AdsPush.Abstraction.HMS
{
    public class HMSPayload
    {
        /// <summary>
        /// Push token of the target user of a message
        /// </summary>
        [JsonProperty("token")]
        public string PushToken { get; set; }

        /// <summary>
        /// Notification message content
        /// </summary>
        public HMSNotification Notification { get; set; }

        /// <summary>
        /// Android-specific message push control.
        /// Mandatory for Android notification messages.
        /// </summary>
        public AndroidConfig Android { get; set; }

        /// <summary>
        /// Web app message push control.
        /// </summary>
        public WebpushConfig Webpush { get; set; }

        /// <summary>
        /// iOS message push control. 
        /// </summary>
        public APNSConfig Apns { get; set; }

    }
}
