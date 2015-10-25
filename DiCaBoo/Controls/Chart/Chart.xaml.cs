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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DiCaBoo.Controls
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        List<ChartRow> mRowsCollection;
        Accounts.GroupBy mGroupBy;
        public Chart(Double height, Double width, Accounts.GroupBy groupBy)
        {
            InitializeComponent();

            canvas.Height = height;
            canvas.Width = width;
            mRowsCollection = new List<ChartRow>();
            mGroupBy = groupBy;
        }

        public void AddLine(string name, Color color, List<ChartPoint> points)
        {
            if (points.Count == 0)
                return;

            mRowsCollection.Add(new ChartRow(name, color, points));
        }

        public void Build()
        {
            double maxY = 0;
            double minY = 0;
            HashSet<DateTime> chartDatesHash = new HashSet<DateTime>();

            foreach (ChartRow item in mRowsCollection)
            {
                foreach (ChartPoint point in item.Row)
                {
                    chartDatesHash.Add(point.Date);
                }

                double tmpMaxY = (double)item.Row.Max(t => t.Value);
                if (tmpMaxY > maxY)
                    maxY = tmpMaxY;

                double tmpMinY = (double)item.Row.Min(t => t.Value);
                if (tmpMinY < minY)
                    minY = tmpMinY;
            }
            Double maxRowHeight = maxY - minY;

            List<DateTime> chartDatesList = chartDatesHash.ToList<DateTime>();
            chartDatesList.Sort();

            double lineWidth = canvas.Width / (chartDatesList.Count * (mRowsCollection.Count + 1));//1 empty row
            double shift = lineWidth * (mRowsCollection.Count + 1);


            double ratio = (canvas.Height - 60) / (double)maxRowHeight;
            double middleHeight = (double)maxY * ratio;
            if (middleHeight < 0)
                middleHeight = 0;

            //horizontal axis
            Line myLine = new Line();
            myLine.Stroke = new SolidColorBrush(Colors.Black);
            myLine.X1 = 0;
            myLine.X2 = canvas.Width;
            myLine.Y1 = middleHeight;
            myLine.Y2 = middleHeight;
            myLine.StrokeThickness = 1;
            canvas.Children.Add(myLine);

            int row = 0;

            foreach (ChartRow item in mRowsCollection)
            {

                double x = lineWidth * ++row;
                int j = 0;
                for (int i = 0; i < chartDatesList.Count; ++i)
                {
                    DateTime date = chartDatesList[i];
                    string dateString = null;
                    if (mGroupBy == Accounts.GroupBy.Day)
                        dateString = date.Date.ToShortDateString();
                    else if (mGroupBy == Accounts.GroupBy.Month)
                        dateString = date.Date.ToString("MM.yy");
                    else if (mGroupBy == Accounts.GroupBy.Year)
                        dateString = date.Date.Year.ToString();

                    if (row == 1 && shift > 50)
                        DrawText(x, middleHeight + 10, dateString);

                    while (j < item.Row.Count)
                    {
                        if (item.Row[j].Date == date)
                        {
                            myLine = new Line();
                            myLine.Stroke = new SolidColorBrush(item.Color);
                            myLine.X1 = x;
                            myLine.X2 = x;
                            myLine.Y1 = middleHeight;
                            myLine.Y2 = middleHeight - (double)item.Row[j].Value * ratio;
                            myLine.StrokeThickness = lineWidth;
                            myLine.ToolTip = string.Format("{0:0.00}\n{1}", item.Row[j].Value, dateString);
                            canvas.Children.Add(myLine);
                            j++;
                        }
                        x += shift;
                        break;
                    }
                }

                //labels
                double lineY = canvas.Height - 15;
                double lineX = row * 80;
                myLine = new Line();
                myLine.Stroke = new SolidColorBrush(item.Color);
                myLine.X1 = lineX - 10;
                myLine.X2 = lineX;
                myLine.Y1 = lineY;
                myLine.Y2 = lineY;
                myLine.StrokeThickness = 10;
                canvas.Children.Add(myLine);

                DrawText(5 + lineX, lineY - 8, item.Name);
            }
        }

        private void DrawText(double x, double y, string text)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = text;
            Canvas.SetLeft(textBlock, x);
            Canvas.SetTop(textBlock, y);
            canvas.Children.Add(textBlock);
        }
    }

    struct ChartRow
    {
        public string Name { get; private set; }
        public Color Color { get; private set; }
        public List<ChartPoint> Row { get; private set; }

        public ChartRow(string name, Color color, List<ChartPoint> row)
        {
            Name = name;
            Color = color;
            Row = row;
        }
    }
}
