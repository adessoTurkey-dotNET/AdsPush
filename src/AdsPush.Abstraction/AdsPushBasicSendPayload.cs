using System;
using System.Collections.Generic;

namespace AdsPush.Abstraction
{
    /// <summary>
    /// AdsPush BasicSend payload
    /// </summary>
    public class AdsPushBasicSendPayload
    {
        /// <summary>
        /// 
        /// </summary>
        public AdsPushBasicSendPayload()
        {
            this.Parameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// Unique notification id.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// Basic notification tyoe.
        /// <see cref="AdsPushType"/>
        /// </summary>
        public AdsPushType PushType { get; set; } = AdsPushType.Alert;

        /// <summary>
        /// Notification Title
        /// <see cref="AdsPushText"/>
        /// </summary>
        public AdsPushText Title { get; set; }

        /// <summary>
        /// Notification detail text.
        /// <see cref="AdsPushText"/>
        /// </summary>
        public AdsPushText Detail { get; set; }

        /// <summary>
        /// Sound file name for notification.
        /// </summary>
        public string Sound { get; set; }
        
        /// <summary>
        /// Related group id to be able to group notification.
        /// Ios thread id 
        /// </summary>
        public string GroupId { get; set; }

        /// <summary>
        /// Set budge for application.
        /// </summary>
        public int? Badge { get; set; }

        /// <summary>
        /// Notification parameters/data.
        /// <see cref="Dictionary{TKey,TValue}"/>
        /// </summary>
        public Dictionary<string, object> Parameters { get; set; }
        
    }
}
