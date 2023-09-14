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

        private NoteSegmentPayload DefaultFileContentSegmentPayload => new NoteSegmentPayload(nameof(FileContent), "");

        public FileSegmentService(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += FileSegmentViewModel_PropertyChanged;

        }



        private void FileSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = new NoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                Title = title.GetStringValue();


                var fileContent = NoteSegment?.GetOrSetNoteSegmentPayloads(nameof(FileContent), DefaultFileContentSegmentPayload);
                FileContent = fileContent.Value;

            }

            else if (e.PropertyName == nameof(FileContent))
            {
                if (FileContent!=null&& FileContent.Length>0)
                {
                    NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(FileContent), FileContent));

                }
                else
                {
                    //NoteSegment?.RemoveNoteSegmentPayloads(nameof(FileContent));
                }
            }


            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Title), Title));
            }
        }

        public override void CreateAction(object obj)
        {

        }


        protected virtual async Task SaveFile(FileResult photo)
        {
            if (photo != null)
            {
                using (Stream sourceStream = await photo.OpenReadAsync())
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

    }
}
