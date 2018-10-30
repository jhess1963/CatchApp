using Abp.Domain.Repositories;
using CatchApp.Members.Policies;
using CatchApp.Users;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.UI;
using System.IO;
using System.Drawing;
using CatchApp.Clubs;

namespace CatchApp.Members
{
    public class MemberAppService : CatchAppAppServiceBase, IMemberAppService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IRepository<MemberImage, long> _memberImageRepository;
        private readonly IMemberPolicy _memberPolicy;
        private readonly IRepository<User, long> _userRepository;
        private readonly IUserAppService _userAppService;
        private readonly IRepository<ClubVisit> _clubVisitRepository;

        public MemberAppService(IMemberRepository memberRepository,
            IRepository<MemberImage, long> memberImageRepository,
            IRepository<User, long> userRepository,
            IMemberPolicy memberPolicy,
            IUserAppService userAppService,
            IRepository<ClubVisit> clubVisitRepository
            )
        {
            _memberRepository = memberRepository;
            _memberImageRepository = memberImageRepository;
            _memberPolicy = memberPolicy;
            _userAppService = userAppService;
            _userRepository = userRepository;
            _clubVisitRepository = clubVisitRepository;
        }

        public async Task CreateMember(CreateMemberDto memberDto)
        {
            Member member = new Member
            {
                UserName = memberDto.UserName,
                Name = memberDto.Name,
                Surname = memberDto.Surname,
                IsMale = memberDto.IsMale,
                IsSingle = memberDto.IsSingle
            };

            _memberPolicy.CheckMember(member);

            await _userAppService.CreateUser(new Users.Dto.CreateUserInput
            {
                UserName = memberDto.UserName,
                Name = memberDto.Name,
                Surname = memberDto.Surname,
                EmailAddress = memberDto.EmailAddress,
                Password = memberDto.Password,
                IsActive = true,
                TenantId = 1 // default
            });

            _memberRepository.InsertAndGetId(member);
        }

        [AbpAuthorize()]
        public GetFriendsOutput GetFriends(GetFriendsInput input)
        {
            DateTime actualTime = input.ActualTime.HasValue ? input.ActualTime.Value : DateTime.Now;

            var friends = _memberRepository.GetAll()
                .Include(x => x.Friends)
                .Where(x => x.UserName.ToUpper() == input.UserName.ToUpper()).First()
                .Friends.ToList();

            GetFriendsOutput output = new GetFriendsOutput
            {
                Friends = friends.MapTo<List<FriendDto>>()
            };

            foreach (var friend in output.Friends)
            {
                var lastModification = _memberImageRepository
                    .GetAll()
                    .Where(x => x.Id == friend.Id)
                    .Select(x => x.LastModificationTime)
                    .FirstOrDefault();
                friend.LastImageModificationTime = lastModification;
                friend.IsOut = _clubVisitRepository.Count(x => x.MemberId == friend.Id && !x.HasLeft && actualTime < x.LeavingDate) > 0;
            }
            return output;
        }

        [AbpAuthorize()]
        public GetMemberOutput GetMember(GetMemberInput input)
        {
            var output = new GetMemberOutput
            {
                Member = _memberRepository.FirstOrDefault(x => x.UserName.ToUpper() == input.UserName.ToUpper()).MapTo<MemberDto>()
            };
            return output;
        }

        /// <summary>
        /// Aktualisiert Member und User
        /// </summary>
        /// <param name="input"></param>
        public void UpdateMember(UpdateMemberInput input)
        {
            Member member = _memberRepository.FirstOrDefault(m => m.UserName == input.UserName);
            // Prüfen, ob User betroffen ist
            bool userHasChanged = (input.Member.UserName != member.UserName || input.Member.Name != member.Name || input.Member.Surname != member.Surname);

            member.UserName = input.Member.UserName;
            member.Name = input.Member.Name;
            member.Surname = input.Member.Surname;
            member.IsMale = input.Member.IsMale;
            member.IsSingle = input.Member.IsSingle;

            _memberPolicy.CheckMember(member);

            _memberRepository.Update(member);

            // Änderungen nur durchführen, wenn Änderungen den User betreffen
            if (userHasChanged)
            {
                User user = _userRepository.FirstOrDefault(x => x.UserName == input.UserName);
                if (user != null)
                {
                    user.UserName = input.Member.UserName;
                    user.Name = input.Member.Name;
                    user.Surname = input.Member.Surname;

                    _userRepository.Update(user);
                }
            }
        }

        public void CreateFriendship(CreateFriendshipInput input)
        {
            long memberId1 = _memberRepository.GetAll().First(x => x.UserName == input.UserName1).Id;
            long memberId2 = _memberRepository.GetAll().First(x => x.UserName == input.UserName2).Id;

            _memberRepository.InsertFriend(memberId1, memberId2);
            _memberRepository.InsertFriend(memberId2, memberId1);
        }

        public void DeleteFriendship(DeleteFriendshipInput input)
        {
            long memberId1 = _memberRepository.GetAll().First(x => x.UserName == input.UserName1).Id;
            long memberId2 = _memberRepository.GetAll().First(x => x.UserName == input.UserName2).Id;

            _memberRepository.DeleteFriend(memberId1, memberId2);
            _memberRepository.DeleteFriend(memberId2, memberId1);
        }

        public async Task UpdateMemberImage(UpdateMemberImageInput input)
        {
            var member = await _memberRepository.FirstOrDefaultAsync(x => x.UserName == input.UserName);
            if (member == null)
                throw new UserFriendlyException(string.Format("User {0} not found!", input.UserName));

            if (input.Image == null || input.Image.Length == 0)
                await _memberImageRepository.DeleteAsync(member.Id);
            else
            {
                input.Image = ImageHelper.ResizeImage(input.Image, 480, 272);
                var memberImage = _memberImageRepository.FirstOrDefault(member.Id);
                if (memberImage == null)
                    await _memberImageRepository.InsertAsync(new MemberImage { Id = member.Id, Image = input.Image });
                else
                {
                    memberImage.Image = input.Image;
                    await _memberImageRepository.UpdateAsync(memberImage);
                }
            }
        }

        public async Task<GetMemberImageOutput> GetMemberImage(GetMemberImageInput input)
        {
            var member = await _memberRepository.FirstOrDefaultAsync(x => x.UserName == input.UserName);
            if (member == null)
                throw new UserFriendlyException(string.Format("User {0} not found!", input.UserName));

            var memberImage = await _memberImageRepository.FirstOrDefaultAsync(x => x.Id == member.Id);

            //memberImage.Image = ImageHelper.ResizeImage(memberImage.Image, 480, 272);

            return new GetMemberImageOutput
            {
                Image = memberImage.Image // ImageHelper.ResizeImage((memberImage != null ? memberImage.Image : null), 150, 150)
            };

        }

    }
}
