using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;

namespace MatoProductivity.Core.Services
{
    public interface INoteSegmentServiceFactory: ISingletonDependency
    {
        INoteSegmentService GetNoteSegmentService(NoteSegment noteSegment);
    }
}