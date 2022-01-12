using System;
using System.Windows;
using System.Windows.Controls;

namespace BatchRenamer.Controls
{
    public class CustomListBox : ListBox
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(Object), typeof(CustomListBox));
        public static readonly DependencyProperty ToolBarProperty =
            DependencyProperty.Register("ToolBar", typeof(StackPanel), typeof(CustomListBox));
        public Object Header
        {
            get { return (Object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public StackPanel ToolBar
        {
            get { return (StackPanel)GetValue(ToolBarProperty); }
            set { SetValue(ToolBarProperty, value); }
        }
    }
}
