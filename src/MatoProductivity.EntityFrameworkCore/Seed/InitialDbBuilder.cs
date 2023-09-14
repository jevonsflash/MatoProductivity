using MatoProductivity.Core.Models.Entities;
using MatoProductivity.Infrastructure.Common;
using System;
using System.Drawing;

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

            CreateNoteSegmentStore("时间戳", "时间/提醒", "DateTimeSegment", "时间戳片段，记录一个瞬时时间，保存在您的笔记中", FaIcons.IconClockO, "#D8292B");
            CreateNoteSegmentStore("笔记", "文本", "TextSegment", "笔记片段，随时用文本记录您的想法", FaIcons.IconStickyNoteO, "#E1A08B");
            CreateNoteSegmentStore("Todo", "文本", "TodoSegment", "笔记片段，随时用文本记录您的想法", FaIcons.IconCheckSquareO, "#C7C3E3");
            CreateNoteSegmentStore("记录值", "文本", "KeyValueSegment", "记录值片段，通过回答对应问题以记录数值", FaIcons.IconCheckSquareO, "#C7C3E3");
            CreateNoteSegmentStore("计时器", "时间/提醒", "TimerSegment", "计时器片段，计时器结束后将提醒您", FaIcons.IconClockO, "#AD9CC2");
            CreateNoteSegmentStore("手绘", "文件", "ScriptSegment", "文档片段，计时器结束后将提醒您", FaIcons.IconClockO, "#AD9CC2");
            CreateNoteSegmentStore("照片/视频", "文件", "MediaSegment", "文档片段，计时器结束后将提醒您", FaIcons.IconClockO, "#AD9CC2");
            CreateNoteSegmentStore("文档", "文件", "DocumentSegment", "文档片段，计时器结束后将提醒您", FaIcons.IconClockO, "#AD9CC2");
            CreateNoteSegmentStore("录音", "文件", "VoiceSegment", "文档片段，计时器结束后将提醒您", FaIcons.IconClockO, "#AD9CC2");


            NoteTemplate noteTemplateEntity = CreateNoteTemplate("宝宝喂奶", null, "#000000", "#FFFFFF");
            NoteTemplate noteTemplateEntity2 = CreateNoteTemplate("灵感", null, "#000000", "#FFFFFF");
            var noteTemplateId = noteTemplateEntity.Id;
            var noteTemplateId2 = noteTemplateEntity2.Id;


           var noteSegmentTemplate1= CreateNoteSegmentTemplate(noteTemplateId, "开始", FaIcons.IconClockO, "DateTimeSegment", "喂奶开始时间", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId, "结束", FaIcons.IconClockO, "DateTimeSegment", "喂奶结束时间", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId, "宝宝饿啦", FaIcons.IconClockO, "TimerSegment", "", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId2, "备注", FaIcons.IconClockO, "TextSegment", "备注信息", "#000000");

            var noteSegmentTemplateId1 = noteSegmentTemplate1.Id;

            CreateNoteSegmentTemplatePayload(noteSegmentTemplateId1, "IsAutoSet", "True");

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

        private NoteSegmentTemplatePayload CreateNoteSegmentTemplatePayload(long noteSegmentTemplateId, string key, string value)
        {
            var noteSegmentTemplateEntity = this.context.Set<NoteSegmentTemplatePayload>().FirstOrDefault(c => c.NoteSegmentTemplateId == noteSegmentTemplateId && c.Key==key);
            if (noteSegmentTemplateEntity == null)
            {
                var item = new NoteSegmentTemplatePayload()
                {
                    NoteSegmentTemplateId = noteSegmentTemplateId,
                    Key=key
                };
                item.SetStringValue(value);

                var noteSegmentTemplateEntityEntry = this.context.Set<NoteSegmentTemplatePayload>().Add(item);
                this.context.SaveChanges();
                noteSegmentTemplateEntity = noteSegmentTemplateEntityEntry.Entity;
            }

            return noteSegmentTemplateEntity;
        }



        private NoteSegmentStore CreateNoteSegmentStore(string title, string category, string type, string desc, string icon, string color = "#000000")
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
                    Icon =icon

                });
                this.context.SaveChanges();
                noteSegmentStoreEntity = noteSegmentStoreEntityEntry.Entity;
            }

            return noteSegmentStoreEntity;
        }
    }
}