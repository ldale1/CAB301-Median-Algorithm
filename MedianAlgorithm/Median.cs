using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedianAlgorithm
{
    public class Median
    {
        /// <summary> Brute force method to find the median of an array  <paramref name="A"/> of n numbers by iterating over each element and counting other elements before in order. </summary>
        /// <param name="A">Array to find the median of</param>
        /// <returns>  Returns the median value in a given array <paramref name="A"/> of n numbers. This i the kth element, where k =|n/2|, if the array was sorted </returns>
        public static double BruteForceMedian(double[] A)
        {
            int n = A.Length;
            double k = Math.Ceiling(n / 2.0);

            // Look at each element in the input array
            for (int i = 0; i < n; i++)
            {
                int numsmaller = 0; // How many elements are smaller than A[i]
                int numequal = 0;   // How many elements are equal to A[i]

                // Compare sample element with all other array elements
                for (int j = 0; j < n; j++)
                {
                    // Is the element we are comparing to smaller than the sample element?
                    if (A[j] < A[i])
                    {
                        numsmaller++;
                    }
                    else
                    {
                        // Otherwise, is the element we are comparing equal to the sample element?
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
            throw new ArgumentException("Array must contain some values");
        }



        /// <summary> Brute force method to find the median of an array  <paramref name="A"/> of n numbers. Counts basic operations. </summary>
        /// <param name="A">Array to find the median of</param>
        /// <returns>  Returns the count of basic operations performed </returns>
        public static double BruteForceMedianCount(double[] A)
        {
            int counter = 0;
            int n = A.Length;
            double k = Math.Abs(n / 2.0);

            for (int i = 0; i < n; i++)
            {
                int numsmaller = 0; // How many elements are smaller than A[i]
                int numequal = 0;   // How many elements are equal to A[i]

                for (int j = 0; j < n; j++)
                {
                    counter++;
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
                    return counter;
                }
            }
            throw new ArgumentException("Something went wrong");
        }


        /// <summary> Brute force method to find the median of an array  <paramref name="A"/> of n numbers. Counts time. </summary>
        /// <param name="A">Array to find the median of</param>
        /// <returns>  Returns the time taken for the function to execute </returns>
        public static double BruteForceMedianTime(double[] A)
        {
            Stopwatch sw = new Stopwatch();
            

            int n = A.Length;
            double k = Math.Abs(n / 2.0);

            for (int i = 0; i < n; i++)
            {
                int numsmaller = 0; // How many elements are smaller than A[i]
                int numequal = 0;   // How many elements are equal to A[i]

                for (int j = 0; j < n; j++)
                {
                    sw.Start();
                    if (A[j] < A[i])
                    {
                        sw.Stop();
                        numsmaller++;
                    }
                    else
                    {
                        sw.Stop();
                        if (A[j] == A[i])
                        {
                            numequal++;
                        }
                    }
                }

                if ((numsmaller < k) && (k <= (numsmaller + numequal)))
                {
                    return sw.ElapsedMilliseconds;
                }
            }
            throw new ArgumentException("Something went wrong");
        }
    }
}