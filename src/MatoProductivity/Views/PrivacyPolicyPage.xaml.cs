using System.ComponentModel;
using Abp.Configuration;
using Abp.Dependency;
using MatoProductivity.Core.Settings;
using MatoProductivity.ViewModels;

namespace MatoProductivity
{
    public partial class PrivacyPolicyPage : ContentPageBase, ISingletonDependency
    {
        public PrivacyPolicyPage()
        {
            InitializeComponent();
        }

        public PrivacyPolicyPage(string url) : this()
        {
            this.MainWebView.Source=url;
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            await navigationService.PopAsync();

        }
    }
}
