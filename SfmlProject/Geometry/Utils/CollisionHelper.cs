using System;
using System.Numerics;

namespace SfmlProject.Geometry {
    public class CollisionHelper {

        public static bool PointInRectangle(Point point, Rectangle rectangle) {
            return (rectangle.UpperLeft.X <= point.X) && (rectangle.LowerRight.X >= point.X) && (rectangle.UpperLeft.Y <= point.Y) && (rectangle.LowerRight.Y >= point.Y);
        }

        public static bool PointInCircle(Point point, Circle circle) {
            float x = point.X - circle.Center.X;
            float y = point.Y - circle.Center.Y;
            return x * x + y * y <= circle.Radius * circle.Radius;
        }

        public static bool PointInPolygon(Point point, Polygon polygon) {
            if (!PointInRectangle(point, polygon.BoundingBox)) {
                return false;
            }
            foreach (Triangle subTriangle in polygon.Triangles) {
                if (subTriangle.Collides(point)) {
                    return true;
                }
            }
            return false;
        }

        public static bool LineIntersectsTriangle(Line line, Triangle triangle) {
            if (!RectangleIntersectsRectangle(line.BoundingBox, triangle.BoundingBox)) {
                return false;
            }
            foreach (Line otherLine in triangle.Lines) {
                if (line.Collides(otherLine)) {
                    return true;
                }
            }
            return triangle.Collides(line.Points[0]);
        }

        public static bool LineIntersectsRectangle(Line line, Rectangle rectangle) {
            if (line.Points[0].Collides(rectangle) || line.Points[1].Collides(rectangle)) {
                return true;
            }
            foreach (Line otherLine in rectangle.Lines) {
                if (line.Collides(otherLine)) {
                    return true;
                }
            }
            return false;
        }

        public static bool LineIntersectsCircle(Line line, Circle circle) {
            if (PointInCircle(line.Points[0], circle) || PointInCircle(line.Points[1], circle)) {
                return true;
            }
            Vector2 lineVector = line.Points[0].Vector - line.Points[1].Vector;
            Vector2 perpVectorClock = new Vector2(lineVector.Y, -lineVector.X);
            perpVectorClock = Vector2.Normalize(perpVectorClock) * circle.Radius;
            Vector2 perpVectorCounter = new Vector2(-lineVector.Y, lineVector.X);
            perpVectorCounter = Vector2.Normalize(perpVectorCounter) * circle.Radius;
            Line perpLineClock = new Line(circle.Center, new Point(circle.Center.X + perpVectorClock.X, circle.Center.Y + perpVectorClock.Y));
            Line perpLineCounter = new Line(circle.Center, new Point(circle.Center.X + perpVectorCounter.X, circle.Center.Y + perpVectorCounter.Y));
            return line.Collides(perpLineClock) || line.Collides(perpLineCounter);
        }

        public static bool LineIntersectsPolygon(Line line, Polygon polygon) {
            if (!line.BoundingBox.Collides(polygon.BoundingBox)) {
                return false;
            }
            foreach (Line shapeLine in polygon.Lines) {
                if (line.Collides(shapeLine)) {
                    return true;
                }
            }
            foreach (Triangle shapeTriangle in polygon.Triangles) {
                if (line.Points[0].Collides(shapeTriangle)) {
                    return true;
                }
            }
            return false;
        }

