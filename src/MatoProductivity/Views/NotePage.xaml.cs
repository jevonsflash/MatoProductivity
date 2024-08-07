using Abp.Dependency;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class NotePage : PopupBase, ITransientDependency
{
    private NotePageViewModel NotePageViewModel => this.BindingContext as NotePageViewModel;

    public NotePage(NotePageViewModel NotePageViewModel, long NoteId)
    {
        InitializeComponent();
        this.BindingContext = NotePageViewModel;
        this.NotePageViewModel.NoteId = NoteId;
        

    }

    private void DragGestureRecognizer_DragStarting_Collection(object sender, DragStartingEventArgs e)
    {

    }

    private void DropGestureRecognizer_DragOver(object sender, DragEventArgs e)
    {

    }
}