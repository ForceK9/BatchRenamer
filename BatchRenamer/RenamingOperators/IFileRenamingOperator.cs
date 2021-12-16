using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer.RenamingOperators
{
    public interface IFileRenamingOperator
    {
        public void Rename(ICollection<FileName> fileNames);
    }
}