        public static bool TriangleIntersectsRectangle(Triangle triangle, Rectangle rectangle) {
            if (!triangle.BoundingBox.Collides(rectangle)) {
                return false;
            }
            foreach (Point point in triangle.Points) {
                if (rectangle.Collides(point)) {
                    return true;
                }
            }
            foreach (Point point in rectangle.Points) {
                if (triangle.Collides(point)) {
                    return true;
                }
            }
            foreach (Line rectangleLine in rectangle.Lines) {
                foreach (Line triangleLine in triangle.Lines) {
                    if (rectangleLine.Collides(triangleLine)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool TriangleIntersectsCircle(Triangle triangle, Circle circle) {
            if (!triangle.BoundingBox.Collides(circle.BoundingBox)) {
                return false;
            }
            if (triangle.Collides(circle.Center)) {
                return true;
            }
            if (PointInCircle(triangle.Points[0], circle)) {
                return true;
            }
            foreach (Line line in triangle.Lines) {
                if (line.Collides(circle)) {
                    return true;
                }
            }
            return false;
        }

        public static bool RectangleIntersectsRectangle(Rectangle rectangle, Rectangle otherRectangle) {
            if ((rectangle.Points[0].X < otherRectangle.Points[0].X) && (rectangle.Points[0].X < otherRectangle.Points[2].X)
                && (rectangle.Points[2].X < otherRectangle.Points[0].X) && (rectangle.Points[2].X < otherRectangle.Points[2].X)) {
                return false;
            }
            if ((rectangle.Points[0].X > otherRectangle.Points[0].X) && (rectangle.Points[0].X > otherRectangle.Points[2].X)
                && (rectangle.Points[2].X > otherRectangle.Points[0].X) && (rectangle.Points[2].X > otherRectangle.Points[2].X)) {
                return false;
            }
            if ((rectangle.Points[0].Y > otherRectangle.Points[0].Y) && (rectangle.Points[0].Y > otherRectangle.Points[2].Y)
                && (rectangle.Points[2].Y > otherRectangle.Points[0].Y) && (rectangle.Points[2].Y > otherRectangle.Points[2].Y)) {
                return false;
            }
            if ((rectangle.Points[0].Y < otherRectangle.Points[0].Y) && (rectangle.Points[0].Y < otherRectangle.Points[2].Y)
                && (rectangle.Points[2].Y < otherRectangle.Points[0].Y) && (rectangle.Points[2].Y < otherRectangle.Points[2].Y)) {
                return false;
            }
            return true;
        }

        public static bool TriangleIntersectsPolygon(Triangle triangle, Polygon otherPolygon) {
            if (!triangle.BoundingBox.Collides(otherPolygon.BoundingBox)) {
                return false;
            }
            foreach (Point point in triangle.Points) {
                if (point.Collides(otherPolygon)) {
                    return true;
                }
            }
            foreach (Point point in otherPolygon.Points) {
                if (point.Collides(triangle)) {
                    return true;
                }
            }
            foreach (Line line in triangle.Lines) {
                foreach (Line otherLine in otherPolygon.Lines) {
                    if (line.Collides(otherLine)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool RectangleIntersectsCircle(Rectangle rectangle, Circle circle) {
            if (!rectangle.Collides(circle.BoundingBox)) {
                return false;
            }
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

        public static bool RectangleIntersectsPolygon(Rectangle rectangle, Polygon polygon) {
            if (!polygon.BoundingBox.Collides(rectangle)) {
                return false;
            }
            foreach (Point point in polygon.Points) {
                if (rectangle.Collides(point)) {
                    return true;
                }
            }
            foreach (Point point in rectangle.Points) {
                if (polygon.Collides(point)) {
                    return true;
                }
            }
            foreach (Line rectangleLine in rectangle.Lines) {
                foreach (Line polygonLine in polygon.Lines) {
                    if (rectangleLine.Collides(polygonLine)) {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool CircleIntersectsPolygon(Circle circle, Polygon otherPolygon) {
            if (!otherPolygon.BoundingBox.Collides(circle.BoundingBox)) {
                return false;
            }
            if (otherPolygon.Collides(circle.Center)) {
                return true;
            }
            if (PointInCircle(otherPolygon.Points[0], circle)) {
                return true;
            }
            foreach (Line line in otherPolygon.Lines) {
                if (line.Collides(circle)) {
                    return true;
                }
            }
            return false;
        }
    }
}
