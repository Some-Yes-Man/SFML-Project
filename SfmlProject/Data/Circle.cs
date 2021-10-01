namespace SFMLTest.Data {
    public class Circle : IIntersect {
        public Point Center { get; set; }
        public double Radius { get; set; }

        public Circle(Point point, double radius) {
            this.Center = point;
            this.Radius = radius;
        }

        public bool Intersects(Point otherPoint) {
            throw new System.NotImplementedException();
        }

        public bool Intersects(Line otherLine) {
            throw new System.NotImplementedException();
        }

        public bool Intersects(Triangle otherTriangle) {
            throw new System.NotImplementedException();
        }

        public bool Intersects(Circle otherCircle) {
            throw new System.NotImplementedException();
        }

        public bool Intersects(Shape otherShape) {
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            return string.Format("C[{0},{1}]", this.Center, this.Radius);
        }
    }
}
