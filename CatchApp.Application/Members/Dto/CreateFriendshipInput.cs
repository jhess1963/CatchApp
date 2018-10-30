using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Members
{
    public class CreateFriendshipInput : IInputDto
    {
        public string UserName1 { get; set; }
        public string UserName2 { get; set; }
    }
}
