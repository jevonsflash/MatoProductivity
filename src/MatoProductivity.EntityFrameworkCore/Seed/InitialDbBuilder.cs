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

            CreateNoteSegmentStore("时间戳", "时间/提醒", "DateTimeSegment", "时间戳片段，记录一个瞬时时间，保存在您的笔记中", "red");
            CreateNoteSegmentStore("笔记", "文本", "TextSegment", "笔记片段，随时用文本记录您的想法", "green");
            CreateNoteSegmentStore("Todo", "文本", "TodoSegment", "笔记片段，随时用文本记录您的想法", "yellow");
            CreateNoteSegmentStore("计时器", "时间/提醒", "TimerSegment", "计时器片段，计时器结束后将提醒您", "yellow");
            CreateNoteSegmentStore("闹钟", "时间/提醒", "ClockSegment", "闹钟片段，到指定时间将提醒您", "yellow");


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



        private NoteSegmentStore CreateNoteSegmentStore(string title, string category, string type, string desc, string color)
        {
            var noteSegmentStoreEntity = this.context.Set<NoteSegmentStore>().FirstOrDefault(c => c.Title == title);
            if (noteSegmentStoreEntity == null)
            {
                var noteSegmentStoreEntityEntry = this.context.Set<NoteSegmentStore>().Add(new NoteSegmentStore()
                {
                    Title = title,
                    Category = category,
                    Type = type,
                    Desc = desc,
                    Color = color,
                });
                this.context.SaveChanges();
                noteSegmentStoreEntity = noteSegmentStoreEntityEntry.Entity;
            }

            return noteSegmentStoreEntity;
        }
    }
}