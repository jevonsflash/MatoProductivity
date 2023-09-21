using MatoProductivity.Core.Models.Entities;
using MatoProductivity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public static class NoteSegmentPayloadFactory
    {
        public static INoteSegmentPayload CreateNoteSegmentPayload(this NoteSegmentService noteSegmentService, string key, object value, string valuetype = null)
        {
            if (noteSegmentService.NoteSegment!=null)
            {
                if (noteSegmentService.NoteSegment is NoteSegment
                    )
                {
                    return new NoteSegmentPayload(key, value, valuetype);
                }
                else if (noteSegmentService.NoteSegment is NoteSegmentTemplate)
                {

                    return new NoteSegmentTemplatePayload(key, value, valuetype);
                }
            }
            return default;

        }
    }
}
