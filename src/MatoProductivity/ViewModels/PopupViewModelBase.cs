using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System.Reflection;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using MatoProductivity.Services;
using MatoProductivity.Views;
using System.Drawing;
using MatoProductivity.Core.ViewModels;
using System.Runtime.CompilerServices;
using Size = Microsoft.Maui.Graphics.Size;

namespace MatoProductivity.ViewModels
{
    public class PopupViewModelBase : ViewModelBase, IPopupViewModelBase
    {
        private readonly IDeviceDisplay deviceDisplay;
        public PopupViewModelBase()
        {
            this.deviceDisplay=DeviceDisplay.Current;
            this.deviceDisplay.MainDisplayInfoChanged+=DeviceDisplay_MainDisplayInfoChanged;
            var displayInfo = this.deviceDisplay.MainDisplayInfo;
            var newWidth = displayInfo.Width;
            var newHeight = displayInfo.Height*0.8;

            var d = displayInfo.Density;
            PopupSize=new Size(newWidth/d, newHeight/d);
        }

        private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            var displayInfo = e.DisplayInfo;
            // 新的宽度和高度
            var newWidth = displayInfo.Width;
            var newHeight = displayInfo.Height*0.8;

            var d = displayInfo.Density;
            PopupSize=new Size(newWidth/d, newHeight/d);
        }


        private Size _popupSize;

        public Size PopupSize
        {
            get { return _popupSize; }
            set
            {
                _popupSize = value;
                RaisePropertyChanged();

            }
        }

    }
}
