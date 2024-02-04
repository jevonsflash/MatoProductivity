﻿using System;
using System.Text;

namespace MatoProductivity.Core.Location
{
    public class Location
    {
        public Location()
        {

        }
        public Location(double latitude, double longitude)
        {
            Latitude=latitude;
            Longitude=longitude;
        }

        public Location(string location)
        {
            var locationArray = location.Split(',');
            Latitude=double.Parse(locationArray[0]);
            Longitude=double.Parse(locationArray[1]);
        }
        /// <summary>
        /// 地球半径（米）
        /// </summary>
        public const double EARTH_RADIUS = 6378137;
        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude { get; set; }
        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// 计算两个位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="location">参与计算的位置信息</param>
        /// <returns>返回两个位置的距离</returns>
        public double CalcDistance(Location location)
        {
            return CalcDistance(this, location);
        }
        /// <summary>
        /// 计算两个位置的距离，返回两点的距离，单位 米
        /// 该公式为GOOGLE提供，误差小于0.2米
        /// </summary>
        /// <param name="location1">参与计算的位置信息</param>
        /// <param name="location2">参与计算的位置信息</param>
        /// <returns>返回两个位置的距离</returns>
        public static double CalcDistance(Location location1, Location location2)
        {
            double radLat1 = Rad(location1.Latitude);
            double radLng1 = Rad(location1.Longitude);
            double radLat2 = Rad(location2.Latitude);
            double radLng2 = Rad(location2.Longitude);
            double a = radLat1 - radLat2;
            double b = radLng1 - radLng2;
            double result = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(b / 2), 2)));
            result *= EARTH_RADIUS;
            return result;
        }
        /// <summary>
        /// 计算位置的偏移距离
        /// </summary>
        /// <param name="location">参与计算的位置</param>
        /// <param name="distance">位置偏移量,单位 米</param>
        /// <returns></returns>
        public static Position CalcOffsetDistance(Location location, double distance)
        {
            double dlng = 2 * Math.Asin(Math.Sin(distance / (2 * EARTH_RADIUS)) / Math.Cos(Rad(location.Latitude)));
            dlng = Deg(dlng);
            double dlat = distance / EARTH_RADIUS;
            dlat = Deg(dlat);
            double leftTopLat = location.Latitude + dlat;
            double leftTopLng = location.Longitude - dlng;

            double leftBottomLat = location.Latitude - dlat;
            double leftBottomLng = location.Longitude - dlng;

            double rightTopLat = location.Latitude + dlat;
            double rightTopLng = location.Longitude + dlng;

            double rightBottomLat = location.Latitude - dlat;
            double rightBottomLng = location.Longitude + dlng;

            return new Position(leftTopLat, leftBottomLat, leftTopLng, leftBottomLng,
                rightTopLat, rightBottomLat, rightTopLng, rightBottomLng);
        }

        /// <summary>
        /// 角度转换为弧度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double Rad(double d)
        {
            return d * Math.PI / 180d;
        }

        /// <summary>
        /// 弧度转换为角度
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double Deg(double d)
        {
            return d * (180 / Math.PI);
        }

        public override string ToString()
        {
            return $"{Latitude},{Longitude}";
        }

        public string ToFriendlyString()
        {

            StringBuilder sb = new StringBuilder();

            // 格式化纬度为度、分、秒形式  
            string friendlyLatitude = Latitude.ToString("F3"); // 3位小数  
            string[] latitudeParts = friendlyLatitude.Split('.');
            int degrees = int.Parse(latitudeParts[0]);
            int minutes = (int)Math.Round((double)(int.Parse(latitudeParts[1]) * 60)); // 将小数部分转换为分钟  
            decimal seconds = (decimal)(int.Parse(latitudeParts[1]) * 3600 - minutes * 60); // 计算剩余的秒数  
            string friendlyLatitudeFormatted = $"{degrees}°{minutes}'{seconds}''N";

            // 格式化经度为度、分、秒形式  
            string friendlyLongitude = Longitude.ToString("F3"); // 3位小数  
            string[] longitudeParts = friendlyLongitude.Split('.');
            int degreesLong = int.Parse(longitudeParts[0]);
            int minutesLong = (int)Math.Round((double)(int.Parse(longitudeParts[1]) * 60)); // 将小数部分转换为分钟  
            decimal secondsLong = (decimal)(int.Parse(longitudeParts[1]) * 3600 - minutesLong * 60); // 计算剩余的秒数  
            string friendlyLongitudeFormatted = $"{degreesLong}°{minutesLong}'{secondsLong}''W";

            sb.AppendLine($"纬度：{friendlyLatitudeFormatted}");
            sb.AppendLine($"经度：{friendlyLongitudeFormatted}");

            return sb.ToString();
        }
    }
}
