using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        private readonly IRepository<NoteSegment, long> repository;
        private readonly IRepository<NoteSegmentPayload, long> payloadRepository;

        public NoteSegmentService(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment)
        {
            Submit = new Command(SubmitAction);
            Create = new Command(CreateAction);
            Remove = new Command(RemoveAction);
            this.repository = repository;
            this.payloadRepository = payloadRepository;
            NoteSegment = noteSegment;
            NoteSegmentState = NoteSegmentState.Config;

            Dragged = new Command(OnDragged);
            DraggedOver = new Command(OnDraggedOver);
            DragLeave = new Command(OnDragLeave);
            Dropped = new Command(i => OnDropped(i));

        }

        private void OnDragged(object item)
        {
            IsBeingDragged=true;
        }

        private void OnDraggedOver(object item)
        {
            if (!IsBeingDragged)
            {
                IsBeingDraggedOver=true;
            }

        }

        private void OnDragLeave(object item)
        {

            IsBeingDraggedOver = false;

        }

        private void OnDropped(object item)
        {
            var itemToMove = Container.NoteSegments.First(i => i.IsBeingDragged);

            if (itemToMove == null ||  itemToMove == this)
                return;


            Container.NoteSegments.Remove(itemToMove);

            var insertAtIndex = Container.NoteSegments.IndexOf(this);

            Container.NoteSegments.Insert(insertAtIndex, itemToMove);
            itemToMove.IsBeingDragged = false;
            this.IsBeingDraggedOver = false;


        }

        private async void RemoveAction(object obj)
        {
            if (Container is INoteSegmentServiceContainer)
            {
                (Container as INoteSegmentServiceContainer).RemoveSegment.Execute(this);
            }
        }

        public abstract void CreateAction(object obj);

        public IReadOnlyNoteSegmentServiceContainer Container { get; set; }


        private NoteSegment noteSegment;

        public NoteSegment NoteSegment
        {
            get { return noteSegment; }
            set
            {
                noteSegment = value;
                RaisePropertyChanged();

            }
        }

        private NoteSegmentState _isConfigState;

        public NoteSegmentState NoteSegmentState
        {
            get { return _isConfigState; }
            set
            {
                _isConfigState = value;
                RaisePropertyChanged();

            }
        }

        [UnitOfWork]
        public virtual async void SubmitAction(object obj)
        {

            var entity = await repository.GetAsync(this.NoteSegment.Id);

            ObjectMapper.Map(this.NoteSegment, entity);


            var payloadEntities = await payloadRepository.GetAllListAsync(c => c.NoteSegmentId == this.NoteSegment.Id);
            foreach (var item in this.NoteSegment.NoteSegmentPayloads)
            {
                if (!payloadEntities.Any(c => c.Key == item.Key))
                {
                    await payloadRepository.InsertAsync(item);

                }
            }


            foreach (var payloadEntity in payloadEntities)
            {
                var currentPayload = this.NoteSegment.GetNoteSegmentPayload(payloadEntity.Key);
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
            UnitOfWorkManager.Current.SaveChanges();

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


        public Command Dragged { get; set; }

        public Command DraggedOver { get; set; }

        public Command DragLeave { get; set; }

        public Command Dropped { get; set; }
    }
}
