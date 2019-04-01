using MedianAlgorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace WindowsFormsApp1
{
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            InitializeComponent();
        }

        private void ChartForm_Load(object sender, EventArgs e)
        {
            // Add a title to the chart
            Title chtTitle = new Title("Algorithm Efficiency", Docking.Top, new Font("Arial", 16), Color.Black);
            chtTitle.Name = "MyTitle";
            EfficiencyChart.Titles.Add(chtTitle);


            // Add some series to the chart
            var series = new Series("Points");
            series.ChartType = SeriesChartType.Point;
            EfficiencyChart.Series.Add(series);

            // Plot the output of time points
            series = new Series("Time");
            series.ChartType = SeriesChartType.Point;
            series.YAxisType = AxisType.Secondary;
            EfficiencyChart.Series.Add(series);

            // Add a line of best fit series
            series = new Series("Best Fit");
            series.ChartType = SeriesChartType.Spline;
            EfficiencyChart.Series.Add(series);

            // Format Chart Area
            ChartArea chartArea = EfficiencyChart.ChartAreas[0];
            Font labelFont = new Font(DefaultFont.ToString(), 10, FontStyle.Bold);
            chartArea.AxisX.Title = "Problem Size";
            chartArea.AxisY.Title = "Count";
            chartArea.AxisY2.Title = "Time (ms)";
            chartArea.AxisX.TitleFont = labelFont;
            chartArea.AxisY.TitleFont = labelFont;
            chartArea.AxisY2.TitleFont = labelFont;
            chartArea.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chartArea.AxisY2.MajorGrid.LineColor = Color.Gray;
            chartArea.AxisX.IsMarginVisible = false;
            chartArea.AxisY.IsMarginVisible = false;

            // Format Legend
            Legend legend = EfficiencyChart.Legends[0];
            legend.Docking = Docking.Bottom;
            legend.Alignment = StringAlignment.Center;
            legend.BorderColor = Color.Black;
            legend.BorderDashStyle = ChartDashStyle.Dash;
            legend.IsDockedInsideChartArea = false;

            // Add to the chart
            update_chart(sender, e);
        }

        private void update_chart(object sender, EventArgs e)
        {
            EfficiencyChart.Titles["MyTitle"].Text = "Size vs Efficiency";

            // What values are we going to plot?
            int problemSize = (int)SizeUpDown.Value;
            int iterations = (int)DataPointsUpDown.Value;

            // X Series
            int[] x_series = Enumerable.Range(1, problemSize).ToArray();
            double[] y_count = x_series.Select(x => Median.BruteForceMedianCount(Enumerable.Range(1, x).Select(y => (double)y).ToArray())).ToArray();
            double[] y_time = x_series.Select(x => Median.BruteForceMedianTime(Enumerable.Range(1, x).Select(y => (double)y).ToArray())).ToArray();

            // Get a line of best fit
            double[] y_bf = x_series.Select(x => (double)0.5*x*x).ToArray();

            // Plot the output of count points
            EfficiencyChart.Series["Points"].Points.DataBindXY(x_series, y_count);
            EfficiencyChart.Series["Time"].Points.DataBindXY(x_series, y_time);
            EfficiencyChart.Series["Best Fit"].Points.DataBindXY(x_series, y_bf);
        }


        private void save_chart(object sender, EventArgs e)
        {
            // Save chart
            System.IO.MemoryStream myStream = new System.IO.MemoryStream();
            EfficiencyChart.Serializer.Save(myStream);

            EfficiencyChart.Width = 2100;
            EfficiencyChart.Height = 1500;

            // save from the chart object itself
            EfficiencyChart.SaveImage(@"D:\MyImage1.jpg", ChartImageFormat.Png);

            // save to a bitmap
            Bitmap bmp = new Bitmap(2100, 1500);
            EfficiencyChart.DrawToBitmap(bmp, new Rectangle(0, 0, 2100, 1500));
            bmp.Save(@"D:\MyImage2.jpg");

            // Reload chart
            EfficiencyChart.Serializer.Load(myStream);
        }
    }
}
