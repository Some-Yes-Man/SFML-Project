using System.Numerics;

namespace SFMLTest.Data {
    public class Point : ICollidesWith {
        private Vector2 vector = new Vector2();
        public float X {
            get { return this.vector.X; }
            set { this.vector.X = value; }
        }
        public float Y {
            get { return this.vector.Y; }
            set { this.vector.Y = value; }
        }
        public Vector2 Vector {
            get { return this.vector; }
        }

        public Point(float x, float y) {
            this.vector = new Vector2(x, y);
        }

        public bool Collides(Point otherPoint) {
            return this.vector.Equals(otherPoint.vector);
        }

        public bool Collides(Line otherLine) {
            return GeometryUtils.PointOnLine(this, otherLine);
        }

        public bool Collides(Triangle otherTriangle) {
            return GeometryUtils.PointInTriangle(this, otherTriangle);
        }

        public bool Collides(Circle otherCircle) {
            return GeometryUtils.PointInCircle(this, otherCircle);
        }

        public bool Collides(Shape otherShape) {
            return GeometryUtils.PointInShape(this, otherShape);
        }

        public override string ToString() {
            return string.Format("P[{0}:{1}]", this.X, this.Y);
        }
    }
}
