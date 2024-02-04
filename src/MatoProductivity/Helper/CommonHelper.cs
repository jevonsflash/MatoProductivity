using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Helper
{
    public class CommonHelper
    {
        public static void Alert(string msg)
        {

            Application.Current.MainPage.DisplayAlert("提示", msg, "好");
        }

        public static void Alert(string msg, string title)
        {

            Application.Current.MainPage.DisplayAlert(title, msg, "好");
        }

        public static async Task<bool> Confirm(string msg, string title, string accept, string cancel)
        {

            return await Application.Current.MainPage.DisplayAlert(title, msg, accept, cancel);
        }

        public static async Task<bool> Confirm(string msg, string title)
        {

            return await Application.Current.MainPage.DisplayAlert(title, msg, "确定", "取消");
        }


        public static async Task<bool> Confirm(string msg)
        {

            return await Application.Current.MainPage.DisplayAlert("提示", msg, "确定", "取消");
        }

        public static void ShowNoAuthorized(string msg)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Application.Current.MainPage.DisplayAlert("需要系统权限", msg, "好");
            });
        }

        public static async Task<string> ActionSheet(params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet("提示", "取消", null, buttons);
        }

        public static async Task<string> ActionSheet(string title, params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, "取消", null, buttons);
        }

        public static async Task<string> ActionSheet(string title, string cancel,  params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, cancel, null, buttons);
        }

        public static async Task<string> ActionSheet(string title, string cancel, string destruction, params string[] buttons)
        {
            return await Application.Current.MainPage.DisplayActionSheet(title, cancel, destruction, buttons);
        }
        public static async Task<string> PromptAsync(string title, string initialValue = null, string msg = null)
        {

            return await Application.Current.MainPage.DisplayPromptAsync(title, msg, "确定", "取消", "请输入内容", initialValue: initialValue);
        }

        public static async Task GoUri(object obj)
        {
            var uri = obj.ToString();
            UriBuilder uriSite = new UriBuilder(uri);
            var prefix = uri.Split("://")[0];
            bool supportsUri = await Launcher.Default.CanOpenAsync(prefix+"://");

            if (supportsUri)
                await Launcher.Default.OpenAsync(uriSite.Uri);
        }

        public static void BeginInvokeOnMainThread(Action action)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(action);
        }
    }



}
