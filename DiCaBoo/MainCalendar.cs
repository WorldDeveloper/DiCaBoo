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

namespace DiCaBoo
{
    public partial class MainWindow : Window
    {
        public void UpdateCalendar()
        {
            MyCalendar calendar = new MyCalendar();
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
                    eventDate.Text = calEvent.EventStart.ToString("dd MMMM yyyy");
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
                eventVenue.Text = "Location: " + calEvent.EventVenue;
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
            throw new NotImplementedException();
        }

        private void RemoveEvent_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = e.Source as MenuItem;
            if (menuItem == null)
                return;

            ContextMenu contextMenu = menuItem.Parent as ContextMenu;
            if (contextMenu == null)
                return;

            DockPanel activePanel = contextMenu.PlacementTarget as DockPanel;
            if (activePanel == null)
                return;

            if (MessageBox.Show("Remove selected event?", "Removing...", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) //don't move up
                return;


            int id = 0;
            if (int.TryParse(activePanel.Tag.ToString(), out id))
            {
                if (MyCalendar.RemoveEvent(id) > 0)
                    calendarPanel.Children.Remove(activePanel);
            }
        }
    }
}