using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using SfmlProjectTests;
using System.Reflection;
using System;

namespace SfmlProject.Geometry.Tests {
    /**
     * Fantastic tool to set up those unit tests.
     * https://www.geogebra.org/geometry
     **/
    [TestClass()]
    public class RectangleTests {
        [TestMethod()]
        public void RectangleTest1() {
            Rectangle rectangle = new Rectangle(new Point(1, 2), new Point(3, 4));
            Assert.IsTrue(rectangle.Points.Any(x => x.Equals(new Point(1, 2))));
            Assert.IsTrue(rectangle.Points.Any(x => x.Equals(new Point(3, 4))));
            Assert.IsTrue(rectangle.Points.Any(x => x.Equals(new Point(3, 2))));
            Assert.IsTrue(rectangle.Points.Any(x => x.Equals(new Point(1, 4))));
        }

        [TestMethod()]
        public void RectangleTest2() {
            Rectangle rectangle = new Rectangle(new Point(1, 2), 2, 2);
            Assert.IsTrue(rectangle.Points.Any(x => x.Equals(new Point(1, 2))));
            Assert.IsTrue(rectangle.Points.Any(x => x.Equals(new Point(3, 4))));
            Assert.IsTrue(rectangle.Points.Any(x => x.Equals(new Point(3, 2))));
            Assert.IsTrue(rectangle.Points.Any(x => x.Equals(new Point(1, 4))));
        }

        [TestMethod()]
        public void CollisionWithPointCalledCorrectly() {
            Rectangle rectangle = new Rectangle(new Point(1, 2), new Point(3, 4));
            Point point1 = new Point(2, 3);
            Assert.IsTrue(rectangle.Collides(point1));
            Point point2 = new Point(4, 3);
            Assert.IsFalse(rectangle.Collides(point2));
        }

        [TestMethod()]
        public void CollisionWithLineCalledCorrectly() {
            Rectangle rectangle = new Rectangle(new Point(1, 2), new Point(3, 4));
            Line line1 = new Line(new Point(2, 3), new Point(4, 3));
            Assert.IsTrue(rectangle.Collides(line1));
            Line line2 = new Line(new Point(2, 1), new Point(4, 1));
            Assert.IsFalse(rectangle.Collides(line2));
        }

        [TestMethod()]
        public void CollisionWithTriangleCalledCorrectly() {
            Rectangle rectangle = new Rectangle(new Point(1, 2), new Point(3, 4));
            Triangle triangle1 = new Triangle(new Point(2, 3), new Point(2, 1), new Point(4, 1));
            Assert.IsTrue(rectangle.Collides(triangle1));
            Triangle triangle2 = new Triangle(new Point(2, -1), new Point(2, 1), new Point(4, 1));
            Assert.IsFalse(rectangle.Collides(triangle2));
        }

        private class RectangleIntersectsRectangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Rectangle(new Point(4, 4), new Point(6, 6)), "Rectangle far away." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Rectangle(new Point(4, 3), new Point(6, 6)), "Rectangle with dimensional corner overlap." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Rectangle(new Point(4, 2), new Point(6, 6)), "Rectangle with dimensional overlap." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Rectangle(new Point(3, 3), new Point(6, 6)), "Rectangle with corner on corner." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Rectangle(new Point(3, 2), new Point(6, 6)), "Rectangle with corner on edge." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Rectangle(new Point(2, 2), new Point(6, 6)), "Rectangle with corner inside." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Rectangle(new Point(2, 0), new Point(6, 6)), "Rectangle with corners inside but not vice-versa." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(4, 4)), new Rectangle(new Point(2, 0), new Point(3, 5)), "Rectangle overlapping but without corners inside." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Rectangle(new Point(1, 1), new Point(3, 3)), "Rectangle is identical." };
            }
        }

        [DataTestMethod]
        [RectangleIntersectsRectangleDataSource]
        public void RectangleIntersectsTriangleTest(bool result, Rectangle rectangle, Rectangle otherRectangle, string name) {
            Assert.AreEqual(result, rectangle.Collides(otherRectangle));
            Assert.AreEqual(result, otherRectangle.Collides(rectangle));
        }

        private class CircleIntersectsRectangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(5, 5), 1), "Circle far away." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(5, 4), 1), "Circle with dimensional 'touch'." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(5, 3), 1), "Circle with dimensional overlap." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(3.8f, 3.8f), 1), "Circle in bounding box." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(4, 3), 1), "Circle intersecting corner." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(3.8f, 3), 1), "Circle intersecting edge." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(3.5f, 3.5f), 1), "Circle with corner inside rectangle." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(2.5f, 2.5f), 1), "Circle with center inside rectangle." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(2, 2), 0.8f), "Circle completely inside rectangle." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(2, 2), 2), "Circle with rectangle inside." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Circle(new Point(2, 2), 1.3f), "Circle intersecting all edges, but no points contained vice-versa." };
            }
        }

        [DataTestMethod]
        [CircleIntersectsRectangleDataSource]
        public void CircleIntersectsRectangleTest(bool result, Rectangle rectangle, Circle circle, string name) {
            Assert.AreEqual(result, rectangle.Collides(circle));
            Assert.AreEqual(result, circle.Collides(rectangle));
        }

        private class PolygonIntersectsRectangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(4, 5), new Point(6, 6), new Point(5, 4), new Point(5, 5)), "Polygon far away." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(4, 5), new Point(6, 6), new Point(5, 3), new Point(5, 5)), "Polygon with dimensional corner overlap." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(4, 5), new Point(6, 6), new Point(5, 2), new Point(5, 5)), "Polygon with dimensional overlap." };
                yield return new object[] { false, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(1, 4), new Point(6, 6), new Point(4, 2), new Point(5, 5)), "Polygon with rectangle in concave bay." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(4, 5), new Point(6, 6), new Point(3, 1), new Point(5, 5)), "Polygon with point on corner." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(4, 5), new Point(6, 6), new Point(3, 2), new Point(5, 5)), "Polygon with point on edge." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(4, 5), new Point(6, 6), new Point(5, 4), new Point(2, 2)), "Polygon with point in rectangle." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(0, 2), new Point(6, 6), new Point(5, 4), new Point(2, 2)), "Polygon edges intersecting but no points in rectangle and vice-versa." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4)), "Polygon containing rectangle completely." };
                yield return new object[] { true, new Rectangle(new Point(1, 1), new Point(3, 3)), new Polygon(new Point(1.5f, 1.5f), new Point(2.5f, 1.5f), new Point(2.5f, 2.5f), new Point(1.5f, 2.5f)), "Polygon completely contained in rectangle." };
            }
        }

        [DataTestMethod]
        [PolygonIntersectsRectangleDataSource]
        public void PolygonIntersectsRectangleTest(bool result, Rectangle rectangle, Polygon polygon, string name) {
            Assert.AreEqual(result, rectangle.Collides(polygon));
            Assert.AreEqual(result, polygon.Collides(rectangle));
        }
    }
}
