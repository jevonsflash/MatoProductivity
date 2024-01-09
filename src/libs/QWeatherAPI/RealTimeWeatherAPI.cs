using QWeatherAPI.Result.RealTimeWeather;
using QWeatherAPI.Tools;
using QWeatherAPI.Tools.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI
{
    public static class RealTimeWeatherAPI
    {
        /// <summary>
        /// 通过经度，纬度获取和风天气实时天气 API 数据。
        /// </summary>
        /// <param name="lon">地区经度。</param>
        /// <param name="lat">地区纬度。</param>
        /// <param name="key">和风天气 API 密钥。</param>
        /// <param name="unit">测量单位，Tools.Units.Metric 为公制，Tools.Units.Inch 为英制，默认采用 公制。</param>
        /// <param name="lang">返回数据语言，默认采用中文。详情请参考：https://dev.qweather.com/docs/resource/language/</param>
        /// <returns>实时天气 API 返回数据。</returns>
        public static async Task<WeatherResult> GetRealTimeWeatherAsync(double lon, double lat, string key, Units unit = Units.Metric, string lang = "zh")
        {
            string _unit;
            switch (unit)
            {
                case Units.Metric:
                    _unit = "m";
                    break;
                case Units.Inch:
                    _unit = "i";
                    break;
                default:
                    goto case Units.Metric;
            }
            var response = await WebRequests.GetRequestAsync($"https://devapi.qweather.com/v7/weather/now?location={lon},{lat}&lang={lang}&unit={_unit}&key={key}");
            string jsonData = await response.Content.ReadAsStringAsync();
            var weather = new WeatherResult(jsonData);
            return weather;
        }

        /// <summary>
        /// 通过 LocationID 获取和风天气实时天气 API 数据。
        /// </summary>
        /// <param name="id">地区 LocationID，可通过 GeoAPI.GetGeoAsync() 来获取。</param>
        /// <param name="key">和风天气 API 密钥。</param>
        /// <param name="unit">测量单位，Tools.Units.Metric 为公制，Tools.Units.Inch 为英制，默认采用 公制。</param>
        /// <param name="lang">返回数据语言，默认采用中文。详情请参考：https://dev.qweather.com/docs/resource/language/</param>
        /// <returns>实时天气 API 返回数据。</returns>
        public static async Task<WeatherResult> GetRealTimeWeatherAsync(string id, string key, Units unit = Units.Metric, string lang = "zh")
        {
            string _unit;
            switch (unit)
            {
                case Units.Metric:
                    _unit = "m";
                    break;
                case Units.Inch:
                    _unit = "i";
                    break;
                default:
                    goto case Units.Metric;
            }
            var response = await WebRequests.GetRequestAsync($"https://devapi.qweather.com/v7/weather/now?location={id}&lang={lang}&unit={_unit}&key={key}");
            string jsonData = await response.Content.ReadAsStringAsync();
            var weather = new WeatherResult(jsonData);
            return weather;
        }
    }
}
