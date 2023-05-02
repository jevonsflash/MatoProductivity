using System;
using Abp.Dependency;
using MatoProductivity.Core.Helper;
using MatoProductivity.Core.Interfaces;
using MatoProductivity.Core.Services;
using MatoProductivity.Core.ViewModel;
using Microsoft.Maui.Controls;

namespace MatoProductivity.ViewModels
{
    public class NowPlayingPageViewModel : ViewModelBase
    {

        private readonly IocManager iocManager;
        public NowPlayingPageViewModel(IocManager iocManager)
        {
            this.iocManager=iocManager;
        }



    }
}
