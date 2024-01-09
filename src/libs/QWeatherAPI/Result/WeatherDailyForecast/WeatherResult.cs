using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI.Result.WeatherDailyForecast
{
    public class WeatherResult
    {
        /// <summary>
        /// API 状态码
        /// </summary>
        public string Code;
        /// <summary>
        /// API 状态最近更新时间
        /// </summary>
        public string UpdateTime;
        /// <summary>
        /// 当前数据的响应式页面，便于嵌入网站或应用
        /// </summary>
        public string FxLink;
        /// <summary>
        /// 逐天天气预报
        /// </summary>
        public Daily[] Daily = new Daily[0];

        internal WeatherResult(string jsonString)
        {
            JObject jsonData = JObject.Parse(jsonString);
            Code = jsonData.Value<string>("code");
            UpdateTime = jsonData.Value<string>("updateTime");
            FxLink = jsonData.Value<string>("fxLink");
            foreach (var item in jsonData.SelectToken("daily"))
            {
                var dailyList = this.Daily.ToList();
                dailyList.Add(new Daily(item));
                this.Daily = dailyList.ToArray();
            }
        }
    }
}
