using BatchRenamer.ViewModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using BatchRenamer.Core;

namespace BatchRenamer
{
    /// <summary>
    /// Interaction logic for BatchRenamer.xaml
    /// </summary>
    public partial class MainWindowV3 : Window
    {
        BatchRenamerViewModel viewModel;
        public MainWindowV3()
        {
            InitializeComponent();
            viewModel = new BatchRenamerViewModel();
            DataContext = viewModel;
        }
        private void RenameButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("This will append a counter to all the files selected. Do you wish to proceed?",
                "Confirm renaming", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                CounterAppendingOperator opt = new CounterAppendingOperator();
                viewModel.ApplyRenamingOperator(opt);
                viewModel.SaveAll();
                MessageBox.Show("Renaming completed.");
            }
        }
    }
}
