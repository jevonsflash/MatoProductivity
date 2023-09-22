﻿namespace MatoProductivity.Core.Amap
{
    /// <summary>
    /// 逆地址解析结果
    /// </summary>
    public class AmapInverseLocationResponse : AmapHttpResponse
    {
        public AmapRegeocode Regeocode { get; set; }
        public AmapInverseLocationResponse()
        {
            Regeocode = new AmapRegeocode();
        }
    }
}
