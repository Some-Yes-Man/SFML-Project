using Microsoft.VisualStudio.TestTools.UnitTesting;
using SfmlProject.Geometry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SfmlProject.Data.Tests {
    [TestClass()]
    public class LineTests {

        [TestMethod()]
        public void LineTest() {
            Line line = new Line(new Point(1, 2), new Point(3, 4));
            Assert.AreEqual(1, line.PointA.X);
            Assert.AreEqual(2, line.PointA.Y);
            Assert.AreEqual(3, line.PointB.X);
            Assert.AreEqual(4, line.PointB.Y);
        }

        [TestMethod()]
        public void CollisionWithPointCalledCorrectly() {
            Line line = new Line(new Point(1, 1), new Point(2, 2));
            Point point = new Point(1.3f, 1.3f);
            Assert.IsTrue(line.Collides(point));
        }

        private class LineDataSource : Attribute, ITestDataSource {
            public IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Line(new Point(3, 4), new Point(4, 3)), "Two completely separate lines without dimensional overlap." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(4, 4)), new Line(new Point(1, 2), new Point(0, 3)), "Two lines away from each other but 'touching' in one dimension." };
                yield return new object[] { false, new Line(new Point(1, 3), new Point(5, 2)), new Line(new Point(2, 1), new Point(4, 2)), "Two lines next to each other but not touching." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(4, 4)), new Line(new Point(2, 3), new Point(0, 4)), "Two lines away from each other but 'overlapping' in one dimension." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(4, 1)), new Line(new Point(1, 2), new Point(5, 2)), "Two horizontal, parallel lines." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(1, 4)), new Line(new Point(2, 1), new Point(2, 5)), "Two vertical, parallel lines." };
                yield return new object[] { false, new Line(new Point(-5, -3), new Point(-1, -4)), new Line(new Point(-4, -5), new Point(-2, -4)), "Two lines next to each other but not touching." };
                yield return new object[] { true, new Line(new Point(1, 1), new Point(2, 2)), new Line(new Point(1, 2), new Point(2, 1)), "Two lines meeting in the middle." };
                yield return new object[] { true, new Line(new Point(1, 1), new Point(2, 2)), new Line(new Point(3, 3), new Point(2, 2)), "Two lines touching at the end." };
                yield return new object[] { true, new Line(new Point(1, 1), new Point(3, 3)), new Line(new Point(1.9f, 2.1f), new Point(3, 1)), "Two lines at an 45 angle, barely intersecting." };
                yield return new object[] { true, new Line(new Point(0, 2), new Point(2, 2)), new Line(new Point(1, 1), new Point(1, 3)), "One horizontal, one vertical line intersecting." };
            }

            public string GetDisplayName(MethodInfo methodInfo, object[] data) {
                return string.Format(CultureInfo.CurrentCulture, "{0} : {1}", methodInfo.Name, data[^1]);
            }
        }

        [DataTestMethod]
        [LineDataSource]
        public void IntersectsLineTest(bool result, Line lineOne, Line lineTwo, string name) {
            Assert.AreEqual(result, lineOne.Collides(lineTwo));
            Assert.AreEqual(result, lineTwo.Collides(lineOne));
        }

        [TestMethod()]
        public void CollisionWithTriangleCalledCorrectly() {
            Line line = new Line(new Point(1, 1), new Point(2, 2));
            Triangle triangle = new Triangle(new Point(1, 2), new Point(2, 1), new Point(4, 4));
            Assert.IsTrue(line.Collides(triangle));
        }

        [TestMethod()]
        public void CollisionWithCircleCalledCorrectly() {
            Line line = new Line(new Point(1, 1), new Point(3, 1));
            Circle circle = new Circle(new Point(2, 2), 1.3f);
            Assert.IsTrue(line.Collides(circle));
        }

        [TestMethod()]
        public void CollisionWithShapeCalledCorrectly() {
            Line line = new Line(new Point(1, 1), new Point(2, 2));
            Polygon shape = new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 4), new Point(2, -3));
            Assert.IsTrue(line.Collides(shape));
        }
    }
}