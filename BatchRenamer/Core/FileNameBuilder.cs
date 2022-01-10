using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer.Core
{
    public class FileNameBuilder
    {
        protected string _savedName;
        public string Path { get; }
        public StringBuilder NameBuilder { get; }
        public StringBuilder ExtensionBuilder { get; }
        public String Name { get { return NameBuilder.ToString(); } }
        public String Extension { get { return ExtensionBuilder.ToString(); } }
        public override string ToString()
        {
            return $"{Path}{Name}{Extension}";
        }

        public FileNameBuilder(string fullName)
        {
            _savedName = fullName;
            NameBuilder = new StringBuilder();
            ExtensionBuilder = new StringBuilder();

            FileName fileName = new FileName(_savedName);

            ExtensionBuilder.Append(fileName.Extension);
            NameBuilder.Append(fileName.Name);
            Path = fileName.Path;
        }
        public void Clear()
        {
            NameBuilder.Clear();
            ExtensionBuilder.Clear();

            FileName fileName = new FileName(_savedName);

            ExtensionBuilder.Append(fileName.Extension);
            NameBuilder.Append(fileName.Name);   
        }
        public void Save()
        {
            _savedName = ToString();
        }

    }
}
