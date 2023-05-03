using System.ComponentModel;
using Abp.Configuration;
using Abp.Dependency;
using MatoProductivity.Core.Settings;
using MatoProductivity.ViewModels;

namespace MatoProductivity
{
    public partial class NoteTemplatePage : ContentPageBase, ITransientDependency
    {

        public NoteTemplatePage(NoteTemplatePageViewModel noteTemplatePageViewModel)
        {
            InitializeComponent();
            this.BindingContext = noteTemplatePageViewModel;
            this.Disappearing += NoteTemplatePage_Disappearing;
            this.Appearing += NoteTemplatePage_Appearing;

        }

        private void NoteTemplatePage_Appearing(object sender, EventArgs e)
        {
            var isHideQueueButton = SettingManager.GetSettingValueForApplication<bool>(CommonSettingNames.IsHideQueueButton);
        }

        private void NoteTemplatePage_Disappearing(object sender, EventArgs e)
        {
            var viewModel = BindingContext as NoteTemplatePageViewModel;
        }

        

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await navigationService.GoPageAsync("QueuePage");
        }

        private async void GoLibrary_OnClicked(object sender, EventArgs e)
        {
            await navigationService.GoPageAsync("LibraryMainPage");
        }

        private void BindableObject_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsVisible")
            {
            }
        }

    }
}
