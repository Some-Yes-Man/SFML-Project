using Microsoft.VisualStudio.TestTools.UnitTesting;
using SfmlProjectTests;
using System.Collections.Generic;
using System.Reflection;

namespace SfmlProject.Geometry.Tests {
    /**
     * Fantastic tool to set up those unit tests.
     * https://www.geogebra.org/geometry
     **/
    [TestClass()]
    public class PointTests {
        [TestMethod()]
        public void PointTest() {
            Point point = new Point(1.5f, -0.32f);
            Assert.AreEqual(1.5f, point.X);
            Assert.AreEqual(-0.32f, point.Y);
        }

        [TestMethod()]
        public void PointsCollide_When_PointsAreIdentical() {
            Point point1 = new Point(1.7f, -0.34f);
            Point point2 = new Point(1.7f, -0.34f);
            Assert.IsTrue(point1.Collides(point2));
            Assert.IsTrue(point2.Collides(point1));
        }

        [TestMethod()]
        public void PointsDontCollide_When_PointsAreDifferent() {
            Point point1 = new Point(1.7f, -0.34f);
            Point point2 = new Point(1.7f, -0.33f);
            Assert.IsFalse(point1.Collides(point2));
            Assert.IsFalse(point2.Collides(point1));
        }

        private class PointOnLineDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(4f, 4f), "Point far away and outside bounding box." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(1f, 3f), "Point far away but with dimensional overlap on edge." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(1.5f, 3f), "Point far away but with dimensional overlap in the middle." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(1.6f, 1.5f), "Point close but not on the line." };
                yield return new object[] { false, new Line(new Point(1, 1), new Point(2, 2)), new Point(1f, 1.1f), "Point close to endpoint but not on the line." };
                yield return new object[] { true, new Line(new Point(1, 1), new Point(2, 2)), new Point(2f, 2f), "Point on endpoint of line." };
                yield return new object[] { true, new Line(new Point(1, 1), new Point(2, 2)), new Point(1.3f, 1.3f), "Point somewhere on the line." };
            }
        }

        [DataTestMethod]
        [PointOnLineDataSource]
        public void PointOnLineTest(bool result, Line line, Point point, string name) {
            Assert.AreEqual(result, point.Collides(line));
            Assert.AreEqual(result, line.Collides(point));
        }

        private class PointInTriangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(1, 2), new Point(2, 1)), new Point(4f, 4f), "Point far away." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(1, 2), new Point(2, 1)), new Point(1.7f, 1.7f), "Point inside bounding box." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(1, 2), new Point(2, 1)), new Point(0.95f, 0.95f), "Point close to corner." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(1, 2), new Point(2, 1)), new Point(1.1f, 1.1f), "Point side and close to corner." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(1, 2), new Point(2, 1)), new Point(1f, 1f), "Point on corner." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(1, 2), new Point(2, 1)), new Point(1f, 1.5f), "Point on edge." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(1, 2), new Point(2, 1)), new Point(1.25f, 1.25f), "Point well in the middle." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(-1, 1)), new Point(0, -0.5f), "Point outside. Winding order #1." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(-1, 1), new Point(2, 3)), new Point(0, -0.5f), "Point outside. Winding order #2." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(-1, 1)), new Point(1, 2), "Point inside. Winding order #1." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(-1, 1), new Point(2, 3)), new Point(1, 2), "Point inside. Winding order #2." };
            }
        }

        [DataTestMethod]
        [PointInTriangleDataSource]
        public void PointInTriangleTest(bool result, Triangle triangle, Point point, string name) {
            Assert.AreEqual(result, point.Collides(triangle));
            Assert.AreEqual(result, triangle.Collides(point));
        }

        private class PointInRectangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 2)), new Point(4f, 4f), "Point far away." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 2)), new Point(2f, 4f), "Point with dimensional overlap." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 2)), new Point(4f, 1f), "Point with dimensional corner overlap." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 2)), new Point(3.05f, 2f), "Point close to corner" };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 2)), new Point(2f, 0.95f), "Point close to edge." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 2)), new Point(3f, 1f), "Point on corner." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 2)), new Point(2f, 1f), "Point on edge." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 2)), new Point(2f, 1.6f), "Point near the middle." };
            }
        }

        [DataTestMethod]
        [PointInRectangleDataSource]
        public void PointInRectangleTest(bool result, Rectangle rectangle, Point point, string name) {
            Assert.AreEqual(result, point.Collides(rectangle));
            Assert.AreEqual(result, rectangle.Collides(point));
        }

        private class PointInCircleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Circle(new Point(2, 2), 1), new Point(4f, 4f), "Point far away." };
                yield return new object[] { false, new Circle(new Point(2, 2), 1), new Point(4f, 2f), "Point with dimensional overlap." };
                yield return new object[] { false, new Circle(new Point(2, 2), 1), new Point(2.9f, 2.9f), "Point in bounding box." };
                yield return new object[] { false, new Circle(new Point(2, 2), 1), new Point(2.71f, 2.71f), "Point close to radius." };
                yield return new object[] { true, new Circle(new Point(2, 2), 1), new Point(2f, 2f), "Point at center." };
                yield return new object[] { true, new Circle(new Point(2, 2), 1), new Point(1.8f, 1.8f), "Point not at center but in circle." };
                yield return new object[] { true, new Circle(new Point(2, 2), 1), new Point(2.707106f, 2.707106f), "Point on radius but not N/S/W/E." };
            }
        }

        [DataTestMethod]
        [PointInCircleDataSource]
        public void PointInCircleTest(bool result, Circle circle, Point point, string name) {
            Assert.AreEqual(result, point.Collides(circle));
            Assert.AreEqual(result, circle.Collides(point));
        }

        private class PointInPolygonDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(4, 4), "Point far away." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(3, 3), "Point with dimensional corner overlap." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(3, 2), "Point with dimensional overlap." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(0, 0), "Point within bounding box." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(1, 0), "Point within concave 'bay'." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(-1, 1), "Point on corner." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(0, 1), "Point on edge." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(1, 2), "Point inside polygon." };
            }
        }

        [DataTestMethod]
        [PointInPolygonDataSource]
        public void PointInPolygonTest(bool result, Polygon polygon, Point point, string name) {
            Assert.AreEqual(result, point.Collides(polygon));
            Assert.AreEqual(result, polygon.Collides(point));
        }
    }
}
