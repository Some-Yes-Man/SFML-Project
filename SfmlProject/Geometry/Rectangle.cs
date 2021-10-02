using SfmlProject.Geometry.Base;
using System;
using System.Collections.Generic;

namespace SfmlProject.Geometry {
    public class Rectangle : ICollidesWith {
        private List<Point> points = new List<Point>();
        public Point UpperLeft { get; set; }
        public Point LowerRight { get; set; }

        public Rectangle(Point point, float width, float height) {
            float xMin = Math.Min(point.X, point.X + width);
            float yMin = Math.Min(point.Y, point.Y + height);
            this.points.Add(point);
            this.points.Add(new Point(point.X + width, point.Y));
            this.points.Add(new Point(point.X + width, point.Y + height));
            this.points.Add(new Point(point.X, point.Y + height));
            // FIXME
        }

        public Rectangle(Point pointA, Point pointB) {

        }

        public bool Collides(Point otherPoint) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Line otherLine) {
            throw new System.NotImplementedException();
        }

        public bool Collides(Triangle otherTriangle) {
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
    }
}
