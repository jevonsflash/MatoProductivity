using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class NoteTemplateListPageViewModel : ViewModelBase, ISingletonDependency
    {
        private readonly IRepository<NoteTemplate, long> repository;
        private readonly IIocResolver iocResolver;
        private readonly NavigationService navigationService;
        private readonly IUnitOfWorkManager unitOfWorkManager;

        public NoteTemplateListPageViewModel(
            IRepository<NoteTemplate, long> repository,
            IIocResolver iocResolver,
            NavigationService navigationService,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            this.Create = new Command(CreateActionAsync);
            Remove = new Command(RemoveAction);
            Edit = new Command(EditAction);
            CreateNote = new Command(CreateNoteAction);
            this.GoToState = new Command(GoToStateAction);
            OpenContextMenu = new Command(OpenContextMenuAction);

            this.repository = repository;
            this.iocResolver = iocResolver;
            this.navigationService = navigationService;
            this.unitOfWorkManager = unitOfWorkManager;
            this.PropertyChanged += NoteTemplatePageViewModel_PropertyChangedAsync;
            //Init();
        }

        private async void OpenContextMenuAction(object obj)
        {
            var tempAppActions = new List<AppAction>();

            var allAppActions = await AppActions.Current.GetAsync();
            var existAppActionIds = new List<long>();
            var appActions = new List<AppAction>();
            foreach (var appAction in allAppActions)
            {
                if (long.TryParse(appAction.Id, out var id))
                {
                    appActions.Add(appAction);
                    existAppActionIds.Add(id);
                }
                else
                {
                    tempAppActions.Add(appAction);

                }
            }
            var availableNoteTemplates = this.NoteTemplates.Select(c => c.NoteTemplate)
            .Where(c => c.CanSimplified)
            .Where(c => !existAppActionIds.Contains(c.Id)).ToList();

            var availableCancelNoteTemplates = this.NoteTemplates.Select(c => c.NoteTemplate)
            .Where(c => c.CanSimplified)
            .Where(c => existAppActionIds.Contains(c.Id)).ToList();


            string firstButton = null;
            var noteTemplateWrapper = (NoteTemplateWrapper)obj;
            if (availableNoteTemplates.Contains(noteTemplateWrapper.NoteTemplate))
            {
                firstButton="PIN到快捷方式";
            }
            else if (availableCancelNoteTemplates.Contains(noteTemplateWrapper.NoteTemplate))
            {
                firstButton="取消PIN到快捷方式";
            }


            var confirmResult = await CommonHelper.ActionSheet(noteTemplateWrapper.NoteTemplate.Title, "取消", null, firstButton, "编辑", "删除", "排序");




            if (confirmResult=="PIN到快捷方式")
            {
                if (AppActions.Current.IsSupported)
                {
                    appActions.Add(new AppAction(noteTemplateWrapper.NoteTemplate.Id.ToString(), noteTemplateWrapper.NoteTemplate.Title));
                    await AppActions.Current.SetAsync(appActions.Concat(tempAppActions));

                }
            }
            else if (confirmResult=="取消PIN到快捷方式")
            {
                if (AppActions.Current.IsSupported)
                {
                    var currentAppAction = appActions.FirstOrDefault(c => c.Id==noteTemplateWrapper.NoteTemplate.Id.ToString());
                    if (currentAppAction!=null)
                    {
                        appActions.Remove(currentAppAction);
                        await AppActions.Current.SetAsync(appActions.Concat(tempAppActions));
                    }


                }
            }
            else if (confirmResult=="编辑")
            {
                this.Edit.Execute(obj);
            }
            else if (confirmResult=="删除")
            {
                this.Remove.Execute(obj);
            }
            else if (confirmResult=="排序")
            {
                this.IsEditing=true;
            }
        }

        private async void CreateActionAsync(object obj)
        {

            using (var objWrapper = iocResolver.ResolveAsDisposable<EditNoteTemplatePage>(new { NoteId = 0 }))
            {
                (objWrapper.Object.BindingContext as EditNoteTemplatePageViewModel).Create.Execute(null);

                await navigationService.PushAsync(objWrapper.Object);
            }
        }

        private async void RemoveAction(object obj)
        {
            var note = (NoteTemplateWrapper)obj;

            var confirmResult = await CommonHelper.Confirm($"是否删除场景「{note.NoteTemplate.Title}」?");
            if (confirmResult == false)
            {
                return;
            }

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

        private async void CreateNoteAction(object obj)
        {
            var noteTemplateWrapper = (NoteTemplateWrapper)obj;
            var note = noteTemplateWrapper.NoteTemplate;

            if (note.CanSimplified)
            {
                using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePageViewModel>())
                {
                    var editNotePageViewModel = objWrapper.Object;
                    editNotePageViewModel.SimplifiedClone.Execute(note.Id);
                    await navigationService.GoPageAsync("Note");

                }

            }
            else
            {
                var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = 0, NoteTemplateId = note.Id });
                (objWrapper.Object.BindingContext as EditNotePageViewModel).NoteSegmentState=Core.Services.NoteSegmentState.Edit;
                objWrapper.Object.Disappearing+=(o, e) =>
                {
                    objWrapper.Dispose();
                };
                await navigationService.PushAsync(objWrapper.Object);

            }

        }

        private void GoToStateAction(object obj)
        {
            this.IsEditing = !this.IsEditing;
        }


        public async Task Init()
        {
            Loading = true;
            await Task.Delay(300);
            await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                await Task.Run(async () =>
                {

                    var noteTemplates = await this.repository.GetAll().Include(c => c.NoteSegmentTemplates).ToListAsync();
                    this.NoteTemplates = new ObservableCollection<NoteTemplateWrapper>(noteTemplates.Select(c => new NoteTemplateWrapper(c) { Container = this }));
                }).ContinueWith((e) => { Loading = false; });
            });
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

        public Command GoToState { get; set; }
        public Command Create { get; set; }
        public Command CreateNote { get; set; }

        public Command Remove { get; set; }
        public Command Edit { get; set; }
        public Command OpenContextMenu { get; set; }
    }

}
