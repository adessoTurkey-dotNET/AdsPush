using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Vapid;

namespace AdsPush.Vapid
{
    /// <summary>
    /// Use to commute VAPID Notification supported services.
    /// </summary>
    public interface IVapidPushNotificationSender
    {
        /// <summary>
        ///  <see cref="VapidSubscription"/> and
        ///  <see cref="AdsPushBasicSendPayload"/>
        /// </summary>
        /// <param name="subscription">Use to pass subscription info</param>
        /// <param name="payload"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<VapidResponse> SendAsync(
            VapidSubscription subscription,
            string payload,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///  <see cref="VapidSubscription"/> and
        ///  <see cref="AdsPushBasicSendPayload"/>
        /// </summary>
        /// <param name="subscription">Use to pass subscription info</param>
        /// <param name="payload">The payload model.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<VapidResponse> SendAsync(
            VapidSubscription subscription,
            VapidRequest payload,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///
        /// </summary>
        /// <param name="subscriptionJson"></param>
        /// <param name="payload"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task SendAsync(
            string subscriptionJson,
            AdsPushBasicSendPayload payload,
            CancellationToken cancellationToken = default);
    }
}
