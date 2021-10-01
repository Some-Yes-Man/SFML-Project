using System;

namespace SFMLTest.Data {
    public class Line : IIntersect {

        public Point PointA { get; set; }
        public Point PointB { get; set; }
        public double slope { get; private set; }
        public double yIntercept { get; private set; }

        public Line(Point pointA, Point pointB) {
            this.PointA = pointA;
            this.PointB = pointB;
            this.slope = (this.PointB.Y - this.PointA.Y) / (this.PointB.X - this.PointA.X);
            this.yIntercept = this.PointA.Y - (this.slope * this.PointA.X);
        }

        public Line(float aX, float aY, float bX, float bY) : this(new Point(aX, aY), new Point(bX, bY)) { }

        public bool Intersects(Point otherPoint) {
            return GeometryUtils.PointOnLine(otherPoint, this);
        }

        public bool Intersects(Line other) {
            // maybe there's no need to calculate anything
            bool overlapInX = Math.Max(this.PointA.X, this.PointB.X) >= Math.Min(other.PointA.X, other.PointB.X) && Math.Min(this.PointA.X, this.PointB.X) <= Math.Max(other.PointA.X, other.PointB.X);
            bool overlapInY = Math.Max(this.PointA.Y, this.PointB.Y) >= Math.Min(other.PointA.Y, other.PointB.Y) && Math.Min(this.PointA.Y, this.PointB.Y) <= Math.Max(other.PointA.Y, other.PointB.Y);
            if (!overlapInX || !overlapInY) {
                return false;
            }
            else {
                double otherAvirtualY = (other.PointA.X * this.slope) + this.yIntercept;
                double otherBvirtualY = (other.PointB.X * this.slope) + this.yIntercept;
                double thisAvirtualY = (this.PointA.X * other.slope) + other.yIntercept;
                double thisBvirtualY = (this.PointB.X * other.slope) + other.yIntercept;
                return ((other.PointA.Y - otherAvirtualY) * (other.PointB.Y - otherBvirtualY) <= 0) && ((this.PointA.Y - thisAvirtualY) * (this.PointB.Y - thisBvirtualY) <= 0);
            }
        }

        public bool Intersects(Triangle otherTriangle) {
            return GeometryUtils.LineIntersectsTriangle(this, otherTriangle);
        }

        public bool Intersects(Circle otherCircle) {
            // FIXME: check Google for implementation
            throw new NotImplementedException();
        }

        public bool Intersects(Shape otherShape) {
            foreach (Line otherLine in otherShape.Lines) {
                if (this.Intersects(otherLine)) {
                    return true;
                }
            }
            // FIXME: need triangles from shape
        }

        public override string ToString() {
            return string.Format("L[{0},{1}]", this.PointA, this.PointB);
        }
    }
}
