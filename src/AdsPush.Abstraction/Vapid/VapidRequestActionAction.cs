namespace AdsPush.Abstraction.Vapid
{
    public class VapidRequestActionAction
    {
        /// <summary>
        /// Identifier for the action.
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// The title for the action button.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// URL of the icon to be displayed for the action button.
        /// </summary>
        public string Icon { get; set; }
    }
}
