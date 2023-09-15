using Abp.Modules;
using MatoProductivity.Core;

namespace MatoProductivity
{
    public static class MatoProductivityBuilderExtensions
    {

        public static MauiAppBuilder UseMatoProductivity<TStartupModule>(this MauiAppBuilder builder) where TStartupModule : AbpModule
        {
            builder.Services.AddMatoProductivityService<TStartupModule>();
            return builder;
        }
    }
}