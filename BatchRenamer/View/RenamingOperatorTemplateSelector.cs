using BatchRenamer.Model;
using BatchRenamer.ViewModel;
using BatchRenamingCore;
using System.Windows;
using System.Windows.Controls;

namespace BatchRenamer
{
    internal class RenamingOperatorTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FileRenamingOperator opt = (FileRenamingOperator)item;
            return RenamingOperatorsLoader.DataTemplates[opt.MagicWord];
        }
    }
}
