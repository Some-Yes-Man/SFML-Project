using SfmlProject.Geometry.Base;
using System;

namespace SfmlProject.Geometry {
    public class Line : ICollidesWith {

        public Point PointA { get; set; }
        public Point PointB { get; set; }
        public double slope { get; private set; }
        public double yIntercept { get; private set; }

        public Line(Point pointA, Point pointB) {
            this.PointA = pointA;
            this.PointB = pointB;
            this.slope = (this.PointB.Y - this.PointA.Y) / (this.PointB.X - this.PointA.X);
            this.yIntercept = this.PointA.Y - this.slope * this.PointA.X;
        }

        public bool Collides(Point otherPoint) {
            return CollisionHelper.PointOnLine(otherPoint, this);
        }

        public bool Collides(Line other) {
            // maybe there's no need to calculate anything
            bool overlapInX = Math.Max(this.PointA.X, this.PointB.X) >= Math.Min(other.PointA.X, other.PointB.X) && Math.Min(this.PointA.X, this.PointB.X) <= Math.Max(other.PointA.X, other.PointB.X);
            bool overlapInY = Math.Max(this.PointA.Y, this.PointB.Y) >= Math.Min(other.PointA.Y, other.PointB.Y) && Math.Min(this.PointA.Y, this.PointB.Y) <= Math.Max(other.PointA.Y, other.PointB.Y);
            if (!overlapInX || !overlapInY) {
                return false;
            }
            else {
                double otherAvirtualY = double.IsInfinity(this.slope) ? other.PointA.Y : other.PointA.X * this.slope + this.yIntercept;
                double otherBvirtualY = double.IsInfinity(this.slope) ? other.PointB.Y : other.PointB.X * this.slope + this.yIntercept;
                double thisAvirtualY = double.IsInfinity(other.slope) ? this.PointA.Y : this.PointA.X * other.slope + other.yIntercept;
                double thisBvirtualY = double.IsInfinity(other.slope) ? this.PointB.Y : this.PointB.X * other.slope + other.yIntercept;
                return (other.PointA.Y - otherAvirtualY) * (other.PointB.Y - otherBvirtualY) <= 0 && (this.PointA.Y - thisAvirtualY) * (this.PointB.Y - thisBvirtualY) <= 0;
            }
        }

        public bool Collides(Triangle otherTriangle) {
            return CollisionHelper.LineIntersectsTriangle(this, otherTriangle);
        }

        public bool Collides(Rectangle otherRectangle) {
            throw new NotImplementedException();
        }

        public bool Collides(Circle otherCircle) {
            return CollisionHelper.LineIntersectsCircle(this, otherCircle);
        }

        public bool Collides(Polygon otherPolygon) {
            return CollisionHelper.LineIntersectsShape(this, otherPolygon);
        }

        public override string ToString() {
            return string.Format("L[{0},{1}]", this.PointA, this.PointB);
        }
    }
}
