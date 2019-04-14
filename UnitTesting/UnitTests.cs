using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MedianAlgorithm;

namespace MedianTests
{
    [TestClass]
    public class ValueTests
    {

        /// Returns the median value in a given array A of n numbers. 
        /// This i the kth element, where k =|n/2|, if the array was sorte

        //An empty array that throws an exception
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Array must contain some values")]
        public void emptySequence()
        {
            double[] testInput = new double[] { };
            double output = Median.BruteForceMedian(testInput);
        }

        //The only element of the array equals the median
        [TestMethod]
        public void singleSequence()
        {
            double[] testInput = new double[] { 1 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 1;

            Assert.AreEqual(expected, output);
        }

        //Positive, ascending sequence with an odd length
        [TestMethod]
        public void oddSequence()
        {
            double[] testInput = new double[] { 1, 2, 3, 4, 5 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        //Positive, ascending sequence with an even length
        [TestMethod]
        public void evenSequence()
        {
            double[] testInput = new double[] { 1, 2, 3, 4, 5, 6 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        //Negative, ascending sequence
        [TestMethod]
        public void negativeSequenceAscending()
        {
            double[] testInput = new double[] { -5, -4, -3, -2, -1 };
            double output = Median.BruteForceMedian(testInput);
            double expected = -3;

            Assert.AreEqual(expected, output);
        }

        // Positive, descending sequence
        [TestMethod]
        public void sequenceDescending()
        {
            double[] testInput = new double[] { 5, 4, 3, 2, 1 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        //Negative, descending sequence
        [TestMethod]
        public void negativeSequence()
        {
            double[] testInput = new double[] { -1, -2, -3, -4, -5 };
            double output = Median.BruteForceMedian(testInput);
            double expected = -3;

            Assert.AreEqual(expected, output);
        }

        //Sequence mixed with off and even integers
        [TestMethod]
        public void mixedSequence()
        {
            double[] testInput = new double[] { -3, -2, -1, 0, 1, 2, 3 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 0;

            Assert.AreEqual(expected, output);
        }


        //Sequence with floats
        [TestMethod]
        public void floatSequence()
        {
            double[] testInput = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 0.3;

            Assert.AreEqual(expected, output);
        }

        // Homogenous sequence
        [TestMethod]
        public void homogenous()
        {
            double[] testInput = new double[] { 3, 3, 3, 3, 3 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        //None Lower
        [TestMethod]
        public void noneLower()
        {
            double[] testInput = new double[] { 1, 1, 1, 2, 2 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 1;

            Assert.AreEqual(expected, output);
        }

        //None greater
        [TestMethod]
        public void noneGreater()
        {
            double[] testInput = new double[] { 1, 1, 2, 2, 2 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 2;

            Assert.AreEqual(expected, output);
        }

        // Sequence with median at start
        [TestMethod]
        public void startMedian()
        {
            double[] testInput = new double[] { 3, 1, 2, 4, 5 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        // Sequence with median at end
        [TestMethod]
        public void endMedian()
        {
            double[] testInput = new double[] { 1, 2, 4, 5, 3 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        //Random even length sequence with a mix of positive and negative
        [TestMethod]
        public void randomMedian()
        {
            double[] testInput = new double[] { -4, 2, 3, -2, 5, -1, 0, 4, 1, -3 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 0;

            Assert.AreEqual(expected, output);
        }


        //Sequence front loaded with a large number
        [TestMethod]
        public void frontLoaded()
        {
            double[] testInput = new double[] { 5, 1, 1, 1, 1 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 1;

            Assert.AreEqual(expected, output);
        }

        //Sequence back loaded with a large number
        [TestMethod]
        public void backLoaded()
        {
            double[] testInput = new double[] { 1, 1, 1, 1, 5 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 1;

            Assert.AreEqual(expected, output);
        }        
    }
}
