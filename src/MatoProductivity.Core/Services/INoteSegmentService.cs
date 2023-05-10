using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;

namespace MatoProductivity.Core.Services
{
    public interface INoteSegmentService:IDraggableItem
    {
        NoteSegment NoteSegment { get; set; }
        Command Submit { get; set; }
        Command Create { get; set; }
        NoteSegmentState NoteSegmentState { get; set; }
        IReadOnlyNoteSegmentServiceContainer Container { get; set; }

        void SubmitAction(object obj);
        void CreateAction(object obj);

    }
}