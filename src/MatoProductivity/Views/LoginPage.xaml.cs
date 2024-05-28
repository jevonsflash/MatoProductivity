using System.ComponentModel;
using Abp.Configuration;
using Abp.Dependency;
using MatoProductivity.Core.Settings;
using MatoProductivity.Core.Views;
using MatoProductivity.ViewModels;

namespace MatoProductivity
{
    public partial class LoginPage : PopupBase, ITransientDependency
    {
        private readonly IIocResolver iocResolver;
        private LoginPageViewModel LoginPageViewModel => this.BindingContext as LoginPageViewModel;

        public event EventHandler<bool> OnFinishedChooise;
        public LoginPage(IIocResolver iocResolver, LoginPageViewModel loginPageViewModel)
        {
            InitializeComponent();
            this.BindingContext = loginPageViewModel;
            this.iocResolver=iocResolver;
        }

        private async void PopupBase_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
        {
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

        private async void TapGestureRecognizer_Tapped_2(object sender, TappedEventArgs e)
        {
           await this.CloseAsync();
        }
    }
}
