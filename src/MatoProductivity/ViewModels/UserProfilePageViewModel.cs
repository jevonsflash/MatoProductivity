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
using System.Diagnostics;

namespace MatoProductivity.ViewModels
{
    public class UserProfilePageViewModel : ViewModelBase, ISingletonDependency, IPopupContainerViewModelBase
    {

        private Popup shortCutSettingPage;
        private readonly NavigationService navigationService;
        private readonly IRepository<Setting, string> settingRepository;
        private readonly IIocResolver iocResolver;
        private AboutMePage aboutMePage;
        private int clickCount;
        readonly Stopwatch clickStopwatch = new Stopwatch();

        public UserProfilePageViewModel(
            NavigationService navigationService,
            IRepository<Setting, string> settingRepository,
            IIocResolver iocResolver
            )
        {
            AppActionSetting = new Command(AppActionSettingAction, (o) => !PopupLoading);
            AboutMe = new Command(AboutMeAction, (o) => !PopupLoading);
            PrivacyPolicy = new Command(PrivacyPolicyAction, (o) => !PopupLoading);
            ThirdPartyLicenses = new Command(ThirdPartyLicensesAction, (o) => !PopupLoading);
            Version = new Command(VersionAction, (o) => !PopupLoading);
            this.navigationService=navigationService;
            this.settingRepository=settingRepository;
            this.iocResolver=iocResolver;
            PropertyChanged+=UserProfilePageViewModel_PropertyChanged;
            Init();
        }

        private async void ThirdPartyLicensesAction(object obj)
        {
            var objWrapper = iocResolver.ResolveAsDisposable<ThirdPartyLicensesPage>();
            await navigationService.PushAsync(objWrapper.Object);
        }

        private async void VersionAction(object obj)
        {
            var currentCircle = clickStopwatch.ElapsedMilliseconds;
            if (currentCircle<500)
            {
                clickCount++;
            }
            if (clickCount>3)
            {
                clickCount=0;
                using (var objWrapper = iocResolver.ResolveAsDisposable<E>())
                {
                    await navigationService.ShowPopupAsync(objWrapper.Object);
                }

            }
            clickStopwatch.Restart();
        }

        private async void PrivacyPolicyAction(object obj)
        {
            var objWrapper = iocResolver.ResolveAsDisposable<PrivacyPolicyPage>();
            await navigationService.PushAsync(objWrapper.Object);
        }

        private async void AboutMeAction(object obj)
        {
            using (var objWrapper = iocResolver.ResolveAsDisposable<AboutMePage>())
            {
                aboutMePage = objWrapper.Object;
                await navigationService.ShowPopupAsync(aboutMePage);
            }
        }

        private void UserProfilePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Theme))
            {
                if (!string.IsNullOrEmpty(Theme))
                {
                    if (Theme=="Dark")
                    {
                        Application.Current.UserAppTheme = AppTheme.Dark;
                    }
                    else if (Theme=="Light")
                    {
                        Application.Current.UserAppTheme = AppTheme.Light;
                    }
                    else
                    {
                        Application.Current.UserAppTheme = AppTheme.Unspecified;
                    }
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
        public Command AboutMe { get; set; }
        public Command PrivacyPolicy { get; set; }
        public Command ThirdPartyLicenses { get; set; }
        public Command Version { get; set; }


    }






}
