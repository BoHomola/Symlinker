using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymLinker.DataEntities
{
    public class Aliases : IDataEntity
    {
        public List<Alias> aliases { get; set; } = new();
    }
    public class Alias : IDataEntity
    {
        public string packageName { get; set; } = "";
        public string alias { get; set; } = "";
    }
}
