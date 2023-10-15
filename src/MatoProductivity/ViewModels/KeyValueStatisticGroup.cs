using MatoProductivity.Core.Models.Entities;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{

    public class NoteStatistic
    {
        public string Title { get;  set; }

        public ICollection<KeyValueStatisticGroup> KeyValueStatisticGroups { get; set; }
        public ICollection<DateTime> CreationTimes { get; set; }
    }

    public class KeyValueStatisticGroup : ObservableCollection<NoteSegment>
    {
        public string Title { get; private set; }

        public KeyValueStatisticGroup(string title, IEnumerable<NoteSegment> keyValueSegments) : base(keyValueSegments)
        {
            Title = title;
        }
    }
}
