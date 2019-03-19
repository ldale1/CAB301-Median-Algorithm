using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MedianAlgorithm;

namespace MedianTests
{
    [TestClass]
    public class ValueTests
    {
        [TestMethod]
        public void basicMedian()
        {
            int[] simpleMedian = new int[] { 1, 2, 3, 4, 5 };
            int output = Median.BruteForceMedian(simpleMedian);
            int expected = 3;


            Assert.AreEqual(expected, output);
        }
    }
}
