using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace SFMLTest.Data {
    public class Shape : IIntersect {
        public List<Point> Points { get; private set; }
        public List<Line> Lines { get; private set; }
        public HashSet<Triangle> Triangles { get; private set; }

        public Shape(params Point[] points) {
            // not enough points given
            if ((points == null) || (points.Length < 3)) {
                throw new ArgumentException("Shapes can only be constructed from 3 or more points.");
            }
            this.Points = new List<Point>(points);
            this.Lines = new List<Line>();
            for (int i = 1; i < points.Length; i++) {
                this.Lines.Add(new Line(points[i - 1], points[i]));
            }
            this.Lines.Add(new Line(points[0], points[points.Length - 1]));
            // lines of shape cross
            foreach (Line currentLine in this.Lines) {
                if (this.Lines.Any(x => !NeighboringLines(currentLine, x) && currentLine.Intersects(x))) {
                    throw new ArgumentException("Shapes are not allowed to have intersecting lines.");
                }
            }
        }

        private bool NeighboringLines(Line x, Line y) {
            int indexDifference = Math.Abs(this.Lines.IndexOf(x) - this.Lines.IndexOf(y));
            return (indexDifference == 1) || (indexDifference == (this.Lines.Count - 1));
        }

        private HashSet<Triangle> CutUpShape() {
            HashSet<Triangle> triangles = new HashSet<Triangle>();
            Queue<Point> pointsLeft = new Queue<Point>(this.Points);
            while (pointsLeft.Count > 3) {
                // FIXME: ....
            }
        }

        public bool Intersects(Point otherPoint) {
            throw new NotImplementedException();
        }

        public bool Intersects(Line otherLine) {
            foreach (Line line in this.Lines) {
                if (line.Intersects(otherLine)) {
                    return true;
                }
            }
            return false;
        }

        public bool Intersects(Triangle otherTriangle) {
            throw new NotImplementedException();
        }

        public bool Intersects(Circle otherCircle) {
            throw new NotImplementedException();
        }

        public bool Intersects(Shape otherShape) {
            foreach (Line otherLine in otherShape.Lines) {
                if (this.Intersects(otherLine)) {
                    return true;
                }
            }
            return false;
        }

        public override string ToString() {
            return string.Format("S[{0}]", string.Join(',', this.Points));
        }
    }
}
