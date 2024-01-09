using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI.Result.WeatherWarning
{
    public struct WarningResult
    {
        /// <summary>
        /// API 状态码，具体含义请参考状态码
        /// </summary>
        public string Code;
        /// <summary>
        /// 当前 API 的最近更新时间
        /// </summary>
        public string UpdateTime;
        /// <summary>
        /// 当前数据的响应式页面，便于嵌入网站或应用
        /// </summary>
        public string FxLink;
        /// <summary>
        /// 预警信息
        /// </summary>
        public Warning[] Warning;

        public WarningResult(string jsonString)
        {
            JObject jsonData = JObject.Parse(jsonString);
            Code = jsonData.Value<string>("code");
            UpdateTime = jsonData.Value<string>("updateTime");
            FxLink = jsonData.Value<string>("fxLink");
            List<Warning> warnings = new List<Warning>();
            foreach (var warning in jsonData.SelectToken("warning"))
            {
                warnings.Add(new Warning(warning));
            }
            Warning = warnings.ToArray();
        }
    }
}
