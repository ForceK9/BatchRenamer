using BatchRenamingCore;
using System.Collections.Generic;
using System.Diagnostics;

namespace FileRenamingOperators.ReplaceCharactersPlugin
{
    public class ReplaceCharactersOperator : FileRenamingOperator
    {
        public override string MagicWord => "ReplaceCharacters";

        public override string Description => "Replace characters";

        protected string _from = "";
        public string From
        {
            get { return _from; }
            set { 
                _from = value;
                NotifyPropertyChanged("From");
            }
        }
        protected string _to = "";
        public string To
        {
            get { return _to; }
            set
            {
                _to = value;
                NotifyPropertyChanged("To");
            }
        }

        public override FileRenamingOperator Clone()
        {
            return new ReplaceCharactersOperator()
            {
                From = _from, To = _to
            };
        }

        public override void Rename(ICollection<FileNameBuilder> builders)
        {
            if (From.Length == 0) return;
            foreach (FileNameBuilder builder in builders)
            {
                builder.Name = builder.Name.Replace(From, To);
            }
        }
    }
}
