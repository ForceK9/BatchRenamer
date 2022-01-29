using BatchRenamingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileRenamingOperators.ReplaceCharactersPlugin
{
    public class ReplaceCharactersOperatorFactory : IRenamingOperatorFactory
    {
        public FileRenamingOperator Create()
        {
            return new ReplaceCharactersOperator();
        }

        public DataTemplate GetDataTemplate()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri($"pack://application:,,,/FileRenamingOperators;component/ReplaceCharactersPlugin/ReplaceCharactersOperatorDataTemplate.xaml");
            DataTemplate? template = dict["renameOperatorTemplate"] as DataTemplate;
            if (template == null) throw new NullReferenceException("Data template is null");
            return template;
        }
    }
}
