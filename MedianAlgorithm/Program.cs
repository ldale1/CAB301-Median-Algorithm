using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedianAlgorithm
{
    class Program
    {
        static void Main(string[] args)
        { 


            double[] A = Enumerable.Range(1, 10).ToArray().Select(x => (double)x).ToArray();
            double basics = Median.BruteForceMedianCount(A);

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
