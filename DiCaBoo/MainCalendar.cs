using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DataLayer;
using System.IO;
using System.Net;
using System.Diagnostics;

namespace DiCaBoo
{
    public partial class MainWindow : Window
    {
        public void UpdateCalendar()
        {
            if (dpCalendarStartDate.SelectedDate == null || dpCalendarEndDate.SelectedDate == null)
                return;

            DateTime startDate = dpCalendarStartDate.SelectedDate.Value;
            DateTime endDate = dpCalendarEndDate.SelectedDate.Value;

            if (endDate < startDate)
                MessageBox.Show("An End date must be greater or equal to a Start date.", "Organizer", MessageBoxButton.OK, MessageBoxImage.Information);

            EventType eventType = cbEventTypes.SelectedItem as EventType;

            MyCalendar calendar;
            if (eventType == null)
                calendar = new MyCalendar(startDate, endDate.Date.AddDays(1));
            else
                calendar = new MyCalendar(startDate, endDate.Date.AddDays(1), eventType.TypeId);

            calendarPanel.Children.Clear();
            DateTime previousDate = DateTime.Now;
            int counter = 0;
            Style eventTitleStyle = Application.Current.Resources["eventTitle"] as Style;
            Style eventDetailsStyle = Application.Current.Resources["eventDetails"] as Style;
            Style dateStyle = Application.Current.Resources["postDate"] as Style;
            Style timeStyle = Application.Current.Resources["postTime"] as Style;

            foreach (CalendarEvent calEvent in calendar)
            {
                DockPanel itemPanel = new DockPanel();
                itemPanel.Tag = calEvent.EventId.ToString();
                itemPanel.MouseEnter += ItemStackPanel_MouseEnter;
                itemPanel.MouseLeave += ItemStackPanel_MouseLeave;
                itemPanel.MouseLeftButtonDown += ItemPanel_MouseLeftButtonDown;
                itemPanel.ContextMenu = CalendarContextMenu();

                TextBlock eventDate = new TextBlock();
                eventDate.Style = dateStyle;

                if (previousDate != calEvent.EventStart.Date)
                {
                    eventDate.Text = calEvent.EventStart.ToString("dd MMMM yyyy, ");
                    eventDate.Text += calEvent.EventStart.ToString("ddd").ToLower();
                    previousDate = calEvent.EventStart.Date;
                    calendarPanel.Children.Add(eventDate);
                }
                TextBlock eventTime = new TextBlock();
                eventTime.Style = timeStyle;
                eventTime.Text = calEvent.EventStart.ToString("HH:mm");

                itemPanel.Children.Add(eventTime);

                StackPanel eventDetails = new StackPanel();

                TextBlock eventTitle = new TextBlock();
                eventTitle.Text = calEvent.EventTitle;
                eventTitle.Style = eventTitleStyle;
                eventDetails.Children.Add(eventTitle);

                TextBlock eventEnd = new TextBlock();
                eventEnd.Text = "Ending: " + calEvent.EventEnd.ToString("HH:mm   ddd dd MMMM yyyy");
                eventEnd.Style = eventDetailsStyle;
                eventDetails.Children.Add(eventEnd);


                TextBlock eventVenue = new TextBlock();
                if (!string.IsNullOrWhiteSpace(calEvent.EventVenue))
                {
                    eventVenue.Text = "Location: ";
                }
                eventVenue.Text += calEvent.EventVenue;
                eventVenue.Style = eventDetailsStyle;
                eventVenue.Visibility = Visibility.Collapsed;
                eventDetails.Children.Add(eventVenue);


                TextBlock eventDescription = new TextBlock();
                eventDescription.Text = calEvent.EventDescription;
                eventDescription.Style = eventDetailsStyle;
                eventDescription.Visibility = Visibility.Collapsed;
                eventDetails.Children.Add(eventDescription);

                itemPanel.Children.Add(eventDetails);
                calendarPanel.Children.Add(itemPanel);

                counter++;
            }
        }

        private void btnManageCalendars_Click(object sender, RoutedEventArgs e)
        {
            Calendars wndEventTypes = new Calendars();
            wndEventTypes.ShowDialog();
        }

        private void btnCreateEvent_Click(object sender, RoutedEventArgs e)
        {
            MainEvent wndEvent = new MainEvent();
            if (wndEvent.ShowDialog() == true)
                UpdateCalendar();
        }

        private void ItemPanel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DockPanel activePanel = sender as DockPanel;
            StackPanel details = activePanel.Children.OfType<StackPanel>().FirstOrDefault();
            if (details == null)
                return;

            TextBlock location = details.Children.OfType<TextBlock>().ElementAt(2);
            TextBlock description = details.Children.OfType<TextBlock>().ElementAt(3);


            if (description == null || location == null)
                return;

            if (description.IsVisible == true)
            {
                description.Visibility = Visibility.Collapsed;
                location.Visibility = Visibility.Collapsed;
                details.Focus();
            }
            else
            {
                description.Visibility = Visibility.Visible;
                location.Visibility = Visibility.Visible;
            }
        }

        private ContextMenu CalendarContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem item = new MenuItem();
            item.Header = "Remove event";
            item.Click += RemoveEvent_Click;
            contextMenu.Items.Add(item);

            MenuItem editItem = new MenuItem();
            editItem.Header = "Edit event";
            editItem.Click += EditEvent_Click;
            contextMenu.Items.Add(editItem);

            return contextMenu;
        }

        private void EditEvent_Click(object sender, RoutedEventArgs e)
        {
            DockPanel activePanel = GetSelectedItem(e);
            if (activePanel == null)
                return;

            int id;
            if (!int.TryParse(activePanel.Tag.ToString(), out id))
                return;

            MainEvent wndEvent = new MainEvent(id);
            if (wndEvent.ShowDialog() == true)
                UpdateCalendar();
        }

        private void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            DockPanel activePanel = GetSelectedItem(e);
            if (activePanel == null)
                return;

            if (MessageBox.Show("Remove selected event?", "Removing...", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) //don't move up
                return;


            int id;
            if (int.TryParse(activePanel.Tag.ToString(), out id))
            {
                if (MyCalendar.RemoveEvent(id) > 0)
                    calendarPanel.Children.Remove(activePanel);
            }
        }

        private DockPanel GetSelectedItem(RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            if (menuItem == null)
                return null;

            ContextMenu contextMenu = menuItem.Parent as ContextMenu;
            if (contextMenu == null)
                return null;

            DockPanel activePanel = contextMenu.PlacementTarget as DockPanel;

            return activePanel;
        }

        private void cbCalendarPeriod_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetEndDate();
        }

        private bool SetEndDate()
        {
            if (cbCalendarPeriod.SelectedItem == null)
                return false;

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
                    return false;
            }

            return true;
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

        private void dpCalendarEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dpCalendarEndDate.SelectedDate == null || dpCalendarStartDate.SelectedDate==null)
                return;

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

        private void btnSkipPeriod_Click(object sender, RoutedEventArgs e)
        {
            cbCalendarPeriod.SelectedItem = null;
            cbCalendarPeriod.IsEditable = true;
            cbCalendarPeriod.IsReadOnly = true;
            cbCalendarPeriod.Text = "Period";
        }

        private void cbEventTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCalendar();
        }
    }
}