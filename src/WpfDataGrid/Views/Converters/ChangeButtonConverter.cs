using System;
using System.Globalization;
using System.Windows.Data;

namespace FundBasicInfoNavigator.Views.Converters
{
    public class ChangeButtonConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value == true)
                ? "Search & Export" 
                : "Search";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
