namespace AdsPush.Abstraction.HMS
{
    public class HMSNotification
    {
        /// <summary>
        /// Notification message title.

        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Notification message content.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        ///	URL of the custom small image on the right of a notification message.If this parameter is not set,
        ///	the image is not displayed. The URL must use the HTTPS protocol,
        ///	for example, https://example.com/image.png.
        /// </summary>
        public string Image { get; set; }

    }
}
