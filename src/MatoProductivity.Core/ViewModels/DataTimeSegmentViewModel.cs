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
        private NoteSegmentPayload DefaultTimeNoteSegmentPayload => new NoteSegmentPayload(nameof(Time), DateTime.Now.ToString());
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


                var defaultTitle = new NoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = this.NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                this.Title = title.GetStringValue();

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

            else if (e.PropertyName == nameof(Title))
            {
                this.NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Title), Title));
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
