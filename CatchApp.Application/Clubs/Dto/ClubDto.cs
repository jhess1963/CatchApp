using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Clubs
{
    [AutoMapFrom(typeof(Club))]
    public class ClubDto : EntityDto
    {
        public string Name { get; set; }

        public int PeopleInside { get; set; }

        public int MalesInside { get; set; }
        public int FemalesInside { get; set; }

        public int SingleMalesInside { get; set; }
        public int SingleFemalesInside { get; set; }

        public bool IsOpen { get; set; }
        public IList<MemberDto> Friends { get; set; }
        public string Categories { get; set; }
        public double Distance { get; set; }

        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public ClubDto()
        {
            Friends = new List<MemberDto>();
        }
    }
}
