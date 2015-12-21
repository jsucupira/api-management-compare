using System.Windows.Controls;
using portal_compare.ViewModel;

namespace portal_compare.View
{
    /// <summary>
    ///     Interaction logic for GroupsView.xaml
    /// </summary>
    public partial class GroupsView : UserControl
    {
        public GroupsView()
        {
            InitializeComponent();
            GroupsViewModel groupViewModel = new GroupsViewModel();
            DataContext = groupViewModel;
        }
    }
}