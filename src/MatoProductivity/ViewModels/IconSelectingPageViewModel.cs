using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Services;
using System.Reflection;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace MatoProductivity.ViewModels
{
    public class IconSelectingPageViewModel : PopupViewModelBase, ISingletonDependency, ISearchViewModel
    {
        private readonly IIocResolver iocResolver;
        private readonly NavigationService navigationService;

        public event EventHandler<string> OnFinishedChooise;

        public IconSelectingPageViewModel(
            IIocResolver iocResolver,
            NavigationService navigationService

            )
        {
            this.Search = new Command(SearchAction);

            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.PropertyChanged += IconPageViewModel_PropertyChangedAsync;
        }

        private async void SearchAction(object obj)
        {
            await this.Init();
        }

        public async Task Init()
        {
            if (this.IconImages!=default)
            {
                return;
            }
            Loading = true;
            await Task.Delay(300);
            await Task.Run(() =>
            {
                this.IconImages = new ObservableCollection<string>() {
                    "automobile",
                    "baby",
                    "diet",
                    "doctor",
                    "emptylist",
                    "medicine",
                    "motherhood",
                    "speed_test",
                    "step_to_the_sun",
                    "teddy_bear",};
            }).ContinueWith((e) => { Loading = false; });

        }

        private async void IconPageViewModel_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedIcon))
            {
                if (SelectedIcon != default)
                {


                    OnFinishedChooise?.Invoke(this, this.SelectedIcon);

                    SelectedIcon = default;
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


        private ObservableCollection<string> _iconImages;

        public ObservableCollection<string> IconImages
        {
            get { return _iconImages; }
            set
            {
                _iconImages = value;
                RaisePropertyChanged();
            }
        }

        private string _selectedIcon;

        public string SelectedIcon
        {
            get { return _selectedIcon; }
            set
            {
                _selectedIcon = value;
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
