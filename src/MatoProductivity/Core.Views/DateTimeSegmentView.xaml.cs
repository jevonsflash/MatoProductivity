using Abp.Dependency;

namespace MatoProductivity.Core.Views;

public partial class DateTimeSegmentView : ContentView, ITransientDependency
{
	public DateTimeSegmentView()
	{
		InitializeComponent();
	}
}