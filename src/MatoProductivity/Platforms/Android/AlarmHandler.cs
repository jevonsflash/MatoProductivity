using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class AlarmHandler : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(LocalNotification.TitleKey);
                string message = intent.GetStringExtra(LocalNotification.MessageKey);

                LocalNotification manager = LocalNotification.Instance ?? new LocalNotification();
                manager.Show(title, message);
            }
        }
    }

}

