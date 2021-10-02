using Microsoft.VisualStudio.TestTools.UnitTesting;
using SfmlProject.Geometry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SfmlProject.Data.Tests {
    [TestClass()]
    public class GeometryUtilsTests {
        private class PointDataSource : Attribute, ITestDataSource {
            public IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(4f, 4f), "Point far away." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(1f, 3f), "Point far away but with dimensional overlap on edge." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(1.5f, 3f), "Point far away but with dimensional overlap in the middle." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(1.6f, 1.5f), "Point close but not on the line." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(1f, 1.1f), "Point close to endpoint but not on the line." };
                yield return new object[] { true, new Line(new Point(1, 1), new Point(2, 2)), new Point(2f, 2f), "Point on endpoint of line." };
                yield return new object[] { true, new Line(new Point(1, 1), new Point(2, 2)), new Point(1.3f, 1.3f), "Point in the middle of the line." };
            }

            public string GetDisplayName(MethodInfo methodInfo, object[] data) {
                return string.Format(CultureInfo.CurrentCulture, "{0} : {1}", methodInfo.Name, data[^1]);
            }
        }

        [DataTestMethod]
        [PointDataSource]
        public void IntersectsPointTest(bool result, Line line, Point point, string name) {
            Assert.AreEqual(result, CollisionHelper.PointOnLine(point, line));
        }

        [TestMethod()]
        public void PointInTriangleTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void PointInTriangleTest1() {
            Assert.Fail();
        }

        [TestMethod()]
        public void PointInCircleTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void LineIntersectsTriangleTest() {
            Assert.Fail();
        }
    }
}