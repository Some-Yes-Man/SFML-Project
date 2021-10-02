using SfmlProject.Geometry.Base;

namespace SfmlProject.Geometry {
    public class Circle : ICollidesWith {
        public Point Center { get; set; }
        public float Radius { get; set; }

        public Circle(Point point, float radius) {
            this.Center = point;
            this.Radius = radius;
        }

        public Circle(float x, float y, float radius) : this(new Point(x, y), radius) {
        }

        public bool Collides(Point otherPoint) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Line otherLine) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Triangle otherTriangle) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Rectangle otherRectangle) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Circle otherCircle) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Polygon otherShape) {
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            return string.Format("C[{0},{1}]", this.Center, this.Radius);
        }
    }
}
