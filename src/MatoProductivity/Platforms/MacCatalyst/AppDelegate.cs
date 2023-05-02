using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace MatoProductivity.Platforms.MacCatalyst
{
    [Register("AppDelegate")]
    public class AppDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}