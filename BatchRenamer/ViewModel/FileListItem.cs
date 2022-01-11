using BatchRenamer.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer.ViewModel
{
    internal class FileListItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _index = -1;
        public int Index { get { return _index; }
            set 
            {
                _index = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Index"));
            }
        }
        public FileName Current { get; }
        public FileNameBuilder Preview { get; }
        public FileListItem(FileName fileName)
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
}
