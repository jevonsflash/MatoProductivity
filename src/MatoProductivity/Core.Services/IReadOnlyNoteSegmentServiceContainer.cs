using System.Collections.ObjectModel;

namespace MatoProductivity.Core.Services
{
    public interface IReadOnlyNoteSegmentServiceContainer
    {
        ObservableCollection<INoteSegmentService> NoteSegments { get; set; }

    }
}