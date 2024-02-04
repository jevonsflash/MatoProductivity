﻿using Abp.Dependency;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.Services;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
using MatoProductivity.Services;
using MatoProductivity.Views;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Input;

namespace MatoProductivity.ViewModels
{
    public class EditNoteTemplatePageViewModel : ViewModelBase, ITransientDependency, INoteSegmentServiceContainer, IDraggableViewModel, IPopupContainerViewModelBase, IDisposable
    {
        private readonly NavigationService navigationService;
        private readonly INoteSegmentTemplateServiceFactory noteSegmentServiceFactory;
        private readonly IRepository<Note, long> noteRepository;
        private readonly IRepository<NoteSegmentTemplate, long> noteSegmentTemplateRepository;
        private readonly IRepository<NoteSegmentTemplatePayload, long> payloadRepository;
        private readonly IRepository<NoteSegmentStore, long> noteSegmentStoreRepository;
        private readonly IRepository<NoteTemplate, long> repository;
        private readonly IUnitOfWorkManager unitOfWorkManager;
        private readonly IIocResolver iocResolver;
        private Popup noteSegmentStoreListPage;
        private Popup iconSelectingPage;

        public EditNoteTemplatePageViewModel(
            NavigationService navigationService,
            INoteSegmentTemplateServiceFactory noteSegmentServiceFactory,
            IRepository<Note, long> noteRepository,
            IRepository<NoteSegmentTemplate, long> noteSegmentTemplateRepository,
            IRepository<NoteSegmentTemplatePayload, long> payloadRepository,
            IRepository<NoteSegmentStore, long> noteSegmentStoreRepository,

            IRepository<NoteTemplate, long> repository, IUnitOfWorkManager unitOfWorkManager, IIocResolver iocResolver)
        {
            Submit = new Command(SubmitAction);
            Clone = new Command(CloneAction);
            CreateSegment = new Command(CreateSegmentAction);
            CreateSegmentFromStore = new Command(CreateSegmentFromStoreAction, (o) => !PopupLoading);
            RemoveSegment = new Command(RemoveSegmentAction);
            Create = new Command(CreateAction);
            Remove = new Command(RemoveAction);
            SelectAllSegment = new Command(SelectAllSegmentAction);
            RemoveSelectedSegment = new Command(RemoveSelectedSegmentAction);
            SubmitBack = new Command(SubmitBackAction);
            ItemDragged = new Command(OnItemDragged);
            ItemDraggedOver = new Command(OnItemDraggedOver);
            ItemDragLeave = new Command(OnItemDragLeave);
            ItemDropped = new Command(i => OnItemDropped(i));
            Back = new Command(BackAction);
            GoToState = new Command(GoToStateAction);
            ChooseIcon = new Command(ChooseIconAction);

            this.navigationService = navigationService;
            this.noteSegmentServiceFactory = noteSegmentServiceFactory;
            this.noteRepository = noteRepository;
            this.noteSegmentTemplateRepository = noteSegmentTemplateRepository;
            this.payloadRepository = payloadRepository;
            this.noteSegmentStoreRepository = noteSegmentStoreRepository;
            this.repository = repository;
            this.unitOfWorkManager = unitOfWorkManager;
            this.iocResolver = iocResolver;
            this.PropertyChanged += EditNoteTemplatePageViewModel_PropertyChanged;
            SelectedNoteSegments = new ObservableCollection<object>();
            NoteSegmentState=NoteSegmentState.Edit;

        }

