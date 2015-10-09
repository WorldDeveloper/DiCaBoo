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
        private int? mTransactionId;
        public Transaction()
        {
            InitializeComponent();
            dpDate.SelectedDate = DateTime.Now;

            InitAccounts();
        }

        private void InitAccounts()
        {
            ShortAccountNode assets = Accounts.GetShortTree("/1/");
            ShortAccountNode incomes = Accounts.GetShortTree("/2/");

            tvCredit.Items.Clear();
            tvCredit.Items.Add(assets);
            tvCredit.Items.Add(incomes);

            tvDebit.Items.Clear();
            tvDebit.Items.Add(assets);
            ShortAccountNode expences = Accounts.GetShortTree("/3/");
            tvDebit.Items.Add(expences);

            cbCreditItem.Content = new Account(null, null);
            cbDebitItem.Content = new Account(null, null);
        }

        public Transaction(int transactionId)
        {
            InitializeComponent();

            try
            {
                Operation operation = DataLayer.Operations.GetTransaction(transactionId);
                if (operation == null)
                {
                    throw new Exception("Can't edit transaction");
                }

                mTransactionId = operation.ID;

                InitAccounts();
                cbCreditItem.Content = operation.Credit;
                cbCredit.SelectedIndex = 0;
                cbDebitItem.Content = operation.Debit;
                cbDebit.SelectedIndex = 0;
                dpDate.SelectedDate = operation.Date;
                txtAmount.Text = operation.Amount.ToString();
                txtNote.Text = operation.Note;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Incorrect input", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
        }



        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!dpDate.SelectedDate.HasValue)
                    throw new Exception("Select a date");

                DateTime date = dpDate.SelectedDate.Value;
                date = date.AddHours(DateTime.Now.Hour).AddMinutes(DateTime.Now.Minute).AddSeconds(DateTime.Now.Second);


                Account credit = (Account)cbCreditItem.Content;
                if (string.IsNullOrWhiteSpace(credit.AccountId))
                    throw new Exception("Select a credit account (From acc)");

                Account debit = (Account)cbDebitItem.Content;
                if (string.IsNullOrWhiteSpace(debit.AccountId))
                    throw new Exception("Select a debit account (To acc)");

                decimal amount = 0M;
                if (!decimal.TryParse(txtAmount.Text, out amount) || amount <= 0)
                    throw new Exception("Enter correct amount");

                int result;
                if (mTransactionId == null)
                    result = DataLayer.Operations.AddTransaction(date, credit.AccountId, debit.AccountId, amount, txtNote.Text);
                else
                    result = DataLayer.Operations.UpdateTransaction((int)mTransactionId, date, credit.AccountId, debit.AccountId, amount, txtNote.Text);


                if(result!=1)
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
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }


        private void cbDebit_DropDownClosed(object sender, EventArgs e)
        {
            if (tvDebit.SelectedItem != null)
            {
                ShortAccountNode selectedNode = ((ShortAccountNode)tvDebit.SelectedItem);
                cbDebitItem.Content = selectedNode.RootAccount;
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
                cbCreditItem.Content = selectedNode.RootAccount;
            }
            cbCredit.SelectedIndex = 0;
            cbCredit.Visibility = Visibility.Visible;
        }

        private void cbCredit_DropDownOpened(object sender, EventArgs e)
        {
            cbCreditItem.Visibility = Visibility.Collapsed;
        }

        private void EditAccounts_Click(object sender, RoutedEventArgs e)
        {
            AccountsWindow accountsWindow = new AccountsWindow();
            accountsWindow.ShowDialog();

            InitAccounts();
        }
    }

}
