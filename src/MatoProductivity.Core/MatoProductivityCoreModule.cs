using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MatoProductivity.Core.Configuration;
using MatoProductivity.Core.Localization;
using MatoProductivity.Core.Services;
using MatoProductivity.Core.Settings;
using MatoProductivity.Core.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

            var configuration = AppConfigurations.Get(documentsPath, development);
            var connectionString = configuration.GetConnectionString(MatoProductivityConsts.ConnectionStringName);

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
