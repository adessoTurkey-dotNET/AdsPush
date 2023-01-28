using Nexus.Core.Push.Abstraction.Settings;

namespace Nexus.Core.Push.Dataroid
{
    public interface IDataroidPushNotificationSenderFactory
    {
        IDataroidPushNotificationSender GetSender(
            string appName);

        IDataroidPushNotificationSender GetSender(
            string appName,
            NexusPushDataroidSettings dataroidSettings);
    }
}
