using Newtonsoft.Json.Linq;
using QWeatherAPI.Result.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI.Result.WeatherDailyForecast
{
    public class Daily
    {
        /// <summary>
        /// 预报日期
        /// </summary>
        public string FxDate;
        /// <summary>
        /// 日出时间
        /// </summary>
        public string Sunrise;
        /// <summary>
        /// 日落时间
        /// </summary>
        public string Sunset;
        /// <summary>
        /// 月升时间
        /// </summary>
        public string Moonrise;
        /// <summary>
        /// 月落时间
        /// </summary>
        public string Moonset;
        /// <summary>
        /// 月相名称
        /// </summary>
        public string MoonPhase;
        /// <summary>
        /// 月相图标代码
        /// </summary>
        public string MoonPhaseIcon;
        /// <summary>
        /// 预报当天最高温度
        /// </summary>
        public int TempMax;
        /// <summary>
        /// 预报当天最低温度
        /// </summary>
        public int TempMin;
        /// <summary>
        /// 预报白天天气状况的图标代码
        /// </summary>
        public string IconDay;
        /// <summary>
        /// 预报白天天气状况文字描述
        /// </summary>
        public string TextDay;
        /// <summary>
        /// 预报夜间天气状况的图标代码
        /// </summary>
        public string IconNight;
        /// <summary>
        /// 预报晚间天气状况文字描述
        /// </summary>
        public string TextNight;
        /// <summary>
        /// 预报白天风向 360 角度
        /// </summary>
        public int Wind360Day;
        /// <summary>
        /// 预报白天风向
        /// </summary>
        public string WindDirDay;
        /// <summary>
        /// 预报白天风力等级
        /// </summary>
        public WindScale WindScaleDay;
        /// <summary>
        /// 预报白天风速，公里/小时
        /// </summary>
        public int WindSpeedDay;
        /// <summary>
        /// 预报夜间风向 360 角度
        /// </summary>
        public int Wind360Night;
        /// <summary>
        /// 预报夜间当天风向
        /// </summary>
        public string WindDirNight;
        /// <summary>
        /// 预报夜间风力等级
        /// </summary>
        public WindScale WindScaleNight;
        /// <summary>
        /// 预报夜间风速，公里/小时
        /// </summary>
        public int WindSpeedNight;
        /// <summary>
        /// 相对湿度，百分比数值
        /// </summary>
        public int Humidity;
        /// <summary>
        /// 预报当天总降水量，默认单位：毫米
        /// </summary>
        public double Precip;
        /// <summary>
        /// 大气压强，默认单位：百帕
        /// </summary>
        public int Pressure;
        /// <summary>
        /// 能见度，默认单位：公里
        /// </summary>
        public int Vis;
        /// <summary>
        /// 云量，百分比数值。可能为空
        /// </summary>
        public int Cloud;
        /// <summary>
        /// 紫外线强度指数
        /// </summary>
        public int UvIndex;

        internal Daily(JToken token)
        {
            FxDate = token.Value<string>("fxDate");
            Sunrise = token.Value<string>("sunrise");
            Sunset = token.Value<string>("sunset");
            Moonrise = token.Value<string>("moonrise");
            Moonset = token.Value<string>("moonset");
            MoonPhase = token.Value<string>("moonPhase");
            MoonPhaseIcon = token.Value<string>("moonPhaseIcon");
            TempMax = token.Value<int>("tempMax");
            TempMin = token.Value<int>("tempMin");
            IconDay = token.Value<string>("iconDay");
            TextDay = token.Value<string>("textDay");
            IconNight = token.Value<string>("iconNight");
            TextNight = token.Value<string>("textNight");
            Wind360Day = token.Value<int>("wind360Day");
            WindDirDay = token.Value<string>("windDirDay");
            WindScaleDay = new WindScale(token.Value<string>("windScaleDay"));
            WindSpeedDay = token.Value<int>("windSpeedDay");
            Wind360Night = token.Value<int>("wind360Night");
            WindDirNight = token.Value<string>("windDirNight");
            WindScaleNight = new WindScale(token.Value<string>("windScaleNight"));
            WindSpeedNight = token.Value<int>("windSpeedNight");
            Humidity = token.Value<int>("humidity");
            Precip = token.Value<double>("precip");
            Pressure = token.Value<int>("pressure");
            Vis = token.Value<int>("vis");
            Cloud = token.Value<int>("cloud");
            UvIndex = token.Value<int>("uvIndex");
        }
    }
}
