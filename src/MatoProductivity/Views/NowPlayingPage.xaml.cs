using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Localization;
using MatoProductivity.Common;
using MatoProductivity.Core;
using MatoProductivity.Core.Helper;
using MatoProductivity.Core.Interfaces;
using MatoProductivity.Core.Localization;
using MatoProductivity.Core.Models;
using MatoProductivity.Core.Settings;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Services;
using MatoProductivity.ViewModels;
using Microsoft.Maui.Controls;

namespace MatoProductivity
{
    public partial class NowPlayingPage : ContentPageBase, ITransientDependency
    {

        public NowPlayingPage(NowPlayingPageViewModel nowPlayingPageViewModel)
        {
            InitializeComponent();
            this.BindingContext = nowPlayingPageViewModel;
            this.Disappearing += NowPlayingPage_Disappearing;
            this.Appearing += NowPlayingPage_Appearing;

        }

        private void NowPlayingPage_Appearing(object sender, EventArgs e)
        {
            var isHideQueueButton = SettingManager.GetSettingValueForApplication<bool>(CommonSettingNames.IsHideQueueButton);
        }

        private void NowPlayingPage_Disappearing(object sender, EventArgs e)
        {
            var viewModel = BindingContext as NowPlayingPageViewModel;
        }

        

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await navigationService.GoPageAsync("QueuePage");
        }

        private async void GoLibrary_OnClicked(object sender, EventArgs e)
        {
            await navigationService.GoPageAsync("LibraryMainPage");
        }

        private void BindableObject_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsVisible")
            {
            }
        }

    }
}
