namespace AdsPush
{
    public interface IAdsPushSenderFactory
    {
        IAdsPushSender GetSender(
            string appName);
    }
}
