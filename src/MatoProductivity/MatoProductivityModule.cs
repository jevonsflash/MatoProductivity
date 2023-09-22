using Abp.Modules;
using Abp.Reflection.Extensions;
using MatoProductivity.Core;
using MatoProductivity.EntityFrameworkCore;
using MatoProductivity.ViewModels;
using MatoProductivity.Views;
using Microsoft.Maui.Controls.Maps;

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
            base.PostInitialize();
        }

    }
}
