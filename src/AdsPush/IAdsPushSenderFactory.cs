namespace AdsPush
{
    /// <summary>
    /// Use this factory class to be able to get instance of  <see cref="IAdsPushSender"/>.
    /// </summary>
    public interface IAdsPushSenderFactory
    {
        /// <summary>
        /// Get push sender by for the requested application.
        /// <see cref="IAdsPushSender"/>
        /// </summary>
        /// <param name="appName">The app name configured in the related settings.</param>
        /// <returns></returns>
        IAdsPushSender GetSender(
            string appName);
    }
}
