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

namespace DiCaBoo
{
    /// <summary>
    /// Interaction logic for AccountsWindow.xaml
    /// </summary>
    public partial class AccountsWindow : Window
    {
        public AccountsWindow()
        {
            InitializeComponent();
            UpdateAccountsTree();

        }

        private void UpdateAccountsTree()
        {
            List<Account> accounts = Accounts.GetList(SqlHierarchyId.GetRoot().ToString());
            tvAccounts.Items.Clear();
            foreach (Account acc in accounts)
            {
                tvAccounts.Items.Add(ChildAccount(acc));
            }
        }

        private TreeViewItem ChildAccount(Account account)
        {
            TreeViewItem newNode = new TreeViewItem() { Header = account.AccountName, Tag=account.AccountId };
            List<Account> accounts = Accounts.GetList(account.AccountId);
            foreach (Account acc in accounts)
            {
                newNode.Items.Add(ChildAccount(acc)); 
            }

            return newNode;
        }

        private void btnAddAccount_Click(object sender, RoutedEventArgs e)
        {
            string newAccount = txtAccount.Text.Trim();
            TreeViewItem selectedNode = tvAccounts.SelectedItem as TreeViewItem;

            if (string.IsNullOrWhiteSpace(newAccount) || selectedNode==null)
                return;

            string parentId = selectedNode.Tag.ToString();
            try
            {
                foreach (Account account in Accounts.GetList(parentId))
                {
                    if (account.AccountName== newAccount)
                    {
                        MessageBox.Show("An account  has not been added. Enter unique account name.", "Duplicated account", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                int result = Accounts.AddAccount(parentId, newAccount);
                if (result > 0)
                {
                    UpdateAccountsTree();
                    selectedNode.IsSelected = true;
                }
                else
                    throw new Exception();

            }
            catch
            {
                MessageBox.Show("Can't add new account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateAccount_Click(object sender, RoutedEventArgs e)
        {
            string newAccountName = txtAccount.Text.Trim();
            TreeViewItem updatedNode = tvAccounts.SelectedItem as TreeViewItem;

            if (string.IsNullOrWhiteSpace(newAccountName) || updatedNode == null)
                return;

            string level = SqlHierarchyId.Parse(updatedNode.Tag.ToString()).GetLevel().ToString();
            if (level == "1")
                return;

            string parentId = SqlHierarchyId.Parse(updatedNode.Tag.ToString()).GetAncestor(1).ToString();

            try
            {
                foreach (Account account in Accounts.GetList(parentId))
                {
                    if (account.AccountName == newAccountName)
                    {
                        MessageBox.Show("An account  has not been updated. Enter unique account name.", "Duplicated account", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                Account updatedAccount = new Account(updatedNode.Tag.ToString(), newAccountName);
                int result = Accounts.UpdateAccount(updatedAccount);
                if (result > 0)
                    UpdateAccountsTree();
                else
                    throw new Exception();

            }
            catch
            {
                MessageBox.Show("Can't update selected account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnRemoveAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TreeViewItem removedNode = tvAccounts.SelectedItem as TreeViewItem;

                if (removedNode == null)
                    return;

                string level = SqlHierarchyId.Parse(removedNode.Tag.ToString()).GetLevel().ToString();
                if (level=="1")
                    return;

                int result = Accounts.RemoveAccount(removedNode.Tag.ToString());
                if (result > 0)
                    UpdateAccountsTree();
                else
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Can't delete account.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCloseAccounts_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void tvAccounts_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem selectedNode = tvAccounts.SelectedItem as TreeViewItem;
            if (selectedNode!= null)
                txtAccount.Text = selectedNode.Header.ToString();
        }

        //private void txtAccount_GotFocus(object sender, RoutedEventArgs e)
        //{
        //    txtAccount.SelectionStart = 0;
        //    txtAccount.SelectionLength = txtAccount.Text.Length;

        //}

    }
}
