using BatchRenamingCore;
using System;
using System.Collections.Generic;

namespace FileRenamingOperators.ChangeExtensionPlugin
{
    public class ChangeExtensionOperator : FileRenamingOperator
    {
        public override string MagicWord => "ChangeExtension";

        public override string Description => "Change Extension";

        protected string _targetExtension = "txt";
        public string TargetExtension { 
            get { return _targetExtension; }
            set { _targetExtension = value; 
                NotifyPropertyChanged("TargetExtension");
            }
        }

        public override FileRenamingOperator Clone()
        {
            return new ChangeExtensionOperator()
            {
                TargetExtension = this.TargetExtension
            };
        }

        public override void Rename(ICollection<FileNameBuilder> builders)
        {
            if (TargetExtension.Length == 0)
            {
                foreach (var builder in builders)
                {
                    builder.Extension = "";
                }
            }
            else
            {
                foreach (var builder in builders)
                {
                    builder.Extension = $".{TargetExtension}";
                }
            }
        }
    }
}
