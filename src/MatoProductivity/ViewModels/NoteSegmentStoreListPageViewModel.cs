using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Services;
using MatoProductivity.Views;
using System;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class NoteSegmentStoreListPageViewModel : ViewModelBase, ISingletonDependency, ISearchViewModel
    {
        private readonly IRepository<NoteSegmentStore, long> repository;
        private readonly IIocResolver iocResolver;
        private readonly NavigationService navigationService;

        public event EventHandler<NoteSegment> OnFinishedChooise;

        public NoteSegmentStoreListPageViewModel(
            IRepository<NoteSegmentStore, long> repository,
            IIocResolver iocResolver,
            NavigationService navigationService

            )
        {
            this.Search = new Command(SearchAction);

            this.repository = repository;
            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.PropertyChanged += NoteSegmentStorePageViewModel_PropertyChangedAsync;
            Init();
        }

        private void SearchAction(object obj)
        {
            this.Init();
        }

        public void Init()
        {
            var noteSegmentStores = this.repository.GetAllList()
                .WhereIf(!string.IsNullOrEmpty(this.SearchKeywords), c => c.Title.Contains(this.SearchKeywords));

            var noteSegmentStoreGroups = noteSegmentStores.GroupBy(c => c.Category).Select(c => new NoteSegmentStoreGroup(c.Key, c));
            this.NoteSegmentStoreGroups = new ObservableCollection<NoteSegmentStoreGroup>(noteSegmentStoreGroups);
        }

        private async void NoteSegmentStorePageViewModel_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedNoteSegmentStore))
            {
                if (SelectedNoteSegmentStore != default)
                {

                    var noteSegment = ObjectMapper.Map<NoteSegment>(this.SelectedNoteSegmentStore);

                    OnFinishedChooise?.Invoke(this, noteSegment);

                    SelectedNoteSegmentStore = default;
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
        private ObservableCollection<NoteSegmentStoreGroup> _noteSegmentStoreGroups;

        public ObservableCollection<NoteSegmentStoreGroup> NoteSegmentStoreGroups
        {
            get { return _noteSegmentStoreGroups; }
            set
            {
                _noteSegmentStoreGroups = value;
                RaisePropertyChanged();
            }
        }

        private NoteSegmentStore _selectedNoteSegmentStore;

        public NoteSegmentStore SelectedNoteSegmentStore
        {
            get { return _selectedNoteSegmentStore; }
            set
            {
                _selectedNoteSegmentStore = value;
                RaisePropertyChanged();

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

        public Command Search { get; set; }

    }

}
