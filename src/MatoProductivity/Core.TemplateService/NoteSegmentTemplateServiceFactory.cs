using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using Microsoft.Maui.Controls.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public class NoteSegmentTemplateServiceFactory : INoteSegmentTemplateServiceFactory
    {
        private readonly IIocResolver iocResolver;

        public NoteSegmentTemplateServiceFactory(IIocResolver iocResolver)
        {
            this.iocResolver = iocResolver;
        }

        public INoteSegmentService GetNoteSegmentService(NoteSegmentTemplate noteSegmentTemplate)
        {
            var type = noteSegmentTemplate.Type;
            INoteSegmentService newModel;
            switch (type)
            {
                case "DateTimeSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<DataTimeSegmentService>(new { noteSegment = noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "TextSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TextSegmentService>(new { noteSegment = noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "TimerSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TimerSegmentService>(new { noteSegment = noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;

                case "TodoSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TodoSegmentService>(new { noteSegment = noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "KeyValueSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<KeyValueSegmentService>(new { noteSegment = noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "MediaSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<MediaSegmentService>(new { noteSegment = noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "DocumentSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<DocumentSegmentService>(new { noteSegment = noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "VoiceSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<VoiceSegmentService>(new { noteSegment = noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "ScriptSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<ScriptSegmentService>(new { noteSegment = noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                default:
                    newModel = null;
                    break;
            }

            return newModel;
        }
    }
}
