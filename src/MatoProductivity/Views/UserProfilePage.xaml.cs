using System.ComponentModel;
using Abp.Configuration;
using Abp.Dependency;
using MatoProductivity.Core.Settings;
using MatoProductivity.ViewModels;

namespace MatoProductivity
{
    public partial class UserProfilePage : ContentPageBase, ITransientDependency
    {
        private UserProfilePageViewModel UserProfilePageViewModel => this.BindingContext as UserProfilePageViewModel;

        public UserProfilePage(UserProfilePageViewModel noteTemplateListPageViewModel)
        {
            InitializeComponent();
            this.BindingContext = noteTemplateListPageViewModel;
        }

    }
}
