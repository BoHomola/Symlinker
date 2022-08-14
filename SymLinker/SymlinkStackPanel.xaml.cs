using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SymLinker
{
    /// <summary>
    /// Interaction logic for SymlinkStackPanel.xaml
    /// </summary>
    public partial class SymlinkStackPanel : UserControl
    {
        public Action<int> DeleteStackPanel;
        public string FolderPathName { get; set; }
        public string SymlinkName { get; set; }
        public string FolderName { get; set; }
        public int ID { get; set; }

        public SymlinkStackPanel()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DeleteStackPanel?.Invoke(ID);
        }
    }
}
