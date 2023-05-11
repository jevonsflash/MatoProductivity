using Abp.Events.Bus;
using MatoProductivity.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Models
{
    public class NotificationEto:EventData
    {
        public NotificationEto(NotificationJobArgs args)
        {
            Args = args;
        }

        public NotificationJobArgs Args { get; }
    }
}
