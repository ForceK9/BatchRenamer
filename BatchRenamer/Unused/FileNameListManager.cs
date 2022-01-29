using BatchRenamingCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace BatchRenamer.ViewModel
{
    internal class FileNameListManager
    {
        internal class FileListItem : INotifyPropertyChanged
        {
            public FileName Current { get; }
            public FileNameBuilder Preview { get; }
            public event PropertyChangedEventHandler? PropertyChanged;
            public FileListItem (FileName fileName)
            {
                Current = fileName; 
                Preview = new FileNameBuilder(fileName.FullName);
            }
            public override bool Equals(Object? obj)
            {
                FileListItem? item = obj as FileListItem;
                if (item == null) return false;
                return Current.Equals(item.Current);
            }
            public void PreviewChanged()
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Preview"));
            }
        }
        protected ObservableCollection<FileListItem> _list;
        public FileNameListManager()
        { 
            // BindingList is for Winforms and is incompatible with CollectionView
            _list = new ObservableCollection<FileListItem>();
        }
        public virtual void ProvideItemSource(ItemsControl itemsControl)
        {
            itemsControl.ItemsSource = _list;
        }

        /*public bool isFull()
        {
            return _list.Count == 100;
        }*/
        
        public virtual void Add(FileName fileName)
        {
            FileListItem newItem = new FileListItem(fileName);
            if (!_list.Contains(newItem))
            {
                _list.Add(newItem);
            }
        }
        public virtual void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }
        public virtual void ApplyRenamingOperator(FileRenamingOperator opt)
        {
            List<FileNameBuilder> builderList = new List<FileNameBuilder>();
            foreach (FileListItem item in _list)
                builderList.Add(item.Preview);
            opt.Rename(builderList);

            // manually fire PropertyChanged event, a dirty solution
            // TODO: make a StringBuilder wrapper for FileNameBuilder.Name and FileNameBuilder.Extension
            foreach (FileListItem item in _list)
            {
                item.PreviewChanged();
            }
        }
        public virtual void SaveAll()
        {
            foreach (FileListItem item in _list)
            {
                item.Current.Assign(item.Preview.ToString());
                item.Preview.Save();
            }
        }
    }
}
