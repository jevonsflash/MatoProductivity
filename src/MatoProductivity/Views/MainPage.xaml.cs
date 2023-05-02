using Abp.Dependency;
using MatoProductivity.Core.Services;

namespace MatoProductivity;

public partial class MainPage : Shell, ITransientDependency
{
    private readonly IocManager iocManager;

    public MainPage(IocManager iocManager)
	{
		InitializeComponent();
        this.iocManager = iocManager;
        this.Init();
        Loaded += MainPage_Loaded;
    }

    private async void MainPage_Loaded(object sender, EventArgs e)
    {
        var musicRelatedViewModel = iocManager.Resolve<MusicRelatedService>();
        await musicRelatedViewModel.InitAll();
    }

    private void Init()
    {
        var nowPlayingPage = iocManager.Resolve<NowPlayingPage>();
        this.NowPlayingPageShellContent.Content = nowPlayingPage;
    }

}