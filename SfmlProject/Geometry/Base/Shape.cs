using System.Collections.Generic;

namespace SfmlProject.Geometry.Base {
    public class Shape {
        public List<Point> Points { get; private set; }
        public List<Line> Lines { get; private set; }

        protected Shape() {
            this.Points = new List<Point>();
            this.Lines = new List<Line>();
        }

        public override string ToString() {
            return string.Format("{0}[{1}]", this.GetType().Name, string.Join(',', this.Points));
        }
    }
}
