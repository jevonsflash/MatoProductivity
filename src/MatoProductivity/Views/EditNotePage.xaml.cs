using Abp.Dependency;
using MatoProductivity.ViewModels;

namespace MatoProductivity.Views;

[QueryProperty(nameof(NoteId), nameof(NoteId))]
[QueryProperty(nameof(NoteTemplateId), nameof(NoteTemplateId))]
public partial class EditNotePage : ContentPageBase, ITransientDependency
{
    private EditNotePageViewModel editNotePageViewModel => this.BindingContext as EditNotePageViewModel;

    public EditNotePage(EditNotePageViewModel editNotePageViewModel)
    {
        InitializeComponent();
        this.BindingContext = editNotePageViewModel;

    }
    private long noteId;

    public long NoteId
    {
        get { return noteId; }
        set
        {
            noteId = value;
            editNotePageViewModel.NoteId = value;

        }
    }

    private long noteTemplateId;

    public long NoteTemplateId
    {
        get { return noteTemplateId; }
        set
        {
            noteTemplateId = value;
            editNotePageViewModel.Clone.Execute(noteTemplateId);

        }
    }


}