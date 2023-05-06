using MatoProductivity.Core.Models.Entities;

namespace MatoProductivity.Core.Services
{
    public interface INoteSegmentService
    {
        NoteSegment NoteSegment { get; set; }
        Command Submit { get; set; }
        Command Create { get; set; }
        NoteSegmentState NoteSegmentState { get; set; }

        void SubmitAction(object obj);
        void CreateAction(object obj);

    }
}