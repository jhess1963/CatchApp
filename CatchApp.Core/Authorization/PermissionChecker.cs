using Abp.Authorization;
using CatchApp.Authorization.Roles;
using CatchApp.MultiTenancy;
using CatchApp.Users;

namespace CatchApp.Authorization
{
    public class PermissionChecker : PermissionChecker<Tenant, Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
