using SFML.System;
using SfmlProject.Geometry.Base;
using SfmlProject.Graphic;
using System;

namespace SfmlProject.Geometry {
    public class Line : Shape, IBoundingBox, ICollidesWith {

        private static readonly float EPSILON = 0.0001f;

        private readonly Point pointA;
        private readonly Point pointB;
        private readonly float slope;
        private readonly float yIntercept;

        private Rectangle boundingBox;
        private DrawableLine renderable;

        public Line(Point pointA, Point pointB) : base() {
            this.pointA = pointA;
            this.pointB = pointB;
            this.slope = (this.pointB.Y - this.pointA.Y) / (this.pointB.X - this.pointA.X);
            this.yIntercept = this.pointA.Y - this.slope * this.pointA.X;

            this.Points.Add(this.pointA);
            this.Points.Add(this.pointB);
            this.Lines.Add(this);
        }

        public Rectangle BoundingBox {
            get {
                if (this.boundingBox == null) {
                    this.boundingBox = new Rectangle(pointA, pointB);
                }
                return this.boundingBox;
            }
        }

        public override SFML.Graphics.Drawable Renderable {
            get {
                if (this.renderable == null) {
                    this.renderable = new DrawableLine(new Vector2f(this.pointA.X, this.pointA.Y), new Vector2f(this.pointB.X, this.pointB.Y));
                }
                return this.renderable;
            }
        }

        public bool Collides(Point otherPoint) {
            return (otherPoint.X >= Math.Min(this.pointA.X, this.pointB.X)) && (otherPoint.X <= Math.Max(this.pointA.X, this.pointB.X))
                && ((float.IsInfinity(this.slope) && (otherPoint.Y >= Math.Min(this.pointA.Y, this.pointB.Y)) && (otherPoint.Y <= Math.Max(this.pointA.Y, this.pointB.Y))) || (Math.Abs((this.slope * otherPoint.X) + this.yIntercept - otherPoint.Y) <= EPSILON));
        }

        public bool Collides(Line other) {
            // maybe there's no need to calculate anything
            bool overlapInX = Math.Max(this.pointA.X, this.pointB.X) >= Math.Min(other.pointA.X, other.pointB.X) && Math.Min(this.pointA.X, this.pointB.X) <= Math.Max(other.pointA.X, other.pointB.X);
            bool overlapInY = Math.Max(this.pointA.Y, this.pointB.Y) >= Math.Min(other.pointA.Y, other.pointB.Y) && Math.Min(this.pointA.Y, this.pointB.Y) <= Math.Max(other.pointA.Y, other.pointB.Y);
            if (!overlapInX || !overlapInY) {
                return false;
            }
            else {
                float otherAvirtualY = float.IsInfinity(this.slope) ? other.pointA.Y : other.pointA.X * this.slope + this.yIntercept;
                float otherBvirtualY = float.IsInfinity(this.slope) ? other.pointB.Y : other.pointB.X * this.slope + this.yIntercept;
                float thisAvirtualY = float.IsInfinity(other.slope) ? this.pointA.Y : this.pointA.X * other.slope + other.yIntercept;
                float thisBvirtualY = float.IsInfinity(other.slope) ? this.pointB.Y : this.pointB.X * other.slope + other.yIntercept;
                return (other.pointA.Y - otherAvirtualY) * (other.pointB.Y - otherBvirtualY) <= 0 && (this.pointA.Y - thisAvirtualY) * (this.pointB.Y - thisBvirtualY) <= 0;
            }
        }

        public bool Collides(Triangle otherTriangle) {
            return CollisionHelper.LineIntersectsTriangle(this, otherTriangle);
        }

        public bool Collides(Rectangle otherRectangle) {
            return CollisionHelper.LineIntersectsRectangle(this, otherRectangle);
        }

        public bool Collides(Circle otherCircle) {
            return CollisionHelper.LineIntersectsCircle(this, otherCircle);
        }

        public bool Collides(Polygon otherPolygon) {
            return CollisionHelper.LineIntersectsPolygon(this, otherPolygon);
        }
    }
}
