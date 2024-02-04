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
    public class TextSegmentService : NoteSegmentService, ITransientDependency
    {

        private INoteSegmentPayload DefaultContentSegmentPayload => this.CreateNoteSegmentPayload(nameof(Content), "");
        public TextSegmentService(
            INoteSegment noteSegment) : base(noteSegment)
        {
            PropertyChanged += TextSegmentViewModel_PropertyChanged;
        }

        private void TextSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = this.CreateNoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Title), defaultTitle);
                Title = title.GetStringValue();


                var content = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Content), DefaultContentSegmentPayload);
                Content = content.GetStringValue();

                var defaultPlaceHolderSegmentPayload = this.CreateNoteSegmentPayload(nameof(PlaceHolder), "请输入" + Title);

                var placeHolder = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(PlaceHolder), defaultPlaceHolderSegmentPayload);
                PlaceHolder = placeHolder.GetStringValue();
            }

            else if (e.PropertyName == nameof(Content))
            {
                if (!string.IsNullOrEmpty(Content))
                {
                    NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(Content), Content));

                }
            }

            else if (e.PropertyName == nameof(PlaceHolder))
            {
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(PlaceHolder), PlaceHolder));
            }
            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(Title), Title));
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
