using Newtonsoft.Json.Linq;

namespace AdsPush.Vapid
{
    public class VapidSubscription
    {
        public static VapidSubscription FromParameters(
            string endpoint,
            string p256dh,
            string auth)
        {
            return new VapidSubscription(
                endpoint,
                p256dh,
                auth);
        }

        public static VapidSubscription FromSubscriptionJson(
            string subscriptionJson)
        {
            var jsonObject = JObject.Parse(subscriptionJson);
            var endpoint = jsonObject["endpoint"]?.ToString();
            var p256dh = jsonObject["keys.p256dh"]?.ToString();
            var auth = jsonObject["keys.auth"]?.ToString();
            return new VapidSubscription(
                endpoint,
                p256dh,
                auth);
        }

        public static VapidSubscription FromBase64EncodedSubscriptionJson(
            string base64EncodedSubscriptionJson)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedSubscriptionJson);
            var json = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return FromSubscriptionJson(json);
        }

        private VapidSubscription(
            string endpoint,
            string p256dh,
            string auth)
        {
            this.Endpoint = endpoint;
            this.P256dh = p256dh;
            this.Auth = auth;
        }

        public string Endpoint { get; }
        public string P256dh { get; }
        public string Auth { get; }
    }
}
