using Microsoft.VisualStudio.TestTools.UnitTesting;
using SfmlProject.Geometry;
using System;

namespace SfmlProject.Data.Tests {
    [TestClass()]
    public class PointTests {
        [TestMethod()]
        public void PointTest() {
            new Point(1.5f, -0.32f);
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

        [TestMethod()]
        public void CollisionWithLineCalledCorrectly() {
            Point point = new Point(1.5f, 1.5f);
            Line line = new Line(new Point(1, 1), new Point(2, 2));
            Assert.IsTrue(point.Collides(line));
        }

        [TestMethod()]
        public void CollisionWithTriangleCalledCorrectly() {
            Point point = new Point(1.5f, 1.5f);
            Triangle triangle = new Triangle(new Point(1, 1), new Point(3, 2), new Point(2, 4));
            Assert.IsTrue(point.Collides(triangle));
        }

        [TestMethod()]
        public void CollisionWithCircleCalledCorrectly() {
            Point point = new Point(1.5f, 1.5f);
            Circle circle = new Circle(2, 2, 1);
            Assert.IsTrue(point.Collides(circle));
        }

        [TestMethod()]
        public void CollisionWithShapeCalledCorrectly() {
            Point point = new Point(1.5f, 1.5f);
            Polygon shape = new Polygon(new Point(1, 1), new Point(-1, 1), new Point(2, 4), new Point(2, -3));
            Assert.IsTrue(point.Collides(shape));
        }
    }
}
