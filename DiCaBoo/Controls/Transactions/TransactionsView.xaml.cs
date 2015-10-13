using DataLayer;
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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiCaBoo.Controls.Transactions
{
    /// <summary>
    /// Interaction logic for Transactions.xaml
    /// </summary>
    public partial class Transactions : UserControl
    {
        public Transactions()
        {
            InitializeComponent();
        }

        private void Transactions_Loaded(object sender, RoutedEventArgs e)
        {

            DiCaBoo.dbDCBDataSet dbDCBDataSet = ((DiCaBoo.dbDCBDataSet)(this.FindResource("dbDCBDataSet")));
            // Load data into the table Transactions. You can modify this code as needed.
            DiCaBoo.dbDCBDataSetTableAdapters.TransactionsTableAdapter dbDCBDataSetTransactionsTableAdapter = new DiCaBoo.dbDCBDataSetTableAdapters.TransactionsTableAdapter();
            dbDCBDataSetTransactionsTableAdapter.Fill(dbDCBDataSet.Transactions);
            System.Windows.Data.CollectionViewSource transactionsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("transactionsViewSource")));
            transactionsViewSource.View.MoveCurrentToLast();
        }

        private void Delete_Transaction(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)transactionsDataGrid.SelectedItem;
            if (row == null)
                return;

            if (MessageBox.Show("Remove the selected record?", "DiCaBoo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            string id = row.Row[0].ToString();
            if (DataLayer.Operations.RemoveTransaction(id) > 0)
                row.Row.Delete();

        }

        private void Edit_Transaction(object sender, RoutedEventArgs e)
        {
            DataRowView row = (DataRowView)transactionsDataGrid.SelectedItem;
            if (row == null)
                return;

            int id = 0;
            if (!int.TryParse(row.Row[0].ToString(), out id))
                throw new Exception("Can't edit transaction");

            Transaction transaction = new Transaction(id);

            if (transaction.ShowDialog() == true)
            {
                Transactions_Loaded(sender, e);//ShowTransactions();
            }
        }
    }
}
