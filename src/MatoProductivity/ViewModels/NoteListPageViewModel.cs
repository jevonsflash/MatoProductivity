using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Services;
using MatoProductivity.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

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
            this.Create = new Command(CreateActionAsync);
            this.Edit = new Command(EditAction);
            this.Remove = new Command(RemoveAction);
            this.RemoveSelected=new Command(RemoveSelectedAction);
            this.SelectAll=new Command(SelectAllAction);
            SelectedNotes = new ObservableCollection<Note>();

            //Init();
        }

        private void SelectAllAction(object obj)
        {
            foreach (var notes in Notes)
            {
                SelectedNotes.Add(notes);
            }
        }

        private void RemoveSelectedAction(object obj)
        {
            foreach (var notes in SelectedNotes.ToList())
            {
                Notes.Remove((Note)notes);
            }
        }

        private void RemoveAction(object obj)
        {
            var note = (Note)obj;
            Notes.Remove(note);


        }

        private async void EditAction(object obj)
        {
            var note = (Note)obj;

            using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = note.Id }))
            {
                await navigationService.PushAsync(objWrapper.Object);
            }
        }

        private async void CreateActionAsync(object obj)
        {

            using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = 0 }))
            {
                (objWrapper.Object.BindingContext as EditNotePageViewModel).Create.Execute(null);

                await navigationService.PushAsync(objWrapper.Object);
            }
        }

        public void Init()
        {
            var notes = this.repository.GetAllList();
            this.Notes = new ObservableCollection<Note>(notes);
            this.Notes.CollectionChanged += Notes_CollectionChanged;
        }

        private async void Notes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var item in e.OldItems)
                {
                    await this.repository.DeleteAsync((item as Note).Id);

                }
            }
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


        private ObservableCollection<Note> _selectedNotes;

        public ObservableCollection<Note> SelectedNotes
        {
            get { return _selectedNotes; }
            set
            {
                _selectedNotes = value;
                RaisePropertyChanged();

            }
        }

        private bool _isEditing;

        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectionMode));

            }
        }



        public SelectionMode SelectionMode => IsEditing ? SelectionMode.Multiple : SelectionMode.Single;


        public Command Create { get; set; }
        public Command Remove { get; set; }
        public Command Edit { get; set; }
        public Command RemoveSelected { get; set; }
        public Command SelectAll { get; set; }
    }
}
