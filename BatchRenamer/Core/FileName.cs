using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer
{
    public class FileName : INotifyPropertyChanged
    {
        private string _savedName;
        public string Path { get; }
        public StringBuilder NameBuilder { get; }
        public StringBuilder ExtensionBuilder { get; }
        public String Name { get { return NameBuilder.ToString(); } }
        public String Extension { get { return ExtensionBuilder.ToString(); } }
        public string FullName
        {
            get { return $"{Path}{Name}{Extension}"; }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public FileName(string fullName)
        {
            _savedName = fullName;
            NameBuilder = new StringBuilder();
            ExtensionBuilder = new StringBuilder();

            int extIdx = fullName.LastIndexOf('.');
            int fileIdx = fullName.LastIndexOf('\\') + 1;
            ExtensionBuilder.Append(fullName.Substring(extIdx, fullName.Length - extIdx));
            NameBuilder.Append(fullName.Substring(fileIdx, extIdx - fileIdx));
            Path = fullName.Substring(0, fileIdx);
        }
        public override bool Equals(Object? obj)
        {
            FileName? fileName = obj as FileName;
            if (fileName == null) return false;
            if (!this._savedName.Equals(fileName._savedName))
                return false;
            return true;
        }

        public void Reset()
        {
            NameBuilder.Clear();
            ExtensionBuilder.Clear();

            int extIdx = _savedName.LastIndexOf('.');
            int fileIdx = _savedName.LastIndexOf('\\') + 1;
            ExtensionBuilder.Append(_savedName.Substring(extIdx, _savedName.Length - extIdx));
            NameBuilder.Append(_savedName.Substring(fileIdx, extIdx - fileIdx));
        }
        public void Save()
        {
            System.IO.File.Move(_savedName, FullName);
            _savedName = FullName;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Extension"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FullName"));
        }
    }
}
