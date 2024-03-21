namespace AdsPush.Abstraction.HMS.Android
{
    public class Button
    {
        public string Name { get; set; }

        /// <summary>
        /// 0: Open the app home page
        /// 1: open a custom app page
        /// 2: open a specified web page
        /// 3: delete a notification message
        /// 4: share a notification message(this action is supported only on Huawei device)
        /// </summary>
        public int ActionType { get; set; }

        /// <summary>
        /// 0: open the page through intent
        /// 1: open the page through action
        /// </summary>
        public int IntentType { get; set; }

        public string Intent { get; set; }

        /// <summary>
        /// When action_type is set to 0 or 1, this parameter is used to transparently
        /// transmit data to an app after a button is tapped.
        /// key-value pairs
        /// </summary>
        public string Data { get; set; }
    }
}
