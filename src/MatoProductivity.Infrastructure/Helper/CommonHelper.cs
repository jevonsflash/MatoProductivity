using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatoProductivity.Infrastructure.Helper
{
    public class CommonHelper
    {
        public static int[] GetRandomArry(int minval, int maxval)
        {

            int[] arr = new int[maxval - minval + 1];
            int i;
            //初始化数组
            for (i = 0; i <= maxval - minval; i++)
            {
                arr[i] = i + minval;
            }
            //随机数
            Random r = new Random();
            for (int j = maxval - minval; j >= 1; j--)
            {
                int address = r.Next(0, j);
                int tmp = arr[address];
                arr[address] = arr[j];
                arr[j] = tmp;
            }
            //输出
            foreach (int k in arr)
            {
                Debug.WriteLine(k + " ");
            }
            return arr;
        }

        public static string FormatTimeSpamString(TimeSpan timeSpan, string format = "已过去|现在|还剩")
        {
            var parseString = format.Split('|');
            if (timeSpan > TimeSpan.Zero)
            {
                return parseString[0];
            }
            else if (timeSpan==TimeSpan.Zero)
            {
                return parseString[1];
            }
            else
            {
                return parseString[2];
            }
        }
        public static string FormatTimeSpamString2(TimeSpan timeSpan, string format = "{0}天{1}小时{2}分{3}秒")
        {
            var affix = "已过";
            if (timeSpan < TimeSpan.Zero)
            {
                timeSpan=TimeSpan.Zero-timeSpan;
                affix="还剩";
            }
            string friendlyString = string.Format(format,
                timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            return affix+friendlyString;
        }
        public static string FormatDateTimeString(DateTime dateTime, string format = "yyyy年M月d日 HH:mm:ss")
        {
            DateTime now = DateTime.Now;
            double diff = (now - dateTime).TotalSeconds;

            if (diff < 30)
            {
                return "刚刚";
            }
            else if (diff < 3600)
            {
                // less 1 hour
                return "几分钟前";
            }
            else if (diff < 3600 * 24)
            {
                return "今天";
            }
            else if (diff < 3600 * 24 * 7)
            {
                return "本周";
            }

            else if (diff < 3600 * 24 * 30)
            {
                return "本月";
            }
            else if (diff < 3600 * 24 * 365)
            {
                return "今年";
            }

            else if (diff < 3600 * 24 * 365*2)
            {
                return "一年前";
            }
            else
            {
                return "很久以前";

            }
        }




        public static string FormatDateTimeString2(DateTime dateTime, string format = "yyyy年M月d日 HH:mm:ss")
        {
            DateTime now = DateTime.Now;
            double diff = (now - dateTime).TotalSeconds;

            if (diff < 30)
            {
                return "刚刚";
            }
            else if (diff < 3600)
            {
                // less 1 hour
                return Math.Ceiling(diff / 60) + "分钟前";
            }
            else if (diff < 3600 * 24)
            {
                return Math.Ceiling(diff / 3600) + "小时前";
            }
            else if (diff < 3600 * 24 * 2)
            {
                return "1天前";
            }
            else
            {
                return dateTime.ToString(format);
            }
        }
    }
}
