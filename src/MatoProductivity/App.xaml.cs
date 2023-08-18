using System;
using System.IO;
using Abp;
using Abp.BackgroundJobs;
using Abp.Castle.Logging.Log4Net;
using Abp.Dependency;
using Castle.Facilities.Logging;
using MatoProductivity.Core;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Microsoft.Maui.Handlers;
using Application = Microsoft.Maui.Controls.Application;

namespace MatoProductivity
{
    public partial class App : Application
    {
        private readonly AbpBootstrapper _abpBootstrapper;

        public App(AbpBootstrapper abpBootstrapper)
        {
            _abpBootstrapper = abpBootstrapper;
            InitializeComponent();
            _abpBootstrapper.Initialize();
            this.MainPage = abpBootstrapper.IocManager.Resolve(typeof(MainPage)) as MainPage;
            EntryHandler.Mapper.AppendToMapping("Background", (handler, view) =>
            {
#if ANDROID
	            var shape = new Android.Graphics.Drawables.ShapeDrawable(new Android.Graphics.Drawables.Shapes.RectShape());

	            if (shape.Paint is not null)
	            {
		            shape.Paint.Color = Android.Graphics.Color.Transparent;
		            shape.Paint.StrokeWidth = 0;
		            shape.Paint.SetStyle(Android.Graphics.Paint.Style.Stroke);
	            }
	            handler.PlatformView.Background = shape;
#elif IOS || MACCATALYST
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
                });
        }
    }
}
