using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Clubs
{
    public class SearchClubsInput : IInputDto
    {
        public string UserName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double DistanceKm { get; set; }

        /// <summary>
        /// optionale Uhrzeit, wenn nicht gesetzt wird Systemzeit verwendet
        /// </summary>
        public DateTime? ActualTime { get; set; }

    }
}
