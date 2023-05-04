using Abp.Dependency;
using Abp.Domain.Repositories;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatoProductivity.Core.ViewModels
{
    public class TextSegmentViewModel : NoteSegmentViewModel, ITransientDependency
    {

        public TextSegmentViewModel(IRepository<NoteSegment, long> repository, NoteSegment noteSegment) : base(repository, noteSegment)
        {
            PropertyChanged += TextSegmentViewModel_PropertyChanged;
        }

        private void TextSegmentViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var noteSegmentPayload = NoteSegment.NoteSegmentPayloads.FirstOrDefault(c => c.Key == "DefaultValue");
            if (noteSegmentPayload != null)
            {
                this.DefaultContent = noteSegmentPayload.Value.ToString();
            }
        }

        public override void CreateAction(object obj)
        {
            
        }

        private string _defaultContent;

        public string DefaultContent
        {
            get { return _defaultContent; }
            set
            {
                _defaultContent = value;
                RaisePropertyChanged();
            }
        }



    }
}
