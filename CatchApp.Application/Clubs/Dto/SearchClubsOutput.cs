using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Clubs
{
    public class SearchClubsOutput : IOutputDto
    {
        public List<ClubSearchDto> Clubs { get; set; }
        public SearchClubsOutput()
        {
            Clubs = new List<ClubSearchDto>();
        }
    }
}
