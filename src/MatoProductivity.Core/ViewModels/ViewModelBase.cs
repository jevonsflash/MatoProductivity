using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Abp;
using Abp.Dependency;
using Abp.Domain.Services;
using MatoProductivity.Core.Models;

namespace MatoProductivity.Core.ViewModels
{
    public abstract class ViewModelBase : AbpServiceBase, INotifyPropertyChanged
    {

        public ViewModelBase()
        {
            LocalizationSourceName = MatoProductivityConsts.LocalizationSourceName;
            Loading=false;
        }

        /// <summary>Occurs after a property value changes.</summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Provides access to the PropertyChanged event handler to derived classes.
        /// </summary>
        protected PropertyChangedEventHandler PropertyChangedHandler { get; }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string propertyName)
        {
            Type type = GetType();
            if (!string.IsNullOrEmpty(propertyName) && type.GetTypeInfo().GetDeclaredProperty(propertyName) == null)
                throw new ArgumentException("Property not found", propertyName);
        }

        public virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            // ISSUE: reference to a compiler-generated field
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;
            propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            // ISSUE: reference to a compiler-generated field
            if (PropertyChanged == null)
                return;
            string propertyName = GetPropertyName(propertyExpression);
            if (string.IsNullOrEmpty(propertyName))
                return;
            RaisePropertyChanged(propertyName);
        }

        protected static string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));
            MemberExpression body = propertyExpression.Body as MemberExpression;
            if (body == null)
                throw new ArgumentException("Invalid argument", nameof(propertyExpression));
            PropertyInfo member = body.Member as PropertyInfo;
            if (member == null)
                throw new ArgumentException("Argument is not a property", nameof(propertyExpression));
            return member.Name;
        }

        private bool _loading;

        public bool Loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                RaisePropertyChanged();

            }
        }


        private static UserInfo _currentUserInfo = new UserInfo() { Name="未登录", Nickname="登录以同步数据" };

        public UserInfo CurrentUserInfo
        {
            get { return _currentUserInfo; }
            set
            {
                _currentUserInfo = value;
                RaisePropertyChanged();

            }
        }
    }
}
