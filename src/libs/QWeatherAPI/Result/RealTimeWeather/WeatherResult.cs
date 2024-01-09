using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI.Result.RealTimeWeather
{
    public class WeatherResult
    {
        /// <summary>
        /// API 状态码
        /// </summary>
        public string Code;
        /// <summary>
        /// API 最近更新时间
        /// </summary>
        public string UpdateTime;
        /// <summary>
        /// 当前数据的响应式页面，便于嵌入网站或应用
        /// </summary>
        public string FxLink;
        /// <summary>
        /// 天气数据
        /// </summary>
        public Now Now;

        /// <summary>
        /// 构造当前天气 API 返回结果
        /// </summary>
        /// <param name="jsonString"></param>
        /// <exception cref="ArgumentException"></exception>
        public WeatherResult(string jsonString)
        {
            JObject jsonData = JObject.Parse(jsonString);
            this.Code = jsonData.Value<string>("code");
            if (this.Code != "200") { throw new ArgumentException(this.Code); }
            this.UpdateTime = jsonData.Value<string>("updateTime");
            this.FxLink = jsonData.Value<string>("fxLink");
            this.Now = new Now(jsonData.SelectToken("now"));
        }
    }
}
