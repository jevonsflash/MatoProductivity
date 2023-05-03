using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatoProductivity.Core.ViewModels
{
    public class NoteSegmentViewModel:ViewModelBase
    {
        private readonly IRepository<NoteSegment, long> repository;

        public NoteSegmentViewModel(IRepository<NoteSegment,long> repository)
        {
            Submit = new Command(SubmitAction);
            this.repository = repository;
        }

        private NoteSegment noteSegment;

        public NoteSegment NoteSegment
        {
            get { return noteSegment; }
            set { noteSegment = value; }
        }


        private async void SubmitAction(object obj)
        {
           await this.repository.UpdateAsync(noteSegment);
        }

        public Command Submit { get; set; }
    }
}
