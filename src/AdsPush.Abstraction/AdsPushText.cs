using System.Collections.Generic;

namespace AdsPush.Abstraction
{
    /// <summary>
    /// Use to create notification text.
    /// </summary>
    public sealed class AdsPushText
    {
        /// <summary>
        /// Sets flat string to the notification.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static AdsPushText CreateUsingString(
            string text)
        {
            return new AdsPushText(text,
                string.Empty,
                null);
        }
        
        /// <summary>
        /// Sets localized text model supported by APNS and FXM 
        /// </summary>
        /// <param name="localizationKey"></param>
        /// <param name="localizationArgs"></param>
        /// <returns></returns>
        public static AdsPushText CreateUsingLocalization(
            string localizationKey,
            IEnumerable<string> localizationArgs)
        {
            return new AdsPushText(string.Empty, 
                localizationKey,
                localizationArgs ?? new List<string>());
        }
        
        private AdsPushText(
            string text,
            string localizationKey,
            IEnumerable<string> localizationArgs)
        {
            this.Text = text;
            this.LocalizationKey = localizationKey;
            this.LocalizationArgs = localizationArgs;
        }
        
        /// <summary>
        /// Flat string.
        /// </summary>
        public string Text { get; }
        
        /// <summary>
        /// Localize string. 
        /// </summary>
        public string LocalizationKey { get; }
        
        /// <summary>
        /// Localize string parameters.
        /// </summary>
        public IEnumerable<string> LocalizationArgs { get; }
    }
}
