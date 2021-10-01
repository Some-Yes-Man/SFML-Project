using System.Collections.Generic;

namespace SFMLTest.Data {
    public class Triangle : IIntersect {
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

        public bool Intersects(Point otherPoint) {
            return GeometryUtils.PointInTriangle(otherPoint, this);
        }

        public bool Intersects(Line otherLine) {
            return GeometryUtils.LineIntersectsTriangle(otherLine, this);
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
            return string.Format("T[{0},{1},{2}]", this.PointA, this.PointB, this.PointC);
        }
    }
}