using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Infrastructure.Helper;
using MatoProductivity.Models;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.EntityFrameworkCore;
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
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IRepository<NoteSegment, long> repository;
        private readonly IIocResolver iocResolver;
        private readonly NavigationService navigationService;

        public StatisticPageViewModel(
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<NoteSegment, long> repository,
            IIocResolver iocResolver,
            NavigationService navigationService)
        {
            this.unitOfWorkManager=unitOfWorkManager;
            this.repository = repository;
            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.PropertyChanged += NotePageViewModel_PropertyChangedAsync;
            this.Search = new Command(SearchAction);
            this.KeyValueStatisticGroups=new ObservableCollection<KeyValueStatisticGroup>();
            SelectedNotes = new ObservableCollection<object>();
            ViewState="Charts";

            //Init();
        }



        private async void SearchAction(object obj)
        {
            await this.Init();
        }


        [UnitOfWork]
        public async Task Init()
        {
            Loading = true;
            await Task.Delay(300);
            await Task.Run(async () =>
            {

                await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {


                    var notes = this.repository.GetAll().Include(c => c.NoteSegmentPayloads)
                    .Where(c => c.Type == "KeyValueSegment")
                     .OrderByDescending(c => c.CreationTime)
                     .ToList();
                    var notegroupedlist = notes
                    .GroupBy(c => GetTitle(c)
                    )
                    .Select(c => new KeyValueStatisticGroup(c.Key, c))
                     .WhereIf(!string.IsNullOrEmpty(this.SearchKeywords), c => c.Title.Contains(this.SearchKeywords))
                    ;
                    this.KeyValueStatisticGroups = new ObservableCollection<KeyValueStatisticGroup>(notegroupedlist);
                });

            }).ContinueWith((e) => { Loading = false; });

        }

        private static string GetTitle(NoteSegment c)
        {
            var result = c.GetNoteSegmentPayload("Title")?.StringValue;
            if (string.IsNullOrEmpty(result))
            {
                result="未分组";

            }
            return result;
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

        private string _viewState;

        public string ViewState
        {
            get { return _viewState; }
            set
            {
                _viewState = value;
                RaisePropertyChanged();
            }
        }

        public Command Search { get; set; }

    }
}
