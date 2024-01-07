﻿using Abp.Dependency;
using Abp.Domain.Repositories;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using MatoProductivity.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public Func<Size> GetImageSize { get; set; }

        public ScriptSegmentService(
            IRepository<NoteSegment, long> repository,
            IRepository<NoteSegmentPayload, long> payloadRepository,
            INoteSegment noteSegment) : base(repository, payloadRepository, noteSegment)
        {
            PropertyChanged += ScriptSegmentViewModel_PropertyChanged;
            this.Undo = new Command(UndoAction);
            this.Storage = new Command(StorageAction);
            this.Clear = new Command(ClearAction, IsScriptValid);
            this.RemovePhoto = new Command(RemovePhotoAction);
            ScriptLines=new ObservableCollection<IDrawingLine>();
            ScriptLines.CollectionChanged+=ScriptLines_CollectionChanged;
            this.LineColorSelectorSource=DefaultLineColorList;
            this.DrawingLineSizeSelectorSource=DefaultDrawingLineSizeList;
            this.LineColor=this.LineColorSelectorSource.FirstOrDefault();
            this.DrawingLineSize=this.DrawingLineSizeSelectorSource.FirstOrDefault();


        }

        private void ScriptLines_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
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
            }
        }

        private void ScriptSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {

            if (e.PropertyName == nameof(FileContent))
            {
                RaisePropertyChanged(nameof(PreviewImage));
            }


        }

        public override void SubmitAction(object obj)
        {
            base.SubmitAction(obj);
            this.Storage.Execute(null);
        }
        public void RemovePhotoAction()
        {
            FileContent = null;

        }
        public async void UndoAction()
        {
            //todo
        }


        public async void StorageAction()
        {
            if (!IsScriptValid())
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

        private bool IsScriptValid()
        {
            return this.ScriptLines!=null && this.ScriptLines.Count>0;
        }

        public void ClearAction()
        {
            ScriptLines.Clear();

        }

        public ImageSource PreviewImage => ImageSource.FromStream(() => new MemoryStream(FileContent));

    }
}
