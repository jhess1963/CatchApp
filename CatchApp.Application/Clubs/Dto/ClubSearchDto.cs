using Abp.Application.Services.Dto;
using CatchApp.Geodata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Clubs
{
    public class ClubSearchDto : EntityDto
    {
        public string Name { get; set; }

        public int PeopleInside { get; set; }
        public bool IsOpen { get; set; }
        public int FriendsInside { get; set; }
        public string Categories { get; set; }
        public double Distance { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public DateTime? LastImageModificationTime { get; set; }
    }
}
