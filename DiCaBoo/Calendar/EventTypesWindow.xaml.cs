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
    /// Interaction logic for EventTypes.xaml
    /// </summary>
    public partial class Calendars : Window
    {
        private EventTypes eventTypes;

        public Calendars()
        {
            InitializeComponent();
            eventTypes = new EventTypes();
            lstTypes.ItemsSource = eventTypes;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnRemoveType_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EventType eventType = (EventType)lstTypes.SelectedItem;
                if (eventType == null)
                    return;

                int result = EventTypes.RemoveEventType(eventType.TypeId);
                if (result > 0)
                    UpdateTypesList();
                else
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Can't delete record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateType_Click(object sender, RoutedEventArgs e)
        {
            string newType = txtNewType.Text;
            EventType updatedType = (EventType)lstTypes.SelectedItem;
            if (string.IsNullOrWhiteSpace(newType) || updatedType == null)
                return;

            try
            {
                foreach (EventType type in eventTypes)
                {
                    if (type.TypeName == newType)
                    {
                        MessageBox.Show("Event type has not been updated. Enter unique event type.", "Duplicated type", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                updatedType = new EventType(updatedType.TypeId, newType);
                int result = EventTypes.UpdateEventType(updatedType);
                if (result > 0)
                    UpdateTypesList();
                else
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Can't update selected item.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnAddType_Click(object sender, RoutedEventArgs e)
        {
            string newType = txtNewType.Text;
            if (string.IsNullOrWhiteSpace(newType))
                return;

            try
            {
                foreach (EventType type in eventTypes)
                {
                    if (type.TypeName == newType)
                    {
                        MessageBox.Show("Event type has not been added. Enter unique event type.", "Duplicated type", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                int result = EventTypes.AddEventType(newType);
                if (result > 0)
                    UpdateTypesList();
                else
                    throw new Exception();
            }
            catch
            {
                MessageBox.Show("Can't add new record.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateTypesList()
        {
            eventTypes = new EventTypes();
            lstTypes.ItemsSource = eventTypes;
        }

        private void txtNewType_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtNewType.Text == "Enter new event type")
                txtNewType.Text = "";
        }

        private void txtNewType_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNewType.Text))
                txtNewType.Text = "Enter new event type";
        }

        private void lstTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EventType eventType = (EventType)lstTypes.SelectedItem;
            if (eventType != null)
                txtNewType.Text = eventType.TypeName.ToString();
        }
    }
}
