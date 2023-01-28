namespace AdsPush.Abstraction
{
    public interface IAdsPushSenderFactory
    {
        IAdsPushSender GetSender(
            string appName);
    }
}
