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
using portal_compare.Model;
using portal_compare.ViewModel;

namespace portal_compare.View
{
    /// <summary>
    /// Interaction logic for CredentialsView.xaml
    /// </summary>
    public partial class CredentialsView : UserControl
    {
        public CredentialsView()
        {
            InitializeComponent();
            DataContext = new CredentialsViewModel();
        }
    }
}
