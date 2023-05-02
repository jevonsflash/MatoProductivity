using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Abp.Domain.Services;
using Castle.MicroKernel;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core;
using Microsoft.Maui.Controls;

namespace MatoProductivity.Services
{
    public class NavigationService : AbpServiceBase, ISingletonDependency
    {
        private readonly IIocManager iocManager;

        private INavigation mainPageNavigation => mainShell.Navigation;
        private Shell mainShell => Shell.Current;

        public NavigationService(
            IIocManager iocManager
            )
        {
            this.iocManager = iocManager;
            LocalizationSourceName = MatoProductivityConsts.LocalizationSourceName;

        }

        public async Task PushAsync(string pageName, object[] args = null)
        {
            var page = GetPageInstance(pageName, args);
            await mainPageNavigation.PushAsync(page);
        }

        public async Task PushModalAsync(string pageName, object[] args = null)
        {
            var page = GetPageInstance(pageName, args);
            await mainPageNavigation.PushModalAsync(page);
        }

        public async Task PushAsync(Page page)
        {
            await mainPageNavigation.PushAsync(page);
        }

        public async Task PushModalAsync(Page page)
        {
            await mainPageNavigation.PushModalAsync(page);
        }

        public async Task PopAsync()
        {
            await mainPageNavigation.PopAsync();
        }

        public async Task PopToRootAsync()
        {
            await mainPageNavigation.PopToRootAsync();
        }


        public async Task GoPageAsync(string obj)
        {
            var route = $"///{obj}";
            await mainShell.GoToAsync(route);
        }

        public async Task ShowPopupAsync(Popup popupPage)
        {
            await Shell.Current.CurrentPage.ShowPopupAsync(popupPage);
        }
        public async Task HidePopupAsync(Popup popupPage)
        {
            popupPage.Close();
        }
        private Page GetPageInstance(string obj, object[] args, IList<ToolbarItem> barItem = null)
        {
            Page result = null;
            var namespacestr = "MatoProductivity";
            Type pageType = Type.GetType(namespacestr + "." + obj, false);
            if (pageType != null)
            {
                try
                {
                    var ctorInfo = pageType.GetConstructors()
                                          .Select(m => new
                                          {
                                              Method = m,
                                              Params = m.GetParameters(),
                                          }).Where(c => c.Params.Length == args.Length)
                                          .FirstOrDefault();
                    if (ctorInfo==null)
                    {
                        throw new Exception("找不到对应的构造函数");
                    }

                    var argsDict = new Arguments();

                    for (int i = 0; i < ctorInfo.Params.Length; i++)
                    {
                        var arg = ctorInfo.Params[i];
                        argsDict.Add(arg.Name, args[i]);
                    }

                    var pageObj = iocManager.IocContainer.Resolve(pageType, argsDict) as Page;

                    if (barItem != null && barItem.Count > 0)
                    {
                        foreach (var toolbarItem in barItem)
                        {
                            pageObj.ToolbarItems.Add(toolbarItem);
                        }
                    }
                    result = pageObj;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
            return result;
        }
    }
}
