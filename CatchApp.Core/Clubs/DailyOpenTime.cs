using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using CatchApp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp
{
    public class DailyOpenTime : AuditedEntity
    {
        public WeekDay DayOfWeek { get; set; }
        public DateTime OpenTime { get; set; }
        public DateTime CloseTime { get; set; }

        [ForeignKey("ClubId")]
        public Club Club { get; set; }
        public int ClubId { get; set; }

        public bool IsOpen(DateTime time)
        {
            if (time.WeekdayOfClubTime() == DayOfWeek)
            {
                DateTime openTime;
                DateTime closeTime;

                openTime = new DateTime(time.Year, time.Month, time.Day, OpenTime.Hour, OpenTime.Minute, 0);
                if (time.Hour < 7)
                    openTime = openTime.AddDays(-1);
                if (CloseTime.Hour >= OpenTime.Hour)
                    closeTime = new DateTime(openTime.Year, openTime.Month, openTime.Day, CloseTime.Hour, CloseTime.Minute, 0);
                else
                {
                    DateTime hilf = openTime.AddDays(1);
                    closeTime = new DateTime(hilf.Year, hilf.Month, hilf.Day, CloseTime.Hour, CloseTime.Minute, 0);
                }
                return (time >= openTime && time <= closeTime);
            }
            return false;
        }

        /// <summary>
        /// Ermittelt die Sperrstunde zum angegebenen Zeitpunkt
        /// Wenn Club nicht geöffnet ist wird null zurückgeliefert.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public DateTime? GetActualCloseTime(DateTime time)
        {
            if (time.WeekdayOfClubTime() == DayOfWeek)
            {
                DateTime openTime;

                openTime = new DateTime(time.Year, time.Month, time.Day, OpenTime.Hour, OpenTime.Minute, 0);
                if (time.Hour < 7)
                    openTime = openTime.AddDays(-1);
                if (CloseTime.Hour >= OpenTime.Hour)
                    return new DateTime(openTime.Year, openTime.Month, openTime.Day, CloseTime.Hour, CloseTime.Minute, 0);
                else
                {
                    DateTime hilf = openTime.AddDays(1);
                    return new DateTime(hilf.Year, hilf.Month, hilf.Day, CloseTime.Hour, CloseTime.Minute, 0);
                }
            }
            else
                return null;
        }

    }

    public enum WeekDay : byte
    {
        Monday = 0,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }
}
