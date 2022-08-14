using System.Collections.Generic;

namespace SymLinker.DataEntities
{
    public class Settings : IDataEntity
    {

        public string TargetPath { get; set; } = string.Empty;
        public List<Symlink> Symlinks { get; set; } = new List<Symlink>();
    }
}
