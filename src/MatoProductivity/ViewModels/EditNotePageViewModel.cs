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
        private readonly IRepository<Note, long> repository;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IIocResolver iocResolver;

        public EditNotePageViewModel(IRepository<Note, long> repository, IUnitOfWorkManager unitOfWorkManager, IIocResolver iocResolver)
        {
            Submit = new Command(SubmitAction);
            Create = new Command(CreateAction);
            this.repository = repository;
            this.unitOfWorkManager = unitOfWorkManager;
            this.iocResolver = iocResolver;
            this.PropertyChanged += EditNotePageViewModel_PropertyChanged;
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


                            var noteSegments = note.NoteSegments;
                            this.NoteSegments = new ObservableCollection<INoteSegmentViewModel>(

                          noteSegments.Select(c => GetNoteSegmentViewModel(c))
                          );
                    });
                }
            }



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
        private void SubmitAction(object obj)
        {
            foreach (var noteSegment in NoteSegments)
            {
                noteSegment.Submit.Execute(null);
            }
        }
        public Command Submit { get; set; }
        public Command Create { get; set; }

    }
}
