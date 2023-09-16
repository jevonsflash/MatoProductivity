using Abp.Dependency;


namespace MatoProductivity.Core.Views;

public partial class WeatherSegmentView : ContentView, ITransientDependency
{
	public WeatherSegmentView()
	{
		InitializeComponent();
	}
}