using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Views;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
        public class NoteTemplateListPageViewModel : ViewModelBase, ISingletonDependency
        {
            private readonly IRepository<NoteTemplate, long> repository;

            public NoteTemplateListPageViewModel(IRepository<NoteTemplate, long> repository)
            {
                this.repository = repository;
                this.PropertyChanged += NoteTemplatePageViewModel_PropertyChangedAsync;
                var noteTemplates = this.repository.GetAllList();
                this.NoteTemplates = new ObservableCollection<NoteTemplate>(noteTemplates);
            }

            private async void NoteTemplatePageViewModel_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
            {
                if (e.PropertyName == nameof(SelectedNoteTemplate))
                {
                    if (SelectedNoteTemplate != default)
                    {
                        var navigationParameter = new Dictionary<string, object>
                    {
                        { "NoteTemplateId", SelectedNoteTemplate.Id }
                    };
                        await Shell.Current.GoToAsync(nameof(EditNotePage), navigationParameter);

                    }
                }
            }
            private ObservableCollection<NoteTemplate> _noteTemplates;

            public ObservableCollection<NoteTemplate> NoteTemplates
            {
                get { return _noteTemplates; }
                set
                {
                    _noteTemplates = value;
                    RaisePropertyChanged();
                }
            }

            private NoteTemplate _selectedNoteTemplate;

            public NoteTemplate SelectedNoteTemplate
            {
                get { return _selectedNoteTemplate; }
                set
                {
                    _selectedNoteTemplate = value;
                    RaisePropertyChanged();

                }
            }



        }





    
}
