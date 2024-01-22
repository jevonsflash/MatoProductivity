using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class NoteListPage : ContentPageBase, ISingletonDependency
{
    private NoteListPageViewModel noteListPageViewModel => this.BindingContext as NoteListPageViewModel;

    public NoteListPage(NoteListPageViewModel noteListPageViewModel)
    {
        InitializeComponent();
        this.BindingContext = noteListPageViewModel;

    }


    private async void Button_Clicked(object sender, EventArgs e)
    {
       await navigationService.ShowPopupAsync((this.Resources["PopupMenu"] as PopupBase));
    }

    private async void ContentPageBase_Loaded(object sender, EventArgs e)
    {
        await noteListPageViewModel.Init();

    }
}