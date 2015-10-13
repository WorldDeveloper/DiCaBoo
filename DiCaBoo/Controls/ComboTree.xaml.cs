using DataLayer;
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

namespace DiCaBoo.Controls
{
    /// <summary>
    /// Interaction logic for ComboTree.xaml
    /// </summary>
    public partial class ComboTree : UserControl
    {
        public event RoutedEventHandler SelectedItemChanged;
        public ComboTree()
        {
            InitializeComponent();
            cbComboTreeItem.Content = new Account(null, null);
        }

        private void cbComboTree_DropDownClosed(object sender, EventArgs e)
        {
            if (tvNestedTree.SelectedItem != null)
            {
                ShortAccountNode selectedNode = ((ShortAccountNode)tvNestedTree.SelectedItem);
                cbComboTreeItem.Content = selectedNode.RootAccount;
            }
           cbComboTree.SelectedIndex = 0;
           cbComboTree.Visibility = Visibility.Visible;
        }

        private void cbComboTree_DropDownOpened(object sender, EventArgs e)
        {
            cbComboTreeItem.Visibility = Visibility.Collapsed;
        }


        private void cbComboTreeItem_Selected(object sender, RoutedEventArgs e)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(sender, e);
        }
    }
}
