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
            chartArea.AxisY2.IsMarginVisible = false;
            chartArea.AxisY2.Minimum = 0;
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
        private void axis_bind(double[] points, String subtitle, String axistitle, AxisType axisType, bool bf = true)
        {
            ChartArea chartArea = EfficiencyChart.ChartAreas[0];
            String seriesName = axisType == AxisType.Primary ? "y1" : "y2";
            // Max scale value
            double scale = Math.Min(Math.Pow(10, (int)Math.Log10(points.Max())), 100);
            Console.WriteLine(scale);
            int val = (int)(Math.Ceiling(points.Max() / scale) * scale);
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

            // Bestfit fit
            if (bf)
            {
                toggle_series(EfficiencyChart.Series[seriesName + "-oh"], true);
                toggle_series(EfficiencyChart.Series[seriesName + "-omega"], true);

                Tuple<int[], double[]> y1_oh = bestfit_oh(points);
                try
                {
                    EfficiencyChart.Series[seriesName + "-oh"].Points.DataBindXY(y1_oh.Item1, y1_oh.Item2);
                    EfficiencyChart.Series[seriesName + "-oh"].LegendText = String.Format("O({0})", subtitle);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }

                Tuple<int[], double[]> y1_omega = bestfit_omega(points);
                try { 
                    EfficiencyChart.Series[seriesName + "-omega"].Points.DataBindXY(y1_omega.Item1, y1_omega.Item2);
                    EfficiencyChart.Series[seriesName + "-omega"].LegendText = String.Format("Ω({0})", subtitle);
                } catch (Exception exc)
                {
                    Console.WriteLine(exc);
                }
            } else {
                toggle_series(EfficiencyChart.Series[seriesName + "-oh"], false);
                toggle_series(EfficiencyChart.Series[seriesName + "-omega"], false);
            }
        }

        // Upper bound
        private Tuple<int[], double[]> bestfit_oh(double[] x_series)
        {
            int startOh = Math.Max(x_series.ToList().IndexOf(x_series.Where(val => val > 0).ToArray()[0]), 0);
            double offset = x_series[startOh];
            x_series = x_series.Skip(startOh).Take(x_series.Length - startOh).Select(x => x - offset).ToArray();
            double g = 0.0002;
            int breakCounter = 10000;
            double[] vals;
            bool tuning;
            do {
                vals = Enumerable.Range(1, x_series.Length).Select(x => g * x * x + offset*0.2).ToArray();
                tuning = vals.Where(val => val < x_series[Array.IndexOf(vals, val)]).Any(); // While any point < x_series
                g += 0.0002;
                breakCounter--;
            } while (tuning && breakCounter > 0) ;
            return Tuple.Create(Enumerable.Range(startOh + 1, x_series.Length).ToArray(), vals.Select(x => x + offset).ToArray());
        }
        // Lower bound
        private Tuple<int[], double[]> bestfit_omega(double[] x_series)
        {
            int startOh = Math.Max(x_series.ToList().LastIndexOf(0), 0);
            x_series = x_series.Skip(startOh).Take(x_series.Length - startOh).ToArray();
            double g = 2;
            int breakCounter = 1000;
            double[] vals;
            bool tuning;
            do
            {
                vals = Enumerable.Range(1, x_series.Length).Select(x => g * x * x  - 1).ToArray();
                tuning = vals.Where(val => val > x_series[Array.IndexOf(vals, val)]).Any(); // While any point > x_series
                // Loop updating
                g -= 0.002;
                breakCounter--;
            } while (tuning && breakCounter > 0);

            Console.WriteLine(tuning);

            return Tuple.Create(Enumerable.Range(startOh + 1, x_series.Length).ToArray(), vals);
        }

        // Update the chart
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

                // Enable secondary
                secondary_toggle(true);

                // Bind to the secondary axis
                double[] y1 = (from size in Enumerable.Range(1, problemSize) select MedianOps.countBasics(size, iterations)).ToArray();
                double[] y2 = (from size in Enumerable.Range(1, problemSize) select MedianOps.countTime(size, iterations)).ToArray();

                axis_bind(y1, "Basic Ops", "Count", AxisType.Primary);
                axis_bind(y2, "Time", "Time (us)", AxisType.Secondary);
            }
            else
            {
                // Labelling
                String subtitle = (chartCombo.SelectedIndex == (int)ComboEnum.BASIC) ? "Basic Ops" : "Time";
                EfficiencyChart.Titles["MyTitle"].Text = String.Format("Size vs {0}", subtitle);
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
