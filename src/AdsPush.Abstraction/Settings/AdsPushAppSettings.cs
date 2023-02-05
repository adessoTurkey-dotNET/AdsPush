using System.Collections.Generic;

namespace AdsPush.Abstraction.Settings
{
    /// <summary>
    /// AdsPush app settings.
    /// </summary>
    public class AdsPushAppSettings
    {
        /// <summary>
        /// 
        /// </summary>
        public AdsPushAppSettings()
        {
            this.TargetMappings = new Dictionary<AdsPushTarget, AdsPushProvider>();
        }
        
        /// <summary>
        /// Mapping for platform and target service for that platform.
        /// </summary>
        public Dictionary<AdsPushTarget, AdsPushProvider> TargetMappings { get; set; }
        
        /// <summary>
        /// Firebase configuration.
        /// </summary>
        public AdsPushFirebaseSettings Firebase { get; set; }
        
        /// <summary>
        /// APNS Configuration.
        /// </summary>
        public AdsPushAPNSSettings Apns { get; set; }
    }
}
