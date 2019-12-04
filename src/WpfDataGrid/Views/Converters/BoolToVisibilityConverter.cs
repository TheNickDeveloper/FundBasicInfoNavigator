using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FundBasicInfoNavigator.Views.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == true) 
                ? new GridLength(1, GridUnitType.Star) 
                : new GridLength(0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
