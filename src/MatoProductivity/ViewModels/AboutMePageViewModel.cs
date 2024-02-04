using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace MatoProductivity.ViewModels
{
    public class AboutMePageViewModel : PopupViewModelBase, ISingletonDependency
    {
        private readonly NavigationService navigationService;

        public event EventHandler<AppAction> OnFinishedChooise;

        public AboutMePageViewModel(
            NavigationService navigationService
            )
        {
            this.GoUriCommand = new Command(GoUriAction);
            this.Back = new Command(BackAction);

            this.navigationService = navigationService;
        }

        private async void GoUriAction(object obj)
        {
            await CommonHelper.GoUri(obj);
        }
        private async void BackAction(object obj)
        {
            await this.navigationService.PopAsync();
        }
        protected override void SetSize(DisplayInfo displayInfo)
        {
            var newWidth = displayInfo.Width*0.8;
            var newHeight = 800;

            var d = displayInfo.Density;
            PopupSize=new Size(newWidth/d, newHeight/d);
        }

        public Command GoUriCommand { get; set; }
        public Command Back { get; set; }

    }

}
