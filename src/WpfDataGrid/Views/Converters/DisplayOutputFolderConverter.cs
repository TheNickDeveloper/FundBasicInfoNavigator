using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FundBasicInfoNavigator.Views.Converters
{
    public class DisplayOutputFolderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == true)
                ? new GridLength(0)
                : new GridLength(1, GridUnitType.Star);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
