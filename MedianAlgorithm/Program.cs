using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MedianAlgorithm
{
    public class MedianOps
    {
        static Random rnd = new Random();

        private static double[] randomArray(int size, int lower, int upper)
        {
            return (from number in Enumerable.Range(0, size) select (double)rnd.Next(lower, upper + 1)).ToArray();
        }

        public static double countBasics(int size, int iterations)
        {
            int randUpper = (int)Math.Floor(size / 2.0);
            int randLower = -randUpper;

            return (from number in Enumerable.Range(0, iterations)
                    select (double) Median.BruteForceMedianCount(randomArray(size, randLower, randUpper)) / iterations
                    ).Sum(); // Could user .Average() but careful for int.MaxSize
        }

        public static double countTime(int size, int iterations)
        {
            DateTime start, end;
            int randUpper = (int)Math.Floor(size / 2.0);
            int randLower = -randUpper;
            double output;
            double[] A;
            double executionTime = 0;
            for (int i = 0; i < iterations; i++)
            {
                A = randomArray(size, randLower, randUpper);

                // Time over algorithm
                start = DateTime.Now;
                output = Median.BruteForceMedian(A);
                end = DateTime.Now;

                // Aggreggate Time
                executionTime += (end - start).Ticks / (10 *iterations);

            }
            return executionTime;

        }

        static void Main(string[] args)
        { 
            double[] A = Enumerable.Range(1, 10).ToArray().Select(x => (double)x).ToArray();

            Console.WriteLine("TIMED:");
            Console.WriteLine("{0} [ms]\n", countTime(20, 250));

            Console.WriteLine("BASICS:");
            Console.WriteLine("{0} [ops]\n", countBasics(100, 100));

            double[] x_series = new double[] { 0, 0, 0, 1, 2, 4 };
            int startOh = x_series.ToList().LastIndexOf(0);
            x_series = x_series.Skip(startOh + 1).Take(x_series.Length - startOh).ToArray();

            double g = 2;
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

            Console.WriteLine(String.Join(",", vals) + " ---- " + g.ToString());

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
