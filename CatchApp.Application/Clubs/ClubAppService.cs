using Abp.Domain.Repositories;
using CatchApp.Geodata;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Abp.AutoMapper;
using System.Threading.Tasks;
using Abp.Authorization;
using CatchApp.Members;
using System.Data.Entity.Spatial;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.Drawing.Drawing2D;

namespace CatchApp.Clubs
{
    [AbpAuthorize]
    public class ClubAppService : CatchAppAppServiceBase, IClubAppService
    {
        private readonly double _allowedDistance = 0.080;

        private readonly IRepository<Club> _clubRepository;
        private readonly IRepository<ClubVisit> _clubVisitRepository;
        private readonly IRepository<ClubImage> _clubImageRepository;
        private readonly IMemberRepository _memberRepository;


        public SearchClubsOutput GetClubs()
        {
            var clubs = _clubRepository.GetAllList();
            var output = new SearchClubsOutput();
            foreach (var club in clubs)
            {
                output.Clubs.Add(new ClubSearchDto
                { Name = club.Name });
            }
            return output;

        }

        public async Task LoadClubImages()
        {
            string filePath = HttpContext.Current.Server.MapPath("/Images/Clubs");
            var clubs = _clubRepository.GetAllList();
            foreach (var club in clubs)
            {
                string imagefilename = string.Format("Club{0}.jpg", club.Id);
                string imageFile = Path.Combine(filePath, imagefilename);
                ImageFormat imgFormat = ImageFormat.Jpeg;

                if (!File.Exists(imageFile))
                {
                    imageFile = Path.ChangeExtension(imageFile, "png");
                    imgFormat = ImageFormat.Png;
                }
                if (File.Exists(imageFile))
                {
                    Image image = Image.FromFile(imageFile);
                    var input = new UpdateClubImageInput { ClubId = club.Id };
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, imgFormat);
                        input.Image = ms.ToArray();
                    }
                    var clubImage = _clubImageRepository.FirstOrDefault(input.ClubId);
                    if (clubImage == null)
                        await _clubImageRepository.InsertAsync(new ClubImage { Id = input.ClubId, Image = input.Image });
                    else
                    {
                        clubImage.Image = input.Image;
                        await _clubImageRepository.UpdateAsync(clubImage);
                    }
                }
            }

        }

        /// <summary>
        /// Ermittelt alle Clubs die in einem angegebenen Bereich liegen inkl.
        /// der Besucher und Freunde in den Clubs
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SearchClubsOutput> SearchClubs(SearchClubsInput input)
        {
            DateTime actTime = input.ActualTime.HasValue ? input.ActualTime.Value : DateTime.Now;
            GeoPoint actualPoint = new GeoPoint(input.Latitude, input.Longitude);
            DbGeography actualGeoPoint = DbGeography.FromText(actualPoint.ToString());
            SearchClubsOutput output = new SearchClubsOutput();

            var friends = _memberRepository.GetAll()
                .Include(x => x.Friends)
                .Where(x => x.UserName.ToUpper() == input.UserName.ToUpper()).First()
                .Friends.ToList();

            double distanceM = input.DistanceKm * 1000;

            var clubs = _clubRepository.GetAll()
                .Include(x => x.OpenTimes)
                .Include(x => x.Categories);
            if (input.Latitude > 0)
                clubs = clubs.Where(x => x.Location.Distance(actualGeoPoint) < distanceM);

            var clubsDto = new List<ClubSearchDto>();

            foreach (var club in clubs.ToList())
            {
                double distanceToClub = Geo.Distance(actualPoint, new GeoPoint(club.Latitude, club.Longitude));
                if (input.Latitude == 0 || distanceToClub <= input.DistanceKm)
                {
                    var visits = await _clubVisitRepository.GetAll().Where(x => x.ClubId == club.Id
                            && !x.HasLeft && actTime <= x.LeavingDate).ToListAsync();
                    int numberOfVisits = visits.Count();

                    int numberOfFriends = visits.Select(x => x.MemberId).Intersect(friends.Select(x => x.Id)).Count();

                    var imageModification = await _clubImageRepository.GetAll()
                        .Where(x => x.Id == club.Id)
                        .Select(x => x.LastModificationTime)
                        .FirstOrDefaultAsync();

                    ClubSearchDto clubDto = new ClubSearchDto
                    {
                        Id = club.Id,
                        Name = club.Name,
                        IsOpen = club.IsOpen(actTime),
                        Distance = Geo.Distance(actualPoint, new GeoPoint(club.Latitude, club.Longitude)),
                        Longitude = club.Longitude,
                        Latitude = club.Latitude,
                        Categories = string.Join(", ", club.Categories.Select(x => x.Name)),
                        FriendsInside = numberOfFriends, 
                        PeopleInside = numberOfVisits,
                        LastImageModificationTime = imageModification
                    };

                    clubsDto.Add(clubDto);
                }
            }
            output.Clubs = clubsDto.OrderBy(x => x.Distance).ToList();
            return output;
        }

        public GetClubOutput GetClub(GetClubInput input)
        {
            DateTime actualTime = input.ActualTime.HasValue ? input.ActualTime.Value : DateTime.Now;

            var club = _clubRepository.GetAll()
                .Include(x => x.OpenTimes)
                .Include(x => x.Categories)
                .Where(x => x.Id == input.ClubId).First();

            var friends = _memberRepository.GetAll()
                .Include(x => x.Friends)
                .Where(x => x.UserName.ToUpper() == input.UserName.ToUpper()).First()
                .Friends.ToList();

            var membersInside = _clubVisitRepository.GetAll()
                .Include(x => x.Member)
                .Where(x => x.ClubId == club.Id
                    && !x.HasLeft && actualTime <= x.LeavingDate).ToList()
                    .Select(x => x.Member).ToList();

            var friendsInside = from m in membersInside
                                join f in friends on m.Id equals f.Id
                                select m;

            var output = new GetClubOutput
            {
                Club = new ClubDto
                {
                    Id = club.Id,
                    Name = club.Name,
                    IsOpen = club.IsOpen(actualTime),
                    Longitude = club.Longitude,
                    Latitude = club.Latitude,
                    Categories = string.Join(", ", club.Categories.Select(x => x.Name)),
                    PeopleInside = membersInside.Count,
                    MalesInside = membersInside.Count(m => m.IsMale),
                }
            };

            output.Club.FemalesInside = output.Club.PeopleInside - output.Club.MalesInside;
            output.Club.SingleMalesInside = membersInside.Where(x => x.IsSingle && x.IsMale).Count();
            output.Club.SingleFemalesInside = membersInside.Where(x => x.IsSingle && !x.IsMale).Count();

            foreach (var friend in friendsInside)
            {
                output.Club.Friends.Add(new MemberDto
                {
                    UserName = friend.UserName,
                    Name = friend.Name,
                    Surname = friend.Surname,
                    IsMale = friend.IsMale,
                    IsSingle = friend.IsSingle
                });
            }

            return output;
        }

        public async Task UpdateClubImage(UpdateClubImageInput input)
        {
            if (input.Image == null || input.Image.Length == 0)
                await _clubImageRepository.DeleteAsync(input.ClubId);
            else
            {
                input.Image = ImageHelper.ResizeImage(input.Image, 480, 272);
                var clubImage = _clubImageRepository.FirstOrDefault(input.ClubId);
                
                if (clubImage == null)
                    await _clubImageRepository.InsertAsync(new ClubImage { Id = input.ClubId, Image = input.Image});
                else
                {
                    clubImage.Image = input.Image;
                    await _clubImageRepository.UpdateAsync(clubImage);
                }
            }
        }

        public async Task<GetClubImageOutput> GetClubImage(GetClubImageInput input)
        {
            var clubImage = await _clubImageRepository.FirstOrDefaultAsync(x => x.Id == input.ClubId);
           
            //clubImage.Image = ImageHelper.ResizeImage(clubImage.Image, 480, 272);
            return new GetClubImageOutput
            {
                Image = clubImage.Image //ImageHelper.ResizeImage((clubImage != null ? clubImage.Image : null), 150, 150)
            };
        }

        public async Task<CheckVisitOutput> CheckVisit(CheckVisitInput input)
        {
            var output = new CheckVisitOutput { IsOut = false };
            Member member = await _memberRepository.GetAll().Where(m => m.UserName.ToUpper() == input.UserName.ToUpper()).FirstAsync();

            GeoPoint actualPoint = new GeoPoint(input.Latitude, input.Longitude);
            DateTime actualTime = input.ActualTime.HasValue ? input.ActualTime.Value : DateTime.Now;

            var visits = await _clubVisitRepository.GetAll().Where(x => x.MemberId == member.Id
            && !x.HasLeft && x.LeavingDate > actualTime).ToListAsync();

            if (!input.IsOut) // in jedem Fall Visits beenden
            {
                foreach (var oldVisit in visits)
                {
                    oldVisit.HasLeft = true;
                    await _clubVisitRepository.UpdateAsync(oldVisit);
                }
                return output;
            }

            var clubs = await _clubRepository.GetAll().Include(x => x.OpenTimes).ToListAsync();

            Dictionary<Club, double> clubDistances = new Dictionary<Club, double>();

            foreach (var club in clubs)
            {
                if (club.IsOpen(actualTime) && Geo.Distance(actualPoint, new GeoPoint(club.Latitude, club.Longitude)) <= _allowedDistance)
                {
                    clubDistances.Add(club, Geo.Distance(actualPoint, new GeoPoint(club.Latitude, club.Longitude)));
                }
            }
            if (clubDistances.Count > 0)
            {
                Club shortest_club = new Club();
                double shortest_distance = -1;
                foreach (var cd in clubDistances)
                {
                    if (shortest_distance == -1)
                    {
                        shortest_club = cd.Key;
                        shortest_distance = cd.Value;
                    }
                    else
                    {
                        if (cd.Value < shortest_distance)
                        {
                            shortest_club = cd.Key;
                            shortest_distance = cd.Value;
                        }
                    }
                }

                var visit = visits.FirstOrDefault(x => x.ClubId == shortest_club.Id);
                if (visit != null)
                {
                    output.IsOut = true;
                    output.ClubId = shortest_club.Id;
                    return output;
                }
                else
                {
                    var ot = shortest_club.OpenTimes.Where(x => x.IsOpen(actualTime)).First();
                    var closingTime = ot.GetActualCloseTime(actualTime);

                    var newVisit = new ClubVisit { ClubId = shortest_club.Id, MemberId = member.Id, EntryDate = actualTime, LeavingDate = closingTime };
                    await _clubVisitRepository.InsertAsync(newVisit);
                    output.IsOut = true;
                    output.ClubId = shortest_club.Id;

                    foreach (var oldVisit in visits)
                    {
                        oldVisit.HasLeft = true;
                        await _clubVisitRepository.UpdateAsync(oldVisit);
                    }
                    return output;
                }
            }
            
            /*foreach (var club in clubs)
            {
                if (club.IsOpen(actualTime) && Geo.Distance(actualPoint, new GeoPoint(club.Latitude, club.Longitude)) <= _allowedDistance)
                {
                    var visit = visits.FirstOrDefault(x => x.ClubId == club.Id);
                    if (visit != null)
                    {
                        output.IsOut = true;
                        output.ClubId = club.Id;
                        return output;
                    }
                    else
                    {
                        var ot = club.OpenTimes.Where(x => x.IsOpen(actualTime)).First();
                        var closingTime = ot.GetActualCloseTime(actualTime);

                        var newVisit = new ClubVisit { ClubId = club.Id, MemberId = member.Id, EntryDate = actualTime, LeavingDate = closingTime };
                        await _clubVisitRepository.InsertAsync(newVisit);
                        output.IsOut = true;
                        output.ClubId = club.Id;

                        foreach (var oldVisit in visits)
                        {
                            oldVisit.HasLeft = true;
                            await _clubVisitRepository.UpdateAsync(oldVisit);
                        }
                        return output;
                    }
                }
            }*/

            foreach (var oldVisit in visits)
            {
                oldVisit.HasLeft = true;
                await _clubVisitRepository.UpdateAsync(oldVisit);
            }
            return output;
        }

        public ClubAppService(IRepository<Club> clubRepository, 
            IRepository<ClubImage> clubImageRepository,
            IMemberRepository memberRepository,
            IRepository<ClubVisit> clubVisitRepository)
        {
            _clubRepository = clubRepository;
            _clubImageRepository = clubImageRepository;
            _memberRepository = memberRepository;
            _clubVisitRepository = clubVisitRepository;
        }

    }
}
