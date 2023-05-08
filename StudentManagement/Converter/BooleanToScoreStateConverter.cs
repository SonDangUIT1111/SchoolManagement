using System;
using System.Globalization;
using System.Windows.Data;

namespace StudentManagement.Converter
{
    public class BooleanToScoreStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool state = System.Convert.ToBoolean(value);
            if (state == true)
                return "Đã chốt";
            else
                return "Chưa chốt";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
