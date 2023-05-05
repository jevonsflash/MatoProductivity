using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using MatoProductivity.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;

namespace MatoProductivity.ViewModels
{
    public class EditNotePageViewModel : ViewModelBase, ITransientDependency
    {
        private readonly IRepository<NoteTemplate, long> templateRepository;
        private readonly IRepository<Note, long> repository;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IIocResolver iocResolver;

        public EditNotePageViewModel(
            IRepository<NoteTemplate, long> templateRepository,
            IRepository<Note, long> repository, IUnitOfWorkManager unitOfWorkManager, IIocResolver iocResolver)
        {
            Submit = new Command(SubmitAction);
            Clone = new Command(CloneAction);
            Create = new Command(CreateAction);
            Remove = new Command(RemoveAction);
            IsConfiguratingNoteSegment = true;
            this.templateRepository = templateRepository;
            this.repository = repository;
            this.unitOfWorkManager = unitOfWorkManager;
            this.iocResolver = iocResolver;
            this.PropertyChanged += EditNotePageViewModel_PropertyChanged;
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
                Init(result);
            });

        }

        private void NoteSegments_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    (item as INoteSegmentViewModel).Submit.Execute(null);
                }
            }
        }

        private void RemoveAction(object obj)
        {
        }

        private void CreateAction(object obj)
        {

            var type = obj as string;
            INoteSegmentViewModel newModel;
            switch (type)
            {
                case "DateTimeSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<DataTimeSegmentViewModel>(new
                    {
                        noteSegment = new NoteSegment()
                        {
                            NoteId = this.NoteId,
                            Title = "Add DateTimeSegment Test",
                            Type = type,
                            Desc = "TestDescDesc",
                            NoteSegmentPayloads = new List<NoteSegmentPayload>()

                        }
                    }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "TextSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TextSegmentViewModel>(new
                    {
                        noteSegment = new NoteSegment()
                        {
                            NoteId = this.NoteId,
                            Title = "Add TextSegment Test",
                            Type = type,
                            Desc = "TestDescDesc",
                            NoteSegmentPayloads = new List<NoteSegmentPayload>()
                        }

                    }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                default:
                    newModel = null;
                    break;
            }
            if (newModel != null)
            {
                newModel.Create.Execute(null);
                this.NoteSegments.Add(newModel);
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
            var noteSegments = note.NoteSegments;
            this.NoteSegments = new ObservableCollection<INoteSegmentViewModel>(

              noteSegments.Select(c => GetNoteSegmentViewModel(c))
              );
            this.NoteSegments.CollectionChanged += NoteSegments_CollectionChanged;
        }

        private INoteSegmentViewModel GetNoteSegmentViewModel(NoteSegment c)
        {
            var type = c.Type;
            INoteSegmentViewModel result;
            switch (type)
            {
                case "DateTimeSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<DataTimeSegmentViewModel>(new { noteSegment = c }))
                    {
                        result = objWrapper.Object;
                    }
                    break;
                case "TextSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TextSegmentViewModel>(new { noteSegment = c }))
                    {
                        result = objWrapper.Object;
                    }
                    break;
                default:
                    result = null;
                    break;
            }

            return result;
        }

        private long noteId;

        public long NoteId
        {
            get { return noteId; }
            set
            {
                noteId = value;
                RaisePropertyChanged();
            }
        }
        private ObservableCollection<INoteSegmentViewModel> _noteSegments;

        public ObservableCollection<INoteSegmentViewModel> NoteSegments
        {
            get { return _noteSegments; }
            set
            {
                _noteSegments = value;
                RaisePropertyChanged();
            }
        }

        private ObservableCollection<INoteSegmentViewModel> _selectedNoteSegments;

        public ObservableCollection<INoteSegmentViewModel> SelectedNoteSegments
        {
            get { return _selectedNoteSegments; }
            set
            {
                _selectedNoteSegments = value;
                RaisePropertyChanged();
            }
        }

        private INoteSegmentViewModel _selectedNoteSegment;

        public INoteSegmentViewModel SelectedNoteSegment
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
        public Command Remove { get; set; }

    }
}
