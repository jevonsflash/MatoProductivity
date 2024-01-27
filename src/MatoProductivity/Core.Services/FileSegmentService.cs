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
    public abstract class FileSegmentService : NoteSegmentService
    {

        private INoteSegmentPayload DefaultFileContentSegmentPayload => this.CreateNoteSegmentPayload(nameof(FileContent), "");

        public FileSegmentService(
            INoteSegment noteSegment) : base(noteSegment)
        {
            PropertyChanged += FileSegmentViewModel_PropertyChanged;

        }



        private void FileSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = this.CreateNoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                Title = title.GetStringValue();


                var fileContent = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(FileContent), DefaultFileContentSegmentPayload);
                FileContent = fileContent.Value;

            }

            else if (e.PropertyName == nameof(FileContent))
            {
                if (IsFileContentNotEmpty)
                {
                    NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(FileContent), FileContent));

                }
                else
                {
                    //NoteSegment?.RemoveNoteSegmentPayloads(nameof(FileContent));
                }
            }


            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayloads(this.CreateNoteSegmentPayload(nameof(Title), Title));
            }
        }

        public override void CreateAction(object obj)
        {

        }


        protected virtual async Task SaveFile(FileResult fileResult)
        {
            if (fileResult != null)
            {
                Stream sourceStream = await fileResult.OpenReadAsync();
                using (MemoryStream fileStream = new MemoryStream())
                {
                    await sourceStream.CopyToAsync(fileStream);
                    this.FileContent=  fileStream.ToArray();
                }

            }
        }


        private byte[] _fileContent;

        public byte[] FileContent
        {
            get { return _fileContent; }
            set
            {
                _fileContent = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsFileContentNotEmpty));
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

        public bool IsFileContentNotEmpty => FileContent!=null&& FileContent.Length>0;

    }
}
