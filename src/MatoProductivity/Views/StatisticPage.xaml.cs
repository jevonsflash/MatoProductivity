using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class StatisticPage : ContentPageBase, ITransientDependency
{
    private StatisticPageViewModel noteListPageViewModel => this.BindingContext as StatisticPageViewModel;

    public StatisticPage(StatisticPageViewModel noteListPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = noteListPageViewModel;

    }

    private void ContentPageBase_Appearing(object sender, EventArgs e)
    {
        noteListPageViewModel.Init();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
       await navigationService.ShowPopupAsync((this.Resources["PopupMenu"] as PopupBase));
    }
}