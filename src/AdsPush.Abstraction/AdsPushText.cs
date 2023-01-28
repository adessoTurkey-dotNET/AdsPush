using System.Collections.Generic;

namespace AdsPush.Abstraction
{
    public sealed class AdsPushText
    {
        public static AdsPushText CreateUsingString(
            string text)
        {
            return new AdsPushText(text,
                string.Empty,
                null);
        }
        
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
        
        public string Text { get; }
        public string LocalizationKey { get; }
        public IEnumerable<string> LocalizationArgs { get; }
    }
}
