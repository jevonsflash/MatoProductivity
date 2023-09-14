using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Notifications;
using MatoProductivity.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MatoProductivity.Core.Services
{


    public class NotificationJob : IAsyncBackgroundJob<NotificationJobArgs>, ITransientDependency
    {
        private readonly IEventBus eventBus;

        public NotificationJob(IEventBus eventBus)
        {
            this.eventBus = eventBus;
        }
        public async Task ExecuteAsync(NotificationJobArgs args)
        {
            await Task.Run(async () =>
            {
                var notificationEto = new NotificationEto(args);
                await eventBus.TriggerAsync(this, notificationEto);
                Console.WriteLine("您有新的消息:" + args.NotificationTitle);
                Console.WriteLine(args.NotificationContent);
            });
        }
    }
}
