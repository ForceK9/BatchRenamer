using BatchRenamer.Core;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
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
        public ICommand AddFilesCommand { get; private set; }
        public ICommand SortCommand { get; private set; }
        public ICommand MoveListCommand { get; private set; }
        public ICommand RemoveFileCommand { get; private set; }
        public BatchRenamerViewModel()
        {
            // BindingList is for Winforms and is incompatible with CollectionView
            _list = new ObservableCollection<FileListItem>();
            ActiveList = new ObservableCollection<FileListItem>();
            StorageList = new ObservableCollection<FileListItem>();
            AddFilesCommand = new DelegateCommand<object>(AddFileCommand_Executed,
                AddFilesCommand_CanExecute);
            SortCommand = new DelegateCommand<object>(SortCommand_Executed,
                SortCommand_CanExecute);
            ActiveList.CollectionChanged += OnActiveListChanged;
        }

        /*public bool isFull()
        {
            return _list.Count == 100;
        }*/

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
            if (!_list.Contains(newItem))
            {
                // the list doesn't have this item yet
                _list.Add(newItem);
                switch (parameter)
                {
                    case ActiveTag:
                        ActiveList.Add(newItem);
                        break;
                    case StorageTag:
                        StorageList.Add(newItem);
                        break;
                }
            }
            else
            {
                // the list already has this item
                FileListItem item = _list[_list.IndexOf(newItem)];
                if (parameter.Equals(ActiveTag) && !ActiveList.Contains(item))
                {
                    // the item is in Storage, move it to ActiveList
                    StorageList.Remove(item);
                    ActiveList.Add(item);
                }
                else if (parameter.Equals(StorageTag) && !StorageList.Contains(item))
                {
                    // the item is in Storage, move it to ActiveList
                    ActiveList.Remove(item);
                    StorageList.Add(item);
                }
            }
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

        private bool SortCommand_CanExecute(object arg)
        {
            return true;
        }

        private void SortCommand_Executed(object arg)
        {
            // TODO: maybe perform some sort of animation here
            List<FileListItem> sortedList = ActiveList.OrderBy(i => i.Current.Name).ToList();
            ActiveList.Clear();
            foreach (FileListItem item in sortedList)
            {
                ActiveList.Add(item);
            }
        }

        private bool MoveListCommand_CanExecute(object arg)
        {
            return true;
        }

        private void MoveListCommand_Executed(object arg)
        {
            List<FileListItem> sortedList = ActiveList.OrderBy(i => i.Current.Name).ToList();
            ActiveList.Clear();
            foreach (FileListItem item in sortedList)
            {
                ActiveList.Add(item);
            }
        }
    }
}
