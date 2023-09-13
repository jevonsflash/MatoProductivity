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
    public class FileSegmentService : NoteSegmentService, ITransientDependency
    {

        private NoteSegmentPayload DefaultFileContentSegmentPayload => new NoteSegmentPayload(nameof(FileContent), "");
        public Command CapturePhoto { get; set; }
        public Command PickPhoto { get; set; }
        public Command RemovePhoto { get; set; }


        public FileSegmentService(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += FileSegmentViewModel_PropertyChanged;
            this.CapturePhoto = new Command(CapturePhotoAction);
            this.PickPhoto = new Command(PickPhotoAction);
            this.RemovePhoto = new Command(RemovePhotoAction);

        }

        public override void SubmitAction(object obj)
        {
            base.SubmitAction(obj);
            Console.WriteLine("!");
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
                if (FileContent!=null)
                {
                    NoteSegment?.SetNoteSegmentPayloads(new NoteSegmentPayload(nameof(FileContent), FileContent));

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


        public async void CapturePhotoAction()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();
                await SaveFile(photo);

            }
        }


        public async void PickPhotoAction()
        {
            FileResult photo = await MediaPicker.Default.PickPhotoAsync();

            await SaveFile(photo);

        }
        public void RemovePhotoAction()
        {
            FileContent=null;

        }
        private async Task SaveFile(FileResult photo)
        {
            if (photo != null)
            {
                using Stream sourceStream = await photo.OpenReadAsync();
                using MemoryStream fileStream = new MemoryStream();

                await sourceStream.CopyToAsync(fileStream);

                this.FileContent=  fileStream.ToArray();
                await sourceStream.DisposeAsync();
                await fileStream.DisposeAsync();

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
                RaisePropertyChanged(nameof(PreviewImage));
            }
        }

        public ImageSource PreviewImage => ImageSource.FromStream(() => new MemoryStream(FileContent));


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
