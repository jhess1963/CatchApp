using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Clubs
{
    public class CheckVisitOutput : IOutputDto
    {
        public bool IsOut { get; set; }
        public int? ClubId { get; set; }
    }
}
