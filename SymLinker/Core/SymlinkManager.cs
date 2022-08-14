using SymLinker.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;
using System.IO;

namespace SymLinker.Core
{
    public class SymlinkManager
    {

        private Dictionary<string,Symlink> Symlinks { get; set; }
        public string TargetPath { get; private set; }


        private IDataLoader<Settings> settingsLoader;
        public bool SettingsLoaded { get; private set; }
        public SymlinkManager()
        {
            settingsLoader = new SettingsLoader();
            Symlinks = new Dictionary<string, Symlink>();
            Settings settings = settingsLoader.LoadData();

            settings.Symlinks.ForEach(x => Symlinks.Add(x.Name, x));
            TargetPath = settings.TargetPath;

            if (!IsSettingsEmpty(settings))
            {
                SettingsLoaded = true;
            }
        }
        public void SetTargetPath(string targetPath)
        {
            TargetPath = targetPath;
            SaveSettings();
        }
        public void AddSymlinks(List<Symlink> symlinks)
        {
            Symlinks.Clear();
            symlinks.ForEach(symlink => Symlinks.TryAdd(symlink.Name, symlink));
            SaveSettings();
        }

        public void CreateSymlink(string name)
        {
            Symlink symlink;
            Symlinks.TryGetValue(name, out symlink);
            if (symlink == null) return;

            string strCmdText;
            strCmdText = $"/C mklink /D {this.TargetPath}\\{symlink.FolderName} {symlink.Path}";
            ProcessStartInfo cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = strCmdText;
            Process? cmd = Process.Start(cmdsi);
            cmd?.WaitForExit();
        }
        
        public void DeleteSymlink(string name)
        {
            Symlink symlink;
            Symlinks.TryGetValue(name, out symlink);
            if (symlink == null) return;

            string strCmdText;
            strCmdText = $"/C rmdir {TargetPath}\\{symlink.FolderName}";
            ProcessStartInfo cmdsi = new ProcessStartInfo("cmd.exe");
            cmdsi.Arguments = strCmdText;
            Process? cmd = Process.Start(cmdsi);
            cmd?.WaitForExit();
        }

        public List<Symlink> GetSymlinks()
        {
            return Symlinks.Values.ToList();
        }

        public bool SymlinkExists(string symlinkName)
        {
            Symlink symlink;
            Symlinks.TryGetValue(symlinkName, out symlink);
            if (symlink == null) return false;

            if (Directory.Exists($"{TargetPath}\\{symlink.FolderName}"))
                return true;
            return false;
        }

        private void SaveSettings()
        {
            settingsLoader.SaveData(new Settings() { TargetPath = TargetPath, Symlinks = Symlinks.Values.ToList() });
        }


        private static bool IsSettingsEmpty(Settings settings)
        {
            return string.IsNullOrEmpty(settings.TargetPath) || settings.Symlinks.Count == 0;
        }

    }
}
