using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace StudentManagement.Converter
{
    internal class DateTimeToDateConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime datetime = System.Convert.ToDateTime(value);
            string date =  datetime.Day.ToString() + "/" + datetime.Month.ToString() + "/" + datetime.Year.ToString();
            return date;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
