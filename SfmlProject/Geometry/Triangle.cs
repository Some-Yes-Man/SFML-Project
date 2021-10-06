using SfmlProject.Geometry.Base;
using SfmlProject.Geometry.Utils;
using System;

namespace SfmlProject.Geometry {
    public class Triangle : Shape, IBoundingBox, ICollidesWith {
        private Rectangle boundingBox;
        private SFML.Graphics.ConvexShape renderable;

        // needed for point collision calculations
        // magic code from the 'internet'; thx to adam https://stackoverflow.com/a/25346777
        // (where glenn failed: https://stackoverflow.com/a/20861130)
        private readonly float y23, x32, y31, x13, det, minD, maxD;

        public Triangle(Point pointA, Point pointB, Point pointC) : base() {
            this.Points.Add(pointA);
            this.Points.Add(pointB);
            this.Points.Add(pointC);
            this.Lines.Add(new Line(pointA, pointB));
            this.Lines.Add(new Line(pointB, pointC));
            this.Lines.Add(new Line(pointC, pointA));

            if (GeometryUtils.CrossProduct(pointB.Vector - pointA.Vector, pointC.Vector - pointB.Vector) == 0) {
                throw new ArgumentException("Triangle cannot be constructed from co-linear points.");
            }

            this.y23 = pointB.Y - pointC.Y;
            this.x32 = pointC.X - pointB.X;
            this.y31 = pointC.Y - pointA.Y;
            this.x13 = pointA.X - pointC.X;
            this.det = y23 * x13 - x32 * y31;
            this.minD = Math.Min(det, 0);
            this.maxD = Math.Max(det, 0);
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

        public override SFML.Graphics.Drawable Renderable {
            get {
                if (this.renderable == null) {
                    this.renderable = new SFML.Graphics.ConvexShape(3);
                    for (int i = 0; i < 3; i++) {
                        this.renderable.SetPoint((uint)i, new SFML.System.Vector2f(this.Points[i].X, this.Points[i].Y));
                    }
                }
                return this.renderable;
            }
        }

        public bool Collides(Point otherPoint) {
            float dx = otherPoint.X - this.Points[2].X;
            float dy = otherPoint.Y - this.Points[2].Y;

            float a = y23 * dx + x32 * dy;
            if (a < minD || a > maxD) { return false; }

            float b = y31 * dx + x13 * dy;
            if (b < minD || b > maxD) { return false; }

            float c = det - a - b;
            if (c < minD || c > maxD) { return false; }

            return true;
        }

        public bool Collides(Line otherLine) {
            return CollisionHelper.LineIntersectsTriangle(otherLine, this);
        }

        public bool Collides(Triangle otherTriangle) {
            if (!this.BoundingBox.Collides(otherTriangle.BoundingBox)) {
                return false;
            }
            foreach (Line line in this.Lines) {
                foreach (Line otherLine in otherTriangle.Lines) {
                    if (line.Collides(otherLine)) {
                        return true;
                    }
                }
            }
            return otherTriangle.Collides(this.Points[0]) || this.Collides(otherTriangle.Points[0]);
        }

        public bool Collides(Rectangle otherRectangle) {
            return CollisionHelper.TriangleIntersectsRectangle(this, otherRectangle);
        }

        public bool Collides(Circle otherCircle) {
            return CollisionHelper.TriangleIntersectsCircle(this, otherCircle);
        }

        public bool Collides(Polygon otherPolygon) {
            return CollisionHelper.TriangleIntersectsPolygon(this, otherPolygon);
        }
    }
}
