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
    public class TextSegmentViewModel : NoteSegmentViewModel, ITransientDependency
    {

        private NoteSegmentPayload DefaultContentSegmentPayload => new NoteSegmentPayload(nameof(Content), "");
        public TextSegmentViewModel(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += TextSegmentViewModel_PropertyChanged;
        }

        private void TextSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = new NoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = this.NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                this.Title = title.GetStringValue();


                var content = this.NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Content), DefaultContentSegmentPayload);
                this.Content = content.GetStringValue();

                var defaultPlaceHolderSegmentPayload = new NoteSegmentPayload(nameof(PlaceHolder), "请输入" + Title);

                var placeHolder = this.NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(PlaceHolder), defaultPlaceHolderSegmentPayload);
                this.PlaceHolder = placeHolder.GetStringValue();
            }

            else if (e.PropertyName == nameof(Content))
            {
                this.NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Content), Content));
            }

            else if (e.PropertyName == nameof(PlaceHolder))
            {
                this.NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(PlaceHolder), PlaceHolder));
            }
            else if (e.PropertyName == nameof(Title))
            {
                this.NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Title), Title));
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
