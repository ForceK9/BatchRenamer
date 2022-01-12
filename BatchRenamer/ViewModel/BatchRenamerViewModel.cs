using BatchRenamer.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BatchRenamer.ViewModel
{
    internal class BatchRenamerViewModel
    {
        private ObservableCollection<FileListItem> _list;
        public ObservableCollection<FileListItem> ActiveList { get; private set; }
        public ObservableCollection<FileListItem> StorageList { get; private set; }
        public const string ActiveTag = "active";
        public const string StorageTag = "storage";
        // MoveList is a RoutedCommand because its CanExecuteChanged event is need for the Button's style changne
        public static readonly RoutedCommand AddFilesCommand = new RoutedCommand();
        public static readonly RoutedCommand SortCommand = new RoutedCommand();
        public static readonly RoutedCommand MoveListCommand = new RoutedCommand();
        public static readonly RoutedCommand RemoveFilesCommand = new RoutedCommand();
        public BatchRenamerViewModel(Window window)
        {
            // BindingList is for Winforms and is incompatible with CollectionView
            _list = new ObservableCollection<FileListItem>();
            ActiveList = new ObservableCollection<FileListItem>();
            StorageList = new ObservableCollection<FileListItem>();
            
            // A delegate to refresh ActiveList's item indexes when it changes
            ActiveList.CollectionChanged += OnActiveListChanged;

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

        public virtual void UnselectAll()
        {
            foreach (FileListItem item in _list)
                item.IsSelected = false;
        }

        public virtual void ApplyRenamingOperator(IFileRenamingOperator opt)
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
            foreach (FileListItem item in ActiveList)
            {
                item.Current.Assign(item.Preview.ToString());
                item.Preview.Save();
            }
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
    }
}
