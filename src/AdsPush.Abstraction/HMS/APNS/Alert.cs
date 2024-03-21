using System.Collections.Generic;

namespace AdsPush.Abstraction.HMS.APNS
{
    public class Alert
    {
        /// <summary>
        /// A short string describing the purpose of the notification.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Subtitle of the alert
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// The text of the alert message.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// A key to an alert-message string in a Localizable.strings file for the current localization
        /// </summary>
        public string LocKey { get; set; }

        /// <summary>
        /// Variable string values to appear in place of the format specifiers in loc-key.
        /// </summary>
        public IEnumerable<string> LocArgs { get; set; }

        /// <summary>
        /// The key to a title string in the Localizable.strings file for the current localization.
        /// </summary>
        public string TitleLocKey { get; set; }

        /// <summary>
        /// Variable string values to appear in place of the format specifiers in title-loc-key.
        /// </summary>
        public IEnumerable<string> TitleLocArgs { get; set; }

        /// <summary>
        /// If a string is specified, the system displays an alert that includes the Close and View buttons.
        /// </summary>
        public string ActionLocKey { get; set; }

        /// <summary>
        /// The filename of an image file in the app bundle, with or without the filename extension.
        /// </summary>
        public string LaunchImage { get; set; }

    }
}
