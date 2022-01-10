using BatchRenamer.Core;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BatchRenamer.Controls
{
    internal class FileNameBuilderConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            FileNameBuilder builder = value as FileNameBuilder;
            if (builder == null) return "NULL";
            else return $"{builder.Name}{builder.Extension}";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
