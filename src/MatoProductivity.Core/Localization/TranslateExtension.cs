using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using Abp.Domain.Services;
using Abp.Localization;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace MatoProductivity.Core.Localization
{
    // You exclude the 'Extension' suffix when using in Xaml markup
    [ContentProperty("Text")]
    public class TranslateExtension : DomainService, IMarkupExtension
    {
        public TranslateExtension()
        {
            LocalizationSourceName = MatoProductivityConsts.LocalizationSourceName;

        }
        public string Text { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            Console.WriteLine(CultureInfo.CurrentUICulture);
            if (Text == null)
                return "";
            var translation = L(Text);
            return translation;
        }



    }
}
