using SymLinker.Core;
using SymLinker.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SymLinker.Controller
{
    public class SymlinkController
    {
        private SymlinkManager symlinkManager;
        private Dictionary<string, string> aliases = new();
        public SymlinkController()
        {
            symlinkManager = new SymlinkManager();
            LoadAliases();
        }

        public bool IsSettingsLoaded()
        {
            return symlinkManager.SettingsLoaded;
        }

        public bool ManipulateSymlink(string symlinkName, bool enable)
        {
            if (enable)
            {
                symlinkManager.CreateSymlink(symlinkName);
            }
            else
            {
                symlinkManager.DeleteSymlink(symlinkName);
            }

            return symlinkManager.SymlinkExists(symlinkName);
        }

        public bool SymlinkExist(string symlinkName)
        {
            return symlinkManager.SymlinkExists(symlinkName);
        }

        public void AddSymlinks(List<Tuple<string, string, string>> rawSymlinks)
        {
            List<Symlink> symlinks = new List<Symlink>();

            rawSymlinks.ForEach(x => symlinks.Add(new Symlink() { Name = x.Item1, FolderName = x.Item2, Path = x.Item3, IsOn = false }));

            symlinkManager.AddSymlinks(symlinks);
        }
        public void AddTargetPath(string targetPath)
        {
            symlinkManager.SetTargetPath(targetPath);
        }
        public string GetTargetPath()
        {
            return symlinkManager.TargetPath;
        }

        public Tuple<string, string, string, bool>[] GetSymlinks()
        {
            List<Tuple<string, string, string, bool>> rawSymlinks = new();
            symlinkManager.GetSymlinks().ForEach(x => rawSymlinks.Add(new Tuple<string, string, string, bool>(x.Name, x.FolderName, x.Path, x.IsOn)));
            return rawSymlinks.ToArray();
        }

        public string GetAlias(string packageName)
        {
            aliases.TryGetValue(packageName, out string alias);
            if (alias != null) return alias;
            return "";

        }



        private void LoadAliases()
        {
            IDataLoader<Aliases> aliasLoader = new AliasLoader();
            var data = aliasLoader.LoadData();
            data.aliases.ForEach(x => aliases.TryAdd(x.packageName, x.alias));

        }
    }
}
