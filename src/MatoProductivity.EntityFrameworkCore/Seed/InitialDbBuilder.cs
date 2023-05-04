using MatoProductivity.Core.Models.Entities;
using System;

namespace MatoProductivity.EntityFrameworkCore.Seed
{
    internal class InitialDbBuilder
    {
        private MatoProductivityDbContext context;

        public InitialDbBuilder(MatoProductivityDbContext context)
        {
            this.context = context;
        }

        internal void Create()
        {
            NoteTemplate noteTemplateEntity = CreateNoteTemplate("Test");
            NoteTemplate noteTemplateEntity2 = CreateNoteTemplate("Test2");
            var noteTemplateId = noteTemplateEntity.Id;

            CreateNoteSegmentTemplate(noteTemplateId, "TestDateTime", "DateTimeSegment", "this is DateTimeSegment test desc");
            CreateNoteSegmentTemplate(noteTemplateId, "TestText", "TextSegment", "this is TextSegment test desc");           
        }

        private NoteTemplate CreateNoteTemplate(string title)
        {
            var noteTemplateEntity = this.context.Set<NoteTemplate>().FirstOrDefault(c => c.Title == title);
            if (noteTemplateEntity == null)
            {
                var noteTemplateEntityEntry = this.context.Set<NoteTemplate>().Add(new NoteTemplate()
                {
                    Title = title,
                });
                this.context.SaveChanges();
                noteTemplateEntity = noteTemplateEntityEntry.Entity;
            }

            return noteTemplateEntity;
        }


        private NoteSegmentTemplate CreateNoteSegmentTemplate(long noteTemplateId, string title, string type, string desc)
        {
            var noteSegmentTemplateEntity = this.context.Set<NoteSegmentTemplate>().FirstOrDefault(c => c.Title == title);
            if (noteSegmentTemplateEntity == null)
            {
                var noteSegmentTemplateEntityEntry = this.context.Set<NoteSegmentTemplate>().Add(new NoteSegmentTemplate()
                {
                    NoteTemplateId = noteTemplateId,
                    Title = title,
                    Type = type,
                    Desc = desc
                });
                this.context.SaveChanges();
                noteSegmentTemplateEntity = noteSegmentTemplateEntityEntry.Entity;
            }

            return noteSegmentTemplateEntity;
        }
    }
}