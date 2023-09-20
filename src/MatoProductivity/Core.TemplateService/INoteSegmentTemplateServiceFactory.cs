using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;

namespace MatoProductivity.Core.Services
{
    public interface INoteSegmentTemplateServiceFactory: ISingletonDependency
    {
        INoteSegmentService GetNoteSegmentService(NoteSegmentTemplate noteSegmentTemplate);
    }
}