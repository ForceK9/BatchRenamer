using BatchRenamer.ViewModel;
using System.Windows;
using CounterAppendingOperatorPlugin;
using System;
using System.Windows.Controls;
using BatchRenamer.Model;

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
            viewModel = new BatchRenamerViewModel(this);
            DataContext = viewModel;
            AddRenamingOperators();
        }

        private void AddRenamingOperators()
        {
            foreach (var opt in RenamingOperatorsLoader.Prototypes.Values)
                menuAddOpt.Items.Add(new MenuItem()
                {
                    Header = opt.Description,
                    Command = BatchRenamerViewModel.AddOperatorCommand,
                    CommandParameter = opt.MagicWord
                });
        }
    }
}
