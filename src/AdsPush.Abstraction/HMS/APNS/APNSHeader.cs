namespace AdsPush.Abstraction.HMS.APNS
{
    public class APNSHeader
    {
        public string ApnsTopic { get; set; }
        public string ApnsExpiration { get; set; }
        public string ApnsPriority { get; set; }
        public string ApnsPushType { get; set; }
    }
}
