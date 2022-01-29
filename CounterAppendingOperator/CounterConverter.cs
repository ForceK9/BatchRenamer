using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CounterAppendingOperatorPlugin
{
    public class CounterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((Int64)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string num = (string)value;
            Int64 max = 999999999;
            if (num.Length >= 10) return max;
            else return Int64.Parse(num);
        }
    }
}
