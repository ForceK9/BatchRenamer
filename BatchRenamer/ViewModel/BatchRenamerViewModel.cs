using BatchRenamer.Controls;
using BatchRenamer.Model;
using BatchRenamingCore;
using CounterAppendingOperatorPlugin;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BatchRenamer.ViewModel
{
    internal class BatchRenamerViewModel
    {
        // File list stuff
        private Window window;
        private ObservableCollection<FileListItem> _list;
        public ObservableCollection<FileListItem> ActiveList { get; private set; }
        public ObservableCollection<FileListItem> StorageList { get; private set; }
        public const string ActiveTag = "active";
        public const string StorageTag = "storage";
        // Note: Use RoutedCommand because its CanExecuteChanged event is needed for various Button's effects
        public static readonly RoutedCommand AddFilesCommand = new RoutedCommand();
        public static readonly RoutedCommand SortCommand = new RoutedCommand();
        public static readonly RoutedCommand MoveListCommand = new RoutedCommand();
        public static readonly RoutedCommand RemoveFilesCommand = new RoutedCommand();
        public static readonly RoutedCommand AddOperatorCommand = new RoutedCommand();
        public static readonly RoutedCommand RemoveOperatorCommand = new RoutedCommand();
        public static readonly RoutedCommand RenameCommand = new RoutedCommand();

        // renaming operation stuff
        public TrulyObservableCollection<FileRenamingOperator> Recipe { get; private set; }

        public BatchRenamerViewModel(Window window)
        {
            // BindingList is for Winforms and is incompatible with CollectionView
            this.window = window;
            _list = new ObservableCollection<FileListItem>();
            ActiveList = new ObservableCollection<FileListItem>();
            StorageList = new ObservableCollection<FileListItem>();
            Recipe = new TrulyObservableCollection<FileRenamingOperator>();
            
            // A delegate to refresh ActiveList's item indexes when it changes
            ActiveList.CollectionChanged += OnActiveListChanged;

            // a delegate to refresh ActiveList's preview when Recipe changes
            Recipe.CollectionChanged += OnRecipeChanged;

            // Add command bindings
            window.CommandBindings.Add(new CommandBinding(
                AddFilesCommand, AddFileCommand_Executed,
                AddFilesCommand_CanExecute));
            window.CommandBindings.Add(new CommandBinding(
                MoveListCommand, MoveListCommand_Executed,
                MoveListCommand_CanExecute));
            window.CommandBindings.Add(new CommandBinding(
                SortCommand, SortCommand_Executed,
                SortCommand_CanExecute));
            window.CommandBindings.Add(new CommandBinding(
                RemoveFilesCommand, RemoveFilesCommand_Executed,
                RemoveFilesCommand_CanExecute));
            window.CommandBindings.Add(new CommandBinding(
                AddOperatorCommand, AddOperatorCommand_Executed,
                AddOperatorCommand_CanExecute));
            window.CommandBindings.Add(new CommandBinding(
                RemoveOperatorCommand, RemoveOperatorCommand_Executed,
                RemoveOperatorCommand_CanExecute));
            window.CommandBindings.Add(new CommandBinding(
                RenameCommand, RenameCommand_Executed,
                RenameCommand_CanExecute));
        }

        /*public bool isFull()
        {
            return _list.Count == 100;
        }*/
        private ObservableCollection<FileListItem>? GetListByTag(string tag, bool getOppositeList = false)
        {
            switch (tag)
            {
                case ActiveTag:
                    return getOppositeList ? StorageList : ActiveList;
                case StorageTag:
                    return getOppositeList ? ActiveList : StorageList;
                default: 
                    return null;
            }
        }

        public void OnActiveListChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            // reassign all file index
            foreach (FileListItem item in ActiveList)
            {
                item.Index = ActiveList.IndexOf(item) + 1;
            }
            //Trace.WriteLine("From activeList");
            RefreshPreview();
        }

        private void OnRecipeChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            RefreshPreview();
        }

        private void RefreshPreview()
        {
            //Trace.WriteLine("refresh begin");
            List<FileNameBuilder> previews = new List<FileNameBuilder>();
            // reset preview
            foreach (FileListItem item in ActiveList)
            {
                item.Preview.Reset();
                previews.Add(item.Preview);
            }

            // apply operator to preview
            foreach(FileRenamingOperator opt in Recipe)
                opt.Rename(previews);

            // manually fire PropertyChanged event
            foreach (FileListItem item in ActiveList)
                item.PreviewChanged();
            //Trace.WriteLine("refresh end");
        }

        public virtual void AddFile(FileName fileName, string parameter)
        {
            FileListItem newItem = new FileListItem(fileName);
            newItem.IsSelected = true;
            ObservableCollection<FileListItem>? listToAdd = GetListByTag(parameter);
            ObservableCollection<FileListItem>? otherList = GetListByTag(parameter, true);
            if (listToAdd == null || otherList == null) return;
            if (!_list.Contains(newItem))
            {
                // the list doesn't have this item yet
                newItem.Current.UnderlyingFileChanged += OnFileChanged;
                _list.Add(newItem);
                listToAdd.Add(newItem);
            }
            else
            {
                // the underlying list already has this item
                FileListItem item = _list[_list.IndexOf(newItem)];
                item.IsSelected = true;
                if (!listToAdd.Contains(item))
                {
                    // the item is in the other list, so we need to switch
                    otherList.Remove(item);
                    listToAdd.Add(item);
                }
            }
        }

        private void OnFileChanged(FileName sender, FileName.FileChangedType type)
        {
            if (type == FileName.FileChangedType.Deleted)
            {
                //Trace.WriteLine(sender);
                FileListItem? itemToRemove = null;
                foreach (FileListItem item in _list)
                {
                    if (item.Current.Equals(sender))
                    {
                        itemToRemove = item;
                    }
                }

                // https://stackoverflow.com/questions/18331723/this-type-of-collectionview-does-not-support-changes-to-its-sourcecollection-fro
                App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
                {
                    RemoveFile(itemToRemove);
                });
            }
            //Trace.WriteLine("from outside");
            RefreshPreview();
        }

        public virtual void RemoveFile(FileListItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (_list.Contains(item))
            {
                // the list doesn't have this item yet
                item.Current.UnderlyingFileChanged -= OnFileChanged;
                _list.Remove(item);
                if (ActiveList.Contains(item)) ActiveList.Remove(item);
                if (StorageList.Contains(item)) StorageList.Remove(item);
            }
        }

        public virtual void UnselectAll()
        {
            foreach (FileListItem item in _list)
                item.IsSelected = false;
        }

        public virtual void ApplyRenamingOperator(FileRenamingOperator opt)
        {
            List<FileNameBuilder> builderList = new List<FileNameBuilder>();
            foreach (FileListItem item in ActiveList)
                builderList.Add(item.Preview);
            opt.Rename(builderList);

            // manually fire PropertyChanged event, a dirty solution
            // TODO: make a StringBuilder wrapper for FileNameBuilder.Name and FileNameBuilder.Extension
            foreach (FileListItem item in ActiveList)
            {
                item.PreviewChanged();
            }
        }
        public virtual void SaveAll()
        {
            /*List <FileListItem> listToRemove = new List<FileListItem>();
            foreach (FileListItem item in ActiveList)
            {
                try
                {
                    item.Current.Assign(item.Preview.ToString());
                    item.Preview.Save();
                }
                catch (Exception ex)
                {
                    listToRemove.Add(item);
                }
            }

            
            if (listToRemove.Count > 0)
            {
                MessageBox.Show("Some files no longer exist and therefore have been removed from list",
                    window.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                foreach (FileListItem item in listToRemove)
                    RemoveFile(item);
            }
            listToRemove.Clear();
            
            RefreshPreview();*/
            foreach (FileListItem item in ActiveList)
            {
                item.Save();
            }
            RefreshPreview();
        }


        // ----------------------Button commands-----------------------
        private void AddFilesCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddFileCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string parameter = (string)e.Parameter;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true; // allow selecting multiple files
            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                UnselectAll();
                foreach (string filename in ofd.FileNames)
                {
                    FileName fileName = new FileName(filename);
                    AddFile(fileName, parameter);
                }
            }

            e.Handled = true;
        }

        /*
        private bool AddFilesCommand_CanExecute(object arg)
        {
            return true;
        }

        private void AddFileCommand_Executed(object arg)
        {
            string parameter = (string)arg;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true; // allow selecting multiple files
            bool? result = ofd.ShowDialog();

            if (result == true)
            {
                foreach (string filename in ofd.FileNames)
                {
                    FileName fileName = new FileName(filename);
                    AddFile(fileName, parameter);
                }
            }
        }
        */

        private void SortCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = ActiveList.Count > 0 && !ActiveList.SequenceEqual(ActiveList.OrderBy(i => i.Current));
        }

        private void SortCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // TODO: maybe perform some sort of animation here
            // https://stackoverflow.com/questions/3973137/order-a-observablecollectiont-without-creating-a-new-one?noredirect=1&lq=1
            List<FileListItem> sortedList = ActiveList.OrderBy(i => i.Current.Name).ToList();
            ActiveList.Clear();
            foreach (FileListItem item in sortedList)
            {
                ActiveList.Add(item);
            }
            e.Handled = true;
        }

        private void MoveListCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            // https://stackoverflow.com/questions/335849/wpf-commandparameter-is-null-first-time-canexecute-is-called
            // https://stackoverflow.com/questions/48887369/canexecute-true-iff-an-item-in-a-listbox-is-selected
            string tag = (string)e.Parameter;

            ObservableCollection<FileListItem>? list = GetListByTag(tag);
            if (list == null) return;

            foreach (FileListItem item in list)
            {
                if (item.IsSelected)
                {
                    e.CanExecute = true;
                    return;
                }
            }
            e.CanExecute = false;
        }

        private void MoveListCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string tag = (string)e.Parameter;

            ObservableCollection<FileListItem>? listToRemove = GetListByTag(tag);
            ObservableCollection<FileListItem>? listToAdd = GetListByTag(tag, true);
            if (listToAdd == null) {
                return;
            }
           
            List<FileListItem> selected = new List<FileListItem>();
            foreach (FileListItem item in listToRemove)
            {
                if (item.IsSelected) selected.Add(item);
            }
            
            foreach (FileListItem item in selected)
            {
                listToRemove.Remove(item);
                listToAdd.Add(item);
                item.IsSelected = false;
            }
            e.Handled = true;
        }

        private void RemoveFilesCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            string tag = (string)e.Parameter;

            ObservableCollection<FileListItem>? list = GetListByTag(tag);
            if (list == null) return;

            foreach (FileListItem item in list)
            {
                if (item.IsSelected)
                {
                    e.CanExecute = true;
                    return;
                }
            }
            e.CanExecute = false;
        }

        private void RemoveFilesCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string tag = (string)e.Parameter;

            ObservableCollection<FileListItem>? listToRemove = GetListByTag(tag);
            if (listToRemove == null) return;

            List<FileListItem> selected = new List<FileListItem>();
            foreach (FileListItem item in listToRemove)
            {
                if (item.IsSelected) selected.Add(item);
            }

            foreach (FileListItem item in selected)
            {
                RemoveFile(item);
            }
            e.Handled = true;
        }

        private void AddOperatorCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void AddOperatorCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string magicWord = (string)e.Parameter;
            if (RenamingOperatorsLoader.Prototypes.ContainsKey(magicWord))
            {
                FileRenamingOperator opt = RenamingOperatorsLoader.Prototypes[magicWord].Clone();
                Recipe.Add(opt);
            }
        }

        private void RemoveOperatorCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            foreach (FileRenamingOperator item in Recipe)
            {
                if (item.IsSelected)
                {
                    e.CanExecute = true;
                    return;
                }
            }
            e.CanExecute = false;
        }

        private void RemoveOperatorCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            List<FileRenamingOperator> selected = new List<FileRenamingOperator>();
            foreach (FileRenamingOperator item in Recipe)
            {
                if (item.IsSelected) selected.Add(item);
            }

            foreach (FileRenamingOperator item in selected)
            {
                Recipe.Remove(item);
            }
            e.Handled = true;
        }

        private void RenameCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = Recipe.Count > 0 && ActiveList.Count > 0;
        }

        private void RenameCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (!ValidateRenameResult())
            {
                e.Handled = true;
                return;
            }

            MessageBoxResult result = MessageBox.Show("Confirm renaming? This action will be irreversible.",
                window.Title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                SaveAll();
                MessageBox.Show("Renaming completed.");
            }
            e.Handled = true;
        }

        private bool ValidateRenameResult()
        {
            string errorMsg = "The renaming recipe can't be applied because of the following error(s):\n\n";
            bool HasErrors = false;
            HashSet<string> names = new HashSet<string>();

            foreach (FileListItem file in ActiveList)
            {
                string preview = file.Preview.ToString();

                if (file.Preview.Name.Length == 0)
                {
                    // file name must not be empty
                    errorMsg += " - Filename after renaming must not be empty.\n";
                    HasErrors = true;
                }
                if (preview.Length > 256)
                {
                    // the filepath cannot exceed 256 characters
                    errorMsg += " - Some file paths after renaming will exceed Window's limit of 256 characters.\n";
                    HasErrors = true;
                }
                if (names.Contains(preview))
                {
                    // there are name collisions
                    errorMsg += " - Name collision: files residing in the same directory having identical name.\n";
                    HasErrors = true;
                }
                names.Add(preview);
            }

            if (!HasErrors) return true;
            else
            {
                errorMsg += "\nPlease modify the recipe and try again.";
                MessageBox.Show(errorMsg, window.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

        }
    }
}
