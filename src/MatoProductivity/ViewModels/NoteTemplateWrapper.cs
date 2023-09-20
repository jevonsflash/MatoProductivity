using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.Services;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Infrastructure;
using MatoProductivity.Infrastructure.ThrottleDebounce;
using Microsoft.Maui.Controls.Shapes;

namespace MatoProductivity.ViewModels
{
    public class NoteTemplateWrapper : ViewModelBase
    {
        public static object throttledLocker = new object();

        public static RateLimitedAction throttledAction = Debouncer.Debounce(null, TimeSpan.FromMilliseconds(500), leading: false, trailing: true);

        public NoteTemplateWrapper(
            NoteTemplate noteTemplate)
        {
            Remove = new Command(RemoveAction);
            NoteTemplate = noteTemplate;

            Dragged = new Command(OnDragged);
            DraggedOver = new Command(OnDraggedOver);
            DragLeave = new Command(OnDragLeave);
            Dropped = new Command(i => OnDropped(i));
            this.PropertyChanged += GridNoteTemplateService_PropertyChanged;
        }

        private void GridNoteTemplateService_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(this.IsBeingDraggedOver))
            {

                if (this.IsBeingDraggedOver && DropPlaceHolderItem != null)
                {
                    // 使用防抖
                    lock (throttledLocker)
                    {
                        var newIndex = Container.NoteTemplates.IndexOf(this);
                        var oldIndex = Container.NoteTemplates.IndexOf(DropPlaceHolderItem as NoteTemplateWrapper);

                        var originalAction = () =>
                        {
                            Container.NoteTemplates.Move(oldIndex, newIndex);
                        };
                        throttledAction.Update(originalAction);
                        throttledAction.Invoke();
                    }

                }
            }

        }

        private void OnDragged(object item)
        {
            IsBeingDragged = true;
            DraggedItem = item;
            this.Container.IsEditing = true;

        }

        private void OnDraggedOver(object item)
        {
            if (!IsBeingDragged && item != null)
            {

                var itemToMove = Container.NoteTemplates.First(i => i.IsBeingDragged);
                if (itemToMove.DraggedItem != null)
                {
                    DropPlaceHolderItem = itemToMove.DraggedItem;

                }
                IsBeingDraggedOver = true;

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
            DropPlaceHolderItem = null;
        }

        private void OnDropped(object item)
        {
            var itemToMove = Container.NoteTemplates.First(i => i.IsBeingDragged);

            if (itemToMove == null)
                return;


            itemToMove.IsBeingDragged = false;
            IsBeingDraggedOver = false;
            DraggedItem = null;
            DropPlaceHolderItem = null;

        }


        private async void RemoveAction(object obj)
        {
            Container.Remove.Execute(this);

        }


        public NoteTemplateListPageViewModel Container { get; set; }


        private NoteTemplate noteTemplate;

        public NoteTemplate NoteTemplate
        {
            get { return noteTemplate; }
            set
            {
                noteTemplate = value;
                RaisePropertyChanged();

            }
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
                if (value != _isBeingDraggedOver)
                {
                    _isBeingDraggedOver = value;
                    RaisePropertyChanged();
                }
            }
        }

        public Command Remove { get; set; }

        public Command Dragged { get; set; }

        public Command DraggedOver { get; set; }

        public Command DragLeave { get; set; }

        public Command Dropped { get; set; }

    }
}
