using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Core.Services;
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
            if (item is INoteSegmentService)
            {
                var dataTemplateName = (item as INoteSegmentService).NoteSegment.Type;
                return Application.Current.Resources[dataTemplateName] as DataTemplate;

            }
            return default;

        }
    }
}
