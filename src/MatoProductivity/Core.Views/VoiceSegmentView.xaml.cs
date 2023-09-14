using Abp.Dependency;
using MatoProductivity.Core.Services;

namespace MatoProductivity.Core.Views;

public partial class VoiceSegmentView : ContentView, ITransientDependency
{
	public VoiceSegmentView()
	{
		InitializeComponent();
	}
}