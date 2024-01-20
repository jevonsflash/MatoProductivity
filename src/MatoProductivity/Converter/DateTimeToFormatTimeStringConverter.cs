using System;
using System.Globalization;
using MatoProductivity.Infrastructure.Helper;
using Microsoft.Maui.Controls;

namespace MatoProductivity.Converter
{
    public class DateTimeToFormatTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value==null)
            {
                return null;
            }
            var time = (DateTime)value;
            var format = "yy年M月d日 HH:mm:ss";
            if (DateTime.Now.Year==time.Year)
            {
                format =  "M月d日 HH:mm:ss";
            }
            string parseFomate = parameter == null ? format : parameter as string;
            return CommonHelper.FormatDateTimeString(time, parseFomate);
            //return time.ToString("yyyy年M月d日 HH:mm:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime ts = DateTime.Parse(value as string);
            return ts;
        }

    }
}
