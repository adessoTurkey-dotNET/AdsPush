using System;
using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction;
using AdsPush.Abstraction.APNS;

namespace AdsPush.APNS
{
    /// <summary>
    /// Use to communicate Apple Push Notification Server.
    /// </summary>
    public interface IApplePushNotificationSender
    {
       /// <summary>
       /// Sends push notification wia APNS.
       /// For request detail: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/sending_notification_requests_to_apns
       /// </summary>
       /// <param name="apnsRequest">The request to send <see cref="APNSRequest"/></param>
       /// <param name="deviceToken">Token of target device.</param>
       /// <param name="apnsId">Unique id to be able to tract notification from APNS</param>
       /// <param name="apnsExpiration">Attempt policy to retry. <see cref="APNSExpiration"/></param>
       /// <param name="apnsPriority">The server priority</param>
       /// <param name="isBackground">To trigger background task. For more: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/pushing_background_updates_to_your_app</param>
       /// <param name="cancellationToken">The to cancel the task. <see cref="CancellationToken"/></param>
       /// <returns>The result of the request <seealso cref="APNSResponse"/></returns>
        Task<APNSResponse> SendAsync(
            APNSRequest apnsRequest,
            string deviceToken,
            Guid apnsId,
            APNSExpiration apnsExpiration = null,
            int apnsPriority = 10,
            bool isBackground = false,
            CancellationToken cancellationToken = default);

       /// <summary>
       /// Sends push notification wia APNS.
       /// For request detail: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/sending_notification_requests_to_apns
       /// </summary>
       /// <param name="objectPayload">A serializable object to be able to put json payload of APNS request</param>
       /// <param name="deviceToken">Token of target device.</param>
       /// <param name="apnsId">Unique id to be able to tract notification from APNS</param>
       /// <param name="apnsExpiration">Attempt policy to retry. <see cref="APNSExpiration"/></param>
       /// <param name="apnsPriority">The server priority</param>
       /// <param name="isBackground">To trigger background task. For more: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/pushing_background_updates_to_your_app</param>
       /// <param name="cancellationToken">The to cancel the task. <see cref="CancellationToken"/></param>
       /// <returns>The result of the request <seealso cref="APNSResponse"/></returns>
        Task<APNSResponse> SendAsync(
            object objectPayload,
            string deviceToken,
            Guid apnsId,
            APNSExpiration apnsExpiration = null,
            int apnsPriority = 10,
            bool isBackground = false,
            CancellationToken cancellationToken = default);

       /// <summary>
       /// Sends push notification wia APNS.
       /// For request detail: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/sending_notification_requests_to_apns
       /// </summary>
       /// <param name="jsonPayload">Json payload of APNS request</param>
       /// <param name="deviceToken">Token of target device.</param>
       /// <param name="apnsId">Unique id to be able to tract notification from APNS</param>
       /// <param name="apnsExpiration">Attempt policy to retry. <see cref="APNSExpiration"/></param>
       /// <param name="apnsPriority">The server priority</param>
       /// <param name="isBackground">To trigger background task. For more: https://developer.apple.com/documentation/usernotifications/setting_up_a_remote_notification_server/pushing_background_updates_to_your_app</param>
       /// <param name="cancellationToken">The to cancel the task. <see cref="CancellationToken"/></param>
       /// <returns>The result of the request <seealso cref="APNSResponse"/></returns>
        Task<APNSResponse> SendAsync(
            string jsonPayload,
            string deviceToken,
            Guid apnsId,
            APNSExpiration apnsExpiration = null,
            int apnsPriority = 10,
            bool isBackground = false,
            CancellationToken cancellationToken = default);


       Task SendAsync(
           string deviceToken,
           AdsPushPayload payload,
           CancellationToken cancellationToken = default);
    }
}
