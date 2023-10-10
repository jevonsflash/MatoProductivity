using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Castle.MicroKernel.Registration;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Infrastructure.Helper;
using MatoProductivity.Models;
using MatoProductivity.Services;
using MatoProductivity.Views;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Common;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MatoProductivity.ViewModels
{
    public class StatisticPageViewModel : ViewModelBase, ISingletonDependency
    {
        private readonly IRepository<NoteSegment, long> repository;
        private readonly IIocResolver iocResolver;
        private readonly NavigationService navigationService;

        public StatisticPageViewModel(
            IRepository<NoteSegment, long> repository,
            IIocResolver iocResolver,
            NavigationService navigationService)
        {
            this.repository = repository;
            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.PropertyChanged += NotePageViewModel_PropertyChangedAsync;
            this.Search = new Command(SearchAction);

            SelectedNotes = new ObservableCollection<object>();

            //Init();
        }
  

        private async void SearchAction(object obj)
        {
            await this.Init();
        }



        public async Task Init()
        {
            Loading = true;
            await Task.Delay(300);
            await Task.Run(() =>
            {
                var notes = this.repository.GetAllList()
                .Where(c => c.Type == "KeyValueSegment")
                .WhereIf(!string.IsNullOrEmpty(this.SearchKeywords), c => c.Title.Contains(this.SearchKeywords));
                var notegroupedlist = notes.OrderByDescending(c => c.CreationTime)
                .GroupBy(c => c.Title.Trim()
                ).Select(c => new KeyValueStatisticGroup(c.Key, c));
                this.KeyValueStatisticGroups = new ObservableCollection<KeyValueStatisticGroup>(notegroupedlist);

                foreach (var keyValueStatisticGroups in this.KeyValueStatisticGroups)
                {
                    keyValueStatisticGroups.CollectionChanged += Notes_CollectionChanged;
                }
            }).ContinueWith((e) => { Loading = false; });

        }


        private bool _loading;

        public bool Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                RaisePropertyChanged();

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

                    SelectedNote = default;
                }
            }

            else if (e.PropertyName == nameof(SearchKeywords))
            {
                if (string.IsNullOrEmpty(SearchKeywords))
                {
                    await Init();
                }
            }
        }



        private ObservableCollection<KeyValueStatisticGroup> _keyValueStatisticGroups;

        public ObservableCollection<KeyValueStatisticGroup> KeyValueStatisticGroups
        {
            get { return _keyValueStatisticGroups; }
            set
            {
                _keyValueStatisticGroups = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(keyValueStatistices));
            }
        }





        public IEnumerable<NoteSegment> keyValueStatistices => KeyValueStatisticGroups.SelectMany(c => c);


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
