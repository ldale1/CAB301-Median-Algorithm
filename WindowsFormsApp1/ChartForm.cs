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
            // What values are we going to plot?
            int problemSize = (int)SizeUpDown.Value;
            int iterations = (int)DataPointsUpDown.Value;

            // X Series
            int[] x_series = Enumerable.Range(1, problemSize).ToArray();
            double[] y_count = x_series.Select(x => Median.BruteForceMedianCount(Enumerable.Range(1, x).Select(y => (double)y).ToArray())).ToArray();
            double[] y_time = x_series.Select(x => Median.BruteForceMedianTime(Enumerable.Range(1, x).Select(y => (double)y).ToArray())).ToArray();
            double[] y_bf = x_series.Select(x => (double)0.5*x*x).ToArray();

            // Plot the output of count points
            EfficiencyChart.Series["Points"].Points.DataBindXY(x_series, y_count);
            EfficiencyChart.Series["Time"].Points.DataBindXY(x_series, y_time);
            EfficiencyChart.Series["Best Fit"].Points.DataBindXY(x_series, y_bf);
        }
    }
}
