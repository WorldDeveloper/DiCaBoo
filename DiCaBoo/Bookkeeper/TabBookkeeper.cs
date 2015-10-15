using DataLayer;
using DiCaBoo.Controls;
using DiCaBoo.Controls.Transactions;
using Microsoft.SqlServer.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DiCaBoo
{
    public partial class MainWindow
    {
        enum AccWindows { Transactions, NetIncomeReport };
        private Transactions mTransactions;

        private void FilterRecords(object sender, RoutedEventArgs e)
        {
            if (bookkeeperPanel.Tag == null)
                return;
            else if (bookkeeperPanel.Tag.ToString() == AccWindows.Transactions.ToString())
            {
                string filter = null;
                if (tpTransactionPeriod.StartDate != null)
                    filter = AddFilter(filter, string.Format("OperationDate>='{0}'", tpTransactionPeriod.StartDate.Value.Date));

                if (tpTransactionPeriod.EndDate != null)
                    filter = AddFilter(filter, string.Format("OperationDate<'{0}'", tpTransactionPeriod.EndDate.Value.Date.AddDays(1)));


                if (ctCredit.cbComboTreeItem.Content != null)
                {
                    Account credit = (Account)ctCredit.cbComboTreeItem.Content;
                    filter = AddFilter(filter, string.Format("Credit='{0}'", credit.AccountName));
                }

                if (ctDebit.cbComboTreeItem.Content != null)
                {
                    Account debit = (Account)ctDebit.cbComboTreeItem.Content;
                    filter = AddFilter(filter, string.Format("Debit='{0}'", debit.AccountName));
                }

                dbDCBDataSet dbDCBDataSet = ((dbDCBDataSet)(mTransactions.FindResource("dbDCBDataSet")));
                DataView dv = dbDCBDataSet.Tables[2].DefaultView;
                dv.RowFilter = filter;
            }
            else if (bookkeeperPanel.Tag.ToString() == AccWindows.NetIncomeReport.ToString())
            {
                btnNetIncome_Click(null, null);
            }
        }

        private string AddFilter(string currentFilter, string newFilter)
        {
            if (string.IsNullOrWhiteSpace(currentFilter))
                return newFilter;
            else
                return new StringBuilder(currentFilter).Append(" AND ").Append(newFilter).ToString();
        }

        private void btnBalance_Click(object sender, RoutedEventArgs e)
        {
            ShowBalance();
            dpBalanceDate.Visibility = Visibility.Visible;
            gridFilters.Visibility = Visibility.Collapsed;
            tpTransactionPeriod.Visibility = Visibility.Collapsed;
            cbGroupBy.Visibility = Visibility.Collapsed;
            if (dpBalanceDate.SelectedDate == null)
                dpBalanceDate.SelectedDate = DateTime.Now.Date;
        }

        private void btnTransactions_Click(object sender, RoutedEventArgs e)
        {
            ShowTransactions();
        }

        private void btnNetIncome_Click(object sender, RoutedEventArgs e)
        {
            dpBalanceDate.Visibility = Visibility.Collapsed;
            gridFilters.Visibility = Visibility.Collapsed;
            tpTransactionPeriod.Visibility = Visibility.Visible;
            cbGroupBy.Visibility = Visibility.Visible;

            TreeList treeList = new TreeList();
            AccountNode incomes = Accounts.ProfitLossReport("/2/", tpTransactionPeriod.StartDate, tpTransactionPeriod.EndDate);
            AccountNode expenses = Accounts.ProfitLossReport("/3/", tpTransactionPeriod.StartDate, tpTransactionPeriod.EndDate);
            AccountNode netIncome = new AccountNode();
            netIncome.RootAccount = new AccountInfo(SqlHierarchyId.Parse("/4/"), "Net income", incomes.RootAccount.Balance + expenses.RootAccount.Balance);
            treeList.Tree.Items.Add(incomes);
            treeList.Tree.Items.Add(expenses);
            treeList.Tree.Items.Add(netIncome);
            bookkeeperPanel.Children.Clear();
            bookkeeperPanel.Children.Add(treeList);
            bookkeeperPanel.Tag = AccWindows.NetIncomeReport;

            BuildChart();
        }

        private void BuildChart()
        {
            Accounts.GroupBy groupBy = Accounts.GroupBy.Month;
            if (cbGroupBy.SelectedItem != null)
            {
                ComboBoxItem ci = cbGroupBy.SelectedItem as ComboBoxItem;
                groupBy = (Accounts.GroupBy)Enum.Parse(typeof(Accounts.GroupBy), ci.Content.ToString());
            }

            Chart chartProfitLoss = new Chart(bookkeeperPanel.ActualHeight / 2, bookkeeperPanel.ActualWidth, groupBy);
            chartProfitLoss.Margin = new Thickness(0, 20, 0, 20);
            List<ChartPoint> incomes = Accounts.GetAmounts("/2/", groupBy, tpTransactionPeriod.StartDate, tpTransactionPeriod.EndDate);
            chartProfitLoss.AddLine("Incomes", Colors.Green, incomes);

            List<ChartPoint> expences = Accounts.GetAmounts("/3/", groupBy, tpTransactionPeriod.StartDate, tpTransactionPeriod.EndDate);
            chartProfitLoss.AddLine("Expenses", Colors.Red, expences);

            chartProfitLoss.Build();

            bookkeeperPanel.Children.Add(chartProfitLoss);
        }

        private void ShowTransactions()
        {
            mTransactions = new Transactions();
            bookkeeperPanel.Children.Clear();
            bookkeeperPanel.Children.Add(mTransactions);
            bookkeeperPanel.Tag = AccWindows.Transactions;
            dpBalanceDate.Visibility = Visibility.Collapsed;
            cbGroupBy.Visibility = Visibility.Collapsed;

            ShortAccountNode assets = Accounts.GetShortTree("/1/");
            ShortAccountNode incomes = Accounts.GetShortTree("/2/");
            ShortAccountNode expences = Accounts.GetShortTree("/3/");

            ctCredit.tvNestedTree.Items.Clear();
            ctCredit.tvNestedTree.Items.Add(assets);
            ctCredit.tvNestedTree.Items.Add(incomes);

            ctDebit.tvNestedTree.Items.Clear();
            ctDebit.tvNestedTree.Items.Add(assets);
            ctDebit.tvNestedTree.Items.Add(expences);

            gridFilters.Visibility = Visibility.Visible;
            tpTransactionPeriod.Visibility = Visibility.Visible;
            FilterRecords(null, null);
        }

        private void btnEditAccounts_Click(object sender, RoutedEventArgs e)
        {
            AccountsWindow accountsWindow = new AccountsWindow();
            accountsWindow.ShowDialog();
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
            if (tr.ShowDialog() == true)
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

        private void btnSkipCredit_Click(object sender, RoutedEventArgs e)
        {
            ctCredit.cbComboTreeItem.IsSelected = false;
            ctCredit.cbComboTreeItem.Content = null;
            FilterRecords(sender, e);
        }

        private void btnSkipDebit_Click(object sender, RoutedEventArgs e)
        {
            ctDebit.cbComboTreeItem.IsSelected = false;
            ctDebit.cbComboTreeItem.Content = null;
            FilterRecords(sender, e);
        }

        private void cbGroupBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterRecords(sender, e);
        }
    }
}
