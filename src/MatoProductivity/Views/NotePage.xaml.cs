using Abp.Dependency;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class NotePage : ContentPageBase, ITransientDependency
{
    private NotePageViewModel NotePageViewModel => this.BindingContext as NotePageViewModel;

    public NotePage(NotePageViewModel NotePageViewModel, long NoteId)
    {
        InitializeComponent();
        this.BindingContext = NotePageViewModel;
        this.NotePageViewModel.NoteId = NoteId;

    }
}