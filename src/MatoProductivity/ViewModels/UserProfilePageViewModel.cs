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
    public class UserProfilePageViewModel : ViewModelBase, ISingletonDependency
    {

        private Popup shortCutSettingPage;
        private readonly NavigationService navigationService;
        private readonly IIocResolver iocResolver;
        public UserProfilePageViewModel(
            NavigationService navigationService,
            IIocResolver iocResolver
            )
        {
            AppActionSetting = new Command(AppActionSettingAction, (o) => !PopupLoading);
            this.navigationService=navigationService;
            this.iocResolver=iocResolver;

            //Init();
        }

        private async void AppActionSettingPageViewModel_OnFinishedChooise(object sender, AppAction appAction)
        {

            await navigationService.HidePopupAsync(shortCutSettingPage);
        }

        private async void AppActionSettingAction(object obj)
        {
            PopupLoading = true;
            AppActionSetting.ChangeCanExecute();
            await Task.Run(() =>
            {
                using (var objWrapper = iocResolver.ResolveAsDisposable<AppActionSettingPage>())
                {
                    shortCutSettingPage = objWrapper.Object;
                    (shortCutSettingPage.BindingContext as AppActionSettingPageViewModel).OnFinishedChooise += AppActionSettingPageViewModel_OnFinishedChooise;
                }
            });
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
        public Command AppActionSetting { get; set; }


    }






}
