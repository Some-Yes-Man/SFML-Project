using SFML.Graphics;
using SfmlProject.Geometry.Base;
using SfmlProject.Graphic;
using System.Numerics;

namespace SfmlProject.Geometry {
    public class Point : ICollidesWith, IRenderable {

        private Vector2 vector = new Vector2();
        private CircleShape renderable;

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

        public Drawable Renderable {
            get {
                if (this.renderable == null) {
                    this.renderable = new CircleShape(1f);
                    this.renderable.Position = new SFML.System.Vector2f(this.X, this.Y);
                }
                return this.renderable;
            }
        }

        public bool Collides(Point otherPoint) {
            return this.vector.Equals(otherPoint.vector);
        }

        public bool Collides(Line otherLine) {
            return otherLine.Collides(this);
        }

        public bool Collides(Triangle otherTriangle) {
            return otherTriangle.Collides(this);
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

        public override bool Equals(object obj) {
            if ((obj == null) || !this.GetType().Equals(obj.GetType())) {
                return false;
            }
            Point other = (Point)obj;
            return this.vector.Equals(other.vector);
        }

        public override int GetHashCode() {
            return this.vector.GetHashCode();
        }

        public override string ToString() {
            return string.Format("P[{0}:{1}]", this.X, this.Y);
        }
    }
}
