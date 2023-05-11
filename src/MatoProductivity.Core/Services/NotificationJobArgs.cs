namespace MatoProductivity.Core.Services
{

    [Serializable]
    public class NotificationJobArgs
    {
        public NotificationJobArgs(string notificationTitle, string notificationContent)
        {
            NotificationTitle = notificationTitle;
            NotificationContent = notificationContent;
        }

        public string NotificationTitle { get; set; }
        public string NotificationContent { get; set; }
    }
}