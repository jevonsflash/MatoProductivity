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
    public class VoiceSegmentService : FileSegmentService, ITransientDependency
    {

        public Command CapturePhoto { get; set; }
        public Command PickPhoto { get; set; }
        public Command RemovePhoto { get; set; }


        public VoiceSegmentService(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += VoiceSegmentViewModel_PropertyChanged;
            this.CapturePhoto = new Command(CapturePhotoAction);
            this.PickPhoto = new Command(PickPhotoAction);
            this.RemovePhoto = new Command(RemovePhotoAction);
        }



        private void VoiceSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(FileContent))
            {
                RaisePropertyChanged(nameof(PreviewImage));
            }


        }


        public async void CapturePhotoAction()
        {
          
        }


        public async void PickPhotoAction()
        {
            

        }
        public void RemovePhotoAction()
        {
            FileContent=null;

        }

        public ImageSource PreviewImage => ImageSource.FromStream(() => new MemoryStream(FileContent));

    }
}
