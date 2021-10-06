using SFML.Graphics;
using SFML.System;

namespace SfmlProject.Graphic {
    public class DrawableLine : Drawable {
        private readonly Vertex[] vertices;

        public DrawableLine(Vector2f pointA, Vector2f pointB) {
            this.vertices = new Vertex[2] { new Vertex(pointA), new Vertex(pointB) };
        }

        public void Draw(RenderTarget target, RenderStates states) {
            target.Draw(this.vertices, 0, 2, PrimitiveType.LineStrip, states);
        }
    }
}
