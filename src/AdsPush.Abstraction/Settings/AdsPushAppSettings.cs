using System.Collections.Generic;

namespace AdsPush.Abstraction.Settings
{
    public class AdsPushAppSettings
    {
        public AdsPushAppSettings()
        {
            this.TargetMappings = new Dictionary<AdsPushTarget, AdsPushProvider>();
        }
        
        public Dictionary<AdsPushTarget, AdsPushProvider> TargetMappings { get; set; }
        public AdsPushFirebaseSettings Firebase { get; set; }
        public AdsPushAPNSSettings Apns { get; set; }
    }
}
