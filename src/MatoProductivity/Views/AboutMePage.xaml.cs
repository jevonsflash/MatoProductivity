using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class AboutMePage : PopupBase, ITransientDependency
{
    private AboutMePageViewModel AboutMePageViewModel => this.BindingContext as AboutMePageViewModel;

    public AboutMePage(AboutMePageViewModel noteSegmentStoreListPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = noteSegmentStoreListPageViewModel;
    }

    private async void PopupBase_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {

    }
}