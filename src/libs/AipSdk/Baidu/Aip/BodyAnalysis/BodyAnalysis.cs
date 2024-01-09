/*
 * Copyright 2017 Baidu, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with
 * the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on
 * an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations under the License.
 */

using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Baidu.Aip.BodyAnalysis
{
    /// <summary>
    /// 人体属性分析
    /// </summary>
    public class Body : AipServiceBase   {
        
        private const string BODY_ANALYSIS =
            "https://aip.baidubce.com/rest/2.0/image-classify/v1/body_analysis";
        
        private const string BODY_ATTR =
            "https://aip.baidubce.com/rest/2.0/image-classify/v1/body_attr";
        
        private const string BODY_NUM =
            "https://aip.baidubce.com/rest/2.0/image-classify/v1/body_num";
        
        private const string GESTURE =
            "https://aip.baidubce.com/rest/2.0/image-classify/v1/gesture";
        
        private const string BODY_SEG =
            "https://aip.baidubce.com/rest/2.0/image-classify/v1/body_seg";
        
        private const string DRIVER_BEHAVIOR =
            "https://aip.baidubce.com/rest/2.0/image-classify/v1/driver_behavior";
        
        private const string BODY_TRACKING =
            "https://aip.baidubce.com/rest/2.0/image-classify/v1/body_tracking";
        
        public Body(string apiKey, string secretKey) : base(apiKey, secretKey)
        {

        }

        protected AipHttpRequest DefaultRequest(string uri)
        {
            return new AipHttpRequest(uri)
            {
                Method = "POST",
                BodyType = AipHttpRequest.BodyFormat.Formed,
                ContentEncoding = Encoding.UTF8
            };
        }

        /// <summary>
        /// 人体关键点识别接口
        /// 对于输入的一张图片（可正常解码，且长宽比适宜），**检测图片中的所有人体，输出每个人体的21个主要关键点，包含头顶、五官、脖颈、四肢等部位，同时输出人体的坐标信息和数量**。
        /// </summary>
        /// <param name="image">二进制图像数据</param>
        /// <param name="options"> 可选参数对象，key: value都为string类型，可选的参数包括
        ///     <list type="bullet">
        ///     </list>
        /// </param>
        /// <return>JObject</return>
        ///
        public JObject BodyAnalysis(byte[] image, Dictionary<string, object> options = null)
        {
            var aipReq = DefaultRequest(BODY_ANALYSIS);
            
            CheckNotNull(image, "image");
            aipReq.Bodys["image"] = System.Convert.ToBase64String(image);
            PreAction();

            if (options != null)
                foreach (var pair in options)
                    aipReq.Bodys[pair.Key] = pair.Value;
            return PostAction(aipReq);
        }

        /// <summary>
        /// 人体检测与属性识别接口
        /// 对于输入的一张图片（可正常解码，且长宽比适宜），**检测图像中的所有人体并返回每个人体的矩形框位置，识别人体的静态属性和行为，共支持20余种属性，包括：性别、年龄阶段、衣着（含类别/颜色）、是否戴帽子、是否戴眼镜、是否背包、是否使用手机、身体朝向等**。
        /// </summary>
        /// <param name="image">二进制图像数据</param>
        /// <param name="options"> 可选参数对象，key: value都为string类型，可选的参数包括
        ///     <list type="bullet">
        ///           <item>  <c>type</c>: gender,<br>age,<br>lower_wear,<br>upper_wear,<br>headwear,<br>glasses,<br>upper_color,<br>lower_color,<br>cellphone,<br>upper_wear_fg,<br>upper_wear_texture,<br>lower_wear_texture,<br>orientation,<br>umbrella,<br>bag,<br>smoke,<br>vehicle,<br>carrying_item,<br>upper_cut,<br>lower_cut,<br>occlusion,<br>is_human | 1）可选值说明：<br>gender-性别，<br>age-年龄阶段，<br>lower_wear-下身服饰，<br>upper_wear-上身服饰，<br>headwear-是否戴帽子，<br>glasses-是否戴眼镜，<br>upper_color-上身服饰颜色，<br>lower_color-下身服饰颜色，<br>cellphone-是否使用手机，<br>upper_wear_fg-上身服饰细分类，<br>upper_wear_texture-上身服饰纹理，<br>orientation-身体朝向，<br>umbrella-是否撑伞；<br>bag-背包,<br>smoke-是否吸烟,<br>vehicle-交通工具,<br>carrying_item-是否有手提物,<br>upper_cut-上方截断,<br>lower_cut-下方截断,<br>occlusion-遮挡,<br>is_human-是否是正常人体<br>2）type 参数值可以是可选值的组合，用逗号分隔；**如果无此参数默认输出全部21个属性** </item>
        ///     </list>
        /// </param>
        /// <return>JObject</return>
        ///
        public JObject BodyAttr(byte[] image, Dictionary<string, object> options = null)
        {
            var aipReq = DefaultRequest(BODY_ATTR);
            
            CheckNotNull(image, "image");
            aipReq.Bodys["image"] = System.Convert.ToBase64String(image);
            PreAction();

            if (options != null)
                foreach (var pair in options)
                    aipReq.Bodys[pair.Key] = pair.Value;
            return PostAction(aipReq);
        }

        /// <summary>
        /// 人流量统计接口
        /// 对于输入的一张图片（可正常解码，且长宽比适宜），**识别和统计图像当中的人体个数（静态统计，暂不支持追踪和去重）**。
        /// </summary>
        /// <param name="image">二进制图像数据</param>
        /// <param name="options"> 可选参数对象，key: value都为string类型，可选的参数包括
        ///     <list type="bullet">
        ///           <item>  <c>area</c>: 特定框选区域坐标，支持多个多边形区域，最多支持10个区域，如输入超过10个区域，截取前10个区域进行识别。<br>**此参数为空或无此参数、或area参数设置错误时，默认识别整个图片的人数** 。<br>area参数设置错误的示例：某个坐标超过原图大小，x、y坐标未成对出现等；注意：**设置了多个区域时，任意一个坐标设置错误，则认为area参数错误、失效**。<br>**area参数设置格式**：<br>1）多个区域用英文分号“;”分隔；<br>2）同一个区域内的坐标用英文逗号“,”分隔，默认尾点和首点相连做闭合。<br>示例：<br>1）单个多边形区域：x1,y1,x2,y2,x3,y3...xn,yn<br>2）多个多边形区域：xa1,ya1,xa2,ya2,xa3,ya3...xan,yan;xb1,yb1,xb2,yb2,xb3,yb3...xbn,ybn;.. </item>
        ///           <item>  <c>show</c>: 是否输出渲染的图片，默认不返回，**选true时返回渲染后的图片(base64)**，其它无效值或为空则默认false </item>
        ///     </list>
        /// </param>
        /// <return>JObject</return>
        ///
        public JObject BodyNum(byte[] image, Dictionary<string, object> options = null)
        {
            var aipReq = DefaultRequest(BODY_NUM);
            
            CheckNotNull(image, "image");
            aipReq.Bodys["image"] = System.Convert.ToBase64String(image);
            PreAction();

            if (options != null)
                foreach (var pair in options)
                    aipReq.Bodys[pair.Key] = pair.Value;
            return PostAction(aipReq);
        }

        /// <summary>
        /// 手势识别接口
        /// 识别图片中的手势类型，返回手势名称、手势矩形框、概率分数，可识别24种常见手势，适用于手势特效、智能家居手势交互等场景**。支持的24类手势列表：拳头、OK、祈祷、作揖、作别、单手比心、点赞、Diss、我爱你、掌心向上、双手比心（3种）、数字（9种）、Rock、竖中指。
        /// </summary>
        /// <param name="image">二进制图像数据</param>
        /// <param name="options"> 可选参数对象，key: value都为string类型，可选的参数包括
        ///     <list type="bullet">
        ///     </list>
        /// </param>
        /// <return>JObject</return>
        ///
        public JObject Gesture(byte[] image, Dictionary<string, object> options = null)
        {
            var aipReq = DefaultRequest(GESTURE);
            
            CheckNotNull(image, "image");
            aipReq.Bodys["image"] = System.Convert.ToBase64String(image);
            PreAction();

            if (options != null)
                foreach (var pair in options)
                    aipReq.Bodys[pair.Key] = pair.Value;
            return PostAction(aipReq);
        }

        /// <summary>
        /// 人像分割接口
        /// 对于输入的一张图片（可正常解码，且长宽比适宜），**识别人体的轮廓范围，与背景进行分离，适用于拍照背景替换、照片合成、身体特效等场景。输入正常人像图片，返回分割后的二值结果图和分割类型（目前仅支持person）**
        /// </summary>
        /// <param name="image">二进制图像数据</param>
        /// <param name="options"> 可选参数对象，key: value都为string类型，可选的参数包括
        ///     <list type="bullet">
        ///           <item>  <c>type</c>: 可以通过设置type参数，自主设置返回哪些结果图，避免造成带宽的浪费<br>1）可选值说明：<br>labelmap - 二值图像，需二次处理方能查看分割效果<br>scoremap - 人像前景灰度图<br>foreground - 人像前景抠图，透明背景<br>2）type 参数值可以是可选值的组合，用逗号分隔；如果无此参数默认输出全部3类结果图 </item>
        ///     </list>
        /// </param>
        /// <return>JObject</return>
        ///
        public JObject BodySeg(byte[] image, Dictionary<string, object> options = null)
        {
            var aipReq = DefaultRequest(BODY_SEG);
            
            CheckNotNull(image, "image");
            aipReq.Bodys["image"] = System.Convert.ToBase64String(image);
            PreAction();

            if (options != null)
                foreach (var pair in options)
                    aipReq.Bodys[pair.Key] = pair.Value;
            return PostAction(aipReq);
        }

        /// <summary>
        /// 驾驶行为分析接口
        /// 对于输入的一张车载监控图片（可正常解码，且长宽比适宜），**识别图像中是否有人体（驾驶员），若检测到至少1个人体，则进一步识别属性行为，可识别使用手机、抽烟、未系安全带、双手离开方向盘、视线未朝前方5种典型行为姿态**。
        /// </summary>
        /// <param name="image">二进制图像数据</param>
        /// <param name="options"> 可选参数对象，key: value都为string类型，可选的参数包括
        ///     <list type="bullet">
        ///           <item>  <c>type</c>: smoke,cellphone,<br>not_buckling_up,<br>both_hands_leaving_wheel,<br>not_facing_front |识别的属性行为类别，英文逗号分隔，默认所有属性都识别；<br>smoke //吸烟，<br>cellphone //打手机 ，<br>not_buckling_up // 未系安全带，<br>both_hands_leaving_wheel // 双手离开方向盘，<br>not_facing_front // 视角未看前方 </item>
        ///     </list>
        /// </param>
        /// <return>JObject</return>
        ///
        public JObject DriverBehavior(byte[] image, Dictionary<string, object> options = null)
        {
            var aipReq = DefaultRequest(DRIVER_BEHAVIOR);
            
            CheckNotNull(image, "image");
            aipReq.Bodys["image"] = System.Convert.ToBase64String(image);
            PreAction();

            if (options != null)
                foreach (var pair in options)
                    aipReq.Bodys[pair.Key] = pair.Value;
            return PostAction(aipReq);
        }

        /// <summary>
        /// 人流量统计-动态版接口
        /// 统计图像中的人体个数和流动趋势，主要适用于**低空俯拍、出入口场景，以人体头肩为主要识别目标**
        /// </summary>
        /// <param name="image">二进制图像数据</param>
        /// <param name="dynamic">true：动态人流量统计，返回总人数、跟踪ID、区域进出人数；<br>false：静态人数统计，返回总人数</param>
        /// <param name="options"> 可选参数对象，key: value都为string类型，可选的参数包括
        ///     <list type="bullet">
        ///           <item>  <c>case_id</c>: 当dynamic为True时，必填；任务ID（通过case_id区分不同视频流，自拟，不同序列间不可重复即可） </item>
        ///           <item>  <c>case_init</c>: 当dynamic为True时，必填；每个case的初始化信号，为true时对该case下的跟踪算法进行初始化，为false时重载该case的跟踪状态。当为false且读取不到相应case的信息时，直接重新初始化 </item>
        ///           <item>  <c>show</c>: 否返回结果图（含统计值和跟踪框渲染），默认不返回，选true时返回渲染后的图片(base64)，其它无效值或为空则默认false </item>
        ///           <item>  <c>area</c>: 当dynamic为True时，必填；静态人数统计时，只统计区域内的人，缺省时为全图统计。<br>动态人流量统计时，进出区域的人流会被统计。<br>逗号分隔，如‘x1,y1,x2,y2,x3,y3...xn,yn'，按顺序依次给出每个顶点的xy坐标（默认尾点和首点相连），形成闭合多边形区域。<br>服务会做范围（顶点左边需在图像范围内）及个数校验（数组长度必须为偶数，且大于3个顶点）。只支持单个多边形区域，建议设置矩形框，即4个顶点。**坐标取值不能超过图像宽度和高度，比如1280的宽度，坐标值最小建议从1开始，最大到1279**。 </item>
        ///     </list>
        /// </param>
        /// <return>JObject</return>
        ///
        public JObject BodyTracking(byte[] image, string dynamic, Dictionary<string, object> options = null)
        {
            var aipReq = DefaultRequest(BODY_TRACKING);
            
            CheckNotNull(image, "image");
            aipReq.Bodys["image"] = System.Convert.ToBase64String(image);
            aipReq.Bodys["dynamic"] = dynamic;
            PreAction();

            if (options != null)
                foreach (var pair in options)
                    aipReq.Bodys[pair.Key] = pair.Value;
            return PostAction(aipReq);
        }


    }
}