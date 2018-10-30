using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatchApp.Extensions;

using Xunit;

namespace CatchApp.Tests.Extensions
{
    public class Date_Tests
    {
        [Fact]
        public static void Check_ClubWeekDay()
        {
            // MO 2.5.2016 22:00  => MO
            WeekDay wd = new DateTime(2016, 5, 2, 22, 0, 0).WeekdayOfClubTime();
            Assert.Equal<WeekDay>(WeekDay.Monday, wd);

            // TUE 3.5.2016 5:00 => MO
            wd = new DateTime(2016, 5, 3, 5, 0, 0).WeekdayOfClubTime();
            Assert.Equal<WeekDay>(WeekDay.Monday, wd);

            // SUN 8.5.2016 18:00 => SO
            wd = new DateTime(2016, 5, 8, 18, 0, 0).WeekdayOfClubTime();
            Assert.Equal<WeekDay>(WeekDay.Sunday, wd);

            // MO 9.5.2016 6:00 => SO
            wd = new DateTime(2016, 5, 9, 6, 0, 0).WeekdayOfClubTime();
            Assert.Equal<WeekDay>(WeekDay.Sunday, wd);
        }
    }
}
