using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Members
{
    public class GetMemberInput : IInputDto
    {
        public string UserName { get; set; }
    }
}
