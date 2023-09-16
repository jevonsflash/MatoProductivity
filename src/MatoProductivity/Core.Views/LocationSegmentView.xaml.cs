using Abp.Dependency;


namespace MatoProductivity.Core.Views;

public partial class LocationSegmentView : ContentView, ITransientDependency
{
	public LocationSegmentView()
	{
		InitializeComponent();
	}
}