using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace MatoProductivity.ViewModels
{
    public class AppActionSettingPageViewModel : PopupViewModelBase, ISingletonDependency, ISearchViewModel
    {
        private readonly IRepository<NoteTemplate, long> repository;
        private readonly IIocResolver iocResolver;
        private readonly NavigationService navigationService;

        public event EventHandler<AppAction> OnFinishedChooise;

        public AppActionSettingPageViewModel(
            IRepository<NoteTemplate, long> repository,
            IIocResolver iocResolver,
            NavigationService navigationService

            )
        {
            this.Search = new Command(SearchAction);
            this.Remove = new Command(RemoveAction);
            this.Add = new Command(AddAction);

            this.repository=repository;
            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.PropertyChanged += AppActionPageViewModel_PropertyChangedAsync;
        }

        private void AddAction(object obj)
        {
            this.AvailableAppActions.Remove(obj as AppAction);
            this.AppActions.Add(obj as AppAction);
        }

        private void RemoveAction(object obj)
        {
            this.AppActions.Remove(obj as AppAction);
            this.AvailableAppActions.Add(obj as AppAction);
        }

        private async void SearchAction(object obj)
        {
            await this.Init();
        }

        public async Task Init()
        {

            Loading = true;
            await Task.Delay(300);
            await Task.Run(async () =>
            {
                var allAppActions = await Microsoft.Maui.ApplicationModel.AppActions.Current.GetAsync();
                var existAppActionIds = new List<long>();
                var appActions = new List<AppAction>();
                foreach (var appAction in appActions)
                {
                    if (long.TryParse(appAction.Id, out var id))
                    {
                        appActions.Add(appAction);
                        existAppActionIds.Add(id);
                    }
                }
                var availableAppActions = this.repository.GetAllList()
                .Where(c => c.CanSimplified)
                .Where(c => !existAppActionIds.Contains(c.Id))
                .Select(c => new AppAction(c.Id.ToString(), c.Title))
                .ToList();

                this.AppActions=new ObservableCollection<AppAction>(appActions);
                this.AvailableAppActions=new ObservableCollection<AppAction>(availableAppActions);

            }).ContinueWith((e) => { Loading = false; });

        }

        private async void AppActionPageViewModel_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedAppAction))
            {
                if (SelectedAppAction != default)
                {


                    OnFinishedChooise?.Invoke(this, this.SelectedAppAction);

                    SelectedAppAction = default;
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


        private ObservableCollection<AppAction> _appActions;

        public ObservableCollection<AppAction> AppActions
        {
            get { return _appActions; }
            set
            {
                _appActions = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<AppAction> _availableAppActions;

        public ObservableCollection<AppAction> AvailableAppActions
        {
            get { return _availableAppActions; }
            set
            {
                _availableAppActions = value;
                RaisePropertyChanged();
            }
        }


        private AppAction _selectedAppAction;

        public AppAction SelectedAppAction
        {
            get { return _selectedAppAction; }
            set
            {
                _selectedAppAction = value;
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
        public Command Remove { get; set; }
        public Command Add { get; set; }

    }

}
