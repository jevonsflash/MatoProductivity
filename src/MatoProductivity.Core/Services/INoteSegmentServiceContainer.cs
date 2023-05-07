using MatoProductivity.Core.Services;
using System.Collections.ObjectModel;

namespace MatoProductivity.Core.ViewModels
{
    public interface INoteSegmentServiceContainer
    {
        Command CreateSegment { get; set; }
        bool IsConfiguratingNoteSegment { get; set; }
        ObservableCollection<INoteSegmentService> NoteSegments { get; set; }
        Command RemoveSegment { get; set; }
    }
}