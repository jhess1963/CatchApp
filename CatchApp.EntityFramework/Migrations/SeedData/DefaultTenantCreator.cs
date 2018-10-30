using System.Linq;
using CatchApp.EntityFramework;
using CatchApp.MultiTenancy;

namespace CatchApp.Migrations.SeedData
{
    public class DefaultTenantCreator
    {
        private readonly CatchAppDbContext _context;

        public DefaultTenantCreator(CatchAppDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateUserAndRoles();
        }

        private void CreateUserAndRoles()
        {
            //Default tenant

            var defaultTenant = _context.Tenants.FirstOrDefault(t => t.TenancyName == "Default");
            if (defaultTenant == null)
            {
                _context.Tenants.Add(new Tenant {TenancyName = "Default", Name = "Default"});
                _context.SaveChanges();
            }
        }
    }
}