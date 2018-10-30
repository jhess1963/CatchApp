using System.Data.Entity.Migrations;
using Abp.MultiTenancy;
using Abp.Zero.EntityFramework;
using CatchApp.Migrations.SeedData;
using EntityFramework.DynamicFilters;

namespace CatchApp.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<CatchApp.EntityFramework.CatchAppDbContext>, IMultiTenantSeed
    {
        public AbpTenantBase Tenant { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "CatchApp";
        }

        protected override void Seed(CatchApp.EntityFramework.CatchAppDbContext context)
        {
            context.DisableAllFilters();

            if (Tenant == null)
            {
                //Host seed
                new InitialHostDbBuilder(context).Create();

                //Default tenant seed (in host database).
                new DefaultTenantCreator(context).Create();
                new TenantRoleAndUserBuilder(context, 1).Create();

                new DefaultMemberBuilder(context).Build();
                new DefaultClubBuilder(context).Build();
            }
            else
            {
                //You can add seed for tenant databases and use Tenant property...
            }

            context.SaveChanges();
        }
    }
}
