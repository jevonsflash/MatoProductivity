using Abp.Dependency;
using MatoProductivity.ViewModels;
using Microsoft.Maui.Handlers;
using System.Collections.Specialized;

namespace MatoProductivity.Views;

public partial class EditNotePage : ContentPageBase, ITransientDependency
{
    private EditNotePageViewModel editNotePageViewModel => this.BindingContext as EditNotePageViewModel;

    public EditNotePage(EditNotePageViewModel editNotePageViewModel)
    {
        InitializeComponent();
        this.BindingContext = editNotePageViewModel;
        this.MainCollectionView.ChildAdded+=MainCollectionView_ChildAdded;
    }

    private async void MainCollectionView_ChildAdded(object sender, ElementEventArgs e)
    {

        await this.MainCollectionViewWrapper.ScrollToAsync(MainCollectionViewWrapper.ScrollX, MainCollectionView.Height, true);

    }

    public EditNotePage(EditNotePageViewModel editNotePageViewModel, long NoteId) : this(editNotePageViewModel)
    {
        this.editNotePageViewModel.NoteId = NoteId;

    }


    public EditNotePage(EditNotePageViewModel editNotePageViewModel, long NoteId, long NoteTemplateId) : this(editNotePageViewModel, NoteId)
    {
        editNotePageViewModel.Clone.Execute(NoteTemplateId);

    }

    private void OnFavoriteSwipeItemInvoked(object sender, EventArgs e)
    {

    }

    private void OnDeleteSwipeItemInvoked(object sender, EventArgs e)
    {

    }

    private void DragGestureRecognizer_DragStarting_Collection(object sender, DragStartingEventArgs e)
    {

    }

    private void Button_Clicked(object sender, EventArgs e)
    {

    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}