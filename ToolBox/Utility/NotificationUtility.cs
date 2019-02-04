namespace ToolBox.Utility
{
    public static class NotificationUtility
    {
        /// <summary>Show Error in System.Windows.MessageBox</summary>
        public static void ShowError(string error)
        {
            ShowMessage(error, "Error.");
        }
        /// <summary>Show Message in System.Windows.MessageBox</summary>
        public static void ShowMessage(string message)
        {
            System.Windows.MessageBox.Show(message);
        }
        /// <summary>Show Message in System.Windows.MessageBox with provided title</summary>
        public static void ShowMessage(string message, string title)
        {
            System.Windows.MessageBox.Show(message, title);
        }
    }
}
