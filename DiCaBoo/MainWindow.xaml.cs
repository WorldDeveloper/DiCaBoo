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
            foreach (DiaryPost post in mDiary)
            {
                StackPanel itemStackPanel = new StackPanel();
                itemStackPanel.Tag = post.ID.ToString();
                itemStackPanel.Orientation = Orientation.Horizontal;
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

                itemStackPanel.Children.Add(postTime);
                TextBlock postContent = new TextBlock();
                postContent.Style = contentStyle;
                postContent.Text = post.Content;
                itemStackPanel.Children.Add(postContent);
                postsStackPanel.Children.Add(itemStackPanel);

                counter++;
            }

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
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }
            Diary.AddPost(text);

            newPost.Document.Blocks.Clear();
            newPost.Document.Blocks.Add(new Paragraph(new Run("What are you thinking about?")));
            newPost.Height = 50;

            UpdateDiary();
        }
    }
}
