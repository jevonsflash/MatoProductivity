using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class IconSelectingPage : PopupBase, ITransientDependency
{
    private IconSelectingPageViewModel IconSelectingPageViewModel => this.BindingContext as IconSelectingPageViewModel;

    public IconSelectingPage(IconSelectingPageViewModel iconSelectingPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = iconSelectingPageViewModel;
    }

    private async void PopupBase_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
        await IconSelectingPageViewModel.Init();

    }
}