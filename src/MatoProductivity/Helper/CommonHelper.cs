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
        public static void ShowMsg(string msg)
        {

            Application.Current.MainPage.DisplayAlert("提示", msg, "好");
        }

        public static void ShowMsg(string msg, string title)
        {

            Application.Current.MainPage.DisplayAlert(title, msg, "好");
        }

        public static void ShowNoAuthorized()
        {
            Application.Current.MainPage.DisplayAlert("需要权限", "MatoProductivity需要您媒体库的权限，劳烦至「设置」「隐私权」「媒体与AppleMusic」 打开权限,谢谢", "好");
        }

        public static async Task<string> PromptAsync(string title, string initialValue = null, string msg = null)
        {

            return await Application.Current.MainPage.DisplayPromptAsync(title, msg, "确定", "取消", "请输入内容", initialValue: initialValue);
        }

        public static void GoUrl(object obj)
        {
            throw new NotImplementedException();
        }

        public static void BeginInvokeOnMainThread(Action action)
        {
            Application.Current.MainPage.Dispatcher.Dispatch(action);
        }
    }



}
