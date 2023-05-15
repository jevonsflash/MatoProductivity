using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Hosting;
using MatoProductivity.Core;
using CommunityToolkit.Maui;

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
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("FontAwesome.ttf", "FontAwesome");
					fonts.AddFont("FontAwesome-Regular.ttf", "FontAwesome-Regular");
					fonts.AddFont("FontAwesome-Solid.ttf", "FontAwesome-Solid");
					fonts.AddFont("Mulish-Black.ttf", "Mulish-Black");
					fonts.AddFont("Mulish-Bold.ttf", "Mulish-Bold");
					fonts.AddFont("Mulish-ExtraBold.ttf", "Mulish-ExtraBold");
					fonts.AddFont("Mulish-ExtraLight.ttf", "Mulish-ExtraLight");
					fonts.AddFont("Mulish-Light.ttf", "Mulish-Light");
					fonts.AddFont("Mulish-Medium.ttf", "Mulish-Medium");
					fonts.AddFont("Mulish-Regular.ttf", "Mulish-Regular");
					fonts.AddFont("Mulish-SemiBold.ttf", "Mulish-SemiBold");
				});
			return builder.Build();
		}
	}
}