using System;
using System.Numerics;

namespace SfmlProject.Geometry {
    public class CollisionHelper {

        private static readonly double EPSILON = 0.00001;

        public static bool PointOnLine(Point point, Line line) {
            return point.X >= Math.Min(line.PointA.X, line.PointB.X) && point.X <= Math.Max(line.PointA.X, line.PointB.X) && Math.Abs(line.slope * point.X + line.yIntercept - point.Y) <= EPSILON;
        }

        public static bool PointInTriangle(Point point, Triangle triangle) {
            double s = triangle.PointA.Y * triangle.PointC.X - triangle.PointA.X * triangle.PointC.Y + (triangle.PointC.Y - triangle.PointA.Y) * point.X + (triangle.PointA.X - triangle.PointC.X) * point.Y;
            double t = triangle.PointA.X * triangle.PointB.Y - triangle.PointA.Y * triangle.PointB.X + (triangle.PointA.Y - triangle.PointB.Y) * point.X + (triangle.PointB.X - triangle.PointA.X) * point.Y;

            if (s < 0 != t < 0) {
                return false;
            }

            double A = -triangle.PointB.Y * triangle.PointC.X + triangle.PointA.Y * (triangle.PointC.X - triangle.PointB.X) + triangle.PointA.X * (triangle.PointB.Y - triangle.PointC.Y) + triangle.PointB.X * triangle.PointC.Y;

            return A < 0 ? s <= 0 && s + t >= A : s >= 0 && s + t <= A;
        }

        public static bool PointInRectangle(Point point, Rectangle otherRectangle) {
            // FIXME
            throw new NotImplementedException();
        }

        public static bool PointInCircle(Point point, Circle otherCircle) {
            double x = point.X - otherCircle.Center.X;
            double y = point.Y - otherCircle.Center.Y;
            return x * x + y * y <= otherCircle.Radius * otherCircle.Radius;
        }

        public static bool PointInShape(Point point, Polygon shape) {
            foreach (Triangle subTriangle in shape.Triangles) {
                if (PointInTriangle(point, subTriangle)) {
                    return true;
                }
            }
            return false;
        }

        public static bool LineIntersectsTriangle(Line line, Triangle triangle) {
            bool overlapInX = Math.Max(line.PointA.X, line.PointB.X) >= Math.Min(Math.Min(triangle.PointA.X, triangle.PointB.X), triangle.PointC.X)
                && Math.Min(line.PointA.X, line.PointB.X) <= Math.Max(Math.Max(triangle.PointA.X, triangle.PointB.X), triangle.PointC.X);
            bool overlapInY = Math.Max(line.PointA.Y, line.PointB.Y) >= Math.Min(Math.Min(triangle.PointA.Y, triangle.PointB.Y), triangle.PointC.Y)
                && Math.Min(line.PointA.Y, line.PointB.Y) <= Math.Max(Math.Max(triangle.PointA.Y, triangle.PointB.Y), triangle.PointC.Y);
            if (!overlapInX || !overlapInY) {
                return false;
            }
            foreach (Line otherLine in triangle.Lines) {
                if (line.Collides(otherLine)) {
                    return true;
                }
            }
            return PointInTriangle(line.PointA, triangle);
        }

        // FIXME: not tested/working
        public static bool LineIntersectsCircle(Line line, Circle otherCircle) {
            Vector2 lineVector = line.PointA.Vector - line.PointB.Vector;
            Vector2 perpVectorClock = new Vector2(lineVector.Y, -lineVector.X);
            Console.WriteLine(perpVectorClock);
            perpVectorClock = Vector2.Normalize(perpVectorClock) * otherCircle.Radius;
            Vector2 perpVectorCounter = new Vector2(-lineVector.Y, lineVector.X);
            Console.WriteLine(perpVectorCounter);
            perpVectorCounter = Vector2.Normalize(perpVectorCounter) * otherCircle.Radius;
            Line perpLineClock = new Line(otherCircle.Center, new Point(otherCircle.Center.X + perpVectorClock.X, otherCircle.Center.Y + perpVectorClock.Y));
            Console.WriteLine(perpLineClock);
            Line perpLineCounter = new Line(otherCircle.Center, new Point(otherCircle.Center.X + perpVectorCounter.X, otherCircle.Center.Y + perpVectorCounter.Y));
            Console.WriteLine(perpLineCounter);
            Console.WriteLine(line);
            return line.Collides(perpLineClock) || line.Collides(perpLineCounter);
        }

        // FIXME: not tested
        public static bool LineIntersectsShape(Line line, Polygon shape) {
            foreach (Line shapeLine in shape.Lines) {
                if (line.Collides(shapeLine)) {
                    return true;
                }
            }
            foreach (Triangle shapeTriangle in shape.Triangles) {
                if (line.PointA.Collides(shapeTriangle)) {
                    return true;
                }
            }
            return false;
        }
    }
}
