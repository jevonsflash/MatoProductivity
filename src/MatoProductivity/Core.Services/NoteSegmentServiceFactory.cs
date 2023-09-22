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
    public class NoteSegmentServiceFactory : INoteSegmentServiceFactory
    {
        private readonly IIocResolver iocResolver;

        public NoteSegmentServiceFactory(IIocResolver iocResolver)
        {
            this.iocResolver = iocResolver;
        }

        public INoteSegmentService GetNoteSegmentService(NoteSegment noteSegment)
        {
            var type = noteSegment.Type;
            INoteSegmentService newModel;
            switch (type)
            {
                case "DateTimeSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<DataTimeSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "TextSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TextSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "TimerSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TimerSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;

                case "TodoSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<TodoSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "KeyValueSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<KeyValueSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "MediaSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<MediaSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "DocumentSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<DocumentSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "VoiceSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<VoiceSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;
                case "ScriptSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<ScriptSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;

                case "LocationSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<LocationSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;


                case "ContactSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<ContactSegmentService>(new { noteSegment }))
                    {
                        newModel = objWrapper.Object;
                    }
                    break;

                case "WeatherSegment":
                    using (var objWrapper = iocResolver.ResolveAsDisposable<WeatherSegmentService>(new { noteSegment }))
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
