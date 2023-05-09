using MatoProductivity.Core.Models.Entities;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class NoteTimeLineGroup : ObservableCollection<Note>
    {
        public string TimeCategory { get; private set; }

        public NoteTimeLineGroup(string category, IEnumerable<Note> Notes) : base(Notes)
        {
            TimeCategory = category;
        }
    }
}
