using Abp.Dependency;
using Abp.Events.Bus;
using MatoProductivity.Core.Models;
using MatoProductivity.Views;

namespace MatoProductivity;

public partial class MainPage : Shell, ITransientDependency
{
    private readonly IocManager iocManager;
    private readonly IEventBus eventBus;

    public MainPage(IocManager iocManager, IEventBus eventBus)
	{
		InitializeComponent();
        this.iocManager = iocManager;
        this.eventBus = eventBus;
        Loaded += MainPage_Loaded;
        this.Init();
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
        this.eventBus.AsyncRegister<NotificationEto>(NotificationInvokedAction);
    }

    private async Task NotificationInvokedAction(NotificationEto arg)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            var title = arg.Args.NotificationTitle;
            var content = arg.Args.NotificationContent;
            await DisplayAlert(title, content, "OK");
        });

    }

    private void Init()
    {
        var noteTemplatePage = iocManager.Resolve<NoteTemplateListPage>();
        this.NoteTemplateContent.Content = noteTemplatePage;

        var notePage = iocManager.Resolve<NoteListPage>();
        this.NoteContent.Content = notePage;

        var statisticPage = iocManager.Resolve<StatisticPage>();
        this.StatisticContent.Content = statisticPage;
    }

}