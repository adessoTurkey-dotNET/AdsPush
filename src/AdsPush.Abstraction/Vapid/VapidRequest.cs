using System.Collections.Generic;

namespace AdsPush.Abstraction.Vapid
{
    public class VapidRequest
    {
        /// <summary>
        /// The title of the notification.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The message body of the notification.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// URL of the image to be displayed in the notification.
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// A string that categorizes a notification.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// The URL of the badge to be displayed in the notification.
        /// </summary>
        public string Badge { get; set; }

        /// <summary>
        /// The URL of the icon to be displayed in the notification.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// The sound to play when the notification is displayed.
        /// </summary>
        public string Sound { get; set; }

        /// <summary>
        /// URL to be opened when the notification is clicked.
        /// </summary>
        public string ClickAction { get; set; }

        /// <summary>
        /// Time to live for the notification, in seconds. Defines how long a push message is retained if the user's device is offline. If not delivered in this time, the message will be dropped.
        /// </summary>
        public long? TTL { get; set; }

        /// <summary>
        /// Indicates whether the notification requires user interaction.
        /// </summary>
        public bool RequireInteraction { get; set; }

        /// <summary>
        /// Set the notification background or not.
        /// </summary>
        public bool Silent { get; set; }

        /// <summary>
        /// Pattern for the vibration (if supported).
        /// </summary>
        public string VibratePattern { get; set; }

        /// <summary>
        /// List of actions to be displayed in the notification.
        /// </summary>
        public List<VapidRequestActionAction> Actions { get; set; }

        /// <summary>
        /// Custom data payload for the notification.
        /// </summary>
        public Dictionary<string, string> Data { get; set; }

        /// <summary>
        /// List of URL arguments for Safari.
        /// </summary>
        public List<string> UrlArgs { get; set; }

        /// <summary>
        /// Language code for the notification.
        /// </summary>
        public string Language { get; set; }
    }
}
