using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedianAlgorithm
{
    public class Median
    {
        /// <summary>
        /// Brute force method to find the median of an array  <paramref name="A"/> of n numbers.
        /// </summary>
        /// <param name="A">Array to find the median of</param>
        /// <returns>
        /// Returns the median value in a given array <paramref name="A"/> of n numbers.
        /// This i the kth element, where k =|n/2|, if the array was sorted
        /// </returns>
        public static int BruteForceMedian(int[] A)
        {
            int n = A.Length;
            int k = Math.Abs(n / 2);

            Console.WriteLine(string.Format("{0}, {1}", n, k));

            for (int i = 0; i < n; i++)
            {
                int numsmaller = 0; // How many elements are smaller than A[i]
                int numequal = 0;   // How many elements are equal to A[i]

                for (int j = 0; j < n; j++)
                {
                    if (A[j] < A[i])
                    {
                        numsmaller++;
                    }
                    else
                    {
                        if (A[j] == A[i])
                        {
                            numequal++;
                        }
                    }
                }

                if ((numsmaller < k) && (k <= (numsmaller + numequal)))
                {
                    return A[i];
                }
            }
            return -1;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            int[] simpleMedian = new int[] { 1, 2, 3, 4, 5 };
            int output = Median.BruteForceMedian(simpleMedian);

            Console.ReadLine();
        }
    }
}