        private async void ChooseIconAction(object obj)
        {

            PopupLoading = true;

            using (var objWrapper = iocResolver.ResolveAsDisposable<IconSelectingPage>())
            {
                iconSelectingPage = objWrapper.Object;
                (iconSelectingPage.BindingContext as IconSelectingPageViewModel).OnFinishedChooise +=EditNoteTemplatePageViewModel_OnFinishedChooise1;
            }

            await navigationService.ShowPopupAsync(iconSelectingPage).ContinueWith((e) =>
            {
                if (iconSelectingPage!=null)
                {
                    (iconSelectingPage.BindingContext as IconSelectingPageViewModel).OnFinishedChooise -= EditNoteTemplatePageViewModel_OnFinishedChooise1;
                    iconSelectingPage = null;
                }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PopupLoading = false;
                });

            });


        }

        private async void EditNoteTemplatePageViewModel_OnFinishedChooise1(object sender, string e)
        {
            this.Icon=e;
            await navigationService.HidePopupAsync(iconSelectingPage);

        }

        private async void SubmitBackAction(object obj)
        {
            if (this.NoteSegmentState==NoteSegmentState.Edit ||this.NoteSegmentState==NoteSegmentState.Config)
            {
                var confirmResult = await CommonHelper.ActionSheet("您有未保存的修改，是否保存？", "取消", null, "保存", "放弃修改");
                if (confirmResult=="保存")
                {
                    this.SubmitAction(obj);
                    this.BackAction(obj);
                }
                else if (confirmResult=="放弃修改")
                {
                    this.BackAction(obj);

                }
            }
            else
            {
                this.BackAction(obj);

            }

        }
        private async void BackAction(object obj)
        {
            await this.navigationService.PopAsync();
        }

        private void GoToStateAction(object obj)
        {
            if (obj is NoteSegmentState)
            {

                if ((NoteSegmentState)obj==NoteSegmentState.PreView &&(this.NoteSegmentState==NoteSegmentState.Config || this.NoteSegmentState==NoteSegmentState.Edit))
                {
                    //this.SubmitBackAction(null);
                    this.SubmitAction(null);
                }

                this.NoteSegmentState=(NoteSegmentState)obj;

            }
        }
        private void OnItemDragged(object item)
        {
            foreach (var noteSegmentTemplate in NoteSegments)
            {
                noteSegmentTemplate.IsBeingDragged = noteSegmentTemplate == item;
            }
        }

        private void OnItemDraggedOver(object item)
        {

            var itemBeingDragged = NoteSegments.FirstOrDefault(i => i.IsBeingDragged);
            foreach (var noteSegmentTemplate in NoteSegments)
            {
                noteSegmentTemplate.IsBeingDraggedOver = item == noteSegmentTemplate && item != itemBeingDragged;
            }

        }

        private void OnItemDragLeave(object item)
        {
            foreach (var noteSegmentTemplate in NoteSegments)
            {
                noteSegmentTemplate.IsBeingDraggedOver = false;
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
            foreach (var noteSegmentTemplates in NoteSegments)
            {
                SelectedNoteSegments.Add(noteSegmentTemplates);
            }
        }

        private void RemoveSelectedSegmentAction(object obj)
        {
            foreach (var noteSegmentTemplates in SelectedNoteSegments.ToList())
            {
                NoteSegments.Remove((INoteSegmentService)noteSegmentTemplates);
            }
        }

        private async void RemoveAction(object obj)
        {
            var confirmResult = await CommonHelper.Confirm($"是否删除场景「{this.Title}」?");
            if (confirmResult == false)
            {
                return;
            }
            await repository.DeleteAsync(this.NoteTemplateId);
            await navigationService.PopAsync();

        }

        private async void CreateAction(object obj)
        {
            Loading = true;
            await Task.Run(async () =>
            {
                await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {
                    var note = new NoteTemplate() { Title = "新建模板" };
                    var result = await repository.InsertAsync(note);
                    await unitOfWorkManager.Current.SaveChangesAsync();

                    Init(result);
                });
            }).ContinueWith((e) => { Loading = false; });
        }

        private async void CloneAction(object obj)
        {
            Loading = true;
            await Task.Run(async () =>
            {
                var id = (long)obj;
                await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                {
                    var note = await noteRepository.GetAll().Include(c => c.NoteSegments)
                                  .ThenInclude(c => c.NoteSegmentPayloads)
                                  .Where(c => c.Id == id).FirstOrDefaultAsync();
                    var noteTemplate = ObjectMapper.Map<NoteTemplate>(note);

                    var result = await repository.InsertAsync(noteTemplate);
                    await unitOfWorkManager.Current.SaveChangesAsync();

                    Init(result);
                });
            }).ContinueWith((e) => { Loading = false; });

        }


        private void RemoveSegmentAction(object obj)
        {
            NoteSegments.Remove(obj as INoteSegmentService);
        }

        private async void CreateSegmentAction(object obj)
        {
            var type = obj as string;
            NoteSegmentTemplate note = default;
            await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var noteTemplate = await noteSegmentStoreRepository.GetAll()
                              .Where(c => c.Type == type).FirstOrDefaultAsync();
                note = ObjectMapper.Map<NoteSegmentTemplate>(noteTemplate);

            });
            _CreateSegment(note);

        }

        private async void EditNoteTemplatePageViewModel_OnFinishedChooise(object sender, NoteSegmentStore noteSegmentStore)
        {
            var note = ObjectMapper.Map<NoteSegmentTemplate>(noteSegmentStore);
            _CreateSegment(note);
            await navigationService.HidePopupAsync(noteSegmentStoreListPage);
        }

        private void _CreateSegment(NoteSegmentTemplate note)
        {
            if (note != default)
            {
                var noteSegmentTemplate = new NoteSegmentTemplate()
                {
                    NoteTemplateId = this.NoteTemplateId,
                    Title = note.Title,
                    Type = note.Type,
                    Desc = note.Desc,
                    Icon = note.Icon,
                    NoteSegmentTemplatePayloads = new List<NoteSegmentTemplatePayload>()

                };


                var newModel = noteSegmentServiceFactory.GetNoteSegmentService(noteSegmentTemplate);
                if (newModel != null)
                {
                    //newModel.Create.Execute(null);
                    newModel.NoteSegmentState = NoteSegmentState;
                    newModel.Container = this;
                    this.NoteSegments.Add(newModel);
                }
            }
        }

        private async void CreateSegmentFromStoreAction(object obj)
        {
            PopupLoading = true;
            CreateSegmentFromStore.ChangeCanExecute();

            using (var objWrapper = iocResolver.ResolveAsDisposable<NoteSegmentStoreListPage>())
            {
                noteSegmentStoreListPage = objWrapper.Object;
                (noteSegmentStoreListPage.BindingContext as NoteSegmentStoreListPageViewModel).OnFinishedChooise += EditNoteTemplatePageViewModel_OnFinishedChooise;
            }

            await navigationService.ShowPopupAsync(noteSegmentStoreListPage).ContinueWith((e) =>
            {
                if (noteSegmentStoreListPage!=null)
                {
                    (noteSegmentStoreListPage.BindingContext as NoteSegmentStoreListPageViewModel).OnFinishedChooise -= EditNoteTemplatePageViewModel_OnFinishedChooise;
                    noteSegmentStoreListPage = null;
                }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PopupLoading = false;
                    CreateSegmentFromStore.ChangeCanExecute();
                });

            });


        }

        private async void EditNoteTemplatePageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteTemplateId))
            {
                Loading = true;
                if (NoteTemplateId != default)
                {
                    await Task.Run(async () =>
                    {
                        await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
                        {
                            var note = await this.repository
                                .GetAll()
                                .Include(c => c.NoteSegmentTemplates)
                                .ThenInclude(c => c.NoteSegmentTemplatePayloads)
                                .Where(c => c.Id == this.NoteTemplateId).FirstOrDefaultAsync();
                            Init(note);

                        });
                    }).ContinueWith((e) => { Loading = false; });
                }

            }
            else if (e.PropertyName == nameof(NoteSegments))
            {
                RaisePropertyChanged(nameof(CanSimplified));
            }
            else if (e.PropertyName == nameof(NoteSegmentState))
            {
                if (NoteSegments != null)
                {
                    foreach (var noteSegmentTemplate in NoteSegments)
                    {
                        noteSegmentTemplate.NoteSegmentState = NoteSegmentState;
                    }
                }

            }

        }

        private void Init(NoteTemplate note)
        {
            if (note != null)
            {
                var noteSegmentTemplates = note.NoteSegmentTemplates;
                this.noteId = note.Id;
                this.NoteSegments = noteSegmentTemplates != null
                    ? new ObservableCollection<INoteSegmentService>(
                  noteSegmentTemplates.Select(GetNoteSegmentTemplateViewModel))
                    : new ObservableCollection<INoteSegmentService>();
                NoteSegments.CollectionChanged +=NoteSegments_CollectionChanged;
                Title = note.Title;
                Desc = note.Desc;
                Icon = note.Icon;
                Color = note.Color;
                BackgroundColor = note.BackgroundColor;
                PreViewContent = note.PreViewContent;
                IsEditable = note.IsEditable;

            }
        }

        private void NoteSegments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var result = e.NewItems[0];
                if (result is IAutoSet)
                {
                    (result as IAutoSet).OnAutoSetChanged += (o, e) => RaisePropertyChanged(nameof(CanSimplified));
                }
                RaisePropertyChanged(nameof(CanSimplified));

            }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                var result = e.OldItems[0];
                if (result is IAutoSet)
                {
                    (result as IAutoSet).OnAutoSetChanged -= (o, e) => RaisePropertyChanged(nameof(CanSimplified));
                }

                RaisePropertyChanged(nameof(CanSimplified));

            }
        }

        private INoteSegmentService GetNoteSegmentTemplateViewModel(NoteSegmentTemplate c)
        {
            var result = noteSegmentServiceFactory.GetNoteSegmentService(c);
            result.NoteSegmentState = NoteSegmentState;
            result.Container = this;
            if (result is IAutoSet)
            {
                (result as IAutoSet).OnAutoSetChanged += (o, e) => RaisePropertyChanged(nameof(CanSimplified));
            }

            return result;
        }


        private long noteId;

        public long NoteTemplateId
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

        private NoteSegmentState _noteSegmentState;

        public NoteSegmentState NoteSegmentState
        {
            get { return _noteSegmentState; }
            set
            {
                _noteSegmentState = value;
                RaisePropertyChanged();

            }
        }

        private bool _popupLoading;

        public bool PopupLoading
        {
            get { return _popupLoading; }
            set
            {
                _popupLoading = value;
                RaisePropertyChanged();

            }
        }



        public SelectionMode SelectionMode => NoteSegmentState==NoteSegmentState.Config ? SelectionMode.Multiple : SelectionMode.Single;

        public bool CanSimplified => NoteSegments == null ? false : NoteSegments.All(this.GetIsItemSimplified);

        public bool GetIsItemSimplified(INoteSegmentService noteSegmentTemplate)
        {
            foreach (var interfaceType in noteSegmentTemplate.GetType().GetInterfaces())
            {
                if (!interfaceType.GetTypeInfo().IsGenericType && interfaceType == typeof(IAutoSet))
                {
                    return (noteSegmentTemplate as IAutoSet).IsAutoSet;
                }
            }
            return false;
        }

        private async void SubmitAction(object obj)
        {
            foreach (var noteSegmentTemplate in NoteSegments)
            {
                noteSegmentTemplate.Submit.Execute(null);

            }
            await unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                await this.repository.UpdateAsync(this.NoteTemplateId, (note) =>
                {
                    note.Title = this.Title;
                    note.Desc = this.Desc;
                    note.Icon = this.Icon;
                    note.Color = this.Color;
                    note.BackgroundColor = this.BackgroundColor;
                    note.PreViewContent = this.PreViewContent;
                    note.IsEditable = this.IsEditable;
                    note.CanSimplified = this.CanSimplified;
                    return Task.FromResult(note);
                });

                var noteSegmentTemplates = await noteSegmentTemplateRepository.GetAllListAsync(c => c.NoteTemplateId == this.NoteTemplateId);

                foreach (var noteSegmentService in NoteSegments)
                {
                    var noteSegmentTemplate = noteSegmentService.NoteSegment as NoteSegmentTemplate;
                    if (!noteSegmentTemplates.Any(c => c.Id == noteSegmentTemplate.Id))
                    {
                        var newNoteSegmentTemplate = new NoteSegmentTemplate();
                        newNoteSegmentTemplate.Id = noteSegmentTemplate.Id;
                        newNoteSegmentTemplate.NoteTemplateId = noteSegmentTemplate.NoteTemplateId;
                        newNoteSegmentTemplate.Title = noteSegmentTemplate.Title;
                        newNoteSegmentTemplate.Type = noteSegmentTemplate.Type;
                        newNoteSegmentTemplate.Status = noteSegmentTemplate.Status;
                        newNoteSegmentTemplate.Desc = noteSegmentTemplate.Desc;
                        newNoteSegmentTemplate.Icon = noteSegmentTemplate.Icon;
                        newNoteSegmentTemplate.Color = noteSegmentTemplate.Color;
                        newNoteSegmentTemplate.Rank = noteSegmentTemplate.Rank;
                        newNoteSegmentTemplate.IsHidden = noteSegmentTemplate.IsHidden;
                        newNoteSegmentTemplate.IsRemovable = noteSegmentTemplate.IsRemovable;

                        newNoteSegmentTemplate.NoteSegmentTemplatePayloads = noteSegmentTemplate.NoteSegmentTemplatePayloads.Select(c => new NoteSegmentTemplatePayload()
                        {
                            NoteSegmentTemplateId = newNoteSegmentTemplate.Id,
                            Key = c.Key,
                            Value = c.Value,
                            ValueType = c.ValueType

                        }).ToList();
                        var entity = await noteSegmentTemplateRepository.InsertAsync(newNoteSegmentTemplate);
                    }
                }

                foreach (var noteSegmentTemplate in noteSegmentTemplates)
                {
                    if (!NoteSegments.Select(c => c.NoteSegment)
                          .Where(c => !(c as NoteSegmentTemplate).IsTransient())
                          .Any(c => (c as NoteSegmentTemplate).Id == noteSegmentTemplate.Id))
                    {
                        await noteSegmentTemplateRepository.DeleteAsync(noteSegmentTemplate.Id);
                    }
                    else
                    {
                        var newNoteSegmentTemplate = NoteSegments.Select(c => c.NoteSegment).FirstOrDefault(c => (c as NoteSegmentTemplate).Id == noteSegmentTemplate.Id);

                        var entity = await noteSegmentTemplateRepository.UpdateAsync((newNoteSegmentTemplate as NoteSegmentTemplate).Id, async (noteSegmentTemplate) =>
                        {
                            await Task.Run(() =>
                            {
                                noteSegmentTemplate.Title = newNoteSegmentTemplate.Title;
                                noteSegmentTemplate.Type = newNoteSegmentTemplate.Type;
                                noteSegmentTemplate.Status = newNoteSegmentTemplate.Status;
                                noteSegmentTemplate.Desc = newNoteSegmentTemplate.Desc;
                                noteSegmentTemplate.Icon = newNoteSegmentTemplate.Icon;
                                noteSegmentTemplate.Color = newNoteSegmentTemplate.Color;
                                noteSegmentTemplate.Rank = newNoteSegmentTemplate.Rank;
                                noteSegmentTemplate.IsHidden = newNoteSegmentTemplate.IsHidden;
                                noteSegmentTemplate.IsRemovable = newNoteSegmentTemplate.IsRemovable;
                            });

                        });


                        var payloadEntities = await payloadRepository.GetAllListAsync(c => c.NoteSegmentTemplateId == entity.Id);
                        foreach (var item in (newNoteSegmentTemplate as NoteSegmentTemplate).NoteSegmentTemplatePayloads)
                        {
                            if (!payloadEntities.Any(c => c.Key == item.Key))
                            {
                                item.NoteSegmentTemplateId = entity.Id;
                                await payloadRepository.InsertAsync(item);

                            }
                        }


                        foreach (var payloadEntity in payloadEntities)
                        {
                            var currentPayload = newNoteSegmentTemplate.GetNoteSegmentPayload(payloadEntity.Key);
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
        }

        public async Task CloseAllPopup()
        {
            await navigationService.HidePopupAsync(noteSegmentStoreListPage);
            await navigationService.HidePopupAsync(iconSelectingPage);
        }

        public void Dispose()
        {
            this.PropertyChanged-=EditNoteTemplatePageViewModel_PropertyChanged;
            if (NoteSegments!=null)
            {
                NoteSegments.CollectionChanged -=NoteSegments_CollectionChanged;

            }

        }

        public Command Submit { get; set; }
        public Command Clone { get; set; }
        public Command Create { get; set; }
        public Command CreateSegment { get; set; }
        public Command CreateSegmentFromStore { get; set; }

        public Command ChooseIcon { get; set; }

        public Command Remove { get; set; }
        public Command RemoveSegment { get; set; }
        public Command RemoveSelectedSegment { get; set; }
        public Command SelectAllSegment { get; set; }

        //Drag and drop
        public Command ItemDragged { get; set; }

        public Command ItemDraggedOver { get; set; }

        public Command ItemDragLeave { get; set; }

        public Command ItemDropped { get; set; }

        public Command GoToState { get; set; }

        public Command Back { get; set; }

        public Command SubmitBack { get; set; }

    }
}
