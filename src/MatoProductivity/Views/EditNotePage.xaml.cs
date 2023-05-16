using Abp.Dependency;
using MatoProductivity.ViewModels;
using Microsoft.Maui.Handlers;

namespace MatoProductivity.Views;

public partial class EditNotePage : ContentPageBase, ITransientDependency
{
    private EditNotePageViewModel editNotePageViewModel => this.BindingContext as EditNotePageViewModel;

    public EditNotePage(EditNotePageViewModel editNotePageViewModel)
    {
        InitializeComponent();
        this.BindingContext = editNotePageViewModel;
        Loaded+=EditNotePage_Loaded;
    }

    private void EditNotePage_Loaded(object sender, EventArgs e)
    {
        EntryHandler.Mapper.AppendToMapping("Background", (handler, view) =>
        {
#if ANDROID
	var shape = new Android.Graphics.Drawables.ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());

	if (shape.Paint is not null)
	{
		shape.Paint.Color = Android.Graphics.Color.Transparent;
		shape.Paint.StrokeWidth = 0;
		shape.Paint.SetStyle(Android.Graphics.Paint.Style.Stroke);
	}
	handler.PlatformView.Background = shape;
#elif IOS || MACCATALYST
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
        });
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
}