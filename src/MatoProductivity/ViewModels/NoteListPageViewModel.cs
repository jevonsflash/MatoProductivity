using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Services;
using MatoProductivity.Views;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class NoteListPageViewModel : ViewModelBase, ISingletonDependency
    {
        private readonly IRepository<Note, long> repository;
        private readonly IIocResolver iocResolver;
        private readonly NavigationService navigationService;

        public NoteListPageViewModel(
            IRepository<Note, long> repository,
            IIocResolver iocResolver,
            NavigationService navigationService)
        {
            this.repository = repository;
            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.PropertyChanged += NotePageViewModel_PropertyChangedAsync;
            //Init();
        }

        public void Init()
        {
            var notes = this.repository.GetAllList();
            this.Notes = new ObservableCollection<Note>(notes);
        }

        private async void NotePageViewModel_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedNote))
            {
                if (SelectedNote != default)
                {
                   

                    using (var objWrapper = iocResolver.ResolveAsDisposable<NotePage>(new { NoteId = SelectedNote.Id }))
                    {
                        await navigationService.PushAsync(objWrapper.Object);
                    }

                    SelectedNote = default;
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
            set
            {
                _selectedNote = value;
                RaisePropertyChanged();

            }
        }



    }
}
