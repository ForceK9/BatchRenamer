using BatchRenamer.Logic;
using BatchRenamer.Controls;
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
using System.Windows.Shapes;

namespace BatchRenamer
{
    /// <summary>
    /// Interaction logic for BatchRenamer.xaml
    /// </summary>
    public partial class MainWindowV2 : Window
    {
        FileNameListManager activeList = new FileNameListManager();
        FileNameListManager storageList = new FileNameListManager();
        public MainWindowV2()
        {
            InitializeComponent();
            activeList.ProvideItemSource(boxActive);
            storageList.ProvideItemSource(boxStorage);
        }
    }
}
