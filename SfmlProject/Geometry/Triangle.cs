using SfmlProject.Geometry.Base;
using System;

namespace SfmlProject.Geometry {
    public class Triangle : Shape, IBoundingBox, ICollidesWith {
        private Rectangle boundingBox;

        public Triangle(Point pointA, Point pointB, Point pointC) : base() {
            this.Points.Add(pointA);
            this.Points.Add(pointB);
            this.Points.Add(pointC);
            this.Lines.Add(new Line(pointA, pointB));
            this.Lines.Add(new Line(pointB, pointC));
            this.Lines.Add(new Line(pointC, pointA));
        }

        public Rectangle BoundingBox {
            get {
                if (this.boundingBox == null) {
                    this.boundingBox = new Rectangle(new Point(Math.Min(Math.Min(Points[0].X, Points[1].X), Points[2].X), Math.Min(Math.Min(Points[0].Y, Points[1].Y), Points[2].Y)),
                        new Point(Math.Max(Math.Max(Points[0].X, Points[1].X), Points[2].X), Math.Max(Math.Max(Points[0].Y, Points[1].Y), Points[2].Y)));
                }
                return this.boundingBox;
            }
        }

        public bool Collides(Point otherPoint) {
            return CollisionHelper.PointInTriangle(otherPoint, this);
        }

        public bool Collides(Line otherLine) {
            return CollisionHelper.LineIntersectsTriangle(otherLine, this);
        }

        // TODO : optimise?
        public bool Collides(Triangle otherTriangle) {
            foreach (Line line in this.Lines) {
                foreach (Line otherLine in otherTriangle.Lines) {
                    if (line.Collides(otherLine)) {
                        return true;
                    }
                }
            }
            return CollisionHelper.PointInTriangle(this.Points[0], otherTriangle);
        }

        public bool Collides(Rectangle otherRectangle) {
            return CollisionHelper.TriangleIntersectsRectangle(this, otherRectangle);
        }

        public bool Collides(Circle otherCircle) {
            return CollisionHelper.TriangleIntersectsCircle(this, otherCircle);
        }

        public bool Collides(Polygon otherPolygon) {
            throw new System.NotImplementedException();
        }
    }
}
