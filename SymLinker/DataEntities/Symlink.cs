using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymLinker.DataEntities
{
    public class Symlink :IDataEntity
    {
        public string Path { get; set; } = String.Empty;
        public string FolderName { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public bool IsOn { get; set; } = false;


    }
}
