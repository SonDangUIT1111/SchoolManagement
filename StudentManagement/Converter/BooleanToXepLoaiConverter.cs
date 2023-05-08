using System;
using System.Globalization;
using System.Windows.Data;
namespace StudentManagement.Converter
{
    public class BooleanToXepLoaiConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool state = System.Convert.ToBoolean(value);
            if (state == true)
                return "Đạt";
            else
                return "Chưa đạt";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
