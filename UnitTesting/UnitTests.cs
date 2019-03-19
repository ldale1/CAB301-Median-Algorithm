using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MedianAlgorithm;

namespace MedianTests
{
    [TestClass]
    public class ValueTests
    {
        [TestMethod]
        public void evenSequence()
        {
            double[] simpleMedian = new double[] { 1, 2, 3, 4, 5 };
            double output = Median.BruteForceMedian(simpleMedian);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void oddSequence()
        {
            double[] simpleMedian = new double[] { 1, 2, 3, 4, 5, 6 };
            double output = Median.BruteForceMedian(simpleMedian);
            double expected = 3.5;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void singleSequence()
        {
            double[] simpleMedian = new double[] { 1 };
            double output = Median.BruteForceMedian(simpleMedian);
            double expected = 1;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void floatSequence()
        {
            double[] simpleMedian = new double[] { 0.1, 0.2, 0.3, 0.4, 0.5 };
            double output = Median.BruteForceMedian(simpleMedian);
            double expected = 0.3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void reverseSequence()
        {
            double[] simpleMedian = new double[] { 5, 4, 3, 2, 1 };
            double output = Median.BruteForceMedian(simpleMedian);
            double expected = 3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void negativeSequence()
        {
            double[] simpleMedian = new double[] { -1, -2, -3, -4, -5 };
            double output = Median.BruteForceMedian(simpleMedian);
            double expected = -3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void negativeSequenceReverse()
        {
            double[] simpleMedian = new double[] { -1, -2, -3, -4, -5 };
            double output = Median.BruteForceMedian(simpleMedian);
            double expected = -3;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void frontLoaded()
        {
            double[] simpleMedian = new double[] { 1, 1, 1, 1, 5 };
            double output = Median.BruteForceMedian(simpleMedian);
            double expected = 1;

            Assert.AreEqual(expected, output);
        }

        [TestMethod]
        public void backLoaded()
        {
            double[] simpleMedian = new double[] { 1, 1, 1, 1, 5 };
            double output = Median.BruteForceMedian(simpleMedian);
            double expected = 1;

            Assert.AreEqual(expected, output);
        }        
    }
}
