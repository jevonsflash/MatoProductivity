using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Views;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class NotePageViewModel : ViewModelBase, ITransientDependency
    {
        private readonly IRepository<Note, long> repository;

        public NotePageViewModel(IRepository<Note, long> repository)
        {
            this.repository = repository;
            this.PropertyChanged += NotePageViewModel_PropertyChangedAsync;
            var notes = this.repository.GetAllList();
            this.Notes = new ObservableCollection<Note>(notes);
        }

        private async void NotePageViewModel_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedNote))
            {
                if (SelectedNote != default)
                {
                    var navigationParameter = new Dictionary<string, object>
                    {
                        { "NoteId", SelectedNote.Id }
                    };
                    await Shell.Current.GoToAsync(nameof(EditNotePage), navigationParameter);

                }
            }
        }
        private ObservableCollection<Note> _notes;

        public ObservableCollection<Note> Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                RaisePropertyChanged();
            }
        }

        private Note _selectedNote;

        public Note SelectedNote
        {
            get { return _selectedNote; }
            set { _selectedNote = value;
                RaisePropertyChanged();

            }
        }



    }
}
