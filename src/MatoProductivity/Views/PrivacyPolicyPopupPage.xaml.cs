using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Core.Views;

public partial class PrivacyPolicyPopupPage : PopupBase, ITransientDependency
{

    public PrivacyPolicyPopupPage()
    {
        InitializeComponent();
        
    }

    public PrivacyPolicyPopupPage(string url) : this()
    {
        this.MainWebView.Source=url;
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
       await this.CloseAsync();
    }
}