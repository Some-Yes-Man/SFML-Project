using Microsoft.VisualStudio.TestTools.UnitTesting;
using SfmlProject.Geometry;
using SfmlProjectTests;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SfmlProject.Geometry.Tests {
    /**
     * Fantastic tool to set up those unit tests.
     * https://www.geogebra.org/geometry
     **/
    [TestClass()]
    public class CircleTests {
        [TestMethod()]
        public void CircleTest1() {
            Circle circle = new Circle(new Point(1, 2), 3);
            Assert.AreEqual(1, circle.Points[0].X);
            Assert.AreEqual(2, circle.Points[0].Y);
            Assert.AreEqual(3, circle.Radius);
        }

        [TestMethod()]
        public void CircleTest2() {
            Circle circle = new Circle(1, 2, 3);
            Assert.AreEqual(1, circle.Points[0].X);
            Assert.AreEqual(2, circle.Points[0].Y);
            Assert.AreEqual(3, circle.Radius);
        }

        [TestMethod()]
        public void CircleTest3() {
            Assert.ThrowsException<ArgumentException>(() => new Circle(1, 2, 0));
        }

        [TestMethod()]
        public void CollisionWithPointCalledCorrectly() {
            Circle circle = new Circle(1, 1, 1);
            Point point1 = new Point(1, 1.5f);
            Assert.IsTrue(circle.Collides(point1));
            Point point2 = new Point(3, 1);
            Assert.IsFalse(circle.Collides(point2));
        }

        [TestMethod()]
        public void CollisionWithLineCalledCorrectly() {
            Circle circle = new Circle(1, 1, 1);
            Line line1 = new Line(new Point(0, 0), new Point(2, 2));
            Assert.IsTrue(circle.Collides(line1));
            Line line2 = new Line(new Point(0, 0), new Point(-1, -1));
            Assert.IsFalse(circle.Collides(line2));
        }

        [TestMethod()]
        public void CollisionWithTriangleCalledCorrectly() {
            Circle circle = new Circle(1, 1, 1);
            Triangle triangle1 = new Triangle(new Point(0, 0), new Point(2, 2), new Point(4, 0));
            Assert.IsTrue(circle.Collides(triangle1));
            Triangle triangle2 = new Triangle(new Point(0, 0), new Point(2, -2), new Point(4, -1));
            Assert.IsFalse(circle.Collides(triangle2));
        }

        [TestMethod()]
        public void CollisionWithRectangleCalledCorrectly() {
            Circle circle = new Circle(1, 1, 1);
            Rectangle rectangle1 = new Rectangle(new Point(0, 0), new Point(1, 3));
            Assert.IsTrue(circle.Collides(rectangle1));
            Rectangle rectangle2 = new Rectangle(new Point(0, 0), new Point(-1, -1));
            Assert.IsFalse(circle.Collides(rectangle2));
        }

        private class CircleIntersectsCircleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Circle(new Point(2, 2), 2), new Circle(new Point(6, 6), 1), "Circle far away." };
                yield return new object[] { false, new Circle(new Point(2, 2), 2), new Circle(new Point(6, 5), 1), "Circle with dimensional 'touch'." };
                yield return new object[] { false, new Circle(new Point(2, 2), 2), new Circle(new Point(6, 4), 1), "Circle with dimensional overlap." };
                yield return new object[] { false, new Circle(new Point(2, 2), 2), new Circle(new Point(4.5f, 4.5f), 1), "Circle inside bounding box." };
                yield return new object[] { false, new Circle(new Point(2, 2), 2), new Circle(new Point(3.8f, 3.8f), 0.5f), "Circle with center inside bounding box." };
                yield return new object[] { true, new Circle(new Point(2, 2), 2), new Circle(new Point(4, 4), 1), "Circle intersecting." };
                yield return new object[] { true, new Circle(new Point(2, 2), 2), new Circle(new Point(2, 5), 1), "Circle touching (vertical)." };
                yield return new object[] { true, new Circle(new Point(2, 2), 2), new Circle(new Point(4.12132f, 4.12132f), 1), "Circle touching (diagonal)." };
                yield return new object[] { true, new Circle(new Point(2, 2), 2), new Circle(new Point(3, 3), 1), "Circle with center inside." };
                yield return new object[] { true, new Circle(new Point(2, 2), 2), new Circle(new Point(2, 2.5f), 1), "Circle completely inside." };
            }
        }

        [DataTestMethod]
        [CircleIntersectsCircleDataSource]
        public void CircleIntersectsCircleTest(bool result, Circle circle, Circle otherCircle, string name) {
            Assert.AreEqual(result, circle.Collides(otherCircle));
            Assert.AreEqual(result, otherCircle.Collides(circle));
        }

        private class PolygonIntersectsCircleDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Circle(new Point(2, 2), 2), new Polygon(new Point(6, 5), new Point(6, 6), new Point(5, 6), new Point(7, 7)), "Circle far away." };
            }
        }

        [DataTestMethod]
        [PolygonIntersectsCircleDataSource]
        public void PolygonIntersectsCircleTest(bool result, Circle circle, Polygon polygon, string name) {
            Assert.Fail();
            Assert.AreEqual(result, circle.Collides(polygon));
            Assert.AreEqual(result, polygon.Collides(circle));
        }
    }
}
