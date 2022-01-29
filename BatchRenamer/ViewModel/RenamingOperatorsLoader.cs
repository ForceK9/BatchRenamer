using BatchRenamingCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BatchRenamer.Model
{
    internal class RenamingOperatorsLoader
    {
        public static Dictionary<string, FileRenamingOperator> Prototypes 
            = new Dictionary<string, FileRenamingOperator>();
        public static Dictionary<string, DataTemplate> DataTemplates
            = new Dictionary<string, DataTemplate>();

        static RenamingOperatorsLoader()
        {
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            //Trace.WriteLine(exeFolder);
            var dlls = new DirectoryInfo(exeFolder).GetFiles("*.dll");

            foreach (var dll in dlls)
            {
                var assembly = Assembly.LoadFile(dll.FullName);
                var types = assembly.GetTypes();

                foreach (Type type in types)
                {
                    if (type.IsClass)
                    {
                        if (type.GetInterfaces().Contains(typeof(IRenamingOperatorFactory)))
                        {
                            try
                            {
                                var factory = Activator.CreateInstance(type) as IRenamingOperatorFactory;
                                FileRenamingOperator item = factory.Create();
                                Prototypes.Add(item.MagicWord, item);
                                DataTemplates.Add(item.MagicWord, factory.GetDataTemplate());
                            }
                            catch (Exception ex)
                            {
                                Trace.WriteLine(ex.ToString());
                            }
                        }
                    }
                }
            }
        }

    }
}
