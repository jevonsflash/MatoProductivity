using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.Services;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;

namespace MatoProductivity.ViewModels
{
    public class EditNotePageViewModel : ViewModelBase, ITransientDependency, INoteSegmentServiceContainer, IDraggableViewModel
    {
        private readonly NavigationService navigationService;
        private readonly INoteSegmentServiceFactory noteSegmentServiceFactory;
        private readonly IRepository<NoteTemplate, long> templateRepository;
        private readonly IRepository<NoteSegment, long> noteSegmentRepository;
        private readonly IRepository<NoteSegmentPayload, long> payloadRepository;
        private readonly IRepository<NoteSegmentStore, long> noteSegmentStoreRepository;
        private readonly IRepository<Note, long> repository;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IIocResolver iocResolver;
        private Popup noteSegmentStoreListPage;

        public EditNotePageViewModel(
            NavigationService navigationService,
            INoteSegmentServiceFactory noteSegmentServiceFactory,
            IRepository<NoteTemplate, long> templateRepository,
            IRepository<NoteSegment, long> noteSegmentRepository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            IRepository<NoteSegmentStore, long> noteSegmentStoreRepository,

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

            ItemDragged = new Command(OnItemDragged);
            ItemDraggedOver = new Command(OnItemDraggedOver);
            ItemDragLeave = new Command(OnItemDragLeave);
            ItemDropped = new Command(i => OnItemDropped(i));
            Back=new Command(BackAction);
            SwitchState=new Command(SwitchStateAction);

            IsConfiguratingNoteSegment = false;
            this.navigationService = navigationService;
            this.noteSegmentServiceFactory = noteSegmentServiceFactory;
            this.templateRepository = templateRepository;
            this.noteSegmentRepository = noteSegmentRepository;
            this.payloadRepository=payloadRepository;
            this.noteSegmentStoreRepository=noteSegmentStoreRepository;
            this.repository = repository;
            this.unitOfWorkManager = unitOfWorkManager;
            this.iocResolver = iocResolver;
            this.PropertyChanged += EditNotePageViewModel_PropertyChanged;
            SelectedNoteSegments = new ObservableCollection<object>();

        }

        private async void BackAction(object obj)
        {
            await this.navigationService.PopAsync();
        }

        private void SwitchStateAction(object obj)
        {
            this.IsConfiguratingNoteSegment= !this.IsConfiguratingNoteSegment;
        }
        private void OnItemDragged(object item)
        {
            foreach (var noteSegment in NoteSegments)
            {
                noteSegment.IsBeingDragged = noteSegment == item;
            }
        }

        private void OnItemDraggedOver(object item)
        {

            var itemBeingDragged = NoteSegments.FirstOrDefault(i => i.IsBeingDragged);
            foreach (var noteSegment in NoteSegments)
            {
                noteSegment.IsBeingDraggedOver = item == noteSegment && item != itemBeingDragged;
            }

        }

        private void OnItemDragLeave(object item)
        {
            foreach (var noteSegment in NoteSegments)
            {
                noteSegment.IsBeingDraggedOver = false;
            }
        }

