using System;
using System.Numerics;

namespace SfmlProject.Geometry {
    public class CollisionHelper {

        public static bool PointInTriangle(Point point, Triangle triangle) {
            // magic method from 'the internet'
            double s = triangle.Points[0].Y * triangle.Points[2].X - triangle.Points[0].X * triangle.Points[2].Y + (triangle.Points[2].Y - triangle.Points[0].Y) * point.X + (triangle.Points[0].X - triangle.Points[2].X) * point.Y;
            double t = triangle.Points[0].X * triangle.Points[1].Y - triangle.Points[0].Y * triangle.Points[1].X + (triangle.Points[0].Y - triangle.Points[1].Y) * point.X + (triangle.Points[1].X - triangle.Points[0].X) * point.Y;

            if (s < 0 != t < 0) {
                return false;
            }

            double A = -triangle.Points[1].Y * triangle.Points[2].X + triangle.Points[0].Y * (triangle.Points[2].X - triangle.Points[1].X) + triangle.Points[0].X * (triangle.Points[1].Y - triangle.Points[2].Y) + triangle.Points[1].X * triangle.Points[2].Y;

            return (A < 0) ? ((s <= 0) && ((s + t) >= A)) : ((s >= 0) && ((s + t) <= A));
        }

        public static bool PointInRectangle(Point point, Rectangle rectangle) {
            return (rectangle.UpperLeft.X <= point.X) && (rectangle.LowerRight.X >= point.X) && (rectangle.UpperLeft.Y <= point.Y) && (rectangle.LowerRight.Y >= point.Y);
        }

        public static bool PointInCircle(Point point, Circle circle) {
            double x = point.X - circle.Center.X;
            double y = point.Y - circle.Center.Y;
            return x * x + y * y <= circle.Radius * circle.Radius;
        }

        public static bool PointInPolygon(Point point, Polygon shape) {
            foreach (Triangle subTriangle in shape.Triangles) {
                if (PointInTriangle(point, subTriangle)) {
                    return true;
                }
            }
            return false;
        }

        public static bool LineIntersectsTriangle(Line line, Triangle triangle) {
            Point linePoint1 = line.Points[0];
            Point linePoint2 = line.Points[1];
            Point trianglePoint1 = triangle.Points[0];
            Point trianglePoint2 = triangle.Points[1];
            Point trianglePoint3 = triangle.Points[2];
            bool overlapInX = Math.Max(linePoint1.X, linePoint2.X) >= Math.Min(Math.Min(trianglePoint1.X, trianglePoint2.X), trianglePoint3.X)
                && Math.Min(linePoint1.X, linePoint2.X) <= Math.Max(Math.Max(trianglePoint1.X, trianglePoint2.X), trianglePoint3.X);
            bool overlapInY = Math.Max(linePoint1.Y, linePoint2.Y) >= Math.Min(Math.Min(trianglePoint1.Y, trianglePoint2.Y), trianglePoint3.Y)
                && Math.Min(linePoint1.Y, linePoint2.Y) <= Math.Max(Math.Max(trianglePoint1.Y, trianglePoint2.Y), trianglePoint3.Y);
            if (!overlapInX || !overlapInY) {
                return false;
            }
            foreach (Line otherLine in triangle.Lines) {
                if (line.Collides(otherLine)) {
                    return true;
                }
            }
            return PointInTriangle(linePoint1, triangle);
        }

        public static bool RectangleIntersectsRectangle(Rectangle rectangle, Rectangle otherRectangle) {
            foreach (Point point in rectangle.Points) {
                if (PointInRectangle(point, otherRectangle)) {
                    return true;
                }
            }
            foreach (Point point in otherRectangle.Points) {
                if (PointInRectangle(point, rectangle)) {
                    return true;
                }
            }
            return false;
        }

        public static bool LineIntersectsRectangle(Line line, Rectangle rectangle) {
            foreach (Line otherLine in rectangle.Lines) {
                if (line.Collides(otherLine)) {
                    return true;
                }
            }
            return false;
        }

        public static bool LineIntersectsCircle(Line line, Circle circle) {
            Vector2 lineVector = line.Points[0].Vector - line.Points[1].Vector;
            Vector2 perpVectorClock = new Vector2(lineVector.Y, -lineVector.X);
            perpVectorClock = Vector2.Normalize(perpVectorClock) * circle.Radius;
            Vector2 perpVectorCounter = new Vector2(-lineVector.Y, lineVector.X);
            perpVectorCounter = Vector2.Normalize(perpVectorCounter) * circle.Radius;
            Line perpLineClock = new Line(circle.Center, new Point(circle.Center.X + perpVectorClock.X, circle.Center.Y + perpVectorClock.Y));
            Line perpLineCounter = new Line(circle.Center, new Point(circle.Center.X + perpVectorCounter.X, circle.Center.Y + perpVectorCounter.Y));
            return line.Collides(perpLineClock) || line.Collides(perpLineCounter);
        }

        // FIXME: not tested
        public static bool LineIntersectsPolygon(Line line, Polygon shape) {
            foreach (Line shapeLine in shape.Lines) {
                if (line.Collides(shapeLine)) {
                    return true;
                }
            }
            foreach (Triangle shapeTriangle in shape.Triangles) {
                if (line.Points[0].Collides(shapeTriangle)) {
                    return true;
                }
            }
            return false;
        }

        // TODO : optimise
        public static bool TriangleIntersectsRectangle(Triangle triangle, Rectangle rectangle) {
            foreach (Line rectangleLine in rectangle.Lines) {
                foreach (Line triangleLine in triangle.Lines) {
                    if (rectangleLine.Collides(triangleLine)) {
                        return true;
                    }
                }
            }
            return false;
        }

        // TODO : optimise
        public static bool TriangleIntersectsCircle(Triangle triangle, Circle otherCircle) {
            if (PointInTriangle(otherCircle.Center, triangle)) {
                return true;
            }
            if (PointInCircle(triangle.Points[0], otherCircle)) {
                return true;
            }
            foreach (Line line in triangle.Lines) {
                if (line.Collides(otherCircle)) {
                    return true;
                }
            }
            return false;
        }

        // TODO : optimise
        public static bool RectangleIntersectsCircle(Rectangle rectangle, Circle circle) {
            if (rectangle.Collides(circle.Center)) {
                return true;
            }
            foreach (Point point in rectangle.Points) {
                if (circle.Collides(point)) {
                    return true;
                }
            }
            foreach (Line line in rectangle.Lines) {
                if (line.Collides(circle)) {
                    return true;
                }
            }
            return false;
        }

        // TODO : optimise
        public static bool RectangleIntersectsPolygon(Rectangle rectangle, Polygon otherPolygon) {
            throw new NotImplementedException();
        }
    }
}
