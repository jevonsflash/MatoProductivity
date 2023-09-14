using Abp.Dependency;
using MatoProductivity.Core.Services;

namespace MatoProductivity.Core.Views;

public partial class ScriptSegmentView : ContentView, ITransientDependency
{
	public ScriptSegmentView()
	{
		InitializeComponent();
	}
}