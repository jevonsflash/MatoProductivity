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
            Note noteEntity = CreateNote("Test");
            Note noteEntity2 = CreateNote("Test2");
            var noteId = noteEntity.Id;

            CreateNoteSegment(noteId, "TestDateTime", "DateTimeSegment", "this is DateTimeSegment test desc");
            CreateNoteSegment(noteId, "TestText", "TextSegment", "this is TextSegment test desc");           
        }

        private Note CreateNote(string title)
        {
            var noteEntity = this.context.Set<Note>().FirstOrDefault(c => c.Title == title);
            if (noteEntity == null)
            {
                var noteEntityEntry = this.context.Set<Note>().Add(new Note()
                {
                    Title = title,
                });
                this.context.SaveChanges();
                noteEntity = noteEntityEntry.Entity;
            }

            return noteEntity;
        }


        private NoteSegment CreateNoteSegment(long noteId, string title, string type, string desc)
        {
            var noteSegmentEntity = this.context.Set<NoteSegment>().FirstOrDefault(c => c.Title == title);
            if (noteSegmentEntity == null)
            {
                var noteSegmentEntityEntry = this.context.Set<NoteSegment>().Add(new NoteSegment()
                {
                    NoteId = noteId,
                    Title = title,
                    Type = type,
                    Desc = desc
                });
                this.context.SaveChanges();
                noteSegmentEntity = noteSegmentEntityEntry.Entity;
            }

            return noteSegmentEntity;
        }
    }
}