using System;
using System.Windows;
using System.Windows.Controls;

namespace BatchRenamer.Controls
{
    public class CustomListBox : Control
    {
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(Object), typeof(CustomListBox));
        public static readonly DependencyProperty ToolBarProperty =
            DependencyProperty.Register("ToolBar", typeof(StackPanel), typeof(CustomListBox));
        public static readonly DependencyProperty ListBoxProperty =
            DependencyProperty.Register("ListBox", typeof(ListBox), typeof(CustomListBox));
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
        public ListBox ListBox
        {
            get { return (ListBox)GetValue(ToolBarProperty); }
            set { SetValue(ToolBarProperty, value); }
        }
    }
}
