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
    public class NotePageViewModel : ViewModelBase, ITransientDependency
    {
        private readonly IRepository<Note, long> repository;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IIocResolver iocResolver;

        public NotePageViewModel(
            IRepository<Note, long> repository, IUnitOfWorkManager unitOfWorkManager, IIocResolver iocResolver)
        {
            Remove = new Command(RemoveAction);
            this.repository = repository;
            this.unitOfWorkManager = unitOfWorkManager;
            this.iocResolver = iocResolver;
            this.PropertyChanged += NotePageViewModel_PropertyChanged;
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

        private async void RemoveAction(object obj)
        {
            await repository.DeleteAsync(this.NoteId);
        }


        private async void NotePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
            result.NoteSegmentState = NoteSegmentState.PreView;
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


        public Command Remove { get; set; }

    }
}
