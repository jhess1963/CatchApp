using Abp.Application.Services;
using Abp.Authorization;

using CatchApp.Geodata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CatchApp.Clubs
{
    public interface IClubAppService : IApplicationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="actualPoint"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        Task<SearchClubsOutput> SearchClubs(SearchClubsInput input);

        SearchClubsOutput GetClubs();

        GetClubOutput GetClub(GetClubInput input);

        Task<CheckVisitOutput> CheckVisit(CheckVisitInput input);

        Task UpdateClubImage(UpdateClubImageInput input);

        Task<GetClubImageOutput> GetClubImage(GetClubImageInput input);

        Task LoadClubImages();
    }
}
