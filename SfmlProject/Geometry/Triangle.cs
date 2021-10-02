using SfmlProject.Geometry.Base;
using System.Collections.Generic;

namespace SfmlProject.Geometry {
    public class Triangle : ICollidesWith {
        private HashSet<Point> points = new HashSet<Point>();
        public Point PointA { get; set; }
        public Point PointB { get; set; }
        public Point PointC { get; set; }
        public HashSet<Line> Lines { get; private set; }

        public Triangle(Point a, Point b, Point c) {
            this.PointA = a;
            this.PointB = b;
            this.PointC = c;
            this.points.Add(a);
            this.points.Add(b);
            this.points.Add(c);
            this.Lines = new HashSet<Line>();
            this.Lines.Add(new Line(a, b));
            this.Lines.Add(new Line(b, c));
            this.Lines.Add(new Line(c, a));
        }

        public bool Collides(Point otherPoint) {
            return CollisionHelper.PointInTriangle(otherPoint, this);
        }

        public bool Collides(Line otherLine) {
            return CollisionHelper.LineIntersectsTriangle(otherLine, this);
        }

        public bool Collides(Triangle otherTriangle) {
            foreach (Point point in this.points) {
                if (CollisionHelper.PointInTriangle(point, otherTriangle)) { }
            }
            throw new System.NotImplementedException();
        }

        public bool Collides(Rectangle otherRectangle) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Circle otherCircle) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Polygon otherPolygon) {
            throw new System.NotImplementedException();
        }

        public override string ToString() {
            return string.Format("T[{0},{1},{2}]", this.PointA, this.PointB, this.PointC);
        }
    }
}