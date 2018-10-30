using Abp.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Members.Policies
{
    public interface IMemberPolicy : IDomainService
    {
        void CheckMember(Member member);
    }
}
