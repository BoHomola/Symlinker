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
    /// Interaction logic for SymlinkToggle.xaml
    /// </summary>
    public partial class SymlinkToggle : UserControl
    {
        public Action<string, bool> Toggled;

        public string SymlinkName{get;set;}
        public bool IsToggled { get; set; }
        public SymlinkToggle()
        {
            InitializeComponent();
            this.DataContext = this;
        }


        private void OnToggle(object sender, RoutedEventArgs e)
        {
            Toggled?.Invoke(SymlinkName, toggle.IsOn);
        }
    }
}
