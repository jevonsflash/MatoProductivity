using MatoProductivity.Core.Models.Entities;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class NoteSegmentStoreGroup : ObservableCollection<NoteSegmentStore>
    {
        public string Category { get; private set; }

        public NoteSegmentStoreGroup(string category, IEnumerable<NoteSegmentStore> NoteSegmentStores) : base(NoteSegmentStores)
        {
            Category = category;
        }
    }






}
