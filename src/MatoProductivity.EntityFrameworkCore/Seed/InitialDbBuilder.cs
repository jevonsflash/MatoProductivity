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
          var noteEntity=  this.context.Set<Note>().Add(new Note() { 
             Title = "Test",

            });
            var noteId = noteEntity.Entity.Id;
            this.context.Set<NoteSegment>().Add(new NoteSegment()
            {
                NoteId = noteId,
                Title = "TestDateTime",
                Type = "DateTimeSegment",

            });
            this.context.Set<NoteSegment>().Add(new NoteSegment()
            {
                NoteId = noteId,
                Title = "TestText",
                Type = "TextSegment",

            });
        }
    }
}