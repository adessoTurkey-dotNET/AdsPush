namespace AdsPush.Abstraction.HMS.APNS
{
    public class APNSPayload
    {
        /// <summary>
        /// Include this key when you want the system to display a standard alert or a banner. 
        /// </summary>
        public Alert Alert { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AlertString { get; set; }

        /// <summary>
        /// Include this key when you want the system to modify the badge of your app icon.
        /// </summary>
        public int? Badge { get; set; }

        /// <summary>
        /// Include this key when you want the system to play a sound. The value of this key is the name of a sound file.
        /// </summary>
        public string Sound { get; set; }

        /// <summary>
        /// Include this key with a value of 1 to configure a background update notification.
        /// </summary>
        public bool ContentAvailable { get; set; }

        /// <summary>
        /// Provide this key with a string value that represents the notification’s type.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Provide this key with a string value that represents the app-specific identifier for grouping notifications.
        /// </summary>
        public string ThreadId { get; set; }
 
    }
}
