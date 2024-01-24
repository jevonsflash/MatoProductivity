using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class UserProfilePageViewModel : ViewModelBase, ISingletonDependency, IPopupContainerViewModelBase
    {

        private Popup shortCutSettingPage;
        private readonly NavigationService navigationService;
        private readonly IRepository<Setting, string> settingRepository;
        private readonly IIocResolver iocResolver;
        public UserProfilePageViewModel(
            NavigationService navigationService,
            IRepository<Setting, string> settingRepository,
            IIocResolver iocResolver
            )
        {
            AppActionSetting = new Command(AppActionSettingAction, (o) => !PopupLoading);
            this.navigationService=navigationService;
            this.settingRepository=settingRepository;
            this.iocResolver=iocResolver;
            PropertyChanged+=UserProfilePageViewModel_PropertyChanged;
            Init();
        }

        private void UserProfilePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Theme))
            {
                if (!string.IsNullOrEmpty(Theme))
                {
                    this.settingRepository.Update(nameof(Theme), c => c.Value=Theme);

                }
            }

            else if (e.PropertyName == nameof(DetailPageMode))
            {
                if (!string.IsNullOrEmpty(DetailPageMode))
                {
                    this.settingRepository.Update(nameof(DetailPageMode), c => c.Value=DetailPageMode);

                }
            }

            else if (e.PropertyName == nameof(IsDetailPreviewPageMode))
            {
                DetailPageMode=IsDetailPreviewPageMode ? "PreviewPage" : "EditPage";
            }
        }

        private string _theme;


        public string Theme
        {
            get { return _theme; }
            set
            {
                _theme = value;
                RaisePropertyChanged();

            }
        }

        private string _detailPageMode;


        public string DetailPageMode
        {
            get { return _detailPageMode; }
            set
            {
                _detailPageMode = value;
                RaisePropertyChanged();

            }
        }


        private bool _isDetailPreviewPageMode;


        public bool IsDetailPreviewPageMode
        {
            get { return _isDetailPreviewPageMode; }
            set
            {
                _isDetailPreviewPageMode = value;
                RaisePropertyChanged();

            }
        }

        private async void Init()
        {
            var settings = settingRepository.GetAllList();
            this.Theme=settings.FirstOrDefault(c => c.Id==nameof(Theme))?.Value;
            this.DetailPageMode=settings.FirstOrDefault(c => c.Id==nameof(DetailPageMode))?.Value;
            if (DetailPageMode=="PreviewPage")
            {
                this.IsDetailPreviewPageMode=true;
            }
        }

        private async void AppActionSettingPageViewModel_OnFinishedChooise(object sender, AppAction appAction)
        {

            await navigationService.HidePopupAsync(shortCutSettingPage);
        }

        private async void AppActionSettingAction(object obj)
        {
            PopupLoading = true;
            AppActionSetting.ChangeCanExecute();

            using (var objWrapper = iocResolver.ResolveAsDisposable<AppActionSettingPage>())
            {
                shortCutSettingPage = objWrapper.Object;
                (shortCutSettingPage.BindingContext as AppActionSettingPageViewModel).OnFinishedChooise += AppActionSettingPageViewModel_OnFinishedChooise;
            }

            await navigationService.ShowPopupAsync(shortCutSettingPage).ContinueWith(async (e) =>
            {
                (shortCutSettingPage.BindingContext as AppActionSettingPageViewModel).OnFinishedChooise -= AppActionSettingPageViewModel_OnFinishedChooise;
                shortCutSettingPage = null;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PopupLoading = false;
                    AppActionSetting.ChangeCanExecute();
                });

            }); ;

        }


        private bool _popupLoading;


        public bool PopupLoading
        {
            get { return _popupLoading; }
            set
            {
                _popupLoading = value;
                RaisePropertyChanged();

            }
        }
        public async Task CloseAllPopup()
        {
            await navigationService.HidePopupAsync(shortCutSettingPage);
        }


        public Command AppActionSetting { get; set; }


    }






}