        private void OnItemDropped(object item)
        {
            var itemToMove = NoteSegments.First(i => i.IsBeingDragged);
            var itemToInsertBefore = item as INoteSegmentService;

            if (itemToMove == null || itemToInsertBefore == null || itemToMove == itemToInsertBefore)
                return;


            NoteSegments.Remove(itemToMove);

            var insertAtIndex = NoteSegments.IndexOf(itemToInsertBefore);

            NoteSegments.Insert(insertAtIndex, itemToMove);
            itemToMove.IsBeingDragged = false;
            itemToInsertBefore.IsBeingDraggedOver = false;
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
            NoteSegment note = default;
            await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var noteTemplate = await noteSegmentStoreRepository.GetAll()
                              .Where(c => c.Type == type).FirstOrDefaultAsync();
                note = ObjectMapper.Map<NoteSegment>(noteTemplate);

            });
            if (note!=default)
            {
                var noteSegment = new NoteSegment()
                {
                    NoteId = this.NoteId,
                    Title = note.Title,
                    Type = type,
                    Desc = note.Desc,
                    Icon=note.Icon,
                    NoteSegmentPayloads = new List<NoteSegmentPayload>()

                };


                var newModel = noteSegmentServiceFactory.GetNoteSegmentService(noteSegment);
                if (newModel != null)
                {
                    newModel.Create.Execute(null);
                    newModel.NoteSegmentState = IsConfiguratingNoteSegment ? NoteSegmentState.Config : NoteSegmentState.Edit;
                    newModel.Container = this;
                    this.NoteSegments.Add(newModel);
                }
            }
        }

        private async void EditNotePageViewModel_OnFinishedChooise(object sender, NoteSegmentStore noteSegmentStore)
        {

            var noteSegment = ObjectMapper.Map<NoteSegment>(noteSegmentStore);

            noteSegment.NoteId = this.NoteId;
            noteSegment.NoteSegmentPayloads = new List<NoteSegmentPayload>();

            var newModel = noteSegmentServiceFactory.GetNoteSegmentService(noteSegment);
            if (newModel != null)
            {
                newModel.Create.Execute(null);
                newModel.NoteSegmentState = IsConfiguratingNoteSegment ? NoteSegmentState.Config : NoteSegmentState.Edit;
                newModel.Container = this;
                this.NoteSegments.Add(newModel);
            }
           (sender as NoteSegmentStoreListPageViewModel).OnFinishedChooise -= EditNotePageViewModel_OnFinishedChooise;
            await navigationService.HidePopupAsync(noteSegmentStoreListPage);
            noteSegmentStoreListPage=null;
        }


        private async void CreateSegmentFromStoreAction(object obj)
        {
            if (noteSegmentStoreListPage!=null)
            {
                (noteSegmentStoreListPage.BindingContext as NoteSegmentStoreListPageViewModel).OnFinishedChooise -= EditNotePageViewModel_OnFinishedChooise;
                noteSegmentStoreListPage=null;
            }

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
                  noteSegments.Select(GetNoteSegmentViewModel))
                    : new ObservableCollection<INoteSegmentService>();
                Title = note.Title;
                Desc = note.Desc;
                Icon = note.Icon;
                Color = note.Color;
                BackgroundColor = note.BackgroundColor;
                PreViewContent = note.PreViewContent;
                IsEditable = note.IsEditable;

            }

        }
        private INoteSegmentService GetNoteSegmentViewModel(NoteSegment c)
        {
            var result = noteSegmentServiceFactory.GetNoteSegmentService(c);
            result.NoteSegmentState = NoteSegmentState.Edit;
            result.Container = this;

            return result;
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

        private async void SubmitAction(object obj)
        {
            foreach (var noteSegment in NoteSegments)
            {
                noteSegment.Submit.Execute(null);

            }
            await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                await this.repository.UpdateAsync(this.NoteId, (note) =>
                {
                    note.Title = this.Title;
                    note.Desc = this.Desc;
                    note.Icon = this.Icon;
                    note.Color = this.Color;
                    note.BackgroundColor = this.BackgroundColor;
                    note.PreViewContent = this.PreViewContent;
                    note.IsEditable = this.IsEditable;
                    return Task.FromResult(note);
                });

                var noteSegments = await noteSegmentRepository.GetAllListAsync(c => c.NoteId == this.NoteId);

                foreach (var noteSegmentService in NoteSegments)
                {
                    var noteSegment = noteSegmentService.NoteSegment;
                    if (!noteSegments.Any(c => c.Id == (noteSegment as NoteSegment).Id))
                    {
                        var newNoteSegment = new NoteSegment();
                        newNoteSegment.Id = (noteSegment as NoteSegment).Id;
                        newNoteSegment.NoteId = (noteSegment as NoteSegment).NoteId;
                        newNoteSegment.Title = noteSegment.Title;
                        newNoteSegment.Type=noteSegment.Type;
                        newNoteSegment.Status=noteSegment.Status;
                        newNoteSegment.Desc=noteSegment.Desc;
                        newNoteSegment.Icon=noteSegment.Icon;
                        newNoteSegment.Color=noteSegment.Color;
                        newNoteSegment.Rank=noteSegment.Rank;
                        newNoteSegment.IsHidden=noteSegment.IsHidden;
                        newNoteSegment.IsRemovable=noteSegment.IsRemovable;

                        newNoteSegment.NoteSegmentPayloads= (noteSegment as NoteSegment).NoteSegmentPayloads.Select(c => new NoteSegmentPayload ()
                        {
                            NoteSegmentId=newNoteSegment.Id,
                            Key=c.Key,
                            Value=c.Value,
                            ValueType=c.ValueType

                        }).ToList();
                        var entity = await noteSegmentRepository.InsertAsync(newNoteSegment);
                    }
                }

                foreach (var noteSegment in noteSegments)
                {
                    if (!NoteSegments.Select(c => c.NoteSegment)
                          .Where(c => !(c as NoteSegment).IsTransient())
                          .Any(c => (c as NoteSegment).Id == noteSegment.Id))
                    {
                        await noteSegmentRepository.DeleteAsync(noteSegment.Id);
                    }
                    else
                    {
                        var newNoteSegment = NoteSegments.Select(c => c.NoteSegment).FirstOrDefault(c => (c as NoteSegment).Id==noteSegment.Id);
                        noteSegment.Id = (newNoteSegment as NoteSegment).Id;
                        noteSegment.NoteId = (newNoteSegment as NoteSegment).NoteId;
                        noteSegment.Title = newNoteSegment.Title;
                        noteSegment.Type=newNoteSegment.Type;
                        noteSegment.Status=newNoteSegment.Status;
                        noteSegment.Desc=newNoteSegment.Desc;
                        noteSegment.Icon=newNoteSegment.Icon;
                        noteSegment.Color=newNoteSegment.Color;
                        noteSegment.Rank=newNoteSegment.Rank;
                        noteSegment.IsHidden=newNoteSegment.IsHidden;
                        noteSegment.IsRemovable=newNoteSegment.IsRemovable;


                        var entity = await noteSegmentRepository.UpdateAsync(noteSegment);


                        var payloadEntities = await payloadRepository.GetAllListAsync(c => c.NoteSegmentId == entity.Id);
                        foreach (var item in entity.NoteSegmentPayloads)
                        {
                            if (!payloadEntities.Any(c => c.Key == item.Key))
                            {
                                item.NoteSegmentId=entity.Id;
                                await payloadRepository.InsertAsync(item);

                            }
                        }


                        foreach (var payloadEntity in payloadEntities)
                        {
                            var currentPayload = newNoteSegment.GetNoteSegmentPayload(payloadEntity.Key);
                            if (currentPayload == null)
                            {
                                await payloadRepository.DeleteAsync(payloadEntity);
                            }
                            else
                            {
                                payloadEntity.Value = currentPayload.Value;
                                payloadEntity.ValueType = currentPayload.ValueType;
                                await payloadRepository.UpdateAsync(payloadEntity);
                            }
                        }

                    }
                }
                UnitOfWorkManager.Current.SaveChanges();


            });

            await navigationService.PopAsync();

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

        //Drag and drop
        public Command ItemDragged { get; set; }

        public Command ItemDraggedOver { get; set; }

        public Command ItemDragLeave { get; set; }

        public Command ItemDropped { get; set; }

        public Command SwitchState { get; set; }

        public Command Back { get; set; }


    }
}
