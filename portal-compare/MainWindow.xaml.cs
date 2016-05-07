using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;
using portal_compare.Model;

namespace portal_compare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.TabControl = TabControl;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ... Get TabControl reference.
            TabControl item = sender as TabControl;
            // ... Set Title to selected tab header.
            if (item != null)
            {
                if (App.CurrentTab != item.SelectedIndex)
                {
                    App.CurrentTab = item.SelectedIndex;
                    switch (item.SelectedIndex)
                    {
                        case 1:
                            App.CompareGroupCommand.Execute();
                            break;
                        case 2:
                            App.CompareProductCommand.Execute();
                            break;
                        case 3:
                            App.CompareApiCommand.Execute();
                            break;
                        case 4:
                            App.LoadApis.Execute();
                            break;
                    }
                }
            }
        }
    }
}
