using MatoProductivity.Core.Models.Entities;

namespace MatoProductivity.Core.ViewModels
{
    public interface INoteSegmentViewModel
    {
        NoteSegment NoteSegment { get; set; }
        Command Submit { get; set; }

        public void SubmitAction(object obj);

    }
}