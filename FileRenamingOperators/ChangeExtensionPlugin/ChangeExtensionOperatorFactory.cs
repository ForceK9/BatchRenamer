using BatchRenamingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileRenamingOperators.ChangeExtensionPlugin
{
    public class ChangeExtensionOperatorFactory : IRenamingOperatorFactory
    {
        public FileRenamingOperator Create()
        {
            return new ChangeExtensionOperator() { TargetExtension = "txt"};
        }

        public DataTemplate GetDataTemplate()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri($"pack://application:,,,/FileRenamingOperators;component/ChangeExtensionPlugin/ChangeExtensionOperatorDataTemplate.xaml");
            DataTemplate? template = dict["changeExtension"] as DataTemplate;
            if (template == null) throw new NullReferenceException("Data template is null");
            return template;
        }
    }
}
