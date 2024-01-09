using QWeatherAPI.Result.GeoAPI.CityLookup;
using QWeatherAPI.Tools.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI
{
    public class GeoAPI
    {
        /// <summary>
        /// 通过地区名称获取 LocationID 和 经纬度。
        /// </summary>
        /// <param name="location">地区名称，支持绝大多数语言。</param>
        /// <param name="key">和风天气 API 密钥。</param>
        /// <param name="adm">城市的上级行政区划。</param>
        /// <param name="range">搜索限定范围。国家代码需使用 ISO 3166 所定义的国家代码，默认为 10。详情请参考：https://www.iso.org/obp/ui/</param>
        /// <param name="limit">地理位置 API 返回数据限制个数。</param>
        /// <param name="lang">返回数据语言，默认采用中文。详情请参考：https://dev.qweather.com/docs/resource/language/</param>
        /// <returns>城市信息 API 返回数据。</returns>
        public static async Task<GeoResult> GetGeoAsync(string location, string key, string adm, string range = "world", int limit = 10, string lang = "zh")
        {
            range = range.ToLower();
            var response = await WebRequests.GetRequestAsync($"https://geoapi.qweather.com/v2/city/lookup?location={location}&number={limit}&adm={adm}&range={range}&lang={lang}&key={key}");
            string jsonData = await response.Content.ReadAsStringAsync();
            return new GeoResult(jsonData);
        }

        /// <summary>
        /// 通过地区名称获取 LocationID 和 经纬度。
        /// </summary>
        /// <param name="location">地区名称，支持绝大多数语言。</param>
        /// <param name="key">和风天气 API 密钥。</param>
        /// <param name="range">搜索限定范围。国家代码需使用 ISO 3166 所定义的国家代码，默认为 10。详情请参考：https://www.iso.org/obp/ui/</param>
        /// <param name="limit">地理位置 API 返回数据限制个数。</param>
        /// <param name="lang">返回数据语言，默认采用中文。详情请参考：https://dev.qweather.com/docs/resource/language/</param>
        /// <returns>城市信息 API 返回数据。</returns>
        public static async Task<GeoResult> GetGeoAsync(string location, string key, string range = "world", int limit = 10, string lang = "zh")
        {
            range = range.ToLower();
            var response = await WebRequests.GetRequestAsync($"https://geoapi.qweather.com/v2/city/lookup?location={location}&number={limit}&range={range}&lang={lang}&key={key}");
            string jsonData = await response.Content.ReadAsStringAsync();
            return new GeoResult(jsonData);
        }
    }
}
