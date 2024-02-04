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
        this.Load();

    }


    private async void Button_Clicked(object sender, EventArgs e)
    {
        await navigationService.ShowPopupAsync((this.Resources["PopupMenu"] as PopupBase));
    }

    private async void ContentPageBase_Loaded(object sender, EventArgs e)
    {

    }

    private async void Load()
    {
        await noteListPageViewModel.Init();

    }

    private void SwipeView_SwipeStarted(object sender, SwipeStartedEventArgs e)
    {
        noteListPageViewModel.SetIsUsingContextMenu(true);
    }

    private void SwipeView_SwipeEnded(object sender, SwipeEndedEventArgs e)
    {
        if (!e.IsOpen)
        {
            noteListPageViewModel.SetIsUsingContextMenu(false);

        }

    }

    private void SwipeItemView_Invoked(object sender, EventArgs e)
    {
        noteListPageViewModel.SetIsUsingContextMenu(false);

    }
}