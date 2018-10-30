using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Timing;

namespace CatchApp
{
    [DependsOn(typeof(CatchAppCoreModule), typeof(AbpAutoMapperModule))]
    public class CatchAppApplicationModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

        }
    }
}
