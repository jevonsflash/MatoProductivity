using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Extensions;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.Maui.ApplicationModel;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace MatoProductivity.ViewModels
{
    public class LoginPageViewModel : PopupViewModelBase, ISingletonDependency
    {
        private readonly NavigationService navigationService;

        public event EventHandler<UserInfo> OnFinishedLogin;

        public LoginPageViewModel(
            NavigationService navigationService
            )
        {
            this.Login = new Command(LoginAction);
            this.Back = new Command(BackAction);

            this.navigationService = navigationService;
        }

        private async void LoginAction(object obj)
        {
            if (!IsAgree)
            {
                CommonHelper.Alert("请先同意《服务协议》与《隐私政策》");
                return;
            }

            Loading = true;
            await Task.Delay(2000).ContinueWith((e) => { Loading = false; });

            if (UserName.Trim()=="admin" && Password.Trim()=="admin")
            {
                CommonHelper.Alert("登录成功");
                OnFinishedLogin?.Invoke(this, new UserInfo() { Name="测试账号01", Nickname="测试账号" });
            }
            else
            {
                CommonHelper.Alert("登录失败：用户名或密码错误");

            }
        }
        private async void BackAction(object obj)
        {
            await this.navigationService.PopAsync();
        }
        protected override void SetSize(DisplayInfo displayInfo)
        {
            var newWidth = displayInfo.Width*0.8;
            var newHeight = 850;

            var d = displayInfo.Density;
            PopupSize=new Size(newWidth/d, newHeight/d);
        }

        private bool _isAgree;

        public bool IsAgree
        {
            get { return _isAgree; }
            set
            {
                _isAgree = value;
                RaisePropertyChanged();
            }
        }

        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                RaisePropertyChanged();
            }
        }
        private string _password;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                RaisePropertyChanged();
            }
        }


        public Command Login { get; set; }
        public Command Back { get; set; }

    }

}
