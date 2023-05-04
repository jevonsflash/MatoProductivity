using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatoProductivity.Core.ViewModels
{
    public class DataTimeSegmentViewModel : NoteSegmentViewModel, ITransientDependency
    {

        private NoteSegmentPayload DefaultIsAutoSetNoteSegmentPayload => new NoteSegmentPayload(nameof(IsAutoSet), false.ToString());
        private NoteSegmentPayload DefaultTimeNoteSegmentPayload => new NoteSegmentPayload(nameof(Time), new DateTime(2020, 1, 1).ToString());
        public DataTimeSegmentViewModel(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += DataTimeSegmentViewModel_PropertyChanged;
        }

        private async void DataTimeSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var time = this.NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Time), DefaultTimeNoteSegmentPayload);
                var isAutoSet = this.NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(IsAutoSet), DefaultIsAutoSetNoteSegmentPayload);

                this.Time = DateTime.Parse(time.GetStringValue());
                this.IsAutoSet = bool.Parse(isAutoSet.GetStringValue());
            }

            else if (e.PropertyName == nameof(IsAutoSet))
            {
                this.NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(IsAutoSet), IsAutoSet));
            }

            else if (e.PropertyName == nameof(Time))
            {
                this.NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Time), Time));
            }

        }


        public override void CreateAction(object obj)
        {
            this.NoteSegment?.SetNoteSegmentPayloads(DefaultTimeNoteSegmentPayload);
            this.NoteSegment?.SetNoteSegmentPayloads(DefaultIsAutoSetNoteSegmentPayload);
        }


        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged();
            }
        }


        private bool _isAutoSet;

        public bool IsAutoSet
        {
            get { return _isAutoSet; }
            set
            {
                _isAutoSet = value;
                RaisePropertyChanged();

            }
        }




    }
}
