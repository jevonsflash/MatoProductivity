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
            INoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += FileSegmentViewModel_PropertyChanged;

        }



        private void FileSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = new NoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = (NoteSegment as NoteSegment)?.GetOrSetNoteSegmentPayloads(nameof(Title), defaultTitle);
                Title = title.GetStringValue();


                var fileContent = (NoteSegment as NoteSegment)?.GetOrSetNoteSegmentPayloads(nameof(FileContent), DefaultFileContentSegmentPayload);
                FileContent = fileContent.Value;

            }

            else if (e.PropertyName == nameof(FileContent))
            {
                if (FileContent!=null&& FileContent.Length>0)
                {
                    (NoteSegment as NoteSegment)?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(FileContent), FileContent));

                }
                else
                {
                    //(NoteSegment as NoteSegment)?.RemoveNoteSegmentPayloads(nameof(FileContent));
                }
            }


            else if (e.PropertyName == nameof(Title))
            {
                (NoteSegment as NoteSegment)?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(Title), Title));
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
