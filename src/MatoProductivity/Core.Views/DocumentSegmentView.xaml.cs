using Abp.Dependency;
using MatoProductivity.Core.Services;

namespace MatoProductivity.Core.Views;

public partial class DocumentSegmentView : ContentView, ITransientDependency
{
	public DocumentSegmentView()
	{
		InitializeComponent();
	}
}