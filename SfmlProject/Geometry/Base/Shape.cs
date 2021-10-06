using SFML.Graphics;
using SfmlProject.Graphic;
using System.Collections.Generic;

namespace SfmlProject.Geometry.Base {
    public abstract class Shape : IRenderable {
        public List<Point> Points { get; private set; }
        public List<Line> Lines { get; private set; }
        public abstract Drawable Renderable { get; }

        protected Shape() {
            this.Points = new List<Point>();
            this.Lines = new List<Line>();
        }

        public override string ToString() {
            return string.Format("{0}[{1}]", this.GetType().Name, string.Join(',', this.Points));
        }
    }
}
