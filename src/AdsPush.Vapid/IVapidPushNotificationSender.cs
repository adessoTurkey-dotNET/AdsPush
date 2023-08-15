using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Vapid;

namespace AdsPush.Vapid
{
    /// <summary>
    /// Defines operations for sending VAPID notifications.
    /// </summary>
    public interface IVapidPushNotificationSender
    {
        /// <summary>
        /// Sends a VAPID push notification with the provided subscription and payload.
        /// </summary>
        /// <param name="subscription">The subscription information.</param>
        /// <param name="payload">The payload as a string.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<VapidResponse> SendAsync(
            VapidSubscription subscription,
            string payload,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a VAPID push notification with the provided subscription and payload.
        /// </summary>
        /// <param name="subscription">The subscription information.</param>
        /// <param name="payload">The payload as a <see cref="VapidRequest"/> model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task<VapidResponse> SendAsync(
            VapidSubscription subscription,
            VapidRequest payload,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a VAPID push notification with the provided subscription JSON and payload.
        /// </summary>
        /// <param name="subscriptionJson">The subscription information as JSON.</param>
        /// <param name="payload">The payload as an <see cref="AdsPushBasicSendPayload"/> model.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendAsync(
            string subscriptionJson,
            AdsPushBasicSendPayload payload,
            CancellationToken cancellationToken = default);
    }
}
