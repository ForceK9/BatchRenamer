using BatchRenamingCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileRenamingOperators.AddPrefixSuffixPlugin
{
    public class AddPrefixSuffixOperator : FileRenamingOperator
    {
        public override string MagicWord => "AddPrefixSuffix";

        public override string Description => "Add prefix / suffix";

        protected string _phrase = "";
        public string Phrase
        {
            get => _phrase;
            set { _phrase = value;
                NotifyPropertyChanged("Phrase");
            }
        }

        protected bool _isPrefix = true;
        public bool IsPrefix
        {
            get => _isPrefix;
            set
            {
                _isPrefix = value;
                NotifyPropertyChanged("IsPrefix");
            }
        }

        public override FileRenamingOperator Clone()
        {
            return new AddPrefixSuffixOperator();
        }

        public override void Rename(ICollection<FileNameBuilder> builders)
        {
            if (IsPrefix)
            {
                foreach (FileNameBuilder builder in builders)
                {
                    builder.Name = Phrase + builder.Name;
                }
            }
            else
            {
                foreach (FileNameBuilder builder in builders)
                {
                    builder.Name += Phrase;
                }
            }
        }
    }
}
