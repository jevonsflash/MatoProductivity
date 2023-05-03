using Abp.Dependency;

namespace MatoProductivity.Views;

public partial class NoteListPage :  ContentPageBase, ITransientDependency
{
	public NoteListPage()
	{
		InitializeComponent();
	}
}