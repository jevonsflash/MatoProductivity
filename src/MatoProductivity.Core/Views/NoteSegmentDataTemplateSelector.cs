using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
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
            if (item is NoteSegment)
            {
                var type = (item as NoteSegment).Type;
                return Application.Current.Resources[type] as DataTemplate;

            }
            return default;

        }
    }
}
