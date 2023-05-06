using Abp.Dependency;
using MatoProductivity.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Core.Services
{
    public class NoteSegmentServiceFactory :  INoteSegmentServiceFactory
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
                default:
                    newModel = null;
                    break;
            }

            return newModel;
        }
    }
}
