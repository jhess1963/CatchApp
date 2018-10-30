using Abp.Application.Services;
using CatchApp.Members;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Members
{
    public interface IMemberAppService : IApplicationService
    {
        Task CreateMember(CreateMemberDto member);
        GetFriendsOutput GetFriends(GetFriendsInput input);

        GetMemberOutput GetMember(GetMemberInput input);

        void UpdateMember(UpdateMemberInput input);

        void CreateFriendship(CreateFriendshipInput input);
        void DeleteFriendship(DeleteFriendshipInput input);

        Task UpdateMemberImage(UpdateMemberImageInput input);
        Task<GetMemberImageOutput> GetMemberImage(GetMemberImageInput input);

    }
}
