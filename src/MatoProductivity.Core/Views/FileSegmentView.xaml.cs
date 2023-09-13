using Abp.Dependency;
using MatoProductivity.Core.Services;

namespace MatoProductivity.Core.Views;

public partial class FileSegmentView : ContentView, ITransientDependency
{
	public FileSegmentView()
	{
		InitializeComponent();
	}
}