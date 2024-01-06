using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Core.Views;

public partial class LocationSelectingPage : PopupBase, ITransientDependency
{
    private LocationSelectingPageViewModel LocationSelectingPageViewModel => this.BindingContext as LocationSelectingPageViewModel;

    public LocationSelectingPage(LocationSelectingPageViewModel locationSelectingPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = locationSelectingPageViewModel;

        rootComponent.Parameters =
new Dictionary<string, object>
{
                    { "LocationSelectingPageViewModel", LocationSelectingPageViewModel }
};
    }

    private async void PopupBase_Opened(object sender, CommunityToolkit.Maui.Core.PopupOpenedEventArgs e)
    {
    }
}