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
using DataLayer;
using System.IO;
using System.Windows.Media.Animation;
using System.Net;
using System.Globalization;
using System.Diagnostics;

namespace DiCaBoo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Settings mSettings;
        public MainWindow()
        {
            InitializeComponent();
            UpdateDiary();
            UpdateCalendar();
            cbEventTypes.ItemsSource = new EventTypes();
            mSettings = new Settings();
            SetSettings();
        }

        private void cbCalendarPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetEndDate();
            UpdateCalendar();
        }

        private void SetEndDate()
        {
            if (cbCalendarPeriod.SelectedItem == null)
                return;

            if (dpCalendarStartDate.SelectedDate == null)
                dpCalendarStartDate.SelectedDate = DateTime.Now;

            DateTime startDate = dpCalendarStartDate.SelectedDate.Value;
            ComboBoxItem selectedPeriod = (ComboBoxItem)cbCalendarPeriod.SelectedItem;

            switch (selectedPeriod.Content.ToString())
            {
                case "Day":
                    dpCalendarEndDate.SelectedDate = startDate;
                    break;
                case "Week":
                    dpCalendarEndDate.SelectedDate = FirstDayOfWeek(startDate).AddDays(6);
                    break;
                case "Month":
                    dpCalendarEndDate.SelectedDate = new DateTime(startDate.Year, startDate.Month, 1).AddMonths(1).AddDays(-1);
                    break;
                case "Year":
                    dpCalendarEndDate.SelectedDate = new DateTime(startDate.Year, 12, 31);
                    break;
                default:
                    return;
            }
        }

        private DateTime FirstDayOfWeek(DateTime dt)
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentCulture;
            var diff = dt.DayOfWeek - culture.DateTimeFormat.FirstDayOfWeek;
            if (diff < 0)
                diff += 7;
            return dt.AddDays(-diff);
        }

        private void dpCalendarStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            SetEndDate();
            UpdateCalendar();
        }


        private void btnSkipEventTypes_Click(object sender, RoutedEventArgs e)
        {
            cbEventTypes.SelectedItem = null;
            cbEventTypes.IsEditable = true;
            cbEventTypes.IsReadOnly = true;
            cbEventTypes.Text = "Event type";
            UpdateCalendar();
        }

        private void cbEventTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCalendar();
        }
    }
}
