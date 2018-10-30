using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Clubs
{
    public class CheckVisitInput : IInputDto
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public string UserName { get; set; }
        public bool IsOut { get; set; }
        public DateTime? ActualTime { get; set; }
    }
}
