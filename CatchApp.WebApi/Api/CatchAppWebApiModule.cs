using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using System.Linq;
using Abp.Modules;
using Abp.WebApi;
using Abp.WebApi.Controllers.Dynamic.Builders;

namespace CatchApp.Api
{
    [DependsOn(typeof(AbpWebApiModule), typeof(CatchAppApplicationModule))]
    public class CatchAppWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(CatchAppApplicationModule).Assembly, "app")
                .Build();

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));

            //ConfigureSwaggerUi();
        }

        //private void ConfigureSwaggerUi()
        //{
        //    Configuration.Modules.AbpWebApi().HttpConfiguration
        //        .EnableSwagger(c =>
        //        {
        //            c.SingleApiVersion("V1", "CatchApp.WebApi");
        //            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
        //        })
        //        .EnableSwaggerUi();
        //}
    }
}
