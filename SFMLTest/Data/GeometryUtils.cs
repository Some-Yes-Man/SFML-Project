using System;

namespace SFMLTest.Data {
    public class GeometryUtils {

        private static readonly double EPSILON = 0.00001;

        public static bool PointOnLine(Point point, Line line) {
            return ((point.X >= Math.Min(line.PointA.X, line.PointB.X)) && (point.X <= Math.Max(line.PointA.X, line.PointB.X))) && (Math.Abs((line.slope * point.X) + line.yIntercept - point.Y) <= EPSILON);
        }

        public static bool PointInTriangle(Point point, Triangle triangle) {
            return PointInTriangle(point, triangle.PointA, triangle.PointB, triangle.PointC);
        }

        public static bool PointInTriangle(Point point, Point triangle1, Point triangle2, Point triangle3) {
            double s = triangle1.Y * triangle3.X - triangle1.X * triangle3.Y + (triangle3.Y - triangle1.Y) * point.X + (triangle1.X - triangle3.X) * point.Y;
            double t = triangle1.X * triangle2.Y - triangle1.Y * triangle2.X + (triangle1.Y - triangle2.Y) * point.X + (triangle2.X - triangle1.X) * point.Y;

            if ((s < 0) != (t < 0)) {
                return false;
            }

            double A = -triangle2.Y * triangle3.X + triangle1.Y * (triangle3.X - triangle2.X) + triangle1.X * (triangle2.Y - triangle3.Y) + triangle2.X * triangle3.Y;

            return (A < 0) ? ((s <= 0) && (s + t >= A)) : ((s >= 0) && (s + t <= A));
        }

        public static bool PointInCircle(Point point, Circle otherCircle) {
            double x = point.X - otherCircle.Center.X;
            double y = point.Y - otherCircle.Center.Y;
            return x * x + y * y <= otherCircle.Radius * otherCircle.Radius;
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
                if (line.Intersects(otherLine)) {
                    return true;
                }
            }
            return PointInTriangle(line.PointA, triangle);
        }
    }
}
