using BatchRenamingCore;
using System;
using System.Windows;

namespace FileRenamingOperators.TrimWhiteSpacePlugin
{
    public class TrimWhiteSpacePluginOperatorFactory : IRenamingOperatorFactory
    {
        public FileRenamingOperator Create()
        {
            return new TrimWhiteSpaceOperator();
        }

        public DataTemplate GetDataTemplate()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri($"pack://application:,,,/FileRenamingOperators;component/TrimWhiteSpacePlugin/TrimWhiteSpaceOperatorDataTemplate.xaml");
            DataTemplate? template = dict["renameOperator"] as DataTemplate;
            if (template == null) throw new NullReferenceException("Data template is null");
            return template;
        }
    }
}
