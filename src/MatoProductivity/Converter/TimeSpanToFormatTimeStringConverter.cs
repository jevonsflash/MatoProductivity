using System;
using System.Globalization;
using MatoProductivity.Infrastructure.Helper;
using Microsoft.Maui.Controls;

namespace MatoProductivity.Converter
{
    public class TimeSpanToFormatStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (TimeSpan)value;
            return CommonHelper.FormatTimeSpamString(time);
            //return time.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime ts = DateTime.Parse(value as string);
            return ts;
        }

    }
}
