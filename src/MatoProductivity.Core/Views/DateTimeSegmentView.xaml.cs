using Abp.Dependency;
using MatoProductivity.Core.ViewModels;

namespace MatoProductivity.Core.Views;

public partial class DateTimeSegmentView : ContentView, ITransientDependency
{
	public DateTimeSegmentView()
	{
		InitializeComponent();
	}
}