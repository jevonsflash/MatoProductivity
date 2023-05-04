using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Views
{
    public class NoteSegmentDataTemplateSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item == null)
            {
                return default;
            }
            if (item is INoteSegmentViewModel)
            {
                var dataTemplateName = (item as INoteSegmentViewModel).NoteSegment.Type;
                return Application.Current.Resources[dataTemplateName] as DataTemplate;

            }
            return default;

        }
    }
}
