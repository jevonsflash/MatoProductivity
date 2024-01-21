using System.Collections.ObjectModel;

namespace MatoProductivity.Core.Services
{
    public interface INoteSegmentServiceContainer : IReadOnlyNoteSegmentServiceContainer
    {
        Command CreateSegment { get; set; }
        NoteSegmentState NoteSegmentState { get; set; }
        Command RemoveSegment { get; set; }
    }
}