using Microsoft.VisualStudio.TestTools.UnitTesting;
using SfmlProjectTests;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SfmlProject.Geometry.Tests {
    /**
     * Fantastic tool to set up those unit tests.
     * https://www.geogebra.org/geometry
     **/
    [TestClass()]
    public class PolygonTests {
        [TestMethod()]
        public void PolygonTest() {
            Polygon polygon = new Polygon(new Point(0, 1), new Point(2, 2), new Point(1, 0), new Point(1, 1));
            Assert.IsTrue(polygon.Points.Any(x => x.Equals(new Point(0, 1))));
            Assert.IsTrue(polygon.Points.Any(x => x.Equals(new Point(2, 2))));
            Assert.IsTrue(polygon.Points.Any(x => x.Equals(new Point(1, 0))));
            Assert.IsTrue(polygon.Points.Any(x => x.Equals(new Point(1, 1))));
        }

        [TestMethod()]
        public void CollisionWithPointCalledCorrectly() {
            Polygon polygon = new Polygon(new Point(0, 1), new Point(2, 2), new Point(1, 0), new Point(1, 1));
            Point point1 = new Point(1.5f, 1.5f);
            Assert.IsTrue(polygon.Collides(point1));
            Point point2 = new Point(0, 0);
            Assert.IsFalse(polygon.Collides(point2));
        }

        [TestMethod()]
        public void CollisionWithLineCalledCorrectly() {
            Polygon polygon = new Polygon(new Point(0, 1), new Point(2, 2), new Point(1, 0), new Point(1, 1));
            Line line1 = new Line(new Point(0, 0), new Point(2, 1));
            Assert.IsTrue(polygon.Collides(line1));
            Line line2 = new Line(new Point(0, 2), new Point(1, 2));
            Assert.IsFalse(polygon.Collides(line2));
        }

        [TestMethod()]
        public void CollisionWithTriangleCalledCorrectly() {
            Polygon polygon = new Polygon(new Point(0, 1), new Point(2, 2), new Point(1, 0), new Point(1, 1));
            Triangle triangle1 = new Triangle(new Point(1, 2), new Point(2, 0), new Point(3, 0));
            Assert.IsTrue(polygon.Collides(triangle1));
            Triangle triangle2 = new Triangle(new Point(0, 2), new Point(2, 3), new Point(0, 3));
            Assert.IsFalse(polygon.Collides(triangle2));
        }

        [TestMethod()]
        public void CollisionWithRectangleCalledCorrectly() {
            Polygon polygon = new Polygon(new Point(0, 1), new Point(2, 2), new Point(1, 0), new Point(1, 1));
            Rectangle rectangle1 = new Rectangle(new Point(1, 0), new Point(2, 0));
            Assert.IsTrue(polygon.Collides(rectangle1));
            Rectangle rectangle2 = new Rectangle(new Point(0, 2), new Point(1, 3));
            Assert.IsFalse(polygon.Collides(rectangle2));
        }

        private class PolygonIntersectsPolygonDataSource : NamedDataSource {
            public override IEnumerable<object[]> GetData(MethodInfo methodInfo) {
                yield return new object[] { false, new Polygon(new Point(3, 1), new Point(2, 3), new Point(1, 3), new Point(4, 4)), new Polygon(new Point(5, 5), new Point(6, 6), new Point(8, 6), new Point(5, 7)), "Polygon far away." };
            }
        }

        [DataTestMethod]
        [PolygonIntersectsPolygonDataSource]
        public void PolygonIntersectsCircleTest(bool result, Polygon polygon, Polygon otherPolygon, string name) {
            Assert.AreEqual(result, polygon.Collides(otherPolygon));
            Assert.AreEqual(result, otherPolygon.Collides(polygon));
        }
    }
}
