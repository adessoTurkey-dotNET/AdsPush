namespace AdsPush.Abstraction.HMS
{
    public class HMSRequest
    {
        /// <summary>
        /// The maim payload that sends in "aps" field in notification request to Huawei Push Server.
        /// </summary>
        public HMSPayload HMSPayload { get; set; }
    }
}
