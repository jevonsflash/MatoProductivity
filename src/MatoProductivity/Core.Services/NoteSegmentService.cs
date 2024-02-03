﻿using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace MatoProductivity.Core.Services
{

    public enum NoteSegmentState
    {
        Config,
        Edit,
        PreView
    }
    public abstract class NoteSegmentService : ViewModelBase, INoteSegmentService
    {
        public NoteSegmentService(
            INoteSegment noteSegment)
        {
            Submit = new Command(SubmitAction);
            Create = new Command(CreateAction);
            Remove = new Command(RemoveAction);
            GoToState = new Command(GoToStateAction);
            NoteSegment = noteSegment;
            NoteSegmentState = NoteSegmentState.Config;
            Dragged = new Command(OnDragged);
            DraggedOver = new Command(OnDraggedOver);
            DragLeave = new Command(OnDragLeave);
            Dropped = new Command(i => OnDropped(i));

        }

        public virtual void GoToStateAction(object obj)
        {
            if (obj is NoteSegmentState)
            {
                this.NoteSegmentState=(NoteSegmentState)obj;

            }
        }

        private void OnDragged(object item)
        {
            IsBeingDragged = true;
            this.DraggedItem = item;
        }

        private void OnDraggedOver(object item)
        {
            if (!IsBeingDragged && item != null)
            {
                IsBeingDraggedOver = true;

                var itemToMove = Container.NoteSegments.First(i => i.IsBeingDragged);
                if (itemToMove.DraggedItem != null)
                {
                    DropPlaceHolderItem = itemToMove.DraggedItem;

                }
            }

        }


        private object _draggedItem;

        public object DraggedItem
        {
            get { return _draggedItem; }
            set
            {
                _draggedItem = value;
                RaisePropertyChanged();
            }
        }

        private object _dropPlaceHolderItem;

        public object DropPlaceHolderItem
        {
            get { return _dropPlaceHolderItem; }
            set
            {
                _dropPlaceHolderItem = value;
                RaisePropertyChanged();
            }
        }

        private void OnDragLeave(object item)
        {

            IsBeingDraggedOver = false;

        }

        private void OnDropped(object item)
        {
            var itemToMove = Container.NoteSegments.First(i => i.IsBeingDragged);

            if (itemToMove == null || itemToMove == this)
                return;


            Container.NoteSegments.Remove(itemToMove);

            var insertAtIndex = Container.NoteSegments.IndexOf(this);

            Container.NoteSegments.Insert(insertAtIndex, itemToMove);
            itemToMove.IsBeingDragged = false;
            this.IsBeingDraggedOver = false;
            this.DraggedItem = null;

        }

        public virtual async void RemoveAction(object obj)
        {
            if (Container is INoteSegmentServiceContainer)
            {
                (Container as INoteSegmentServiceContainer).RemoveSegment.Execute(this);
            }
        }

        public abstract void CreateAction(object obj);

        public static async Task<bool> CheckPermissionIsGrantedAsync<TPermission>(string explain = "此功能需要相应的权限，请在设置中开启权限") where TPermission : BasePermission, new()
        {
            var result = await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                PermissionStatus status = await Permissions.CheckStatusAsync<TPermission>();

                if (status == PermissionStatus.Granted)
                {
                    return true;
                }

                if (status == PermissionStatus.Denied && DeviceInfo.Platform == DevicePlatform.iOS)
                {
                    return false;
                }

                if (Permissions.ShouldShowRationale<TPermission>())
                {
                    CommonHelper.ShowNoAuthorized(explain);
                }

                status = await Permissions.RequestAsync<TPermission>();
                var result = status == PermissionStatus.Granted;
                if (!result)
                {
                    CommonHelper.Alert("您拒绝了权限，部分功能将受限");

                }
                return result;
            });
            return result;
        }


        public IReadOnlyNoteSegmentServiceContainer Container { get; set; }


        protected INoteSegment noteSegment;

        public INoteSegment NoteSegment
        {
            get { return noteSegment; }
            set
            {
                noteSegment = value;
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

        [UnitOfWork]
        public virtual void SubmitAction(object obj)
        {

        }


        private bool _isBeingDragged;
        public bool IsBeingDragged
        {
            get { return _isBeingDragged; }
            set
            {
                _isBeingDragged = value;
                RaisePropertyChanged();

            }
        }

        private bool _isBeingDraggedOver;
        public bool IsBeingDraggedOver
        {
            get { return _isBeingDraggedOver; }
            set
            {
                _isBeingDraggedOver = value;
                RaisePropertyChanged();

            }
        }

        public Command Submit { get; set; }
        public Command Create { get; set; }
        public Command Remove { get; set; }
        public Command GoToState { get; set; }


        public Command Dragged { get; set; }

        public Command DraggedOver { get; set; }

        public Command DragLeave { get; set; }

        public Command Dropped { get; set; }
    }
}
