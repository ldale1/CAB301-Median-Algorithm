using System;
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
                executionTime += (end - start).Ticks;

            }
            return executionTime / (10 * iterations);

        }

        static void Main(string[] args)
        { 
            double[] A = Enumerable.Range(1, 10).ToArray().Select(x => (double)x).ToArray();

            Console.WriteLine("TIMED:");
            Console.WriteLine("{0} [ms]\n", countTime(20, 250));

            Console.WriteLine("BASICS:");
            Console.WriteLine("{0} [ops]\n", countBasics(100, 100));
        }
    }
}
