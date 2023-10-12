using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MatoProductivity.Converter
{
    public class IsValid2BoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = false;

            if (value != null)
            {
                if (value is string)
                {
                    result =
                    !string.IsNullOrEmpty(value as string);
                }
                else if (value is ICollection)
                {
                    result =
                        (value as ICollection).Count != 0;
                }
                else
                {
                    result = true;
                }
            }

            if (parameter!=null)
            {
                var isInvert = bool.Parse(parameter as string);
                if (isInvert)
                {
                    result=!result;
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
