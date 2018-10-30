using Abp.Domain.Repositories;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Members.Policies
{
    public class MemberPolicy : CatchAppServiceBase, IMemberPolicy
    {
        private readonly IRepository<Member, long> _memberRepository;

        public MemberPolicy(IRepository<Member, long> memberRepository)
        {
            _memberRepository = memberRepository;
        }
        public void CheckMember(Member member)
        {
            if (_memberRepository.Count(x => x.Id != member.Id && x.UserName.ToUpper().Equals(member.UserName.ToUpper())) > 0)
                throw new UserFriendlyException("Username already exists!");
        }
    }
}
