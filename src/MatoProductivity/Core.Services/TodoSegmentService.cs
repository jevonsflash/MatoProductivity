using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public class TodoSegmentService : NoteSegmentService, ITransientDependency
    {

        private NoteSegmentPayload DefaultContentSegmentPayload => new NoteSegmentPayload(nameof(Content), "");

        private NoteSegmentPayload DefaultIsDoneSegmentPayload => new NoteSegmentPayload(nameof(IsDone), false.ToString());

        public TodoSegmentService(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += TodoSegmentViewModel_PropertyChanged;
        }

        private void TodoSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = new NoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                Title = title.GetStringValue();
                var content = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Content), DefaultContentSegmentPayload);
                Content = content.GetStringValue();
                var isDone = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(IsDone), DefaultIsDoneSegmentPayload);
                bool parsedIsDone;
                if (bool.TryParse(isDone.GetStringValue(), out parsedIsDone))
                {
                    IsDone = parsedIsDone;
                }
            }

            else if (e.PropertyName == nameof(Content))
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Content), Content));

                }
            }

            else if (e.PropertyName == nameof(IsDone))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(IsDone), IsDone));

            }
            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Title), Title));
            }
        }

        public override void CreateAction(object obj)
        {

        }

        private string _content;

        public string Content
        {
            get { return _content; }
            set
            {
                _content = value;
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

        private bool _isDone;

        public bool IsDone
        {
            get { return _isDone; }
            set
            {
                _isDone = value;
                RaisePropertyChanged();
            }
        }




    }
}
