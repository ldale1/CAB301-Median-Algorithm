using MedianAlgorithm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace ChartApp
{
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            InitializeComponent();

            // Format combobox
            chartCombo.SelectedIndex = (int)ComboEnum.BASIC;
            chartCombo.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        // ComboBox Representations
        enum ComboEnum {
            BOTH = 0,
            BASIC = 1,
            TIME = 2
        };

        private void ChartForm_Load(object sender, EventArgs e)
        {
            // Add a title to the chart
            Title chtTitle = new Title("Algorithm Efficiency", Docking.Top, new Font("Arial", 16), Color.Black);
            chtTitle.Name = "MyTitle";
            EfficiencyChart.Titles.Add(chtTitle);

            // Add some series to the chart
            var series = new Series("Basic");
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
        
        // Put on a series or take it off
        private void series_on(Series series, int axis = 1)
        {
            series.IsVisibleInLegend = true;
            int axisType = (axis == 1) ? 0 : 1;
            series.YAxisType = (AxisType) axisType;
        }
        private void series_off(Series series) {
            series.IsVisibleInLegend = false;
            series.Points.Clear();
        }


        // Upper bound
        private double[] bestfit_oh(double[] y)
        {
            //x_series.Select(x => 0.5 * x * x).ToArray()
            return new double[0];
        }
        // Lower bound
        private double[] bestfit_omega(double[] y)
        {
            return new double[0];
        }
        // Middle
        private double[] bestfit_theta(double[] y)
        {
            return new double[0];
        }

        private void update_chart(object sender, EventArgs e)
        {
            // What values are we going to plot?
            int problemSize = (int)SizeUpDown.Value;
            int iterations = (int)DataPointsUpDown.Value;
            int[] x_series = Enumerable.Range(1, problemSize).ToArray();

            // Update
            ChartArea chartArea = EfficiencyChart.ChartAreas[0];
            if (chartCombo.SelectedIndex == (int)ComboEnum.BOTH)
            {
                // Formatting
                EfficiencyChart.Titles["MyTitle"].Text = "Size vs Basic Ops and Time";
                chartArea.AxisY.Title = "Count";
                chartArea.AxisY2.Title = "Time (ms)";
                series_on(EfficiencyChart.Series["Basic"]);
                series_on(EfficiencyChart.Series["Time"], 2);

                // Plot the output
                double[] y_basic = (from size in Enumerable.Range(1, problemSize)
                                    select MedianOps.countBasics(size, iterations)
                                    ).ToArray();
                double[] y_time = (from size in Enumerable.Range(1, problemSize)
                                   select MedianOps.countTime(size, iterations)
                                   ).ToArray();
                EfficiencyChart.Series["Basic"].Points.DataBindXY(x_series, y_basic);
                EfficiencyChart.Series["Time"].Points.DataBindXY(x_series, y_time);
            } else if (chartCombo.SelectedIndex == (int)ComboEnum.BASIC)
            {
                // Formatting
                EfficiencyChart.Titles["MyTitle"].Text = "Size vs Basic Operations";
                chartArea.AxisY.Title = "Count";
                series_on(EfficiencyChart.Series["Basic"]);
                series_off(EfficiencyChart.Series["Time"]);

                // Plot the output
                double[] y_basic = (from size in Enumerable.Range(1, problemSize)
                                    select MedianOps.countBasics(size, iterations)
                                    ).ToArray();
                EfficiencyChart.Series["Basic"].Points.DataBindXY(x_series, y_basic);


                try
                {
                    // Get a line of best fit
                    double[] y_bf = bestfit_oh(y_basic);
                    EfficiencyChart.Series["Best Fit"].Points.DataBindXY(x_series, y_bf);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
            } else
            {
                // Formatting
                EfficiencyChart.Titles["MyTitle"].Text = "Size vs Time";
                chartArea.AxisY.Title = "Time (ms)";
                series_off(EfficiencyChart.Series["Basic"]);
                series_on(EfficiencyChart.Series["Time"]);

                // Plot the output
                double[] y_time = (from size in Enumerable.Range(1, problemSize)
                                   select MedianOps.countTime(size, iterations)
                                   ).ToArray();
                EfficiencyChart.Series["Time"].Points.DataBindXY(x_series, y_time);
            }


            series_off(EfficiencyChart.Series["Best Fit"]);
        }

        private void save_chart(object sender, EventArgs e)
        {
            // Open a save box
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory())));
            saveDialog.Title = "Save Chart";
            saveDialog.FileName = "mychart";
            saveDialog.DefaultExt = "jpg";
            saveDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                int newWidth = 2100;
                int newHeight = 1500;

                // Save chart
                MemoryStream myStream = new MemoryStream();
                EfficiencyChart.Serializer.Save(myStream);

                // Update chart display to make look nice
                EfficiencyChart.Width = newWidth;
                EfficiencyChart.Height = newHeight;

                // Save from the chart object itself
                EfficiencyChart.SaveImage(saveDialog.FileName, ChartImageFormat.Png);

                // Save to a bitmap
                Bitmap bmp = new Bitmap(2100, 1500);
                EfficiencyChart.DrawToBitmap(bmp, new Rectangle(0, 0, newWidth, newHeight));
                //bmp.Save(@"D:\MyImage2.jpg");

                // Reload chart
                EfficiencyChart.Serializer.Load(myStream);
            }
        }
    }
}
