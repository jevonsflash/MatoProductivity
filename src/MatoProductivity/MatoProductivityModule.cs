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
        public override async void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MatoProductivityModule).GetAssembly());
            if (VersionTracking.Default.IsFirstLaunchEver)
            {
                MatoProductivityEntityFrameworkCoreModule.SkipDbSeed = false;
                var appAction = new AppAction("create_note", "创建新笔记", icon: "app_info_action_icon");
                await AppActions.Current.SetAsync([appAction]);
            }
            else
            {
                MatoProductivityEntityFrameworkCoreModule.SkipDbSeed = true;

            }
        }

        public override void PostInitialize()
        {
            base.PostInitialize();
        }

    }
}
