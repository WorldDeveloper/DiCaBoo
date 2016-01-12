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
            ShortAccountNode expences = Accounts.GetShortTree("/3/");

            ctCredit.tvNestedTree.Items.Clear();
            ctCredit.tvNestedTree.Items.Add(assets);
            ctCredit.tvNestedTree.Items.Add(incomes);

            ctDebit.tvNestedTree.Items.Clear();
            ctDebit.tvNestedTree.Items.Add(assets);
            ctDebit.tvNestedTree.Items.Add(expences);
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

                ctCredit.cbComboTreeItem.Content = operation.Credit;
                ctCredit.cbComboTree.SelectedIndex = 0;

                ctDebit.cbComboTreeItem.Content = operation.Debit;
                ctDebit.cbComboTree.SelectedIndex = 0;

                dpDate.SelectedDate = operation.Date;
                txtAmount.Text = operation.Amount.ToString("N2");
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


                Account credit = (Account)ctCredit.cbComboTreeItem.Content;
                if (string.IsNullOrWhiteSpace(credit.AccountId))
                    throw new Exception("Select a credit account (From acc)");

                Account debit = (Account)ctDebit.cbComboTreeItem.Content;
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


                if (result != 1)
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

        private void EditAccounts_Click(object sender, RoutedEventArgs e)
        {
            AccountsWindow accountsWindow = new AccountsWindow();
            accountsWindow.ShowDialog();

            InitAccounts();
        }
    }
}
