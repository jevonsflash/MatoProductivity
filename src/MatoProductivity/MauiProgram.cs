using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using MatoProductivity.Core;
using CommunityToolkit.Maui;
using Abp.Modules;
using MatoProductivity.Controls;
using Microsoft.Extensions.Logging;

namespace MatoProductivity
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMatoProductivity<MatoProductivityModule>()
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("FontAwesome.ttf", "FontAwesome");
                    fonts.AddFont("FontAwesome-Regular.ttf", "FontAwesome_Regular");
                    fonts.AddFont("FontAwesome-Solid.ttf", "FontAwesome_Solid");
                    fonts.AddFont("Mulish-Black.ttf", "Mulish_Black");
                    fonts.AddFont("Mulish-Bold.ttf", "Mulish_Bold");
                    fonts.AddFont("Mulish-ExtraBold.ttf", "Mulish_ExtraBold");
                    fonts.AddFont("Mulish-ExtraLight.ttf", "Mulish_ExtraLight");
                    fonts.AddFont("Mulish-Light.ttf", "Mulish_Light");
                    fonts.AddFont("Mulish-Medium.ttf", "Mulish_Medium");
                    fonts.AddFont("Mulish-Regular.ttf", "Mulish_Regular");
                    fonts.AddFont("Mulish-SemiBold.ttf", "Mulish_SemiBold");
                })
                 .ConfigureEssentials(essentials =>
                 {
                     essentials
                     .UseVersionTracking()
                     .OnAppAction(App.HandleAppActions);
                 });


            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }


    }
}