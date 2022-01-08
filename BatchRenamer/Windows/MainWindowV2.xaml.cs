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
using Microsoft.Win32;
using BatchRenamer.Core;

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

        private void AddBtnActive_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true; // allow selecting multiple files
            bool? result = ofd.ShowDialog();
            if (result == true)
            {
                foreach (string filename in ofd.FileNames)
                {
                    FileName fileName = new FileName(filename);
                    activeList.Add(fileName);
                }
            }
        }

        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("This will append a counter to all the files selected. Do you wish to proceed?",
                "Confirm renaming", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                CounterAppendingOperator opt = new CounterAppendingOperator();
                activeList.ApplyRenamingOperator(opt);
                activeList.SaveAll();
                MessageBox.Show("Renaming completed.");
            }
        }
    }
}
