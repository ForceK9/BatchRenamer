using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BatchRenamingCore
{
    public abstract class FileRenamingOperator : INotifyPropertyChanged
    {
        protected bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
            }
        }
        public abstract string MagicWord { get; }
        public abstract string Description { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public abstract void Rename(ICollection<FileNameBuilder> builders);
        public abstract FileRenamingOperator Clone();
        public void NotifyPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
