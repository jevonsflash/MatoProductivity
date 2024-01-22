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

            CreateNoteSegmentStore("时间戳", "时间/提醒", "DateTimeSegment", "记录一个瞬时时间", FaIcons.IconClockO, "#D8292B");
            CreateNoteSegmentStore("计时器", "时间/提醒", "TimerSegment", "创建计时器提醒", FaIcons.IconBell, "#D8292B");
            CreateNoteSegmentStore("笔记", "文本", "TextSegment", "随时用文本记录您的想法", FaIcons.IconStickyNoteO, "#E1A08B");
            CreateNoteSegmentStore("Todo", "文本", "TodoSegment", "记录一个Todo项目", FaIcons.IconCheckSquareO, "#E1A08B");
            CreateNoteSegmentStore("数值", "文本", "KeyValueSegment", "记录数值，以便统计数据", FaIcons.IconLineChart, "#E1A08B");
            CreateNoteSegmentStore("手绘", "文件", "ScriptSegment", "创建一个手绘", FaIcons.IconPaintBrush, "#AD9CC2");
            CreateNoteSegmentStore("照片/视频", "文件", "MediaSegment", "拍照或摄像", FaIcons.IconCamera, "#AD9CC2");
            CreateNoteSegmentStore("文档", "文件", "DocumentSegment", "从您设备中选取一个文档", FaIcons.IconFile, "#AD9CC2");
            CreateNoteSegmentStore("录音", "文件", "VoiceSegment", "记录一段声音", FaIcons.IconMicrophone, "#AD9CC2");
            CreateNoteSegmentStore("地点", "其它", "LocationSegment", "获取当前地点，或者从地图上选取一个地点", FaIcons.IconMapMarker, "#6D987C");
            CreateNoteSegmentStore("天气", "其它", "WeatherSegment", "获取当前天气信息", FaIcons.IconCloud, "#6D987C");
            CreateNoteSegmentStore("联系人", "其它", "ContactSegment", "从您设备的通讯录中选择一个联系人", FaIcons.IconUser, "#6D987C");



            #region 喂奶

            NoteTemplate noteTemplateEntity = CreateNoteTemplate("喂奶", "宝宝成长", "baby", "#000000", "#FFFFFF");

            var noteTemplateId = noteTemplateEntity.Id;
            var noteSegmentTemplate1 = CreateNoteSegmentTemplate(noteTemplateId, "开始", FaIcons.IconClockO, "DateTimeSegment", "喂奶开始时间", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId, "宝宝饿啦", FaIcons.IconClockO, "TimerSegment", "", "#000000");
            var noteSegmentTemplate1_2 = CreateNoteSegmentTemplate(noteTemplateId, "数量(ml)", FaIcons.IconCheckSquareO, "KeyValueSegment", "", "#000000");
            var noteSegmentTemplateId1 = noteSegmentTemplate1.Id;
            CreateNoteSegmentTemplatePayload(noteSegmentTemplateId1, "IsAutoSet", true);

            #endregion
            #region 打疫苗


            NoteTemplate noteTemplateEntity3 = CreateNoteTemplate("打疫苗", "宝宝成长", "medicine", "#000000", "#FFFFFF");
            var noteTemplateId3 = noteTemplateEntity3.Id;
            CreateNoteSegmentTemplate(noteTemplateId3, "接种日期", FaIcons.IconClockO, "DateTimeSegment", "接种日期", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId3, "下次接种提醒", FaIcons.IconClockO, "TimerSegment", "下次接种提醒", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId3, "备注", FaIcons.IconStickyNoteO, "TextSegment", "备注信息", "#000000");

            #endregion
            #region 量体温

            NoteTemplate noteTemplateEntity4 = CreateNoteTemplate("量体温", "宝宝成长", "speed_test", "#000000", "#FFFFFF");
            var noteTemplateId4 = noteTemplateEntity4.Id;
            var noteSegmentTemplate4 = CreateNoteSegmentTemplate(noteTemplateId4, "时间", FaIcons.IconClockO, "DateTimeSegment", "", "#000000");
            var noteSegmentTemplateId4 = noteSegmentTemplate4.Id;
            var noteSegmentTemplate4_2 = CreateNoteSegmentTemplate(noteTemplateId4, "体温(℃)", FaIcons.IconCheckSquareO, "KeyValueSegment", "", "#000000");

            CreateNoteSegmentTemplatePayload(noteSegmentTemplateId4, "IsAutoSet", true);

            #endregion
            #region 吃辅食

            NoteTemplate noteTemplateEntity5 = CreateNoteTemplate("吃辅食", "宝宝成长", "diet", "#000000", "#FFFFFF");
            var noteTemplateId5 = noteTemplateEntity5.Id;
            CreateNoteSegmentTemplate(noteTemplateId5, "数量(ml)", FaIcons.IconCheckSquareO, "KeyValueSegment", "", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId5, "过敏备注", FaIcons.IconStickyNoteO, "TextSegment", "备注信息", "#000000");


            #endregion
            #region 做儿保


            NoteTemplate noteTemplateEntity7 = CreateNoteTemplate("做儿保", "宝宝成长", "doctor", "#000000", "#FFFFFF");
            var noteTemplateId7 = noteTemplateEntity7.Id;
            var noteSegmentTemplate7_1 = CreateNoteSegmentTemplate(noteTemplateId7, "日期", FaIcons.IconClockO, "DateTimeSegment", "日期", "#000000");

            CreateNoteSegmentTemplatePayload(noteSegmentTemplate7_1.Id, "IsAutoSet", true);
            CreateNoteSegmentTemplate(noteTemplateId7, "地点", FaIcons.IconClockO, "TimerSegment", "地点", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId7, "备注", FaIcons.IconStickyNoteO, "TextSegment", "备注信息", "#000000");


            #endregion
            #region 宝宝出行


            NoteTemplate noteTemplateEntity8 = CreateNoteTemplate("宝宝出行", "宝宝成长", "motherhood", "#000000", "#FFFFFF");
            var noteTemplateId8 = noteTemplateEntity8.Id;
            var noteSegmentTemplate8_1 = CreateNoteSegmentTemplate(noteTemplateId8, "棉柔巾", FaIcons.IconCheckSquareO, "TodoSegment", "", "#000000");
            var noteSegmentTemplate8_2 = CreateNoteSegmentTemplate(noteTemplateId8, "尿不湿", FaIcons.IconCheckSquareO, "TodoSegment", "", "#000000");
            var noteSegmentTemplate8_3 = CreateNoteSegmentTemplate(noteTemplateId8, "奶粉", FaIcons.IconCheckSquareO, "TodoSegment", "", "#000000");
            var noteSegmentTemplate8_4 = CreateNoteSegmentTemplate(noteTemplateId8, "防蚊贴", FaIcons.IconCheckSquareO, "TodoSegment", "", "#000000");
            var noteSegmentTemplate8_5 = CreateNoteSegmentTemplate(noteTemplateId8, "保温杯", FaIcons.IconCheckSquareO, "TodoSegment", "", "#000000");
            var noteSegmentTemplate8_6 = CreateNoteSegmentTemplate(noteTemplateId8, "衣服", FaIcons.IconCheckSquareO, "TodoSegment", "", "#000000");
            var noteSegmentTemplate8_7 = CreateNoteSegmentTemplate(noteTemplateId8, "袜子", FaIcons.IconCheckSquareO, "TodoSegment", "", "#000000");
            var noteSegmentTemplate8_8 = CreateNoteSegmentTemplate(noteTemplateId8, "遮阳帽", FaIcons.IconCheckSquareO, "TodoSegment", "", "#000000");




            #endregion

            #region 灵感


            NoteTemplate noteTemplateEntity2 = CreateNoteTemplate("灵感", "其它", "step_to_the_sun", "#000000", "#FFFFFF");
            var noteTemplateId2 = noteTemplateEntity2.Id;
            CreateNoteSegmentTemplate(noteTemplateId2, "记下想法", FaIcons.IconStickyNoteO, "TextSegment", "备注信息", "#000000");
            CreateNoteSegmentTemplate(noteTemplateId2, "画出想法", FaIcons.IconPencilSquareO, "ScriptSegment", "备注信息", "#000000");


            #endregion
            #region 记油耗

            NoteTemplate noteTemplateEntity6 = CreateNoteTemplate("记油耗", "其它", "automobile", "#000000", "#FFFFFF");
            var noteTemplateId6 = noteTemplateEntity6.Id;
            var noteSegmentTemplate6 = CreateNoteSegmentTemplate(noteTemplateId6, "里程", FaIcons.IconCheckSquareO, "KeyValueSegment", "", "#000000");
            var noteSegmentTemplate6_1 = CreateNoteSegmentTemplate(noteTemplateId6, "数量升", FaIcons.IconCheckSquareO, "KeyValueSegment", "", "#000000");
            var noteSegmentTemplate6_2 = CreateNoteSegmentTemplate(noteTemplateId6, "价格", FaIcons.IconCheckSquareO, "KeyValueSegment", "", "#000000");


            CreateNoteSegmentTemplatePayload(noteSegmentTemplate6.Id, "Content", 10000);
            CreateNoteSegmentTemplatePayload(noteSegmentTemplate6_1.Id, "Content", 60);
            CreateNoteSegmentTemplatePayload(noteSegmentTemplate6_2.Id, "Content", 400);
            #endregion

            #region 打卡

            NoteTemplate noteTemplateEntity9 = CreateNoteTemplate("打卡", "其它", "speed_test", "#000000", "#FFFFFF", true);
            var noteTemplateId9 = noteTemplateEntity9.Id;
            var noteSegmentTemplate9 = CreateNoteSegmentTemplate(noteTemplateId9, "时间", FaIcons.IconClockO, "DateTimeSegment", "", "#000000");
            var noteSegmentTemplateId9 = noteSegmentTemplate9.Id;
            CreateNoteSegmentTemplatePayload(noteSegmentTemplateId9, "IsAutoSet", true);

            #endregion

            #region 打卡2

            NoteTemplate noteTemplateEntity10 = CreateNoteTemplate("打卡2", "其它", "speed_test", "#000000", "#FFFFFF", true);
            var noteTemplateId10 = noteTemplateEntity10.Id;
            var noteSegmentTemplate10 = CreateNoteSegmentTemplate(noteTemplateId10, "时间", FaIcons.IconClockO, "DateTimeSegment", "", "#000000");
            var noteSegmentTemplateId10 = noteSegmentTemplate10.Id;
            CreateNoteSegmentTemplatePayload(noteSegmentTemplateId10, "IsAutoSet", true);

            #endregion
        }

        private NoteTemplate CreateNoteTemplate(string title, string type, string icon, string color = "#000000", string backgroundColor = "#FFFFFF", bool canSimplified = false)
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
                    CanSimplified=canSimplified

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
                var item = new NoteSegmentTemplatePayload(key, value)
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