using SFML.Graphics;
using SfmlProject.Geometry;
using System.Collections.Generic;

namespace SfmlProject.Graphic {
    public class DrawablePolygon : Drawable {
        private readonly HashSet<ConvexShape> triangles = new HashSet<ConvexShape>();

        public DrawablePolygon(IEnumerable<Triangle> triangles) {
            foreach (Triangle triangle in triangles) {
                ConvexShape shape = new ConvexShape(3);
                for (int i = 0; i < 3; i++) {
                    shape.SetPoint((uint)i, new SFML.System.Vector2f(triangle.Points[i].X, triangle.Points[i].Y));
                }
                shape.OutlineColor = Color.Magenta;
                shape.OutlineThickness = 1;
                this.triangles.Add(shape);
            }
        }

        public void Draw(RenderTarget target, RenderStates states) {
            foreach (ConvexShape shape in this.triangles) {
                shape.Draw(target, states);
            }
        }
    }
}
