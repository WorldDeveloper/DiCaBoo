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
                DockPanel itemPanel = new DockPanel();
                itemPanel.Tag = post.ID.ToString();
                itemPanel.MouseEnter += ItemStackPanel_MouseEnter;
                itemPanel.MouseLeave += ItemStackPanel_MouseLeave;
                itemPanel.ContextMenu = DiaryContextMenu();

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

                FlowDocumentScrollViewer postContent = new FlowDocumentScrollViewer();
                FlowDocument doc = new FlowDocument();
                TextRange tr = new TextRange(doc.ContentStart, doc.ContentEnd);
                MemoryStream ms = GetMemoryStreamFromString(post.Content);
                tr.Load(ms, DataFormats.Rtf);
                postContent.Document = doc;
                postContent.ContextMenu = null;
                postContent.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;

                itemPanel.Children.Add(postContent);
                postsStackPanel.Children.Add(itemPanel);

                counter++;
            }
        }

        private ContextMenu DiaryContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem copyItem = new MenuItem();
            copyItem.Command = ApplicationCommands.Copy;
            contextMenu.Items.Add(copyItem);

            MenuItem item = new MenuItem();
            item.Header = "Remove record";
            item.Click += RemovePost_Click;
            contextMenu.Items.Add(item);

            MenuItem editItem = new MenuItem();
            editItem.Header = "Edit record";
            editItem.Click += EditRecord_Click;
            contextMenu.Items.Add(editItem);

            return contextMenu;
        }

        private void EditRecord_Click(object sender, RoutedEventArgs e)
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


            int id = 0;
            if (int.TryParse(activePanel.Tag.ToString(), out id))
            {
                FlowDocumentScrollViewer textViewer = activePanel.Children.OfType<FlowDocumentScrollViewer>().FirstOrDefault();
                if (textViewer==null)
                    return;

                //copy document from selected record to newPost RichTextBox
                RichTextBox updatedPost = new RichTextBox();
                updatedPost.Tag = id.ToString();
                updatedPost.LostFocus += UpdatedPost_LostFocus;

                TextRange tr = new TextRange(textViewer.Document.ContentStart, textViewer.Document.ContentEnd);
                MemoryStream ms = new MemoryStream();
                tr.Save(ms, DataFormats.Rtf);

                TextRange range2 = new TextRange(updatedPost.Document.ContentEnd, updatedPost.Document.ContentEnd);
                range2.Load(ms, DataFormats.Rtf);

                activePanel.Children.Remove(textViewer);
                activePanel.Children.Add(updatedPost);
                updatedPost.Focus();
            }
        }

        private void UpdatedPost_LostFocus(object sender, RoutedEventArgs e)
        {
            RichTextBox updatedPost = sender as RichTextBox;
            TextRange tr = new TextRange(updatedPost.Document.ContentStart, updatedPost.Document.ContentEnd);
            MemoryStream ms = new MemoryStream();
            tr.Save(ms, DataFormats.Rtf);
            string SQLData = ASCIIEncoding.Default.GetString(ms.ToArray());

            int id = 0;
            if (!int.TryParse(updatedPost.Tag.ToString(), out id))
            {
                UpdateDiary();
                return;
            }

            Diary.UpdatePost(id, SQLData);
            UpdateDiary();
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

            if (MessageBox.Show("Remove selected record?", "Removing...", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No) //don't move up
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
            TextRange tr = new TextRange(newPost.Document.ContentStart, newPost.Document.ContentEnd);
            MemoryStream ms = new MemoryStream();
            tr.Save(ms, DataFormats.Rtf);
            string SQLData = ASCIIEncoding.Default.GetString(ms.ToArray());

            if (string.IsNullOrWhiteSpace(SQLData) || Diary.AddPost(SQLData) != 1  )
            {
                return;
            }

            newPost.Document.Blocks.Clear();
            newPost.Document.Blocks.Add(new Paragraph(new Run("What are you thinking about?")));
            newPost.Height = 50;

            UpdateDiary();
        }

        public static MemoryStream GetMemoryStreamFromString(string s)
        {
            if (s == null || s.Length == 0)
                return null;
            MemoryStream m = new MemoryStream();
            StreamWriter sw = new StreamWriter(m);
            sw.Write(s);
            sw.Flush();
            return m;
        }
    }
}
