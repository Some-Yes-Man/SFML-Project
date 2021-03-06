using SfmlProject.Geometry.Base;
using System;
using System.Numerics;

namespace SfmlProject.Geometry {
    public class Circle : Shape, IBoundingBox, ICollidesWith {
        private Rectangle boundingBox;
        private SFML.Graphics.CircleShape renderable;

        public Point Center { get; set; }
        public float Radius { get; set; }

        public Circle(Point point, float radius) {
            this.Center = point;
            this.Points.Add(point);
            this.Radius = radius;
            if (radius == 0) {
                throw new ArgumentException("Circles need a non-zero radius.");
            }
        }

        public Circle(float x, float y, float radius) : this(new Point(x, y), radius) {
        }

        public Rectangle BoundingBox {
            get {
                if (this.boundingBox == null) {
                    this.boundingBox = new Rectangle(new Point(this.Center.X - this.Radius, this.Center.Y - this.Radius),
                        new Point(this.Center.X + this.Radius, this.Center.Y + this.Radius));
                }
                return this.boundingBox;
            }
        }

        public override SFML.Graphics.Drawable Renderable {
            get {
                if (this.renderable == null) {
                    this.renderable = new SFML.Graphics.CircleShape(this.Radius);
                    this.renderable.Position = new SFML.System.Vector2f(this.Center.X, this.Center.Y);
                }
                return this.renderable;
            }
        }

        public bool Collides(Point otherPoint) {
            return CollisionHelper.PointInCircle(otherPoint, this);
        }

        public bool Collides(Line otherLine) {
            return CollisionHelper.LineIntersectsCircle(otherLine, this);
        }

        public bool Collides(Triangle otherTriangle) {
            return CollisionHelper.TriangleIntersectsCircle(otherTriangle, this);
        }

        public bool Collides(Rectangle otherRectangle) {
            return CollisionHelper.RectangleIntersectsCircle(otherRectangle, this);
        }

        public bool Collides(Circle otherCircle) {
            Vector2 distance = this.Center.Vector - otherCircle.Center.Vector;
            float maxDistance = this.Radius + otherCircle.Radius;
            return distance.X * distance.X + distance.Y * distance.Y <= maxDistance * maxDistance;
        }

        public bool Collides(Polygon otherPolygon) {
            return CollisionHelper.CircleIntersectsPolygon(this, otherPolygon);
        }
    }
}
