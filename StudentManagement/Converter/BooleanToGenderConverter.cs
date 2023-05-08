using System;
using System.Globalization;
using System.Windows.Data;

namespace StudentManagement.Converter
{
    public class BooleanToGenderConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool gender = System.Convert.ToBoolean(value);
            if (gender == true)
                return "Nam";
            else
                return "Nữ";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.ToString().ToUpper() == "Nam")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
