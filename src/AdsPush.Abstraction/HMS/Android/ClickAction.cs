namespace AdsPush.Abstraction.HMS.Android
{
    public class ClickAction
    {
        /// <summary>
        /// 1: tap to open a custom app page
        /// 2: tap to open a specified URL
        /// 3: tap to start the app
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Use to navigate other app page with data or flags (optional) - type1
        /// </summary>
        public string Intent { get; set; }

        /// <summary>
        /// URL to be opened. The URL must be an HTTPS URL. type2
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Action corresponding to the activity of the page to be opened when
        /// the custom app page is opened through the action. type1
        /// </summary>
        public string Action { get; set; }
    }
}
