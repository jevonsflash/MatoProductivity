using Abp.Modules;
using Abp.Reflection.Extensions;
using MatoProductivity.Core;
using MatoProductivity.EntityFrameworkCore;
using MatoProductivity.ViewModels;
using MatoProductivity.Views;

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

        public override void PostInitialize()
        {
            Routing.RegisterRoute(nameof(EditNotePage), typeof(EditNotePage));
            base.PostInitialize();
        }

    }
}
