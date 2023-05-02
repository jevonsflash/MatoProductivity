using Abp.Modules;
using Abp.Reflection.Extensions;
using MatoProductivity.Core;
using MatoProductivity.Core.Services;
using MatoProductivity.EntityFrameworkCore;
using MatoProductivity.ViewModels;

namespace MatoProductivity
{
    [DependsOn(
        typeof(MatoProductivityCoreModule),
        typeof(MatoProductivityEntityFrameworkCoreModule))]
    public class MatoProductivityModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MatoProductivityModule).GetAssembly());
        }

    }
}
