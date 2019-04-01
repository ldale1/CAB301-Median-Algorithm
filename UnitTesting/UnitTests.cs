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



        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Array must contain some values")]
        public void emptySequence()
        {
            double[] testInput = new double[] { };
            double output = Median.BruteForceMedian(testInput);
        }


        [TestMethod]
        public void singleSequence()
        {
            double[] testInput = new double[] { 1 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 1;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void oddSequence()
        {
            double[] testInput = new double[] { 1, 2, 3, 4, 5 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void evenSequence()
        {
            double[] testInput = new double[] { 1, 2, 3, 4, 5, 6 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 3.5;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void floatSequence()
        {
            double[] testInput = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 0.3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void sequenceDescending()
        {
            double[] testInput = new double[] { 5, 4, 3, 2, 1 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void negativeSequence()
        {
            double[] testInput = new double[] { -1, -2, -3, -4, -5 };
            double output = Median.BruteForceMedian(testInput);
            double expected = -3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void negativeSequenceAscending()
        {
            double[] testInput = new double[] { -5, -4, -3, -2, -1 };
            double output = Median.BruteForceMedian(testInput);
            double expected = -3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void mixedSequence()
        {
            double[] testInput = new double[] { -3, -2, -1, 0, 1, 2, 3 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 0;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void frontLoaded()
        {
            double[] testInput = new double[] { 5, 1, 1, 1, 1 };
            double output = Median.BruteForceMedian(testInput);
            double expected = 1;

            Assert.AreEqual(expected, output);
        }

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
