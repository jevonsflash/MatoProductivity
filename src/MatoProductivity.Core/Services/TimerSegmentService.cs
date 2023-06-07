using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading.Timers;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public class TimerSegmentService : NoteSegmentService, ITransientDependency, IHasTimer
    {
        private readonly AbpAsyncTimer timer;
        private readonly IBackgroundJobManager backgroundJobManager;
        private NoteSegmentPayload DefaultIsShowFromNowNoteSegmentPayload => new NoteSegmentPayload(nameof(IsShowFromNow), false.ToString());
        private NoteSegmentPayload DefaultTimeNoteSegmentPayload => new NoteSegmentPayload(nameof(Time), (DateTime.Now+new TimeSpan(5, 0, 0)).ToString());
        public TimerSegmentService(
             AbpAsyncTimer timer,
            IBackgroundJobManager backgroundJobManager,
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += TimerSegmentViewModel_PropertyChanged;
            this.timer = timer;
            this.timer.Period = 1000;
            this.timer.Elapsed = async (timer) =>
            {
                await Task.Run(() => RaisePropertyChanged(nameof(TimeFromNow)));
            };
            this.timer.Start();
            this.backgroundJobManager = backgroundJobManager;
        }

        private async void TimerSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var time = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Time), DefaultTimeNoteSegmentPayload);
                var isAutoSet = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(IsShowFromNow), DefaultIsShowFromNowNoteSegmentPayload);

                var defaultNotificationContentString = NoteSegment.Title;

                var defaultNotificationContent = new NoteSegmentPayload(nameof(NotificationContent), defaultNotificationContentString);
                var notificationContent = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(NotificationContent), defaultNotificationContent);
                NotificationContent = notificationContent.GetStringValue();

                DateTime parsedTime;
                if (DateTime.TryParse(time.GetStringValue(), out parsedTime))
                {
                    Time = parsedTime.Date;
                    TimeOffset = parsedTime.TimeOfDay;
                }

                bool parsedIsShowFromNow;
                if (bool.TryParse(isAutoSet.GetStringValue(), out parsedIsShowFromNow))
                {
                    IsShowFromNow = parsedIsShowFromNow;
                }
            }

            else if (e.PropertyName == nameof(IsShowFromNow))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(IsShowFromNow), IsShowFromNow));
            }

            else if (e.PropertyName == nameof(ExactTime))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Time), ExactTime));
            }

            else if (e.PropertyName == nameof(NotificationContent))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(NotificationContent), NotificationContent));
            }

        }


        public override void CreateAction(object obj)
        {
            NoteSegment?.SetNoteSegmentPayloads(DefaultTimeNoteSegmentPayload);
            NoteSegment?.SetNoteSegmentPayloads(DefaultIsShowFromNowNoteSegmentPayload);
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

        public DateTime ExactTime => Time + TimeOffset;

        public TimeSpan TimeFromNow => DateTime.Now - ExactTime;

        private string _notificationContent;

        public string NotificationContent
        {
            get { return _notificationContent; }
            set
            {
                _notificationContent = value;
                RaisePropertyChanged();
            }
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

        [UnitOfWork]
        public override async void SubmitAction(object obj)
        {
            base.SubmitAction(obj);
            var delay = ExactTime - DateTime.Now;
            var notificationJobArgs = new NotificationJobArgs("提醒", this.NotificationContent);
            var result = await backgroundJobManager.EnqueueAsync<NotificationJob, NotificationJobArgs>(notificationJobArgs, BackgroundJobPriority.Normal, delay);
            Debug.WriteLine($"已添加一条通知任务,任务ID: {result},触发延时: {delay}");



        }
    }
}
