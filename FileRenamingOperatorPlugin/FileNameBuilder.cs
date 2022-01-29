using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamingCore
{
    public class FileNameBuilder
    {
        protected string _savedName;
        public string Path { get; }
        public String Name { get; set; }
        public String Extension { get; set; }
        public override string ToString()
        {
            return $"{Path}{Name}{Extension}";
        }

        public FileNameBuilder(string fullName)
        {
            _savedName = fullName;

            FileName fileName = new FileName(_savedName);

            Extension = fileName.Extension;
            Name = fileName.Name;
            Path = fileName.Path;
        }
        public void Reset()
        {
            FileName fileName = new FileName(_savedName);

            Extension = fileName.Extension;
            Name = fileName.Name;   
        }
        public void Save()
        {
            _savedName = ToString();
        }

    }
}
