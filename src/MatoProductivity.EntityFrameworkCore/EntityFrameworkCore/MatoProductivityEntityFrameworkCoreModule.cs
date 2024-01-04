using System;
using System.Transactions;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.EntityFrameworkCore.Configuration;
using Abp.EntityFrameworkCore.Uow;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MatoProductivity.Core;
using MatoProductivity.EntityFrameworkCore.Seed;
using Microsoft.EntityFrameworkCore;

namespace MatoProductivity.EntityFrameworkCore
{
    [DependsOn(
        typeof(MatoProductivityCoreModule),
        typeof(AbpEntityFrameworkCoreModule))]
    public class MatoProductivityEntityFrameworkCoreModule : AbpModule
    {

        public static bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            Configuration.Modules.AbpEfCore().AddDbContext<MatoProductivityDbContext>(options =>
            {
                if (options.ExistingConnection != null)
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                }
                else
                {
                    DbContextOptionsConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                }
            });
        }
        public override void Initialize()
        {

            IocManager.RegisterAssemblyByConvention(typeof(MatoProductivityEntityFrameworkCoreModule).GetAssembly());

        }

        public override void PostInitialize()
        {
            Helper.WithDbContextHelper.WithDbContext<MatoProductivityDbContext>(IocManager, RunMigrate);
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }

        public static void RunMigrate(MatoProductivityDbContext dbContext)
        {
            dbContext.Database.Migrate();
        }


    }
}