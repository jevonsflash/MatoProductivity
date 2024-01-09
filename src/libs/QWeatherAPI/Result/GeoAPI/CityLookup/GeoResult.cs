using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI.Result.GeoAPI.CityLookup
{
    public class GeoResult
    {
        /// <summary>
        /// API 状态码
        /// </summary>
        public string Code;
        /// <summary>
        /// 位置搜索结果
        /// </summary>
        public Location[] Locations = new Location[0];

        /// <summary>
        /// 构造地理位置 API 返回结果
        /// </summary>
        /// <param name="jsonString"></param>
        /// <exception cref="ArgumentException"></exception>
        public GeoResult(string jsonString)
        {
            JObject jsonData = JObject.Parse(jsonString);
            this.Code = jsonData.Value<string>("code");
            if (this.Code != "200") { throw new ArgumentException(this.Code); }
            foreach (JToken location in jsonData.SelectToken("location"))
            {
                var locationList = this.Locations.ToList();
                locationList.Add(new Location(location));
                this.Locations = locationList.ToArray();
            }
        }
    }
}
