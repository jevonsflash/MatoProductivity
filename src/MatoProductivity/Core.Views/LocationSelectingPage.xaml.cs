using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Core.Views;

public partial class LocationSelectingPage : ContentPageBase, ITransientDependency
{
    private LocationSelectingPageViewModel LocationSelectingPageViewModel => this.BindingContext as LocationSelectingPageViewModel;

    public LocationSelectingPage(LocationSelectingPageViewModel locationSelectingPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = locationSelectingPageViewModel;
    }
}