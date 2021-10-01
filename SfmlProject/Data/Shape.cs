﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace SFMLTest.Data {
    public class Shape : ICollidesWith {
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
                if (GeometryUtils.CrossProduct(currentLine.PointA.Vector - currentLine.PointB.Vector, nextLine.PointA.Vector - nextLine.PointB.Vector) == 0) {
                    throw new ArgumentException("Shapes are not allowed to have co-linear points (points in a row).");
                }
            }
            this.Triangles = this.Triangulate();
        }

        private bool NeighboringLines(Line x, Line y) {
            int indexDifference = Math.Abs(this.Lines.IndexOf(x) - this.Lines.IndexOf(y));
            return (indexDifference == 1) || (indexDifference == (this.Lines.Count - 1));
        }

        private HashSet<Triangle> Triangulate() {
            HashSet<Triangle> triangles = new HashSet<Triangle>();
            List<Point> pointsLeft = new List<Point>(this.Points);
            Point currentPoint = pointsLeft[0];
            while (pointsLeft.Count > 3) {
                int currentIndex = pointsLeft.IndexOf(currentPoint);
                Point previousPoint = (currentIndex == 0) ? pointsLeft[^1] : pointsLeft[currentIndex - 1];
                Point nextPoint = (currentIndex == pointsLeft.Count - 1) ? pointsLeft[0] : pointsLeft[currentIndex + 1];
                // angles smaller 180
                if (GeometryUtils.CrossProduct(previousPoint.Vector - currentPoint.Vector, currentPoint.Vector - nextPoint.Vector) < 0) {
                    Triangle possibleTriangle = new Triangle(previousPoint, currentPoint, nextPoint);
                    bool validTriangle = true;
                    foreach (Point point in pointsLeft.Where(x => !x.Equals(currentPoint) && !x.Equals(previousPoint) && !x.Equals(nextPoint))) {
                        if (GeometryUtils.PointInTriangle(point, possibleTriangle)) {
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

        public bool Collides(Circle otherCircle) {
            throw new NotImplementedException();
        }

        public bool Collides(Shape otherShape) {
            foreach (Line otherLine in otherShape.Lines) {
                if (this.Collides(otherLine)) {
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
