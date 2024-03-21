using AdsPush.Abstraction.APNS;
using AdsPush.Abstraction;
using System.Threading.Tasks;
using System.Threading;
using AdsPush.Abstraction.HMS;
using System;

namespace AdsPush.HMS
{
    public interface IHMSPushNotificationSender
    {
        /// <summary>
        /// Sends push notification wia HMS.
        /// </summary>
        /// <param name="hmsRequest">The request to send <see cref="HMSRequest"/></param>
        /// <param name="deviceToken">Token of target device.</param>
        /// <param name="cancellationToken">The to cancel the task. <see cref="CancellationToken"/></param>
        /// <returns>The result of the request <seealso cref="HMSResponse"/></returns>
        Task<HMSResponse> SendAsync(
            HMSRequest hmsRequest,
            string deviceToken,
            Guid applicationId,
            CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Sends push notification wia HMS.
        /// </summary>
        /// <param name="deviceToken">Token of target device.</param>        
        /// <param name="cancellationToken">The to cancel the task. <see cref="CancellationToken"/></param>
        /// <returns>The result of the request <seealso cref="HMSResponse"/></returns>
        Task<HMSResponse> SendAsync(
            string dataPayload,
            string deviceToken,
            Guid applicationId,
            CancellationToken cancellationToken = default(CancellationToken));

        Task SendAsync(
            string deviceToken,
            AdsPushBasicSendPayload payload,
            CancellationToken cancellationToken = default(CancellationToken));
    }
}
