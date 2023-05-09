using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Services;
using MatoProductivity.Views;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class NoteTemplateListPageViewModel : ViewModelBase, ISingletonDependency
    {
        private readonly IRepository<NoteTemplate, long> repository;
        private readonly IIocResolver iocResolver;
        private readonly NavigationService navigationService;

        public NoteTemplateListPageViewModel(
            IRepository<NoteTemplate, long> repository,
            IIocResolver iocResolver,
            NavigationService navigationService

            )
        {
            this.repository = repository;
            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.PropertyChanged += NoteTemplatePageViewModel_PropertyChangedAsync;
            //Init();
        }
    
        public void Init()
        {
            var noteTemplates = this.repository.GetAllList();
            this.NoteTemplates = new ObservableCollection<NoteTemplate>(noteTemplates);
        }

        private async void NoteTemplatePageViewModel_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedNoteTemplate))
            {
                if (SelectedNoteTemplate != default)
                {
                    using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = 0, NoteTemplateId= SelectedNoteTemplate.Id }))
                    {
                        await navigationService.PushAsync(objWrapper.Object);
                    }
    
                    SelectedNoteTemplate = default;
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
