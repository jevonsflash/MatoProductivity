using MatoProductivity.Core.Models.Entities;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class KeyValueStatisticGroup : ObservableCollection<NoteSegment>
    {
        public string Title { get; private set; }

        public KeyValueStatisticGroup(string title, IEnumerable<NoteSegment> keyValueSegments) : base(keyValueSegments)
        {
            Title = title;
        }
    }
}
