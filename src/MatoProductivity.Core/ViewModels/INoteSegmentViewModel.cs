using MatoProductivity.Core.Models.Entities;

namespace MatoProductivity.Core.ViewModels
{
    public interface INoteSegmentViewModel
    {
        NoteSegment NoteSegment { get; set; }
        Command Submit { get; set; }
        Command Create { get; set; }

        void SubmitAction(object obj);
        void CreateAction(object obj);

    }
}