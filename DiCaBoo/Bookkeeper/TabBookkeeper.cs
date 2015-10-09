using DataLayer;
using DiCaBoo.Controls.Transactions;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        }

        private void btnEditAccounts_Click(object sender, RoutedEventArgs e)
        {
            AccountsWindow accountsWindow = new AccountsWindow();
            accountsWindow.ShowDialog();
        }

        private void btnBalance_Click(object sender, RoutedEventArgs e)
        {
            TreeList treeList = new TreeList();
            AccountNode parent = Accounts.GetTree("/1/");//my resources  //Accounts.GetTree(SqlHierarchyId.GetRoot().ToString())//all accounts
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
    }
}
