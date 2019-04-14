using System;
using System.Diagnostics;
using System.Linq;


namespace MedianAlgorithm
{
    public class MedianOps
    {
        static Random rnd = new Random();

        // Return a random array of doubles between two numbers
        private static double[] randomArray(int size, int lower, int upper)
        {
            return (from number in Enumerable.Range(0, size) select (double)rnd.Next(lower, upper + 1)).ToArray();
        }

        public static double countBasics(int size, int iterations)
        {
            int randUpper = (int)Math.Floor(size / 2.0);
            return (from number in Enumerable.Range(0, iterations)
                    select (double) Median.BruteForceMedianCount(randomArray(size, -randUpper, randUpper)) / iterations
                    ).Sum(); // Could use .Average() but careful for int.MaxSize
        }

        public static double countTime(int size, int iterations)
        {
            Stopwatch stopWatch = new Stopwatch();
            int randUpper = (int)Math.Floor(size / 2.0);
            double output;
            double[] A;
            for (int i = 0; i < iterations; i++)
            {
                A = randomArray(size, -randUpper, randUpper);

                // Time over algorithm
                stopWatch.Start();
                output = Median.BruteForceMedian(A);
                stopWatch.Stop();
            }
            return stopWatch.ElapsedTicks/(10*iterations);

        }

        static void Main(string[] args)
        { 
            double[] A = Enumerable.Range(1, 10).ToArray().Select(x => (double)x).ToArray();

            Console.WriteLine("TIMED:");
            Console.WriteLine("{0} [ms]\n", countTime(100, 100));

            Console.WriteLine("BASICS:");
            Console.WriteLine("{0} [ops]\n", countBasics(100, 100));
        }
    }
}
