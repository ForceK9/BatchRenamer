using BatchRenamingCore;
using System;
using System.Windows;

namespace FileRenamingOperators.LowerUpperCasePlugin
{
    public class LowerUpperCasePluginOperatorFactory : IRenamingOperatorFactory
    {
        public FileRenamingOperator Create()
        {
            return new LowerUpperCaseOperator();
        }

        public DataTemplate GetDataTemplate()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri($"pack://application:,,,/FileRenamingOperators;component/LowerUpperCasePlugin/LowerUpperCaseOperatorDataTemplate.xaml");
            DataTemplate? template = dict["renameOperator"] as DataTemplate;
            if (template == null) throw new NullReferenceException("Data template is null");
            return template;
        }
    }
}
