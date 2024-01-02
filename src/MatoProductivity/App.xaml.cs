using System;
using System.IO;
using Abp;
using Abp.BackgroundJobs;
using Abp.Castle.Logging.Log4Net;
using Abp.Dependency;
using Castle.Facilities.Logging;
using MatoProductivity.Core;
using MatoProductivity.Helper;
using MatoProductivity.Services;
using MatoProductivity.ViewModels;
using MatoProductivity.Views;
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



            EditorHandler.Mapper.AppendToMapping("Background", (handler, view) =>
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
                handler.PlatformView.BorderStyle = UIKit.UITextViewBorderStyle.None;
#endif
            });
            TimePickerHandler.Mapper.AppendToMapping("Background", (handler, view) =>
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
            DatePickerHandler.Mapper.AppendToMapping("Background", (handler, view) =>
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

        public static void HandleAppActions(AppAction appAction)
        {
            App.Current.Dispatcher.Dispatch(async () =>
            {
                if (appAction.Id == "create_note")
                {
                    using (var objWrapper = IocManager.Instance.ResolveAsDisposable<EditNotePageViewModel>())
                    {
                        var editNotePageViewModel = objWrapper.Object;
                        editNotePageViewModel.SimplifiedClone.Execute((long)2);
                    }
                }
                else if (true)
                {
                    var navigationService = IocManager.Instance.Resolve<NavigationService>();
                    if (navigationService != null)
                    {
                        await navigationService.PopToRootAsync();
                        await navigationService.GoPageAsync("NoteContent");
                        using (var objWrapper = IocManager.Instance.ResolveAsDisposable<EditNotePage>(new { NoteId = 0 }))
                        {
                            (objWrapper.Object.BindingContext as EditNotePageViewModel).Create.Execute(null);

                            await navigationService.PushAsync(objWrapper.Object);
                        }
                    }
                }
            });
        }
    }
}
