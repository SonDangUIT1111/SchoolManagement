using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
namespace StudentManagement.Converter
{
        public class BooleanToGreenRedConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                bool state = System.Convert.ToBoolean(value);
                if (state == true)
                    return new SolidColorBrush(Colors.LimeGreen);
                else
                    return new SolidColorBrush(Colors.Red);
            }
            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return true;
            }
        }
}
