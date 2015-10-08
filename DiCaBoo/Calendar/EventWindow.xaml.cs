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
        private EventTypes mEventTypes;
        private int? mModifiedEventId;

        public MainEvent()
        {
            InitializeComponent();


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
                minutes.Add((i * 5).ToString("00"));
            }
            cbFromMin.ItemsSource = minutes;
            cbFromMin.SelectedIndex = 0;

            cbUntilMin.ItemsSource = minutes;
            cbUntilMin.SelectedIndex = 0;

            dpFromDate.SelectedDate = DateTime.Now.Date;
            dpUntilDate.SelectedDate = DateTime.Now.AddHours(1).Date;

            mEventTypes = new EventTypes();
            cbEventTypes.ItemsSource = mEventTypes;
        }

        public MainEvent(int id) : this()
        {
            CalendarEvent modifiedEvent = MyCalendar.GetEvent(id);
            if (modifiedEvent == null)
                throw new Exception("Can't edit event.");

            mModifiedEventId = modifiedEvent.EventId;

            txtTitle.Text = modifiedEvent.EventTitle;
            dpFromDate.SelectedDate = modifiedEvent.EventStart;
            cbFromHour.Text = modifiedEvent.EventStart.Hour.ToString("00");
            cbFromMin.Text = modifiedEvent.EventStart.Minute.ToString("00");
            dpUntilDate.SelectedDate = modifiedEvent.EventEnd;
            cbUntilHour.Text = modifiedEvent.EventEnd.Hour.ToString("00");
            cbUntilMin.Text = modifiedEvent.EventEnd.Minute.ToString("00");
            foreach (EventType item in cbEventTypes.Items)
            {
                if (item.TypeId == modifiedEvent.EventTypeId)
                {
                    cbEventTypes.SelectedItem = item;
                    break;
                }
            }
            txtWhere.Text = modifiedEvent.EventVenue;
            txtDescription.Text = modifiedEvent.EventDescription;


        }

        private void btnAddType_Click(object sender, RoutedEventArgs e)
        {
            Calendars newCalendar = new Calendars();
            newCalendar.ShowDialog();
            mEventTypes = new EventTypes();
            cbEventTypes.ItemsSource = mEventTypes;
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


                DateTime fromDateTime = ParseDateTime(dpFromDate.SelectedDate.Value, cbFromHour.Text, cbFromMin.Text, "From");
                DateTime untilDateTime = ParseDateTime(dpUntilDate.SelectedDate.Value, cbUntilHour.Text, cbUntilMin.Text, "Until");

                if (fromDateTime >= untilDateTime)
                    throw new Exception("Until date/time must be greater than From date/time.");

                EventType parsedType = cbEventTypes.SelectedItem as EventType;
                if (parsedType == null)
                    throw new Exception("Select event type.");

                int res;
                if (mModifiedEventId == null)
                    res = MyCalendar.AddEvent(parsedType.TypeId, fromDateTime, untilDateTime, txtTitle.Text, txtDescription.Text, txtWhere.Text);
                else
                    res = MyCalendar.UpdateEvent(new CalendarEvent(mModifiedEventId.Value, parsedType.TypeId, parsedType.TypeName, fromDateTime, untilDateTime, txtTitle.Text, txtDescription.Text, txtWhere.Text));

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
            if (parsedDate == null)
                throw new Exception("Select " + type + " date.");

            int hour = 0;
            if (!int.TryParse(parsedHour, out hour) || hour < 0 || hour > 23)
                throw new Exception("Enter correct " + type + " hours");

            int min = 0;
            if (!int.TryParse(parsedMin, out min) || min < 0 || min > 59)
                throw new Exception("Enter correct " + type + " minutes");

            parsedDate += new TimeSpan(hour, min, 0);
            return parsedDate;
        }

        private void dpFromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(dpFromDate.SelectedDate.HasValue)
                dpUntilDate.SelectedDate = dpFromDate.SelectedDate.Value;
        }
    }
}
