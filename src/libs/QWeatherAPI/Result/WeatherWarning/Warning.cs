using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI.Result.WeatherWarning
{
    public struct Warning
    {
        /// <summary>
        /// 本条预警的唯一标识，可判断本条预警是否已经存在
        /// </summary>
        public string ID;
        /// <summary>
        /// 预警发布单位，可能为空
        /// </summary>
        public string Sender;
        /// <summary>
        /// 预警发布时间
        /// </summary>
        public string PubTime;
        /// <summary>
        /// 预警信息标题
        /// </summary>
        public string Title;
        /// <summary>
        /// 预警开始时间，可能为空
        /// </summary>
        public string StartTime;
        /// <summary>
        /// 预警结束时间，可能为空
        /// </summary>
        public string EndTime;
        /// <summary>
        /// 预警信息的发布状态
        /// </summary>
        public string Status;
        /// <summary>
        /// 预警等级
        /// </summary>
        public string Level;
        /// <summary>
        /// 预警类型ID
        /// </summary>
        public string Type;
        /// <summary>
        /// 预警类型名称
        /// </summary>
        public string TypeName;
        /// <summary>
        /// 预警信息的紧迫程度，可能为空
        /// </summary>
        public string Text;
        /// <summary>
        /// 预警信息的确定性，可能为空
        /// </summary>
        public string Urgency;
        /// <summary>
        /// 预警详细文字描述
        /// </summary>
        public string Certainty;
        /// <summary>
        /// 与本条预警相关联的预警 ID，当预警状态为 cancel 或 update 时返回。可能为空
        /// </summary>
        public string Related;

        public Warning(JToken token)
        {
            ID = token.Value<string>("id");
            Sender = token.Value<string>("sender");
            PubTime = token.Value<string>("pubTime");
            Title = token.Value<string>("title");
            StartTime = token.Value<string>("startTime");
            EndTime = token.Value<string>("endTime");
            Status = token.Value<string>("status");
            Level = token.Value<string>("level");
            Type = token.Value<string>("type");
            TypeName = token.Value<string>("typeName");
            Text = token.Value<string>("text");
            Urgency = token.Value<string>("urgency");
            Certainty = token.Value<string>("certainly");
            Related = token.Value<string>("related");
        }
    }
}
