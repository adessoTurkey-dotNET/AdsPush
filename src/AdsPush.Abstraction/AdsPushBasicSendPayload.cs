using System;
using System.Collections.Generic;

namespace AdsPush.Abstraction
{
    public class AdsPushBasicSendPayload
    {
        public AdsPushBasicSendPayload()
        {
            this.Parameters = new Dictionary<string, object>();
        }

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public AdsPushType PushType { get; set; } = AdsPushType.Alert;

        public AdsPushText Title { get; set; }

        public AdsPushText Detail { get; set; }

        public string Sound { get; set; }
        public string GroupId { get; set; }

        public int? Badge { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
        
    }
}
