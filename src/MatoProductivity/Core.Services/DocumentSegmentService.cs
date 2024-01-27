using Abp.Dependency;
using Abp.Domain.Repositories;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Storage;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Helper;
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
            INoteSegment noteSegment) : base(noteSegment)
        {
            PropertyChanged += DocumentSegmentViewModel_PropertyChanged;
            this.PickDocument = new Command(PickDocumentAction);
            this.RemoveDocument = new Command(RemoveDocumentAction);
            this.ShareDocument = new Command(ShareDocumentAction, () => IsFileContentNotEmpty);
            this.SaveDocument = new Command(SaveDocumentAction, () => IsFileContentNotEmpty);

        }



        private void DocumentSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName==nameof(IsFileContentNotEmpty))
            {
                this.ShareDocument.ChangeCanExecute();
                this.SaveDocument.ChangeCanExecute();
            }
            if (e.PropertyName == nameof(NoteSegment))
            {
                var title = NoteSegment?.GetNoteSegmentPayload(nameof(Title));
                FileName = title?.GetStringValue();

            }
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
                    await Toast.Make($"文件已保存: {fileSaverResult.FilePath}").Show();
                }
                else
                {
                    await Toast.Make($"文件保存失败: {fileSaverResult.Exception.Message}").Show();
                }
            }
        }

        public async void PickDocumentAction()
        {
            PickOptions options = new()
            {
                PickerTitle = "请选择一个文件"
            };
            FileResult result = null;
            try
            {
                result = await FilePicker.Default.PickAsync(options);

            }
            catch (Exception ex)
            {
                CommonHelper.Alert("文件读取失败:"+ex.Message);
            }
            if (result != null)
            {
                await SaveFile(result);
                var filename = result.FileName;
                var filenameTitle = this.CreateNoteSegmentPayload(nameof(NoteSegment.Title), filename);
                NoteSegment?.SetNoteSegmentPayloads(filenameTitle);

                this.FileName=filename;

            }
        }

        private string _fileName;

        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                RaisePropertyChanged();
            }
        }

        public void RemoveDocumentAction()
        {
            FileContent = null;
        }
    }
}
