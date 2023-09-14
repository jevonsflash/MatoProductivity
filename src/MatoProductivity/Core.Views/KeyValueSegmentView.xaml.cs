using Abp.Dependency;


namespace MatoProductivity.Core.Views;

public partial class KeyValueSegmentView : ContentView, ITransientDependency
{
	public KeyValueSegmentView()
	{
		InitializeComponent();
	}
}