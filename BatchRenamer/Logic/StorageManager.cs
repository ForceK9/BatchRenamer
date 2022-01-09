using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace BatchRenamer.Logic
{
    // The Storage is an alphabetically-sorted list
    internal sealed class StorageManager : FileNameListManager
    {
        public override void ProvideItemSource(ItemsControl itemsControl)
        {
            ICollectionView myDataView = CollectionViewSource.GetDefaultView(_list);
            myDataView.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            itemsControl.ItemsSource = myDataView;
        }
    }
}
