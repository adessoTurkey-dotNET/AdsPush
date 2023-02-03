using System.Threading;
using System.Threading.Tasks;
using AdsPush.Abstraction;
using AdsPush.APNS;
using AdsPush.Firebase;

namespace AdsPush
{
    public interface IAdsPushSender
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="pushToken"></param>
        /// <param name="payload"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="AdsPushException">When any error occurred related to the push sending and configuration based.</exception>
        Task BasicSendAsync(
            AdsPushTarget target,
            string pushToken,
            AdsPushBasicSendPayload payload,
            CancellationToken cancellationToken = default);

        IApplePushNotificationSender GetApnsSender();
        IFirebasePushNotificationSender GetFirebaseSender();
    }
}
