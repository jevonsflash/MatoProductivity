using Abp.Dependency;


namespace MatoProductivity.Core.Views;

public partial class TextSegmentView : ContentView, ITransientDependency
{
	public TextSegmentView()
	{
		InitializeComponent();
	}
}