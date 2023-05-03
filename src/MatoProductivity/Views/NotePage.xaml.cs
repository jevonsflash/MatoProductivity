using Abp.Dependency;

namespace MatoProductivity.Views;

public partial class NotePage :  ContentPageBase, ITransientDependency
{
	public NotePage()
	{
		InitializeComponent();
	}
}