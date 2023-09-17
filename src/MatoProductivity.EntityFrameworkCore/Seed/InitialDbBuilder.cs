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
            CreateNoteSegmentStore("计时器", "时间/提醒", "TimerSegment", "计时器片段，计时器结束后将提醒您", FaIcons.IconBell, "#AD9CC2");
            CreateNoteSegmentStore("手绘", "文件", "ScriptSegment", "您可自由地手绘一个涂鸦，由我们进行记录", FaIcons.IconPencilSquareO, "#AD9CC2");
            CreateNoteSegmentStore("照片/视频", "文件", "MediaSegment", "拍照或摄像并存储", FaIcons.IconCamera, "#AD9CC2");
            CreateNoteSegmentStore("文档", "文件", "DocumentSegment", "从您设备中选取一个文档，并存储", FaIcons.IconFile, "#AD9CC2");
            CreateNoteSegmentStore("录音", "文件", "VoiceSegment", "记录一段声音", FaIcons.IconMicrophone, "#AD9CC2");
            CreateNoteSegmentStore("地点", "其它", "LocationSegment", "获取当前地点，或者从地图上选取一个地点并记录", FaIcons.IconMapMarker, "#AD9CC2");
            CreateNoteSegmentStore("天气", "其它", "WeatherSegment", "获取并记录当前天气信息", FaIcons.IconCloud, "#AD9CC2");
            CreateNoteSegmentStore("联系人", "其它", "ContactSegment", "从您设备的通讯录中选择一个联系人，记录其联系方式", FaIcons.IconUser, "#AD9CC2");


            NoteTemplate noteTemplateEntity = CreateNoteTemplate("喂奶","宝宝成长", null, "#000000", "#FFFFFF");
            NoteTemplate noteTemplateEntity2 = CreateNoteTemplate("灵感", "其它", null, "#000000", "#FFFFFF");
            NoteTemplate noteTemplateEntity3 = CreateNoteTemplate("打疫苗", "宝宝成长", null, "#000000", "#FFFFFF");
            NoteTemplate noteTemplateEntity4 = CreateNoteTemplate("记体温", "宝宝成长", null, "#000000", "#FFFFFF");
            NoteTemplate noteTemplateEntity5 = CreateNoteTemplate("吃辅食", "宝宝成长", null, "#000000", "#FFFFFF");
            NoteTemplate noteTemplateEntity6 = CreateNoteTemplate("记油耗", "其它", null, "#000000", "#FFFFFF");
            NoteTemplate noteTemplateEntity7 = CreateNoteTemplate("做儿保", "宝宝成长", null, "#000000", "#FFFFFF");
            var noteTemplateId = noteTemplateEntity.Id;


            var noteSegmentTemplate1 = CreateNoteSegmentTemplate(noteTemplateId, "开始", FaIcons.IconClockO, "DateTimeSegment", "喂奶开始时间", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId, "结束", FaIcons.IconClockO, "DateTimeSegment", "喂奶结束时间", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId, "宝宝饿啦", FaIcons.IconClockO, "TimerSegment", "", "#000000");

            var noteSegmentTemplateId1 = noteSegmentTemplate1.Id;

            CreateNoteSegmentTemplatePayload(noteSegmentTemplateId1, "IsAutoSet", true);


            var noteTemplateId2 = noteTemplateEntity2.Id;
            CreateNoteSegmentTemplate(noteTemplateId2, "备注", FaIcons.IconClockO, "TextSegment", "备注信息", "#000000");


            var noteTemplateId3 = noteTemplateEntity3.Id;
            CreateNoteSegmentTemplate(noteTemplateId3, "接种日期", FaIcons.IconClockO, "DateTimeSegment", "接种日期", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId3, "下次接种提醒", FaIcons.IconClockO, "TimerSegment", "下次接种提醒", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId3, "备注", FaIcons.IconClockO, "TextSegment", "备注信息", "#000000");

            var noteTemplateId4 = noteTemplateEntity4.Id;
            var noteSegmentTemplate4 = CreateNoteSegmentTemplate(noteTemplateId4, "时间", FaIcons.IconClockO, "DateTimeSegment", "接种时间", "#000000");
            var noteSegmentTemplateId4 = noteSegmentTemplate4.Id;

            CreateNoteSegmentTemplatePayload(noteSegmentTemplateId4, "IsAutoSet", true);

            var noteTemplateId6 = noteTemplateEntity6.Id;
            var noteSegmentTemplate6 = CreateNoteSegmentTemplate(noteTemplateId6, "里程", FaIcons.IconClockO, "KeyValueSegment", "接种时间", "#000000");
            var noteSegmentTemplate6_1 = CreateNoteSegmentTemplate(noteTemplateId6, "数量升", FaIcons.IconClockO, "KeyValueSegment", "接种时间", "#000000");
            var noteSegmentTemplate6_2 = CreateNoteSegmentTemplate(noteTemplateId6, "价格", FaIcons.IconClockO, "KeyValueSegment", "接种时间", "#000000");


            CreateNoteSegmentTemplatePayload(noteSegmentTemplate6.Id, "Content", 10000);
            CreateNoteSegmentTemplatePayload(noteSegmentTemplate6_1.Id, "Content", 60);
            CreateNoteSegmentTemplatePayload(noteSegmentTemplate6_2.Id, "Content", 400);


            var noteTemplateId7 = noteTemplateEntity7.Id;
            var noteSegmentTemplate7_1 = CreateNoteSegmentTemplate(noteTemplateId7, "日期", FaIcons.IconClockO, "DateTimeSegment", "日期", "#000000");

            CreateNoteSegmentTemplatePayload(noteSegmentTemplate7_1.Id, "IsAutoSet", true);
            CreateNoteSegmentTemplate(noteTemplateId7, "地点", FaIcons.IconClockO, "TimerSegment", "地点", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId7, "备注", FaIcons.IconClockO, "TextSegment", "备注信息", "#000000");

        }

        private NoteTemplate CreateNoteTemplate(string title, string type, string icon, string color = "#000000", string backgroundColor = "#FFFFFF")
        {
            var noteTemplateEntity = this.context.Set<NoteTemplate>().FirstOrDefault(c => c.Title == title);
            if (noteTemplateEntity == null)
            {
                var noteTemplateEntityEntry = this.context.Set<NoteTemplate>().Add(new NoteTemplate()
                {
                    Title = title,
                    Type=type,
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
            var noteSegmentTemplateEntity = this.context.Set<NoteSegmentTemplate>().FirstOrDefault(c => c.Title == title && c.NoteTemplateId==noteTemplateId);
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

        private NoteSegmentTemplatePayload CreateNoteSegmentTemplatePayload(long noteSegmentTemplateId, string key, object value)
        {
            var noteSegmentTemplateEntity = this.context.Set<NoteSegmentTemplatePayload>().FirstOrDefault(c => c.NoteSegmentTemplateId == noteSegmentTemplateId && c.Key==key);
            if (noteSegmentTemplateEntity == null)
            {
                var item = new NoteSegmentTemplatePayload(key,value)
                {
                    NoteSegmentTemplateId = noteSegmentTemplateId,
                };
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