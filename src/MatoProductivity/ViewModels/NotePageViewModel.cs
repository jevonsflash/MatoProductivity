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
using MatoProductivity.Core.ViewModels;
using System.Drawing;

namespace MatoProductivity.ViewModels
{
    public class NotePageViewModel : ViewModelBase, ITransientDependency, IReadOnlyNoteSegmentServiceContainer
    {
        private readonly NavigationService navigationService;
        private readonly INoteSegmentServiceFactory noteSegmentServiceFactory;
        private readonly IRepository<Note, long> repository;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IIocResolver iocResolver;

        public event EventHandler OnDone;
        public NotePageViewModel(
            NavigationService navigationService,
            INoteSegmentServiceFactory noteSegmentServiceFactory,
            IRepository<Note, long> repository,
            IUnitOfWorkManager unitOfWorkManager,
            IIocResolver iocResolver)
        {
            Remove = new Command(RemoveAction);
            Edit = new Command(EditAction);
            Share = new Command(ShareAction);
            this.navigationService = navigationService;
            this.noteSegmentServiceFactory = noteSegmentServiceFactory;
            this.repository = repository;
            this.unitOfWorkManager = unitOfWorkManager;
            this.iocResolver = iocResolver;
            this.PropertyChanged += NotePageViewModel_PropertyChanged;

        }

        private void ShareAction(object obj)
        {
        }

        private async void EditAction(object obj)
        {
            using (var objWrapper = iocResolver.ResolveAsDisposable<EditNotePage>(new { NoteId = this.NoteId }))
            {
                await navigationService.PushAsync(objWrapper.Object);
                OnDone?.Invoke(this, EventArgs.Empty);

            }
        }

        private async void RemoveAction(object obj)
        {
            await repository.DeleteAsync(this.NoteId);
            OnDone?.Invoke(this, EventArgs.Empty);

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
            Title = note.Title;
            Desc = note.Desc;
            Icon = note.Icon;
            Color = note.Color;
            BackgroundColor = note.BackgroundColor;
            PreViewContent = note.PreViewContent;
            IsEditable = note.IsEditable;

            foreach (var noteSegment in NoteSegments)
            {
                noteSegment.Container = this;
            }
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

        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }



        private string _desc;

        public string Desc
        {
            get { return _desc; }
            set
            {
                _desc = value;
                RaisePropertyChanged();
            }
        }

        private string _icon;

        public string Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;
                RaisePropertyChanged();
            }
        }

        private string _color;

        public string Color
        {
            get { return _color; }
            set
            {
                _color = value;
                RaisePropertyChanged();
            }
        }

        private string _backgroundColor;


        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                RaisePropertyChanged();
            }
        }


        private string _preViewContent;

        public string PreViewContent
        {
            get { return _preViewContent; }
            set
            {
                _preViewContent = value;
                RaisePropertyChanged();
            }
        }

        private bool _isEditable;

        public bool IsEditable
        {
            get { return _isEditable; }
            set
            {
                _isEditable = value;
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
        public Command Share { get; set; }

    }
}
