using Abp.Dependency;


namespace MatoProductivity.Core.Views;

public partial class ContactSegmentView : ContentView, ITransientDependency
{
	public ContactSegmentView()
	{
		InitializeComponent();
	}
}