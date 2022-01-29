using BatchRenamingCore;
using System.Collections.Generic;

namespace FileRenamingOperators.TrimWhiteSpacePlugin
{
    public class TrimWhiteSpaceOperator : FileRenamingOperator
    {
        public override string MagicWord => "TrimWhitespace";

        public override string Description => "Trim whitespace";

        public override FileRenamingOperator Clone()
        {
            return new TrimWhiteSpaceOperator();
        }

        public override void Rename(ICollection<FileNameBuilder> builders)
        {
            foreach(var builder in builders)
            {
                string name = builder.Name;
                builder.Name = name.Trim();
            }
        }
    }
}
