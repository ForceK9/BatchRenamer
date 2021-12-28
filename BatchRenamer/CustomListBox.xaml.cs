using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BatchRenamer
{
    /// <summary>
    /// Interaction logic for CustomListBox.xaml
    /// </summary>
    public partial class CustomListBox : UserControl
    {
        // Dependency proprety to allow specifying custom Listbox in xaml
        public static DependencyProperty ListBoxProperty = 
            DependencyProperty.Register("ListBox", typeof(ListBox), typeof(CustomListBox));
        public static DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(Object), typeof(CustomListBox));
        public static DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(StackPanel), typeof(CustomListBox));
        public ListBox ListBoxContent
        {
            get { return (ListBox)GetValue(ListBoxProperty); }
            set { SetValue(ListBoxProperty, value); }
        }
        public Object Title
        {
            get { return (Object)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }
        public StackPanel Header
        {
            get { return (StackPanel)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
        public CustomListBox()
        {
            InitializeComponent();
        }
    }
}
