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
    public class MediaSegmentService : FileSegmentService, ITransientDependency
    {
        public Command CapturePhoto { get; set; }
        public Command PickPhoto { get; set; }
        public Command RemovePhoto { get; set; }


        public MediaSegmentService(
            INoteSegment noteSegment) : base(noteSegment)
        {
            PropertyChanged += MediaSegmentViewModel_PropertyChanged;
            this.CapturePhoto = new Command(CapturePhotoAction);
            this.PickPhoto = new Command(PickPhotoAction);
            this.RemovePhoto = new Command(RemovePhotoAction);

        }

        private void MediaSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(FileContent))
            {
                RaisePropertyChanged(nameof(PreviewImage));
            }
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
            FileContent = null;

        }

        public ImageSource PreviewImage => ImageSource.FromStream(() => new MemoryStream(FileContent));

    }
}
