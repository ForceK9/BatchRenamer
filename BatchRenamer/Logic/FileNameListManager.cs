using BatchRenamer.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BatchRenamer.Logic
{
    internal class FileNameListManager
    {
        //public static int SIZE_LIMIT => 100;
        // FileNameListItem is a proxy for FileName, holding extra information
        public class FileNameListItem : FileName
        {
            public bool isHidden;
            public FileNameListItem(string fullName) : base(fullName)
            {
                this.isHidden = false;
            }
        }

        private BindingList<FileName> _list;
        public FileNameListManager()
        {
            _list = new BindingList<FileName>();
        }
        public void ProvideItemSource(ItemsControl itemsControl)
        {
            itemsControl.ItemsSource = _list;
        }

        /*public bool isFull()
        {
            return _list.Count == 100;
        }*/
        
        public void Add(FileName newItem)
        {
            if (!_list.Contains(newItem))
            {
                _list.Add(newItem);
            }
        }
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public void ApplyRenamingOperator(IFileRenamingOperator opt)
        {
            opt.Rename(_list);
        }
        public void SaveAll()
        {
            foreach (FileName item in _list)
                item.Save();
        }
    }
}
