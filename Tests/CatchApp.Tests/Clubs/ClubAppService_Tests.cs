using Abp.Domain.Repositories;
using CatchApp.Clubs;
using CatchApp.Geodata;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CatchApp.Tests.Clubs
{
    public class ClubAppService_Tests : CatchAppTestBase
    {
        private readonly IClubAppService _clubAppService;
        private readonly IRepository<Member, long> _memberRepository;
        private readonly IRepository<ClubVisit> _clubVisitRepository;

        public ClubAppService_Tests()
        {
            _clubAppService = LocalIocManager.Resolve<IClubAppService>();
            _memberRepository = LocalIocManager.Resolve<IRepository<Member, long>>();
            _clubVisitRepository = LocalIocManager.Resolve<IRepository<ClubVisit>>();
        }

        [Fact]
        public void Should_GetClubs()
        {
            var output = _clubAppService.GetClubs();
            output.Clubs.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async void Should_SearchClubs()
        {
            var input = new SearchClubsInput { Latitude = 47.051769, Longitude = 15.452930, DistanceKm = 3, UserName = "Hans" };
            var output = await _clubAppService.SearchClubs(input);
            output.Clubs.Count.ShouldBeGreaterThan(0);
        }

        [Fact]
        public async void Should_SearchClubs_PeopleInside()
        {
            var hans = _memberRepository.FirstOrDefault(x => x.UserName == "Hans");

            var input = new SearchClubsInput
            {
                Latitude = 47.051769,
                Longitude = 15.452930,
                DistanceKm = 10,
                UserName = hans.UserName,
                ActualTime = new DateTime(2016, 6, 8, 23, 0, 0)
            };
            var output = await _clubAppService.SearchClubs(input);
            var club1 = output.Clubs.FirstOrDefault(x => x.Id == 1);
            if (club1 != null)
            {
                club1.PeopleInside.ShouldBe(7);
                club1.FriendsInside.ShouldBe(1);
            }
        }

        [Fact]
        public void Should_GetClub()
        {
            var input = new GetClubInput
            {
                ClubId = 1,
                UserName = "Men1",
                ActualTime = new DateTime(2016, 6, 8, 23, 0, 0)
            };
            var output = _clubAppService.GetClub(input);

            output.Club.PeopleInside.ShouldBe(7);
            output.Club.Friends.Count.ShouldBe(4);
            output.Club.MalesInside.ShouldBe(5);
            output.Club.FemalesInside.ShouldBe(2);
            output.Club.SingleFemalesInside.ShouldBe(1);
            output.Club.SingleMalesInside.ShouldBe(2);
        }

        [Fact]
        public async void Should_CheckVisit_RemainingVisit()
        {
            DateTime actualTime = new DateTime(2016, 6, 8, 23, 0, 0);

            var input = new CheckVisitInput
            {
                Latitude = 47.0341535, // Bollwerk
                Longitude = 15.4141751,
                UserName = "Clubber",
                ActualTime = actualTime,
                IsOut = true
            };
            var member = _memberRepository.FirstOrDefault(x => x.UserName == input.UserName);

            var output = await _clubAppService.CheckVisit(input);
            output.IsOut.ShouldBe(true);
            output.ClubId.ShouldBe(1);
        }

        [Fact]
        public async void Should_CheckVisit_ChangeVisit()
        {
            DateTime actualTime = new DateTime(2016, 6, 8, 23, 0, 0);

            var input = new CheckVisitInput
            {
                Latitude = 47.074637, // Scheinbar
                Longitude = 15.451033,
                UserName = "Clubber",
                ActualTime = actualTime,
                IsOut = true
            };
            var member = _memberRepository.FirstOrDefaultAsync(x => x.UserName == input.UserName);

            var output = await _clubAppService.CheckVisit(input);
            
            output.ClubId.ShouldBe(2);
            output.IsOut.ShouldBe(true);
            //var visits = _clubVisitRepository.GetAll().Where(x => x.MemberId == member.Id && !x.HasLeft && actualTime < x.LeavingDate).ToList();
            //visits.Count.ShouldBe(1);
            //visits.First().ClubId.ShouldBe(2);
        }

        [Fact]
        public async void Should_CheckVisit_DeleteVisit()
        {
            DateTime actualTime = new DateTime(2016, 6, 8, 23, 0, 0);

            var input = new CheckVisitInput
            {
                Latitude = 47.041938, // außerhalb
                Longitude = 15.300563,
                UserName = "Clubber",
                ActualTime = actualTime,
                IsOut = true
            };

            var member = await _memberRepository.FirstOrDefaultAsync(x => x.UserName == input.UserName);
            var output = await _clubAppService.CheckVisit(input);
            output.IsOut.ShouldBe(false);
            output.ClubId.ShouldBeNull();
        }

        /// <summary>
        /// Wenn IsOut = false dann erfolgt keine Prüfung und
        /// alle offenen Visits werden sofort beendet.
        /// </summary>
        [Fact]
        public async void Should_CheckVisit_NotOut()
        {
            DateTime actualTime = new DateTime(2016, 6, 8, 23, 0, 0);

            var input = new CheckVisitInput
            {
                Latitude = 47.0341535, // Bollwerk
                Longitude = 15.4141751,
                UserName = "Clubber",
                ActualTime = actualTime,
                IsOut = false
            };

            var member = await _memberRepository.FirstOrDefaultAsync(x => x.UserName == input.UserName);
            var output = await _clubAppService.CheckVisit(input);
            output.IsOut.ShouldBe(false);
            output.ClubId.ShouldBeNull();
        }

    }
}

