using System.Collections.Generic;

namespace AdsPush.Abstraction.HMS.Android
{
    public class AndroidNotification
    {
        // Basic Payload
        /*
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImageUrl { get; set; } */


        /// <summary>
        /// Custom app icon of Android app. Must be stored in /res/raw directory
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Custom notification bar button color in #RRGGBB format
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Custom ringtone. Must be stored in the /res/raw directory
        /// </summary>
        public string Sound { get; set; }

        public bool DefaultSound { get; set; }

        /// <summary>
        /// Notification tag.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Message tapping action.
        /// </summary>
        public ClickAction ClickAction { get; set; }

        /// <summary>
        /// ID in a string format of the localized message title.
        /// </summary>
        public string TitleLocKey { get; set; }

        /// <summary>
        ///  Variables of the localized message title. F
        /// </summary>
        public IEnumerable<string> TitleLocArgs { get; set; }

        /// <summary>
        /// ID in a string format of the localized message body.
        /// </summary>
        public string BodyLocKey { get; set; }

        /// <summary>
        /// Variables of the localized message body.
        /// </summary>
        public IEnumerable<string> BodyLocArgs { get; set; }

        /// <summary>
        /// Custom channel for displaying notification messages.  (new in Android O or later)
        /// </summary>        
        public string ChannelId { get; set; }

        /// <summary>
        /// Brief description of a notification message to an Android app.

        /// </summary>
        public string NotifySummary { get; set; }

        /// <summary>
        /// Notification bar style. The options are as follows:  0,1,3
        /// </summary>
        public int Style { get; set; }

        /// <summary>
        /// Android notification message title in large text style 1
        /// </summary>
        public string BigTitle { get; set; }

        /// <summary>
        /// Android notification message body in large text style 1
        /// </summary>
        public string BigBody { get; set; }

        /// <summary>
        /// Unique notification ID of a message. If a message does not contain the ID or the ID is -1,
        /// Push Kit will generate a unique ID for the message. Different notification messages can
        /// use the same notification ID, so that new messages can overwrite old messages.
        /// </summary>
        public int NotifyId { get; set; }


        /// <summary>
        /// Message group. For example, if 10 messages that contain the same value of group are sent to a device,
        /// the device displays only the latest message and the total number of messages received in the group,
        /// but does not display these 10 messages.
        /// </summary>
        public string Group { get; set; }

        public int Badge { get; set; }

        /// <summary>
        /// Content displayed on the status bar after the device receives a notification message
        /// Prior  to API level 21 (Lollipop), gets or sets the text that is displayed in the status
        ///   bar when the notification first arrives.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Time when Android notification messages are delivered, in the UTC timestamp format. If you send
        /// multiple messages at the same time, they will be sorted based on this value and displayed
        /// in the Android notification panel.
        /// </summary>
        public string When { get; set; }

        /// <summary>
        /// Android notification message priority Loq/Normal
        /// </summary>
        public string Importance { get; set; }

        /// <summary>
        /// Indicates whether to use the default vibration mode.
        /// </summary>
        public bool UseDefaultVibrate { get; set; }

        /// <summary>
        /// Indicates whether to use the default breathing ligth.
        /// </summary>
        public bool UseDefaultLight { get; set; }

        /// <summary>
        /// Custom vibration mode for an Android notification message.
        /// </summary>
        public IEnumerable<string> VibrateConfig { get; set; }

        /// <summary>
        /// Android notification message visibility.
        /// VISIBILITY_UNSPECIFIED, PUBLIC, PRIVATE, SECRET, 
        /// </summary>
        public string Visibility { get; set; }

        /// <summary>
        /// Custom breathing light color.
        /// </summary>
        public LigthSettings LightSettings { get; set; }

        /// <summary>
        /// Indicates whether to display notification messages when an app is running in the foreground.
        /// </summary>
        public bool ForegroundShow { get; set; }

        /// <summary>
        /// ID of the user-app relationship
        /// </summary>
        public string ProfileId { get; set; }

        /// <summary>
        /// Action buttons of a notification message. A maximum of three buttons can be set.
        /// </summary>
        public IEnumerable<Button> Buttons { get; set; }

    }
}
