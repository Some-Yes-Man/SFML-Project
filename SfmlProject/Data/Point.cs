using System.Numerics;

namespace SFMLTest.Data {
    public class Point : IIntersect {
        private Vector2 vector = new Vector2();
        public float X {
            get { return this.vector.X; }
            set { this.vector.X = value; }
        }
        public float Y {
            get { return this.vector.Y; }
            set { this.vector.Y = value; }
        }

        public Point(float x, float y) {
            this.vector = new Vector2(x, y);
        }

        public bool Intersects(Point otherPoint) {
            return (this.X == otherPoint.X) && (this.Y == otherPoint.Y);
        }

        public bool Intersects(Line otherLine) {
            return GeometryUtils.PointOnLine(this, otherLine);
        }

        public bool Intersects(Triangle otherTriangle) {
            return GeometryUtils.PointInTriangle(this, otherTriangle);
        }

        public bool Intersects(Circle otherCircle) {
            return GeometryUtils.PointInCircle(this, otherCircle);
        }

        public bool Intersects(Shape otherShape) {
            // FIXME: split into triangles and check
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            return string.Format("P[{0}:{1}]", this.X, this.Y);
        }
    }
}
