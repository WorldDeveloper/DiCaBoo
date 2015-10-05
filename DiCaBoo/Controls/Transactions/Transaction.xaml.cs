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
            dpDate.SelectedDate = DateTime.Now;

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
            try
            {
                if (!dpDate.SelectedDate.HasValue)
                    throw new Exception("Select a date");

                DateTime date = dpDate.SelectedDate.Value;
                date=date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);

                if (tvCredit.SelectedItem == null)
                    throw new Exception("Select a credit account (From acc)");

                string credit = ((ShortAccountNode)tvCredit.SelectedItem).RootAccount.AccountId;

                if (tvDebit.SelectedItem == null)
                    throw new Exception("Select a debit account (To acc)");

                string debit = ((ShortAccountNode)tvDebit.SelectedItem).RootAccount.AccountId;

                decimal amount = 0M;
                if (!decimal.TryParse(txtAmount.Text, out amount) ||amount<=0)
                    throw new Exception("Enter correct amount");

                if (Operations.AddTransaction(date, credit, debit, amount, txtNote.Text)!=1)
                {
                    MessageBox.Show("Can't save transaction.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Incorrect input", MessageBoxButton.OK, MessageBoxImage.Information);
            }


            // MessageBox.Show(((ShortAccountNode)tvTest.SelectedItem).RootAccount.AccountId);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
           
        }


        private void cbDebit_DropDownClosed(object sender, EventArgs e)
        {
            if (tvDebit.SelectedItem != null)
            {
                ShortAccountNode selectedNode = ((ShortAccountNode)tvDebit.SelectedItem);
                cbDebitItem.Content = selectedNode.RootAccount.AccountName;
            }
            cbDebit.SelectedIndex = 0;
            cbDebit.Visibility = Visibility.Visible;
        }

        private void cbDebit_DropDownOpened(object sender, EventArgs e)
        {
            cbDebitItem.Visibility = Visibility.Collapsed;
        }


        private void cbCredit_DropDownClosed(object sender, EventArgs e)
        {
            if (tvCredit.SelectedItem != null)
            {
                ShortAccountNode selectedNode = ((ShortAccountNode)tvCredit.SelectedItem);
                cbCreditItem.Content = selectedNode.RootAccount.AccountName;
            }
            cbCredit.SelectedIndex=0;
            cbCredit.Visibility = Visibility.Visible;
        }

        private void cbCredit_DropDownOpened(object sender, EventArgs e)
        {
            cbCreditItem.Visibility = Visibility.Collapsed;
        }
    }

}
