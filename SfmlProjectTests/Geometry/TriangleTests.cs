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

        private class LineIntersectsTriangleDataSource : NamedDataSource {
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
        [LineIntersectsTriangleDataSource]
        public void TriangleIntersectsTriangleTest(bool result, Triangle triangle, Triangle otherTriangle, string name) {
            Assert.AreEqual(result, otherTriangle.Collides(triangle));
            Assert.AreEqual(result, triangle.Collides(otherTriangle));
        }

        // FIXME : continue
    }
}
