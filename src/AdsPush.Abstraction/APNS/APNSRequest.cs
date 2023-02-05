using System.Collections.Generic;

namespace AdsPush.Abstraction.APNS
{
    /// <summary>
    /// Native APNS request model.
    /// </summary>
    public class APNSRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public APNSRequest()
        {
            this.AdditionalParameters = new Dictionary<string, object>();
        }

        /// <summary>
        /// The maim payload that sends in "aps" field in notification request to Apple Push Notification Sever.
        /// </summary>
        public APNSPayload ApnsPayload { get; set; }

        /// <summary>
        /// The values that attaches to the main request with the aps field. Each key of <see cref="Dictionary{TKey,TValue}"/> represents a filed in the notification request.
        /// </summary>
        public Dictionary<string, object> AdditionalParameters { get; set; }
    }
}
