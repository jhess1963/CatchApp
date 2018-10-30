using Abp.Domain.Repositories;
using CatchApp.Members;
using CatchApp.Users;
using Shouldly;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CatchApp.Tests.Members
{
    public class MemberAppService_Tests : CatchAppTestBase
    {
        private readonly IMemberAppService _memberAppService;
        private readonly IMemberRepository _memberRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IUserAppService _userAppService;

        public MemberAppService_Tests()
        {
            _memberAppService = LocalIocManager.Resolve<IMemberAppService>();
            _memberRepository = LocalIocManager.Resolve<IMemberRepository>();
            _userRepository = LocalIocManager.Resolve<IRepository<User, long>>();
            _userAppService = LocalIocManager.Resolve<IUserAppService>();
        }

        [Fact]
        public async Task Should_CreateMember()
        {
            CreateMemberDto memberDto = new CreateMemberDto
            {
                UserName = "Test",
                Name = "FirstName",
                Surname = "LastName",
                Password = "password",
                EmailAddress = "test@test.at",
                IsMale = true,
                IsSingle = true
            };

            await _memberAppService.CreateMember(memberDto);

            var member = _memberRepository.FirstOrDefault(x => x.UserName == memberDto.UserName);
            member.ShouldNotBeNull();
            var user = _userRepository.FirstOrDefault(x => x.UserName == member.UserName);
            user.ShouldNotBeNull();
        }

        [Fact]
        public void Should_GetFriends()
        {
            GetFriendsInput input = new GetFriendsInput
            {
                UserName = "Hans",
                ActualTime = new DateTime(2016, 6, 8, 23, 0, 0)
            };
            var output = _memberAppService.GetFriends(input);

            output.Friends.Count.ShouldBe(1);
            var friend = output.Friends.FirstOrDefault();
            friend.ShouldNotBeNull();
            if (friend != null)
                friend.IsOut.ShouldBe(true);
        }

        [Fact]
        public async void Should_UpdateMember()
        {
            UpdateMemberInput input = new UpdateMemberInput
            {
                UserName = "Hans",
                Member = new MemberDto
                {
                    UserName = "Hans",
                    Name = "Hess",
                    Surname = "Johann",
                    IsMale = true,
                    IsSingle = true // changed field
                }
            };

            var userOutput = await _userAppService.GetUsers();

            User user = _userRepository.FirstOrDefault(x => x.UserName == input.UserName);
            Member member = _memberRepository.FirstOrDefault(m => m.UserName == input.UserName);
            long userId = user.Id;
            long memberId = member.Id;

            DateTime? modificationTime = user.LastModificationTime;

            _memberAppService.UpdateMember(input);

            member = _memberRepository.Get(memberId);
            member.IsSingle.ShouldBe(true);

            user = _userRepository.Get(userId);
            // keine Änderung in User
            // user.LastModificationTime.ShouldBe(modificationTime);

            // Änderung des UserName bewirkt Änderung in Member und User
            input.Member.UserName = "Hans1";
            _memberAppService.UpdateMember(input);

            member = _memberRepository.Get(memberId);
            member.UserName.ShouldBe(input.Member.UserName);

            user = _userRepository.Get(userId);
            user.UserName.ShouldBe(input.Member.UserName);
        }

        [Fact]
        public void Should_DeleteFriendship()
        {
            DeleteFriendshipInput input = new DeleteFriendshipInput
            {
                UserName1 = "Hans",
                UserName2 = "Clubber"
            };

            _memberAppService.DeleteFriendship(input);
        }

        [Fact]
        public void Should_CreateFriendShip()
        {
            CreateFriendshipInput input = new CreateFriendshipInput
            {
                UserName1 = "Hans",
                UserName2 = "Alex"
            };

            _memberAppService.CreateFriendship(input);
        }
    }
}
