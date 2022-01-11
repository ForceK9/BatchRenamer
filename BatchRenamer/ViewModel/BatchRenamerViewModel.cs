using BatchRenamer.Core;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace BatchRenamer.ViewModel
{
    internal class BatchRenamerViewModel
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
        private ObservableCollection<FileListItem> _list;
        public ObservableCollection<FileListItem> ActiveList { get; private set; }
        public ObservableCollection<FileListItem> StorageList { get; private set; }
        public ICommand AddFilesCommand { get; private set; }
        public BatchRenamerViewModel()
        {
            // BindingList is for Winforms and is incompatible with CollectionView
            _list = new ObservableCollection<FileListItem>();
            ActiveList = new ObservableCollection<FileListItem>();
            StorageList = new ObservableCollection<FileListItem>();
            AddFilesCommand = new DelegateCommand<object>(AddFileCommand_Executed,
                AddFilesCommand_CanExecute);

        }

        /*public bool isFull()
        {
            return _list.Count == 100;
        }*/
        
        public virtual void AddFile(FileName fileName, string parameter)
        {
            FileListItem newItem = new FileListItem(fileName);
            if (_list.Contains(newItem)) return;
            _list.Add(newItem);
            switch (parameter)
            {
                case "active":
                    ActiveList.Add(newItem);
                    break;
                case "storage":
                    StorageList.Add(newItem);
                    break;
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

        private bool AddFilesCommand_CanExecute(object arg)
        {
            //MessageBox.Show(arg.ToString());
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
    }
}
