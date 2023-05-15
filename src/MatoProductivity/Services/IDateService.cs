using System;
using System.Collections.Generic;
using Abp.Dependency;
using MatoProductivity.Models;

namespace MatoProductivity.Services
{
    public interface IDateService : ISingletonDependency
    {
        WeekModel GetWeek(DateTime date);

        List<DayModel> GetDayList(DateTime firstDayInWeek, DateTime lastDayInWeek);
    }
}
