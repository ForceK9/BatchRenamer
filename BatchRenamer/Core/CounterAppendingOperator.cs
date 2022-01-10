using System.Collections.Generic;

namespace BatchRenamer.Core
{
    public class CounterAppendingOperator : IFileRenamingOperator
    {
        // TODO: modify to allow specifying start, steps and number of digits
        public void Rename(ICollection<FileNameBuilder> builders)
        {
            int current = 0;
            int numOfDigits = (builders.Count-1).ToString().Length;
            foreach(FileNameBuilder file in builders)
            {
                file.NameBuilder.Append('_')
                                .Append(current.ToString().PadLeft(numOfDigits, '0'));
                current++;
            }
        }
    }
}
