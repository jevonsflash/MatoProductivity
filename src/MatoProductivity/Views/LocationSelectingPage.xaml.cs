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

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        this.ContentFrame.IsVisible=true;
        this.ContentButton.IsVisible=false;
        this.ContentLabel.IsVisible=false;

    }

    private void Entry_Completed(object sender, EventArgs e)
    {
        EditDone();
    }


    private void Entry_Unfocused(object sender, FocusEventArgs e)
    {
        EditDone();

    }
    private void EditDone()
    {
        this.ContentFrame.IsVisible=false;
        this.ContentButton.IsVisible=true;
        this.ContentLabel.IsVisible=true;
    }

}