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
            double executionTime = 0;
            double[] A;

            int randUpper = (int)Math.Floor(size / 2.0);
            int randLower = -randUpper;

            for (int i = 0; i < iterations; i++)
            {
                A = randomArray(size, randLower, randUpper);

                // Time over algorithm
                start = DateTime.Now;
                Median.BruteForceMedian(A);
                end = DateTime.Now;

                // Aggreggate Time
                executionTime += (end - start).TotalMilliseconds / iterations;
            }
            return executionTime;
        }

        static void Main(string[] args)
        { 
            double[] A = Enumerable.Range(1, 10).ToArray().Select(x => (double)x).ToArray();

            Console.WriteLine("TIMED:");
            Console.WriteLine("{0} [ms]\n", countTime(1000, 1000));

            Console.WriteLine("BASICS:");
            Console.WriteLine("{0} [ops]", countBasics(1000, 1000));

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
