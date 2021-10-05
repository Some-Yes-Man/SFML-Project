using Microsoft.VisualStudio.TestTools.UnitTesting;
using SfmlProjectTests;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SfmlProject.Geometry.Tests {
    /**
     * Fantastic tool to set up those unit tests.
     * https://www.geogebra.org/geometry
     **/
    [TestClass()]
    public class TriangleTests {
        [TestMethod()]
        public void TriangleTest() {
            Triangle triangle = new Triangle(new Point(1, 2), new Point(3, 4), new Point(5, 0));
            Assert.AreEqual(1, triangle.Points[0].X);
            Assert.AreEqual(2, triangle.Points[0].Y);
            Assert.AreEqual(3, triangle.Points[1].X);
            Assert.AreEqual(4, triangle.Points[1].Y);
            Assert.AreEqual(5, triangle.Points[2].X);
            Assert.AreEqual(0, triangle.Points[2].Y);
        }

        [TestMethod()]
        public void TriangleTest2() {
            Assert.ThrowsException<ArgumentException>(() => new Triangle(new Point(1, 1), new Point(2, 2), new Point(3, 3)));
        }

        [TestMethod()]
        public void CollisionWithPointCalledCorrectly() {
            Triangle triangle = new Triangle(new Point(1, 2), new Point(3, 4), new Point(5, 0));
            Point point1 = new Point(3f, 2f);
            Assert.IsTrue(triangle.Collides(point1));
            Point point2 = new Point(2f, 1f);
            Assert.IsFalse(triangle.Collides(point2));
        }

        [TestMethod()]
        public void CollisionWithLineCalledCorrectly() {
            Triangle triangle = new Triangle(new Point(1, 2), new Point(3, 4), new Point(5, 0));
            Line line1 = new Line(new Point(1, 1), new Point(2, 1));
            Assert.IsFalse(triangle.Collides(line1));
            Line line2 = new Line(new Point(1, 1), new Point(2, 2));
            Assert.IsTrue(triangle.Collides(line2));
        }

        private class TriangleIntersectsTriangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(5, 4), new Point(6, 5), new Point(5, 7)), "Triangle far away." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(5, 3), new Point(6, 5), new Point(5, 7)), "Triangle with dimensional corner overlap." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(5, 2), new Point(6, 5), new Point(5, 7)), "Triangle with dimensional overlap." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(3.5f, 2), new Point(6, 5), new Point(5, 7)), "Triangle with one corner in bounding box." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(2, 4), new Point(5, 1), new Point(5, 7)), "Triangle with one edge in bounding box." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(2, 4), new Point(4, 4), new Point(2, 3)), "Triangle with one corner on corner." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(2, 4), new Point(4, 4), new Point(3, 2)), "Triangle with one corner on edge." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(1, 4), new Point(4, 4), new Point(2, 2)), "Triangle with one corner inside and vice-versa." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(1, 2), new Point(4, 2), new Point(2, 1.5f)), "Triangle with one corner inside and NOT vice-versa." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(1, 2), new Point(4, 2), new Point(2, 0)), "Triangle with overlap but not corners in other triangle or vice-versa." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Triangle(new Point(1.5f, 1.5f), new Point(2, 2), new Point(3, 1.5f)), "Triangle completely contained in other triangle." };
            }
        }

        [DataTestMethod]
        [TriangleIntersectsTriangleDataSource]
        public void TriangleIntersectsTriangleTest(bool result, Triangle triangle, Triangle otherTriangle, string name) {
            Assert.AreEqual(result, otherTriangle.Collides(triangle));
            Assert.AreEqual(result, triangle.Collides(otherTriangle));
        }

        private class RectangleIntersectsTriangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(5, 4), new Point(7, 5)), "Rectangle far away." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(5, 3), new Point(7, 5)), "Rectangle with dimensional corner overlap." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(5, 2), new Point(7, 5)), "Rectangle with dimensional overlap." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(3.5f, 2.5f), new Point(5, 5)), "Rectangle with one corner in bounding box." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(4, 1), new Point(5, 4)), "Rectangle with one corner on a corner." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(3, 2), new Point(5, 4)), "Rectangle with one corner on an edge." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(3, 0), new Point(5, 1.5f)), "Rectangle with one corner inside and vice-versa." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(3, 1.5f), new Point(5, 4)), "Rectangle with one corner inside and NOT vice-versa." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(1.5f, 0), new Point(3.5f, 2.5f)), "Rectangle with overlap but no corners in triangle or vice-versa." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(0, 0), new Point(5, 4)), "Rectangle with triangle completely contained in it." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Rectangle(new Point(2, 1.5f), new Point(2.5f, 2)), "Rectangle completely contained in triangle." };
            }
        }

        [DataTestMethod]
        [RectangleIntersectsTriangleDataSource]
        public void RectangleIntersectsTriangleTest(bool result, Triangle triangle, Rectangle rectangle, string name) {
            Assert.AreEqual(result, triangle.Collides(rectangle));
            Assert.AreEqual(result, rectangle.Collides(triangle));
        }

        private class CircleIntersectsTriangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(6, 5), 1), "Circle far away." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(5, 5), 1), "Circle with dimensional 'touch'." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(4, 5), 1), "Circle with dimensional overlap." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(4.1f, 3.1f), 1), "Circle inside bounding box." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(3.9f, 2.9f), 1), "Circle with center inside bounding box." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(3.5f, 2.5f), 1), "Circle with radius intersecting edge." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(2, 4), 1), "Circle with radius intersecting corner." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(2, 3.5f), 1), "Circle intersecting two edges." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(2, 0), 1), "Circle touch edge with radius." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(2.5f, 1.5f), 0.2f), "Circle completely inside triangle." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(2.5f, 1.5f), 2), "Circle containing triangle completely." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(2.3f, 1.7f), 1), "Circle intersecting all three sides but no points." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Circle(new Point(3, 2), 0.5f), "Circle with center on edge and intersecting said edge twice." };
            }
        }

        [DataTestMethod]
        [CircleIntersectsTriangleDataSource]
        public void CircleIntersectsTriangleTest(bool result, Triangle triangle, Circle circle, string name) {
            Assert.AreEqual(result, triangle.Collides(circle));
            Assert.AreEqual(result, circle.Collides(triangle));
        }

        private class PolygonIntersectsTriangleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(7, 6), new Point(5, 6), new Point(8, 8), new Point(8, 4)), "Polygon far away." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(6, 6), new Point(4, 6), new Point(7, 8), new Point(7, 4)), "Polygon with dimensional corner overlap." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(5, 6), new Point(3, 6), new Point(6, 8), new Point(6, 4)), "Polygon with dimensional overlap." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(6, 3), new Point(3.5f, 2.5f), new Point(7, 5), new Point(7, 1)), "Polygon with point in bounding box." };
                yield return new object[] { false, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(3, 3.1f), new Point(2, 4), new Point(5, 6), new Point(5, 2)), "Polygon with edge in bounding box." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(5, 4), new Point(2, 3), new Point(6, 6), new Point(6, 2)), "Polygon with point on corner." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(5, 4), new Point(3, 2), new Point(6, 6), new Point(6, 2)), "Polygon with point on edge." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(5, 4), new Point(2, 2), new Point(6, 6), new Point(6, 2)), "Polygon with point inside triangle." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(5, 4), new Point(0, 2), new Point(6, 6), new Point(6, 2)), "Polygon with triangle point inside." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(5, 4), new Point(2, 0), new Point(6, 6), new Point(6, 2)), "Polygon with with two edges intersecting two triangle edges." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(1, 0), new Point(6, 1), new Point(4, 4), new Point(0, 5)), "Polygon containing triangle completely." };
                yield return new object[] { true, new Triangle(new Point(1, 1), new Point(2, 3), new Point(4, 1)), new Polygon(new Point(1.5f, 1.5f), new Point(2, 2.5f), new Point(2, 2), new Point(2.5f, 1.5f)), "Polygon completely inside triangle." };
            }
        }

        [DataTestMethod]
        [PolygonIntersectsTriangleDataSource]
        public void PolygonIntersectsTriangleTest(bool result, Triangle triangle, Polygon polygon, string name) {
            Assert.AreEqual(result, triangle.Collides(polygon));
            Assert.AreEqual(result, polygon.Collides(triangle));
        }
    }
}
