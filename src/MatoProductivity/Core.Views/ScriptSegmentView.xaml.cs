using Abp.Dependency;
using MatoProductivity.Core.Services;

namespace MatoProductivity.Core.Views;
public partial class ScriptSegmentView : ContentView, ITransientDependency
{

    public ScriptSegmentView()
    {
        InitializeComponent();
        HideCollectionViews();
        Loaded+=ScriptSegmentView_Loaded;
    }

    private void ScriptSegmentView_Loaded(object sender, EventArgs e)
    {
        (BindingContext as ScriptSegmentService).GetImageSize=() => new Size(this.MainEditor.Width, this.MainEditor.Height);
    }

    private void DrawingLineSizeButton_Clicked(object sender, EventArgs e)
    {
        ColorCollectionView.IsVisible = false;
        DrawingLineSizeCollectionView.IsVisible =  !DrawingLineSizeCollectionView.IsVisible;
    }

    private void LineColorButton_Clicked(object sender, EventArgs e)
    {
        DrawingLineSizeCollectionView.IsVisible = false;
        ColorCollectionView.IsVisible =  !ColorCollectionView.IsVisible;

    }

    private void HideCollectionViews()
    {
        ColorCollectionView.IsVisible = false;
        DrawingLineSizeCollectionView.IsVisible = false;
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        HideCollectionViews();

    }
}