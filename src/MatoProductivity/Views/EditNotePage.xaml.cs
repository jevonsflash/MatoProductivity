using Abp.Dependency;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

public partial class EditNotePage : ContentPageBase, ITransientDependency
{
    private EditNotePageViewModel editNotePageViewModel => this.BindingContext as EditNotePageViewModel;

    public EditNotePage(EditNotePageViewModel editNotePageViewModel)
    {
        InitializeComponent();
        this.BindingContext = editNotePageViewModel;
    }

    public EditNotePage(EditNotePageViewModel editNotePageViewModel, long NoteId) : this(editNotePageViewModel)
    {
        this.editNotePageViewModel.NoteId = NoteId;

    }

    public EditNotePage(EditNotePageViewModel editNotePageViewModel, long NoteId, long NoteTemplateId) : this(editNotePageViewModel, NoteId)
    {
        editNotePageViewModel.Clone.Execute(NoteTemplateId);

    }

}