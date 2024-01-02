using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class AppActionSettingPage : PopupBase, ITransientDependency
{
    private AppActionSettingPageViewModel AppActionSettingPageViewModel => this.BindingContext as AppActionSettingPageViewModel;

    public AppActionSettingPage(AppActionSettingPageViewModel noteSegmentStoreListPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = noteSegmentStoreListPageViewModel;
    }

    private async void PopupBase_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        await AppActionSettingPageViewModel.Init();

    }
}