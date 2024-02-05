using Abp.Dependency;
using Abp.Domain.Repositories;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Views;
using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Infrastructure.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Size = Microsoft.Maui.Graphics.Size;

namespace MatoProductivity.Core.Services
{
    public class ScriptSegmentService : FileSegmentService, ITransientDependency
    {

        public static List<string> DefaultLineColorList = new List<string>() {
            "#000000",
            "#F9371C",
            "#F97C1C",
            "#F9C81C",
            "#41D0B6",
            "#2CADF6",
            "#6562FC"
        };

        public static List<DrawingLineSize> DefaultDrawingLineSizeList = new List<DrawingLineSize>() {
          new DrawingLineSize(){Name=FaIcons.IconCircle, Value=2, FontSize=5},
          new DrawingLineSize(){Name=FaIcons.IconCircle, Value=5, FontSize=10},
          new DrawingLineSize(){Name=FaIcons.IconCircle, Value=8, FontSize=18},
        };
        public Command Undo { get; set; }
        public Command Storage { get; set; }
        public Command Clear { get; set; }
        public Command RemovePhoto { get; set; }
        public Command ShareDocument { get; set; }
        public Command SaveDocument { get; set; }
        public Func<Size> GetImageSize { get; set; }

        public ScriptSegmentService(
            INoteSegment noteSegment) : base(noteSegment)
        {
            PropertyChanged += ScriptSegmentViewModel_PropertyChanged;
            this.Undo = new Command(UndoAction);
            this.Clear = new Command(ClearAction, () => IsScriptValid);
            this.RemovePhoto = new Command(RemovePhotoAction);
            this.ShareDocument = new Command(ShareDocumentAction, () => IsScriptValid);
            this.SaveDocument = new Command(SaveDocumentAction, () => IsScriptValid);
            this.LineColorSelectorSource=DefaultLineColorList;
            this.DrawingLineSizeSelectorSource=DefaultDrawingLineSizeList;
            this.LineColor=this.LineColorSelectorSource.FirstOrDefault();
            this.DrawingLineSize=this.DrawingLineSizeSelectorSource.FirstOrDefault();



        }

        public async void ShareDocumentAction()
        {
            await this.GetScriptImage();

            string fn = this.Title+".png";

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
            await this.GetScriptImage();

            string fn = this.Title+".png";

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


        protected override void FileSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NoteSegment))
            {
                var defaultTitle = this.CreateNoteSegmentPayload(nameof(Title), NoteSegment.Title);
                var title = NoteSegment?.GetOrSetNoteSegmentPayload(nameof(Title), defaultTitle);
                Title = title.GetStringValue();

                var serializedScriptLines = NoteSegment?.GetNoteSegmentPayload(nameof(ScriptLines));
                if (serializedScriptLines!=null)
                {
                    try
                    {
                        var deserializedScriptLines = JsonConvert.DeserializeObject<IEnumerable<DrawingLine>>(serializedScriptLines.StringValue);
                        ScriptLines =new ObservableCollection<IDrawingLine>(deserializedScriptLines);
                        ScriptLines.CollectionChanged+=ScriptLines_CollectionChanged;


                    }
                    catch (Exception ex)
                    {
                        Logger.Warn(ex.Message);
                    }
                }
                else
                {
                    ScriptLines=new ObservableCollection<IDrawingLine>();
                    ScriptLines.CollectionChanged+=ScriptLines_CollectionChanged;

                }

            }


            else if (e.PropertyName == nameof(Title))
            {
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(Title), Title));
            }
        }


        private void ScriptLines_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (this.ScriptLines.Count>0)
            {
                var serializedScriptLines = JsonConvert.SerializeObject(this.ScriptLines);
                NoteSegment?.SetNoteSegmentPayload(this.CreateNoteSegmentPayload(nameof(ScriptLines), serializedScriptLines));
            }

            this.Clear.ChangeCanExecute();
        }

        private List<DrawingLineSize> _drawingLineSizeSelectorSource;

        public List<DrawingLineSize> DrawingLineSizeSelectorSource
        {
            get { return _drawingLineSizeSelectorSource; }
            set
            {
                _drawingLineSizeSelectorSource = value;
                RaisePropertyChanged();

            }
        }

        private DrawingLineSize _drawingLineSize;

        public DrawingLineSize DrawingLineSize
        {
            get { return _drawingLineSize; }
            set
            {
                _drawingLineSize = value;
                RaisePropertyChanged();

            }
        }

        private string _lineColor;

        public string LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                RaisePropertyChanged();
            }
        }


        private List<string> _lineColorSelectorSource;

        public List<string> LineColorSelectorSource
        {
            get { return _lineColorSelectorSource; }
            set
            {
                _lineColorSelectorSource = value;
                RaisePropertyChanged();
            }
        }


        private ObservableCollection<IDrawingLine> _scriptLines;

        public ObservableCollection<IDrawingLine> ScriptLines
        {
            get { return _scriptLines; }
            set
            {
                _scriptLines = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsScriptValid));
            }
        }

        private void ScriptSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(FileContent))
            {
                RaisePropertyChanged(nameof(PreviewImage));
            }


        }

        public override async void SubmitAction(object obj)
        {
            base.SubmitAction(obj);
            await GetScriptImage();
        }
        public void RemovePhotoAction()
        {
            FileContent = null;

        }
        public async void UndoAction()
        {
            //todo
        }




        private async Task GetScriptImage()
        {
            if (!IsScriptValid)
            {
                return;
            }
            var size = new Size(300, 300);
            if (GetImageSize!=null)
            {
                size = GetImageSize.Invoke();
            }
            var stream = await DrawingView.GetImageStream(this.ScriptLines, size, Microsoft.Maui.Controls.Brush.White);
            if (stream != null)
            {
                using (stream)
                using (MemoryStream fileStream = new MemoryStream())
                {
                    await stream.CopyToAsync(fileStream);
                    this.FileContent=  fileStream.ToArray();
                }
            }
        }

        public bool IsScriptValid => this.ScriptLines!=null && this.ScriptLines.Count>0;


        public void ClearAction()
        {
            ScriptLines.Clear();

        }

        public ImageSource PreviewImage => ImageSource.FromStream(() => new MemoryStream(FileContent));

    }
}
