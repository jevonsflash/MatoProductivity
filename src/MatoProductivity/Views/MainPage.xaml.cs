using Abp.Dependency;
using MatoProductivity.Views;

namespace MatoProductivity;

public partial class MainPage : Shell, ITransientDependency
{
    private readonly IocManager iocManager;

    public MainPage(IocManager iocManager)
	{
		InitializeComponent();
        this.iocManager = iocManager;
        this.Init();
    }



    private void Init()
    {
        var noteTemplatePage = iocManager.Resolve<NoteTemplateListPage>();
        this.NoteTemplateContent.Content = noteTemplatePage;

        var notePage = iocManager.Resolve<NoteListPage>();
        this.NoteContent.Content = notePage;
    }

}