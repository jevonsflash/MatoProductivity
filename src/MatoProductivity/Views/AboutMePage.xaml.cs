using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class AboutMePage : PopupBase, ITransientDependency
{
    private AboutMePageViewModel AboutMePageViewModel => this.BindingContext as AboutMePageViewModel;

    public AboutMePage(AboutMePageViewModel aboutMePageViewModel)
    {
        InitializeComponent();
        this.BindingContext = aboutMePageViewModel;
    }

    private async void PopupBase_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {

    }
}