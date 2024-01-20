using System;
using System.Globalization;
using MatoProductivity.Infrastructure.Helper;
using Microsoft.Maui.Controls;

namespace MatoProductivity.Converter
{
    public class TimeSpanRangeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (TimeSpan)value;
            var format = parameter==default ? "已过去|现在|还剩" : parameter.ToString();
            return CommonHelper.FormatTimeSpamString(time, format);
            //return time.ToString("yyyy年M月d日 HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime ts = DateTime.Parse(value as string);
            return ts;
        }

    }
}
