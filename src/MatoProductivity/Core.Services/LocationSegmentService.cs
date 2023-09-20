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
    public class LocationSegmentService : NoteSegmentService, ITransientDependency
    {

        private NoteSegmentPayload DefaultContentSegmentPayload => new NoteSegmentPayload(nameof(Content), "");
        public LocationSegmentService(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            INoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += LocationSegmentViewModel_PropertyChanged;
        }

        private void LocationSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = new NoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = (NoteSegment as NoteSegment)?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                Title = title.GetStringValue();


                var content = (NoteSegment as NoteSegment)?.GetOrSetNoteSegmentPayloads(nameof(Content), DefaultContentSegmentPayload);
                Content = content.GetStringValue();

                var defaultPlaceHolderSegmentPayload = new NoteSegmentPayload(nameof(PlaceHolder), "请输入" + Title);

                var placeHolder = (NoteSegment as NoteSegment)?.GetOrSetNoteSegmentPayloads(nameof(PlaceHolder), defaultPlaceHolderSegmentPayload);
                PlaceHolder = placeHolder.GetStringValue();
            }

            else if (e.PropertyName == nameof(Content))
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    (NoteSegment as NoteSegment)?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Content), Content));

                }
            }

            else if (e.PropertyName == nameof(PlaceHolder))
            {
                (NoteSegment as NoteSegment)?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(PlaceHolder), PlaceHolder));
            }
            else if (e.PropertyName == nameof(Title))
            {
                (NoteSegment as NoteSegment)?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Title), Title));
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

        private string _placeHolder;

        public string PlaceHolder
        {
            get { return _placeHolder; }
            set
            {
                _placeHolder = value;
                RaisePropertyChanged();
            }
        }




    }
}
