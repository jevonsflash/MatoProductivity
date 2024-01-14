using Abp.Dependency;
using MatoProductivity.ViewModels;
using Microsoft.Maui.Handlers;

namespace MatoProductivity.Views;

public partial class EditNoteTemplatePage : ContentPageBase, ITransientDependency
{
    private EditNoteTemplatePageViewModel editNoteTemplatePageViewModel => this.BindingContext as EditNoteTemplatePageViewModel;

    public EditNoteTemplatePage(EditNoteTemplatePageViewModel editNoteTemplatePageViewModel)
    {
        InitializeComponent();
        this.BindingContext = editNoteTemplatePageViewModel;
        this.MainCollectionView.ChildAdded+=MainCollectionView_ChildAdded;
    }


    public EditNoteTemplatePage(EditNoteTemplatePageViewModel editNoteTemplatePageViewModel, long NoteId) : this(editNoteTemplatePageViewModel)
    {
        this.editNoteTemplatePageViewModel.NoteTemplateId = NoteId;

    }

    private async void MainCollectionView_ChildAdded(object sender, ElementEventArgs e)
    {

        await this.MainCollectionViewWrapper.ScrollToAsync(MainCollectionViewWrapper.ScrollX, MainCollectionView.Height, true);

    }

    public EditNoteTemplatePage(EditNoteTemplatePageViewModel editNoteTemplatePageViewModel, long NoteId, long NoteTemplateId) : this(editNoteTemplatePageViewModel, NoteId)
    {
        editNoteTemplatePageViewModel.Clone.Execute(NoteTemplateId);

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
}