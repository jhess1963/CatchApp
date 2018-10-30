using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Extensions
{
    /// <summary>
    /// Extensions
    /// </summary>
    public static class CatchAppExtensions
    {
        /// <summary>
        /// Calculates the week-day of the actual time, take into consideration
        /// that the day starts and ends on 6.00 (and not at 12.00)
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static WeekDay WeekdayOfClubTime(this DateTime date)
        {
            if (date.Hour < 7)
                date = date.AddDays(-1);

            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return WeekDay.Sunday;
                case DayOfWeek.Monday:
                    return WeekDay.Monday;
                case DayOfWeek.Tuesday:
                    return WeekDay.Tuesday;
                case DayOfWeek.Wednesday:
                    return WeekDay.Wednesday;
                case DayOfWeek.Thursday:
                    return WeekDay.Thursday;
                case DayOfWeek.Friday:
                    return WeekDay.Friday;
                case DayOfWeek.Saturday:
                    return WeekDay.Saturday;
            }
            return WeekDay.Monday;
        }
    }
}
