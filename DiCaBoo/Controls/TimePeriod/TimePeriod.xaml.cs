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

namespace DiCaBoo
{
    /// <summary>
    /// Interaction logic for Period.xaml
    /// </summary>
    public partial class Period : UserControl
    {
        public event RoutedEventHandler DateChanged;


        public Period()
        {
            InitializeComponent();
        }

        public DateTime? StartDate {
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
        public int TimePeriodIndex
        {
            get
            {
                return cbPeriod.SelectedIndex;
            }
            set
            {
                if (value>cbPeriod.Items.Count-1 || value<-1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                cbPeriod.SelectedIndex = value;
            }
        }

        private void cbPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetEndDate();
        }

        DateTime? curStartDate;
        private void dpStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpStartDate.SelectedDate == null)
            {
                MessageBox.Show("Incorrect date", "DiCaBoo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                dpStartDate.SelectedDate = curStartDate;
            }

            //fix double event invocation
            if (dpStartDate.SelectedDate != curStartDate)
                curStartDate = dpStartDate.SelectedDate;
            else
                return;

            if (!SetEndDate() && DateChanged != null)
                DateChanged(sender, e);
        }

        DateTime? curEndDate;
        private void dpEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpEndDate.SelectedDate == null || (dpStartDate.SelectedDate != null && dpStartDate.SelectedDate > dpEndDate.SelectedDate))
            {
                MessageBox.Show("Incorrect date", "DiCaBoo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                dpEndDate.SelectedDate = curEndDate;
            }

            //fix double event invocation
            if (dpEndDate.SelectedDate != curEndDate)
                curEndDate = dpEndDate.SelectedDate;
            else
                return;

            if (dpEndDate.SelectedDate == null || dpStartDate.SelectedDate == null)
                return;

            if (DateChanged != null)
                DateChanged(sender, e);
        }

        private void btnSkipPeriod_Click(object sender, RoutedEventArgs e)
        {
            cbPeriod.SelectedItem = null;
            cbPeriod.Text = "Period";
        }

        private DateTime FirstDayOfWeek(DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff);
        }

        private bool SetEndDate()
        {
            ComboBoxItem selectedPeriod = (ComboBoxItem)cbPeriod.SelectedItem;
            string period = "";
            if (cbPeriod.SelectedItem != null)
                period = selectedPeriod.Content.ToString();

            if (dpStartDate.SelectedDate == null)
                dpStartDate.SelectedDate = DateTime.Now;

            DateTime startDate = dpStartDate.SelectedDate.Value;

            DateTime newEndDate;
            if (dpEndDate.SelectedDate != null && dpStartDate.SelectedDate<=dpEndDate.SelectedDate)
                newEndDate = dpEndDate.SelectedDate.Value;
            else
                newEndDate = startDate;

            switch (period)
            {
                case "Day":
                    newEndDate = startDate;
                    break;
                case "Week":
                    newEndDate = FirstDayOfWeek(startDate).AddDays(6);
                    break;
                case "Month":
                    newEndDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
                    break;
                case "Year":
                    newEndDate = new DateTime(startDate.Year, 12, 31);
                    break;
            }

            if (dpEndDate.SelectedDate == null || dpEndDate.SelectedDate.Value.Date != newEndDate)
            {
                dpEndDate.SelectedDate = newEndDate;
                return true;
            }

            return false;
        }
    }
}
