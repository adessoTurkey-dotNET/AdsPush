using System.Threading;
using System.Threading.Tasks;

namespace AdsPush.Abstraction
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
        Task SendAsync(
            AdsPushTarget target,
            string pushToken,
            AdsPushPayload payload,
            CancellationToken cancellationToken = default);

    }
}
