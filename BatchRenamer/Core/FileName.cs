using System;
using System.ComponentModel;

namespace BatchRenamer.Core
{
    public class FileName : INotifyPropertyChanged, IComparable<FileName>
    {
        public string Path { get; protected set; }
        public String Name { get; protected set;  }
        public String Extension { get; protected set; }
        public string FullName
        {
            get { return $"{Path}{Name}{Extension}"; }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public FileName(string fullName)
        {
            SilentAssign(fullName);
        }

        // assigning without raising PropertyChanged. Use this carefully
        protected virtual void SilentAssign(string fullName)
        {
            int extIdx = fullName.LastIndexOf('.');
            int fileIdx = fullName.LastIndexOf('\\') + 1;
            Extension = fullName.Substring(extIdx, fullName.Length - extIdx);
            Name = fullName.Substring(fileIdx, extIdx - fileIdx);
            Path = fullName.Substring(0, fileIdx);
        }
        public virtual void Assign(string fullName)
        {
            System.IO.File.Move(FullName, fullName);

            SilentAssign(fullName);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Extension"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FullName"));
        }
        public override bool Equals(Object? obj)
        {
            FileName? fileName = obj as FileName;
            if (fileName == null) return false;
            return FullName.Equals(fileName.FullName);
        }
        public virtual int CompareTo(FileName? fileName)
        {
            if (fileName == null) return -1;
            int res = Name.CompareTo(fileName.Name);
            if (res == 0) return FullName.CompareTo(fileName.FullName);
            else return res;
        }

        //public static implicit operator FileNameBase(string fullName) => new FileNameBase(fullName);
    }
}
