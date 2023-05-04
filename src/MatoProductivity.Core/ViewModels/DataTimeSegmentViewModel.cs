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

        public DataTimeSegmentViewModel(IRepository<NoteSegment, long> repository, NoteSegment noteSegment) : base(repository, noteSegment)
        {
            PropertyChanged += DataTimeSegmentViewModel_PropertyChanged;
        }

        private void DataTimeSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTimeNoteSegmentPayload = NoteSegment.NoteSegmentPayloads.FirstOrDefault(c => c.Key == nameof(DefaultTime));
                if (defaultTimeNoteSegmentPayload != null)
                {
                    this.DefaultTime = DateTime.Parse(defaultTimeNoteSegmentPayload.GetStringValue());
                }

                var isAutoSetNoteSegmentPayload = NoteSegment.NoteSegmentPayloads.FirstOrDefault(c => c.Key == nameof(IsAutoSet));
                if (isAutoSetNoteSegmentPayload != null)
                {
                    this.IsAutoSet = bool.Parse(isAutoSetNoteSegmentPayload.GetStringValue());
                }
            }

            else if (e.PropertyName == nameof(IsAutoSet))
            {
                this.NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(IsAutoSet), DefaultIsAutoSetNoteSegmentPayload);
            }

            else if (e.PropertyName == nameof(DefaultTime))
            {
                this.NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(DefaultTime), DefaultTimeNoteSegmentPayload);
            }
        }
        private NoteSegmentPayload DefaultIsAutoSetNoteSegmentPayload => new NoteSegmentPayload(nameof(IsAutoSet), false.ToString());
        private NoteSegmentPayload DefaultTimeNoteSegmentPayload => new NoteSegmentPayload(nameof(DefaultTime), new DateTime(2020, 1, 1).ToString());


        public override void CreateAction(object obj)
        {
            this.NoteSegment?.SetNoteSegmentPayloads(DefaultTimeNoteSegmentPayload);      
            this.NoteSegment?.SetNoteSegmentPayloads(DefaultTimeNoteSegmentPayload);
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

        private DateTime _defaultTime;

        public DateTime DefaultTime
        {
            get { return _defaultTime; }
            set
            {
                _defaultTime = value;
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
