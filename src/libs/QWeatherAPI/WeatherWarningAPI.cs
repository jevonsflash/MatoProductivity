using QWeatherAPI.Result.WeatherWarning;
using QWeatherAPI.Tools;
using QWeatherAPI.Tools.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI
{
    public static class WeatherWarningAPI
    {
        /// <summary>
        /// 通过经度，纬度获取天气预警 API 数据。
        /// </summary>
        /// <param name="lon">地区经度。</param>
        /// <param name="lat">地区纬度。</param>
        /// <param name="key">和风天气 API 密钥。</param>
        /// <param name="lang">返回数据语言，默认采用中文。详情请参考：https://dev.qweather.com/docs/resource/language/</param>
        /// <returns>天气预警 API 返回数据。</returns>
        public static async Task<WarningResult> GetWeatherWarningAsync(double lon, double lat, string key, string lang = "zh")
        {
            var response = await WebRequests.GetRequestAsync($"https://devapi.qweather.com/v7/warning/now?location={lon},{lat}&lang={lang}&key={key}");
            string jsonData = await response.Content.ReadAsStringAsync();
            var weather = new WarningResult(jsonData);
            return weather;
        }

        /// <summary>
        /// 通过 LocationID 获取天气预警 API 数据。
        /// </summary>
        /// <param name="id">地区 LocationID，可通过 GeoAPI.GetGeoAsync() 来获取。</param>
        /// <param name="key">和风天气 API 密钥。</param>
        /// <param name="lang">返回数据语言，默认采用中文。详情请参考：https://dev.qweather.com/docs/resource/language/</param>
        /// <returns>天气预警 API 返回数据。</returns>
        public static async Task<WarningResult> GetWeatherWarningAsync(string id, string key, string lang = "zh")
        {
            var response = await WebRequests.GetRequestAsync($"https://devapi.qweather.com/v7/warning/now?location={id}&lang={lang}&key={key}");
            string jsonData = await response.Content.ReadAsStringAsync();
            var weather = new WarningResult(jsonData);
            return weather;
        }
    }
}
