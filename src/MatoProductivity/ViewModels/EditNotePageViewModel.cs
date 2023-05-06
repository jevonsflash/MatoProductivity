using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.Services;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Reflection;

namespace MatoProductivity.ViewModels
{
    public class EditNotePageViewModel : ViewModelBase, ITransientDependency
    {
        private readonly NavigationService navigationService;
        private readonly INoteSegmentServiceFactory noteSegmentServiceFactory;
        private readonly IRepository<NoteTemplate, long> templateRepository;
        private readonly IRepository<Note, long> repository;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IIocResolver iocResolver;
        private Popup noteSegmentStoreListPage;

        public EditNotePageViewModel(
            NavigationService navigationService,
            INoteSegmentServiceFactory noteSegmentServiceFactory,
            IRepository<NoteTemplate, long> templateRepository,
            IRepository<Note, long> repository, IUnitOfWorkManager unitOfWorkManager, IIocResolver iocResolver)
        {
            Submit = new Command(SubmitAction);
            Clone = new Command(CloneAction);
            CreateSegment = new Command(CreateSegmentAction);
            CreateSegmentFromStore = new Command(CreateSegmentFromStoreAction);
            RemoveSegment = new Command(RemoveSegmentAction);
            Create = new Command(CreateAction);
            Remove = new Command(RemoveAction);
            SelectAllSegment = new Command(SelectAllSegmentAction);
            RemoveSelectedSegment = new Command(RemoveSelectedSegmentAction);
            IsConfiguratingNoteSegment = true;
            this.navigationService = navigationService;
            this.noteSegmentServiceFactory = noteSegmentServiceFactory;
            this.templateRepository = templateRepository;
            this.repository = repository;
            this.unitOfWorkManager = unitOfWorkManager;
            this.iocResolver = iocResolver;
            this.PropertyChanged += EditNotePageViewModel_PropertyChanged;
            SelectedNoteSegments = new ObservableCollection<object>();

        }

        private void SelectAllSegmentAction(object obj)
        {
            foreach (var noteSegments in NoteSegments)
            {
                SelectedNoteSegments.Add(noteSegments);
            }
        }

        private void RemoveSelectedSegmentAction(object obj)
        {
            foreach (var noteSegments in SelectedNoteSegments.ToList())
            {
                NoteSegments.Remove((INoteSegmentService)noteSegments);
            }
        }

        private async void RemoveAction(object obj)
        {
            await repository.DeleteAsync(this.NoteId);
            await navigationService.PopAsync();

        }

