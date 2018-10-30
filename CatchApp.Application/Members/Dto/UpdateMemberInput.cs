using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Members
{
    public class UpdateMemberInput : IInputDto
    {
        public MemberDto Member { get; set; }
        public string UserName { get; set; }
    }
}
