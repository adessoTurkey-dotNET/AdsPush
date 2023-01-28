using System.Collections.Generic;
using Newtonsoft.Json;

namespace AdsPush.Abstraction.APNS
{
    /// <summary>
    /// The alert objects that shows in user's device.
    /// For more: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/generating_a_remote_notification
    /// </summary>
    public class APNSAlert
    {
        /// <summary>
        /// The title of the notification. Apple Watch displays this string in the short look notification interface. Specify a string that’s quickly understood by the user.
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// Additional information that explains the purpose of the notification.
        /// </summary>
        [JsonProperty("subtitle")]
        public string Subtitle { get; set; }

        /// <summary>
        /// The content of the alert message.
        /// </summary>
        [JsonProperty("body")]
        public string Body { get; set; }

        /// <summary>
        /// The name of the launch image file to display.
        /// If the user chooses to launch your app, the contents of the specified image or storyboard file are displayed instead of your app’s normal launch image.
        /// </summary>
        [JsonProperty("launch-image")]
        public string LaunchImage { get; set; }

        /// <summary>
        /// The key for a localized title string. Specify this key instead of the title key to retrieve the title from your app’s Localizable.strings files.
        /// The value must contain the name of a key in your strings file.
        /// </summary>
        [JsonProperty("title-loc-key")]
        public string TitleLocationKey { get; set; }

        /// <summary>
        /// An array of strings containing replacement values for variables in your title string.
        /// Each %@ character in the string specified by the title-loc-key is replaced by a value from this array.
        /// The first item in the array replaces the first instance of the %@ character in the string, the second item replaces the second instance, and so on.
        /// </summary>
        [JsonProperty("title-loc-args")]
        public IEnumerable<string> TitleLocationKeyArgs { get; set; }

        /// <summary>
        /// The key for a localized subtitle string. Specify this key instead of the title key to retrieve the title from your app’s Localizable.strings files.
        /// The value must contain the name of a key in your strings file.
        /// </summary>
        [JsonProperty("subtitle-loc-key")]
        public string SubtitleLocationKey { get; set; }

        /// <summary>
        /// An array of strings containing replacement values for variables in your subtitle string.
        /// Each %@ character in the string specified by the title-loc-key is replaced by a value from this array.
        /// The first item in the array replaces the first instance of the %@ character in the string, the second item replaces the second instance, and so on.
        /// </summary>
        [JsonProperty("subtitle-loc-args")]
        public IEnumerable<string> SubtitleLocationKeyArgs { get; set; }

        /// <summary>
        /// The key for a localized message string. Specify this key instead of the title key to retrieve the title from your app’s Localizable.strings files.
        /// The value must contain the name of a key in your strings file.
        /// </summary>
        [JsonProperty("loc-key")]
        public string BodyLocationKey { get; set; }

        /// <summary>
        /// An array of strings containing replacement values for variables in your subtitle string.
        /// Each %@ character in the string specified by the title-loc-key is replaced by a value from this array.
        /// The first item in the array replaces the first instance of the %@ character in the string, the second item replaces the second instance, and so on.
        /// </summary>
        [JsonProperty("loc-args")]
        public IEnumerable<string> BodyLocationKeyArgs { get; set; }
    }
}
