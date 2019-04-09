using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MedianAlgorithm;


namespace ChartApp
{
    static class ChartProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        { 
            double[] simpleMedian = new double[] { 1, 2, 3, 4, 5 };
            double output = Median.BruteForceMedian(simpleMedian);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ChartForm());
        }
    }
}
