using CatchApp.EntityFramework;
using EntityFramework.DynamicFilters;

namespace CatchApp.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly CatchAppDbContext _context;

        public InitialHostDbBuilder(CatchAppDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
