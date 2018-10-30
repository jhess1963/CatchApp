using System.Data.Entity;
using System.Reflection;
using Abp.Modules;
using CatchApp.EntityFramework;

namespace CatchApp.Migrator
{
    [DependsOn(typeof(CatchAppDataModule))]
    public class CatchAppMigratorModule : AbpModule
    {
        public override void PreInitialize()
        {
            Database.SetInitializer<CatchAppDbContext>(null);

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}