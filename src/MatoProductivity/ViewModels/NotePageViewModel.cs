using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System.Reflection;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.Services;
using MatoProductivity.Core.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using MatoProductivity.Services;
using MatoProductivity.Views;

namespace MatoProductivity.ViewModels
{
    public class NotePageViewModel : ViewModelBase, ITransientDependency
    {
        private readonly NavigationService navigationService;
        private readonly INoteSegmentServiceFactory noteSegmentServiceFactory;
        private readonly IRepository<Note, long> repository;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IIocResolver iocResolver;

        public NotePageViewModel(
            NavigationService navigationService,

            INoteSegmentServiceFactory noteSegmentServiceFactory,
            IRepository<Note, long> repository,
            IUnitOfWorkManager unitOfWorkManager,
            IIocResolver iocResolver)
        {
            Remove = new Command(RemoveAction);
            Edit = new Command(EditAction);
            this.navigationService = navigationService;
            this.noteSegmentServiceFactory = noteSegmentServiceFactory;
            this.repository = repository;
            this.unitOfWorkManager = unitOfWorkManager;
            this.iocResolver = iocResolver;
            this.PropertyChanged += NotePageViewModel_PropertyChanged;
        }

        private async void EditAction(object obj)
        {
            using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = this.NoteId }))
            {
                await navigationService.PopAsync(false);
                await navigationService.PushAsync(objWrapper.Object);
            }
        }

        private async void RemoveAction(object obj)
        {
            await repository.DeleteAsync(this.NoteId);
            await navigationService.PopAsync();

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
            else if (e.PropertyName == nameof(NoteSegments))
            {
                RaisePropertyChanged(nameof(CanSimplified));
            }
        }

        private void Init(Note note)
        {
            var noteSegments = note.NoteSegments;
            this.NoteSegments = new ObservableCollection<INoteSegmentService>(

              noteSegments.Select(GetNoteSegmentViewModel)
              );
        }

        private INoteSegmentService GetNoteSegmentViewModel(NoteSegment c)
        {
            var result = noteSegmentServiceFactory.GetNoteSegmentService(c);
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

        public bool CanSimplified => NoteSegments.All(this.GetIsItemSimplified);

        public bool GetIsItemSimplified(INoteSegmentService noteSegment)
        {
            foreach (var interfaceType in noteSegment.GetType().GetInterfaces())
            {
                if (!interfaceType.GetTypeInfo().IsGenericType && interfaceType == typeof(IAutoSet))
                {
                    return true;
                }
            }
            return false;
        }

        public Command Remove { get; set; }
        public Command Edit { get; set; }

    }
}
