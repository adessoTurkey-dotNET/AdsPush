using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FirebaseAdmin.Messaging;
using AdsPush.Abstraction;
using AdsPush.Abstraction.Firebase;

namespace AdsPush.Firebase
{
    /// <summary>
    /// Use to trigger firebase push notification request
    /// </summary>
    public interface IFirebasePushNotificationSender
    {
        /// <summary>
        /// Send push to single device. <seealso cref="Message"/>
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FirebaseNotificationResult> SendToSingleAsync(
            Message notification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends single notification to the multi devices tha provided device token in <paramref name="notification"/> object.
        /// </summary>
        /// <param name="notification"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FirebaseNotificationBatchResult> SendToMultiDeviceAsync(
            MulticastMessage notification,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends different notifications to different devices as batch.
        /// </summary>
        /// <param name="notifications"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<FirebaseNotificationBatchResult> SendBatchNotificationAsync(
            IEnumerable<Message> notifications,
            CancellationToken cancellationToken = default);

        Task SendAsync(
            AdsPushTarget target,
            string token,
            AdsPushPayload payload,
            CancellationToken cancellationToken = default);
    }
}
