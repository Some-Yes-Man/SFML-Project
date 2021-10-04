using Microsoft.VisualStudio.TestTools.UnitTesting;
using SfmlProjectTests;
using System.Collections.Generic;
using System.Reflection;

namespace SfmlProject.Geometry {
    /**
     * Fantastic tool to set up those unit tests.
     * https://www.geogebra.org/geometry
     **/
    [TestClass()]
    public class LineTests {
        [TestMethod()]
        public void LineTest() {
            Line line = new Line(new Point(1, 2), new Point(3, 4));
            Assert.AreEqual(1, line.Points[0].X);
            Assert.AreEqual(2, line.Points[0].Y);
            Assert.AreEqual(3, line.Points[1].X);
            Assert.AreEqual(4, line.Points[1].Y);
        }

        [TestMethod()]
        public void CollisionWithPointCalledCorrectly() {
            Line line = new Line(new Point(1, 1), new Point(2, 2));
            Point point1 = new Point(1.3f, 1.3f);
            Assert.IsTrue(line.Collides(point1));
            Point point2 = new Point(1.3f, 1.4f);
            Assert.IsFalse(line.Collides(point2));
        }

        private class LineDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
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
        }

        [DataTestMethod]
        [LineDataSource]
        public void IntersectsLineTest(bool result, Line lineOne, Line lineTwo, string name) {
            Assert.AreEqual(result, lineOne.Collides(lineTwo));
            Assert.AreEqual(result, lineTwo.Collides(lineOne));
        }

        private class LineIntersectsTriangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(5, 5), new Point(6, 6)), "Line far away." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(4, 4), new Point(6, 6)), "Line with dimensional corner overlap." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(3, 3), new Point(6, 6)), "Line with dimensional overlap." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(3, 2.5f), new Point(6, 6)), "Line inside bounding box." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(4, 1), new Point(6, 6)), "Line with one point on corner of triangle." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(3, 2), new Point(6, 6)), "Line with one point on triangle edge." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(4, 4), new Point(4, -1)), "Line intersecting a corner." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(2, 2), new Point(6, 6)), "Line intersecting one line." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(2, 0), new Point(6, 6)), "Line intersecting two lines." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(2, 2), new Point(3, 1.5f)), "Line inside triangle." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Line(new Point(0, 5), new Point(6, -1)), "Line co-linear to an edge but longer." };
            }
        }

        [DataTestMethod]
        [LineIntersectsTriangleDataSource]
        public void LineIntersectsTriangleTest(bool result, Triangle triangle, Line line, string name) {
            Assert.AreEqual(result, line.Collides(triangle));
            Assert.AreEqual(result, triangle.Collides(line));
        }

        private class LineIntersectsRectangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(5, 5), new Point(6, 6)), "Line far away." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(4, 4), new Point(6, 6)), "Line with dimensional corner overlap." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(5, 5), new Point(6, 6)), "Line with dimensional overlap." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(3, 4), new Point(6, 6)), "Line with two points of dimensional overlap on different sides." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(4, 3), new Point(6, 6)), "Line with point on corner." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(3, 3), new Point(6, 6)), "Line with point on edge." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(2, 2), new Point(6, 6)), "Line with one point in rectangle." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(2, 2), new Point(3, 2)), "Line inside rectangle." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(3, 4), new Point(5, 2)), "Line intersecting corner." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(3, 4), new Point(5, 0)), "Line intersecting two neighboring edges." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(0, 2), new Point(5, 2)), "Line intersecting two opposite edges." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(0, 3), new Point(5, 3)), "Line horizontal and co-linear to an edge but longer." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 3)), new Line(new Point(1, 0), new Point(1, 4)), "Line vertical and co-linear to an edge but longer." };
            }
        }

        [DataTestMethod]
        [LineIntersectsRectangleDataSource]
        public void LineIntersectsRectangleTest(bool result, Rectangle rectangle, Line line, string name) {
            Assert.AreEqual(result, line.Collides(rectangle));
            Assert.AreEqual(result, rectangle.Collides(line));
        }

        private class LineIntersectsCircleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Circle(new Point(2, 2), 1), new Line(new Point(5, 5), new Point(6, 6)), "Line far away." };
                yield return new object[] { false, new Circle(new Point(2, 2), 1), new Line(new Point(4, 3), new Point(6, 6)), "Line with dimensional corner overlap" };
                yield return new object[] { false, new Circle(new Point(2, 2), 1), new Line(new Point(4, 2), new Point(6, 6)), "Line with dimensional overlap." };
                yield return new object[] { false, new Circle(new Point(2, 2), 1), new Line(new Point(2.8f, 2.8f), new Point(6, 6)), "Line with one corner inside bounding box." };
                yield return new object[] { true, new Circle(new Point(2, 2), 1), new Line(new Point(0, 4), new Point(1.29289f, 2.7071f)), "Line with one corner on circle." };
                yield return new object[] { true, new Circle(new Point(2, 2), 1), new Line(new Point(1.5f, 2.5f), new Point(2.5f, 2)), "Line inside circle." };
                yield return new object[] { true, new Circle(new Point(2, 2), 1), new Line(new Point(2.2f, 2.5f), new Point(2.7f, 2.5f)), "Line inside circle, no overlap with center." };
                yield return new object[] { true, new Circle(new Point(2, 2), 1), new Line(new Point(2, 2), new Point(3.5f, 0.5f)), "Line with point on the center of circle." };
                yield return new object[] { true, new Circle(new Point(2, 2), 1), new Line(new Point(0.5f, 0.5f), new Point(1.5f, 1.5f)), "Line with one point inside circle." };
            }
        }

        [DataTestMethod]
        [LineIntersectsCircleDataSource]
        public void LineIntersectsCircleTest(bool result, Circle circle, Line line, string name) {
            Assert.AreEqual(result, line.Collides(circle));
            Assert.AreEqual(result, circle.Collides(line));
        }

        private class LineIntersectsPolygonDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(4f, 4f), new Point(5f, 5f)), "Line far away." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(4f, 3f), new Point(5f, 5f)), "Line with dimensional corner overlap." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(4f, 2f), new Point(5f, 5f)), "Line with dimensional overlap." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(-2f, 4f), new Point(0f, 2f)), "Line within bounding box." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(0f, -1f), new Point(1f, 0f)), "Line within concave 'bay'." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(2f, 3f), new Point(5f, 5f)), "Line on corner." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(2f, 2f), new Point(5f, 5f)), "Line on edge." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(1f, -1f), new Point(3f, -1f)), "Line crossing corner." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(1f, 2f), new Point(0f, 3f)), "Line crossing one edge." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(0f, 0f), new Point(-1f, 2f)), "Line crossing two edges." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(3f, -1f), new Point(-1f, 2f)), "Line crossing more edges." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(0.6f, 1.6f), new Point(1.4f, 1.6f)), "Line inside polygon." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Line(new Point(3f, -3f), new Point(0f, 3f)), "Line on line." };
            }
        }

        [DataTestMethod]
        [LineIntersectsPolygonDataSource]
        public void LineIntersectsPolygonTest(bool result, Polygon polygon, Line line, string name) {
            Assert.AreEqual(result, line.Collides(polygon));
            Assert.AreEqual(result, polygon.Collides(line));
        }
    }
}
