using MatoProductivity.Infrastructure;
using System;

namespace MatoProductivity.Models
{
    public class WeekModel : ObservableObject
    {
        public DateTime LastDay { get; set; }
        public DateTime StartDay { get; set; }
        public string WeekString { get; set; }
    }
}
