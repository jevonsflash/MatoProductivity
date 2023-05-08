using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Infrastructure.Common;
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

            CreateNoteSegmentStore("时间戳", "时间/提醒", "DateTimeSegment", "时间戳片段，记录一个瞬时时间，保存在您的笔记中", "#D8292B");
            CreateNoteSegmentStore("笔记", "文本", "TextSegment", "笔记片段，随时用文本记录您的想法", "#E1A08B");
            CreateNoteSegmentStore("Todo", "文本", "TodoSegment", "笔记片段，随时用文本记录您的想法", "#C7C3E3");
            CreateNoteSegmentStore("计时器", "时间/提醒", "TimerSegment", "计时器片段，计时器结束后将提醒您", "#AD9CC2");
            CreateNoteSegmentStore("闹钟", "时间/提醒", "ClockSegment", "闹钟片段，到指定时间将提醒您", "#A07DA0");


            NoteTemplate noteTemplateEntity = CreateNoteTemplate("给孩子喂奶", null, "#000000", "#FFFFFF");
            NoteTemplate noteTemplateEntity2 = CreateNoteTemplate("灵感", null, "#000000", "#FFFFFF");
            var noteTemplateId = noteTemplateEntity.Id;
            var noteTemplateId2 = noteTemplateEntity2.Id;


            CreateNoteSegmentTemplate(noteTemplateId, "开始时间", FaIcons.IconClockO, "DateTimeSegment", "喂奶开始时间", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId, "结束时间", FaIcons.IconClockO, "DateTimeSegment", "喂奶结束时间", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId2, "备注", FaIcons.IconClockO, "TextSegment", "备注信息", "#000000");
        }

        private NoteTemplate CreateNoteTemplate(string title, string icon, string color = "#000000", string backgroundColor = "#FFFFFF")
        {
            var noteTemplateEntity = this.context.Set<NoteTemplate>().FirstOrDefault(c => c.Title == title);
            if (noteTemplateEntity == null)
            {
                var noteTemplateEntityEntry = this.context.Set<NoteTemplate>().Add(new NoteTemplate()
                {
                    Title = title,
                    Icon=icon,
                    Color=color,
                    BackgroundColor=backgroundColor,

                });
                this.context.SaveChanges();
                noteTemplateEntity = noteTemplateEntityEntry.Entity;
            }

            return noteTemplateEntity;
        }


        private NoteSegmentTemplate CreateNoteSegmentTemplate(long noteTemplateId, string title, string icon, string type, string desc, string color = "#000000")
        {
            var noteSegmentTemplateEntity = this.context.Set<NoteSegmentTemplate>().FirstOrDefault(c => c.Title == title);
            if (noteSegmentTemplateEntity == null)
            {
                var noteSegmentTemplateEntityEntry = this.context.Set<NoteSegmentTemplate>().Add(new NoteSegmentTemplate()
                {
                    NoteTemplateId = noteTemplateId,
                    Title = title,
                    Type = type,
                    Desc = desc,
                    Icon=icon,
                    Color=color
                });
                this.context.SaveChanges();
                noteSegmentTemplateEntity = noteSegmentTemplateEntityEntry.Entity;
            }

            return noteSegmentTemplateEntity;
        }



        private NoteSegmentStore CreateNoteSegmentStore(string title, string category, string type, string desc, string color = "#000000")
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