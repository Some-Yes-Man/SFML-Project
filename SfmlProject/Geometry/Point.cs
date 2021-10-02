using SfmlProject.Geometry.Base;
using System.Numerics;

namespace SfmlProject.Geometry {
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
            return otherLine.Collides(this);
        }

        public bool Collides(Triangle otherTriangle) {
            return CollisionHelper.PointInTriangle(this, otherTriangle);
        }

        public bool Collides(Rectangle otherRectangle) {
            return CollisionHelper.PointInRectangle(this, otherRectangle);
        }

        public bool Collides(Circle otherCircle) {
            return CollisionHelper.PointInCircle(this, otherCircle);
        }

        public bool Collides(Polygon otherPolygon) {
            return CollisionHelper.PointInPolygon(this, otherPolygon);
        }

        public override string ToString() {
            return string.Format("P[{0}:{1}]", this.X, this.Y);
        }
    }
}
