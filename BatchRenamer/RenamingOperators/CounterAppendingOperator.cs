using System.Collections.Generic;

namespace BatchRenamer.RenamingOperators
{
    public class CounterAppendingOperator : IFileRenamingOperator
    {
        // TODO: modify to allow specifying start, steps and number of digits
        public void Rename(ICollection<FileName> fileNames)
        {
            int current = 0;
            int numOfDigits = (fileNames.Count-1).ToString().Length;
            foreach(FileName file in fileNames)
            {
                file.NameBuilder.Append('_')
                                .Append(current.ToString().PadLeft(numOfDigits, '0'));
                current++;
            }
        }
    }
}
