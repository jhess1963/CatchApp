using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CatchApp.Extensions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Spatial;

namespace CatchApp
{
    public class Club : AuditedEntity
    {
        public const int MaxLenghtClubName = 50;

        [MaxLength(MaxLenghtClubName)]
        public string Name { get; set; }

        public IList<Category> Categories { get; set; }

        public DbGeography Location { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public IList<DailyOpenTime> OpenTimes { get; set; }
        public IList<ClubVisit> Visits { get; set; }

        public virtual ClubImage ClubImage { get; set; }

        public Club()
        {
            OpenTimes = new List<DailyOpenTime>();
            Categories = new List<Category>();
        }

        /// <summary>
        /// Prüft, ob Öffnungszeit für die angegebene Uhrzeit vorhanden ist.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool IsOpen(DateTime time)
        {
            return OpenTimes.Count(x => x.IsOpen(time)) > 0;
        }
    }
}
