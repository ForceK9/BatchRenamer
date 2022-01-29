using BatchRenamingCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BatchRenamer.ViewModel
{
    internal class FileListItem : INotifyPropertyChanged
    {
        private int _index = -1;
        private bool _isSelected;
        public event PropertyChangedEventHandler? PropertyChanged;
        public int Index { get { return _index; }
            set 
            {
                _index = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Index"));
            }
        }
        public bool IsSelected { 
            get { return _isSelected; } 
            set { 
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            } 
        }
        public FileName Current { get; }
        public FileNameBuilder Preview { get; protected set; }
        public FileListItem(FileName fileName)
        {
            Current = fileName;
            Current.PropertyChanged += UpdatePreview;
            Preview = new FileNameBuilder(fileName.FullName);
        }

        private void UpdatePreview(object? sender, PropertyChangedEventArgs e)
        {
            if (!e.PropertyName.Equals("FullName")) return;
            Preview = new FileNameBuilder(Current.FullName);
            PreviewChanged();
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

        public void Save()
        {
            Current.Assign(Preview.ToString());
        }

    }
}
