using Newtonsoft.Json.Linq;

namespace AdsPush.Vapid
{
    /// <summary>
    /// Represents a VAPID subscription used for sending push notifications.
    /// </summary>
    public class VapidSubscription
    {
        /// <summary>
        /// Creates a new instance of <see cref="VapidSubscription"/> using the provided parameters.
        /// </summary>
        /// <param name="endpoint">The URL endpoint of the subscription.</param>
        /// <param name="p256dh">The p256dh value of the subscription.</param>
        /// <param name="auth">The auth value of the subscription.</param>
        /// <returns>A new <see cref="VapidSubscription"/> instance.</returns>
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

        /// <summary>
        /// Creates a new instance of <see cref="VapidSubscription"/> by parsing the subscription JSON.
        /// </summary>
        /// <param name="subscriptionJson">The JSON representation of the subscription.</param>
        /// <returns>A new <see cref="VapidSubscription"/> instance.</returns>
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

        /// <summary>
        /// Creates a new instance of <see cref="VapidSubscription"/> by parsing the base64-encoded subscription JSON.
        /// </summary>
        /// <param name="base64EncodedSubscriptionJson">The base64-encoded JSON representation of the subscription.</param>
        /// <returns>A new <see cref="VapidSubscription"/> instance.</returns>
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
            Endpoint = endpoint;
            P256dh = p256dh;
            Auth = auth;
        }

        /// <summary>
        /// Gets the URL endpoint of the subscription.
        /// </summary>
        public string Endpoint { get; }

        /// <summary>
        /// Gets the p256dh value of the subscription.
        /// </summary>
        public string P256dh { get; }

        /// <summary>
        /// Gets the auth value of the subscription.
        /// </summary>
        public string Auth { get; }
    }
}
