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
        private readonly AbpAsyncTimer timer;

        private INoteSegmentPayload DefaultIsAutoSetNoteSegmentPayload => this.CreateNoteSegmentPayload(nameof(IsAutoSet), false.ToString());
        private INoteSegmentPayload DefaultTimeNoteSegmentPayload => this.CreateNoteSegmentPayload(nameof(Time), DateTime.Now.ToString());

        public event EventHandler<AutoSetChangedEventArgs> OnAutoSetChanged;

        public DataTimeSegmentService(
             AbpAsyncTimer timer,
            INoteSegment noteSegment) : base(noteSegment)
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


                var defaultTitle = this.CreateNoteSegmentPayload(nameof(Title), NoteSegment.Title);
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
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(IsAutoSet), IsAutoSet));
                OnAutoSetChanged?.Invoke(this, new AutoSetChangedEventArgs(this.IsAutoSet));
            }

            else if (e.PropertyName == nameof(ExactTime))
            {
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Time), ExactTime));
            }

            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Title), Title));
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
