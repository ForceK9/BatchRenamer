using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BatchRenamer.RenamingOperators
{
    public interface IRenamingOperator
    {
        public void Rename(ICollection<FileName> fileNames);
    }
}
