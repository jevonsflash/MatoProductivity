using MatoProductivity.Infrastructure;
using System;
using System.ComponentModel;

namespace MatoProductivity.Models
{

    public class DayModel : ObservableObject
    {
        public int Column { get; set; }
        public DateTime Date { get; set; }
        public int Day { get; set; }
        public string DayName { get; set; }
        public DayStateEnum State { get; set; }
    }
}
