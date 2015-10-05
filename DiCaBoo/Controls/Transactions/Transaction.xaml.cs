using DataLayer;
using Microsoft.SqlServer.Types;
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
using System.Windows.Shapes;

namespace DiCaBoo.Controls.Transactions
{
    /// <summary>
    /// Interaction logic for Transaction.xaml
    /// </summary>
    public partial class Transaction : Window
    {
        public Transaction()
        {
            InitializeComponent();
            ShortAccountNode assets = Accounts.GetShortTree("/1/");
            ShortAccountNode incomes = Accounts.GetShortTree("/2/");

            tvCredit.Items.Add(assets);
            tvCredit.Items.Add(incomes);


            tvDebit.Items.Add(assets);
            ShortAccountNode expences = Accounts.GetShortTree("/3/");
            tvDebit.Items.Add(expences);
        }

       

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
           // MessageBox.Show(((ShortAccountNode)tvTest.SelectedItem).RootAccount.AccountId);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }


        private void creditHeader_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            creditPopup.Placement = System.Windows.Controls.Primitives.PlacementMode.RelativePoint;
            creditPopup.VerticalOffset = creditHeader.Height;
            creditPopup.StaysOpen = true;
            creditPopup.Height = tvCredit.Height;
            creditPopup.Width = creditHeader.Width;
            creditPopup.IsOpen = true;
        }


        private void tvCredit_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var trv = sender as TreeView;
            ShortAccountNode trvItem = (ShortAccountNode)trv.SelectedItem;

            creditHeader.Text = trvItem.RootAccount.AccountName;
            creditPopup.IsOpen = false;
        }


        private void cbDebit_DropDownClosed(object sender, EventArgs e)
        {
            if (tvDebit.SelectedItem != null)
            {
                ShortAccountNode selectedNode = ((ShortAccountNode)tvDebit.SelectedItem);
                cbDebitItem.Content = selectedNode.RootAccount.AccountName;
            }
            cbDebit.SelectedIndex=0;
            cbDebit.Visibility = Visibility.Visible;
        }

        private void cbDebit_DropDownOpened(object sender, EventArgs e)
        {
            cbDebitItem.Visibility = Visibility.Collapsed;
        }
    }

}
