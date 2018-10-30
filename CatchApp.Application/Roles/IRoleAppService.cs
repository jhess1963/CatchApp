using System.Threading.Tasks;
using Abp.Application.Services;
using CatchApp.Roles.Dto;

namespace CatchApp.Roles
{
    public interface IRoleAppService : IApplicationService
    {
        Task UpdateRolePermissions(UpdateRolePermissionsInput input);
    }
}
