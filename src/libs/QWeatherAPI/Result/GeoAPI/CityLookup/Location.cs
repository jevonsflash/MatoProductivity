using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI.Result.GeoAPI.CityLookup
{
    public class Location
    {
        /// <summary>
        /// 地区/城市名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 地区/城市 ID
        /// </summary>
        public string ID;
        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat;
        /// <summary>
        /// 经度
        /// </summary>
        public double Lon;
        /// <summary>
        /// 地区/城市的上级行政区划名称
        /// </summary>
        public string Adm2;
        /// <summary>
        /// 地区/城市所属一级行政区域
        /// </summary>
        public string Adm1;
        /// <summary>
        /// 地区/城市所属国家
        /// </summary>
        public string Country;
        /// <summary>
        /// 地区/城市所在时区
        /// </summary>
        public string Tz;
        /// <summary>
        /// 地区/城市目前与UTC时间偏移的小时数
        /// </summary>
        public string UtcOffset;
        /// <summary>
        /// 地区/城市是否当前处于夏令时
        /// </summary>
        public bool IsDst;
        /// <summary>
        /// 地区/城市的属性
        /// </summary>
        public string Type;
        /// <summary>
        /// 地区评分
        /// </summary>
        public string Rank;

        public Location(JToken token)
        {
            this.Name = token.Value<string>("name");
            this.ID = token.Value<string>("id");
            this.Lat = token.Value<double>("lat");
            this.Lon = token.Value<double>("lon");
            this.Adm2 = token.Value<string>("adm2");
            this.Adm1 = token.Value<string>("adm1");
            this.Country = token.Value<string>("country");
            this.Tz = token.Value<string>("tz");
            this.UtcOffset = token.Value<string>("utcOffset");
            if (token.Value<string>("isDst") == "1")
            {
                this.IsDst = true;
            }
            else
            {
                this.IsDst = false;
            }
            this.Type = token.Value<string>("type");
            this.Rank = token.Value<string>("rank");
        }
    }
}
