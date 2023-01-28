using System.Threading;
using System.Threading.Tasks;
using Nexus.Core.Push.Abstraction;

namespace Nexus.Core.Push.Dataroid
{
    public interface IDataroidPushNotificationSender
    {
        Task SendAsync(
            NexusPushTarget target,
            string customerId,
            NexusPushPayload payload,
            CancellationToken cancellationToken = default);
    }
}