        private async void CreateAction(object obj)
        {
            await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var note = new Note() { Title = "未命名" };
                var result = await repository.InsertAsync(note);
                await unitOfWorkManager.Current.SaveChangesAsync();

                Init(result);
            });
        }

        private async void CloneAction(object obj)
        {
            var id = (long)obj;
            await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var noteTemplate = await templateRepository.GetAll().Include(c => c.NoteSegmentTemplates)
                              .ThenInclude(c => c.NoteSegmentTemplatePayloads)
                              .Where(c => c.Id == id).FirstOrDefaultAsync();
                var note = ObjectMapper.Map<Note>(noteTemplate);

                var result = await repository.InsertAsync(note);
                await unitOfWorkManager.Current.SaveChangesAsync();

                Init(result);
            });

        }


        private void RemoveSegmentAction(object obj)
        {
            NoteSegments.Remove(obj as INoteSegmentService);
        }

        private async void CreateSegmentAction(object obj)
        {
            var type = obj as string;
            var noteSegment = new NoteSegment()
            {
                NoteId = this.NoteId,
                Title = "Add " + type + " Test",
                Type = type,
                Desc = "TestDescDesc",
                NoteSegmentPayloads = new List<NoteSegmentPayload>()

            };
            var newModel = noteSegmentServiceFactory.GetNoteSegmentService(noteSegment);
            if (newModel != null)
            {
                newModel.Create.Execute(null);
                this.NoteSegments.Add(newModel);
            }
        }

        private async void EditNotePageViewModel_OnFinishedChooise(object sender, NoteSegment noteSegment)
        {
            noteSegment.NoteId = this.NoteId;
            noteSegment.NoteSegmentPayloads = new List<NoteSegmentPayload>();

            var newModel = noteSegmentServiceFactory.GetNoteSegmentService(noteSegment);
            if (newModel != null)
            {
                newModel.Create.Execute(null);
                this.NoteSegments.Add(newModel);
            }
           (sender as NoteSegmentStoreListPageViewModel).OnFinishedChooise -= EditNotePageViewModel_OnFinishedChooise;
            await navigationService.HidePopupAsync(noteSegmentStoreListPage);
        }


        private async void CreateSegmentFromStoreAction(object obj)
        {
            using (var objWrapper = iocResolver.ResolveAsDisposable<NoteSegmentStoreListPage>())
            {
                noteSegmentStoreListPage = objWrapper.Object;
                (noteSegmentStoreListPage.BindingContext as NoteSegmentStoreListPageViewModel).OnFinishedChooise += EditNotePageViewModel_OnFinishedChooise;
                await navigationService.ShowPopupAsync(noteSegmentStoreListPage);
            }
        }

        private async void EditNotePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteId))
            {
                if (NoteId != default)
                {
                    await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                    {
                        var note = await this.repository
                            .GetAll()
                            .Include(c => c.NoteSegments)
                            .ThenInclude(c => c.NoteSegmentPayloads)
                            .Where(c => c.Id == this.NoteId).FirstOrDefaultAsync();
                        Init(note);

                    });
                }

            }
            else if (e.PropertyName == nameof(NoteSegments))
            {
                RaisePropertyChanged(nameof(CanSimplified));
            }
            else if (e.PropertyName == nameof(IsConfiguratingNoteSegment))
            {
                if (NoteSegments != null)
                {
                    foreach (var noteSegment in NoteSegments)
                    {
                        noteSegment.NoteSegmentState = IsConfiguratingNoteSegment ? NoteSegmentState.Config : NoteSegmentState.Edit;
                    }
                }

            }

        }

        private void Init(Note note)
        {
            if (note != null)
            {
                var noteSegments = note.NoteSegments;
                this.noteId = note.Id;
                this.NoteSegments = noteSegments != null
                    ? new ObservableCollection<INoteSegmentService>(
                  noteSegments.Select(noteSegmentServiceFactory.GetNoteSegmentService))
                    : new ObservableCollection<INoteSegmentService>();

            }

        }



        private long noteId;

        public long NoteId
        {
            get { return noteId; }
            set
            {
                if (noteId != value)
                {
                    noteId = value;
                    RaisePropertyChanged();
                }

            }
        }
        private ObservableCollection<INoteSegmentService> _noteSegments;

        public ObservableCollection<INoteSegmentService> NoteSegments
        {
            get { return _noteSegments; }
            set
            {
                _noteSegments = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<object> _selectedNoteSegments;

        public ObservableCollection<object> SelectedNoteSegments
        {
            get { return _selectedNoteSegments; }
            set
            {
                _selectedNoteSegments = value;
                RaisePropertyChanged();
            }
        }

        private INoteSegmentService _selectedNoteSegment;

        public INoteSegmentService SelectedNoteSegment
        {
            get { return _selectedNoteSegment; }
            set
            {
                _selectedNoteSegment = value;
                RaisePropertyChanged();
            }
        }

        private bool _isConfiguratingNoteSegment;

        public bool IsConfiguratingNoteSegment
        {
            get { return _isConfiguratingNoteSegment; }
            set
            {
                _isConfiguratingNoteSegment = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(SelectionMode));

            }
        }

        public SelectionMode SelectionMode => IsConfiguratingNoteSegment ? SelectionMode.Multiple : SelectionMode.Single;

        public bool CanSimplified => NoteSegments == null ? false : NoteSegments.All(this.GetIsItemSimplified);

        public bool GetIsItemSimplified(INoteSegmentService noteSegment)
        {
            foreach (var interfaceType in noteSegment.GetType().GetInterfaces())
            {
                if (!interfaceType.GetTypeInfo().IsGenericType && interfaceType == typeof(IAutoSet))
                {
                    return (noteSegment as IAutoSet).IsAutoSet;
                }
            }
            return false;
        }

        private void SubmitAction(object obj)
        {
            foreach (var noteSegment in NoteSegments)
            {
                noteSegment.Submit.Execute(null);
            }
        }
        public Command Submit { get; set; }
        public Command Clone { get; set; }



        public Command Create { get; set; }
        public Command CreateSegment { get; set; }
        public Command CreateSegmentFromStore { get; set; }
        public Command Remove { get; set; }
        public Command RemoveSegment { get; set; }
        public Command RemoveSelectedSegment { get; set; }
        public Command SelectAllSegment { get; set; }

    }
}
