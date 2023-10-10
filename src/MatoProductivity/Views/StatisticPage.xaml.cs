using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;
using Microsoft.AspNetCore.Components.WebView.Maui;

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
        rootComponent.Parameters =
    new Dictionary<string, object>
    {
            { "StatisticPageViewModel", noteListPageViewModel }
    };
    }


}