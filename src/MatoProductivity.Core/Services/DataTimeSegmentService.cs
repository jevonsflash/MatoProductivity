using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public class DataTimeSegmentService : NoteSegmentService, ITransientDependency, IAutoSet
    {

        private NoteSegmentPayload DefaultIsAutoSetNoteSegmentPayload => new NoteSegmentPayload(nameof(IsAutoSet), false.ToString());
        private NoteSegmentPayload DefaultTimeNoteSegmentPayload => new NoteSegmentPayload(nameof(Time), DateTime.Now.ToString());
        public DataTimeSegmentService(
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
                var time = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Time), DefaultTimeNoteSegmentPayload);
                var isAutoSet = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(IsAutoSet), DefaultIsAutoSetNoteSegmentPayload);


                var defaultTitle = new NoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                Title = title.GetStringValue();

                DateTime parsedTime;
                if (DateTime.TryParse(time.GetStringValue(), out parsedTime))
                {
                    Time =parsedTime.Date;
                    TimeOffset=parsedTime.TimeOfDay;
                }

                bool parsedIsAutoSet;
                if (bool.TryParse(isAutoSet.GetStringValue(), out parsedIsAutoSet))
                {
                    IsAutoSet =parsedIsAutoSet;
                }
            }

            else if (e.PropertyName == nameof(IsAutoSet))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(IsAutoSet), IsAutoSet));
            }

            else if (e.PropertyName == nameof(Time) || e.PropertyName == nameof(TimeOffset))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Time), Time+TimeOffset));
            }

            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Title), Title));
            }

        }


        public override void CreateAction(object obj)
        {
            NoteSegment?.SetNoteSegmentPayloads(DefaultTimeNoteSegmentPayload);
            NoteSegment?.SetNoteSegmentPayloads(DefaultIsAutoSetNoteSegmentPayload);
        }


        private DateTime _time;

        public DateTime Time
        {
            get { return _time; }
            set
            {
                _time = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ExactTime));
            }
        }

        private TimeSpan _timeOffset;

        public TimeSpan TimeOffset
        {
            get { return _timeOffset; }
            set
            {
                _timeOffset = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(ExactTime));
            }
        }

        public DateTime ExactTime => Time+TimeOffset;



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
