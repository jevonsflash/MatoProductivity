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
        public Command SwitchDone { get; set; }

        private INoteSegmentPayload DefaultContentSegmentPayload => this.CreateNoteSegmentPayload(nameof(Content), "");

        private INoteSegmentPayload DefaultIsDoneSegmentPayload => this.CreateNoteSegmentPayload(nameof(IsDone), false.ToString());

        public TodoSegmentService(
            IRepository<NoteSegmentPayload, long> payloadRepository,
            INoteSegment noteSegment) : base(noteSegment)
        {
            PropertyChanged += TodoSegmentViewModel_PropertyChanged;
            SwitchDone=new Command(SwitchDoneAction);
            this.payloadRepository=payloadRepository;
        }

        private async void SwitchDoneAction(object obj)
        {
            var isdone = NoteSegment?.GetNoteSegmentPayload(nameof(IsDone));
            isdone.SetStringValue(((bool)obj).ToString());

            var payloadEntities = await payloadRepository.GetAllListAsync(c => c.NoteSegmentId == (NoteSegment as NoteSegment).Id);


            foreach (var payloadEntity in payloadEntities)
            {
                var currentPayload = NoteSegment?.GetNoteSegmentPayload(payloadEntity.Key);
                if (currentPayload == null)
                {
                    await payloadRepository.DeleteAsync(payloadEntity);
                }
                else
                {
                    payloadEntity.Value = currentPayload.Value;
                    payloadEntity.ValueType = currentPayload.ValueType;
                    await payloadRepository.UpdateAsync(payloadEntity);
                }
            }
        }

        private void TodoSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = this.CreateNoteSegmentPayload(nameof(Title), "未命名"+NoteSegment.Title);
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
                    NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Content), Content));

                }
            }

            else if (e.PropertyName == nameof(IsDone))
            {
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(IsDone), IsDone));

            }
            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Title), Title));
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
        private readonly IRepository<NoteSegmentPayload, long> payloadRepository;

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
