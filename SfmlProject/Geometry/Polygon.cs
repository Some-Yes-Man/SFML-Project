using System;
using System.Linq;
using System.Collections.Generic;
using SfmlProject.Geometry.Base;
using SfmlProject.Geometry.Utils;

namespace SfmlProject.Geometry {
    public class Polygon : Shape, IBoundingBox, ICollidesWith {
        private Rectangle boundingBox;
        public HashSet<Triangle> Triangles { get; private set; }

        public Polygon(params Point[] points) {
            // not enough points given
            if (points == null || points.Length < 3) {
                throw new ArgumentException("Shapes can only be constructed from 3 or more points.");
            }
            this.Points.AddRange(points);
            for (int i = 0; i < points.Length; i++) {
                this.Lines.Add(new Line(points[i], points[(i + 1) % points.Length]));
            }
            // lines of shape cross
            foreach (Line currentLine in this.Lines) {
                if (this.Lines.Any(x => !x.Equals(currentLine) && !NeighboringLines(currentLine, x) && currentLine.Collides(x))) {
                    throw new ArgumentException("Shapes are not allowed to have intersecting lines.");
                }
            }
            // check for co-linear lines
            for (int i = 0; i < this.Lines.Count; i++) {
                Line currentLine = this.Lines[i];
                Line nextLine = this.Lines[(i + 1) % this.Lines.Count];
                if (GeometryUtils.CrossProduct(currentLine.Points[0].Vector - currentLine.Points[1].Vector, nextLine.Points[0].Vector - nextLine.Points[1].Vector) == 0) {
                    throw new ArgumentException("Shapes are not allowed to have co-linear points (points in a row).");
                }
            }
            this.Triangles = this.Triangulate();
        }

        public Rectangle BoundingBox {
            get {
                if (this.boundingBox == null) {
                    float minX = float.MaxValue;
                    float minY = float.MaxValue;
                    float maxX = float.MinValue;
                    float maxY = float.MinValue;
                    foreach (Point point in this.Points) {
                        minX = Math.Min(minX, point.X);
                        minY = Math.Min(minY, point.Y);
                        maxX = Math.Max(maxX, point.X);
                        maxY = Math.Max(maxY, point.Y);
                    }
                    this.boundingBox = new Rectangle(new Point(minX, minY), new Point(maxX, maxY));
                }
                return this.boundingBox;
            }
        }

        private bool NeighboringLines(Line x, Line y) {
            int indexDifference = Math.Abs(this.Lines.IndexOf(x) - this.Lines.IndexOf(y));
            return indexDifference == 1 || indexDifference == this.Lines.Count - 1;
        }

        private HashSet<Triangle> Triangulate() {
            HashSet<Triangle> triangles = new HashSet<Triangle>();
            List<Point> pointsLeft = new List<Point>(this.Points);
            Point currentPoint = pointsLeft[0];
            while (pointsLeft.Count > 3) {
                int currentIndex = pointsLeft.IndexOf(currentPoint);
                Point previousPoint = currentIndex == 0 ? pointsLeft[^1] : pointsLeft[currentIndex - 1];
                Point nextPoint = currentIndex == pointsLeft.Count - 1 ? pointsLeft[0] : pointsLeft[currentIndex + 1];
                // angles smaller 180
                if (GeometryUtils.CrossProduct(previousPoint.Vector - currentPoint.Vector, currentPoint.Vector - nextPoint.Vector) < 0) {
                    Triangle possibleTriangle = new Triangle(previousPoint, currentPoint, nextPoint);
                    bool validTriangle = true;
                    foreach (Point point in pointsLeft.Where(x => !x.Equals(currentPoint) && !x.Equals(previousPoint) && !x.Equals(nextPoint))) {
                        if (CollisionHelper.PointInTriangle(point, possibleTriangle)) {
                            validTriangle = false;
                            break;
                        }
                    }
                    if (validTriangle) {
                        triangles.Add(possibleTriangle);
                        pointsLeft.Remove(currentPoint);
                    }
                }
                currentPoint = nextPoint;
            }
            triangles.Add(new Triangle(pointsLeft[0], pointsLeft[1], pointsLeft[2]));
            return triangles;
        }

        public bool Collides(Point otherPoint) {
            throw new NotImplementedException();
        }

        public bool Collides(Line otherLine) {
            foreach (Line line in this.Lines) {
                if (line.Collides(otherLine)) {
                    return true;
                }
            }
            return false;
        }

        public bool Collides(Triangle otherTriangle) {
            throw new NotImplementedException();
        }

        public bool Collides(Rectangle otherRectangle) {
            throw new NotImplementedException();
        }

        public bool Collides(Circle otherCircle) {
            throw new NotImplementedException();
        }

        public bool Collides(Polygon otherPolygon) {
            foreach (Line otherLine in otherPolygon.Lines) {
                if (this.Collides(otherLine)) {
                    return true;
                }
            }
            return false;
        }
    }
}
