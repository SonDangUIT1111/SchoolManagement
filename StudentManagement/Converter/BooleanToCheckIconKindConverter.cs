using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace StudentManagement.Converter
{
    internal class BooleanToCheckIconKindConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool state = System.Convert.ToBoolean(value);
            if (state == true)
                return MaterialDesignThemes.Wpf.PackIconKind.CheckCircle;
            else
                return MaterialDesignThemes.Wpf.PackIconKind.AlertCircle;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }
    }
}
