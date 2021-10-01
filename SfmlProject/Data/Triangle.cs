using System.Collections.Generic;

namespace SFMLTest.Data {
    public class Triangle : ICollidesWith {
        public Point PointA { get; set; }
        public Point PointB { get; set; }
        public Point PointC { get; set; }
        public HashSet<Line> Lines { get; private set; }

        public Triangle(Point a, Point b, Point c) {
            this.PointA = a;
            this.PointB = b;
            this.PointC = c;
            this.Lines = new HashSet<Line>();
            this.Lines.Add(new Line(a, b));
            this.Lines.Add(new Line(b, c));
            this.Lines.Add(new Line(c, a));
        }

        public bool Collides(Point otherPoint) {
            return GeometryUtils.PointInTriangle(otherPoint, this);
        }

        public bool Collides(Line otherLine) {
            return GeometryUtils.LineIntersectsTriangle(otherLine, this);
        }

        public bool Collides(Triangle otherTriangle) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Circle otherCircle) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Shape otherShape) {
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            return string.Format("T[{0},{1},{2}]", this.PointA, this.PointB, this.PointC);
        }
    }
}