using GongSolutions.Wpf.DragDrop;
using GongSolutions.Wpf.DragDrop.Utilities;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace BatchRenamer.Controls
{
    // Attach some actions after the DefaultDropHandler is finish
    internal class CustomDropHandler : DefaultDropHandler
    {
        public override void Drop(IDropInfo dropInfo)
        {
            base.Drop(dropInfo);
            // refresh the view, mostly for the row index of ActiveFileList to be recalculated
            var source = dropInfo.DragInfo.SourceCollection.TryGetList();
            var target = dropInfo.TargetCollection.TryGetList();
            ICollectionView cv = CollectionViewSource.GetDefaultView(source);
            cv.Refresh();
            if (!source.IsSameObservableCollection(target))
            {
                cv = CollectionViewSource.GetDefaultView(target);
                cv.Refresh();
            }
        }
    }
}
