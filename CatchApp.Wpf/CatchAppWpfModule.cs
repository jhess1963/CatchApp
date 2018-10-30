using Abp.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CatchApp.Wpf
{
    [DependsOn(typeof(CatchAppDataModule), typeof(CatchAppApplicationModule))]
    public class CatchAppWpfModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
