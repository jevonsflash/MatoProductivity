using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace MatoProductivity.Converter
{
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var time = (DateTime)value;
            string parseFomate = parameter == null ? "F" : parameter as string;
            return time.ToString(parseFomate);
            //return time.ToString("yyyy年MM月dd日 HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime ts = DateTime.Parse(value as string);
            return ts;
        }

    }
}
