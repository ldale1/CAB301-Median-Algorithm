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
            var series = new Series("y1");
            series.ChartType = SeriesChartType.Point;
            EfficiencyChart.Series.Add(series);

            // Plot the output of time points
            series = new Series("y2");
            series.ChartType = SeriesChartType.Point;
            series.YAxisType = AxisType.Secondary;
            EfficiencyChart.Series.Add(series);

            // Add a line of best fit series
            series = new Series("y1-oh");
            series.ChartType = SeriesChartType.Spline;
            EfficiencyChart.Series.Add(series);

            // Add a line of best fit series
            series = new Series("y1-omega");
            series.ChartType = SeriesChartType.Spline;
            EfficiencyChart.Series.Add(series);

            // Add a line of best fit series
            series = new Series("y2-oh");
            series.ChartType = SeriesChartType.Spline;
            EfficiencyChart.Series.Add(series);

            // Add a line of best fit series
            series = new Series("y2-omega");
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
        private void series_toggle(Series series, bool onoff)
        {
            series.IsVisibleInLegend = onoff;
            if (!onoff) { series.Points.Clear(); }
        }


        // Upper bound
        private double[] bestfit_oh(double[] x_series)
        {
            double g = 0.0;
            int breakCounter = 100;
            double[] vals;
            bool tuning;
            do {
                vals = Enumerable.Range(1, x_series.Length).Select(x => g * x * x + 1).ToArray();
                tuning = vals.Where(val => val < x_series[Array.IndexOf(vals, val)]).Any(); // While any point < x_series
                // Loop updating
                g += 0.025;
                breakCounter--;
            } while (tuning && breakCounter > 0) ;
            return vals;
        }
        // Lower bound
        private double[] bestfit_omega(double[] x_series)
        {
            double g = 0.65;
            int breakCounter = 100;
            double[] vals;
            bool tuning;
            do
            {
                vals = Enumerable.Range(1, x_series.Length).Select(x => g * x * x - 1).ToArray();
                tuning = vals.Where(val => val > x_series[Array.IndexOf(vals, val)]).Any(); // While any point > x_series
                // Loop updating
                g -= 0.025;
                breakCounter--;
            } while (tuning && breakCounter > 0);
            return vals;
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

                // Enable secondary
                series_toggle(EfficiencyChart.Series["y2"], true);
                series_toggle(EfficiencyChart.Series["y2-oh"], true);
                series_toggle(EfficiencyChart.Series["y2-omega"], true);

                // Plot the output
                double[] y_basic = (from size in Enumerable.Range(1, problemSize)
                                    select MedianOps.countBasics(size, iterations)).ToArray();
                EfficiencyChart.Series["y1"].Points.DataBindXY(x_series, y_basic);

                // Plot the output for time
                double[] y_time = (from size in Enumerable.Range(1, problemSize)
                                   select MedianOps.countTime(size, iterations)).ToArray();
                EfficiencyChart.Series["y2"].Points.DataBindXY(x_series, y_time);
            }
            else
            {
                // Labelling
                String subtitle = (chartCombo.SelectedIndex == (int)ComboEnum.BASIC) ? "Basic Ops" : "Time";
                EfficiencyChart.Titles["MyTitle"].Text = String.Format("Size vs {0}", subtitle);
                String axistitle = (chartCombo.SelectedIndex == (int)ComboEnum.BASIC) ? "Count" : "Time [ms]";
                chartArea.AxisY.Title = axistitle;

                // Disable secondary
                EfficiencyChart.Series["y1"].LegendText = subtitle;
                EfficiencyChart.Series["y1-oh"].LegendText = String.Format("O({0})", subtitle);
                EfficiencyChart.Series["y1-omega"].LegendText = String.Format("Ω({0})", subtitle);

                // Toggle off the secondary axis
                series_toggle(EfficiencyChart.Series["y2"], false);
                series_toggle(EfficiencyChart.Series["y2-oh"], false);
                series_toggle(EfficiencyChart.Series["y2-omega"], false);

                EfficiencyChart.Series["y1-oh"].BorderWidth = 5;
                EfficiencyChart.Series["y1-omega"].BorderWidth = 5;

                // Plot the series
                double[] y1 = (chartCombo.SelectedIndex == (int)ComboEnum.BASIC) ?
                     (from size in Enumerable.Range(1, problemSize) select MedianOps.countBasics(size, iterations)).ToArray() :
                     (from size in Enumerable.Range(1, problemSize) select MedianOps.countTime(size, iterations)).ToArray();
                EfficiencyChart.Series["y1"].Points.DataBindXY(x_series, y1);

                // Bestfit fit
                double[] y1_oh = bestfit_oh(y1);
                double[] y1_omega = bestfit_omega(y1);
                //EfficiencyChart.Series["y1-oh"].Points.DataBindXY(x_series, y1_oh);
                //EfficiencyChart.Series["y1-omega"].Points.DataBindXY(x_series, y1_omega);
            }
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
