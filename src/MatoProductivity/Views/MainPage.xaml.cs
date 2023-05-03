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
        var noteTemplatePage = iocManager.Resolve<NoteTemplatePage>();
        this.NoteTemplateContent.Content = noteTemplatePage;

        var notePage = iocManager.Resolve<NotePage>();
        this.NoteContent.Content = notePage;
    }

}