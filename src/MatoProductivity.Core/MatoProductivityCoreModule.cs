using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MatoProductivity.Core.Localization;
using MatoProductivity.Core.Models.Entities;
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



            Configuration.Modules.AbpAutoMapper().Configurators.Add(config =>
            {
                config.CreateMap<NoteTemplate, Note>()
                .ForMember(
                    c => c.NoteSegments,
                    options => options.MapFrom(input => input.NoteSegmentTemplates))              
                  .ForMember(
                    c => c.Id,
                    options => options.Ignore());


                config.CreateMap<Note, NoteTemplate>()
                   .ForMember(
                       c => c.NoteSegmentTemplates,
                       options => options.MapFrom(input => input.NoteSegments))
                  .ForMember(
                    c => c.Id,
                    options => options.Ignore());


                config.CreateMap<NoteSegmentTemplate, NoteSegment>()
                .ForMember(
                    c => c.Note,
                    options => options.MapFrom(input => input.NoteTemplate))
                .ForMember(
                    c => c.NoteSegmentPayloads,
                    options => options.MapFrom(input => input.NoteSegmentTemplatePayloads))
                 .ForMember(
                    c => c.NoteId,
                    options => options.MapFrom(input => input.NoteTemplateId))
                  .ForMember(
                    c => c.Id,
                    options => options.Ignore());

                config.CreateMap<NoteSegment, NoteSegmentTemplate>()
                   .ForMember(
                    c => c.NoteTemplate,
                    options => options.MapFrom(input => input.Note))
                   .ForMember(
                    c => c.NoteTemplateId,
                    options => options.MapFrom(input => input.NoteId))
                   .ForMember(
                       c => c.NoteSegmentTemplatePayloads,
                       options => options.MapFrom(input => input.NoteSegmentPayloads))
                  .ForMember(
                    c => c.Id,
                    options => options.Ignore());

                config.CreateMap<NoteSegmentTemplatePayload, NoteSegmentPayload>()
                   .ForMember(
                       c => c.NoteSegment,
                       options => options.MapFrom(input => input.NoteSegmentTemplate))
                   .ForMember(
                    c => c.NoteSegmentId,
                    options => options.MapFrom(input => input.NoteSegmentTemplateId))
                  .ForMember(
                    c => c.Id,
                    options => options.Ignore());


                config.CreateMap<NoteSegmentPayload, NoteSegmentTemplatePayload>()
                   .ForMember(
                       c => c.NoteSegmentTemplate,
                       options => options.MapFrom(input => input.NoteSegment))
                   .ForMember(
                    c => c.NoteSegmentTemplateId,
                    options => options.MapFrom(input => input.NoteSegmentId))
                  .ForMember(
                    c => c.Id,
                    options => options.Ignore());

            });


            base.PreInitialize();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MatoProductivityCoreModule).GetAssembly());
        }
    }
}
