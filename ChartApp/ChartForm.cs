using MedianAlgorithm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
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
            Title chtTitle = new Title("", Docking.Top, new Font("Arial", 16), Color.Black);
            chtTitle.Name = "Title";
            EfficiencyChart.Titles.Add(chtTitle);

            chtTitle = new Title("", Docking.Top, new Font("Arial", 8), Color.Black);
            chtTitle.Name = "InfoTitle";
            EfficiencyChart.Titles.Add(chtTitle);

            // Lines for best fit series
            Series series;
            foreach (String seriesName in new List<String> { "y1", "y1-oh", "y1-omega", "y2", "y2-oh", "y2-omega" })
            {
                series = new Series(seriesName);
                if (seriesName.IndexOf("-") < 0) {
                    series.ChartType = SeriesChartType.Point;
                }
                else {
                    series.ChartType = SeriesChartType.Spline;
                    series.BorderWidth = 2;
                }
                if (seriesName.IndexOf("2") >= 0) { series.YAxisType = AxisType.Secondary;}
                EfficiencyChart.Series.Add(series);
            }

            // Format Chart Area
            ChartArea chartArea = EfficiencyChart.ChartAreas[0];
            Font labelFont = new Font(DefaultFont.ToString(), 10, FontStyle.Bold);
            chartArea.AxisX.Title = "Problem Size";
            chartArea.AxisX.TitleFont = labelFont;
            chartArea.AxisY.TitleFont = labelFont;
            chartArea.AxisY2.TitleFont = labelFont;
            chartArea.AxisX.IsMarginVisible = false;
            chartArea.AxisX.Minimum = 0;
            chartArea.AxisY.IsMarginVisible = false;
            chartArea.AxisY.Minimum = 0;
            chartArea.AxisY.LabelStyle.Angle = -25;
            chartArea.AxisY2.IsMarginVisible = false;
            chartArea.AxisY2.Minimum = 0;
            chartArea.AxisY2.LabelStyle.Angle = -25;
            chartArea.AxisY2.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
            chartArea.AxisY2.MajorGrid.LineColor = Color.Gray;

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
        private void toggle_series(Series series, bool onoff)
        {
            series.IsVisibleInLegend = onoff;
            if (!onoff) { series.Points.Clear(); }
        }
        private void secondary_toggle(bool onoff)
        {
            foreach (String seriesName in new List<String> { "y2", "y2-oh", "y2-omega" })
            {
                toggle_series(EfficiencyChart.Series[seriesName], onoff);
            }
        }
        private void axis_bind(double[] points, String subtitle, String axistitle, AxisType axisType)
        {
            ChartArea chartArea = EfficiencyChart.ChartAreas[0];
            String seriesName = axisType == AxisType.Primary ? "y1" : "y2";
            // Max scale value
            double scale = Math.Max(Math.Pow(10, (int)Math.Log10(points.Max()))/10, 1);
            int val = (int)Math.Max((Math.Ceiling(points.Max() / scale) * scale), 1);
            //
            if (axisType == AxisType.Primary)
            {
                EfficiencyChart.ChartAreas[0].AxisY.Title = axistitle;
                chartArea.AxisY.Maximum = val;
            } else
            {
                EfficiencyChart.ChartAreas[0].AxisY2.Title = axistitle;
                chartArea.AxisY2.Maximum = val;
            }

            // Bind to axis
            int[] x_series = Enumerable.Range(1, points.Length).ToArray();
            EfficiencyChart.Series[seriesName].Points.DataBindXY(x_series, points);
            EfficiencyChart.Series[seriesName].LegendText = subtitle;

            toggle_series(EfficiencyChart.Series[seriesName + "-oh"], true);
            toggle_series(EfficiencyChart.Series[seriesName + "-omega"], true);

            Tuple<int[], double[], double> y1_oh = bestfit_oh(points);
            try
            {
                EfficiencyChart.Series[seriesName + "-oh"].Points.DataBindXY(y1_oh.Item1, y1_oh.Item2);
                EfficiencyChart.Series[seriesName + "-oh"].LegendText = String.Format("O({0}) : {1}x^2", subtitle, Math.Round(y1_oh.Item3, 4));
            }
            catch (Exception exc)
            {
                toggle_series(EfficiencyChart.Series[seriesName + "-oh"], false);
                Console.WriteLine(exc);
            }

            Tuple<int[], double[], double> y1_omega = bestfit_omega(points);
            try { 
                EfficiencyChart.Series[seriesName + "-omega"].Points.DataBindXY(y1_omega.Item1, y1_omega.Item2);
                EfficiencyChart.Series[seriesName + "-omega"].LegendText = String.Format("Ω({0}) : {1}x^2", subtitle, Math.Round(y1_omega.Item3, 4));
            } catch (Exception exc)
            {
                toggle_series(EfficiencyChart.Series[seriesName + "-omega"], false);
                Console.WriteLine(exc);
            }
        }

        // Upper bound
        private Tuple<int[], double[], double> bestfit_oh(double[] x_series)
        {
            int startOh;
            try
            {
                startOh = Math.Max(x_series.ToList().IndexOf(x_series.Where(val => val > 0).ToArray()[0]), 0);
            }
            catch (IndexOutOfRangeException exc)
            {
                Console.WriteLine(exc);
                startOh = 0;
            }
            double offset = x_series[startOh];
            x_series = x_series.Skip(startOh).Take(x_series.Length - startOh).Select(x => x - offset).ToArray();
            double g = 0;
            int breakCounter = 10000;
            double[] vals;
            bool tuning;
            do {
                g += 0.0002;
                vals = Enumerable.Range(1, x_series.Length).Select(x => g * x * x + offset * 9).ToArray();
                tuning = vals.Where(val => val < x_series[Array.IndexOf(vals, val)]).Any(); // While any point < x_series
                breakCounter--;
            } while (tuning && breakCounter > 0) ;
            return Tuple.Create(Enumerable.Range(startOh + 1, x_series.Length).ToArray(), vals.Select(x => x + offset).ToArray(), g);
        }
        // Lower bound
        private Tuple<int[], double[], double> bestfit_omega(double[] x_series)
        {
            int startOh = Math.Max(x_series.ToList().LastIndexOf(0), 0);
            x_series = x_series.Skip(startOh).Take(x_series.Length - startOh).ToArray();
            double g = 2;
            int breakCounter = 10000;
            double[] vals;
            bool tuning;
            do
            {
                g -= 0.0002;
                vals = Enumerable.Range(1, x_series.Length).Select(x => g * x * x-1).ToArray();
                tuning = vals.Where(val => val > x_series[Array.IndexOf(vals, val)]).Any(); // While any point > x_series
                breakCounter--;
            } while (tuning && breakCounter > 0);
            return Tuple.Create(Enumerable.Range(startOh + 1, x_series.Length).ToArray(), vals, g);
        }

        // Update the chart
        private void update_chart(object sender, EventArgs e)
        {
            // What values are we going to plot?
            int problemSize = (int)SizeUpDown.Value;
            int iterations = (int)DataPointsUpDown.Value;
            EfficiencyChart.Titles["InfoTitle"].Text = String.Format("Size: {0}, Iterations: {1}", problemSize, iterations);
            int[] x_series = Enumerable.Range(1, problemSize).ToArray();

            // Update
            ChartArea chartArea = EfficiencyChart.ChartAreas[0];
            if (chartCombo.SelectedIndex == (int)ComboEnum.BOTH)
            {
                // Formatting
                EfficiencyChart.Titles["Title"].Text = "Size vs Basic Ops and Time";

                // Enable secondary
                secondary_toggle(true);

                // Bind to the secondary axis
                double[] y1 = (from size in Enumerable.Range(1, problemSize) select MedianOps.countBasics(size, iterations)).ToArray();
                axis_bind(y1, "Basic Ops", "Count", AxisType.Primary);
                double[] y2 = (from size in Enumerable.Range(1, problemSize) select MedianOps.countTime(size, iterations)).ToArray();
                axis_bind(y2, "Time", "Time (us)", AxisType.Secondary);
            }
            else
            {
                // Labelling
                String subtitle = (chartCombo.SelectedIndex == (int)ComboEnum.BASIC) ? "Basic Ops" : "Time";
                EfficiencyChart.Titles["Title"].Text = String.Format("Size vs {0}", subtitle);
                String axistitle = (chartCombo.SelectedIndex == (int)ComboEnum.BASIC) ? "Count" : "Time [us]";

                // Toggle off the secondary axis
                secondary_toggle(false);

                // Bind to the secondary axis
                double[] y1 = (chartCombo.SelectedIndex == (int)ComboEnum.BASIC) ?
                     (from size in Enumerable.Range(1, problemSize) select MedianOps.countBasics(size, iterations)).ToArray() :
                     (from size in Enumerable.Range(1, problemSize) select MedianOps.countTime(size, iterations)).ToArray();
                bool bf = (chartCombo.SelectedIndex == (int)ComboEnum.BASIC);
                axis_bind(y1, subtitle, axistitle, AxisType.Primary);
            }
        }

        // Save functionality
        private void save_chart(object sender, EventArgs e)
        {
            // Open a save box
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.InitialDirectory = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Directory.GetCurrentDirectory()))) + "\\Charts\\";
            saveDialog.Title = "Save Chart";
            saveDialog.FileName = "mychart";
            saveDialog.DefaultExt = "jpg";
            saveDialog.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                // Save chart
                MemoryStream myStream = new MemoryStream();
                EfficiencyChart.Serializer.Save(myStream);

                // Save from the chart object itself
                EfficiencyChart.SaveImage(saveDialog.FileName, ChartImageFormat.Png);
                
                /* Get a consistent output */
                // Fonts
                float fontscaler = 0.6f;
                EfficiencyChart.Titles["Title"].Font = new Font("Arial", 40 * fontscaler, FontStyle.Bold);
                EfficiencyChart.Titles["InfoTitle"].Font = new Font("Arial", 22 * fontscaler);
                ChartArea chartArea = EfficiencyChart.ChartAreas[0];
                Font tickFont = new Font("Arial", 24 * fontscaler, FontStyle.Bold);
                chartArea.AxisX.LabelStyle.Font = tickFont;
                chartArea.AxisY.LabelStyle.Font = tickFont;
                chartArea.AxisY2.LabelStyle.Font = tickFont;
                Font labelFont = new Font("Arial", 28 * fontscaler, FontStyle.Bold);
                chartArea.AxisX.TitleFont = labelFont;
                chartArea.AxisY.TitleFont = labelFont;
                chartArea.AxisY2.TitleFont = labelFont;
                EfficiencyChart.Legends[0].Font = new Font("Arial", 26 * fontscaler);

                // Higher resolution
                int newWidth = 1600;
                int newHeight = 900;
                EfficiencyChart.Width = newWidth;
                EfficiencyChart.Height = newHeight;

                // Save to a bitmap
                Bitmap bmp = new Bitmap(newWidth, newHeight);
                EfficiencyChart.DrawToBitmap(bmp, new Rectangle(0, 0, newWidth, newHeight));
                bmp.Save(saveDialog.FileName.Split('.')[0] + "-hires.png");

                // Reload chart
                EfficiencyChart.Serializer.Load(myStream);
            }
        }
    }
}
