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

namespace DiCaBoo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            UpdateDiary();
        }

        public void UpdateDiary()
        {
            Diary mDiary = new Diary();
            postsStackPanel.Children.Clear();
            DateTime previousDate=DateTime.Now;
            int counter = 0;
            Style contentStyle = Application.Current.Resources["postContent"] as Style;
            Style dateStyle = Application.Current.Resources["postDate"] as Style;
            Style timeStyle = Application.Current.Resources["postTime"] as Style;

            ContextMenu contextMenu = new ContextMenu();
            MenuItem item = new MenuItem();
            item.Header = "Remove post";
            item.Click += RemovePost_Click;
            contextMenu.Items.Add(item);

            foreach (DiaryPost post in mDiary)
            {
                DockPanel itemPanel = new DockPanel();
                itemPanel.Tag = post.ID.ToString();
                itemPanel.MouseEnter += ItemStackPanel_MouseEnter;
                itemPanel.MouseLeave += ItemStackPanel_MouseLeave;
                itemPanel.ContextMenu = contextMenu;

                TextBlock postDate = new TextBlock();
                postDate.Style = dateStyle;
                
                if (previousDate != post.DateTime.Date)
                {
                    postDate.Text = post.DateTime.ToString("dd MMMM yyyy");
                    previousDate = post.DateTime.Date;
                    postsStackPanel.Children.Add(postDate);
                }
                TextBlock postTime = new TextBlock();
                postTime.Style = timeStyle;
                postTime.Text = post.DateTime.ToString("HH:mm");

                itemPanel.Children.Add(postTime);
                TextBlock postContent = new TextBlock();
                postContent.Style = contentStyle;
                postContent.Text = post.Content;
                postContent.TextWrapping = TextWrapping.Wrap;
                itemPanel.Children.Add(postContent);
                postsStackPanel.Children.Add(itemPanel);

                counter++;
            }

        }

        private void RemovePost_Click(object sender, RoutedEventArgs e)
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

            if (MessageBox.Show("Remove selected post?", "Post removing...", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) //don't move up
                return;


            int id = 0;
            if (int.TryParse(activePanel.Tag.ToString(), out id))
            {
                if (Diary.RemovePost(id)>0)
                    postsStackPanel.Children.Remove(activePanel);
            }

        }

        private void ItemStackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            DockPanel activePanel = sender as DockPanel;
            activePanel.Background = new SolidColorBrush(Colors.White);
            
        }

        private void ItemStackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            DockPanel activePanel = sender as DockPanel;
            activePanel.Background = new SolidColorBrush(Colors.Beige);
        }

        private void NewPost_GotFocus(object sender, RoutedEventArgs e)
        {
            newPost.Height = Double.NaN;
            string text = new TextRange(newPost.Document.ContentStart, newPost.Document.ContentEnd).Text;

            if (text == "What are you thinking about?\r\n")
                newPost.Document.Blocks.Clear();
        }

        private void NewPost_LostFocus(object sender, RoutedEventArgs e)
        {
            string text = new TextRange(newPost.Document.ContentStart, newPost.Document.ContentEnd).Text;
            if (string.IsNullOrWhiteSpace(text))
            {
                newPost.Document.Blocks.Add(new Paragraph(new Run("What are you thinking about?")));
                newPost.Height = 50;
            }
        }

        private void publishPost_Click(object sender, RoutedEventArgs e)
        {
            string text = new TextRange(newPost.Document.ContentStart, newPost.Document.ContentEnd).Text;
            if (string.IsNullOrWhiteSpace(text) || Diary.AddPost(text) !=1)
            {
                return;
            }

            newPost.Document.Blocks.Clear();
            newPost.Document.Blocks.Add(new Paragraph(new Run("What are you thinking about?")));
            newPost.Height = 50;

            UpdateDiary();
        }
    }
}
