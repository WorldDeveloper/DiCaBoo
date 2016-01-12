using DataLayer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public  ObservableCollection<Operation> mOperations;
        public Transactions()
        {
            InitializeComponent();
            mOperations =new ObservableCollection<Operation>(Operations.GetOperations());
            transactionsDataGrid.ItemsSource = mOperations;
        }

        //private void Transactions_Loaded(object sender, RoutedEventArgs e)
        //{

        //    DiCaBoo.dbDCBDataSet dbDCBDataSet = ((DiCaBoo.dbDCBDataSet)(this.FindResource("dbDCBDataSet")));

        //    // Load data into the table Transactions. You can modify this code as needed.
        //    DiCaBoo.dbDCBDataSetTableAdapters.TransactionsTableAdapter dbDCBDataSetTransactionsTableAdapter = new DiCaBoo.dbDCBDataSetTableAdapters.TransactionsTableAdapter();
        //    dbDCBDataSetTransactionsTableAdapter.Fill(dbDCBDataSet.Transactions);
        //    System.Windows.Data.CollectionViewSource transactionsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("transactionsViewSource")));
        //    transactionsViewSource.View.MoveCurrentToLast();
        //}

        private void Delete_Transaction(object sender, RoutedEventArgs e)
        {
            Operation operation = (Operation)transactionsDataGrid.SelectedItem;
            if (operation == null || operation.ID <1)
                return;

            if (MessageBox.Show("Remove the selected record?", "DiCaBoo", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;

            if (DataLayer.Operations.RemoveTransaction(operation.ID) > 0)
            {
                mOperations.Remove(operation);
                //transactionsDataGrid.Items.Refresh();
            }
        }

        private void Edit_Transaction(object sender, RoutedEventArgs e)
        {
            Operation operation = (Operation)transactionsDataGrid.SelectedItem;
            if (operation == null || operation.ID < 1)
                return;

            Transaction transaction = new Transaction(operation.ID);

            if (transaction.ShowDialog() == true)
            {
                mOperations =new ObservableCollection<Operation>(Operations.GetOperations());
                transactionsDataGrid.ItemsSource = mOperations;
            }
        }
    }
}
