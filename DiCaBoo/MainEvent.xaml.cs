using DataLayer;
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
    /// Interaction logic for Event.xaml
    /// </summary>

    public partial class MainEvent : Window
    {
        private EventTypes eventTypes;

        public MainEvent()
        {
            InitializeComponent();
            eventTypes = new EventTypes();

            List<string> hours = new List<string>();
            for (int i = 0; i < 24; i++)
            {
                hours.Add(i.ToString("00"));
            }
            cbFromHour.ItemsSource = hours;
            cbFromHour.SelectedIndex = 0;

            cbUntilHour.ItemsSource = hours;
            cbUntilHour.SelectedIndex = 0;

            List<string> minutes = new List<string>();
            for (int i = 0; i < 12; i++)
            {
                minutes.Add((i*5).ToString("00"));
            }
            cbFromMin.ItemsSource = minutes;
            cbFromMin.SelectedIndex = 0;

            cbUntilMin.ItemsSource = minutes;
            cbUntilMin.SelectedIndex = 0;

            dpFromDate.SelectedDate = DateTime.Now.Date;
            dpUntilDate.SelectedDate = DateTime.Now.AddHours(1).Date;
            cbEventTypes.ItemsSource = eventTypes;
        }

        private void btnAddType_Click(object sender, RoutedEventArgs e)
        {
            Calendars newCalendar = new Calendars();
            newCalendar.ShowDialog();
            eventTypes = new EventTypes();
            cbEventTypes.ItemsSource = eventTypes;
            cbEventTypes.SelectedIndex = -1;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                    throw new Exception("Enter event title.");

                DateTime fromDateTime=ParseDateTime(dpFromDate.SelectedDate.Value, cbFromHour.Text, cbFromMin.Text, "From");
                DateTime untilDateTime= ParseDateTime(dpUntilDate.SelectedDate.Value, cbUntilHour.Text, cbUntilMin.Text, "Until");
                if (fromDateTime>= untilDateTime)
                    throw new Exception("Until date/time must be greater than From date/time.");

                EventType parsedType = cbEventTypes.SelectedItem as EventType;
                if (parsedType == null)
                    throw new Exception("Select event type.");

                int res=MyCalendar.AddEvent(parsedType.TypeId, fromDateTime, untilDateTime, txtTitle.Text, txtDescription.Text, txtWhere.Text);
                if (res != 1)
                {
                    MessageBox.Show("Can't save event.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Incorrect input", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private DateTime ParseDateTime(DateTime parsedDate, string parsedHour, string parsedMin, string type)
        {
            if (parsedDate== null)
                throw new Exception("Select "+type+" date.");

            int hour = 0;
            if (!int.TryParse(parsedHour, out hour) || hour < 0 || hour > 23)
                throw new Exception("Enter correct "+type+" hours");

            int min = 0;
            if (!int.TryParse(parsedMin, out min) || min < 0 || min > 59)
                throw new Exception("Enter correct "+type+" minutes");

            parsedDate += new TimeSpan(hour, min, 0);
            return parsedDate;
        }
    }
}
