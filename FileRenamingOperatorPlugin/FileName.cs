using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace BatchRenamingCore
{
    public class FileName : INotifyPropertyChanged, IComparable<FileName>
    {
        public enum FileChangedType
        {
            Deleted,
            Renamed
        }

        protected FileSystemWatcher watcher;
        public string Path { get; protected set; }
        public String Name { get; protected set;  }
        public String Extension { get; protected set; }
        public string FullName
        {
            get { return $"{Path}{Name}{Extension}"; }
        }
        public delegate void UnderlyingFileChangedHandler(FileName sender, FileChangedType type);
        public event UnderlyingFileChangedHandler UnderlyingFileChanged;
        public event PropertyChangedEventHandler? PropertyChanged;

        public FileName(string fullName)
        {
            SilentAssign(fullName);
            watcher = new FileSystemWatcher(Path);
            watcher.Deleted += Watcher_Deleted;
            watcher.Renamed += Watcher_Renamed;
            watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (e.OldName.Equals($"{Name}{Extension}"))
            {
                //Trace.WriteLine("-----" + e.FullPath);
                SilentAssign(e.FullPath);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Extension"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FullName"));
                UnderlyingFileChanged?.Invoke(this, FileChangedType.Renamed);
            }
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            //Trace.WriteLine(e.Name);
            if (e.Name.Equals($"{Name}{Extension}")) 
                UnderlyingFileChanged?.Invoke(this, FileChangedType.Deleted);
        }

        // assigning without raising PropertyChanged. Use this carefully
        protected virtual void SilentAssign(string fullName)
        {
            int extIdx = fullName.LastIndexOf('.');
            if (extIdx == -1) extIdx = fullName.Length;
            int fileIdx = fullName.LastIndexOf('\\') + 1;
            Extension = fullName.Substring(extIdx, fullName.Length - extIdx);
            Name = fullName.Substring(fileIdx, extIdx - fileIdx);
            Path = fullName.Substring(0, fileIdx);
        }
        public virtual bool Assign(string fullName)
        {
            if (!File.Exists(FullName)) return false;
            try
            {
                File.Move(FullName, fullName);
                SilentAssign(fullName);

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Extension"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FullName"));
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
