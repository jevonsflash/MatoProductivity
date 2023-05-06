using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
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
            this.repository = repository;
            this.payloadRepository = payloadRepository;
            NoteSegment = noteSegment;
            NoteSegmentState = NoteSegmentState.Config;
        }

        public abstract void CreateAction(object obj);


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

                   
            var payloadEntity = await payloadRepository.GetAllListAsync(c => c.NoteSegmentId == this.NoteSegment.Id);

            payloadEntity.Clear();

            ObjectMapper.Map(this.NoteSegment.NoteSegmentPayloads, payloadEntity);
        
            UnitOfWorkManager.Current.SaveChanges();

        }

        public Command Submit { get; set; }
        public Command Create { get; set; }
    }
}
