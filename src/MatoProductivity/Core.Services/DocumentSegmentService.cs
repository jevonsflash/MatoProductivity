using Abp.Dependency;
using Abp.Domain.Repositories;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public class DocumentSegmentService : FileSegmentService, ITransientDependency
    {
        public Command PickDocument { get; set; }
        public Command RemoveDocument { get; set; }
        public Command ShareDocument { get; set; }
        public Command SaveDocument { get; set; }


        public DocumentSegmentService(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            NoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += DocumentSegmentViewModel_PropertyChanged;
            this.PickDocument = new Command(PickDocumentAction);
            this.RemoveDocument = new Command(RemoveDocumentAction);
            this.ShareDocument = new Command(ShareDocumentAction);
            this.SaveDocument = new Command(SaveDocumentAction);

        }



        private void DocumentSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

        }

        public async void ShareDocumentAction()
        {
            string fn = this.Title;

            // Create an output filename
            string targetFile = Path.Combine(FileSystem.Current.CacheDirectory, fn);
            using (var outputStream = File.Create(targetFile))
            using (var inputStream = new MemoryStream(this.FileContent))
            {
                // Copy the file to the AppDataDirectory
                await inputStream.CopyToAsync(outputStream);
            }
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "分享"+fn,
                File = new ShareFile(targetFile)
            });
        }

        public async void SaveDocumentAction()
        {
            string fn = this.Title;

            // Create an output filename
            using (var inputStream = new MemoryStream(this.FileContent))
            {



                var fileSaverResult = await FileSaver.Default.SaveAsync(fn, inputStream, CancellationToken.None);
                if (fileSaverResult.IsSuccessful)
                {
                    await Toast.Make($"The file was saved successfully to location: {fileSaverResult.FilePath}").Show();
                }
                else
                {
                    await Toast.Make($"The file was not saved successfully with error: {fileSaverResult.Exception.Message}").Show();
                }
            }
        }

        public async void PickDocumentAction()
        {
            PickOptions options = new()
            {
                PickerTitle = "请选择一个文件"
            };

            var result = await FilePicker.Default.PickAsync(options);
            if (result != null)
            {
                await SaveFile(result);
                this.Title=result.FileName;
            }
        }

        public void RemoveDocumentAction()
        {
            FileContent = null;
        }
    }
}
