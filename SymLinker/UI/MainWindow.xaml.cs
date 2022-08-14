using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MessageBox = System.Windows.MessageBox;
using System.Diagnostics;
using SymLinker.Controller;
using System.IO;
using Path = System.IO.Path;
using System.Threading;
using SymLinker.DataEntities;
using SymLinker.Core;

namespace SymLinker
{
    public partial class MainWindow : MetroWindow
    {
        private SymlinkController symlinkController { get; set; } = new();

        private bool settingsEnabled;
        private bool SettingsEnabled
        {
            get { return settingsEnabled; }
            set
            {
                if (settingsEnabled == value) return;

                settingsEnabled = value;
                if (value)
                {
                    LoadSettings();
                    SettingsPanel.Visibility = Visibility.Visible;
                    MainPanel.Visibility = Visibility.Hidden;
                }
                else
                {
                    LoadMainPanel();
                    SettingsPanel.Visibility = Visibility.Hidden;
                    MainPanel.Visibility = Visibility.Visible;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            LoadMainPanel();

        }

        #region Main Panel

        private List<SymlinkToggle> Toggles { get; set; } = new();

        private void LoadMainPanel()
        {

            Toggles.ForEach(x => x.Toggled -= OnSymlinkSwitch);
            Toggles.Clear();
            MainSymlinksStackPanel.Children.Clear();


            foreach (var rawSymlink in symlinkController.GetSymlinks())
            {
                SymlinkToggle symlinkToggle = new SymlinkToggle();
                symlinkToggle.SymlinkName = rawSymlink.Item1;
                symlinkToggle.Toggled += OnSymlinkSwitch;
                symlinkToggle.IsToggled = symlinkController.SymlinkExist(rawSymlink.Item1);
                Toggles.Add(symlinkToggle);
                MainSymlinksStackPanel.Children.Add(symlinkToggle);
            }
        }

        private void OnSymlinkSwitch(string symlinkName, bool toggled)
        {
            symlinkController.ManipulateSymlink(symlinkName, toggled);
        }

        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsEnabled = true;
        }

        #endregion


        #region Settings Panel
        private List<SymlinkStackPanel> settingsSymlinkStackPanels { get; set; } = new();

        private void RemoveSymlinkStackPanel(int id)
        {
            var symlink = settingsSymlinkStackPanels.Find(x => x.ID == id);
            if (symlink != null)
            {
                SettingsSymlinksStackPanel.Children.Remove(symlink);
                symlink.DeleteStackPanel -= RemoveSymlinkStackPanel;
                settingsSymlinkStackPanels.Remove(symlink);
            }
        }

        private void SelectDestinationFolder_Click(object sender, RoutedEventArgs e)
        {

            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {

                    //var subDirs = Directory.GetDirectories(dialog.SelectedPath);
                    DestinatinFolderPathTextBox.Text = dialog.SelectedPath;
                }

            }
        }

        private void AddSymlinkFolder(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {

                    string[] subDirs = Directory.GetDirectories(dialog.SelectedPath);

                    foreach (string directory in subDirs)
                    {
                        string folderName = Path.GetFileName(directory);
                        SymlinkStackPanel symlinkStackPanel = new SymlinkStackPanel();
                        symlinkStackPanel.FolderPathName = directory;
                        string alias = symlinkController.GetAlias(folderName);
                        symlinkStackPanel.SymlinkName = string.IsNullOrEmpty(alias) ? folderName : alias;
                        symlinkStackPanel.FolderName = folderName;
                        symlinkStackPanel.DeleteStackPanel += RemoveSymlinkStackPanel;
                        symlinkStackPanel.ID = settingsSymlinkStackPanels.Count > 0 ? settingsSymlinkStackPanels[settingsSymlinkStackPanels.Count - 1].ID + 1 : 0;
                        settingsSymlinkStackPanels.Add(symlinkStackPanel);
                        SettingsSymlinksStackPanel.Children.Add(symlinkStackPanel);
                    }
                }

            }
        }

        private void LoadSettings()
        {
            settingsSymlinkStackPanels.ForEach(x => x.DeleteStackPanel -= RemoveSymlinkStackPanel);
            settingsSymlinkStackPanels.Clear();
            SettingsSymlinksStackPanel.Children.Clear();


            DestinatinFolderPathTextBox.Text = symlinkController.GetTargetPath();
            foreach (var rawSymlink in symlinkController.GetSymlinks())
            {
                SymlinkStackPanel symlinkStackPanel = new SymlinkStackPanel();
                symlinkStackPanel.SymlinkName = rawSymlink.Item1;
                symlinkStackPanel.FolderName = rawSymlink.Item2;
                symlinkStackPanel.FolderPathName = rawSymlink.Item3;
                symlinkStackPanel.DeleteStackPanel += RemoveSymlinkStackPanel;
                symlinkStackPanel.ID = settingsSymlinkStackPanels.Count > 0 ? settingsSymlinkStackPanels[settingsSymlinkStackPanels.Count - 1].ID + 1 : 0;
                settingsSymlinkStackPanels.Add(symlinkStackPanel);
                SettingsSymlinksStackPanel.Children.Add(symlinkStackPanel);
            }
        }

        private void SaveSettings(object sender, RoutedEventArgs e)
        {
            if (DestinatinFolderPathTextBox.Text == String.Empty) MessageBox.Show("Destination folder is empty!");

            symlinkController.AddTargetPath(DestinatinFolderPathTextBox.Text);
            List<Tuple<string, string, string>> rawSymlinks = new List<Tuple<string, string, string>>();
            settingsSymlinkStackPanels.ForEach(x => rawSymlinks.Add(new Tuple<string, string, string>(x.SymlinkName, x.FolderName, x.FolderPathName)));
            symlinkController.AddSymlinks(rawSymlinks);
            SettingsEnabled = false;
        }
        #endregion
    }
}
