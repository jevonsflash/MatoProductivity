using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.MicroKernel.Registration;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Infrastructure.Helper;
using MatoProductivity.Services;
using MatoProductivity.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace MatoProductivity.ViewModels
{
    public class NoteListPageViewModel : ViewModelBase, ISingletonDependency, ISearchViewModel
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
            this.RemoveSelected = new Command(RemoveSelectedAction);
            this.SelectAll = new Command(SelectAllAction);
            this.Search = new Command(SearchAction);
            SelectedNotes = new ObservableCollection<object>();

            //Init();
        }

        private void SearchAction(object obj)
        {
            this.Init();
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
            foreach (var note in SelectedNotes.ToList())
            {
                foreach (var noteGroup in this.NoteGroups)
                {
                    var delete = noteGroup.FirstOrDefault(c => c.Id == (note as Note).Id);
                    noteGroup.Remove(delete);
                }
            }

        }

        private void RemoveAction(object obj)
        {
            var note = (Note)obj;
            foreach (var noteGroup in this.NoteGroups)
            {
                var delete = noteGroup.FirstOrDefault(c => c.Id == note.Id);
                noteGroup.Remove(delete);
            }

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
            var notes = this.repository.GetAllList()
                .WhereIf(!string.IsNullOrEmpty(this.SearchKeywords), c => c.Title.Contains(this.SearchKeywords));
            var notegroupedlist = notes.GroupBy(c => CommonHelper.FormatTimeString(c.CreationTime, "M月d日")).Select(c => new NoteTimeLineGroup(c.Key, c));
            this.NoteGroups = new ObservableCollection<NoteTimeLineGroup>(notegroupedlist);
            foreach (var noteGroups in this.NoteGroups)
            {
                noteGroups.CollectionChanged += Notes_CollectionChanged;
            }
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

            else if (e.PropertyName == nameof(SearchKeywords))
            {
                if (string.IsNullOrEmpty(SearchKeywords))
                {
                    Init();
                }
            }
        }

        private ObservableCollection<NoteTimeLineGroup> _noteGroups;

        public ObservableCollection<NoteTimeLineGroup> NoteGroups
        {
            get { return _noteGroups; }
            set
            {
                _noteGroups = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Notes));
            }
        }





        public IEnumerable<Note> Notes => NoteGroups.SelectMany(c => c);


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


        private ObservableCollection<object> _selectedNotes;

        public ObservableCollection<object> SelectedNotes
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

        private string _searchKeywords;

        public string SearchKeywords
        {
            get { return _searchKeywords; }
            set
            {
                _searchKeywords = value;
                RaisePropertyChanged();

            }
        }



        public SelectionMode SelectionMode => IsEditing ? SelectionMode.Multiple : SelectionMode.Single;


        public Command Create { get; set; }
        public Command Remove { get; set; }
        public Command Edit { get; set; }
        public Command RemoveSelected { get; set; }
        public Command SelectAll { get; set; }
        public Command Search { get; set; }
    }
}
