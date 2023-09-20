using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
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
            this.Create = new Command(CreateActionAsync);
            Remove = new Command(RemoveAction);
            Edit = new Command(EditAction);
            this.SwitchState = new Command(SwitchStateAction);
            this.repository = repository;
            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.PropertyChanged += NoteTemplatePageViewModel_PropertyChangedAsync;
            //Init();
        }
        private async void CreateActionAsync(object obj)
        {

            //using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = 0 }))
            //{
            //    (objWrapper.Object.BindingContext as EditNotePageViewModel).Create.Execute(null);

            //    await navigationService.PushAsync(objWrapper.Object);
            //}
        }

        private void RemoveAction(object obj)
        {
            var note = (NoteTemplateWrapper)obj;

            var delete = NoteTemplates.FirstOrDefault(c => c.NoteTemplate.Id == note.NoteTemplate.Id);
            NoteTemplates.Remove(delete);


        }

        private async void EditAction(object obj)
        {
            var noteTemplateWrapper = (NoteTemplateWrapper)obj;
            var note = noteTemplateWrapper.NoteTemplate;
            using (var objWrapper = iocResolver.ResolveAsDisposable<EditNoteTemplatePage>(new { NoteId = note.Id }))
            {
                await navigationService.PushAsync(objWrapper.Object);
            }
        }

        private void SwitchStateAction(object obj)
        {
            this.IsEditing = !this.IsEditing;
        }


        public void Init()
        {
            var noteTemplates = this.repository.GetAllList();
            this.NoteTemplates = new ObservableCollection<NoteTemplateWrapper>(noteTemplates.Select(c => new NoteTemplateWrapper(c) { Container = this }));
        }

        private async void NoteTemplatePageViewModel_PropertyChangedAsync(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedNoteTemplate))
            {
                if (SelectedNoteTemplate != default)
                {
                    using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = 0, NoteTemplateId = SelectedNoteTemplate.NoteTemplate.Id }))
                    {
                        await navigationService.PushAsync(objWrapper.Object);
                    }

                    SelectedNoteTemplate = default;
                }
            }
        }
        private ObservableCollection<NoteTemplateWrapper> _noteTemplates;

        public ObservableCollection<NoteTemplateWrapper> NoteTemplates
        {
            get { return _noteTemplates; }
            set
            {
                _noteTemplates = value;
                RaisePropertyChanged();
            }
        }

        private NoteTemplateWrapper _selectedNoteTemplate;

        public NoteTemplateWrapper SelectedNoteTemplate
        {
            get { return _selectedNoteTemplate; }
            set
            {
                _selectedNoteTemplate = value;
                RaisePropertyChanged();

            }
        }

        private bool _isEditing;

        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectionMode));

            }
        }
        public SelectionMode SelectionMode => IsEditing ? SelectionMode.Multiple : SelectionMode.Single;

        public Command SwitchState { get; set; }
        public Command Create { get; set; }

        public Command Remove { get; set; }
        public Command Edit { get; set; }
    }






}
