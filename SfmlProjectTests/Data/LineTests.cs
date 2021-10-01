using SFMLTest.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SFMLTest.Data.Tests {
    [TestClass()]
    public class LineTests {

        [TestMethod()]
        public void LineConstructorTest1() {
            Line line = new Line(new Point(1, 2), new Point(3, 4));
            Assert.AreEqual(1, line.PointA.X);
            Assert.AreEqual(2, line.PointA.Y);
            Assert.AreEqual(3, line.PointB.X);
            Assert.AreEqual(4, line.PointB.Y);
        }

        [TestMethod()]
        public void LineConstructorTest2() {
            Line line = new Line(1, 2, 3, 4);
            Assert.AreEqual(1, line.PointA.X);
            Assert.AreEqual(2, line.PointA.Y);
            Assert.AreEqual(3, line.PointB.X);
            Assert.AreEqual(4, line.PointB.Y);
        }

        private class PointDataSource : Attribute, ITestDataSource {
            public IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Line(1, 1, 2, 2), new Point(4f, 4f), "Point far away." };
                yield return new object[] { false, new Line(1, 1, 2, 2), new Point(1f, 3f), "Point far away but with dimensional overlap on edge." };
                yield return new object[] { false, new Line(1, 1, 2, 2), new Point(1.5f, 3f), "Point far away but with dimensional overlap in the middle." };
                yield return new object[] { false, new Line(1, 1, 2, 2), new Point(1.6f, 1.5f), "Point close but not on the line." };
                yield return new object[] { false, new Line(1, 1, 2, 2), new Point(1f, 1.1f), "Point close to endpoint but not on the line." };
                yield return new object[] { true, new Line(1, 1, 2, 2), new Point(2f, 2f), "Point on endpoint of line." };
                yield return new object[] { true, new Line(1, 1, 2, 2), new Point(1.3f, 1.3f), "Point in the middle of the line." };
            }

            public string GetDisplayName(MethodInfo methodInfo, object[] data) {
                return string.Format(CultureInfo.CurrentCulture, "{0} : {1}", methodInfo.Name, data[^1]);
            }
        }

        [DataTestMethod]
        [PointDataSource]
        public void IntersectsPointTest(bool result, Line line, Point point, string name) {
            Assert.AreEqual(result, line.Intersects(point));
        }

        private class LineDataSource : Attribute, ITestDataSource {
            public IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Line(1, 1, 2, 2), new Line(3, 4, 4, 3), "Two completely separate lines without dimensional overlap." };
                yield return new object[] { false, new Line(1, 1, 4, 4), new Line(1, 2, 0, 3), "Two lines away from each other but 'touching' in one dimension." };
                yield return new object[] { false, new Line(1, 3, 5, 2), new Line(2, 1, 4, 2), "Two lines next to each other but not touching." };
                yield return new object[] { false, new Line(1, 1, 4, 4), new Line(2, 3, 0, 4), "Two lines away from each other but 'overlapping' in one dimension." };
                yield return new object[] { false, new Line(1, 1, 4, 1), new Line(1, 2, 5, 2), "Two horizontal, parallel lines." };
                yield return new object[] { false, new Line(1, 1, 1, 4), new Line(2, 1, 2, 5), "Two vertical, parallel lines." };
                yield return new object[] { false, new Line(1, 1, 1, 4), new Line(2, 1, 2, 5), "Two vertical, parallel lines." };
                yield return new object[] { true, new Line(1, 1, 2, 2), new Line(1, 2, 2, 1), "Two lines meeting in the middle." };
                yield return new object[] { true, new Line(1, 1, 2, 2), new Line(3, 3, 2, 2), "Two lines touching at the end." };
                yield return new object[] { false, new Line(-5, -3, -1, -4), new Line(-4, -5, -2, -4), "Two lines next to each other but not touching." };
            }

            public string GetDisplayName(MethodInfo methodInfo, object[] data) {
                return string.Format(CultureInfo.CurrentCulture, "{0} : {1}", methodInfo.Name, data[^1]);
            }
        }

        [DataTestMethod]
        [LineDataSource]
        public void IntersectsLineTest(bool result, Line lineOne, Line lineTwo, string name) {
            Assert.AreEqual(result, lineOne.Intersects(lineTwo));
            Assert.AreEqual(result, lineTwo.Intersects(lineOne));
        }
    }
}