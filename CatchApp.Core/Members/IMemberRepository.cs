using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Members
{
    public interface IMemberRepository : IRepository<Member, long>
    {
        void DeleteFriend(long memberId, long friendId);
        void InsertFriend(long memberId, long friendId);
    }
}
