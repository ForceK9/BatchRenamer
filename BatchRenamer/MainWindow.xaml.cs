﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using BatchRenamer.RenamingOperators;
using Microsoft.Win32;

namespace BatchRenamer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        FileNameListManager _fileManager = new FileNameListManager();
        public MainWindow()
        {
            InitializeComponent();
            _fileManager.ProvideItemSource(listBoxOfFiles);
            //listBoxOfFiles.ItemsSource = _fileManager._list;
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true; // allow selecting multiple files
            bool? result = ofd.ShowDialog();
            if (result == true)
            {
                foreach (string filename in ofd.FileNames)
                {
                    FileName fileName = new FileName(filename);
                    _fileManager.Add(fileName);
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
                _fileManager.ApplyRenamingOperator(opt);
                _fileManager.SaveAll();
                MessageBox.Show("Renaming completed.");
            }
        }
    }
}
