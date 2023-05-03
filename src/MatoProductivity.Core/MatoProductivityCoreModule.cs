using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MatoProductivity.Core.Localization;
using MatoProductivity.Core.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MatoProductivity.Core
{
    [DependsOn(
       typeof(AbpAutoMapperModule))]
    public class MatoProductivityCoreModule : AbpModule
    {
        private readonly string development;

        public MatoProductivityCoreModule()
        {
            development = EnvironmentName.Development;

        }
        public override void PreInitialize()
        {
            LocalizationConfigurer.Configure(Configuration.Localization);

            Configuration.Settings.Providers.Add<CommonSettingProvider>();

            string documentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MatoProductivityConsts.LocalizationSourceName);

            var connectionString = "Data Source=file:mato.db;";

            var dbName = "mato.db";
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), MatoProductivityConsts.LocalizationSourceName, dbName);

            Configuration.DefaultNameOrConnectionString = String.Format(connectionString, dbPath);
            base.PreInitialize();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MatoProductivityCoreModule).GetAssembly());
        }
    }
}
