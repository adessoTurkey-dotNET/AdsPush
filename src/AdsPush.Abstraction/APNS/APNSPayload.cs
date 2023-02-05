using Newtonsoft.Json;

namespace AdsPush.Abstraction.APNS
{
    /// <summary>
    /// APNS Default payload.
    /// For more: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/generating_a_remote_notification
    /// </summary>
    public class APNSPayload
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("apns-push-type")]
        public string PushType { get; set; } = "alert";

        /// <summary>
        /// The alert objects that represents main notification content.
        /// <see cref="APNSAlert"/>
        /// </summary>
        [JsonProperty("alert")]
        public APNSAlert Alert { get; set; }

        /// <summary>
        /// The name of a sound file in your app’s main bundle or in the Library/Sounds folder of your app’s container directory.
        /// Specify the string “default” to play the system sound. Use this key for regular notifications.
        /// For critical alerts, use the sound dictionary instead. For information about how to prepare sounds, see UNNotificationSound.
        /// </summary>
        [JsonProperty("sound")]
        public string Sound { get; set; }

        /// <summary>
        /// An app-specific identifier for grouping related notifications.
        /// This value corresponds to the threadIdentifier property in the UNNotificationContent object.
        /// </summary>
        [JsonProperty("thread-id")]
        public string ThreadId { get; set; }

        /// <summary>
        /// The notification’s type. This string must correspond to the identifier of one of the UNNotificationCategory objects you register at launch time.
        /// See also:
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// The notification service app extension flag.
        /// If the value is 1, the system passes the notification to your notification service app extension before delivery.
        /// Use your extension to modify the notification’s content.
        /// See Modifying Content in Newly Delivered Notifications from https://developer.apple.com/documentation/usernotifications/modifying_content_in_newly_delivered_notifications
        /// </summary>
        [JsonProperty("target-content-id")]
        public string TargetContentId { get; set; }

        /// <summary>
        /// The background notification flag. To perform a silent background update, specify the value 1 and don’t include the alert, badge, or sound keys in your payload.
        /// </summary>
        [JsonProperty("content-available")]
        [JsonConverter(typeof(BoolToStringConverter))]
        public bool ContentAvailable { get; set; } = true;

        /// <summary>
        /// The number to display in a badge on your app’s icon. Specify 0 to remove the current badge, if any.
        /// </summary>
        [JsonProperty("badge")]
        public int? Badge { get; set; }

        /// <summary>
        /// The identifier of the window brought forward. The value of this key will be populated on the UNNotificationContent object created from the push payload.
        /// Access the value using the UNNotificationContent object’s targetContentIdentifier property.
        /// </summary>
        [JsonProperty("mutable-content")]
        [JsonConverter(typeof(BoolToStringConverter))]
        public bool MutableContent { get; set; } = true;

        /// <summary>
        /// The importance and delivery timing of a notification.
        /// The string values “passive”, “active”, “time-sensitive”, or “critical” correspond to the UNNotificationInterruptionLevel enumeration cases.
        /// See from https://developer.apple.com/documentation/usernotifications/unnotificationinterruptionlevel
        /// </summary>
        [JsonProperty("interruption-level")]
        public string InterruptionLevel { get; set; }

        /// <summary>
        /// The relevance score, a number between 0 and 1, that the system uses to sort the notifications from your app.
        /// The highest score gets featured in the notification summary.
        /// See relevanceScore from https://developer.apple.com/documentation/usernotifications/unnotificationcontent/3821031-relevancescore
        /// </summary>
        [JsonProperty("relevance-score")]
        public double RelevanceScore { get; set; }

        /// <summary>
        /// The criteria the system evaluates to determine if it displays the notification in the current Focus. For more information,
        /// see SetFocusFilterIntent from https://developer.apple.com/documentation/appintents/setfocusfilterintent
        /// </summary>
        [JsonProperty("filter-criteria")]
        public string FilterCriteria { get; set; }
    }
}
