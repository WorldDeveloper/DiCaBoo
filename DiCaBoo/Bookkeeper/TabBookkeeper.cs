using DataLayer;
using DiCaBoo.Controls.Transactions;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DiCaBoo
{
    public partial class MainWindow
    {
        public void test(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("It works");
        }

        //private void MenuItemBalance_Click(object sender, RoutedEventArgs e)
        //{
        //    TreeList treeList = new TreeList();
        //    AccountNode parent = Accounts.GetTree(SqlHierarchyId.GetRoot().ToString());//Accounts.GetTree("/1/");//
        //    treeList.Tree.Items.Add(parent);
        //    bookkeeperPanel.Children.Clear();
        //    bookkeeperPanel.Children.Add(treeList);
        //}



        //private void MenuItemEditAccounts_Click(object sender, RoutedEventArgs e)
        //{
        //    AccountsWindow accountsWindow = new AccountsWindow();
        //    accountsWindow.ShowDialog();
        //}


        private void btnTransactions_Click(object sender, RoutedEventArgs e)
        {
            ShowTransactions();
        }

        private void ShowTransactions()
        {
            Transactions transactions = new Transactions();
            bookkeeperPanel.Children.Clear();
            bookkeeperPanel.Children.Add(transactions);
            dpBalanceDate.Visibility = Visibility.Collapsed;
            tpTransactionPeriod.Visibility = Visibility.Visible;

        }

        private void btnEditAccounts_Click(object sender, RoutedEventArgs e)
        {
            AccountsWindow accountsWindow = new AccountsWindow();
            accountsWindow.ShowDialog();
        }

        private void btnBalance_Click(object sender, RoutedEventArgs e)
        {
            ShowBalance();
            tpTransactionPeriod.Visibility = Visibility.Collapsed;
            dpBalanceDate.Visibility = Visibility.Visible;
            if (dpBalanceDate.SelectedDate == null)
                dpBalanceDate.SelectedDate = DateTime.Now.Date;

        }

        private void ShowBalance()
        {
            if (dpBalanceDate.SelectedDate == null)
                dpBalanceDate.SelectedDate = DateTime.Now;

            TreeList treeList = new TreeList();
            AccountNode parent = Accounts.GetTree("/1/", dpBalanceDate.SelectedDate.Value);//my resources  //Accounts.GetTree(SqlHierarchyId.GetRoot().ToString())//all accounts
            treeList.Tree.Items.Add(parent);
            bookkeeperPanel.Children.Clear();
            bookkeeperPanel.Children.Add(treeList);
        }

        private void btnAddTransaction_Click(object sender, RoutedEventArgs e)
        {
            Transaction tr = new Transaction();
            if(tr.ShowDialog()== true)
                ShowTransactions();
        }

        DateTime? curBalanceDate;
        private void dpBalanceDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpBalanceDate.SelectedDate == null)
            {
                MessageBox.Show("Incorrect date", "DiCaBoo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                dpBalanceDate.SelectedDate = curBalanceDate;
            }

            if (dpBalanceDate.SelectedDate != curBalanceDate)
                curBalanceDate = dpBalanceDate.SelectedDate;
            else
                return;

            ShowBalance();

        }
    }
}
