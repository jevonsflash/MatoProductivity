using MatoProductivity.Core.Services;
using System.Collections.ObjectModel;

namespace MatoProductivity.Core.ViewModels
{
    public interface IReadOnlyNoteSegmentServiceContainer
    {
        ObservableCollection<INoteSegmentService> NoteSegments { get; set; }

    }
}