using SfmlProject.Geometry.Base;
using System;

namespace SfmlProject.Geometry {
    public class Rectangle : Shape, IBoundingBox, ICollidesWith {
        public Point UpperLeft { get; set; }
        public Point LowerRight { get; set; }

        public Rectangle(Point point, float width, float height) {
            this.UpperLeft = new Point(Math.Min(point.X, point.X + width), Math.Min(point.Y, point.Y + height));
            this.LowerRight = new Point(Math.Max(point.X, point.X + width), Math.Max(point.Y, point.Y + height));
            this.Points.Add(point);
            this.Points.Add(new Point(point.X + width, point.Y));
            this.Points.Add(new Point(point.X + width, point.Y + height));
            this.Points.Add(new Point(point.X, point.Y + height));
            this.Lines.Add(new Line(this.Points[0], this.Points[1]));
            this.Lines.Add(new Line(this.Points[1], this.Points[2]));
            this.Lines.Add(new Line(this.Points[2], this.Points[3]));
            this.Lines.Add(new Line(this.Points[3], this.Points[0]));
        }

        public Rectangle(Point pointA, Point pointB) {
            this.UpperLeft = new Point(Math.Min(pointA.X, pointB.X), Math.Min(pointA.Y, pointB.Y));
            this.LowerRight = new Point(Math.Max(pointA.X, pointB.X), Math.Max(pointA.Y, pointB.Y));
            this.Points.Add(this.UpperLeft);
            this.Points.Add(new Point(this.LowerRight.X, this.UpperLeft.Y));
            this.Points.Add(this.LowerRight);
            this.Points.Add(new Point(this.UpperLeft.X, this.LowerRight.Y));
            this.Lines.Add(new Line(this.Points[0], this.Points[1]));
            this.Lines.Add(new Line(this.Points[1], this.Points[2]));
            this.Lines.Add(new Line(this.Points[2], this.Points[3]));
            this.Lines.Add(new Line(this.Points[3], this.Points[0]));
        }

        public Rectangle BoundingBox {
            get {
                return this;
            }
        }

        public bool Collides(Point otherPoint) {
            return CollisionHelper.PointInRectangle(otherPoint, this);
        }

        public bool Collides(Line otherLine) {
            return CollisionHelper.LineIntersectsRectangle(otherLine, this);
        }

        public bool Collides(Triangle otherTriangle) {
            return CollisionHelper.TriangleIntersectsRectangle(otherTriangle, this);
        }

        public bool Collides(Rectangle otherRectangle) {
            return CollisionHelper.RectangleIntersectsRectangle(this, otherRectangle);
        }

        public bool Collides(Circle otherCircle) {
            return CollisionHelper.RectangleIntersectsCircle(this, otherCircle);
        }

        public bool Collides(Polygon otherPolygon) {
            return CollisionHelper.RectangleIntersectsPolygon(this, otherPolygon);
        }
    }
}
