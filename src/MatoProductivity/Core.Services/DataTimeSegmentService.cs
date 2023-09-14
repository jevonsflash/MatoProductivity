using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Threading.Timers;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public class DataTimeSegmentService : NoteSegmentService, ITransientDependency, IAutoSet, IHasTimer
    {

        private NoteSegmentPayload DefaultIsAutoSetNoteSegmentPayload => new NoteSegmentPayload(nameof(IsAutoSet), false.ToString());
        private NoteSegmentPayload DefaultTimeNoteSegmentPayload => new NoteSegmentPayload(nameof(Time), DateTime.Now.ToString());
        public DataTimeSegmentService(
             AbpAsyncTimer timer,
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += DataTimeSegmentViewModel_PropertyChanged;
            this.timer = timer;
            this.timer.Period = 1000;
            this.timer.Elapsed = async (timer) =>
            {
                await Task.Run(() => RaisePropertyChanged(nameof(TimeFromNow)));
            };
            this.timer.Start();
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

            else if (e.PropertyName == nameof(ExactTime))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Time), ExactTime));
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

        private bool _isShowFromNow;


        public bool IsShowFromNow
        {
            get { return _isShowFromNow; }
            set
            {
                _isShowFromNow = value;
                RaisePropertyChanged();

            }
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
                RaisePropertyChanged(nameof(TimeFromNow));
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
                RaisePropertyChanged(nameof(TimeFromNow));
            }
        }

        public DateTime ExactTime => Time+TimeOffset;

        public TimeSpan TimeFromNow => DateTime.Now - ExactTime;

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
        private readonly AbpAsyncTimer timer;

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
