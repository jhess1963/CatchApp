using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using CatchApp.Authorization.Roles;
using CatchApp.Editions;
using CatchApp.Users;

namespace CatchApp.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, Role, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore
            ) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore
            )
        {
        }
    }
}