using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Members
{
    [AutoMapFrom(typeof(Member))]
    public class FriendDto : EntityDto<long>
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsMale { get; set; }
        public bool IsSingle { get; set; }

        public bool IsOut { get; set; }
        public DateTime? LastImageModificationTime { get; set; }
    }
}
