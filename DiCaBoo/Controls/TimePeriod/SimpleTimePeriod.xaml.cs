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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiCaBoo.Controls
{
    /// <summary>
    /// Interaction logic for Period.xaml
    /// </summary>
    public partial class SimpleTimePeriod : UserControl
    {
        public event RoutedEventHandler DateChanged;


        public SimpleTimePeriod()
        {
            InitializeComponent();
        }

        public DateTime? StartDate
        {
            get
            {
                return dpStartDate.SelectedDate;
            }
            set
            {
                dpStartDate.SelectedDate = value;
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return dpEndDate.SelectedDate;
            }
            set
            {
                dpEndDate.SelectedDate = value;
            }
        }


        DateTime? curStartDate;
        private void dpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpEndDate.SelectedDate != null && dpEndDate.SelectedDate < dpStartDate.SelectedDate)
            {
                dpStartDate.SelectedDate = curStartDate;
                MessageBox.Show("End date must be greater than or equal to a start date", "DiCaBoo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            //fix double event invocation
            if (dpStartDate.SelectedDate != curStartDate)
                curStartDate = dpStartDate.SelectedDate;
            else
                return;

            if (DateChanged != null)
                DateChanged(sender, e);
        }

        DateTime? curEndDate;
        private void dpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpStartDate.SelectedDate != null && dpEndDate.SelectedDate < dpStartDate.SelectedDate)
            {
                dpEndDate.SelectedDate = curEndDate;
                MessageBox.Show("End date must be greater than or equal to a start date", "DiCaBoo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            //fix double event invocation
            if (dpEndDate.SelectedDate != curEndDate)
                curEndDate = dpEndDate.SelectedDate;
            else
                return;

            if (DateChanged != null)
                DateChanged(sender, e);
        }
    }
}
