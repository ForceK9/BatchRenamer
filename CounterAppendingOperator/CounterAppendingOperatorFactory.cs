using BatchRenamingCore;
using System;
using System.Windows;
using Xceed.Wpf.Toolkit;

namespace CounterAppendingOperatorPlugin
{
    public class CounterAppendingOperatorFactory : IRenamingOperatorFactory
    {
        public FileRenamingOperator Create()
        {
            return new CounterAppendingOperator() { Start = 1, Step = 1};
        }

        public DataTemplate GetDataTemplate()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri($"pack://application:,,,/CounterAppendingOperatorPlugin;component/CounterAppendingOperatorDataTemplate.xaml");
            DataTemplate? template = dict["counterAppender"] as DataTemplate;
            if (template == null) throw new NullReferenceException("Data template is null");
            return template;
        }
    }
}
