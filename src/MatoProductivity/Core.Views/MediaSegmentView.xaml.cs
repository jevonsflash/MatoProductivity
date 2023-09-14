using Abp.Dependency;
using MatoProductivity.Core.Services;

namespace MatoProductivity.Core.Views;

public partial class MediaSegmentView : ContentView, ITransientDependency
{
	public MediaSegmentView()
	{
		InitializeComponent();
	}
}