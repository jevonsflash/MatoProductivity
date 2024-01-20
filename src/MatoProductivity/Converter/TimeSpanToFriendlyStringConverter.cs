using System;
using System.Globalization;
using MatoProductivity.Infrastructure.Helper;
using Microsoft.Maui.Controls;

namespace MatoProductivity.Converter
{
    public class TimeSpanToFriendlyStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (TimeSpan)value;
            var format = parameter==default ? "{0}天{1}时{2}分{3}秒" : parameter.ToString();
            return CommonHelper.FormatTimeSpamString2(time, format);
            //return time.ToString("yyyy年M月d日 HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime ts = DateTime.Parse(value as string);
            return ts;
        }

    }
}
