using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatoProductivity.Core.ViewModels
{
    public abstract class NoteSegmentViewModel : ViewModelBase, INoteSegmentViewModel
    {
        private readonly IRepository<NoteSegment, long> repository;
        private readonly IRepository<NoteSegmentPayload, long> payloadRepository;

        public NoteSegmentViewModel(


            IRepository<NoteSegment, long> repository,

            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment)
        {
            Submit = new Command(SubmitAction);
            Create = new Command(CreateAction);
            this.repository = repository;
            this.payloadRepository = payloadRepository;
            this.NoteSegment = noteSegment;
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


        public virtual async void SubmitAction(object obj)
        {
            await payloadRepository.DeleteAsync(c => c.NoteSegmentId == NoteSegment.Id);
            await this.repository.InsertOrUpdateAsync(noteSegment);
        }

        public Command Submit { get; set; }
        public Command Create { get; set; }
    }
}
