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

        public INoteSegmentService GetNoteSegmentTemplateService(NoteSegmentTemplate noteSegmentTemplate)
        {
            var type = noteSegmentTemplate.Type;
            INoteSegmentService newModel;
            switch (type)
            {
                case "DateTimeSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<DataTimeSegmentService>(new { noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "TextSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TextSegmentService>(new { noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "TimerSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TimerSegmentService>(new { noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;

                case "TodoSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TodoSegmentService>(new { noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "KeyValueSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<KeyValueSegmentService>(new { noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "MediaSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<MediaSegmentService>(new { noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "DocumentSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<DocumentSegmentService>(new { noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "VoiceSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<VoiceSegmentService>(new { noteSegmentTemplate }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "ScriptSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<ScriptSegmentService>(new { noteSegmentTemplate }))
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
