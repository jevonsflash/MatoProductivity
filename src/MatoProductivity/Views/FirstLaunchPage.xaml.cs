using System.ComponentModel;
using Abp.Configuration;
using Abp.Dependency;
using MatoProductivity.Core.Settings;
using MatoProductivity.Core.Views;
using MatoProductivity.ViewModels;

namespace MatoProductivity
{
    public partial class FirstLaunchPage : PopupBase, ITransientDependency
    {
        private readonly IIocResolver iocResolver;

        public event EventHandler<bool> OnFinishedChooise;
        public FirstLaunchPage(IIocResolver iocResolver)
        {
            InitializeComponent();
            this.iocResolver=iocResolver;
        }

        private async void PopupBase_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
        {
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            this.OnFinishedChooise?.Invoke(this, true);
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            this.OnFinishedChooise?.Invoke(this, false);

        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            using var objWrapper = iocResolver.ResolveAsDisposable<PrivacyPolicyPopupPage>(new { url = "https://blog.matoapp.net/apps/matoproductivity/license.html" });
            await navigationService.ShowPopupAsync(objWrapper.Object);
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, TappedEventArgs e)
        {
            using var objWrapper = iocResolver.ResolveAsDisposable<PrivacyPolicyPopupPage>(new { url = "https://blog.matoapp.net/apps/matoproductivity/privacy.html" });
            await navigationService.ShowPopupAsync(objWrapper.Object);
        }
    }
}
