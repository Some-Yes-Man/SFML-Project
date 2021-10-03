using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace SfmlProject.Geometry.Tests {
    [TestClass()]
    public class CollisionHelperTests {
        /**
         * Fantastic tool to set up those unit tests.
         * https://www.geogebra.org/geometry
         **/
        private abstract class NamedDataSource : Attribute, ITestDataSource {
            public abstract IEnumerable<object[]> GetData(MethodInfo methodInfo);

            public string GetDisplayName(MethodInfo methodInfo, object[] data) {
                return string.Format(CultureInfo.CurrentCulture, "{0} : {1}", methodInfo.Name, data[^1]);
            }
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
            }
        }

        [DataTestMethod]
        [PointInTriangleDataSource]
        public void PointInTriangleTest(bool result, Triangle triangle, Point point, string name) {
            Assert.AreEqual(result, CollisionHelper.PointInTriangle(point, triangle));
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
            Assert.AreEqual(result, CollisionHelper.PointInRectangle(point, rectangle));
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
            Assert.AreEqual(result, CollisionHelper.PointInCircle(point, circle));
        }

        private class PointInPolygonDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(4f, 4f), "Point far away." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(3f, 2f), "Point with dimensional overlap." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(4f, 4f), "Point within bounding box." };
                yield return new object[] { false, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(0f, 2f), "Point within concave 'bay'." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(-1f, 1f), "Point on corner." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(0f, 1f), "Point on edge." };
                yield return new object[] { true, new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 3), new Point(2, -1)), new Point(0.5f, 0.5f), "Point inside polygon." };
            }
        }

        [DataTestMethod]
        [PointInPolygonDataSource]
        public void PointInPolygonTest(bool result, Polygon polygon, Point point, string name) {
            Assert.AreEqual(result, CollisionHelper.PointInPolygon(point, polygon));
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
            Assert.AreEqual(result, CollisionHelper.LineIntersectsTriangle(line, triangle));
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
            Assert.AreEqual(result, CollisionHelper.LineIntersectsRectangle(line, rectangle));
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
            Assert.AreEqual(result, CollisionHelper.LineIntersectsCircle(line, circle));
        }

        [TestMethod()]
        public void LineIntersectsPolygonTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void TriangleIntersectsRectangleTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void TriangleIntersectsCircleTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void RectangleIntersectsRectangleTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void RectangleIntersectsCircleTest() {
            Assert.Fail();
        }

        [TestMethod()]
        public void RectangleIntersectsPolygonTest() {
            Assert.Fail();
        }
    }
}
