using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SFMLTest.Data.Tests {
    [TestClass()]
    public class PointTests {
        [TestMethod()]
        public void PointConstructorTest() {
            Point point = new Point(2, 3);
            Assert.AreEqual(2, point.X);
            Assert.AreEqual(3, point.Y);
        }
    }
}
