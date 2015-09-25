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
            cbEventTypes.ItemsSource = new EventTypes();
            dpCalendarStartDate.SelectedDate = DateTime.Now.Date;
            cbCalendarPeriod.SelectedIndex = 2;

            mSettings = new Settings();
            SetSettings();
        }

     
    }
}
