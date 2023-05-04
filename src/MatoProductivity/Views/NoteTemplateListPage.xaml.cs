using System.ComponentModel;
using Abp.Configuration;
using Abp.Dependency;
using MatoProductivity.Core.Settings;
using MatoProductivity.ViewModels;

namespace MatoProductivity
{
    public partial class NoteTemplateListPage : ContentPageBase, ITransientDependency
    {
        private NoteTemplateListPageViewModel NoteTemplateListPageViewModel => this.BindingContext as NoteTemplateListPageViewModel;

        public NoteTemplateListPage(NoteTemplateListPageViewModel noteTemplateListPageViewModel)
        {
            InitializeComponent();
            this.BindingContext = noteTemplateListPageViewModel;
        }


    }
}
