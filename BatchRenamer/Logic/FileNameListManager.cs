using BatchRenamer.Core;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace BatchRenamer.Logic
{
    internal class FileNameListManager
    {
        protected ObservableCollection<FileName> _list;
        public FileNameListManager()
        { 
            // BindingList is for Winforms and is incompatible with CollectionView
            _list = new ObservableCollection<FileName>();
        }
        public virtual void ProvideItemSource(ItemsControl itemsControl)
        {
            itemsControl.ItemsSource = _list;
        }

        /*public bool isFull()
        {
            return _list.Count == 100;
        }*/
        
        public virtual void Add(FileName newItem)
        {
            if (!_list.Contains(newItem))
            {
                _list.Add(newItem);
            }
        }
        public virtual void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public virtual void ApplyRenamingOperator(IFileRenamingOperator opt)
        {
            opt.Rename(_list);
        }
        public virtual void SaveAll()
        {
            foreach (FileName item in _list)
                item.Save();
        }
    }
}
