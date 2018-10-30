using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Clubs
{
    public class GetClubInput : IInputDto
    {
        public int ClubId { get; set; }

        public string UserName { get; set; }
        public DateTime? ActualTime { get; set; }
    }
}
