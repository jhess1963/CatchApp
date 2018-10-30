using Abp.EntityFramework;
using CatchApp.Members;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.EntityFramework.Repositories
{
    public class MemberRepository : CatchAppRepositoryBase<Member, long>, IMemberRepository
    {
        public MemberRepository(IDbContextProvider<CatchAppDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
            
        }

        public void DeleteFriend(long memberId, long friendId)
        {
            Member member = Context.Members.Include(x => x.Friends).FirstOrDefault(x => x.Id == memberId);
            Member friend = member.Friends.FirstOrDefault(x => x.Id == friendId);
            if (friend != null)
                member.Friends.Remove(friend);
        }

        public void InsertFriend(long memberId, long friendId)
        {
            Member member = Context.Members.Include(x => x.Friends).FirstOrDefault(x => x.Id == memberId);
            Member friend = member.Friends.FirstOrDefault(x => x.Id == friendId);
            if (friend == null)
            {
                friend = Context.Members.Find(friendId);
                member.Friends.Add(friend);
            }
        }

    }
}
