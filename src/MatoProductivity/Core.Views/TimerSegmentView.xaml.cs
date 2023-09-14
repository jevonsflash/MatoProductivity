using Abp.Dependency;

namespace MatoProductivity.Core.Views;

public partial class TimerSegmentView : ContentView, ITransientDependency
{
	public TimerSegmentView()
	{
		InitializeComponent();
	}
}