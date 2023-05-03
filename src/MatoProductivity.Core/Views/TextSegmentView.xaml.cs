using Abp.Dependency;
using MatoProductivity.Core.ViewModels;

namespace MatoProductivity.Core.Views;

public partial class TextSegmentView : ContentView, ITransientDependency
{
	public TextSegmentView()
	{
		InitializeComponent();
	}
}