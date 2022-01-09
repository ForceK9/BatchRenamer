using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace BatchRenamer.Controls
{
    //https://stackoverflow.com/questions/660528/how-to-display-row-numbers-in-a-listview/662232#662232
    public class IndexConverter : IValueConverter
    {
        public object Convert(object value, Type TargetType, object parameter, CultureInfo culture)
        {
            ListBoxItem item = (ListBoxItem)value;
            ListBox? listBox = ItemsControl.ItemsControlFromItemContainer(item) as ListBox;
            int index;
            if (listBox == null) index = 0;
            else index = listBox.ItemContainerGenerator.IndexFromContainer(item) + 1;
            return index.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
