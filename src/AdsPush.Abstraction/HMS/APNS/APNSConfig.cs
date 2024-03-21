using System.Collections.Generic;

namespace AdsPush.Abstraction.HMS.APNS
{
    public class APNSConfig
    {
        public IReadOnlyDictionary<string, string> Headers { get; set; }
        public APNSPayload APNSPayload { get; set; }
        public IDictionary<string, object> CustomData { get; set; }

        public HMSOptions HMSOptions { get; set; }
    }
}
