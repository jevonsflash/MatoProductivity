using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QWeatherAPI.Result.Share
{
    public class WindScale
    {
        /// <summary>
        /// 最大风力等级
        /// </summary>
        public int ScaleMax;
        /// <summary>
        /// 最小风力等级
        /// </summary>
        public int ScaleMin;

        internal WindScale(string scale)
        {
            string[] splitScale = scale.Split('-');
            ScaleMin = int.Parse(splitScale[0]);
            ScaleMax = int.Parse(splitScale[1]);
        }
    }
}
